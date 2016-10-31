<?php

/**
 * Class Interapptive_ShipWorks_Model_Order_XMLWriter
 *
 * @category ShipWorks
 * @package  Interapptive\ShipWorks
 * @author   ShipWorks <support@shipworks.com>
 * @license  www.shipworks.com Commercial License
 * @link     www.shipworks.com
 */
class Interapptive_ShipWorks_Model_Order_XMLWriter
{
    /**
     * Takes and returns an order in ShipWorks schema 1.1.0 XML
     *
     * @param Mage_Sales_Model_Resource_Order $order The order to write to xml
     *
     * @return DOMNode
     */
    public function writeOrder($order)
    {
        $helper = Mage::helper("ShipWorks");

        $storeId = $order->getStoreId();

        //Generate the Order XML
        $orderXML = new SimpleXMLElement("<Order/>");

        $incrementId = $order->getIncrementId();

        $orderPrefix = '';
        $orderNumber = '';
        $orderPostfix = '';

        $numberArray = str_split($incrementId);

        foreach ($numberArray as $orderNumberPart) {
            if (!is_numeric($orderNumberPart) && $orderNumber == '') {
                $orderPrefix .= $orderNumberPart;
            } elseif (is_numeric($orderNumberPart) && $orderPostfix == '') {
                $orderNumber .= $orderNumberPart;
            } elseif ($orderNumber != '') {
                $orderPostfix .= $orderNumberPart;
            }
        }

        $orderXML->addChild("OrderNumber", $orderNumber);
        $orderXML->addChild("OrderNumberPrefix", $orderPrefix);
        $orderXML->addChild("OrderNumberPostfix", $orderPostfix);
        $orderXML->addChild("OrderDate", $order->getCreatedAt());
        $orderXML->addChild("LastModified", $order->getUpdatedAt());
        $orderXML->addChild("ShippingMethod", $order->getShippingDescription());
        $orderXML->addChild("StatusCode", $order->getStatus());
        $orderXML->addChild("CustomerID", $order->getCustomerId());

        $orderXML->addChild("Debug");
        $orderXML->Debug->addChild("OrderID", $order->getId());

        // Create a new document
        $orderDOM = new DOMDocument;
        $orderDOM->formatOutput = true;

        // Load XML
        $orderDOM->loadXML($orderXML->asXML());

        // Order Notes
        $helper->addToXML($orderDOM, $this->writeOrderNotes($order));

        // Add Shipping Node into the order dom
        $helper->addToXML(
            $orderDOM, $this->writeOrderAddress(
            $order, "BillingAddress"
        )
        );

        // Add Billing Node into the order dom
        $helper->addToXML(
            $orderDOM, $this->writeOrderAddress($order, "ShippingAddress"
        )
        );

        //Payment Information
        $helper->addToXML(
            $orderDOM, $this->writeOrderPayment($order->getPayment())
        );

        //Order Totals
        $helper->addToXML($orderDOM, $this->writeOrderTotals($order));

        //Order Items
        $helper->addToXML(
            $orderDOM, $this->writeOrderItems($order->getAllItems(), $storeId)
        );

        return $orderDOM->getElementsByTagName("Order")->item(0);
    }


    /**
     * Takes an order and returns Notes element containing a collection of
     * individual Note Complex types from ShipWorks schema 1.1.0 XSD
     *
     * @param Mage_Sales_Model_Resource_Order $order
     *
     * @return DOMNode
     */
    private function writeOrderNotes($order)
    {
        $helper = Mage::helper("ShipWorks");

        // Create a new document
        $notesDOM = new DOMDocument;
        $notesDOM->formatOutput = true;

        // Load Root XML Element
        $notesDOM->loadXML("<Notes/>");

        //Order Gift Message
        // check for order-level gift messages
        if ($order->getGiftMessageId()) {
            $message = Mage::helper('giftmessage/message')->getGiftMessage(
                $order->getGiftMessageId()
            );
            $messageString = "Gift message for " . $message['recipient'] . ": "
                . $message['message'];

            $helper->addToXML(
                $notesDOM, $this->writeOrderNote($messageString, true)
            );
        }

        return $notesDOM->getElementsByTagName("Notes")->item(0);
    }

