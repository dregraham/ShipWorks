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
	define( '_JEXEC', true );

	$moduleVersion = "3.0.2.0";
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
	function writeElement($tag, $value)
	{
		writeStartTag($tag);
		echo toUtf8(htmlspecialchars($value));
		writeCloseTag($tag);
	}

	// Outputs an xml element with the provided attributes
	function writeElementWithAttributes($tag, $value, $attributes)
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

	// end basic XML helper functions

	//virtue mart setup     
	require('configuration.php');
	require('administrator/components/com_virtuemart/virtuemart.cfg.php');

	if( @VM_ENCRYPT_FUNCTION == 'AES_ENCRYPT') {
		define('VM_DECRYPT_FUNCTION', 'AES_DECRYPT');
	} else {
		define('VM_DECRYPT_FUNCTION', 'DECODE');
	}

	//are we on an SSL connection?
	$secure = ($_SERVER['HTTPS'] == 'on' || $_SERVER['HTTPS'] == '1');

	$dbPrefix = 'jos_';

	if (class_exists("JConfig"))
	{
		$CONFIG = new JConfig();
		if ($CONFIG->dbprefix)
		{
			$dbPrefix = $CONFIG->dbprefix;
		}
		$conn = mysql_connect($CONFIG->host, $CONFIG->user, $CONFIG->password);
	}

	// Open the XML output and root
	writeXmlDeclaration();
	writeStartTag("ShipWorks", array("moduleVersion" => $moduleVersion, "schemaVersion" => $schemaVersion));

	// Enforce SSL
	if (!$secure && REQUIRE_SECURE)
	{
		outputError(10, 'A secure (https://) connection is required.');
	}

	// Connect to database
	else if (!db_connect())
	{
		outputError(20, 'The ShipWorks module was unable to connect to database.');
	}
	// Proceed
	else
	{
		// verify that the credentials are valid
		if (checkAdminLogin())
		{
			$action = (isset($_POST['action']) ? $_POST['action'] : '');
			if (not_null($action)) 
			{
				switch (strtolower($action)) 
				{
					case 'getmodule': Action_GetModule(); break;
					case 'getstore': Action_GetStore(); break;
					case 'getcount': Action_GetCount(); break;
					case 'getorders': Action_GetOrders(); break;
					case 'getstatuscodes': Action_GetStatusCodes(); break;
					case 'updatestatus': Action_UpdateStatus(); break;
					case 'debug': Action_Debug(); break;
					default: outputError(70, "'$action' not supported");
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
		if (strtoupper($serverName) == "LOCALHOST" || $serverName == "127.0.0.1")
		{
			// default to the http request host used to access the server
			$serverName = $hostName;
		}

		preg_match('/(.*)\//', $scriptPath, $matches);
		$cartRoot = "$serverName$matches[1]";

		return "http://$cartRoot/components/com_virtuemart/shop_image/product/";
	}

	// Debugging 
	function Action_Debug()
	{
		$imageRoot = GetImageRoot();

		print "ENCODE_KEY = ". ENCODE_KEY;
		print "VM_ENCRYPT_FUNCTION = ". @VM_ENCRYPT_FUNCTION;
		print "VM_DECRYPT_FUNCTION = ". VM_DECRYPT_FUNCTION;
		print "Image Root = $imageRoot";
	}

	// Check to see if admin functions exist.  And if so, determine if the user
	// has access.
	function checkAdminLogin()
	{
		global $dbPrefix;	
		$loginOK = false;

		if (isset($_POST['username']) && isset($_POST['password']))
		{
			$username = $_POST['username'];
			$password = $_POST['password'];

			$sql = "SELECT id, password, gid"
				. " FROM ". $dbPrefix. "users"
				. " WHERE username='" .mysql_real_escape_string($username). "'";
			$check_adminquery = mysql_query($sql);
			if (mysql_num_rows($check_adminquery)) 
			{
				$check_admin = mysql_fetch_array($check_adminquery);

				// Check that password is good
				$parts  = explode( ':', $check_admin['password'] );
				$crypt  = $parts[0];
				$salt   = @$parts[1];
				$testcrypt = getCryptedPassword($password, $salt);

				if ($crypt == $testcrypt) {
					$loginOK = true;
				}
			}

			if (!$loginOK)
			{
				outputError(50, "The username or password is incorrect.");
			}
		}
		else
		{
			outputError(60, "The username and password was not supplied.");
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
		writeElementWithAttributes("OnlineCustomerID", "", array("supported" => "true"));
		writeElementWithAttributes("OnlineStatus", "", array("supported" => "true", "supportsComments" => "true"));
		writeElementWithAttributes("OnlineShipmentUpdate", "", array("supported" => "false"));
		writeCloseTag("Capabilities");

		writeCloseTag("Module");			
	}

	// Write store data
	function Action_GetStore()
	{
		global $dbPrefix;
		$sql = "SELECT * FROM ". $dbPrefix. "vm_vendor ORDER BY vendor_id LIMIT 1";
		$vendorquery = mysql_query($sql);
		$vendor = mysql_fetch_array($vendorquery);
		writeStartTag("Store");
		writeElement("Name", str_replace("\\", "", $vendor['vendor_store_name']));
		writeElement("CompanyOrOwner", str_replace("\\", "", $vendor['vendor_name']));
		writeElement("Email", $vendor['contact_email']);
		writeElement("Street1", $vendor['vendor_address_1']);
		writeElement("Street2", $vendor['vendor_address_2']);
		writeElement("City", $vendor['vendor_city']);
		writeElement("State", $vendor['vendor_state']);
		writeElement("PostalCode", $vendor['vendor_zip']);
		writeElement("Country", substr($vendor['vendor_country'], 0, 2));
		writeElement("Phone", $vendor['contact_phone_1']);
		writeElement("Website", $vendor['vendor_url']);

		writeCloseTag("Store");
	}

	// Get the count of orders greater than the start ID
	function Action_GetCount()
	{         
		global $dbPrefix;
		$start = "1900-01-01T00:00:00";

		if (isset($_POST['start']))
		{
			$start = toLocalSqlDate($_POST['start']);
		}

		// Write the params for easier diagnostics
		writeStartTag("Parameters");
		writeElement("Start", $start);
		writeCloseTag("Parameters");

		// convert to unix date format
		$start = strtotime($start);

		$ordersQuery = mysql_query(
				"select count(*) as count " .
				" from ". $dbPrefix. "vm_orders o " .
				" where mdate > $start"
				);

		$count = 0;

		if (mysql_num_rows($ordersQuery)) 
		{
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

		$start = "1900-01-01T00:00:00";
		$maxcount = 50;

		if (isset($_POST['start']))
		{
			$start = toLocalSqlDate($_POST['start']);
		}

		if (isset($_POST['maxcount']))
		{
			$maxcount = $_POST['maxcount'];
		}

		// Only get orders through 2 seconds ago.
		$end = time() - 2;

		// Write the params for easier diagnostics
		writeStartTag("Parameters");
		writeElement("Start", $start);
		writeElement("MaxCount", $maxcount);
		writeCloseTag("Parameters");

		// convert to unix date format
		$start = strtotime($start);

		writeStartTag("Orders");

		$ordersQuery = mysql_query(
				"select * " .
				" from ". $dbPrefix. "vm_orders " .
				" where mdate > $start " .
				"    and mdate <= $end " .
				" order by mdate asc " .
				" limit 0, " . $maxcount);

		$lastModified = null;
		$processedIds = "";

		while ($row = mysql_fetch_array($ordersQuery)) 
		{
			// keep track of the most current processed modified time
			$lastModified = $row['mdate'];

			// keep track of Ids we have processed
			if ($processedIds != "")
			{
				$processedIds .= ", ";
			}
			$processedIds .= $row['order_id'];

			WriteOrder($row);
		}

		// If we processed some orders, we may have to get some more
		if ($processedIds != "")
		{
			// This make sure we don't split a page between orders of the same modified time
			// If there's any that didn't make the maxcount cutoff with the same last modified time
			// as the greatest last modified time we already processed, this will get them
			$moreQuery = mysql_query(
					"select * ".
					"from ". $dbPrefix. "vm_orders o ".
					"where order_id not in ($processedIds) ". 
					"and mdate = '$lastModified'");

			while ($row = mysql_fetch_array($ordersQuery))
			{
				WriteOrder($row);
			}
		}

		// requery for those orders with the same end
		writeCloseTag("Orders");
	}

	function WriteOrder($row)
	{
		global $dbPrefix;
		$shipping_method_options = split('\|', $row['ship_method_id']);
		if (count($shipping_method_options) > 2)
		{
			$shipping_method = $shipping_method_options[1]. " ". $shipping_method_options[2];
		}
		else
		{
			$shipping_method = $shipping_method_options[0];
		}

		writeStartTag("Order");

		writeElement("OrderNumber", $row['order_id']);
		writeElement("OrderDate", toGmt($row['cdate']));
		writeElement("LastModified", toGmt($row['mdate']));
		writeElement("ShippingMethod", $shipping_method);
		writeElement("StatusCode", ord($row['order_status']));
		writeElement("CustomerID", $row['user_id']);

		writeStartTag("Notes");
		writeElementWithAttributes("Note", $row['customer_note'], array("public" => "true"));
		writeCloseTag("Notes");

		$customerBTQuery = mysql_query("select * from ". $dbPrefix. "vm_order_user_info where order_id = " . $row['order_id'] . " AND address_type='BT'");
		$customerBT = mysql_fetch_array($customerBTQuery);

		$customerSTQuery = mysql_query("select * from ". $dbPrefix. "vm_order_user_info where order_id = " . $row['order_id'] . " AND address_type='ST'");
		$customerST = mysql_fetch_array($customerSTQuery);
		if ($customerST == null) $customerST = $customerBT;

		writeStartTag("ShippingAddress");
		writeElement("FirstName", $customerST['first_name']);
		writeElement("LastName", $customerST['last_name']);
		writeElement("Company", $customerST['company']);
		writeElement("Street1", $customerST['address_1']);
		writeElement("Street2", $customerST['address_2']);
		writeElement("City", $customerST['city']);
		writeElement("State", $customerST['state']);
		writeElement("PostalCode", $customerST['zip']);
		writeElement("Country", GetCountryName($customerST['country']));
		writeElement("Phone", $customerST['phone_1']);
		writeElement("Email", $customerST['user_email']);
		writeCloseTag("ShippingAddress");

		writeStartTag("BillingAddress");
		writeElement("FirstName", $customerBT['first_name']);
		writeElement("LastName", $customerBT['last_name']);
		writeElement("Company", $customerBT['company']);
		writeElement("Street1", $customerBT['address_1']);
		writeElement("Street2", $customerBT['address_2']);
		writeElement("City", $customerBT['city']);
		writeElement("State", $customerBT['state']);
		writeElement("PostalCode", $customerBT['zip']);
		writeElement("Country", GetCountryName($customerBT['country']));
		writeElement("Phone", $customerBT['phone_1']);
		writeElement("Email", $customerBT['user_email']);
		writeCloseTag("BillingAddress");

		$paymentQuery = mysql_query("SELECT payment_method_id, ". VM_DECRYPT_FUNCTION."(order_payment_number,'".ENCODE_KEY."')
				AS account_number, order_payment_code, order_payment_expire, order_payment_name, 
				order_payment_log FROM ". $dbPrefix. "vm_order_payment WHERE order_id = '".$row['order_id']."'");
		$payment = mysql_fetch_array($paymentQuery);
		$paymentmethod = GetPaymentMethod($payment['payment_method_id']);

		writeStartTag("Payment");
		writeElement("Method", $paymentmethod['payment_method_name']);

		$isCC = $paymentmethod['is_creditcard'];
		if ($isCC)
		{
			$acct = $payment['account_number'];

			if (preg_match("/4.*/", $acct))
			{
				$ccType = 'VISA';
			}
			else if (preg_match("/34.*/", $acct) || preg_match("/37.*/", $acct))
			{
				$ccType = "AMERICAN EXPRESS";
			}
			else if (preg_match("/51.*/", $acct) || preg_match("/55.*/", $acct))
			{
				$ccType = "MASTERCARD";
			}
			else if (preg_match("/6011.*/", $acct))
			{
				$ccType = "DISCOVER"; 
			} 

			writeStartTag("CreditCard");
			writeElement("Type", $ccType);
			writeElement("Owner", $payment['order_payment_name']);

			if ($secure)
			{
				writeElement("Number", $acct);
			}
			else
			{
				writeElement("Number", '********');
			}

			writeElement("Expires", $payment['order_payment_expire']);
			writeCloseTag("CreditCard");
		}

		writeCloseTag("Payment");

		WriteOrderItems($row['order_id']);
		WriteOrderTotals($row);

		writeCloseTag("Order");
	}

	// returns the corresponding payment method row
	function GetPaymentMethod($payment_method_id)
	{
		global $dbPrefix;

		$q = mysql_query("SELECT * FROM ". $dbPrefix. "vm_payment_method WHERE payment_method_id = ". $payment_method_id);
		if ($q)
		{
			return mysql_fetch_array($q);
		}
		else
		{
			return array("payment_method_name" => "unkown", "is_creditcard" => false);
		}
	}

	// Write all totals lines for the order
	function WriteOrderTotals($order)
	{
		writeStartTag("Totals");
		$i = 1;

		if ($order['order_subtotal'] > 0)
		{
			WriteTotal($i++, "Sub-Total", $order['order_subtotal'], "subtotal", "none");
		}

		if ($order['order_total'] > 0)
		{
			WriteTotal($i++, "Total", $order['order_total'], "total", "none");
		}

		if ($order['order_shipping'] > 0)
		{
			WriteTotal($i++, "Shipping", $order['order_shipping'], "shipping");
		}

		if ($order['order_tax'] > 0)
		{
			WriteTotal($i++, 'Tax', $order['order_tax'], 'tax');
		}

		if ($order['order_shipping_tax'] > 0)
		{
			WriteTotal($i++, 'Shipping Tax', $order['order_shipping_tax'], 'shipping tax');
		}

		if ($order['order_discount'] != 0)
		{
			$title = 'Discount';
			if ($order['order_discount'] < 0)
			{
				$title = 'Fee';
			}

			WriteTotal($i++, $title, $order['order_discount'], $title, "subtract");
		}

		if ($order['coupon_discount'] > 0)
		{
			$title = "Coupon (". $order['coupon_code']. ")";
			WriteTotal($i++, $title, $order['coupon_discount'], $title, "subtract");
		}

		writeCloseTag("Totals");
	}

	// Write Order Total
	function WriteTotal($totalID, $title, $value, $class, $impact = "add")
	{
		writeElementWithAttributes("Total", $value, array( "name" => $title, "class" => $class, "impact" => $impact));
	}

	// Write XML for all products for the given order
	function WriteOrderItems($orderID)
	{
		global $dbPrefix;
		$imageRoot = GetImageRoot();

		$itemQuery = mysql_query(
				"select * from ". $dbPrefix. "vm_order_item" .
				" where order_id = " . $orderID);

		writeStartTag("Items");

		while($item = mysql_fetch_array($itemQuery))
		{
			// Get product info
			$productQuery = mysql_query(
					"select * from ". $dbPrefix. "vm_product" .
					" where product_id = " . $item['product_id']);

			$product = mysql_fetch_array($productQuery);

			// Build fully qualified image url
			$imageUrl = $product['product_thumb_image'];
			if (isset($imageUrl) and strlen($imageUrl) > 0)
			{
				$imageUrl = $imageRoot . $imageUrl;
			}

			writeStartTag("Item");
			writeElement("ItemID", $item['order_item_id']);
			writeElement("ProductID", $item['product_id']);
			writeElement("Code", $item['order_item_sku']);
			writeElement("SKU", $item['order_item_sku']);
			writeElement("Name", $item['order_item_name']);
			writeElement("Quantity", $item['product_quantity']);
			writeElement("UnitPrice", $item['product_item_price']);
			writeElement("Image", $imageUrl);

			// assuming everything is in pounds (the default)
			$weightFactor = 1;
			$uom = $product['product_weight_uom'];
			if (preg_match('/^(oz|ounce)/i', $uom))
			{
				$weightFactor = 1/16;
			}

            writeElement("Weight", $product['product_weight'] * $weightFactor);

			// Write attributes
			WriteItemAttributes($item['product_id'], $item['product_attribute']);

			writeCloseTag("Item");
		}

		writeCloseTag("Items");
	}

	// Attempts to locate a product attribute id in the database, returning a magic 999999 if it isn't
	function GetAttributeID($productID, $attribute_name, $attribute_value)
	{
		global $dbPrefix;
		
		$query = mysql_query("SELECT * FROM ". $dbPrefix. "vm_product_attribute WHERE ". 
				"product_id = $productID AND ".
				"attribute_name = '". mysql_real_escape_string($attribute_name). "' AND ".
				"attribute_value = '". mysql_real_escape_string($attribute_value). "'");
		if ($row = mysql_fetch_array($query))
		{
			return $row['attribute_id'];
		}
		else
		{
			return 999999;
		}
	}

	// Write all attributes for the item
	function WriteItemAttributes($productID, $attributeColumn)
	{
		writeStartTag("Attributes");

		if (strlen($attributeColumn) > 0)
		{
			$attributeDefinitions = split('<br/>', $attributeColumn);
			foreach ($attributeDefinitions as $attributeString)
			{
				// we're just going to output each attribute without a price because the price in the 
				// database includes various taxes, while the subtotal does not.  This would cause
				// shipworks to incorrectly display values
				list($name, $value) = split(':', $attributeString, 2);

				$name = trim($name);
				$value = trim($value);
				$price = 0;

				writeStartTag("Attribute");
				writeElement("AttributeID", GetAttributeID($productID, $name, $value));
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

		$codesQuery = mysql_query("SELECT * FROM ". $dbPrefix. "vm_order_status");

		while ($row = mysql_fetch_array($codesQuery))
		{
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
		if (!isset($_POST['order']) || !isset($_POST['status']) || !isset($_POST['comments']))
		{
			outputError(40, "Not all parameters supplied.");
			return;
		}

		$orderID = (int) $_POST['order'];
		$code = chr($_POST['status']);

		$comments = mysql_escape_string($_POST['comments']);

		mysql_query( 
				"insert into ". $dbPrefix. "vm_order_history ". 
				" (order_id, order_status_code, date_added, customer_notified, comments) " .
				" values (" . $orderID . ", '$code' , now(), 0, '" . $comments . "')");

		mysql_query(
				"update ". $dbPrefix. "vm_orders ".
				" set order_status = '$code' " . 
				" where order_id = " . $orderID);

		echo "<UpdateSuccess/>";	
	}

	function GetCountryName($code)
	{
		global $dbPrefix;
		$qry = mysql_query("SELECT country_name FROM ". $dbPrefix. "vm_country WHERE country_3_code = '$code' OR country_2_code = '$code'");
		$row = mysql_fetch_array($qry);
		if ($row)
		{       
			return $row["country_name"];
		}
		else 
		{
			return $code;           
		}
	}

	// Convert the date to xml date string
	function toGmt($dateUnix)
	{
		return gmdate("Y-m-d\TH:i:s", $dateUnix);
	}

	//virtue mart functions
	function db_connect()
	{
		global $conn;

		if (class_exists("JConfig"))
		{
			$CONFIG = new JConfig();
			$conn = mysql_connect($CONFIG->host, $CONFIG->user, $CONFIG->password);
			return mysql_select_db($CONFIG->db);
		}
		else
		{
			// Joomla 1.0
			global $mosConfig_host;
			global $mosConfig_user;
			global $mosConfig_password;
			global $mosConfig_db;

			$conn = mysql_connect($mosConfig_host, $mosConfig_user, $mosConfig_password);
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
			if ( (is_string($value) || is_int($value)) && ($value != '') && ($value != 'NULL') && (strlen(trim($value)) > 0)) {
				return true;
			} else {
				return false;
			}
		}
	}

	// encrypts a string using the encryption method that virtuemart does
	function getCryptedPassword($plaintext, $salt = '', $encryption = 'md5-hex')
	{
		$encrypted = ($salt) ? md5($plaintext.$salt) : md5($plaintext);
		return $encrypted;
	}

	// converts a date string in utc format to one in local time, sql format
	function toLocalSqlDate($sqlUtc)
	{   
		$pattern = "/^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2})$/i";

		if (preg_match($pattern, $sqlUtc, $dt)) 
		{
			$unixUtc = gmmktime($dt[4], $dt[5], $dt[6], $dt[2], $dt[3], $dt[1]);  

			return date("Y-m-d H:i:s", $unixUtc);
		}

		return $sqlUtc;
	}


?>
