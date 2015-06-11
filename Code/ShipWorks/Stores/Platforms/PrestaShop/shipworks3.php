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

define('REQUIRE_SECURE', FALSE);
$moduleVersion = "3.10.0";
$schemaVersion = "1.1.0";

require'config/config.inc.php';

//Timezone from PrestaShop
$psTimeZone = new DateTimeZone(Configuration::get('PS_TIMEZONE'));

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
	outputError(10, "Invalid URL, HTTPS is required");
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

//Check username, password
function checkAdminLogin()
{
	$loginOK = false;

	if (isset($_REQUEST['username']) && isset($_REQUEST['password']))
	{

		$username = $_REQUEST['username'];
		$password = $_REQUEST['password'];
		
		$sql = new DbQuery();
		$sql->select('id_employee');
		$sql->from('employee', 'e');
		$sql->where("e.email = '$username'");

		$employeeId = Db::getInstance()->getRow($sql);
		
		if($employeeId)
		{
			$employeeId = $employeeId['id_employee'];

			$encryptedPassword = Tools::encrypt($password);
			if (Employee::checkPassword($employeeId, $encryptedPassword))
			{
				$loginOK = true;        
			}
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

	writeElement("Platform", "PrestaShop");
	writeElement("Developer", "Interapptive, Inc. (support@interapptive.com)");

	writeStartTag("Capabilities");
	writeElement("DownloadStrategy", "ByModifiedTime");
	writeFullElement("OnlineCustomerID", "", array("supported" => "true", "dataType" => "numeric"));
	writeFullElement("OnlineStatus", "", array("supported" => "true", "supportsComments" => "false", "downloadOnly"=>"false", "dataType" => "numeric"));
	writeFullElement("OnlineShipmentUpdate", "", array("supported" => "true"));
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

	$name = Configuration::get('PS_SHOP_NAME');
	$companyOrOwner = Configuration::get('');
	$email = Configuration::get('PS_SHOP_EMAIL');
	$street1 = Configuration::get('PS_SHOP_ADDR1');
	$street2 = Configuration::get('PS_SHOP_ADDR2');
	$street3 = '';
	$city = Configuration::get('PS_SHOP_CITY');
	$state = Configuration::get('PS_SHOP_STATE');
	$postalCode = Configuration::get('PS_SHOP_CODE');
	$country = Configuration::get('PS_SHOP_COUNTRY');
	$phone = Configuration::get('PS_SHOP_PHONE');
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

// Get the count of orders greater than the start date
function Action_GetCount()
{         
	global $psTimeZone;
	$start = '1970-01-01';
	
	if($_REQUEST['start'])
	{
		$start = $_REQUEST['start'];
	}
	
	//Date/Time from ShipWorks in UTC
	$start = new DateTime($start, new DateTimeZone('UTC'));
	$start = $start->setTimezone($psTimeZone);
	
	// only get orders through 2 seconds ago
	$end = new DateTime(date("Y-m-d\TH:i:s", time()-2), $psTimeZone);

	// Write the params for easier diagnostics
	writeStartTag("Parameters");
	writeElement("Start", $start->format("Y-m-d\TH:i:s"));
	writeElement("End", $end->format("Y-m-d\TH:i:s"));
	writeCloseTag("Parameters");

	$startSQL = $start->format("Y-m-d H:i:s");
	$endSQL = $end->format("Y-m-d H:i:s");
	
	$sql = new DbQuery();
	$sql->select('count(*) as count');
	$sql->from('orders', 'o');
	$sql->where("o.date_upd > '$startSQL'");
	$sql->where("o.date_upd < '$endSQL'");

	$count = Db::getInstance()->getRow($sql);

	writeElement("OrderCount", $count['count']);
}

// Get all orders greater than the given start date, limited by max count
function Action_GetOrders()
{
	$start = '1970-01-01';
	$maxcount = 50;
	global $psTimeZone;

	if (isset($_REQUEST['start']))
	{
		$start = $_REQUEST['start'];
	}

	if (isset($_REQUEST['maxcount']))
	{
		$maxcount = (int)$_REQUEST['maxcount'];
	}
	
	//Date/Time from ShipWorks in UTC
	$start = new DateTime($start, new DateTimeZone('UTC'));
	$start = $start->setTimezone($psTimeZone);
	
	// only get orders through 2 seconds ago
	$end = new DateTime(date("Y-m-d\TH:i:s", time()-2), $psTimeZone);

	// Write the params for easier diagnostics
	writeStartTag("Parameters");
	writeElement("Start", $start->format("Y-m-d\TH:i:s"));
	writeElement("End", $end->format("Y-m-d\TH:i:s"));
	writeElement("MaxCount", $maxcount);
	writeCloseTag("Parameters");                                    

	$startSQL = $start->format("Y-m-d H:i:s");
	$endSQL = $end->format("Y-m-d H:i:s");
	
	$sql = new DbQuery();
	$sql->select('id_order');
	$sql->from('orders', 'o');
	$sql->where("o.date_upd > '$startSQL'");
	$sql->where("o.date_upd < '$endSQL'");
	$sql->orderBy("o.date_upd");
	$sql->limit($maxcount,0);
	
	$orderids = Db::getInstance()->executeS($sql);
	
	writeStartTag("Orders");

	$start = null;
	$processedIds = '';
	
	foreach ($orderids as $orderid)
	{
		$order = new Order($orderid['id_order']);
		$start = new DateTime($order->date_upd, $psTimeZone);
		$startSQL = $start->format("Y-m-d H:i:s");
		
		// Add the id to the list we have processed
		if ($processedIds != "")
		{
			$processedIds .= ", ";
		}
		
		$processedIds .= $orderid['id_order'];
		WriteOrder($order);
	}

	//make sure that we dont skip an order if it has the same lastmodified as order #50 from above
	if ($processedIds)
	{
		$sql = new DbQuery();
		$sql->select('id_order');
		$sql->from('orders', 'o');
		$sql->where("o.date_upd = '$startSQL'");
		$sql->where("o.id_order not in ($processedIds)");
		
		$skippedOrderids = Db::getInstance()->executeS($sql);
		
		foreach ($skippedOrderids as $orderid)
		{
			$order = new Order($orderid['id_order']);
			WriteOrder($order);
		}
	}

	writeCloseTag("Orders");
}

// Output the order as xml
function WriteOrder($order)
{                 
	global $secure;
	global $psTimeZone;
	
	$orderDate = new DateTime($order->date_add, $psTimeZone);
	$orderDate = $orderDate->setTimezone(new DateTimeZone('UTC'));
	
	$lastModified = new DateTime($order->date_upd, $psTimeZone);
	$lastModified = $lastModified->setTimezone(new DateTimeZone('UTC'));
	
	writeStartTag("Order");

	writeElement("OrderNumber", $order->id);
	writeElement("OrderDate", $orderDate->format("Y-m-d\TH:i:s"));
	writeElement("LastModified", $lastModified->format("Y-m-d\TH:i:s"));
	
	$carrier = new Carrier($order->id_carrier);
	
	writeElement("ShippingMethod", $carrier->name);
	writeElement("StatusCode", $order->current_state);

	writeStartTag("Notes");
	
	$sql = new DbQuery();
	$sql->select('id_message');
	$sql->from('message', 'm');
	$sql->where("m.id_order = '$order->id'");
	
	$messageIds = Db::getInstance()->executeS($sql);
	
	foreach($messageIds as $messageId)
	{
		$message = new Message($messageId['id_message']);
		writeFullElement("Note", $message->message, array("public" => "true"));
	}
	
	writeCloseTag("Notes");
	
	$customer = new Customer($order->id_customer);
	$billToAddress = new Address($order->id_address_invoice);
	$billToState = new State($billToAddress->id_state);
	
	writeStartTag("BillingAddress");
	writeElement("FirstName", $billToAddress->firstname);
	writeElement("LastName", $billToAddress->lastname);
	writeElement("Company",$billToAddress->company);
	writeElement("Street1", $billToAddress->address1);
	writeElement("Street2",$billToAddress->address2);
	writeElement("Street3","");
	writeElement("City", $billToAddress->city);
	writeElement("State", $billToState->iso_code);
	writeElement("PostalCode", $billToAddress->postcode);
	writeElement("Country", $billToAddress->country);
	writeElement("Phone", $billToAddress->phone);
	writeElement("Email",$customer->email);
	writeCloseTag("BillingAddress");
	
	
	$shipToAddress = new Address($order->id_address_delivery);
	$shipToState = new State($shipToAddress->id_state);
	
	writeStartTag("ShippingAddress");
	writeElement("FirstName", $shipToAddress->firstname);
	writeElement("LastName", $shipToAddress->lastname);
	writeElement("Company",$shipToAddress->company);
	writeElement("Street1", $shipToAddress->address1);
	writeElement("Street2",$shipToAddress->address2);
	writeElement("Street3","");
	writeElement("City", $shipToAddress->city);
	writeElement("State", $shipToState->iso_code);
	writeElement("PostalCode", $shipToAddress->postcode);
	writeElement("Country", $shipToAddress->country);
	writeElement("Phone", $shipToAddress->phone);
	writeElement("Email",$customer->email);
	writeCloseTag("ShippingAddress");
	

	writeStartTag("Payment");
	writeElement("Method", $order->payment);
	writeCloseTag("Payment");

	WriteOrderItems($order->getProducts());

	WriteOrderTotals($order);

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
function WriteOrderTotals($order)
{
	writeStartTag("Totals");

	WriteOrderTotal("Shipping and Handling", $order->total_shipping_tax_incl, "shipping", "add");

	WriteOrderTotal("Discounts", $order->total_discounts, "discount","subtract");

	WriteOrderTotal("Grand Total", $order->total_paid, "total", "none");
	
	writeCloseTag("Totals");
}

// Write XML for all products for the given order
function WriteOrderItems($orderItems)
{
	writeStartTag("Items");

	foreach ($orderItems as $item)
	{
		writeStartTag("Item");

		writeElement("Code", $item['product_reference']);
		writeElement("SKU", $item['product_reference']);
		writeElement("Name", $item['product_name']);
		writeElement("Quantity", (int)$item['product_quantity']);
		writeElement("UnitPrice", $item['product_price']);
		writeElement("Weight", $item['product_weight']);

		writeCloseTag("Item");
	}

	writeCloseTag("Items");
}

// Returns the shipping status codes for the store
function Action_GetStatusCodes()
{
	$status = new OrderState();
	$shippingStatus = $status->getOrderStates('1');

	writeStartTag("StatusCodes");
	foreach ($shippingStatus as $status)
	{
		writeStartTag("StatusCode");
		writeElement("Code", $status['id_order_state']);
		writeElement("Name", $status['name']);
		writeCloseTag("StatusCode");
	}
	writeCloseTag("StatusCodes");
	
}

//Update order status
function action_UpdateStatus()
{
	$orderId = 0;
	$statusCode = '';
	$comments = '';

	if (!isset($_POST['order']) || !isset($_POST['status']) || !isset($_POST['comments']))
	{
		outputError(40, "Insufficient parameters");
		return;
	}

	$orderId = $_REQUEST['order'];
	$statusCode = (int)$_REQUEST['status'];
	$comments = $_REQUEST['comments'];

	// write the params for easier diagnostics
	writeStartTag("Parameters");
	writeElement("Order", $orderId);	
	writeElement("Status", $statusCode);
	writeElement("Comments", $comments);
	writeCloseTag("Parameters");
	
	$order = new Order($orderId);
	$order->setCurrentState($statusCode);
	echo "<UpdateSuccess/>";	
}

//Upload tracking
function action_UpdateShipment()
{
	$orderId = 0;
	$trackingNumber = '';
	

	if (!isset($_POST['order']) || !isset($_POST['tracking']))
	{
		outputError(40, "Insufficient parameters");
		return;
	}

	$orderId = $_POST['order'];
	$trackingNumber = $_POST['tracking'];
	
	// write the params for easier diagnostics
	writeStartTag("Parameters");
	writeElement("OrderID", $orderId);	
	writeElement("Tracking", $trackingNumber);
	writeCloseTag("Parameters");

	$order = new Order($orderId);

	$order->shipping_number = $trackingNumber;
	$order->update();
	
	$orderCarrier = new OrderCarrier($orderId);
	$orderCarrier->tracking_number = $trackingNumber;
	$orderCarrier->update();
	
	echo "<UpdateSuccess/>";	
}
?>