    /**
     * Returns the Note complex type from ShipWorks 1.1.0 XSD
     *
     * @param string $note   the note to add
     * @param bool   $public should the note be visible publicly
     *
     * @return DOMNode
     */
    private function writeOrderNote($note, $public = false)
    {
        $public = ($public) ? 'true' : 'false';

        $note = htmlspecialchars($note);

        $noteXML = new SimpleXMLElement("<Note>$note</Note>");
        $noteXML->addAttribute("public", $public);

        $noteDom = new DOMDocument();
        $noteDom->loadXML($noteXML->asXML());

        return $noteDom->getElementsByTagName("Note")->item(0);
    }

    /**
     * Writes order payment element from ShipWorks Schema 1.1.0 XSD
     *
     * @param $payment
     *
     * @return DOMNode
     */
    private function writeOrderPayment($payment)
    {
        $paymentXML = new SimpleXMLElement("<Payment/>");

        $paymentXML->addChild(
            "Method",
            Mage::helper('payment')->getMethodInstance($payment->getMethod())
                ->getTitle()
        );

        // CC info
        $cc_num = $payment->getCcLast4();
        if (!empty($cc_num)) {
            $cc_num = '************' . $payment->getCcLast4();
        }
        $cc_year = sprintf(
            '%02u%s', $payment->getCcExpMonth(),
            substr($payment->getCcExpYear(), 2)
        );

        $paymentXML->addChild("CreditCard");
        $paymentXML->CreditCard->addChild("Type", $payment->getCcType());
        $paymentXML->CreditCard->addChild("Type", $payment->getCcOwner());
        $paymentXML->CreditCard->addChild("Type", $cc_num);
        $paymentXML->CreditCard->addChild("Type", $cc_year);

        $Dom = new DOMDocument();

        $Dom->loadXML($paymentXML->asXML());

        return $Dom->getElementsByTagName("Payment")->item(0);
    }

    /**
     * @param $order
     *
     * @return DOMNode
     */
    private function writeOrderTotals($order)
    {
        $helper = Mage::helper("ShipWorks");

        // Create a new document
        $totalsDOM = new DOMDocument;
        $totalsDOM->formatOutput = true;

        // Load XML
        $totalsDOM->loadXML("<Totals/>");

        //Order Subtotal
        $helper->addToXML(
            $totalsDOM, $this->writeOrderTotal(
            "Order Subtotal", $order->getSubtotal(), "ot_subtotal", "none"
        )
        );

        //Order Shipping and Handling
        $helper->addToXML(
            $totalsDOM, $this->writeOrderTotal(
            "Shipping and Handling", $order->getShippingAmount(), "shipping",
            "add"
        )
        );

        //TAX
        if ($order->getTaxAmount() > 0) {
            $helper->addToXML(
                $totalsDOM, $this->writeOrderTotal(
                "Tax", $order->getTaxAmount(), "tax", "add"
            )
            );
        }

        //Discounts
        // Magento 1.4 started storing discounts as negative values
        if (version_compare(Mage::getVersion(), "1.4.0") >= 0
            && $order->getDiscountAmount() < 0
        ) {
            $couponCode = $order->getCouponCode();
            $helper->addToXML(
                $totalsDOM, $this->writeOrderTotal(
                "Discount ($couponCode)", -1 * $order->getDiscountAmount(),
                "discount", "subtract"
            )
            );
        }

        if (version_compare(Mage::getVersion(), "1.4.0") < 0
            && $order->getDiscountAmount() > 0
        ) {
            $couponCode = $order->getCouponCode();
            $helper->addToXML(
                $totalsDOM, $this->writeOrderTotal(
                "Discount ($couponCode)", $order->getDiscountAmount(),
                "discount", "subtract"
            )
            );
        }

        if ($order->getGiftcertAmount() > 0) {
            $helper->addToXML(
                $totalsDOM, $this->writeOrderTotal(
                "Gift Certificate", $order->getGiftcertAmount(),
                "giftcertificate", "subtract"
            )
            );
        }

        if ($order->getAdjustmentPositive()) {
            $helper->addToXML(
                $totalsDOM, $this->writeOrderTotal(
                "Adjustment Refund", $order->getAdjustmentPositive(), "refund",
                "subtract"
            )
            );
        }

        if ($order->getAdjustmentNegative()) {
            $helper->addToXML(
                $totalsDOM, $this->writeOrderTotal(
                "Adjustment Fee", $order->getAdjustmentPositive(), "fee", "add"
            )
            );
        }

        $helper->addToXML(
            $totalsDOM, $this->writeOrderTotal(
            "Grand Total", $order->getGrandTotal(), "total", "none"
        )
        );

        return $totalsDOM->getElementsByTagName("Totals")->item(0);

    }

