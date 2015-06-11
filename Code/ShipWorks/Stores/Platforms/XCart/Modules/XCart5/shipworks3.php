<?php 

    /*
    |
    | This file and the source codes contained herein are the property 
    | of Interapptive, Inc.  Use of this file is restricted to the specific 
    | terms and conditions in the License Agreement associated with this 
    | file.     Distribution of this file or portions of this file for uses
    | not covered by the License Agreement is not allowed without a written 
    | agreement signed by an officer of Interapptive, Inc.
    | 
    | The code contained herein may not be reproduced, copied or
    | redistributed in any form, as part of another product or otherwise.
    | Modified versions of this code may not be sold or redistributed.
    |
    | Copyright 2009-2014 Interapptive, Inc.  All rights reserved.
    | http://www.interapptive.com/
    |
    |
     */
    define('REQUIRE_SECURE', TRUE);
    $moduleVersion = "3.9.3.0";
    $schemaVersion = "1.0.0";
    
    require_once 'top.inc.php';
    
    header("Content-Type: text/xml;charset=utf-8");
    header("Last-Modified: " . gmdate("D, d M Y H:i:s") . " GMT");
    
    // HTTP/1.1
    header("Cache-Control: no-store, no-cache, must-revalidate");
    header("Cache-Control: post-check=0, pre-check=0", false);

    // HTTP/1.0
    header("Pragma: no-cache");     

    // write xml documenta declaration
    function writeXmlDeclaration()
    {
        echo "<?xml version=\"1.0\" standalone=\"yes\" ?>";
    }
    
	function writeStartTag($tag, $attributes = null)
	{
            echo '<' . $tag;

            if ($attributes != null)
            {
                echo ' ';

                foreach ($attributes as $name => $attribValue)
                {
                    echo $name. '="'. htmlspecialchars($attribValue). '" ';	
                }
            }

            echo '>';
	}
    
    // write closing xml tag
    function writeCloseTag($tag)
    {
        echo '</' . $tag . '>';
    }

    // Output the given tag\value pair
    function writeElement($tag, $value)
    {
        writeStartTag($tag);
        echo htmlspecialchars($value);
        writeCloseTag($tag);
    }

    // Outputs the given name/value pair as an xml tag with attributes
    function writeFullElement($tag, $value, $attributes)
    {
        echo '<'. $tag. ' ';

        foreach ($attributes as $name => $attribValue)
        {
            echo $name. '="'. htmlspecialchars($attribValue). '" ';	
        }
        echo '>';
        echo htmlspecialchars($value);
        writeCloseTag($tag);
    }

    
    // Function used to output an error and quit.
    function outputError($code, $error)
    {       
        writeStartTag("Error");
        writeElement("Code", $code);
        writeElement("Description", $error);
        writeCloseTag("Error");
    }       

    $secure = false;
    try
    {
        if (isset($_SERVER['HTTPS']))
        {
            $secure = ($_SERVER['HTTPS'] == 'on' || $_SERVER['HTTPS'] == '1');
        }
    }
    catch(Exception $e)
    {
    }

    // Open the XML output and root
    writeXmlDeclaration();
    writeStartTag("ShipWorks", array("moduleVersion" => $moduleVersion, "schemaVersion" => $schemaVersion));
   
    // Enforse SSL
    if (!$secure && REQUIRE_SECURE)
    {
        outputError(10, 'A secure (https://) connection is required.');
    }
    else
    {
        if (checkAdminLogin())
        {
            $action = (isset($_REQUEST['action']) ? $_REQUEST['action'] : '');
            switch (strtolower($action)) 
            {
				case 'getmodule': Action_GetModule(); break;
				case 'getstore': Action_GetStore(); break;
				case 'getcount': Action_GetCount(); break;
				case 'getorders': Action_GetOrders(); break;
				case 'getstatuscodes': Action_GetStatusCodes(); break;
                                case 'updatestatus': action_UpdateStatus();break;
                                case 'updateshipment': action_UpdateShipment(); break;
				default:
					outputError(20, "Invalid action '$action'");
				}
        }
    }
    
    // Close the output
    writeCloseTag("ShipWorks");
    
    //Check username, password and admin access    
    function checkAdminLogin()
    {
        $loginOK = false;

        if (isset($_REQUEST['username']) && isset($_REQUEST['password']))
        {

            //Get the table prefix
            $tablePrefix = Includes\Utils\Database::getTablesPrefix();
            
            $username = $_REQUEST['username'];
            $password = $_REQUEST['password'];

            
            //Grab hash for provided user only if admin (access_level = 100) 
            $hash = Includes\Utils\Database::fetchColumn("SELECT password FROM $tablePrefix"."profiles WHERE login = '$username' AND access_level = 100");
            if (\XLite\Core\Auth::getInstance()->comparePassword($hash,$password))
            {
                $loginOK = true;        
            }
        }

        if (!$loginOK)
        {
            outputError(50, "Username or password is incorrect");
        }

        return $loginOK;
    }
	
    // Get module data
    function action_GetModule()
    {
        writeStartTag("Module");

            writeElement("Platform", "X-Cart");
            writeElement("Developer", "Interapptive, Inc. (support@interapptive.com)");

            writeStartTag("Capabilities");
                writeElement("DownloadStrategy", "ByModifiedTime");
                writeFullElement("OnlineCustomerID", "", array("supported" => "true", "dataType" => "text"));
                writeFullElement("OnlineStatus", "", array("supported" => "true", "dataType" => "text" ));
                writeFullElement("OnlineShipmentUpdate", "", array("supported" => "true"));
            writeCloseTag("Capabilities");

        writeCloseTag("Module");			
    }
    
    // Write store data
    function Action_GetStore()
    {
        //Get the table prefix
        $tablePrefix = Includes\Utils\Database::getTablesPrefix();
        
        $config = Includes\Utils\Database::fetchAll("SELECT name, value FROM $tablePrefix"."config");
        
        $storeinfo = array();
        
        foreach($config as $info){
            $storeinfo[$info['name']] = $info['value'];
        }
        
        $name = $storeinfo['company_name'];;
        $owner = $storeinfo['company_name'];;
        $email = $storeinfo['orders_department'];
        $address = $storeinfo['company_address'];
        $city = $storeinfo['location_city'];
        $country = $storeinfo['location_country'];
        $website = $storeinfo['company_website'];
        //$phone = $storeinfo['company_phone'];
        
        writeStartTag("Store");
            writeElement("Name", $name);
            writeElement("CompanyOrOwner", $owner);
            writeElement("Email", $email);
            writeElement("Street1", $address);
            writeElement("City", $city);
            writeElement("Country", $country);
            writeElement("Website", $website);
        writeCloseTag("Store");
    }

    // Get the count of orders greater than the start ID
    function Action_GetCount()
    {         
        $start = 0;
        
        if (isset($_REQUEST['start']))
        {
            $start = $_REQUEST['start'];
        }

        // only get orders through 2 seconds ago
        $end = time() - 2;

        // Convert start to timestamp
        $start = strtotime($start);

        // Write the params for easier diagnostics
        writeStartTag("Parameters");
        writeElement("Start", $start);
        writeElement("End", $end);
        writeCloseTag("Parameters");

        //Only count paid orders
        $qb = \XLite\Core\Database::getEM()->createQueryBuilder();
        $qb->select('count(o)')
           ->from('XLite\Model\Order', 'o')
           ->where("o.lastRenewDate > $start")
           ->andWhere("o.paymentStatus = 4");

        $query = $qb->getQuery();
        $result = $query->getResult();
        $count =  $result[0][1];

        writeElement("OrderCount", $count);
    }

    // Get all orders greater than the given start id, limited by max count
    function Action_GetOrders()
    {
        $start = 0;
        $maxcount = 50;

        if (isset($_REQUEST['start']))
        {
            $start = $_REQUEST['start'];
        }

        if (isset($_REQUEST['maxcount']))
        {
            $maxcount = $_REQUEST['maxcount'];
        }

        // only get orders through 2 seconds ago
        $end = time() - 2;

        // Convert start to timestamp
        $start = strtotime($start);

        // Write the params for easier diagnostics
        writeStartTag("Parameters");
        writeElement("Start", $start);
        writeElement("End", $end);
        writeElement("MaxCount", $maxcount);
        writeCloseTag("Parameters");                                    

        //Only get paid orders
        $qb = \XLite\Core\Database::getEM()->createQueryBuilder();
        $qb->select('o')
           ->from('XLite\Model\Order', 'o')
           ->where("o.lastRenewDate > $start")
           ->andWhere("o.paymentStatus = 4")
           ->orderBy("o.lastRenewDate", "ASC");

        $query = $qb->getQuery();
        $orders = $query->getResult();

        writeStartTag("Orders");

        $lastModified = null;
        $processedIds = "";

        foreach ($orders as $order)
        {
            // keep track of the ids we've downloaded
            $lastModified = $order->getLastRenewDate();

            if ($processedIds != "")
            {
                $processedIds .= ", ";
            }
            $processedIds .= $order->getOrderid();

            WriteOrder($order);
        }

        // if we processed some orders we may have to get some more
        if ($processedIds != "")
        {
            $qb = \XLite\Core\Database::getEM()->createQueryBuilder();
            $qb->select('o')
               ->from('XLite\Model\Order', 'o')
               ->where("o.lastRenewDate > $lastModified")
               ->andWhere("o.paymentStatus = 4")
               ->andWhere("o.order_id NOT IN ($processedIds)");
            
            $query = $qb->getQuery();
            
            $orders = $query->getResult();
                
            foreach ($orders as $order)
            {
                WriteOrder($order);
            }
        }

        writeCloseTag("Orders");
    }
    
    // Outputs notes elements
    function WriteNote($noteText, $public)
    {
        $attributes = array("public" => $public ? "true" : "false");

        writeFullElement("Note", $noteText, $attributes);
    }
    
    // Output the order as xml
    function WriteOrder($order)
    {                 
    global $secure;
        writeStartTag("Order");

        writeElement("OrderNumber", $order->getOrderNumber());
        writeElement("OrderDate", date("Y-m-d\TH:i:s", $order->getDate()));
        writeElement("LastModified", date("Y-m-d\TH:i:s", $order->getLastRenewDate()));
        writeElement("ShippingMethod", $order->getShippingMethodName());
        writeElement("StatusCode", $order->getShippingStatusCode());

        writeStartTag("Notes");
            //Admin Notes
            WriteNote($order->getAdminNotes(), false);
            //Customer Checkout Notes
            WriteNote($order->getNotes(), false);
        writeCloseTag("Notes");
        
        
        $profile = $order->getProfile();
        $addresses = $profile->getAddresses();
        
        foreach ($addresses as $address){
            if ($address->is_billing){
                $billingAddress = $address;
            }
            if ($address->is_shipping){
                $shippingAddress = $address;
            }
        }
        
        writeStartTag("BillingAddress");
            writeElement("FullName", $billingAddress->getfirstname().' '.$billingAddress->getlastname() );
            //Currently there is no company field in XCart
            //writeElement("Company","");
            writeElement("Street1", $billingAddress->getStreet());
            //Currently there is no Street2 field in XCart
            //writeElement("Street2","");
            //Currently there is no Street3 field in XCart
            //writeElement("Street3","");
            writeElement("City", $billingAddress->getCity());
            writeElement("State", $billingAddress->getState()->getCode());
            writeElement("PostalCode", $billingAddress->getZipcode());
            writeElement("Country", $billingAddress->getCountry()->getCode());
            writeElement("Phone", $billingAddress->getphone());
            writeElement("Email",$profile->getLogin());
        writeCloseTag("BillingAddress");

        writeStartTag("ShippingAddress");
            writeElement("FullName", $shippingAddress->getfirstname().' '.$shippingAddress->getlastname() );
            //Currently there is no company field in XCart
            //writeElement("Company","");
            writeElement("Street1", $shippingAddress->getStreet(1));
            //Currently there is no Street2 field in XCart
            //writeElement("Street2","");
            //Currently there is no Street3 field in XCart
            //writeElement("Street3","");
            writeElement("City", $shippingAddress->getCity());
            writeElement("State", $shippingAddress->getState()->getCode());
            writeElement("PostalCode", $shippingAddress->getZipcode());
            writeElement("Country", $shippingAddress->getCountry()->getCode());
            writeElement("Phone", $shippingAddress->getphone());
            writeElement("Email",$profile->getLogin());
        writeCloseTag("ShippingAddress");

        writeStartTag("Payment");
            writeElement("Method", $order->getPaymentMethodName());
        writeCloseTag("Payment");

        WriteOrderItems($order->getItems());

        WriteOrderTotals($order);

        writeCloseTag("Order");
    }

    // writes a single order total
    function WriteOrderTotal($name, $value, $class, $impact = "add")
    {
        if ($value > 0)
        {
            writeFullElement("Total", $value, array("name" => $name, "class" => $class, "impact" => $impact));
        }
    }
    
    // Write all totals lines for the order
    function WriteOrderTotals($order)
    {
        writeStartTag("Totals");

        WriteOrderTotal("Order Subtotal", $order->getSubtotal(), "ot_subtotal", "none");
        WriteOrderTotal("Shipping and Handling", $order->getShippingAmount(), "shipping", "add");

        $orderCharges = $order->getSurcharges();
        
        foreach ($orderCharges as $charge){
            
            $value = $charge->getValue();
            
            if ($value < 0) {
                $impact = "subtract";
            }  else {
                $impact = "add";
            }
            
            $name = $charge ->getName();
            $class = $charge ->getType();
            
            WriteOrderTotal($name, abs($value), $class, $impact); 
        }
        
        WriteOrderTotal("Grand Total", $order->getTotal(), "total", "none");

        writeCloseTag("Totals");
    }

    // Write XML for all products for the given order
    function WriteOrderItems($orderItems)
    {
        writeStartTag("Items");

        foreach ($orderItems as $item){

	writeStartTag("Item");
        
            writeElement("Code", $item->getsku());
            writeElement("SKU", $item->getsku());
            writeElement("Name", $item->getname());
            writeElement("Quantity", (int)$item->getamount());
            writeElement("UnitPrice", $item->getitemnetprice());
            writeElement("Weight", $item->getWeight());

            writeStartTag("Attributes");
                $attributes = $item->getAttributeValues();
                foreach($attributes as $attribute){

                writeStartTag("Attribute");
                        writeElement("Name", $attribute->getName());
                        writeElement("Value", $attribute->getValue());
                writeCloseTag("Attribute");

                }
            writeCloseTag("Attributes");
            
            
            writeCloseTag("Item");
        }

        writeCloseTag("Items");
    }

    // Returns the shipping status codes for the store
    function Action_GetStatusCodes()
    {
        writeStartTag("StatusCodes");

        $ShippingStatus = \XLite\Core\Database::getEM()->createQuery("select s from XLite\Model\Order\Status\Shipping s")->getResult();

        foreach ($ShippingStatus as $status){

            writeStartTag("StatusCode");
            writeElement("Code", $status->getCode());
            writeElement("Name", $status->getCustomerName());
            writeCloseTag("StatusCode");
        }
        
        writeCloseTag("StatusCodes");
    }

    // Updates the status of an order in XCart
    function action_UpdateStatus()
    {
        $ordernumber = 0;
        $newstatus = '';

        if (!isset($_POST['order']) || !isset($_POST['status']))
        {
            outputError(40, "Insufficient parameters");
            return;
        }

        if (isset($_POST['order']))
        {
            $ordernumber = $_POST['order'];
        }

        if (isset($_POST['status']))
        {
            $newstatus = $_POST['status'];
        }

        // write the params for easier diagnostics
        writeStartTag("Parameters");
            writeElement("Order", $ordernumber);	
            writeElement("Status", $newstatus);
        writeCloseTag("Parameters");

        
        $order = \XLite\Core\Database::getRepo('XLite\Model\Order')->findOneBy(array('orderNumber'=>"$ordernumber"));
        
        $order->setShippingStatus($newstatus);
        
        \XLite\Core\Database::getRepo('XLite\Model\Order')->flushChanges();
            
        echo "<UpdateSuccess/>";	
    }
    
    // Updates the shipment information of an order in XCart.  Currently it's just
    // the tracking number from ShipWorks.
    function action_UpdateShipment()
    {
        $ordernumber = 0;
        $trackingNumber = '';

        if (!isset($_POST['order']) || !isset($_POST['tracking']))
        {
            outputError(40, "Insufficient parameters");
            return;
        }

        if (isset($_POST['order']))
        {
            $ordernumber = $_POST['order'];
        }
                                
        if (isset($_POST['tracking']))
        {
            $trackingNumber = $_POST['tracking'];
        }

        // write the params for easier diagnostics
        writeStartTag("Parameters");
            writeElement("OrderID", $ordernumber);	
            writeElement("Tracking", $trackingNumber);
        writeCloseTag("Parameters");

        $order = \XLite\Core\Database::getRepo('XLite\Model\Order')->findOneBy(array('orderNumber'=>"$ordernumber"));
        
        $trackingInfo = array('order'=>$order,'value'=>"$trackingNumber");
        
        $trackingTest = \XLite\Core\Database::getRepo('XLite\Model\OrderTrackingNumber')->insert($trackingInfo);
        
        \XLite\Core\Database::getRepo('XLite\Model\Order')->flushChanges();
               
        echo "<UpdateSuccess/>";	
    }


?>
