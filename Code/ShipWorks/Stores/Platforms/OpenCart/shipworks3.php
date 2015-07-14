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
| Copyright 2009-2012 Interapptive, Inc.  All rights reserved.
| http://www.interapptive.com/
|
|
 */
define('REQUIRE_SECURE', TRUE);
$moduleVersion = "4.0.0.1";
$schemaVersion = "1.1.0";

header("Content-Type: text/xml;charset=utf-8");
header("Last-Modified: " . gmdate("D, d M Y H:i:s") . " GMT");

// HTTP/1.1
header("Cache-Control: no-store, no-cache, must-revalidate");
header("Cache-Control: post-check=0, pre-check=0", false);

// HTTP/1.0
header("Pragma: no-cache");     

//OpenCart
date_default_timezone_set ('UTC');

// Configuration
if (is_file('admin/config.php')) {
	require_once('admin/config.php');
}

// Startup
require_once(DIR_SYSTEM . 'startup.php');

require_once 'admin/model/sale/order.php';

// Registry
$registry = new Registry();

// Config
$config = new Config();
$registry->set('config', $config);

// Database
$db = new DB(DB_DRIVER, DB_HOSTNAME, DB_USERNAME, DB_PASSWORD, DB_DATABASE);
$registry->set('db', $db);

// Settings
$query = $db->query("SELECT * FROM " . DB_PREFIX . "setting WHERE store_id = '0'");

foreach ($query->rows as $setting) {
	if (!$setting['serialized']) {
		$config->set($setting['key'], $setting['value']);
	} else {
		$config->set($setting['key'], unserialize($setting['value']));
	}
}

// Loader
$loader = new Loader($registry);
$registry->set('load', $loader);

// Url
$url = new Url(HTTP_SERVER, $config->get('config_secure') ? HTTPS_SERVER : HTTP_SERVER);
$registry->set('url', $url);

// Log
$log = new Log($config->get('config_error_filename'));
$registry->set('log', $log);

$offsetQuery = $db->query("SELECT UNIX_TIMESTAMP() - UNIX_TIMESTAMP(UTC_TIMESTAMP()) AS offset")->row['offset'];
$offsetInSeconds = (int)($offsetQuery ? $offsetQuery : 0);
$mySqlTimeZone = TimeZoneName($offsetInSeconds);

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
	outputError(10, 'Invalid URL, HTTPS is required');
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
			case 'updatestatus': Action_UpdateStatus(); break;
			default:
				outputError(20, "Invalid action '$action'");
		}
	}
}

// Close the output
writeCloseTag("ShipWorks");

function checkAdminLogin()
{
	global $db;
	$loginOK = FALSE;

	if (isset($_REQUEST['username']) && isset($_REQUEST['password']))
	{
		$username = $_REQUEST['username'];
		$password = $_REQUEST['password'];

		$user_query = $db->query("SELECT * FROM `" . DB_PREFIX . "user` WHERE username = '" . $db->escape($username) . "' AND (password = SHA1(CONCAT(salt, SHA1(CONCAT(salt, SHA1('" . $db->escape($password) . "'))))) OR password = '" . $db->escape(md5($password)) . "') AND status = '1'")->row;


		if($user_query && $user_query['user_group_id'] == 1)
		{
			$loginOK = true;  
		}
	}

	if (!$loginOK)
	{
		outputError(50, "Username or password is incorrect");
		//$loginOK = true;
	}

	return $loginOK;
}

function Action_GetModule()
{
	writeStartTag("Module");
	
	writeElement("Platform", "OpenCart");
	writeElement("Developer", "Interapptive, Inc. (support@interapptive.com)");
	
	writeStartTag("Capabilities");
	writeElement("DownloadStrategy", "ByModifiedTime");
	writeFullElement("OnlineCustomerID", "", array("supported" => "true", "dataType" => "numeric"));
	writeFullElement("OnlineStatus", "", array("supported" => "true", "dataType" => "numeric", "downloadOnly" => "false", "supportsComments"=>"true" ));
	writeFullElement("OnlineShipmentUpdate", "", array("supported" => "false"));
	writeCloseTag("Capabilities");
	
	writeCloseTag("Module");
}

// Write store data
function Action_GetStore()
{
	global $config;
	
	$name = $config->get('config_name');
	$owner = $config->get('config_owner');
	$email = $config->get('config_email');
	$country = "";
	$website = "";
	$state = "";
	
	writeStartTag("Store");
	writeElement("Name", $name);
	writeElement("CompanyOrOwner", $owner);
	writeElement("Email", $email);
	writeElement("State", $state);
	writeElement("Country", $country);
	writeElement("Website", $website);
	writeCloseTag("Store");
}

