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
define('REQUIRE_SECURE', TRUE);
$moduleVersion = "3.10.0.0";
$schemaVersion = "1.0.0";

// include the Mage engine
require_once 'app/Mage.php';
umask(0);

// retrieve the store code
$storeCode = '';
if (isset($_REQUEST['storecode']))
{
	$storeCode = $_REQUEST['storecode'];	
}

// using output buffering to get around headers that magento is setting after we've started output
ob_start();

header("Content-Type: text/xml;charset=utf-8");
header("Last-Modified: " . gmdate("D, d M Y H:i:s") . " GMT");

// HTTP/1.1
header("Cache-Control: no-store, no-cache, must-revalidate");
header("Cache-Control: post-check=0, pre-check=0", false);

// HTTP/1.0
header("Pragma: no-cache");     

// Tests the Magento version to see if it's greater than or equal to the targetVersion
function MagentoVersionGreaterOrEqualTo($targetVersion)
{
	$mageVersion = Mage::getVersion();
	
	$currentParts = preg_split('[\.]', $mageVersion);
	$targetParts = preg_split('[\.]', $targetVersion);

	$i = 0;
	foreach ($currentParts as $currentPart)
	{
		if ($i >= count($targetParts))
		{
			// gotten this far, means that current version of 1.4.0.1 > target version 1.4.0
			return true;
		}

		$targetPart = $targetParts[$i];

		// if this iteration's target version part is greater than the magento version part, we're done.
		if ((int)$targetPart > (int)$currentPart)
		{
			return false;
		}
		else if ((int)$targetPart < (int)$currentPart)
		{
			// the magento version part is greater, then we're done
			return true;
		}


		// otherwise to this point the two are equal, continue
		$i++;
	}

	// got this far means the two are equal
	return true;
}

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

try
{
	// start the mage engine
	Mage::app($storeCode);
}
catch (Mage_Core_Model_Store_Exception $e)
{
	outputError(100, "Invalid store code");
	writeCloseTag("ShipWorks");
	exit;
}

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
			case 'updateorder': Action_UpdateOrder(); break;
			default:
				outputError(20, "Invalid action '$action'");
		}
	}
}

// Close the output
writeCloseTag("ShipWorks");

