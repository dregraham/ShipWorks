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
    | http://www.interapptive.com/
    |
    |
     */

    define('REQUIRE_SECURE', TRUE);
    $moduleVersion = "3.10.0";
    $schemaVersion = "1.1.0";
    
    require_once 'includes/application_top.php';
    require_once 'includes/classes/order.php';
    require_once 'includes/classes/products.php';
    require_once 'admin/includes/functions/general.php';
        
    $offsetQuery = $lC_Database->simpleQuery("SELECT UNIX_TIMESTAMP() - UNIX_TIMESTAMP(UTC_TIMESTAMP()) AS offset")->fetch_assoc();
    $offsetinSeconds = (int)($offsetQuery['offset'] ? $offsetQuery['offset'] : 0);
    $mySqlTimeZoneName = TimeZoneName($offsetinSeconds);
    
    $mySqlTimeZone = new DateTimeZone($mySqlTimeZoneName);

    
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
   
    if (isset($_REQUEST['action']) || isset($_REQUEST['action']) || isset($_REQUEST['action']))
    {
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
                                    default:
                                            outputError(20, "'$action' is not supported.");
                                    }
            }
        }
    }
    else
    {
        $error = "This is the Module URL you need to enter into the ShipWorks application, for more help please go to http://support.shipworks.com";
        outputError(1, $error);
    }
    
    // Close the output
    writeCloseTag("ShipWorks");
    
    //Check username, password and admin access    
    function checkAdminLogin()
    {
        global $lC_Database;
        $loginOK = false;

        if (isset($_REQUEST['username']) && isset($_REQUEST['password']))
        {
            $username = $lC_Database->parseString($_REQUEST['username']);
            $password = $lC_Database->parseString($_REQUEST['password']);

            $userInfoQuery = "SELECT id, user_password FROM ". TABLE_ADMINISTRATORS." WHERE user_name = '$username'";
            
            $userInfo = $lC_Database->simpleQuery($userInfoQuery)->fetch_assoc();
            
            if (lc_validate_password($password, $userInfo['user_password']))
            {
                $loginOK = true;        
            }
        }

        if (!$loginOK)
        {
            outputError(50, "The username or password is incorrect or user provided does not have administrator access.");
        }

        return $loginOK;
    }
	
    // Get module data
    function action_GetModule()
    {
        writeStartTag("Module");

            writeElement("Platform", "LoadedCommerce");
            writeElement("Developer", "Interapptive, Inc. (support@interapptive.com)");

            writeStartTag("Capabilities");
                writeElement("DownloadStrategy", "ByModifiedTime");
                writeFullElement("OnlineCustomerID", "", array("supported" => "true", "dataType" => "numeric"));
                writeFullElement("OnlineStatus", "", array("supported" => "true", "supportsComments" => "true", "downloadOnly"=>"false", "dataType" => "numeric"));
                writeFullElement("OnlineShipmentUpdate", "", array("supported" => "false"));
            writeCloseTag("Capabilities");
            writeStartTag("Communications");
            writeFullElement("Http", "", array("expect100Continue" => "true"));
            writeElement("ResponseEncoding", "UTF-8");
            writeCloseTag("Communications");

        writeCloseTag("Module");			
    }
    
    // Write store data
    function Action_GetStore()
    {
        global $lC_Database;
        
        $configQuery = "SELECT `configuration_key` as `key`, `configuration_value` as `value` FROM ".TABLE_CONFIGURATION;
        $result = $lC_Database->simpleQuery($configQuery);

        while($row = $result->fetch_row()) 
        {
            $config[$row[0]]=$row[1];
        }
        
        $name = $config['STORE_NAME'];
        $companyOrOwner = $config['STORE_OWNER'] ;
        $email = $config['STORE_OWNER_EMAIL_ADDRESS'];
        $street1 = '';
        $street2 = '';
        $street3 = '';
        $city = '';
        $state = '';
        $postalCode = '';
        $country = '';
        $phone = '';
        $website = '';
        
        writeStartTag("Store");
            writeElement("Name", $name);
            writeElement("CompanyOrOwner", $companyOrOwner);
            writeElement("Email", $email);
            writeElement("Street1", $street1);
            writeElement("Street2", $street2);
            writeElement("Street3", $street3);
            writeElement("City", $city);
            writeElement("State", $state);
            writeElement("PostalCode", $postalCode);
            writeElement("Country", $country);
            writeElement("Phone", $phone);
            writeElement("Website", $website);
        writeCloseTag("Store");
    }

    // Get the count of orders greater than the start ID
    function Action_GetCount()
    {         
        global $lC_Database;
        global $mySqlTimeZone;

        $start = "1970-01-01 00:00:00";
        
        if(isset($_REQUEST['start']))
        {
            $start = $_REQUEST['start'];
        }
        
        //Date/Time from ShipWorks in UTC
        $start = new DateTime($start, new DateTimeZone('UTC'));
        $start = $start->setTimezone($mySqlTimeZone);
        
        // only get orders through 2 seconds ago
        $end = new DateTime(date("Y-m-d\TH:i:s", time()-2), $mySqlTimeZone);

        // Write the params for easier diagnostics
        writeStartTag("Parameters");
        writeElement("Start", $start->format("Y-m-d\TH:i:s"));
        writeElement("End", $end->format("Y-m-d\TH:i:s"));
        writeCloseTag("Parameters");

        $countQuery = "select count(*) as count from ".TABLE_ORDERS;
        //
        $result = $lC_Database->simpleQuery($countQuery)->fetch_assoc();

        writeElement("OrderCount", (int)$result['count']);
    }

    // Get all orders greater than the given start id, limited by max count
    function Action_GetOrders()
    {
        global $lC_Database;
        global $mySqlTimeZone;

        $start = "1970-01-01 00:00:00";
        $maxcount = 50;
        
        if(isset($_REQUEST['start']))
        {
            $start = $_REQUEST['start'];
        }

        if (isset($_REQUEST['maxcount']))
        {
            $maxcount = $_REQUEST['maxcount'];
        }
        
        //Date/Time from ShipWorks in UTC
        $start = new DateTime($start, new DateTimeZone('UTC'));
        $start = $start->setTimezone($mySqlTimeZone);
        
        // only get orders through 2 seconds ago
        $end = new DateTime(date("Y-m-d\TH:i:s", time()-2), $mySqlTimeZone);

        $startDate = $start->format("Y-m-d\TH:i:s");
        $endDate = $end->format("Y-m-d\TH:i:s");
        
        $ordersQuery = "SELECT orders_id, orders_status, IFNULL(last_modified,date_purchased) as modified "
                     . "FROM ".TABLE_ORDERS." "
                     . "WHERE IFNULL(last_modified,date_purchased) > '$startDate' AND IFNULL(last_modified,date_purchased) < '$endDate' "
                     . "ORDER BY modified ASC LIMIT 0,".$maxcount;
        
        // Write the params for easier diagnostics
        writeStartTag("Parameters");
        writeElement("Start", $startDate);
        writeElement("End", $endDate);
        writeCloseTag("Parameters");
        
        $ordersResult = $lC_Database->simpleQuery($ordersQuery);
        
        writeStartTag("Orders");

        $start = null;
        $processedIds = null;
        
        foreach ($ordersResult as $orderInfo)
        {
            $start = new DateTime($orderInfo['modified'], $mySqlTimeZone);
            
             if ($processedIds != "")
            {
                $processedIds .= ", ";
            }
                
            $processedIds .= $orderInfo['orders_id'];
            
            WriteOrder($orderInfo);
        }

        //make sure that we dont skip an order if it has the same lastmodified as order #50 from above
        if ($processedIds)
        {
            $startDate = $start->format("Y-m-d\TH:i:s");
            
            $skippedOrdersQuery = "SELECT orders_id, orders_status, IFNULL(last_modified,date_purchased) as modified "
                                . "FROM ".TABLE_ORDERS." "
                                . "WHERE IFNULL(last_modified,date_purchased) = '$startDate' AND orders_id not in ($processedIds) "
                                . "ORDER BY modified ASC LIMIT 0,".$maxcount;
            
            $skippedOrders = $lC_Database->simpleQuery($skippedOrdersQuery);

            if($skippedOrders)
            {
                foreach($skippedOrders as $orderInfo)
                {
                    WriteOrder($orderInfo);
                }
            }
        }

        writeCloseTag("Orders");
    }
    
    // Output the order as xml
    function WriteOrder($orderInfo)
    {                 
        global $secure;
        global $mySqlTimeZone;
        
        $order = new lC_Order($orderInfo['orders_id']);
        
        $orderDate = new DateTime($order->info['date_purchased'], $mySqlTimeZone);
        $orderDate = $orderDate->setTimezone(new DateTimeZone('UTC'));

        $lastModified = new DateTime($orderInfo['modified'], $mySqlTimeZone);
        $lastModified = $lastModified->setTimezone(new DateTimeZone('UTC'));
        
        writeStartTag("Order");

        writeElement("OrderNumber", $orderInfo['orders_id']);
        writeElement("OrderDate", $orderDate->format("Y-m-d\TH:i:s"));
        writeElement("LastModified", $lastModified->format("Y-m-d\TH:i:s"));
        writeElement("ShippingMethod", $order->info['shipping_method']);
        writeElement("StatusCode", $orderInfo['orders_status']);
        writeElement("CustomerID", $order->customer['id']);
//        writeStartTag("Notes");
//            $messagesCollection = new PrestaShopCollectionCore('Message');
//            $messages = $messagesCollection->where("id_order", '=', $order->id)->getAll();
//            
//            foreach($messages as $message)
//            {
//                writeFullElement("Note", $message->message, array("public" => "true"));
//            }
//        writeCloseTag("Notes");



        writeStartTag("BillingAddress");
            writeElement("FullName", $order->customer['name']);
            writeElement("Company",$order->customer['company']);
            writeElement("Street1", $order->customer['street_address']);
            writeElement("Street2",$order->customer['suburb']);
            writeElement("Street3","");
            writeElement("City", $order->customer['city']);
            writeElement("State", $order->customer['state']);
            writeElement("PostalCode", $order->customer['postcode']);
            writeElement("Country", $order->customer['country_iso2']);
            writeElement("Phone", $order->customer['telephone']);
            writeElement("Email",$order->customer['email_address']);
        writeCloseTag("BillingAddress");

        writeStartTag("ShippingAddress");
            writeElement("FullName", $order->billing['name']);
            writeElement("Company",$order->billing['company']);
            writeElement("Street1", $order->billing['street_address']);
            writeElement("Street2",$order->billing['suburb']);
            writeElement("Street3","");
            writeElement("City", $order->billing['city']);
            writeElement("State", $order->billing['state']);
            writeElement("PostalCode", $order->billing['postcode']);
            writeElement("Country", $order->billing['country_iso2']);
            writeElement("Phone", $order->customer['telephone']);
            writeElement("Email",$order->customer['email_address']);
        writeCloseTag("ShippingAddress");

        writeStartTag("Payment");
            writeElement("Method", $order->info['payment_method']);
        writeCloseTag("Payment");

        WriteOrderItems($order->products);

        WriteOrderTotals($orderInfo['orders_id']);

        writeCloseTag("Order");
    }

    // Outputs notes elements
    function WriteNote($noteText, $public)
    {
        $attributes = array("public" => $public ? "true" : "false");

        writeFullElement("Note", $noteText, $attributes);
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
    function WriteOrderTotals($ordersId)
    {
        global $lC_Database;
        $totalQuery = "SELECT * FROM ".TABLE_ORDERS_TOTAL." WHERE orders_id = $ordersId";
        
        $totals = $lC_Database->simpleQuery($totalQuery);
        
        writeStartTag("Totals");
        
        foreach ($totals as $total)
        {
            $name = $total['title'];
            $value = $total['value'];
            $class = '';
            $impact = '';
            
            switch ($total["class"]){
                case "sub_total":
                    $class = "SUBTOTAL";
                    $impact = "none";
                    break;
                case "shipping":
                    $class = "SHIPPING";
                    $impact = "add";
                    break;
                case "total":
                    $class = "TOTAL";
                    $impact = "none";
                    break;
                case "coupon":
                    $class = "DISCOUNT";
                    $impact = "subtract";
                    break;
                default:
                    $class = $total['class'];
                    $impact = "add";
            }
            
            WriteOrderTotal($total['title'], $value, $class, $impact);
        }
        
        writeCloseTag("Totals");
    }

    // Write XML for all products for the given order
    function WriteOrderItems($orderItems)
    {
        writeStartTag("Items");

        foreach ($orderItems as $item)
        {
            $product = new lC_Product($item['id']);
            
            writeStartTag("Item");

                writeElement("Code", $item['model']);
                writeElement("SKU", $item['sku']);
                writeElement("Name", $item['name']);
                writeElement("Quantity", (int)$item['qty']);
                writeElement("UnitPrice", $item['price']);
                writeElement("Weight", ($product->getData('weight')? $product->getData('weight') : 0));

            writeCloseTag("Item");
        }

        writeCloseTag("Items");
    }

    // Returns the shipping status codes for the store
    function Action_GetStatusCodes()
    {
        global $lC_Database;
        $statusQuery = "SELECT * FROM ".TABLE_ORDERS_STATUS;
        
        $statuses = $lC_Database->simpleQuery($statusQuery);
        
        writeStartTag("StatusCodes");
        foreach ($statuses as $status)
        {
            writeStartTag("StatusCode");
            writeElement("Code", $status['orders_status_id']);
            writeElement("Name", $status['orders_status_name']);
            writeCloseTag("StatusCode");
        }
        writeCloseTag("StatusCodes");
        
    }

    function action_UpdateStatus()
    {
        global $lC_Database;
        
        $orderID = 0;
        $statusCode = '';
        $comments = '';

        if (!isset($_POST['order']) || !isset($_POST['status']) || !isset($_POST['comments']))
        {
            outputError(50, "Not all parameters supplied.");
            return;
        }

        $orderID = (int)$_REQUEST['order'];
        $statusCode = (int)$_REQUEST['status'];
        $comments = $lC_Database->parseString(($_REQUEST['comments']));

        // write the params for easier diagnostics
        writeStartTag("Parameters");
            writeElement("Order", $orderID);	
            writeElement("Status", $statusCode);
            writeElement("Comments", $comments);
        writeCloseTag("Parameters");

        $username = $lC_Database->parseString($_POST['username']);
        $userIdQuery = "SELECT id FROM ". TABLE_ADMINISTRATORS." WHERE user_name = '$username'";
            
        $userId = $lC_Database->simpleQuery($userIdQuery)->fetch_assoc();
        $userId = $userId['id'];
        
        $statusHistoryQuery = "INSERT INTO ".TABLE_ORDERS_STATUS_HISTORY." "
                            . "(`orders_id`, `orders_status_id`, `date_added`, `customer_notified`, `comments`, `administrators_id`, `append_comment`)"
                            . "VALUES ('$orderID',$statusCode,now(),0,'$comments',$userId,1)";
        
        $lC_Database->simpleQuery($statusHistoryQuery);

        $statusQuery = "UPDATE ".TABLE_ORDERS.""
                            . " SET `orders_status`= $statusCode"
                            . " WHERE orders_id = $orderID";
        
        $lC_Database->simpleQuery($statusQuery);
        

        echo "<UpdateSuccess/>";	
    }
    
    function TimeZoneName($offsetinSeconds){

    $timeZone = timezone_name_from_abbr("", $offsetinSeconds, FALSE);

    return $timeZone;
    }
?>