    /**
     * @param        $name
     * @param        $value
     * @param        $class
     * @param string $impact
     *
     * @return DOMNode
     */
    private function writeOrderTotal($name, $value, $class, $impact = "add")
    {

        $totalXML = new SimpleXMLElement("<Total>$value</Total>");

        $totalXML->addAttribute("name", $name);
        $totalXML->addAttribute("class", $class);
        $totalXML->addAttribute("impact", $impact);

        $Dom = new DOMDocument();

        $Dom->loadXML($totalXML->asXML());

        return $Dom->getElementsByTagName("Total")->item(0);
    }

    /**
     * Writes the order items xml node
     *
     * @param $orderItems
     * @param $storeId
     *
     * @return DOMNode
     */
    private function writeOrderItems($orderItems, $storeId)
    {
        $helper = Mage::helper('ShipWorks');
        $customAttributes = $helper->getStoreConfig($storeId);
        $attributes = Mage::getModel('eav/config')
            ->getEntityType(Mage_Catalog_Model_Product::ENTITY)->getAttributeCollection();

        $productIds = array();
        foreach ($orderItems as $item) {
            $productIds[] = $item->getProductId();
        }

        $productCollection = Mage::getModel('catalog/product')->getCollection()
            ->addAttributeToSelect(array_values($customAttributes))
            ->addIdFilter($productIds)
            ->load();

        // Create a new document
        $itemsDOM = new DOMDocument;
        $itemsDOM->formatOutput = true;

        // Load XML
        $itemsDOM->loadXML('<Items/>');

        foreach ($orderItems as $item) {
            $product = $productCollection->getItemById($item->getProductId());

            // keep track of item Id and types
            $parentMap[$item->getItemId()] = $item->getProductType();

            // get the sku
            if ($item->getProductType()
                == Mage_Catalog_Model_Product_Type::TYPE_CONFIGURABLE
            ) {
                $sku = $item->getProductOptionByCode('simple_sku');
            } else {
                $sku = $item->getSku();
            }

            // weights are handled differently if the item is a bundle or part of a bundle
            $weight = $item->getWeight();
            if ($item->getIsVirtual()) {
                $weight = 0;
            }

            if ($item->getProductType()
                == Mage_Catalog_Model_Product_Type::TYPE_BUNDLE
            ) {
                $name = $item->getName() . ' ' . '(bundle)';
                $unitPrice = $this->getCalculationPrice($item);
            } else {
                $name = $item->getName();

                // if it's part of a bundle
                if (is_null($item->getParentItemId())) {
                    $unitPrice = $this->getCalculationPrice($item);
                } else {
                    // need to see if the parent is a bundle or not
                    $isBundle = ($parentMap[$item->getParentItemId()]
                        == Mage_Catalog_Model_Product_Type::TYPE_BUNDLE);
                    if ($isBundle) {
                        // it's a bundle member - price and weight come from the bundle definition itself
                        $unitPrice = 0;
                        $weight = 0;
                    } else {
                        // don't even want to include if the parent item is anything but a bundle
                        continue;
                    }
                }
            }

            // Magento 1.4+ has Cost
            $unitCost = 0;
			
            if (version_compare(Mage::getVersion(), '1.4.0') >= 0 && $item->getBaseCost() > 0) 
			{
                $unitCost = $item->getBaseCost();
            } else {
                if (version_compare(Mage::getVersion(), '1.3.0') >= 0) 
				{
                    // Magento 1.3 didn't seem to copy Cost to the item from the product
                    // fallback to the Cost defined on the product.
                    if ($product->getCost() > 0) 
					{
                        $unitCost = $product->getCost();
                    }
                }
            }

            $itemXML = new SimpleXMLElement('<Item/>');

            $itemXML->addChild('ItemID', $item->getItemId());
            $itemXML->addChild('ProductId', $item->getProductId());

            if ($customAttributes['Code'] != '') {
                $codeCode = $customAttributes['Code'];

                $attributeText = $product->getAttributeText($codeCode);
                if($attributeText == ''){
                    $attributeText = $product->getData($codeCode);
                }
                $itemXML->addChild("Code", $attributeText);
            }

            if ($customAttributes['Location'] != '') {
                $codeLocation = $customAttributes['Location'];

                $attributeText = $product->getAttributeText($codeLocation);
                if($attributeText == ''){
                    $attributeText = $product->getData($codeLocation);
                }
                $itemXML->addChild("Location", $attributeText);
            }

            $itemXML->addChild('SKU', $sku);
            $itemXML->addChild('Name', $name);
            $itemXML->addChild('Quantity', (int)$item->getQtyOrdered());
            $itemXML->addChild('UnitPrice', $unitPrice);
            $itemXML->addChild('UnitCost', $unitCost);

            if (!$weight) {
                $weight = 0;
            }
            $itemXML->addChild('Weight', $weight);

            // Create a new document
            $itemDOM = new DOMDocument;
            $itemDOM->formatOutput = true;

            // Load XML
            $itemDOM->loadXML($itemXML->asXML());

            $helper->addToXML(
                $itemDOM, $this->writeItemAttributes($item, $product, $attributes, $customAttributes)
            );

            $helper->addToXML(
                $itemsDOM, $itemDOM->getElementsByTagName('Item')->item(0)
            );

            $product->clearInstance();
        }
        return $itemsDOM->getElementsByTagName('Items')->item(0);
    }


