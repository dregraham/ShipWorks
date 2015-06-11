<?php

	define('REQUIRE_SECURE', true);

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
	| Copyright 2005-2012 Interapptive, Inc.  All rights reserved.
	| http://www.interapptive.com/
	|
	|
	*/
	
	$moduleVersion = "3.0.1.2";
	$schemaVersion = "1.0.0";
	
	header("Last-Modified: " . gmdate("D, d M Y H:i:s") . " GMT");
	
	// HTTP/1.1
	header("Content-Type: text/xml;charset=utf-8");
	header("Cache-Control: no-store, no-cache, must-revalidate");
	header("Cache-Control: post-check=0, pre-check=0", false);

	// HTTP/1.0
	header("Pragma: no-cache");	

	function toUtf8($string)
	{
		return iconv("ISO-8859-1", "UTF-8//TRANSLIT", $string);
	}
	
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
			
	require('includes/configure.php');
	require(DIR_WS_INCLUDES . 'database_tables.php');
	require(DIR_WS_CLASSES . 'order.php');
	require(DIR_WS_FUNCTIONS . 'database.php');
	require(DIR_WS_FUNCTIONS . 'general.php');
	
	$secure = (getenv('HTTPS') == 'on');
		
	// Open the XML output and root
	writeXmlDeclaration();
	writeStartTag("ShipWorks", array("moduleVersion" => $moduleVersion, "schemaVersion" => $schemaVersion));
	
	// Enforse SSL
	if (!$secure && REQUIRE_SECURE)
	{
		outputError(10, "Invalid URL, HTTPS is required");
	}
	
	// Connect to database
	else if (!tep_db_connect())
	{
		outputError(70, "The ShipWorks module was unable to connect to database");
	}
	// Check we are in the admin folder
	else if (!function_exists("tep_get_orders_status"))
	{
	    outputError(1000, 
	        "The ShipWorks module could not find certain required osCommerce functions, " .
			"which could be caused if the module is not installed in the 'admin' directory of your store");
	}
	// Proceed
	else
	{
		// Define all config values
		$configuration_query = tep_db_query('select configuration_key as cfgKey, configuration_value as cfgValue from ' . TABLE_CONFIGURATION);
		while ($configuration = tep_db_fetch_array($configuration_query)) 
		{
			define($configuration['cfgKey'], $configuration['cfgValue']);
		}
		
		// Prepare language
		include(DIR_WS_CLASSES . 'language.php');
		$lng = new language();
		$lng->get_browser_language();
		$language = $lng->language['directory'];
		$languages_id = $lng->language['id'];
		
		// If the admin module is installed, we make use of it
		if (checkAdminLogin())
		{
		    $action = (isset($_REQUEST['action']) ? $_REQUEST['action'] : '');
		    if (tep_not_null($action)) 
		    {
			    switch (strtolower($action)) 
			    {
					case 'getmodule': Action_GetModule(); break;
				    case 'getstore': Action_GetStore(); break;
				    case 'getcount': Action_GetCount(); break;
				    case 'getorders': Action_GetOrders(); break;
				    case 'getstatuscodes': Action_GetStatusCodes(); break;
				    case 'updatestatus': Action_UpdateStatus(); break;
				    default: outputError(20, "Invalid action '$action'");
			    }
		    }
		}
	}
	
	// Close the output
	writeCloseTag("ShipWorks");
	
	// Check to see if admin functions exist.  And if so, determine if the user
	// has access.
	function checkAdminLogin()
	{		
	    if (function_exists("tep_admin_check_login"))
	    {	        
	        // If the admin_check function exists, the password_funcs should be available
	        // in the admin area.
	        require_once(DIR_WS_FUNCTIONS . 'password_funcs.php');
	    
	        $loginOK = false;
	        
	        if (isset($_REQUEST['username']) && isset($_REQUEST['password']))
	        {
                $email_address = tep_db_prepare_input($_REQUEST['username']);
                $password = tep_db_prepare_input($_REQUEST['password']);
    	            	        
                $check_admin_query = tep_db_query(
                    "select admin_id as login_id, admin_password as login_password " . 
                    " from " . TABLE_ADMIN . 
                    " where admin_email_address = '" . tep_db_input($email_address) . "'");
                                    
                if (tep_db_num_rows($check_admin_query)) 
                {
                    $check_admin = tep_db_fetch_array($check_admin_query);
                                        
                    // Check that password is good
                    if (tep_validate_password($password, $check_admin['login_password'])) 
                    {
                        $loginOK = true;
                    }
                }
                
                if (!$loginOK)
                {
                    outputError(50, "Username or password is incorrect");
                }
            }
            else
            {
                outputError(30, "Username or password was not supplied");
            }

	        return $loginOK;
	    }
	    else
	    {
	        return true;
	    }
	}
	
	// Get module data
	function Action_GetModule()
	{
		writeStartTag("Module");
		
			writeElement("Platform", "osCommerce");
			writeElement("Developer", "Interapptive, Inc. (support@interapptive.com)");
			
			writeStartTag("Capabilities");
				writeElement("DownloadStrategy", "ByModifiedTime");
				writeFullElement("OnlineCustomerID", "", array("supported" => "true", "dataType" => "numeric"));
				writeFullElement("OnlineStatus", "", array("supported" => "true", "dataType" => "numeric", "supportsComments" => "true" ));
				writeFullElement("OnlineShipmentUpdate", "", array("supported" => "false"));
			writeCloseTag("Capabilities");
			
	   writeCloseTag("Module");			
	}
	
	// Write store data
	function Action_GetStore()
	{
		writeStartTag("Store");
			writeElement("Name", STORE_NAME);
			writeElement("CompanyOrOwner", STORE_OWNER);
			writeElement("Email", STORE_OWNER_EMAIL_ADDRESS);
			writeElement("State", tep_get_zone_code(STORE_COUNTRY, STORE_ZONE, "AL"));
			writeElement("Country", GetCountryCode(STORE_COUNTRY));
			writeElement("Website", HTTP_CATALOG_SERVER);
		writeCloseTag("Store");
	}
	
	// Get the count of orders greater than the start ID
	function Action_GetCount()
	{	  		
		$start = '1900-01-01 00:00:00';

		if (isset($_REQUEST['start']))
		{
			$start = $_REQUEST['start'];
		}
		
		// Convert to local SQL time
		$start = toLocalSqlDate($start);
		
		// Write the params for easier diagnostics
		writeStartTag("Parameters");
		    writeElement("Start", $start);
		writeCloseTag("Parameters");
		
		$ordersQuery = tep_db_query(
			"select count(*) as count " .
			" from " . TABLE_ORDERS . " o, " . TABLE_ORDERS_TOTAL . " ot " .
			" where ot.orders_id = o.orders_id " .
			"   and ot.class = 'ot_total' " . 
			"   and ( " .
			"          (last_modified IS NOT NULL and last_modified > '$start') " .
			"       or (last_modified IS NULL and date_purchased > '$start') " .
			"       )" 
			);

		$count = 0;
		
		if (tep_db_num_rows($ordersQuery)) 
		{
			$rows = tep_db_fetch_array($ordersQuery);
			$count = $rows['count'];
		}

		writeElement("OrderCount", $count);
	}

	// Get all orders greater than the given start id, limited by max count
	function Action_GetOrders()
	{
		global $secure;
		
		$start = '1900-01-01 00:00:00';
		$maxcount = 50;

		if (isset($_REQUEST['start']))
		{
			$start = $_REQUEST['start'];
		}
		
	    // Write the params for easier diagnostics
	    writeStartTag("Parameters");
        writeElement("StartGMT", $start);
        
		// Convert to local SQL time
		$start = toLocalSqlDate($start);

		if (isset($_REQUEST['maxcount']))
		{
			$maxcount = $_REQUEST['maxcount'];
		}
		
		// Only get orders through 2 seconds ago.
		$end = date("Y-m-d H:i:s", time() - 2);
		
	    writeElement("StartLocal", $start);
	    writeElement("End", $end);
	    writeElement("MaxCount", $maxcount);
		writeCloseTag("Parameters");
						
		writeStartTag("Orders");
		
		$ordersQuery = tep_db_query(
			"select *, " .
			"            CASE " .
			"               WHEN last_modified IS NOT NULL THEN last_modified " .
			"               ELSE date_purchased " .
			"            END as Modified" .
			" from " . TABLE_ORDERS . " o, " . TABLE_ORDERS_TOTAL . " ot " .
			" where ot.orders_id = o.orders_id " .
			"   and ot.class = 'ot_total' " . 
			" having Modified > '$start' " .
			"    and Modified <= '$end' " .
			" order by Modified asc " .
			" limit 0, " . $maxcount);
		
		$lastModified = null;
		$processedIds = "";
			
	    while ($row = tep_db_fetch_array($ordersQuery)) 
	    {
	        // Save the most current processed mod time
	        $lastModified = $row['Modified'];
	        
	        // Add the id to the list we have processed
	        if ($processedIds != "")
	        {
	            $processedIds .= ", ";
	        }
	        $processedIds .= $row['orders_id'];
	        
	        WriteOrder($row);
	    }
	    
	    // If we processed some orders, we may have to get some more
	    if ($processedIds != "")
	    {
	        // This makes sure we don't split a page between orders of the same modified time.
	        // If there were any that didnt make the maxcount cutoff with the same last modified time
	        // as the greatest last modified time we already processed, this will get them.
	        $moreQuery = tep_db_query(
			    "select *, " .
			    "            CASE " .
			    "               WHEN last_modified IS NOT NULL THEN last_modified " .
			    "               ELSE date_purchased " .
			    "            END as Modified" .
			    " from " . TABLE_ORDERS . " o, " . TABLE_ORDERS_TOTAL . " ot " .
			    " where ot.orders_id = o.orders_id " .
			    "   and ot.class = 'ot_total' " . 
			    "   and o.orders_id not in ($processedIds) " .
			    " having Modified = '$lastModified' ");
			    
		    while ($row = tep_db_fetch_array($moreQuery)) 
	        {
	            WriteOrder($row);
	        }
	    }
	    
	    writeCloseTag("Orders");
	}
	
	function WriteNote($noteText, $dateAdded, $public)
	{
		if (strlen($noteText) > 0)
		{
			$attributes = array("public" => $public ? "true" : "false",
								"date" => toGmt($dateAdded));
		
			writeFullElement("Note", $noteText, $attributes);
		}
	}
	
	function WriteOrder($row)
	{       
		global $secure;

		$currencyFactor = (int) $row['currency_value'];
		
		$shipping_method_query = tep_db_query("select title from " . TABLE_ORDERS_TOTAL . " where orders_id = '" . (int)$row['orders_id'] . "' and class = 'ot_shipping'");
		$shipping_method = tep_db_fetch_array($shipping_method_query);
		$shipping_method = ((substr($shipping_method['title'], -1) == ':') ? substr(strip_tags($shipping_method['title']), 0, -1) : strip_tags($shipping_method['title']));
		
		// The customer comment will be the first history item
		$commentQuery = tep_db_query(
			"select comments, date_added from " . TABLE_ORDERS_STATUS_HISTORY .
			" where orders_id = " .  $row['orders_id'] . " " .
			" order by date_added asc " .
			" limit 0, 1" );
		$commentRow = tep_db_fetch_array($commentQuery);
			
		writeStartTag("Order");
		
			writeElement("OrderNumber", $row['orders_id']);
			writeElement("OrderDate", toGmt($row['date_purchased']));
			writeElement("LastModified", toGmt($row['Modified']));
			writeElement("ShippingMethod", $shipping_method);
			writeElement("StatusCode", $row['orders_status']);
			
			// See if the customer actually exists
			$customerExistQuery = tep_db_query("select * from " . TABLE_CUSTOMERS . " where customers_id = " . $row['customers_id']);
			$customerExists = null != tep_db_fetch_array($customerExistQuery);
			writeElement("CustomerID", $customerExists ? $row['customers_id'] : "-1");
			
			writeStartTag("Notes");
				WriteNote($commentRow['comments'], $commentRow['date_added'], "true");
			writeCloseTag("Notes");
			
	        $billEmail = $row['customers_email_address'];
            $billPhone = $row['customers_telephone'];
            $shipEmail = "";
            $shipPhone = "";
            
            if ($row['delivery_name'] == $row['billing_name'] &&
				$row['delivery_street_address'] == $row['billing_street_address'] &&
				$row['delivery_postcode'] == $row['billing_postcode'])
			{
				$shipEmail = $billEmail;
				$shipPhone = $billPhone;			
			}
			
			writeStartTag("ShippingAddress");
				writeElement("FullName", $row['delivery_name']);
				writeElement("Company", $row['delivery_company']);
				writeElement("Street1", $row['delivery_street_address']);
				writeElement("Street2", $row['delivery_suburb']);
				writeElement("Street3", "");
				writeElement("City", $row['delivery_city']);
				writeElement("State", $row['delivery_state']);
				writeElement("PostalCode", $row['delivery_postcode']);
				writeElement("Country", $row['delivery_country']);
				writeElement("Phone", $shipPhone);
				writeElement("Email", $shipEmail);
			writeCloseTag("ShippingAddress");
			
			writeStartTag("BillingAddress");
				writeElement("FullName", $row['billing_name']);
				writeElement("Company", $row['billing_company']);
				writeElement("Street1", $row['billing_street_address']);
				writeElement("Street2", $row['billing_suburb']);
				writeElement("Street3", "");
				writeElement("City", $row['billing_city']);
				writeElement("State", $row['billing_state']);
				writeElement("PostalCode", $row['billing_postcode']);
				writeElement("Country", $row['billing_country']);
				writeElement("Phone", $billPhone);
				writeElement("Email", $billEmail);
			writeCloseTag("BillingAddress");
			
			writeStartTag("Payment");
				writeElement("Method", $row['payment_method']);
				
				writeStartTag("CreditCard");
					writeElement("Type", $row['cc_type']);
					writeElement("Owner", $row['cc_owner']);
    				
					if ($secure)
					{
						writeElement("Number", $row['cc_number']);
					}
					else
					{
						writeElement("Number", "*******");
					}
    				
					writeElement("Expires", $row['cc_expires']);
				writeCloseTag("CreditCard");
				
			writeCloseTag("Payment");
			
			WriteOrderItems($row['orders_id'], $currencyFactor);
			
			WriteOrderTotals($row['orders_id']);
			
			writeStartTag("Debug");
				writeElement("LastModifiedLocal", $row['Modified']);
			writeCloseTag("Debug");
		writeCloseTag("Order");
	}
	
	// Write all totals lines for the order
	function WriteOrderTotals($orderID)
	{
		$totalQuery = tep_db_query(
			"select * from " . TABLE_ORDERS_TOTAL . 
			" where orders_id = " . $orderID);
			
		writeStartTag("Totals");
		
		while($total = tep_db_fetch_array($totalQuery))
		{
			$value = $total['value'];
			$class = $total['class'];
			$name = $total['title'];
			$impact = "add";
			
			// Coupons \ Discounts are added to the totals table as positive values,
			// though they should reflect as negative values on the order total.
			if ($class == "ot_coupon" ||
			    $class == "ot_gv" ||
			    $class == "ot_lev_discount" ||
			    $class == "ot_qty_discount" ||
		 	    $class == "ot_redemptions")
			{
			    $impact = "subtract";
		    }
		    else if ($class == "ot_subtotal" ||
					 $class == "ot_total")
			{
				$impact = "none";		 
			}		 
			
			$class = preg_replace("/^ot_/i", "", $class);
			
			$attributes = array("id" => $total['orders_total_id'],
								"name" => $name,
								"class" => $class,
								"impact" => $impact);
			writeFullElement("Total", $value, $attributes);
		}
		
		writeCloseTag("Totals");
	}

	// Write XML for all products for the given order
	function WriteOrderItems($orderID, $currencyFactor)
	{
	    $imageRoot = HTTP_CATALOG_SERVER . DIR_WS_CATALOG . DIR_WS_IMAGES;
	    
		$itemQuery = tep_db_query(
			"select * from " . TABLE_ORDERS_PRODUCTS . 
			" where orders_id = " . $orderID);
			
		writeStartTag("Items");
		
		while($item = tep_db_fetch_array($itemQuery))
		{
			// Get product info
			$productQuery = tep_db_query(
				"select * from " . TABLE_PRODUCTS .
				" where products_id = " . $item['products_id']);
				
			$product = tep_db_fetch_array($productQuery);
			
			// Build fully qualified image url
			$imageUrl = $product['products_image'];
			if (isset($imageUrl) and strlen($imageUrl) > 0)
			{
			    $imageUrl = $imageRoot . $imageUrl;
			}
			
			writeStartTag("Item");
				writeElement("ItemID", $item['orders_products_id']);
				writeElement("ProductID", $item['products_id']);
				writeElement("Code", $item['products_model']);
				writeElement("Name", $item['products_name']);
				writeElement("Quantity", $item['products_quantity']);
				writeElement("UnitPrice", $item['products_price'] * $currencyFactor);
				writeElement("Image", $imageUrl);

				$weight = $product['products_weight'];
				if ($weight)
				{
					writeElement("Weight", $weight);
				}
				else
				{
					writeElement("Weight", "0");
				}
				
				// Write attributes
				WriteItemAttributes($item['orders_products_id'], $currencyFactor);
				
			writeCloseTag("Item");
		}
		
		writeCloseTag("Items");
	}
	
	// Write all attributes for the item
	function WriteItemAttributes($itemID, $currencyFactor)
	{
		$attQuery = tep_db_query(
			"select * from " . TABLE_ORDERS_PRODUCTS_ATTRIBUTES . 
			" where orders_products_id = " . $itemID);
			
		writeStartTag("Attributes");
		
		while($att = tep_db_fetch_array($attQuery))
		{
			$price = $att['options_values_price'];
			
			if ($att['price_prefix'] == '-')
			{
				$price = -$price;
			}
			
			writeStartTag("Attribute");
				writeElement("AttributeID", $att['orders_products_attributes_id']);
				writeElement("Name", $att['products_options']);
				writeElement("Value", $att['products_options_values']);
				writeElement("Price", $price * $currencyFactor);
			writeCloseTag("Attribute");
		}
		
		writeCloseTag("Attributes");
	}
	
	function Action_GetStatusCodes()
	{
		writeStartTag("StatusCodes");
		
		$codes = tep_get_orders_status();
		reset($codes);
		while (list($key, $val) = each($codes))
		{
			writeStartTag("StatusCode");
				writeElement("Code", $val['id']);
				writeElement("Name", $val['text']);
			writeCloseTag("StatusCode");
		}
		
		writeCloseTag("StatusCodes");
	}
	
	function Action_UpdateStatus()
	{	    
	    if (!isset($_REQUEST['order']) || !isset($_REQUEST['status']) || !isset($_REQUEST['comments']))
	    {
	        outputError(40, "Insufficient parameters");
	        return;
	    }
	    
	    $orderID = (int) $_REQUEST['order'];
	    $code = (int) $_REQUEST['status'];
	    
	    $comments = mysql_escape_string($_REQUEST['comments']);
	    
        tep_db_query(
            "insert into " . TABLE_ORDERS_STATUS_HISTORY . 
            " (orders_id, orders_status_id, date_added, customer_notified, comments) " .
            " values (" . $orderID . ", " . $code . ", now(), 0, '" . $comments . "')");
            
        tep_db_query(
            "update " . TABLE_ORDERS .
            " set orders_status = " . $code . " " . 
            " where orders_id = " . $orderID);
            
		echo "<UpdateSuccess/>";	
	}
	
	function toGmt($dateSql)
	{
        $pattern = "/^(\d{4})-(\d{2})-(\d{2})\s+(\d{2}):(\d{2}):(\d{2})$/i";

        if (preg_match($pattern, $dateSql, $dt)) 
        {
            $dateUnix = mktime($dt[4], $dt[5], $dt[6], $dt[2], $dt[3], $dt[1]);
            return gmdate("Y-m-d\TH:i:s", $dateUnix);
        }
        
        return $dateSql;
    }
    
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

	function GetCountryCode($country_id) 
	{
		$country_query = tep_db_query("select countries_iso_code_2 from " . TABLE_COUNTRIES . " where countries_id = '" . (int)$country_id . "'");

		if (!tep_db_num_rows($country_query)) {
		return $country_id;
		} else {
		$country = tep_db_fetch_array($country_query);
		return $country['countries_iso_code_2'];
		}
	}

?>
