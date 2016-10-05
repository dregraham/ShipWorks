<?php

error_reporting(E_ALL);

class Interapptive_ShipWorks_Model_ObjectModel_Api extends Mage_Api_Model_Resource_Abstract
{
	public $output = '';
	public $schemaVersion = "1.0.0";

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


	// Function used to output an error and quit.
	function outputError($code, $error)
	{       
		$this->writeStartTag("Error");
		$this->writeElement("Code", $code);
		$this->writeElement("Description", $error);
		$this->writeCloseTag("Error");
	}       


	// Writes the start xml tag
	public function writeStartTag($tag, $attributes = null)
	{
		$this->output .= '<' . $tag;

		if ($attributes != null)
		{
			$this->output .= ' ';

			foreach ($attributes as $name => $attribValue)
			{
				$this->output .= $name. '="'. htmlspecialchars($attribValue). '" ';	
			}
		}

		$this->output .= '>';
	}

	// write xml documenta declaration
	function writeXmlDeclaration()
	{
		$this->output .= "<?xml version=\"1.0\" standalone=\"yes\" ?>";
	}

	// write closing xml tag
	function writeCloseTag($tag)
	{
		$this->output .= '</' . $tag . '>';
	}

	// Output the given tag\value pair
	function writeElement($tag, $value)
	{
		$this->writeStartTag($tag);
		$this->output .= htmlspecialchars($value);
		$this->writeCloseTag($tag);
	}