// Get the count of orders greater than the start ID
function Action_GetCount()
{         
	global $mySqlTimeZone;
	global $db;
	
	$start = '1970-01-01';
	
	if (isset($_REQUEST['start']))
	{
		$start = $_REQUEST['start'];
	}
	
	//DateTime Object using start in UTC
	$lastmodified = new DateTime($start, new DateTimeZone('UTC'));
	
	//Convert to MySql offset
	$lastmodified->setTimezone(new DateTimeZone($mySqlTimeZone));
	$lastmodified = $lastmodified->format("Y-m-d H:i:s");

	//Count of orders newer than $start
	$query = $db->query("SELECT count(*) AS count FROM `" . DB_PREFIX . "order` WHERE date_modified > '$lastmodified'")->row['count'];
	$count = (int)($query ? $query : 0);
	
	writeElement("OrderCount", $count);
}

// Get all orders greater than the given start id, limited by max count
function Action_GetOrders()
{
	global $mySqlTimeZone;
	global $db;
	
	$start = '1970-01-01';
	
	if (isset($_REQUEST['start']))
	{
		$start = $_REQUEST['start'];
	}
	
	//DateTime Object using start in UTC
	$lastmodified = new DateTime($start, new DateTimeZone('UTC'));
	
	//Convert to MySql offset
	$lastmodified->setTimezone(new DateTimeZone($mySqlTimeZone));
	$lastmodified = $lastmodified->format("Y-m-d H:i:s");

	//Count of orders newer than $start
	$orders = $db->query("SELECT order_id AS order_id FROM `" . DB_PREFIX . "order` WHERE date_modified > '$lastmodified' order by date_modified asc LIMIT 0,50")->rows;
	
	writeStartTag("Orders");
	
	foreach($orders as $order){
		
		WriteOrder($order['order_id']);
	}
	
	writeCloseTag("Orders");
}

// Output the order as xml
function WriteOrder($orderid)
{  
	global $mySqlTimeZone;
	global $registry;
	
	writeStartTag("Order");
	
	$order = new ModelSaleOrder($registry);
	$orderDetails = $order->getOrder($orderid);
	
	//Convert from mysql time to utc
	$orderDate = new DateTime($orderDetails['date_added'], new DateTimeZone($mySqlTimeZone));
	$orderDate->setTimezone(new DateTimeZone('UTC'));

	//Convert from mysql time to utc
	$orderLastModified = new DateTime($orderDetails['date_modified'], new DateTimeZone($mySqlTimeZone));
	$orderLastModified->setTimezone(new DateTimeZone('UTC'));
	
	writeElement("OrderNumber", $orderDetails['order_id']);
	writeElement("OrderDate", $orderDate->format("Y-m-d\TH:i:s"));
	writeElement("LastModified",$orderLastModified->format("Y-m-d\TH:i:s"));
	writeElement("ShippingMethod", $orderDetails['shipping_method']);
	writeElement("StatusCode", $orderDetails['order_status_id']);
	writeElement("CustomerID", $orderDetails['customer_id']);
	
	//BillingAddress
	writeStartTag("BillingAddress");
	writeElement("FirstName", $orderDetails['payment_firstname']);
	writeElement("LastName", $orderDetails['payment_lastname']);
	writeElement("Company", $orderDetails['payment_company']);
	writeElement("Street1", $orderDetails['payment_address_1']);
	writeElement("Street2", $orderDetails['payment_address_2']);
	writeElement("Street3", '');
	writeElement("City", $orderDetails['payment_city']);
	writeElement("State", $orderDetails['payment_zone']);
	writeElement("PostalCode", $orderDetails['payment_postcode']);
	writeElement("Country", $orderDetails['payment_country']);
	writeElement("Phone", $orderDetails['telephone']);
	writeElement("Email", $orderDetails['email']);
	writeCloseTag("BillingAddress");
	
	//ShippingAddress
	writeStartTag("ShippingAddress");
	writeElement("FirstName", $orderDetails['shipping_firstname']);
	writeElement("LastName", $orderDetails['shipping_lastname']);
	writeElement("Company", $orderDetails['shipping_company']);
	writeElement("Street1", $orderDetails['shipping_address_1']);
	writeElement("Street2", $orderDetails['shipping_address_2']);
	writeElement("Street3", '');
	writeElement("City", $orderDetails['shipping_city']);
	writeElement("State", $orderDetails['shipping_zone']);
	writeElement("PostalCode", $orderDetails['shipping_postcode']);
	writeElement("Country", $orderDetails['shipping_country']);
	writeElement("Phone", $orderDetails['telephone']);
	writeElement("Email", $orderDetails['email']);
	writeCloseTag("ShippingAddress");
	
	//Items
	WriteOrderItems($order->getOrderProducts($orderDetails['order_id']));
	
	//Totals
	WriteOrderTotals($order->getOrderTotals($orderDetails['order_id']));

	//Payment
	writeStartTag("Payment");
	writeElement("Method", $orderDetails['payment_method']);

	writeStartTag("CreditCard");
	writeElement("Type", "");
	writeElement("Owner", "");
	writeElement("Number", "");
	writeElement("Expires", "");
	writeCloseTag("CreditCard");

	writeCloseTag("Payment");

	//Notes
	writeStartTag("Notes");
	writeFullElement("Note", $orderDetails['comment'], array("public" => "true"));
	writeCloseTag("Notes");
	
	writeCloseTag("Order");
}


