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
| Copyright Interapptive, Inc.  All rights reserved.
| http://www.interapptive.com/
|
|
*/

define('REQUIRE_SECURE', true);

use Tygh\Bootstrap;
use Tygh\Exceptions\DatabaseException;
use Tygh\Registry;

// Register autoloader
$this_dir = dirname(__FILE__);
$classLoader = require($this_dir . '/app/lib/vendor/autoload.php');
$classLoader->add('Tygh', $this_dir . '/app');

// Prepare environment and process request vars
list($_REQUEST, $_SERVER) = Bootstrap::initEnv($_GET, $_POST, $_SERVER, $this_dir);

// Get config data
$config = require(DIR_ROOT . '/config.php');

// Load core functions
$fn_list = array(
    'fn.database.php',
    'fn.users.php',
    'fn.catalog.php',
    'fn.cms.php',
    'fn.cart.php',
    'fn.locations.php',
    'fn.common.php',
    'fn.fs.php',
    'fn.images.php',
    'fn.init.php',
    'fn.control.php',
    'fn.search.php',
    'fn.promotions.php',
    'fn.log.php',
    'fn.companies.php',
    'fn.addons.php'
);

$fn_list[] = 'fn.' . strtolower(PRODUCT_EDITION) . '.php';

foreach ($fn_list as $file) {
    require($config['dir']['functions'] . $file);
}

Registry::set('class_loader', $classLoader);
Registry::set('config', $config);
unset($config);

