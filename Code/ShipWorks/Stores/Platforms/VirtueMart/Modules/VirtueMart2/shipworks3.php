<?php
/*
    |
    | This file and the source codes contained herein are the property
    | of Interapptive, Inc.  Use of this file is restricted to the specific
    | terms and conditions in the License Agreement associated with this
    | file.  Distribution of this file or portions of this file for uses
    | not covered by the License Agreement is not allowed without a written
    | agreement signed by an officer of Interapptive, Inc.
    |
    | The code contained herein may not be reproduced, copied or
    | redistributed in any form, as part of another product or otherwise.
    | Modified versions of this code may not be sold or redistributed.
    |
    | Copyright 2009-2012 Interapptive, Inc.  All rights reserved.
    | http://www.interapptive.com/
 */

// flag indicating if we will require SSL connection or not (default is true)
define('REQUIRE_SECURE', true);

//set this constant so we can poke into joomla/virtuemart files
define('_JEXEC', true);

//virtue mart setup
define('JPATH_BASE', '.');
define('DS', DIRECTORY_SEPARATOR);
require_once('configuration.php');
require_once(JPATH_BASE . DS . 'includes' . DS . 'defines.php');
require_once(JPATH_BASE . DS . 'includes' . DS . 'framework.php');
require_once(JPATH_BASE . DS . 'libraries' . DS . 'joomla' . DS
	. 'factory.php');

$moduleVersion = "3.4.0.2";
$schemaVersion = "1.0.0";

header("Last-Modified: " . gmdate("D, d M Y H:i:s") . " GMT");

// turn off caching - the HTTP/1.1 way
header("Content-Type: text/xml;charset=utf-8");
header("Cache-Control: no-store, no-cache, must-revalidate");
header("Cache-Control: post-check=0, pre-check=0", false);

// turn off caching - the HTTP/1.0 way
header("Pragma: no-cache");

function toUtf8($string)
{
	return iconv("ISO-8859-1", "UTF-8//TRANSLIT", $string);
}

//basic XML helper functions
function writeXmlDeclaration()
{
	echo "<?xml version=\"1.0\" standalone=\"yes\" ?>";
}

function writeStartTag($tag, $attributes = null)
{
	echo toUtf8('<' . $tag);

	if ($attributes != null) {
		echo ' ';

		foreach ($attributes as $name => $attribValue) {
			echo toUtf8($name . '="' . htmlspecialchars($attribValue) . '" ');
		}
	}

	echo '>';
}

function writeCloseTag($tag)
{
	echo toUtf8('</' . $tag . '>');
}

// Output the given tag\value pair
function writeElement($tag, $value)
{
	writeStartTag($tag);
	echo toUtf8(htmlspecialchars($value));
	writeCloseTag($tag);
}