    /**
     * Writes the item attributes node
     *
     * @param $item
     * @param $product
     * @param $attributes
     * @param $customAttributes
     *
     * @return DOMNode
     */
    private function writeItemAttributes($item, $product, $attributes, $customAttributes)
    {
        $helper = Mage::helper('ShipWorks');

        // Create a new document
        $attributesDOM = new DOMDocument;
        $attributesDOM->formatOutput = true;

        // Load XML
        $attributesDOM->loadXML('<Attributes/>');

        $attributes->addFieldToFilter(
            'attribute_code', array_values($customAttributes)
        );

        foreach ($attributes as $attribute) {
            if ($attribute->getFrontendLabel() != ''
                && $product->getData($attribute->getAttributeCode()) != ''
            ) {
                $attributeText = $product->getAttributeText($attribute->getAttributeCode());
                if($attributeText == ''){
                    $attributeText = $product->getData($attribute->getAttributeCode());
                }
                $helper->addToXML(
                    $attributesDOM,
                    $this->writeItemAttribute(
                        $attribute->getFrontendLabel(),
                        $attributeText
                    )
                );
            }
        }

        $attributes->clear();

        $opt = $item->getProductOptions();
        if ($item->getProductType()
            == Mage_Catalog_Model_Product_Type::TYPE_CONFIGURABLE
        ) {
            if (is_array($opt) && isset($opt['attributes_info'])
                && is_array(
                    $opt['attributes_info']
                )
                && is_array($opt['info_buyRequest'])
                && is_array($opt['info_buyRequest']['super_attribute'])
            ) {
                $attr_id = $opt['info_buyRequest']['super_attribute'];
                reset($attr_id);
                foreach ($opt['attributes_info'] as $sub) {
                    $helper->addToXML(
                        $attributesDOM,
                        $this->writeItemAttribute($sub['label'], $sub['value'])
                    );
                    next($attr_id);
                }
            }
        }

        if (is_array($opt) && isset($opt['options'])
            && is_array(
                $opt['options']
            )
        ) {
            foreach ($opt['options'] as $sub) {
                $helper->addToXML(
                    $attributesDOM,
                    $this->writeItemAttribute($sub['label'], $sub['value'])
                );
            }
        }

        // Order-item level Gift Messages are created as item attributes in ShipWorks
        if ($item->getGiftMessageId()) {
            $message = Mage::helper('giftmessage/message')->getGiftMessage(
                $item->getGiftMessageId()
            );

            // write the gift message as an attribute
            $helper->addToXML(
                $attributesDOM,
                $this->writeItemAttribute('Gift Message', $message['message'])
            );

            // write the gift messgae recipient as an attribute
            $helper->addToXML(
                $attributesDOM, $this->writeItemAttribute(
                'Gift Message, Recipient', $message['recipient']
            )
            );

        }

        return $attributesDOM->getElementsByTagName('Attributes')->item(0);
    }

