<?php
        define('IS_ADMIN_FLAG', true);
        define('SHIPWORKS_MODULE_ACTIVE', true);
        define('REQUIRE_SECURE', TRUE);

        global $db;
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
           | Adapted from OSCommerce file
           | by unknown author
           | and Dave Scotese for MediaSolutionsHoldings and Interapptive
           | and modified by Delia Wilson Lunsford, Sept. 2007.
         */

        /**
         * Hooks added for customization of Zencart Shipworks module
         * ---------------------------------------------------------
         *
         * Shipworks.php for Zencart calls these functions but ignores the
         * return values.  They are intended to allow Zen users to customize
         * data flowing between shipworks and the Zencart database.
         *
         * The read hooks pass the queryFactoryResult object to the following
         * functions in the corresponding functions in this file so that the
         * data in each row can be updated as it is visited, and before
         * shipworks processes it:
         *      sw_GetOrders_row_hook( &$row )
         *      sw_OrderItems_row_hook( &$item )
         *      sw_OrderTotals_row_hook( &$total )
         *      sw_ItemAttributes_row_hook( &$att )
         *      sw_ShippingMethod_hook( &$shipping_method )
         *
         * ::::NOTE::::
         * If you declare the read hooks without the ampersand, your changes
         * will be lost.  The objects must be passed by reference if you want
         * to use them to change the data shipworks will use.
         *
         * The following hook is called in Action_UpdateStatus() after
         * Shipworks has updated the status history table when the
         * "update online status" action is used.
         *      sw_UpdateStatus_hook( $orderID, $code, $comments )
         *
         */

        $moduleVersion = "3.3.7.1";
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

        // Output the given tag\value pair
        function writeElement($tag, $value)
        {
                writeStartTag($tag);
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

        // As of Zen 1.5.0 if $_POST['action'] is set init_sessions.php would redirect to index.php and we'd fail.  So we extract our action now, and then unset it.
        $action = (isset($_REQUEST['action']) ? $_REQUEST['action'] : '');
	    unset($_POST['action']);

        require('includes/application_top.php');

        $secure = ($_SERVER['HTTPS'] == 'on' || $_SERVER['HTTPS'] == '1');

        // Open the XML output and root
        writeXmlDeclaration();
        writeStartTag("ShipWorks", array("moduleVersion" => $moduleVersion, "schemaVersion" => $schemaVersion));

        // Enforse SSL
        if (!$secure && REQUIRE_SECURE)
        {
                outputError(10, "Invalid URL, HTTPS is required");
        }
        // Proceed
        else
        {
                global $db;

                if (checkAdminLogin())
                {
                        $action = (isset($_REQUEST['action']) ? $_REQUEST['action'] : '');
                        if (zen_not_null($action))
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

        function checkAdminLogin()
        {
                global $_REQUEST;
                global $db;

                $loginOK = false;

                if (isset($_REQUEST['username']) && isset($_REQUEST['password']))
                {
                        $admin = zen_db_prepare_input($_REQUEST['username']);
                        $password = zen_db_prepare_input($_REQUEST['password']);

                        $check_admin = $db->Execute("select admin_id as login_id, admin_pass as login_pass " .
                                        " from " . TABLE_ADMIN .
                                        " where admin_name = '". zen_db_input($admin) . "'");

                        // Check that password is good Zen v1.3
                        if (validatePassword($password, $check_admin->fields['login_pass']))
                        {
                                $loginOK = true;
                        }
                        
                        // Check that password is good Zen v1.5
                        if (zen_validate_password($password, $check_admin->fields['login_pass'])){
                                $loginOK = true;
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

        // Get module data
        function Action_GetModule()
        {
                writeStartTag("Module");

                writeElement("Platform", "ZenCart");
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
                writeElement("State", zen_get_zone_code(STORE_COUNTRY, STORE_ZONE, "CA"));
                writeElement("Country", GetCountryCode(STORE_COUNTRY));
                writeElement("Website", HTTP_CATALOG_SERVER);

                writeCloseTag("Store");
        }

        // Get the count of orders greater than the start ID
        function Action_GetCount()
        {
                global $_REQUEST, $db;

                $start = 0;

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

                $rows = $db->Execute(
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

                if (!$rows->EOF)
                {
                        $count = $rows->fields['count'];
                }

                writeElement("OrderCount", $count);
        }

        // Outputs notes elements
        function WriteNote($noteText, $dateAdded, $public)
        {
				$regex = "/^\d{4}\-\d{2}\-\d{2}$/";
				if (preg_match($regex, $dateAdded))
				{
					$dateAdded = $dateAdded. " 12:00:00";
				}

                $attributes = array("public" => $public ? "true" : "false",
                                "date" => toGmt($dateAdded));

                writeFullElement("Note", $noteText, $attributes);
        }

        // Get all orders greater than the given start id, limited by max count
        function Action_GetOrders()
        {
                global $_REQUEST;
                global $db;

                $start = 0;
                $maxcount = 50;

                if (isset($_REQUEST['start']))
                {
                        $start = $_REQUEST['start'];
                }

                // Convert to local SQL time
                $start = toLocalSqlDate($start);

                if (isset($_REQUEST['maxcount']))
                {
                        $maxcount = $_REQUEST['maxcount'];
                }

                // Only get orders through 2 seconds ago.
                $end = date("Y-m-d H:i:s", time() - 2);

                // Write the params for easier diagnostics
                writeStartTag("Parameters");
                writeElement("Start", $start);
                writeElement("End", $end);
                writeElement("MaxCount", $maxcount);
                writeCloseTag("Parameters");

                writeStartTag("Orders");


                $row = $db->Execute(
                                "select * ," .
                                "             CASE " .
                                "               WHEN last_modified IS NOT NULL THEN last_modified" .
                                "               ELSE date_purchased" .
                                "            END as Modified" .
                                " from " . TABLE_ORDERS . " o, " . TABLE_ORDERS_TOTAL . " ot " .
                                " where ot.orders_id = o.orders_id " .
                                "   and ot.class = 'ot_total' " .
                                " having Modified > '$start' " .
                                "    and Modified <= '$end' " .
                                " order by Modified asc " .
                                " limit 0, " . $maxcount);
                // loop over results
                while (!$row->EOF)
                {
                        if( function_exists( "sw_GetOrders_row_hook" ) )
                        {
                                sw_GetOrders_row_hook( $row );
                        }

                        // save the most current processed mod time
                        $lastModified = $row->fields['Modified'];

                        // keep track of processed/exported ids
                        if ($processedIds != "")
                        {
                                $processedIds .= ", ";
                        }
                        $processedIds .= $row->fields['orders_id'];

                        // output the order xml
                        WriteOrder($row);

                        // Next order
                        $row->MoveNext();
                }

                // if we processed some orders, we may need to get some more
                if ($processedIds != "")
                {
                        // This makes sure we don't split a page between orders of the same modified time.
                        // If there were any that didn't make the maxcount cutoff with the same last modified time
                        // as the greatest last modified time we already processed, this will get them.
                        $row = $db->Execute(
                                        "select * ," .
                                        "             CASE " .
                                        "               WHEN last_modified IS NOT NULL THEN last_modified" .
                                        "               ELSE date_purchased" .
                                        "            END as Modified" .
                                        " from " . TABLE_ORDERS . " o, " . TABLE_ORDERS_TOTAL . " ot " .
                                        " where ot.orders_id = o.orders_id " .
                                        "   and ot.class = 'ot_total' " .
                                        "   and o.orders_id not in ($processedIds) " .
                                        "having Modified = '$lastModified' " );

                        while (!$row->EOF)
                        {
                                if( function_exists( "sw_GetOrders_row_hook" ) )
                                {
                                        sw_GetOrders_row_hook( $row );
                                }

                                // output xml
                                WriteOrder($row);

                                // Next order
                                $row->MoveNext();
                        }
                }

                writeCloseTag("Orders");
        }

        // Write a single order as Xml
        function WriteOrder($row)
        {
                global $db;

                $currencyFactor = (int) $row->fields['currency_value'];

                $shipping_method = $db->Execute("select title from " . TABLE_ORDERS_TOTAL . " where orders_id = '" . (int)$row->fields['orders_id'] . "' and class = 'ot_shipping'");
                if( function_exists( 'sw_ShippingMethod_hook' ) )
                {
                        sw_ShippingMethod_hook( $shipping_method );
                }

                $shipping_method = $shipping_method->fields['title'];
                $shipping_method = ((substr($shipping_method, -1) == ':') ? substr(strip_tags($shipping_method), 0, -1) : strip_tags($shipping_method));

                // The customer comment will be the first history item
                $comments = $db->Execute(
                                "select comments, date_added from " . TABLE_ORDERS_STATUS_HISTORY .
                                " where orders_id = " .  $row->fields['orders_id'] . " " . 
                                " order by date_added asc LIMIT 0,1");

                writeStartTag("Order");

                writeElement("OrderNumber", $row->fields['orders_id']);
                writeElement("OrderDate", toGmt($row->fields['date_purchased']));
                writeElement("LastModified", toGmt($row->fields['Modified']));
                writeElement("ShippingMethod", $shipping_method);
                writeElement("StatusCode", $row->fields['orders_status']);

                // See if the customer actually exists
                $customerExists = null != $db->Execute("select * from " . TABLE_CUSTOMERS . " where customers_id = " . $row->fields['customers_id']);
                writeElement("CustomerID", $customerExists ? $row->fields['customers_id'] : "-1");

                writeStartTag("Notes");
                while (!$comments->EOF)
                {
                        if ($comments->fields['comments'] != "")
                        {
                                WriteNote($comments->fields['comments'], $comments->fields['date_added'], "true");
                        }

                        $comments->MoveNext();
                }
                writeCloseTag("Notes");

                $billEmail = $row->fields['customers_email_address'];
                $billPhone = $row->fields['customers_telephone'];
                $shipEmail = "";
                $shipPhone = "";

                if ($row->fields['delivery_name'] == $row->fields['billing_name'] &&
                                $row->fields['delivery_street_address'] == $row->fields['billing_street_address'] &&
                                $row->fields['delivery_postcode'] == $row->fields['billing_postcode'])
                {
                        $shipEmail = $billEmail;
                        $shipPhone = $billPhone;			
                }

                writeStartTag("ShippingAddress");
                writeElement("FullName", $row->fields['delivery_name']);
                writeElement("Company", $row->fields['delivery_company']);
                writeElement("Street1", $row->fields['delivery_street_address']);
                writeElement("Street2", $row->fields['delivery_suburb']);
                writeElement("Street3", "");
                writeElement("City", $row->fields['delivery_city']);
                writeElement("State", GetStateCode($row->fields['delivery_country'], $row->fields['delivery_state']));
                writeElement("PostalCode", $row->fields['delivery_postcode']);
                writeElement("Country", $row->fields['delivery_country']);
                writeElement("Phone", $shipPhone);
                writeElement("Email", $shipEmail);
                writeCloseTag("ShippingAddress");

                writeStartTag("BillingAddress");
                writeElement("FullName", $row->fields['billing_name']);
                writeElement("Company", $row->fields['billing_company']);
                writeElement("Street1", $row->fields['billing_street_address']);
                writeElement("Street2", $row->fields['billing_suburb']);
                writeElement("Street3", "");
                writeElement("City", $row->fields['billing_city']);
                writeElement("State", GetStateCode($row->fields['billing_country'], $row->fields['billing_state']));
                writeElement("PostalCode", $row->fields['billing_postcode']);
                writeElement("Country", $row->fields['billing_country']);
                writeElement("Phone", $billPhone);
                writeElement("Email", $billEmail);
                writeCloseTag("BillingAddress");

                writeStartTag("Payment");
                writeElement("Method", $row->fields['payment_method']);

                writeStartTag("CreditCard");
                writeElement("Type", $row->fields['cc_type']);
                writeElement("Owner", $row->fields['cc_owner']);
                writeElement("Number", "*******");
                writeElement("Expires", $row->fields['cc_expires']);
                writeCloseTag("CreditCard");

                writeCloseTag("Payment");

                WriteOrderItems($row->fields['orders_id'], $currencyFactor);

                WriteOrderTotals($row->fields['orders_id']);

                writeCloseTag("Order");

        }


        // Write all totals lines for the order
        function WriteOrderTotals($orderID)
        {
                global $db;
                $total = $db->Execute(
                                "select * from " . TABLE_ORDERS_TOTAL .
                                " where orders_id = " . $orderID);

                writeStartTag("Totals");

                while(!$total->EOF)
                {
                        if( function_exists( "sw_OrderTotals_row_hook" ) )
                        {
                                sw_OrderTotals_row_hook( $total );
                        }

                        $value = $total->fields['value'];
                        $class = $total->fields['class'];
                        $impact = "add";

                        // Coupons \ Discounts are added to the totals table as positive values,
                        // though they should reflect as negative values on the order total.
                        if ($class == "ot_coupon" ||
                                        $class == "ot_gv" ||
                                        $class == "ot_lev_discount" ||
                                        $class == "ot_group_pricing" ||
                                        $class == "ot_quantity_discount")
                        {
                                $impact = "subtract";
                        }
                        else if ($class == "ot_subtotal" ||
                                        $class == "ot_total")
                        {
                                $impact = "none";			 
                        }

                        $class = preg_replace("/^ot_/i", "", $class);

                        $attributes = array("id" => $total->fields['orders_total_id'],
                                        "name" => $total->fields['title'],
                                        "class" => $class,
                                        "impact" => $impact);
                        writeFullElement("Total", $value, $attributes);

                        $total->MoveNext();
                }

                writeCloseTag("Totals");
        }



        // Write XML for all products for the given order
        function WriteOrderItems($orderID, $currencyFactor)
        {
                global $db;
                $imageRoot = HTTP_CATALOG_SERVER . DIR_WS_CATALOG . DIR_WS_IMAGES;

                $item = $db->Execute(
                                "select * from " . TABLE_PRODUCTS .
                                " INNER JOIN ".TABLE_ORDERS_PRODUCTS." USING(products_id)".
                                " where orders_id = " . $orderID);

                writeStartTag("Items");

                while(!$item->EOF)
                {
                        if( function_exists( "sw_OrderItems_row_hook" ) )
                        {
                                sw_OrderItems_row_hook( $item );
                        }

                        // Build fully qualified image url
                        $imageUrl = $item->fields['products_image'];
                        if (isset($imageUrl) and strlen($imageUrl) > 0)
                        {
                                $imageUrl = $imageRoot . $imageUrl;
                        }

                        writeStartTag("Item");
                        writeElement("ItemID", $item->fields['orders_products_id']);
                        writeElement("ProductID", $item->fields['products_id']);
                        writeElement("Code", $item->fields['products_model']);
                        writeElement("Name", $item->fields['products_name']);
                        writeElement("Quantity", $item->fields['products_quantity']);
                        writeElement("Image", $imageUrl);

                        $pricedByAttributes = $item->fields['products_priced_by_attribute'];

                        // Write attributes
                        $totals = WriteItemAttributes($item, $currencyFactor);

                        writeElement("UnitPrice", $totals["totalprice"]);
                        writeElement("Weight", $item->fields['products_weight'] + $totals["totalweight"]);

                        writeCloseTag("Item");

		                // write out onetime charges if there is one
		                $oneTime = $item->fields['onetime_charges'];
		                if ($oneTime > 0)
		                {
								$code = "OTC_". $item->fields['products_model'];

		                        writeStartTag("Item");
		                        writeElement("ItemID", $item->fields['orders_products_id']);
		                        writeElement("ProductID", "0");
		                        writeElement("Code", $code);
		                        writeElement("Name", "One Time Charge");
								writeElement("Quantity", "1");
								writeElement("Weight", "0");
								writeElement("UnitPrice", $oneTime);
		                        writeCloseTag("Item");
		                }

                        $item->MoveNext();
                }

                writeCloseTag("Items");
        }

        // Write all attributes for the item
        function WriteItemAttributes($item, $currencyFactor) //$itemID, $productIsFree, $currencyFactor, $pricedByAttributes, $productsPrice, $finalPrice)
        {
                global $db;

                // Item-level fields
                $itemID = $item->fields['orders_products_id'];
                $productIsFree = $item->fields['product_is_free'];
                $pricedByAttributes = $item->fields['products_priced_by_attribute'];
                $productsPrice = $item->fields['products_price'];
                $finalPrice = $item->fields['final_price'];

                $att = $db->Execute(
                                "select * from " . TABLE_ORDERS_PRODUCTS_ATTRIBUTES .
                                " where orders_products_id = " . $itemID);

                writeStartTag("Attributes");

                $totalPrice = 0;
                $totalWeight = 0;

                while(!$att->EOF)
                {
                        // customization hook
                        if( function_exists( "sw_ItemAtributtes_row_hook" ) )
                        {
                                sw_ItemAttributes_row_hook( $att );
                        }

                        // attribute price
                        if (($att->fields['product_attribute_is_free'] && $productIsFree) || $productsPrice == 0)
                        {
                                // don't include this price
                                $price = 0;
                        }
                        else
                        {
                                $price = $att->fields['options_values_price'];
                        }


                        // negate
                        if ($att->fields['price_prefix'] == '-')
                        {
                                $price = -$price;
                        }

                        $name = $att->fields['products_options'];
                        $value = $att->fields['products_options_values']; //$value = "$value ($". number_format(round($price, 2), 2). ")";

                        // sum totals
                        $totalPrice = $totalPrice + $price;
                        $totalWeight = $totalWeight + $att->fields['products_attributes_weight'];

                        writeStartTag("Attribute");
                        writeElement("AttributeID", $att->fields['orders_products_attributes_id']);
                        writeElement("Name", $name);
                        writeElement("Value", $value);
                        writeElement("Price", $price);
                        writeCloseTag("Attribute");

                        $att->MoveNext();
                }


                writeCloseTag("Attributes");

                writeStartTag("Debug");
                writeElement("pricedByAttributes", $pricedByAttributes);
                writeElement("finalPrice", $finalPrice);
                writeElement("productsPrice", $productsPrice);
                writeElement("totalPriceBefore", $totalPrice);

                if ($pricedByAttributes)
                {
                        $totalPrice = $totalPrice + $finalPrice;
                }
                else
                {
                        $totalPrice = $productsPrice;
                }
                writeElement("totalPriceAfter", $totalPrice);

                writeCloseTag("Debug");

                return array("totalprice" => $totalPrice, "totalweight" => $totalWeight);
        }


        function Action_GetStatusCodes()
        {
                writeStartTag("StatusCodes");

                $codes = zen_get_orders_status();
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
                global $_REQUEST;
                global $db;

                if (!isset($_REQUEST['order']) || !isset($_REQUEST['status']) || !isset($_REQUEST['comments']))
                {
                        outputError(40, "Insufficient parameters");
                        return;
                }

                $orderID = (int) $_REQUEST['order'];
                $code = (int) $_REQUEST['status'];

                $comments = mysql_escape_string($_REQUEST['comments']);

                $db->Execute(
                                "insert into " . TABLE_ORDERS_STATUS_HISTORY .
                                " (orders_id, orders_status_id, date_added, customer_notified, comments) " .
                                " values (" . $orderID . ", " . $code . ", now(), 0, '" . $comments . "')");

                $db->Execute(
                                "update " . TABLE_ORDERS .
                                " set orders_status = " . $code . " " .
                                " where orders_id = " . $orderID);

                if( function_exists( "sw_UpdateStatus_hook" ) )
                {
                        sw_UpdateStatus_hook( $orderID, $code, $comments );
                }

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

        // Returns the zen country id for the given named country
        function GetCountryID($country_name)
        {
                global $db;
                $qry = sprintf("select countries_id from ". TABLE_COUNTRIES. " where countries_name = '%s'",
                                mysql_real_escape_string($country_name));

                $countryQuery = $db->Execute($qry);
                if ($countryQuery->RecordCount() == 0)
                {
                        return $country_name;
                }
                else
                {
                        return $countryQuery->fields['countries_id'];
                }
        }

        // Returns the state code for a given state and country name
        function GetStateCode($country_name, $state_name)
        {
                global $db;

                $country_id = GetCountryID($country_name);

                if ($country_id == '')
                {
                        return $state_name;
                }

                // now lookup the state code based on countryid and state name
                $qry = sprintf("select zone_code from ". TABLE_ZONES. " where zone_country_id = '%s' and zone_name = '%s'",
                                mysql_real_escape_string($country_id),
                                mysql_real_escape_string($state_name));
                $stateQuery = $db->Execute($qry);
                if ($stateQuery->RecordCount() == 0)
                {
                        return $state_name;
                }
                else
                {
                        return $stateQuery->fields['zone_code'];
                }

        }

        // returns the country code for a given zen country id
        function GetCountryCode($country_id)
        {
                global $db;
                $country = $db->Execute("select countries_iso_code_2 from " . TABLE_COUNTRIES . " where countries_id = '" . (int)$country_id . "'");

                if (!$country->RecordCount() > 1) {
                        return $country_id;
                } else {

                        return $country->fields['countries_iso_code_2'];
                }
        }


        // Copy of zen_validate_password
        function validatePassword($plain, $encrypted)
        {
            if (zen_not_null($plain) && zen_not_null($encrypted))
            {
                // split apart the hash / salt
                $stack = explode(':', $encrypted);

                if (sizeof($stack) != 2)
                    return false;

                if (md5($stack[1] . $plain) == $stack[0])
                {
                    return true;
                }
            }
            return false;
        }

?>