// Outputs an xml element with the provided attributes
function writeElementWithAttributes($tag, $value, $attributes)
{
	echo toUtf8('<' . $tag . ' ');

	foreach ($attributes as $name => $attribValue) {
		echo toUtf8($name . '="' . htmlspecialchars($attribValue) . '" ');
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

// end basic XML helper functions

if (@VM_ENCRYPT_FUNCTION == 'AES_ENCRYPT') {
	define('VM_DECRYPT_FUNCTION', 'AES_DECRYPT');
} else {
	define('VM_DECRYPT_FUNCTION', 'DECODE');
}

//are we on an SSL connection?
$secure = ($_SERVER['HTTPS'] == 'on' || $_SERVER['HTTPS'] == '1');

$dbPrefix = 'jos_';

if (class_exists("JConfig")) {
	$CONFIG = new JConfig();
	if ($CONFIG->dbprefix) {
		$dbPrefix = $CONFIG->dbprefix;
	}
	$conn = mysql_connect($CONFIG->host, $CONFIG->user, $CONFIG->password);
}

// Open the XML output and root
writeXmlDeclaration();
writeStartTag(
	"ShipWorks",
	array("moduleVersion" => $moduleVersion, "schemaVersion" => $schemaVersion)
	);

// Enforce SSL
if (!$secure && REQUIRE_SECURE) {
	outputError(10, 'Invalid URL, HTTPS is required');
} // Connect to database
else {
	if (!db_connect()) {
		outputError(
			70, "The ShipWorks module was unable to connect to database"
			);
	} // Proceed
	else {

		// verify that the credentials are valid
		if (checkAdminLogin()) {
			$action = (isset($_REQUEST['action']) ? $_REQUEST['action'] : '');
			if (not_null($action)) {
				switch (strtolower($action)) {
					case 'getmodule':
						Action_GetModule();
						break;
					case 'getstore':
						Action_GetStore();
						break;
					case 'getcount':
						Action_GetCount();
						break;
					case 'getorders':
						Action_GetOrders();
						break;
					case 'getstatuscodes':
						Action_GetStatusCodes();
						break;
					case 'updatestatus':
						Action_UpdateStatus();
						break;
					case 'debug':
						Action_Debug();
						break;
					default:
						outputError(20, "Invalid action '$action'");
				}
			}
		}
	}
}

// Close the output
writeCloseTag("ShipWorks");

// Determines the root web location for product images
function GetImageRoot()
{
	global $secure;

	$scriptPath = $_SERVER["SCRIPT_NAME"];
	$serverName = $_SERVER["SERVER_NAME"];
	$hostName = $_SERVER["HTTP_HOST"];

	// get the host name to the website since it isn't stored anywhere
	if (strtoupper($serverName) == "LOCALHOST" || $serverName == "127.0.0.1") {
		// default to the http request host used to access the server
		$serverName = $hostName;
	}

	preg_match('/(.*)\//', $scriptPath, $matches);
	$cartRoot = "$serverName$matches[1]";

	return "http://$cartRoot/";
}

// Debugging
function Action_Debug()
{
	$imageRoot = GetImageRoot();

	print "\n\nENCODE_KEY = " . ENCODE_KEY;
	print "\n\nVM_ENCRYPT_FUNCTION = " . @VM_ENCRYPT_FUNCTION;
	print "\n\nVM_DECRYPT_FUNCTION = " . VM_DECRYPT_FUNCTION;
	print "\n\nImage Root = $imageRoot";
}

// Check to see if admin functions exist.  And if so, determine if the user
// has access.
function checkAdminLogin()
{
	global $dbPrefix;
	$loginOK = false;

	if (isset($_REQUEST['username']) && isset($_REQUEST['password'])) {
		$username = $_REQUEST['username'];
		$password = $_REQUEST['password'];

		$sql = "SELECT id, password"
			. " FROM " . $dbPrefix . "users"
			. " WHERE username='" . mysql_real_escape_string($username) . "'";
		$check_adminquery = mysql_query($sql);

		//$check_adminquery is false...
		//Joomla < 2.5.18 method of auth
		if (mysql_num_rows($check_adminquery)) {
			$check_admin = mysql_fetch_array($check_adminquery);

			// Check that password is good
			$parts = explode(':', $check_admin['password']);
			$crypt = $parts[0];
			$salt = @$parts[1];
			$testcrypt = getCryptedPassword($password, $salt);

			if ($crypt == $testcrypt) {
				$loginOK = true;
			}
		}

		//Attempt this only if $loginOK is still false
		//Joomla > 2.5.17 method of auth
		if (!$loginOK) {
			$db = JFactory::getDBO();

			$query = "SELECT id,username, password FROM " . $dbPrefix
				. "users WHERE username = '$username'";
			$db->setQuery($query);

			if ($result = $db->loadObject()) {
				$match = JUserHelper::verifyPassword(
					$password, $result->password, $result->id
					);

				if ($match === true) {
					$loginOK = true;
				} else {
					$loginOK = false;
				}
			} else {
				$loginOK = false;
			}
		}

		if (!$loginOK) {
			outputError(50, "Username or password is incorrect");
		}
	} else {
		outputError(40, "Insufficient parameters");
	}

	return $loginOK;
}

// Get module data
function Action_GetModule()
{
	writeStartTag("Module");

	writeElement("Platform", "VirtueMart");
	writeElement("Developer", "Interapptive, Inc. (support@interapptive.com)");

	writeStartTag("Capabilities");
	writeElement("DownloadStrategy", "ByModifiedTime");
	writeElementWithAttributes(
		"OnlineCustomerID", "", array("supported" => "true")
		);
	writeElementWithAttributes(
		"OnlineStatus", "",
		array("supported" => "true", "supportsComments" => "true")
		);
	writeElementWithAttributes(
		"OnlineShipmentUpdate", "", array("supported" => "false")
		);
	writeCloseTag("Capabilities");

	writeCloseTag("Module");
}

// Write store data
function Action_GetStore()
{
	global $dbPrefix;
	$sql = "SELECT * FROM " . $dbPrefix . "virtuemart_vmusers vmu" .
		" inner join " . $dbPrefix . "users u on u.id=vmu.virtuemart_user_id" .
		" inner join " . $dbPrefix
		. "virtuemart_userinfos ui on ui.virtuemart_user_id = vmu.virtuemart_user_id"
		.
		" inner join " . $dbPrefix
		. "virtuemart_vendors_en_gb en on en.virtuemart_vendor_id = vmu.virtuemart_vendor_id"
		.
		" inner join " . $dbPrefix
		. "virtuemart_states s on s.virtuemart_state_id = ui.virtuemart_state_id"
		.
		" inner join " . $dbPrefix
		. "virtuemart_countries c on c.virtuemart_country_id = ui.virtuemart_country_id"
		.
		" WHERE user_is_vendor=1 " .
		" ORDER BY vmu.virtuemart_vendor_id LIMIT 1";

	$vendorquery = mysql_query($sql);
	$vendor = mysql_fetch_array($vendorquery);

	writeStartTag("Store");
	writeElement("Name", str_replace("\\", "", $vendor['name']));
	writeElement("CompanyOrOwner", str_replace("\\", "", $vendor['company']));
	writeElement("Email", $vendor['email']);
	writeElement("Street1", $vendor['address_1']);
	writeElement("Street2", $vendor['address_2']);
	writeElement("City", $vendor['city']);
	writeElement("State", $vendor['state_name']);
	writeElement("PostalCode", $vendor['zip']);
	writeElement("Country", substr($vendor['country_2_code'], 0, 2));
	writeElement("Phone", $vendor['phone_1']);
	writeElement("Website", $vendor['vendor_url']);

	writeCloseTag("Store");
}

// Get the count of orders greater than the start ID
function Action_GetCount()
{
	global $dbPrefix;
	$start = GetStart();

	// Write the params for easier diagnostics
	writeStartTag("Parameters");
	writeElement("Start", $start);
	writeCloseTag("Parameters");

	$ordersQuery = mysql_query(
		"select count(*) as count " .
		" from " . $dbPrefix . "virtuemart_orders o " .
		" where modified_on > '$start'"
		);
	$count = 0;

	if (mysql_num_rows($ordersQuery)) {
		$rows = mysql_fetch_array($ordersQuery);
		$count = $rows['count'];
	}

	writeElement("OrderCount", $count);
}

// Get all orders greater than the given start id, limited by max count
function Action_GetOrders()
{
	global $dbPrefix;
	global $secure;

	$maxcount = 50;

	$start = GetStart();

	if (isset($_POST['maxcount'])) {
		$maxcount = $_POST['maxcount'];
	}

	// Only get orders through 2 seconds ago.
	$end = toFormattedDateFromObject(time() - 2);


	// Write the params for easier diagnostics
	writeStartTag("Parameters");
	writeElement("Start", $start);
	writeElement("MaxCount", $maxcount);
	writeCloseTag("Parameters");


	writeStartTag("Orders");

	$sql = "select * " .
		" from " . $dbPrefix . "virtuemart_orders " .
		" where modified_on > '$start'  " .
		"    and modified_on <= '$end' " .
		" order by modified_on asc " .
		" limit 0, " . $maxcount;

	$ordersQuery = mysql_query($sql);


	$lastModified = null;
	$processedIds = "";

	while ($row = mysql_fetch_array($ordersQuery)) {
		// keep track of the most current processed modified time
		$lastModified = $row['modified_on'];

		// keep track of Ids we have processed
		if ($processedIds != "") {
			$processedIds .= ", ";
		}
		$processedIds .= $row['virtuemart_order_id'];

		WriteOrder($row);
	}

	// If we processed some orders, we may have to get some more
	if ($processedIds != "") {
		// This make sure we don't split a page between orders of the same modified time
		// If there's any that didn't make the maxcount cutoff with the same last modified time
		// as the greatest last modified time we already processed, this will get them
		$moreQuery = mysql_query(
			"select * " .
			"from " . $dbPrefix . "virtuemart_orders o " .
			"where virtuemart_order_id not in ($processedIds) " .
			"and modified_on = '$lastModified'"
			);

		while ($row = mysql_fetch_array($ordersQuery)) {
			WriteOrder($row);
		}
	}

	// requery for those orders with the same end
	writeCloseTag("Orders");
}

function GetStart()
{
	$start = "1900-01-01T00:00:00";
	if (isset($_POST['start'])) {
		$start = $_POST['start'];
	}
	return $start;
}

function WriteOrder($row)
{
	global $dbPrefix;

	writeStartTag("Order");
	writeElement("OrderNumber", $row['virtuemart_order_id']);
	writeElement("OrderDate", toFormattedDateFromString($row['created_on']));
	writeElement(
		"LastModified", toFormattedDateFromString($row['modified_on'])
		);
	writeElement(
		"ShippingMethod",
		GetShippingMethod($row['virtuemart_shipmentmethod_id'])
		);
	writeElement("StatusCode", ord($row['order_status']));
	writeElement("CustomerID", $row['virtuemart_user_id']);

	writeStartTag("Notes");
	writeElementWithAttributes(
		"Note", $row['customer_note'], array("public" => "true")
		);
	writeCloseTag("Notes");

	$sqlAddressWithoutType
		= "select u.*, s.state_name, c.country_name from " . $dbPrefix
		. "virtuemart_order_userinfos u" .
		" left outer join " . $dbPrefix
		. "virtuemart_states s on s.virtuemart_state_id = u.virtuemart_state_id"
		.
		" inner join " . $dbPrefix
		. "virtuemart_countries c on c.virtuemart_country_id = u.virtuemart_country_id"
		.
		" where virtuemart_order_id = " . $row['virtuemart_order_id']
		. " AND address_type=";

	$customerBTQuery = mysql_query($sqlAddressWithoutType . "'BT'");
	$customerBT = mysql_fetch_array($customerBTQuery);

	$customerSTQuery = mysql_query($sqlAddressWithoutType . "'ST'");
	$customerST = mysql_fetch_array($customerSTQuery);

	if ($customerST == null) {
		$customerST = $customerBT;
	}

	writeStartTag("ShippingAddress");
	writeElement("FirstName", $customerST['first_name']);
	writeElement("LastName", $customerST['last_name']);
	writeElement("Company", $customerST['company']);
	writeElement("Street1", $customerST['address_1']);
	writeElement("Street2", $customerST['address_2']);
	writeElement("City", $customerST['city']);
	writeElement("State", $customerST['state_name']);
	writeElement("PostalCode", $customerST['zip']);
	writeElement("Country", $customerST['country_name']);
	writeElement("Phone", $customerST['phone_1']);
	writeElement("Email", $customerST['email']);
	writeCloseTag("ShippingAddress");

	writeStartTag("BillingAddress");
	writeElement("FirstName", $customerBT['first_name']);
	writeElement("LastName", $customerBT['last_name']);
	writeElement("Company", $customerBT['company']);
	writeElement("Street1", $customerBT['address_1']);
	writeElement("Street2", $customerBT['address_2']);
	writeElement("City", $customerBT['city']);
	writeElement("State", $customerBT['state_name']);
	writeElement("PostalCode", $customerBT['zip']);
	writeElement("Country", $customerBT['country_name']);
	writeElement("Phone", $customerBT['phone_1']);
	writeElement("Email", $customerBT['email']);
	writeCloseTag("BillingAddress");

	writeStartTag("Payment");
	writeElement(
		"Method", GetPaymentMethod($row['virtuemart_paymentmethod_id'])
		);
	writeCloseTag("Payment");

	WriteOrderItems($row['virtuemart_order_id']);
	WriteOrderTotals($row);

	writeCloseTag("Order");
}

// returns the corresponding shipping name
function GetShippingMethod($shipping_method_id)
{

	global $dbPrefix;

	$sql
		= "SELECT * FROM " . $dbPrefix . "virtuemart_shipmentmethods_en_gb sm" .
		" WHERE virtuemart_shipmentmethod_id = " . $shipping_method_id;

	$q = mysql_query($sql);

	if ($q) {
		$row = mysql_fetch_array($q);
		return $row['shipment_name'];
	} else {
		return "";
	}


}

// returns the corresponding payment method row
function GetPaymentMethod($payment_method_id)
{
	global $dbPrefix;

	$sql = "SELECT * FROM " . $dbPrefix . "virtuemart_paymentmethods_en_gb pm" .
		" WHERE virtuemart_paymentmethod_id = " . $payment_method_id;

	$q = mysql_query($sql);
	if ($q) {
		$row = mysql_fetch_array($q);
		return $row['payment_name'];
	} else {
		return "";
	}
}

// Write all totals lines for the order
function WriteOrderTotals($order)
{
	writeStartTag("Totals");
	$i = 1;

	if ($order['order_subtotal'] > 0) {
		WriteTotal(
			$i++, "Sub-Total", $order['order_subtotal'], "subtotal", "none"
			);
	}

	if ($order['order_total'] > 0) {
		WriteTotal($i++, "Total", $order['order_total'], "total", "none");
	}

	if ($order['order_shipment'] > 0) {
		WriteTotal($i++, "Shipping", $order['order_shipment'], "shipping");
	}

	if ($order['order_tax'] > 0) {
		WriteTotal($i++, 'Tax', $order['order_tax'], 'tax');
	}

	if ($order['order_shipment_tax'] > 0) {
		WriteTotal(
			$i++, 'Shipping Tax', $order['order_shipment_tax'], 'shipping tax'
			);
	}

	if ($order['order_discountAmount'] != 0) {
		$title = 'Discount';
		if ($order['order_discountAmount'] > 0) {
			$title = 'Fee';
		}

		WriteTotal($i++, $title, $order['order_discountAmount'], $title, "add");
	}

	if ($order['coupon_discount'] > 0) {
		$title = "Coupon (" . $order['coupon_code'] . ")";
		WriteTotal($i++, $title, $order['coupon_discount'], $title, "subtract");
	}

	writeCloseTag("Totals");
}

// Write Order Total
function WriteTotal($totalID, $title, $value, $class, $impact = "add")
{
	writeElementWithAttributes(
		"Total", $value,
		array("name" => $title, "class" => $class, "impact" => $impact)
		);
}

// Write XML for all products for the given order
function WriteOrderItems($orderID)
{
	global $dbPrefix;
	$imageRoot = GetImageRoot();

	$itemQuery = mysql_query(
		"select * from " . $dbPrefix . "virtuemart_order_items" .
		" where virtuemart_order_id = " . $orderID
		);

	writeStartTag("Items");

	while ($item = mysql_fetch_array($itemQuery)) {
		// Get product info
		$sql = "select p.*,m.file_url_thumb" .
			" from " . $dbPrefix . "virtuemart_products p" .
			" inner join " . $dbPrefix
			. "virtuemart_product_medias pm on pm.virtuemart_product_id = p.virtuemart_product_id"
			.
			" inner join " . $dbPrefix
			. "virtuemart_medias m on m.virtuemart_media_id = pm.virtuemart_media_id"
			.
			" where p.virtuemart_product_id = "
			. $item['virtuemart_product_id'];
		$productQuery = mysql_query($sql);

		$product = mysql_fetch_array($productQuery);

		// Build fully qualified image url
		$imageUrl = $product['file_url_thumb'];
		if (isset($imageUrl) and strlen($imageUrl) > 0) {
			$imageUrl = $imageRoot . $imageUrl;
		}

		writeStartTag("Item");
		writeElement("ItemID", $item['virtuemart_order_item_id']);
		writeElement("ProductID", $item['virtuemart_product_id']);
		writeElement("Code", $item['order_item_sku']);
		writeElement("SKU", $item['order_item_sku']);
		writeElement("Name", $item['order_item_name']);
		writeElement("Quantity", $item['product_quantity']);
		writeElement("UnitPrice", $item['product_item_price']);
		writeElement("Image", $imageUrl);

		// assuming everything is in pounds (the default)
		$weightFactor = 1;
		$uom = $product['product_weight_uom'];
		if (preg_match('/^(oz|ounce)/i', $uom)) {
			$weightFactor = 1 / 16;
		}

		writeElement("Weight", $product['product_weight'] * $weightFactor);

		// Write attributes
		WriteItemAttributes(
			$item['virtuemart_product_id'], $item['product_attribute']
			);

		writeCloseTag("Item");
	}

	writeCloseTag("Items");
}

// Write all attributes for the item
function WriteItemAttributes($productID, $attributeColumn)
{
	writeStartTag("Attributes");

	if (strlen($attributeColumn) > 0) {
		$attributeDefinitions = json_decode($attributeColumn, true);
		foreach ($attributeDefinitions as $customfield_id => $valueHtml) {
			$xml = new SimpleXMLElement("<root>" . $valueHtml . "</root>");

			// we're just going to output each attribute without a price because the price in the
			// database includes various taxes, while the subtotal does not.  This would cause
			// shipworks to incorrectly display values

			$nameElement = $xml->xpath("/root/span[@class='costumTitle']");
			$valueElement = $xml->xpath("/root/span[@class='costumValue']");

			$name = (string)$nameElement[0];
			$value = (string)$valueElement[0];

			$name = trim($name);
			$value = trim($value);
			$price = 0;

			writeStartTag("Attribute");
			writeElement("Name", $name);
			writeElement("Value", $value);
			writeElement("Price", $price);
			writeCloseTag("Attribute");
		}
	}

	writeCloseTag("Attributes");
}

// Return the collection of valid status codes
function Action_GetStatusCodes()
{
	global $dbPrefix;
	writeStartTag("StatusCodes");

	$codesQuery = mysql_query(
		"SELECT * FROM " . $dbPrefix . "virtuemart_orderstates"
		);

	while ($row = mysql_fetch_array($codesQuery)) {
		writeStartTag("StatusCode");
		writeElement("Code", ord($row['order_status_code']));
		writeElement("Name", $row['order_status_name']);
		writeCloseTag("StatusCode");
	}

	writeCloseTag("StatusCodes");
}

// Update order status
function Action_UpdateStatus()
{
	global $dbPrefix;

	if (!isset($_POST['order']) || !isset($_POST['status'])
		|| !isset($_POST['comments'])
	) {
		outputError(40, "Insufficient parameters");
		return;
	}

	$orderID = (int)$_POST['order'];
	$code = chr($_POST['status']);

	$comments = mysql_escape_string($_POST['comments']);


	mysql_query(
		"insert into " . $dbPrefix . "virtuemart_order_histories " .
		" (virtuemart_order_id, order_status_code, created_on, customer_notified, comments) "
		.
		" values (" . $orderID . ", '$code' , now(), 0, '" . $comments . "')"
		);

	mysql_query(
		"update " . $dbPrefix . "virtuemart_orders " .
		" set order_status = '$code' " .
		" where virtuemart_order_id = " . $orderID
		);

	echo "<UpdateSuccess/>";
}

// Convert the date string to xml date string
function toFormattedDateFromString($sqlDateString)
{
	$sqlDate = date("Y-m-d\TH:i:s", strtotime($sqlDateString));

	return $sqlDate;
}

// Convert the date to string
function toFormattedDateFromObject($sqlDateString)
{
	return gmdate("Y-m-d\TH:i:s", $sqlDateString);
}

//virtue mart functions
function db_connect()
{
	global $conn;

	if (class_exists("JConfig")) {
		$CONFIG = new JConfig();
		$conn = mysql_connect($CONFIG->host, $CONFIG->user, $CONFIG->password);
		return mysql_select_db($CONFIG->db);
	} else {
		// Joomla 1.0
		global $mosConfig_host;
		global $mosConfig_user;
		global $mosConfig_password;
		global $mosConfig_db;

		$conn = mysql_connect(
			$mosConfig_host, $mosConfig_user, $mosConfig_password
			);
		return mysql_select_db($mosConfig_db);
	}
}

function not_null($value)
{
	if (is_array($value)) {
		if (sizeof($value) > 0) {
			return true;
		} else {
			return false;
		}
	} else {
		if ((is_string($value) || is_int($value)) && ($value != '')
			&& ($value != 'NULL')
			&& (strlen(trim($value)) > 0)
		) {
			return true;
		} else {
			return false;
		}
	}
}

// encrypts a string using the encryption method that virtuemart does
function getCryptedPassword($plaintext, $salt = '', $encryption = 'md5-hex')
{
	$encrypted = ($salt) ? md5($plaintext . $salt) : md5($plaintext);
	return $encrypted;
}

// converts a date string in utc format to one in local time, sql format
function toLocalSqlDate($sqlUtc)
{
	$pattern = "/^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2})$/i";

	if (preg_match($pattern, $sqlUtc, $dt)) {
		$unixUtc = gmmktime($dt[4], $dt[5], $dt[6], $dt[2], $dt[3], $dt[1]);

		return date("Y-m-d H:i:s", $unixUtc);
	}

	return $sqlUtc;
}

?>