	// Outputs the given name/value pair as an xml tag with attributes
	function writeFullElement($tag, $value, $attributes)
	{
		$this->output .= '<'. $tag. ' ';

		foreach ($attributes as $name => $attribValue)
		{
			$this->output .= $name. '="'. htmlspecialchars($attribValue). '" ';	
		}
		$this->output .= '>';
		$this->output .= htmlspecialchars($value);
		
		$this->writeCloseTag($tag);
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

	
		
	// writes the opening ShipWorks tag
	public function writeShipWorksStart()
	{
		$version = Mage::getModel("ShipWorks/Version")->getModuleVersion();
		
		$this->writeStartTag("ShipWorks", array("moduleVersion" => $version, "schemaVersion" => $this->schemaVersion));
	}

	// gets module capabilities
	public function getModule()
	{
		$this->output = '';

		$this->writeXmlDeclaration();
		$this->writeShipWorksStart();

		$this->writeStartTag("Module");

		$this->writeElement("Platform", "Magento");
		$this->writeElement("Developer", "Interapptive, Inc. (support@interapptive.com)");

		$this->writeStartTag("Capabilities");
		$this->writeElement("DownloadStrategy", "ByModifiedTime");
		$this->writeFullElement("OnlineCustomerID", "", array("supported" => "true", "dataType" => "numeric"));
		$this->writeFullElement("OnlineStatus", "", array("supported" => "true", "dataType" => "text", "downloadOnly" => "true" ));
		$this->writeFullElement("OnlineShipmentUpdate", "", array("supported" => "false"));
		$this->writeCloseTag("Capabilities");

		$this->writeCloseTag("Module");
		$this->writeCloseTag("ShipWorks");

		return $this->output;
	}

	// Write store data
	public function getStore()
	{
		$this->output = '';

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

		$this->writeShipWorksStart();
		$this->writeStartTag("Store");
		$this->writeElement("Name", $name);
		$this->writeElement("CompanyOrOwner", $owner);
		$this->writeElement("Email", $email);
		$this->writeElement("State", $state);
		$this->writeElement("Country", $country);
		$this->writeElement("Website", $website);
		$this->writeCloseTag("Store");

		$this->writeCloseTag("ShipWorks");

		return $this->output;
	}

	// goes from a store Code to its ID
	function StoreIdFromCode($storeCode)
	{
		if ($storeCode == '')
		{
			return Mage::app()->getDefaultStoreView()->getId();
		}
		foreach (Mage::app()->getStores() as $store)
		{
			if ($store->getCode() == $storeCode)
			{
				return $store->getId();
			}
		}

		return 0;
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

	// Write XML for all products for the given order
	function WriteOrderItems($orderItems)
	{
		$this->writeStartTag("Items");

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
				$unitPrice = $this->getCalculationPrice($item);
			}
			else
			{
				$name = $item->getName();

				// if it's part of a bundle
				if (is_null($item->getParentItemId()))
				{
					$unitPrice = $this->getCalculationPrice($item);
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
			if ($this->MagentoVersionGreaterOrEqualTo('1.4.0') && $item->getBaseCost() > 0)
			{
				$unitCost = $item->getBaseCost();
			}
			else if ($this->MagentoVersionGreaterOrEqualTo('1.3.0'))
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

			$this->writeStartTag("Item");

			$this->writeElement("ItemID", $item->getItemId());
			$this->writeElement("ProductID", $item->getProductId());
			$this->writeElement("Code", $sku);
			$this->writeElement("SKU", $sku);
			$this->writeElement("Name", $name);
			$this->writeElement("Quantity", (int)$item->getQtyOrdered());
			$this->writeElement("UnitPrice", $unitPrice);
			$this->writeElement("UnitCost", $unitCost);

			if (!$weight)
			{
				$weight = 0;
			}
			$this->writeElement("Weight", $weight);


			$this->writeStartTag("Attributes");
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
						$this->writeStartTag("Attribute");
						$this->writeElement("Name", $sub['label']);
						$this->writeElement("Value", $sub['value']);
						$this->writeCloseTag("Attribute");

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
					$this->writeStartTag("Attribute");
					$this->writeElement("Name", $sub['label']);
					$this->writeElement("Value", $sub['value']);
					$this->writeCloseTag("Attribute");
				}
			}

			// Order-item level Gift Messages are created as item attributes in ShipWorks
			if ($item->getGiftMessageId())
			{
				$message = Mage::helper('giftmessage/message')->getGiftMessage($item->getGiftMessageId());

				// write the gift message as an attribute
				$this->writeStartTag("Attribute");
				$this->writeElement("Name", "Gift Message");
				$this->writeelement("Value", $message['message']);
				$this->writeCloseTag("Attribute");

				// write the gift messgae recipient as an attribute
				$this->writeStartTag("Attribute");
				$this->writeElement("Name", "Gift Message, Recipient");
				$this->writeelement("Value", $message['recipient']);
				$this->writeCloseTag("Attribute");
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

			$this->writeCloseTag("Attributes");

			$this->writeCloseTag("Item");
		}

		$this->writeCloseTag("Items");
	}

	// Write all totals lines for the order
	function WriteOrderTotals($order)
	{
		$this->writeStartTag("Totals");

		$this->WriteOrderTotal("Order Subtotal", $order->getSubtotal(), "ot_subtotal", "none");
		$this->WriteOrderTotal("Shipping and Handling", $order->getShippingAmount(), "shipping", "add");

		if ($order->getTaxAmount() > 0)
		{
			$this->WriteOrderTotal("Tax", $order->getTaxAmount(), "tax", "add");
		}

		// Magento 1.4 started storing discounts as negative values
		if ($this->MagentoVersionGreaterOrEqualTo('1.4.0') && $order->getDiscountAmount() < 0)
		{
			$couponcode = $order->getCouponCode();
			$this->WriteOrderTotal("Discount ($couponcode)", -1 * $order->getDiscountAmount(), "discount", "subtract");
		}

		if (!$this->MagentoVersionGreaterOrEqualTo('1.4.0') && $order->getDiscountAmount() > 0)
		{
			$couponcode = $order->getCouponCode();
			$this->WriteOrderTotal("Discount ($couponcode)", $order->getDiscountAmount(), "discount", "subtract");
		}

		if ($order->getGiftcertAmount() > 0)
		{
			$this->WriteOrderTotal("Gift Certificate", $order->getGiftcertAmount(), "giftcertificate", "subtract");
		}

		if ($order->getAdjustmentPositive())
		{
			$this->WriteOrderTotal("Adjustment Refund", $order->getAdjustmentPositive(), "refund", "subtract");
		}

		if ($order->getAdjustmentNegative())
		{
			$this->WriteOrderTotal("Adjustment Fee", $order->getAdjustmentPositive(), "fee", "add");
		}

		$this->WriteOrderTotal("Grand Total", $order->getGrandTotal(), "total", "none");

		$this->writeCloseTag("Totals");
	}

	// writes a single order total
	function WriteOrderTotal($name, $value, $class, $impact = "add")
	{
		if ($value > 0)
		{
			$this->writeFullElement("Total", $value, array("name" => $name, "class" => $class, "impact" => $impact));
		}
	}
	// Gets the price of an order item
	function getCalculationPrice($item)
	{
		if ($item instanceof Mage_Sales_Model_Order_Item)
		{ 
			if ($this->MagentoVersionGreaterOrEqualTo('1.3.0'))
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



	// Output the order as xml
	function WriteOrder($order)
	{                 
		$this->writeStartTag("Order");

		$incrementId = $order->getIncrementId();
		$orderPostfix = '';
		$parts = preg_split('[\-]', $incrementId, 2);

		if (count($parts) == 2)
		{
			$incrementId = $parts[0];
			$orderPostfix = $parts[1];
		}

		$this->writeElement("OrderNumber", $incrementId);
		$this->writeElement("OrderDate", $this->FormatDate($order->getCreatedAt()));
		$this->writeElement("LastModified", $this->FormatDate($order->getUpdatedAt()));
		$this->writeElement("ShippingMethod", $order->getShippingDescription());
		$this->writeElement("StatusCode", $order->getStatus());
		$this->writeElement("CustomerID", $order->getCustomerId());

		// check for order-level gift messages
		if ($order->getGiftMessageId())
		{
			$message = Mage::helper('giftmessage/message')->getGiftMessage($order->getGiftMessageId());
			$messageString = "Gift message for ". $message['recipient']. ": ". $message['message'];

			$this->writeStartTag("Notes");
			$this->writeFullElement("Note", $messageString, array("public" => "true"));
			$this->writeCloseTag("Notes");
		}

		$address = $order->getBillingAddress();
		$this->writeStartTag("BillingAddress");
		$this->writeElement("FullName", $address->getName());
		$this->writeElement("Company", $address->getCompany());
		$this->writeElement("Street1", $address->getStreet(1));
		$this->writeElement("Street2", $address->getStreet(2));
		$this->writeElement("Street3", $address->getStreet(3));
		$this->writeElement("City", $address->getCity());
		$this->writeElement("State", $address->getRegionCode());
		$this->writeElement("PostalCode", $address->getPostcode());
		$this->writeElement("Country", $address->getCountryId());
		$this->writeElement("Phone", $address->getTelephone());
		$this->writeElement("Email", $order->getCustomerEmail());
		$this->writeCloseTag("BillingAddress");

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

		$this->writeStartTag("ShippingAddress");
		$this->writeElement("FullName", $address->getName());
		$this->writeElement("Company", $address->getCompany());
		$this->writeElement("Street1", $address->getStreet(1));
		$this->writeElement("Street2", $address->getStreet(2));
		$this->writeElement("Street3", $address->getStreet(3));
		$this->writeElement("City", $address->getCity());
		$this->writeElement("State", $address->getRegionCode());
		$this->writeElement("PostalCode", $address->getPostcode());
		$this->writeElement("Country", $address->getCountryId());
		$this->writeElement("Phone", $address->getTelephone());

		// if the addressses appear to be the same, use customer email as shipping email too
		if ($address->getName() == $billFullName &&
				$address->getStreet(1) == $billStreet1 &&
				$address->getCity() == $billCity &&
				$address->getPostcode() == $billZip)
		{
			$this->writeElement("Email", $order->getCustomerEmail());
		}

		$this->writeCloseTag("ShippingAddress"); 

		$payment = $order->getPayment();

		// CC info
		$cc_num = $payment->getCcLast4();
		if (!empty($cc_num))
		{
			$cc_num = '************'.$payment->getCcLast4();
		}
		$cc_year = sprintf('%02u%s', $payment->getCcExpMonth(), substr($payment->getCcExpYear(), 2)); 


		$this->writeStartTag("Payment");
		$this->writeElement("Method", Mage::helper('payment')->getMethodInstance($payment->getMethod())->getTitle());

		$this->writeStartTag("CreditCard");
		$this->writeElement("Type", $payment->getCcType());
		$this->writeElement("Owner", $payment->getCcOwner());
		$this->writeElement("Number", $cc_num);
		$this->writeElement("Expires", $cc_year);
		$this->writeCloseTag("CreditCard");

		$this->writeCloseTag("Payment");

		$this->WriteOrderItems($order->getAllItems());
		$this->WriteOrderTotals($order);

		$this->writeStartTag("Debug");
		$this->writeElement("OrderID", $order->getEntityId());
		$this->writeElement("OrderNumberPostfix", $orderPostfix);
		$this->writeCloseTag("Debug");

		$this->writeCloseTag("Order");
	}

	// get orders
	public function getOrders($start, $maxCount, $storeCode)
	{
		$storeId = $this->StoreIdFromCode($storeCode);

		// Only get orders through 2 seconds ago.
		$end = date("Y-m-d H:i:s", time() - 2);

		// Convert to local SQL time
		$start = $this->toLocalSqlDate($start);

		// Write the params for easier diagnostics
		$this->writeXmlDeclaration();
		$this->writeShipWorksStart();
		$this->writeStartTag("Parameters");
		$this->writeElement("Start", $start);
		$this->writeElement("End", $end);
		$this->writeElement("MaxCount", $maxcount);
		$this->writeCloseTag("Parameters");                                    

		// setup the query
		$orders = Mage::getModel('sales/order')->getCollection();
		$orders->addAttributeToSelect("*")
			->getSelect()
			->where("(updated_at > '$start' AND updated_at <= '$end' AND store_id = $storeId)")
			->order('updated_at', 'asc');

		// configure paging
		$orders->setCurPage(1)
			->setPageSize($maxcount)
			->loadData();

		$this->writeStartTag("Orders");

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

			$this->WriteOrder($order);
		}

		// if we processed some orders we may have to get some more
		if ($processedIds != "")
		{
			$orders = Mage::getModel('sales/order')->getCollection();
			$orders->addAttributeToSelect("*")->getSelect()->where("updated_at = '$lastModified' AND entity_id not in ($processedIds) AND store_id = $storeId");

			foreach ($orders as $order)
			{
				$this->WriteOrder($order);
			}
		}

		$this->writeCloseTag("Orders");
		$this->writeCloseTag("ShipWorks");

		return $this->output;
	}

	// Takes the actions necessary to get an order to Complete
	function CompleteOrder($order, $comments, $carrierData, $tracking, $sendEmail)
	{
		// first create a shipment
		$shipment = $order->prepareShipment();
		if ($shipment)
		{
			$shipment->register();
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
			if ($sendEmail == '1')
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
		$this->writeElement("test", $order->getStatus());
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
					$this->CompleteOrder($order, $comments, $carrierData, $tracking);
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
					outputError(80, "Unknown order command '$command'.");
					break;
			}

			$this->writeStartTag("Debug");
			$this->writeElement("OrderStatus", $order->getStatus());
			$this->writeCloseTag("Debug");
		}
		catch (Exception $ex)
		{
			$this->outputError(90, "Error Executing Command. ".  $ex->getMessage());
		}
	}


	// Update the status of an order
	function updateOrder($order, $command, $comments, $tracking, $carrierData)
	{
		$this->output = '';
		
		$this->writeXmlDeclaration();
		$this->writeShipWorksStart();

		$this->writeStartTag("Parameters");
		$this->writeElement("Order", $order);
		$this->writeElement("Command", $command);
		$this->writeElement("Comments", $comments);
		$this->writeElement("Tracking", $tracking);
		$this->writeElement("CarrierData", $carrierData);
		$this->writeCloseTag("Parameters");

		// newer version of ShipWorks, pull the entity id
		$order = Mage::getModel('sales/order')->load($order);

		$this->ExecuteOrderCommand($order, $command, $comments, $carrierData, $tracking);

		$this->writeCloseTag("ShipWorks");
		return $this->output;
	}


	// gets status codes
	function getStatusCodes()
	{
		$this->writeXmlDeclaration();
		$this->writeShipWorksStart();
		$this->writeStartTag("StatusCodes");

		$statuses_node = Mage::getConfig()->getNode('global/sales/order/statuses');

		foreach ($statuses_node->children() as $status)
		{
			$this->writeStartTag("StatusCode");
			$this->writeElement("Code", $status->getName());
			$this->writeElement("Name", $status->label);
			$this->writeCloseTag("StatusCode");
		}

		$this->writeCloseTag("StatusCodes");
		$this->writeCloseTag("ShipWorks");

		return $this->output;
	}

	// Get the count of orders greater than the start ID
	public function getCount($start, $storeCode)
	{         
		$this->output = '';

		$storeId = $this->StoreIdFromCode($storeCode);

		// only get orders through 2 seconds ago
		$end = date("Y-m-d H:i:s", time() - 2);

		// Convert to local SQL time
		$start = $this->toLocalSqlDate($start);

		// Write the params for easier diagnostics
		$this->writeXmlDeclaration();
		$this->writeShipWorksStart();
		$this->writeStartTag("Parameters");
		$this->writeElement("Start", $start);
		$this->writeElement("StoreID", $storeId);
		$this->writeCloseTag("Parameters");

		$orders = Mage::getModel('sales/order')->getCollection();
		$orders->addAttributeToSelect("updated_at")->getSelect()->where("(updated_at > '$start' AND updated_at <= '$end' AND store_id = $storeId)");
		$count = $orders->count();

		$this->writeElement("OrderCount", $count);
		$this->writeCloseTag("ShipWorks");

		return $this->output;
	}
}
?>