// Write XML for all products for the given order
function WriteOrderItems($orderItems)
{
	global $registry;
	global $db;

	writeStartTag("Items");
	
	foreach ($orderItems as $item)
	{
		$order_id = $item['order_id'];
		$product_id = $item['product_id'];
		$order_product_id = $item['order_product_id'];

		$product = $db->query("SELECT DISTINCT *, (SELECT keyword FROM `" . DB_PREFIX . "url_alias` WHERE query = 'product_id=" . (int)$product_id . "') AS keyword FROM `" . DB_PREFIX . "product` p LEFT JOIN `" . DB_PREFIX . "product_description` pd ON (p.product_id = pd.product_id) WHERE p.product_id = '" . (int)$product_id ."'")->rows;
		$product = reset($product);

		$weight = 0;
		$productWeightClassId = (int)$product['weight_class_id'];

		if($productWeightClassId != ''){
			$weightsTable = $db->query("SELECT * FROM `" . DB_PREFIX . "weight_class` wc INNER JOIN `" . DB_PREFIX . "weight_class_description` wcd ON wc.weight_class_id = wcd.weight_class_id WHERE wc.weight_class_id = $productWeightClassId")->row;

			$weight = ($product['weight']/$weightsTable['value']) * 2.2046;
		}

		writeStartTag("Item");
		writeElement("Code", $item['model']);
		writeElement("SKU", $product['sku']);
		writeElement("Name", $item['name']);
		writeElement("Quantity", (int)$item['quantity']);
		writeElement("UnitPrice", (float)$item['price']);
		writeElement("Weight", number_format((float)$weight, 3, '.', ''));
		writeElement("Image", HTTP_CATALOG."image/".$product['image']);
		//writeElement("Thumbnail", HTTP_CATALOG."image/".$product['image']);
		writeStartTag("Attributes");

		$options = $db->query("SELECT name, value FROM `" . DB_PREFIX . "order_option` WHERE order_id = $order_id AND order_product_id = $order_product_id")->rows;

		foreach($options as $option){

			writeStartTag("Attribute");
			writeElement("Name", $option['name']);
			writeelement("Value", $option['value']);
			writeCloseTag("Attribute");
		}

		writeCloseTag("Attributes");
		
		writeCloseTag("Item");
	}
	
	writeCloseTag("Items");
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
function WriteOrderTotals($orderTotals)
{
	writeStartTag("Totals");
	
	foreach($orderTotals as $total)
	{
		if($total['code'] == 'sub_total')
		{
			WriteOrderTotal("Order Subtotal", (float)$total["value"], "subtotal", "none");
		}
		if($total['code'] == 'shipping')
		{
			WriteOrderTotal("Shipping and Handling", (float)$total["value"], "shipping", "add");
		}
		if($total['code'] == 'total')
		{
			WriteOrderTotal("Grand Total", (float)$total["value"], "total", "none");
		}
	}
	writeCloseTag("Totals");
}


// Returns the status codes for the store
function Action_GetStatusCodes()
{
	global $db;
	$statusQuery = $db->query("SELECT DISTINCT order_status_id as StatusCode, name as StatusName FROM ". DB_PREFIX ."order_status")->rows;
	
	writeStartTag("StatusCodes");
	foreach($statusQuery as $status)
	{
		writeStartTag("StatusCode");
		writeElement("Code", (int)$status['StatusCode']);
		writeElement("Name", $status['StatusName']);
		writeCloseTag("StatusCode");
	}
	writeCloseTag("StatusCodes");
	
}

function Action_UpdateStatus()
{
	global $db;
	
	$order_id = (int)$_REQUEST['order'];
	$order_status_id = (int)$_REQUEST['status'];
	$comment = (string)$_REQUEST['comments'];
	$notify = 1;

	$addOrderHistory = $db->query("INSERT INTO " . DB_PREFIX . "order_history SET order_id = '" . $order_id . "', order_status_id = '" . $order_status_id . "', notify = '" . $notify . "', comment = '" . $db->escape($comment) . "', date_added = NOW()");
	$updateOrderStatus = $db->query("UPDATE " . DB_PREFIX . "order SET order_status_id ='" . $order_status_id . "' WHERE order_id ='" . $order_id."'");
}

function TimeZoneName($offsetinSeconds)
{
	$timeZone = timezone_name_from_abbr("", $offsetinSeconds, FALSE);
	return $timeZone;
}

?>