// Connect to database
if (!db_initiate(Registry::get('config.db_host'), Registry::get('config.db_user'), Registry::get('config.db_password'), Registry::get('config.db_name'))) {
    throw new DatabaseException('Cannot connect to the database server');
}

    # ShipWorks configuration
    $moduleVersion = "3.10.0.0";
    $schemaVersion = "1.0.0";

    header("Content-Type: text/xml;charset=utf-8");
    header("Last-Modified: " . gmdate("D, d M Y H:i:s") . " GMT");

    // HTTP/1.1
    header("Cache-Control: no-store, no-cache, must-revalidate");
    header("Cache-Control: post-check=0, pre-check=0", false);

    // HTTP/1.0
    header("Pragma: no-cache");	

    function writeXmlDeclaration()
    {
        echo "<?xml version=\"1.0\" standalone=\"yes\" ?>";
    }

    function toUtf8($string)
    {
        return iconv("ISO-8859-1", "UTF-8//TRANSLIT", $string);
    }

    function writeStartTag($tag, $attributes = null)
    {
        echo toUtf8('<' . $tag);

        if ($attributes != null)
        {
            echo ' ';

            foreach ($attributes as $name => $attribValue)
            {
                echo toUtf8($name. '="'. htmlspecialchars($attribValue). '" ');	
            }
        }

        echo '>';
    }

    function writeCloseTag($tag)
    {
        echo toUtf8('</' . $tag . '>');
    }

    // Output the given tag\value pair
    function writeElementSafe($tag, $value)
    {
        writeStartTag($tag);
        echo toUtf8(htmlspecialchars($value));
        writeCloseTag($tag);
    }

    // Output the given tag\value pair
    function writeElement($tag, $value)
    {
        writeStartTag($tag);
        echo toUtf8($value);
        writeCloseTag($tag);
    }

    // Outputs the given name/value pair as an xml tag with attributes
    function writeFullElement($tag, $value, $attributes)
    {
        echo toUtf8('<'. $tag. ' ');

        foreach ($attributes as $name => $attribValue)
        {
            echo toUtf8($name. '="'. htmlspecialchars($attribValue). '" ');	
        }
        echo '>';
        echo toUtf8(htmlspecialchars($value));
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

    $secure = ($_SERVER['HTTPS'] == 'on' || $_SERVER['HTTPS'] == '1');

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
        // If the admin module is installed, we make use of it
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
                case 'updateshipment': Action_UpdateShipment(); break;
                case 'updatestatus': Action_UpdateStatus(); break;
				default:
					outputError(20, "'$action' is not supported.");
			}
        }
    }
    
    writeCloseTag("ShipWorks");
    
    function checkAdminLogin()
    {
        $loginOK = false;

        if (isset($_REQUEST['username']) && isset($_REQUEST['password']))
        {
            $username = $_REQUEST['username'];
            $password = $_REQUEST['password'];

            $user_data = db_get_row("SELECT user_id, password, salt FROM ?:users WHERE email = ?i", $username);
            if (fn_generate_salted_password($password, $user_data['salt']) == $user_data['password'])
            {
                $loginOK = true;        
            }
        }

        if (!$loginOK)
        {
            outputError(50, "The username or password is incorrect.");
        }

        return $loginOK;
    }

    function Action_GetModule()
    {
        writeStartTag("Module");

        writeElement("Platform", "CS-Cart");
        writeElement("Developer", "Interapptive, Inc. (support@interapptive.com)");
        
        writeStartTag("Capabilities");
            writeElement("DownloadStrategy", "ByOrderNumber");
            writeFullElement("OnlineCustomerID", "", array("supported" => "false"));
            writeFullElement("OnlineStatus", "", array("supported" => "true", "dataType" => "text"));
            writeFullElement("OnlineShipmentUpdate", "", array("supported" => "true"));
        writeCloseTag("Capabilities");
            
        writeCloseTag("Module");
    }
    
    function Action_GetStore()
    {
        $name = Registry::get('settings.Company.company_name');
        $owner = Registry::get('settings.Company.company_name');
        $email = Registry::get('settings.Company.company_site_administrator');
        $country = Registry::get('settings.Company.company_country');


        writeStartTag("Store");
            writeElement("Name", $name);
            writeElement("CompanyOrOwner", $owner);
            writeElement("Email", $email);
            writeElement("State", $state);
            writeElement("Country", $country);
        writeCloseTag("Store");
    }
    
    function Action_GetCount()
    {
        $start = 0;

        if (isset($_REQUEST['start']))
        {
            $start = $_REQUEST['start'];
        }

        // write the params for easier diagnostics
        writeStartTag("Parameters");
            writeElement("Start", $start);	
        writeCloseTag("Parameters");

        $count = db_get_fields("SELECT COUNT(*) FROM ?:orders WHERE order_id > ?i", $start);

        writeElement("OrderCount", $count[0]);
    }
        
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

        // write the parameters out for diagnostics
        writeStartTag("Parameters");
        writeElement("Start", $start);
        writeElement("MaxCount", $maxcount);	
        writeCloseTag("Parameters"); 

        writeStartTag("Orders");

        $orderids = db_get_fields("SELECT order_id FROM ?:orders WHERE order_id > ?i limit ?i", $start, $maxcount);
        
        if($orderids)
		{
            foreach ($orderids as $orderid) 
			{        
				WriteOrder($orderid);	
            }
        }
        writeCloseTag("Orders");
    }

    function WriteOrder($orderid)
    {
        $orderDetails = fn_get_order_info($orderid);
        
        $order_shipping = '';
		
		if ($orderDetails['shipping'] && is_array($orderDetails['shipping'])) 
		{
			foreach ($orderDetails['shipping'] as $ship) 
			{
				if ($ship['shipping']) 
				{
					$order_shipping = $ship['shipping'];
					break;
				}
			}
		}
        
        
         writeStartTag("Order");

            writeElement("OrderNumber", $orderDetails['order_id']);
            writeElement("OrderDate", date('Y-m-d\TH:i:s',$orderDetails['timestamp']));
            writeElement("ShippingMethod", $order_shipping);
            writeElement("StatusCode", $orderDetails['status']);	

        
            //writeStartTag("Notes");
                //WriteNote($orderDetails['order']['notes'], false);
                //WriteNote($orderDetails['order']['customer_notes'], true);
            //writeCloseTag("Notes");

            // Shipping address information
            writeStartTag("ShippingAddress");

                // output company here since xcart doesn't put company on the ship or bill address
                writeElementSafe("FirstName", $orderDetails['s_firstname']);
                writeElementSafe("LastName",  $orderDetails['s_lastname']);
                writeElementSafe("Company", $orderDetails['company']);
                writeElementSafe("Street1", $orderDetails['s_address']);
                writeElementSafe("Street2", $orderDetails['s_address_2']);
                writeElementSafe("Street3", "");
                writeElementSafe("City", $orderDetails['s_city']);
                writeElementSafe("State", $orderDetails['s_state']);
                writeElementSafe("PostalCode", $orderDetails['s_zipcode']);
                writeElementSafe("Country", $orderDetails['s_country']);
                writeElementSafe("Phone", $orderDetails['s_phone']);
                writeElementSafe("Email", $orderDetails['email']);

            writeCloseTag("ShippingAddress");

            // Billing address information
            writeStartTag("BillingAddress");
                writeElementSafe("FirstName", $orderDetails['b_firstname']);
                writeElementSafe("LastName",  $orderDetails['b_lastname']);
                writeElementSafe("Company", $orderDetails['company']);
                writeElementSafe("Street1", $orderDetails['b_address']);
                writeElementSafe("Street2", $orderDetails['b_address_2']);
                writeElementSafe("Street3", "");
                writeElementSafe("City", $orderDetails['b_city']);
                writeElementSafe("State", $orderDetails['b_state']);
                writeElementSafe("PostalCode", $orderDetails['b_zipcode']);
                writeElementSafe("Country", $orderDetails['b_country']);
                writeElementSafe("Phone", $orderDetails['b_phone']);
                writeElementSafe("Email", $orderDetails['email']);
            writeCloseTag("BillingAddress");

            WriteOrderItems($orderDetails['products']);
            
            WriteOrderTotals($orderDetails);

        writeCloseTag("Order");
    }

    function WriteOrderItems($orderItems)
    {
        writeStartTag("Items");

            // go through each item in the collection
            foreach ($orderItems as $item)
            {
				writeStartTag("Item");
    
					$itemWeight = db_get_field("SELECT weight FROM ?:products WHERE product_id=?i", $item['product_id']);
    
					writeElementSafe("Code", $item['product_code']);
					writeElementSafe("SKU", $item['product_code']);
					writeElementSafe("Name", $item['product']);
					writeElement("Quantity", $item['amount']);
					writeElement("UnitPrice", $item['price']);
        
					writeElement("Weight", $itemWeight);
                
				writeCloseTag("Item");
			}

        writeCloseTag("Items");
    }

    function WriteOrderTotals($order)
    {
        writeStartTag("Totals");

            WriteOrderTotal("Tax",  $order['tax_subtotal'], "Tax", "add");
        
            WriteOrderTotal("Shipping",  $order['shipping_cost'], "Shipping", "add");
        
            WriteOrderTotal("SubTotal",  $order['subtotal'], "SubTotal", "none");
        
            WriteOrderTotal("Total",  $order['total'], "total", "none");

            WriteOrderTotal("PaymentSurcharge",  $order['payment_surcharge'], "PaymentFee", "add");
                        
        writeCloseTag("Totals");
    }

    function WriteOrderTotal($name, $value, $class, $impact = "add")
    {
        if ($value > 0)
        {
            writeFullElement("Total", $value, array("name" => $name, "class" => $class, "impact" => $impact));
        }
    }
    
    function Action_GetStatusCodes()
    {
        writeStartTag("StatusCodes");
        
            $statuses = fn_get_statuses(STATUSES_ORDER,array(),false,false,'en');

            foreach ($statuses as $status)
            {
                writeStartTag("StatusCode");
                writeElement("Code", $status['status']);
                writeElement("Name", $status['description']);
                writeCloseTag("StatusCode");
            }
            
        writeCloseTag("StatusCodes");
    }

    function Action_UpdateShipment()
    {
        if (isset($_REQUEST['order'])) 
        {
            $orderid = $_REQUEST['order'];
            $order = fn_get_order_info($orderid);
        }
        
        $tracking_number = $_REQUEST['tracking'];
        
        $products = $order['products'];
    
        switch ($_REQUEST['carrier']) {
        case 'USPS':
                $carrier = 'USP';
                break;
        case 'UPS':
                $carrier = 'UPS';
                break;
        case 'FedEx':
                $carrier = 'FDX';
                break;
        case 'DHL':
                $carrier = 'DHL';
                break;
        default:
                $carrier = '';
        }
       		
        if($order)
        {	
            $shipment_data = array(
            'shipping_id'=> $order['shipping']['0']['shipping_id'],
            'tracking_number' => $tracking_number,
            'carrier' => $carrier,
            'timestamp' => !empty($_REQUEST['shippingdate']) ? strtotime($_REQUEST['shippingdate']) : time(),
            'comments' => ''
            );

            $shipment_id = db_query("INSERT INTO ?:shipments ?e", $shipment_data);

            if($shipment_id)
            {
                foreach($products as $product)
                {

                    $shipment_item = array(
                        'item_id' => $product['item_id'],
                        'shipment_id' => $shipment_id,
                        'order_id' => $orderid,
                        'product_id' => $product['product_id'],
                        'amount' => $product['shipment_amount']
                    );

                    db_query("INSERT INTO ?:shipment_items ?e", $shipment_item);
                }
            }
        }
    }
    
    function Action_UpdateStatus()
    {
        if (isset($_REQUEST['order'])) 
		{
            $orderid = $_REQUEST['order'];
        }
        
        if (isset($_REQUEST['status'])) 
		{
            $statusto = $_REQUEST['status'];
        }
        
        $order = fn_get_order_info($orderid);
        
        if($order)
        {
            $statusfrom = $order['status'];

            writeStartTag("Parameters");
            writeElement("order", $orderid);
            writeElement("status", $statusto);
            writeCloseTag("Parameters");

            if(!fn_change_order_status($orderid, $statusto, $statusfrom, false)){

                    outputError(73, "Unable to change order status");
            } 
        }	               
    }