// Check to see if admin functions exist.  And if so, determine if the user
// has access.
function checkAdminLogin()
{
	$loginOK = false;

	if (isset($_REQUEST['username']) && isset($_REQUEST['password']))
	{
		$username = $_REQUEST['username'];
		$password = $_REQUEST['password'];

		$user = Mage::getSingleton('admin/session')->login($username, $password);
		if ($user && $user->getId())
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

function Action_GetModule()
{
	writeStartTag("Module");
	
	writeElement("Platform", "Magento");
	writeElement("Developer", "Interapptive, Inc. (support@interapptive.com)");
	
	writeStartTag("Capabilities");
	writeElement("DownloadStrategy", "ByModifiedTime");
	writeFullElement("OnlineCustomerID", "", array("supported" => "true", "dataType" => "numeric"));
	writeFullElement("OnlineStatus", "", array("supported" => "true", "dataType" => "text", "downloadOnly" => "true" ));
	writeFullElement("OnlineShipmentUpdate", "", array("supported" => "false"));
	writeCloseTag("Capabilities");
	
	writeCloseTag("Module");
}

// Write store data
function Action_GetStore()
{
	// get state name
	$region_model = Mage::getModel('directory/region');
	if (is_object($region_model))
	{
		$state = $region_model->load(Mage::getStoreConfig('shipping/origin/region_id'))->getDefaultName();
	}

	$name = Mage::getStoreConfig('system/store/name');
	$owner = Mage::getStoreConfig('trans_email/ident_general/name');
	$email = Mage::getStoreConfig('trans_email/ident_general/email');
	$country = Mage::getStoreConfig('shipping/origin/country_id');
	$website = Mage::getURL();

	writeStartTag("Store");
	writeElement("Name", $name);
	writeElement("CompanyOrOwner", $owner);
	writeElement("Email", $email);
	writeElement("State", $state);
	writeElement("Country", $country);
	writeElement("Website", $website);
	writeCloseTag("Store");
}

// Converts an xml datetime string to sql date time
function toLocalSqlDate($sqlUtc)
{   
	$pattern = "/^(\d{4})-(\d{2})-(\d{2})\T(\d{2}):(\d{2}):(\d{2})$/i";

	if (preg_match($pattern, $sqlUtc, $dt)) 
	{
		$unixUtc = gmmktime($dt[4], $dt[5], $dt[6], $dt[2], $dt[3], $dt[1]);  

		return date("Y-m-d H:i:s", $unixUtc);
	}

	return $sqlUtc;
}


// Get the count of orders greater than the start ID
function Action_GetCount()
{         
	$start = 0;
	$storeId = Mage::app()->getStore()->storeId;

	if (isset($_REQUEST['start']))
	{
		$start = $_REQUEST['start'];
	}

	// only get orders through 2 seconds ago
	$end = date("Y-m-d H:i:s", time() - 2);

	// Convert to local SQL time
	$start = toLocalSqlDate($start);

	// Write the params for easier diagnostics
	writeStartTag("Parameters");
	writeElement("Start", $start);
	writeCloseTag("Parameters");

	$orders = Mage::getModel('sales/order')->getCollection();
	$orders->addAttributeToSelect("updated_at")->getSelect()->where("(updated_at > '$start' AND updated_at <= '$end' AND store_id = $storeId)");
	$count = $orders->count();

	writeElement("OrderCount", $count);
}

// Get all orders greater than the given start id, limited by max count
function Action_GetOrders()
{
	$storeId = Mage::app()->getStore()->storeId;
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

	// Only get orders through 2 seconds ago.
	$end = date("Y-m-d H:i:s", time() - 2);

	// Convert to local SQL time
	$start = toLocalSqlDate($start);

	// Write the params for easier diagnostics
	writeStartTag("Parameters");
	writeElement("Start", $start);
	writeElement("End", $end);
	writeElement("MaxCount", $maxcount);
	writeCloseTag("Parameters");                                    

	// setup the query
	$orders = Mage::getModel('sales/order')->getCollection();
	$orders->addAttributeToSelect("*")
		->getSelect()
		->where("(updated_at > '$start' AND updated_at <= '$end' AND store_id = $storeId)")
		->order('updated_at', 'asc');
	
	// configure paging
	$orders->setCurPagE(1)
		->setPageSize($maxcount)
		->loadData();

	writeStartTag("Orders");

	$lastModified = null;
	$processedIds = "";

	foreach ($orders as $order)
	{
		// keep track of the ids we've downloaded
		$lastModified = $order->getUpdatedAt();

		if ($processedIds != "")
		{
			$processedIds .= ", ";
		}
		$processedIds .= $order->getEntityId();

		WriteOrder($order);
	}

	// if we processed some orders we may have to get some more
	if ($processedIds != "")
	{
		$orders = Mage::getModel('sales/order')->getCollection();
		$orders->addAttributeToSelect("*")->getSelect()->where("updated_at = '$lastModified' AND entity_id not in ($processedIds) AND store_id = $storeId");

		foreach ($orders as $order)
		{
			WriteOrder($order);
		}
	}

	writeCloseTag("Orders");
}

// Output the order as xml
function WriteOrder($order)
{                 
	global $secure;
	writeStartTag("Order");

	$incrementId = $order->getIncrementId();
	
	$orderPrefix = '';
	$orderNumber = '';
	$orderPostfix = '';
	$ordernumberParts = '';

	if(is_numeric($incrementId))
	{
		$orderNumber = $incrementId;
	}
	else
	{
		if(strpos($incrementId, '-') !== false)
		{
			$ordernumberParts = explode('-', $incrementId);

			if(count($ordernumberParts == 2))
			{
				if(is_numeric($ordernumberParts[0]))
				{
					$orderNumber = $ordernumberParts[0];
					$orderPostfix = '-'.$ordernumberParts[1];
				}
				else
				{
					$orderPrefix = $ordernumberParts[0]."-";
					$orderNumber = $ordernumberParts[1];
				}
			}
		} 
		elseif (!is_numeric($incrementId)) 
		{
			if(preg_split('/(?=\d)/', $incrementId,2)[0] != '')
			{
				$ordernumberParts = preg_split('/(?=\d)/', $incrementId,2);

				$orderPrefix = $ordernumberParts[0];
				$orderNumber = $ordernumberParts[1];
			}
			elseif(is_numeric(preg_split('/(?=\D)/', $incrementId,2)[0]))
			{
				$ordernumberParts = preg_split('/(?=\D)/', $incrementId,2);

				$orderNumber = $ordernumberParts[0];
				$orderPostfix = $ordernumberParts[1];
			}
		}
	}
	
	writeElement("OrderNumberPrefix", $orderPrefix);
	writeElement("OrderNumber", $incrementId);
	writeElement("OrderNumberPostfix", $orderPostfix);
	writeElement("OrderDate", FormatDate($order->getCreatedAt()));
	writeElement("LastModified", FormatDate($order->getUpdatedAt()));
	writeElement("ShippingMethod", $order->getShippingDescription());
	writeElement("StatusCode", $order->getStatus());
	writeElement("CustomerID", $order->getCustomerId());

	// check for order-level gift messages
	if ($order->getGiftMessageId())
	{
		$message = Mage::helper('giftmessage/message')->getGiftMessage($order->getGiftMessageId());
		$messageString = "Gift message for ". $message['recipient']. ": ". $message['message'];

		writeStartTag("Notes");
		writeFullElement("Note", $messageString, array("public" => "true"));
		writeCloseTag("Notes");
	}

	$address = $order->getBillingAddress();
	writeStartTag("BillingAddress");
	writeElement("FullName", $address->getName());
	writeElement("Company", $address->getCompany());
	writeElement("Street1", $address->getStreet(1));
	writeElement("Street2", $address->getStreet(2));
	writeElement("Street3", $address->getStreet(3));
	writeElement("City", $address->getCity());
	writeElement("State", $address->getRegionCode());
	writeElement("PostalCode", $address->getPostcode());
	writeElement("Country", $address->getCountryId());
	writeElement("Phone", $address->getTelephone());
	writeElement("Email", $order->getCustomerEmail());
	writeCloseTag("BillingAddress");


	$billFullName = $address->getName();
	$billStreet1 = $address->getStreet(1);
	$billCity = $address->getCity();
	$billZip = $address->getPostcode();
	
	$address = $order->getShippingAddress();
	if (!$address)
	{
		// sometimes the shipping address isn't specified, so use billing
		$address = $order->getBillingAddress();
	}

	writeStartTag("ShippingAddress");
	writeElement("FullName", $address->getName());
	writeElement("Company", $address->getCompany());
	writeElement("Street1", $address->getStreet(1));
	writeElement("Street2", $address->getStreet(2));
	writeElement("Street3", $address->getStreet(3));
	writeElement("City", $address->getCity());
	writeElement("State", $address->getRegionCode());
	writeElement("PostalCode", $address->getPostcode());
	writeElement("Country", $address->getCountryId());
	writeElement("Phone", $address->getTelephone());

	// if the addressses appear to be the same, use customer email as shipping email too
	if ($address->getName() == $billFullName &&
		$address->getStreet(1) == $billStreet1 &&
		$address->getCity() == $billCity &&
		$address->getPostcode() == $billZip)
	{
		writeElement("Email", $order->getCustomerEmail());
	}

	writeCloseTag("ShippingAddress");


	$payment = $order->getPayment();

	// CC info
	if ($secure)
	{
		$cc_num = $payment->getCcNumber();
	}
	else
	{
		$cc_num = $payment->getCcLast4();
		if (!empty($cc_num))
		{
			$cc_num = '************'.$payment->getCcLast4();
		}
	}
	$cc_year = sprintf('%02u%s', $payment->getCcExpMonth(), substr($payment->getCcExpYear(), 2)); 


	writeStartTag("Payment");
	writeElement("Method", Mage::helper('payment')->getMethodInstance($payment->getMethod())->getTitle());

	writeStartTag("CreditCard");
	writeElement("Type", $payment->getCcType());
	writeElement("Owner", $payment->getCcOwner());
	writeElement("Number", $cc_num);
	writeElement("Expires", $cc_year);
	writeCloseTag("CreditCard");

	writeCloseTag("Payment");

	WriteOrderItems($order->getAllItems());

	WriteOrderTotals($order);
	
	writeStartTag("Debug");
	writeElement("OrderID", $order->getEntityId());
	writeElement("OrderNumberPostfix", $orderPostfix);
	writeCloseTag("Debug");

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

	if ($order->getTaxAmount() > 0)
	{
		WriteOrderTotal("Tax", $order->getTaxAmount(), "tax", "add");
	}

	// Magento 1.4 started storing discounts as negative values
	if (MagentoVersionGreaterOrEqualTo('1.4.0') && $order->getDiscountAmount() < 0)
	{
		$couponcode = $order->getCouponCode();
		WriteOrderTotal("Discount ($couponcode)", -1 * $order->getDiscountAmount(), "discount", "subtract");
	}

	if (!MagentoVersionGreaterOrEqualTo('1.4.0') && $order->getDiscountAmount() > 0)
	{
		$couponcode = $order->getCouponCode();
		WriteOrderTotal("Discount ($couponcode)", $order->getDiscountAmount(), "discount", "subtract");
	}

	if ($order->getGiftcertAmount() > 0)
	{
		WriteOrderTotal("Gift Certificate", $order->getGiftcertAmount(), "giftcertificate", "subtract");
	}

	if ($order->getAdjustmentPositive())
	{
		WriteOrderTotal("Adjustment Refund", $order->getAdjustmentPositive(), "refund", "subtract");
	}

	if ($order->getAdjustmentNegative())
	{
		WriteOrderTotal("Adjustment Fee", $order->getAdjustmentPositive(), "fee", "add");
	}

	WriteOrderTotal("Grand Total", $order->getGrandTotal(), "total", "none");

	writeCloseTag("Totals");
}

// Gets the price of an order item
function getCalculationPrice($item)
{
	if ($item instanceof Mage_Sales_Model_Order_Item)
	{ 
		if (MagentoVersionGreaterOrEqualTo('1.3.0'))
		{
			return $item->getPrice();
		}
		else
		{
			if ($item->hasCustomPrice())
			{
				return $item->getCustomPrice();
			}
			else if ($item->hasOriginalPrice())
			{
				return $item->getOriginalPrice();
			}
		}
	}

	return 0;
}

// Write XML for all products for the given order
function WriteOrderItems($orderItems)
{
	writeStartTag("Items");
	
	$parentMap = Array();

	// go through each item in the collection
	foreach ($orderItems as $item)
	{
		// keep track of item Id and types
		$parentMap[$item->getItemId()] = $item->getProductType();

		// get the sku
		if ($item->getProductType() == Mage_Catalog_Model_Product_Type::TYPE_CONFIGURABLE)
		{
			$sku = $item->getProductOptionByCode('simple_sku');
		}
		else
		{
			$sku = $item->getSku();
		}   
		
		// weights are handled differently if the item is a bundle or part of a bundle
		$weight = $item->getWeight();
		if ($item->getIsVirtual())
		{
			$weight = 0;
		}

		if ($item->getProductType() == Mage_Catalog_Model_Product_Type::TYPE_BUNDLE)
		{
			$name = $item->getName(). " (bundle)";
			$unitPrice = getCalculationPrice($item);
		}
		else
		{
			$name = $item->getName();

			// if it's part of a bundle
			if (is_null($item->getParentItemId()))
			{
				$unitPrice = getCalculationPrice($item);
			}
			else
			{
				// need to see if the parent is a bundle or not
				$isBundle = ($parentMap[$item->getParentItemId()] == Mage_Catalog_Model_Product_Type::TYPE_BUNDLE);
				if ($isBundle)
				{
					// it's a bundle member - price and weight come from the bundle definition itself
					$unitPrice = 0;
					$weight = 0;
				}
				else
				{ 
					// don't even want to include if the parent item is anything but a bundle
					continue;
				}
			}
		}

		// Magento 1.4+ has Cost
		$unitCost = 0;
		if (MagentoVersionGreaterOrEqualTo('1.4.0') && $item->getBaseCost() > 0)
		{
			$unitCost = $item->getBaseCost();
		}
		else if (MagentoVersionGreaterOrEqualTo('1.3.0'))
		{
			// Magento 1.3 didn't seem to copy Cost to the item from the product
			// fallback to the Cost defined on the product.

			$product = Mage::getModel('catalog/product');
			$productId = $item->getProductId();
			$product->load($productId);

			if ($product->getCost() > 0)
			{
				$unitCost = $product->getCost();
			}
		}

		writeStartTag("Item");

		writeElement("ItemID", $item->getItemId());
		writeElement("ProductID", $item->getProductId());
		writeElement("Code", $sku);
		writeElement("SKU", $sku);
		writeElement("Name", $name);
		writeElement("Quantity", (int)$item->getQtyOrdered());
		writeElement("UnitPrice", $unitPrice);
		writeElement("UnitCost", $unitCost);

		if (!$weight)
		{
			$weight = 0;
		}
		writeElement("Weight", $weight);


		writeStartTag("Attributes");
		$opt = $item->getProductOptions();
		if ($item->getProductType() == Mage_Catalog_Model_Product_Type::TYPE_CONFIGURABLE)
		{
			if (is_array($opt) &&
				isset($opt['attributes_info']) &&
				is_array($opt['attributes_info']) &&
				is_array($opt['info_buyRequest']) &&
				is_array($opt['info_buyRequest']['super_attribute']))
			{
				$attr_id = $opt['info_buyRequest']['super_attribute'];
				reset($attr_id);
				foreach ($opt['attributes_info'] as $sub)
				{
					writeStartTag("Attribute");
					writeElement("Name", $sub['label']);
					writeElement("Value", $sub['value']);
					writeCloseTag("Attribute");

					next($attr_id);
				}
			}
		}

		if (is_array($opt) &&
			isset($opt['options']) &&
			is_array($opt['options']))
		{
			foreach ($opt['options'] as $sub)
			{
				writeStartTag("Attribute");
				writeElement("Name", $sub['label']);
				writeElement("Value", $sub['value']);
				writeCloseTag("Attribute");
			}
		}

		// Order-item level Gift Messages are created as item attributes in ShipWorks
		if ($item->getGiftMessageId())
		{
			$message = Mage::helper('giftmessage/message')->getGiftMessage($item->getGiftMessageId());

			// write the gift message as an attribute
			writeStartTag("Attribute");
			writeElement("Name", "Gift Message");
			writeelement("Value", $message['message']);
			writeCloseTag("Attribute");

			// write the gift messgae recipient as an attribute
			writeStartTag("Attribute");
			writeElement("Name", "Gift Message, Recipient");
			writeelement("Value", $message['recipient']);
			writeCloseTag("Attribute");
		}


		// Uncomment the following lines to include a custom product attribute in the downloaded data.
		// These will appear as Order Item Attributes in ShipWorks.
		//$product = Mage::getModel('catalog/product');
		//$productId = $product->getIdBySku($sku);
		//$product->load($productId);
		//$value = $product->getAttributeText("attribute_code_here");
		//if ($value)
		//{
		//     // write the gift message as an attribute
		//     writeStartTag("Attribute");
		//     writeElement("Name", "Attribute_title_here");
		//     writeelement("Value", $value);
		//     writeCloseTag("Attribute");
		//}

		writeCloseTag("Attributes");

		writeCloseTag("Item");
	}

	writeCloseTag("Items");
}

// Returns the status codes for the store
function Action_GetStatusCodes()
{
	writeStartTag("StatusCodes");

	$statuses_node                  = Mage::getConfig()->getNode('global/sales/order/statuses');

	foreach ($statuses_node->children() as $status)
	{
		writeStartTag("StatusCode");
		writeElement("Code", $status->getName());
		writeElement("Name", $status->label);
		writeCloseTag("StatusCode");
	}

	writeCloseTag("StatusCodes");
}

// Update the status of an order
function Action_UpdateOrder()
{
	// gather paramtetes
	if (!isset($_REQUEST['order'])  || 
		!isset($_REQUEST['command']) || !isset($_REQUEST['comments']))
	{
		outputError(40, "Insufficient parameters");
		return;
	}

	// newer version of ShipWorks, pull the entity id
	$orderID = (int)$_REQUEST['order'];
	$order = Mage::getModel('sales/order')->load($orderID);

	$command = (string) $_REQUEST['command'];
	$comments = $_REQUEST['comments'];
	$tracking = $_REQUEST['tracking'];
	$carrierData = $_REQUEST['carrier'];

	ExecuteOrderCommand($order, $command, $comments, $carrierData, $tracking);
}

// Takes the actions necessary to get an order to Complete
function CompleteOrder($order, $comments, $carrierData, $tracking)
{
	//Check to see if the order already shipments
	if($order->hasShipments())
	{
		//Order already has shipments
		$existingShipments = $order->getShipmentsCollection();
		
		//Grab the first shipment to add tracking to
		$shipment = $existingShipments->getFirstItem();
	}
	else 
	{
		//Order has no shipments
		$shipment = $order->prepareShipment();
		$shipment->register();
	}   
	
	//Do Shipment Stuff
	if ($shipment)
	{
		$shipment->addComment($comments, false);
		$order->setIsInProcess(true);

		// add tracking info if it was supplied
		if (strlen($tracking) > 0)
		{
			$track = Mage::getModel('sales/order_shipment_track')->setNumber($tracking);

			# carrier data is of the format code|title
			$carrierData = preg_split("[\|]", $carrierData);
			$track->setCarrierCode($carrierData[0]);
			$track->setTitle($carrierData[1]);

			$shipment->addTrack($track);
		}

		$transactionSave = Mage::getModel('core/resource_transaction')
			->addObject($shipment)
			->addObject($shipment->getOrder())
			->save();

		// send the email if it's requested
		if (isset($_REQUEST['sendemail']) && $_REQUEST['sendemail'] == '1')
		{
			$shipment->sendEmail(true);
		} 
	} 
	
	// invoice the order
	if ($order->hasInvoices())
	{
		// select the last invoice to attach the note to
		$invoice = $order->getInvoiceCollection()->getLastItem();
	}
	else
	{
		// prepare a brand-new invoice
		$invoice = $order->prepareInvoice();
		$invoice->register();	    
	}

	// capture the invoice if possible
	if ($invoice->canCapture())
	{
		$invoice->Capture();
	}

	// some magento versions prevent multiple pay() calls from have impact,
	// but others don't.  If pay is called multiple times, Order.Total Paid is off.
	if ($invoice->getState() != Mage_Sales_Model_Order_Invoice::STATE_PAID)
	{
		$invoice->pay();
	}

	// set the comment
	$invoice->addComment($comments);

	// save the new invoice
	$transactionSave = Mage::getModel('core/resource_transaction')
		->addObject($invoice)
		->addObject($invoice->getOrder());
	$transactionSave->save();

	// Saving will force magento to move the state/status
	$order->save();
}


// Changes the status of an order 
function ExecuteOrderCommand($order, $command, $comments, $carrierData, $tracking)
{
	try
	{
		// to change statuses, we need to unhold if necessary
		if ($order->canUnhold())
		{
			$order->unhold();
			$order->save();
		}

		switch (strtolower($command))
		{
			case "complete":
				CompleteOrder($order, $comments, $carrierData, $tracking);
				break;
			case "cancel":
				$order->cancel();
				$order->addStatusToHistory($order->getStatus(), $comments);
				$order->save();
				break;
			case "hold":
				$order->hold();
				$order->addStatusToHistory($order->getStatus(), $comments);
				$order->save();
				break;
			default:
				outputError(80, "Unknown command '$command' for order update");
				break;
		}

		writeStartTag("Debug");
		writeElement("OrderStatus", $order->getStatus());
		writeCloseTag("Debug");
	}
	catch (Exception $ex)
	{
		outputError(90, "Unknown error while executing command: " . $ex->getMessage());
	}
}

// Converts a sql data string to xml date format
function FormatDate($dateSql)
{
	$pattern = "/^(\d{4})-(\d{2})-(\d{2})\s+(\d{2}):(\d{2}):(\d{2})$/i";

	if (preg_match($pattern, $dateSql, $dt)) 
	{
		$dateUnix = mktime($dt[4], $dt[5], $dt[6], $dt[2], $dt[3], $dt[1]);
		return gmdate("Y-m-d\TH:i:s", $dateUnix);
	}

	return $dateSql;
}

// end output
ob_end_flush();
?>