    /**
     * Writes an individual Item Attribute Node
     *
     * @param string $name  attribute name
     * @param string $value attribute value
     * @param int    $price attribute price
     * @param string $debug debug information
     *
     * @return DOMNode
     */
    private function writeItemAttribute($name, $value, $price = 0, $debug = '')
    {
        $attributeXML = new SimpleXMLElement('<Attribute/>');

        $attributeXML->addChild('Name', $name);
        $attributeXML->addChild('Value', $value);

        if ($price > 0) {
            $attributeXML->addChild('Price', $price);
        }

        if ($debug != '') {
            $attributeXML->addChild('Debug', $debug);
        }

        $attributeDom = new DOMDocument();

        $attributeDom->loadXML($attributeXML->asXML());

        return $attributeDom->getElementsByTagName('Attribute')->item(0);
    }


    /**
     * Gets the price of an order item
     *
     * @param $item
     *
     * @return float|int
     */
    private function getCalculationPrice($item)
    {
        if ($item instanceof Mage_Sales_Model_Order_Item) {
            if (version_compare(Mage::getVersion(), '1.3.0') >= 0) {
                return $item->getPrice();
            } else {
                if ($item->hasCustomPrice()) {
                    return $item->getCustomPrice();
                } else {
                    if ($item->hasOriginalPrice()) {
                        return $item->getOriginalPrice();
                    }
                }
            }
        }

        return 0;
    }

    /**
     * writes an address node
     *
     * @param $order
     * @param $rootElement
     *
     * @return DOMNode
     */
    private function writeOrderAddress($order, $rootElement)
    {

        $addressXML = new SimpleXMLElement("<$rootElement/>");

        $address = $order->getShippingAddress();

        // sometimes the shipping address isn't specified, so use billing
        // Or we are writing the billing address so we use billing
        if (!$address || $rootElement == "BillingAddress")
        {
            $address = $order->getBillingAddress();
        }

        $billFullName = $order->getBillingAddress()->getName();
        $billStreet1 = $order->getBillingAddress()->getStreet(1);
        $billCity = $order->getBillingAddress()->getCity();
        $billZip = $order->getBillingAddress()->getPostcode();

        $addressXML->addChild('FullName', $address->getName());
        $addressXML->addChild('Company', $address->getCompany());
        $addressXML->addChild('Street1', $address->getStreet(1));
        $addressXML->addChild('Street2', $address->getStreet(2));
        $addressXML->addChild('Street3', $address->getStreet(3));
        $addressXML->addChild('City', $address->getCity());
        $addressXML->addChild('State', $address->getRegionCode());
        $addressXML->addChild('PostalCode', $address->getPostcode());
        $addressXML->addChild('Country', $address->getCountryId());
        $addressXML->addChild('Phone', $address->getTelephone());

        // if the addressses appear to be the same, use customer email as shipping email too
        if ($address->getName() == $billFullName &&
            $address->getStreet(1) == $billStreet1 &&
            $address->getCity() == $billCity &&
            $address->getPostcode() == $billZip ||
            $rootElement == 'BillingAddress')
        {
            $addressXML->addChild('Email', $order->getCustomerEmail());
        }

        $Dom = new DOMDocument();

        $Dom->loadXML($addressXML->asXML());

        return $Dom->getElementsByTagName($rootElement)->item(0);
    }

}