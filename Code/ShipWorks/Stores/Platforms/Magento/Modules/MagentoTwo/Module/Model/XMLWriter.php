<?php

namespace ShipWorks\Module\Model;

use \ShipWorks\Module\Api\WriterInterface;
use \Magento\GiftMessage\Api\OrderRepositoryInterface;
use \Magento\GiftMessage\Api\OrderItemRepositoryInterface;

/**
 * Class XMLWriter
 *
 * @category ShipWorks
 * @package  Shipworks\Module
 * @author   ShipWorks <support@shipworks.com>
 * @license  www.shipworks.com Commercial License
 * @link     www.shipworks.com
 */
class XMLWriter implements WriterInterface
{
    // Instance of \Magento\GiftMessage\Api\OrderRepositoryInterface
    protected $OrderGiftMessageRepository;

    // Instance of \Magento\GiftMessage\Api\OrderItemRepositoryInterface
    protected $OrderItemGiftMessageRepository;

    /**
     * XMLWriter constructor.
     * @param \Magento\GiftMessage\Api\OrderRepositoryInterface $orderGiftMessageRepository
     * @param \Magento\GiftMessage\Api\OrderItemRepositoryInterface $orderItemGiftMessageRepository
     */
    public function __construct(
        OrderRepositoryInterface $orderGiftMessageRepository,
        OrderItemRepositoryInterface $orderItemGiftMessageRepository
    ) {
        $this->OrderGiftMessageRepository = $orderGiftMessageRepository;
        $this->OrderItemGiftMessageRepository = $orderItemGiftMessageRepository;
    }

    /**
     * Takes an orders collection and returns orders
     * xml in ShipWorks schema 1.1.0 XML
     *
     * @param  int $count The order to write to xml
     * @return string orders xml
     */
    public function writeOrdersCount($count)
    {
        $result = (int)$count;

        return $this->processResult("<OrderCount>$result</OrderCount>", 'OrderCount');
    }

    /**
     * Takes an orders collection and returns orders
     * xml in ShipWorks schema 1.1.0 XML
     *
     * @param  \Magento\Sales\Api\Data\OrderInterface[] $orders The order to write to xml
     * @return string orders xml
     */
    public function writeOrders($orders)
    {
        // Create a new document
        $ordersDOM = new \DOMDocument;
        $ordersDOM->formatOutput = true;
        $ordersDOM->loadXML('<Orders/>');

        if ($orders != null) {
            foreach ($orders as $order) {
                // create the order node
                $orderNode = $this->writeOrder($order);
                
                // Add the node to the orders dom
                if ($orderNode instanceof \DOMNode) {
                    $this->addToXML($ordersDOM, $orderNode);
                }
            }
        }
        return $this->processResult($ordersDOM->saveXML(), 'Orders');
    }

    /**
     * Writes an exception
     *
     * @param \string $code
     * @param \string $message
     *
     * @return \string xml error
     */
    public function writeException($code, $message)
    {
        $errorXml = new \SimpleXMLElement("<Error/>");
        $errorXml->addChild("Code", $this->escapeXml($code));
        $errorXml->addChild("Description", $this->escapeXml($message));
        return $errorXml->saveXML();
    }

    /**
     * Writes an exception
     *
     * @param \Magento\Sales\Api\Data\OrderInterface $order
     * @param \string                                $message
     *
     * @return \string xml error
     */
    public function writeSuccess($order, $message = "")
    {
        $successXml = new \SimpleXMLElement("<Success/>");
        $successXml->addChild("OrderStatus", $this->escapeXml($order->getStatus()));
        $successXml->addChild("Message", $this->escapeXml($message));
        return $successXml->saveXML();
    }

    /**
     * returns a node for the given order
     *
     * @param   \Magento\Sales\Api\Data\OrderInterface $order
     * @returns \DOMNode order node
     */
    private function writeOrder($order)
    {
        //Generate the Order XML
        $orderXML = new \SimpleXMLElement("<Order/>");

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

        $orderDate = new \DateTime($order->getCreatedAt());
        $lastModified = new \DateTime($order->getUpdatedAt());

        $orderXML->addChild("OrderNumber", $this->escapeXml($orderNumber));
        $orderXML->addChild("OrderNumberPrefix", $this->escapeXml($orderPrefix));
        $orderXML->addChild("OrderNumberPostfix", $this->escapeXml($orderPostfix));
        $orderXML->addChild("OrderDate", $this->escapeXml($orderDate->format('Y-m-d\TH:i:s')));
        $orderXML->addChild("LastModified", $this->escapeXml($lastModified->format('Y-m-d\TH:i:s')));
        $orderXML->addChild("ShippingMethod", $this->escapeXml($order->getShippingDescription()));
        $orderXML->addChild("StatusCode", $this->escapeXml($order->getStatus()));
        $orderXML->addChild("CustomerID", $this->escapeXml($order->getCustomerId()));

        $orderXML->addChild("Debug");
        $orderXML->Debug->addChild("OrderID", $this->escapeXml($order->getEntityId()));

        // Create a new document
        $orderDOM = new \DOMDocument;
        $orderDOM->formatOutput = true;

        // Load XML
        $orderDOM->loadXML($orderXML->asXML());

        // Add Shipping Node into the order dom
        $this->addToXML($orderDOM, $this->writeOrderAddress($order, "BillingAddress"));

        // Add Billing Node into the order dom
        $this->addToXML($orderDOM, $this->writeOrderAddress($order, "ShippingAddress"));

        //Payment Information
        $this->addToXML($orderDOM, $this->writeOrderPayment($order->getPayment()));

        //Order Totals
        $this->addToXML($orderDOM, $this->writeOrderTotals($order));

        //Order Items
        $this->addToXML($orderDOM, $this->writeOrderItems($order->getItems()));

        $this->addToXML($orderDOM, $this->writeOrderNotes($order));

        return $orderDOM->getElementsByTagName("Order")->item(0);
    }

    /**
     * Takes an DOMDocument and imports DOMNode into it
     *
     * @param \DOMDocument $DOM  DOMDocument to import the node into
     * @param \DOMNode     $Node DOMNode to be imported
     *
     * @return \DOMDocument
     */
    private function addToXML($DOM, $Node)
    {
        $Node = $DOM->importNode($Node, true);
        $DOM->documentElement->appendChild($Node);

        return $DOM;
    }

    /**
     * writes an address node
     *
     * @param \Magento\Sales\Api\Data\OrderInterface $order
     * @param string                                 $rootElement
     *
     * @return \DOMNode
     */
    private function writeOrderAddress($order, $rootElement)
    {
        $addressXML = new \SimpleXMLElement("<$rootElement/>");

        $address = $order->getShippingAddress();

        // sometimes the shipping address isn't specified, so use billing
        // Or we are writing the billing address so we use billing
        if (!$address || $rootElement == "BillingAddress") {
            $address = $order->getBillingAddress();
        }

        $billLastName = $order->getBillingAddress()->getLastname();
        $billStreet = $order->getBillingAddress()->getStreet();
        $billStreet1 = isset($billStreet[0]) ? $billStreet[0] : "";
        $billCity = $order->getBillingAddress()->getCity();
        $billZip = $order->getBillingAddress()->getPostcode();

        $street = $address->getStreet();

        $street1 = isset($street[0]) ? $street[0] : "";
        $street2 = isset($street[1]) ? $street[1] : "";
        $street3 = isset($street[2]) ? $street[2] : "";

        $addressXML->addChild('FirstName', $this->escapeXml($address->getFirstname()));
        $addressXML->addChild('LastName', $this->escapeXml($address->getLastname()));
        $addressXML->addChild('Company', $this->escapeXml($address->getCompany()));
        $addressXML->addChild('Street1', $this->escapeXml($street1));
        $addressXML->addChild('Street2', $this->escapeXml($street2));
        $addressXML->addChild('Street3', $this->escapeXml($street3));
        $addressXML->addChild('City', $this->escapeXml($address->getCity()));
        $addressXML->addChild('State', $this->escapeXml($address->getRegionCode()));
        $addressXML->addChild('PostalCode', $this->escapeXml($address->getPostcode()));
        $addressXML->addChild('Country', $this->escapeXml($address->getCountryId()));
        $addressXML->addChild('Phone', $this->escapeXml($address->getTelephone()));
        $addressXML->addChild('Fax', $this->escapeXml($address->getFax()));

        // if the addresses appear to be the same, use customer email as shipping email too
        if ($address->getLastname() == $billLastName
            && $street1 == $billStreet1
            && $address->getCity() == $billCity
            && $address->getPostcode() == $billZip
            || $rootElement == 'BillingAddress'
        ) {
            $addressXML->addChild('Email', $this->escapeXml($order->getCustomerEmail()));
        }

        $Dom = new \DOMDocument();

        $Dom->loadXML($addressXML->asXML());

        return $Dom->getElementsByTagName($rootElement)->item(0);
    }

    /**
     * Writes order payment element from ShipWorks Schema 1.1.0 XSD
     *
     * @param \Magento\Sales\Api\Data\OrderPaymentInterface $payment
     *
     * @return \DOMNode
     */
    private function writeOrderPayment($payment)
    {
        $paymentXML = new \SimpleXMLElement("<Payment/>");

        $paymentXML->addChild("Method", $this->escapeXml($payment->getMethod()));

        // CC info
        $cc_num = $payment->getCcLast4();
        if (!empty($cc_num)) {
            $cc_num = '************' . $payment->getCcLast4();
        }
        $cc_year = sprintf('%02u%s', $payment->getCcExpMonth(), substr($payment->getCcExpYear(), 2));

        $paymentXML->addChild("CreditCard");
        $paymentXML->CreditCard->addChild("Type", $this->escapeXml($payment->getCcType()));
        $paymentXML->CreditCard->addChild("Owner", $this->escapeXml($payment->getCcOwner()));
        $paymentXML->CreditCard->addChild("Number", $this->escapeXml($cc_num));
        $paymentXML->CreditCard->addChild("Expires", $this->escapeXml($cc_year));

        $Dom = new \DOMDocument();

        $Dom->loadXML($paymentXML->asXML());

        return $Dom->getElementsByTagName("Payment")->item(0);
    }

    /**
     * Writes order total information
     *
     * @param \Magento\Sales\Api\Data\OrderInterface $order
     *
     * @return \DOMNode
     */
    private function writeOrderTotals($order)
    {
        // Create a new document
        $totalsDOM = new \DOMDocument;
        $totalsDOM->formatOutput = true;

        // Load XML
        $totalsDOM->loadXML("<Totals/>");

        //Order Subtotal
        $orderSubTotal = $this->writeOrderTotal("Order Subtotal", $order->getSubtotal(), "ot_subtotal", "none");
        $this->addToXML($totalsDOM, $orderSubTotal);

        //Order Shipping and Handling
        $shipping = $order->getShippingAmount();
        if ($shipping != null && $shipping > 0) {
            $shipping = $this->writeOrderTotal("Shipping and Handling", $order->getShippingAmount(), "shipping", "add");
            $this->addToXML($totalsDOM, $shipping);
        }

        //TAX
        if ($order->getTaxAmount() > 0) {
            $this->addToXML($totalsDOM, $this->writeOrderTotal("Tax", $order->getTaxAmount(), "tax", "add"));
        }

        if ($order->getAdjustmentPositive() != null) {
            $adjustPosAmount = $order->getAdjustmentPositive();
            $adjustPosNode = $this->writeOrderTotal("Adjustment Refund", $adjustPosAmount, "refund", "subtract");
            $this->addToXML($totalsDOM, $adjustPosNode);
        }

        if ($order->getAdjustmentNegative() != null) {
            $adjusttNeg = $this->writeOrderTotal("Adjustment Fee", $order->getAdjustmentPositive(), "fee", "add");
            $this->addToXML($totalsDOM, $adjusttNeg);
        }

        $grandTotal = $order->getGrandTotal();
        if ($grandTotal != null && $grandTotal > 0) {
            $this->addToXML($totalsDOM, $this->writeOrderTotal("Grand Total", $grandTotal, "total", "none"));
        }

        return $totalsDOM->getElementsByTagName("Totals")->item(0);
    }

    /**
     * @param  $name
     * @param  $value
     * @param  $class
     * @param $impact
     *
     * @return \DOMNode
     */
    private function writeOrderTotal($name, $value, $class, $impact = "add")
    {

        $totalXML = new \SimpleXMLElement("<Total>$value</Total>");

        $totalXML->addAttribute("name", $name);
        $totalXML->addAttribute("class", $class);
        $totalXML->addAttribute("impact", $impact);

        $Dom = new \DOMDocument();

        $Dom->loadXML($totalXML->asXML());

        return $Dom->getElementsByTagName("Total")->item(0);
    }

    /**
     * Writes the order items xml node
     *
     * @param \Magento\Sales\Api\Data\OrderItemInterface[] $items
     *
     * @return \DOMNode
     */
    private function writeOrderItems($items)
    {
        // Create a new document
        $itemsDOM = new \DOMDocument;
        $itemsDOM->formatOutput = true;

        // Load XML
        $itemsDOM->loadXML('<Items/>');

        foreach ($items as $item) {
            if (!$item->getParentItemId()) {
                $this->addToXML($itemsDOM, $this->writeOrderItem($item));
            }
        }

        return $itemsDOM->getElementsByTagName('Items')->item(0);
    }

    /**
     * Writes the order item xml node
     *
     * @param \Magento\Sales\Api\Data\OrderItemInterface $item
     *
     * @return \DOMNode
     */
    private function writeOrderItem($item)
    {
        // Create a new document
        $itemXml = new \SimpleXMLElement("<Item/>");

        // Build the item xml using simple xml
        $itemXml->addChild('ItemID', $this->escapeXml($item->getItemId()));
        $itemXml->addChild('ProductID', $this->escapeXml($item->getProductId()));
        $itemXml->addChild('Name', $this->escapeXml($item->getName()));
        $itemXml->addChild('SKU', $this->escapeXml($item->getSku()));
        $itemXml->addChild('Code', $this->escapeXml($item->getSku()));
        $itemXml->addChild('Weight', (float)$this->escapeXml($item->getWeight()));
        $itemXml->addChild('Quantity', (int)$this->escapeXml($item->getQtyOrdered()));
        $itemXml->addChild('UnitPrice', $this->escapeXml($item->getPrice()));

        // Create a new document
        $itemDOM = new \DOMDocument;
        $itemDOM->formatOutput = true;

        // Load XML
        $itemDOM->loadXML($itemXml->saveXML());

        // Add item attributes
        $attributesDom = new \DOMDocument;
        $attributesDom->formatOutput = true;
        $attributesDom->loadXML("<Attributes/>");

        $this->addItemAttributes($item, $attributesDom);
        $this->addItemGiftMessage($item, $attributesDom);

        $this->addToXML($itemDOM, $attributesDom->getElementsByTagName('Attributes')->item(0));

        // Return DOMNode
        return $itemDOM->getElementsByTagName('Item')->item(0);
    }

    /**
     * Adds item attributes to the Attributes dom
     *
     * @param \Magento\Sales\Api\Data\OrderItemInterface $item
     * @param \DOMDocument AttributesDOM
     *
     * @return \DOMNode
     */
    private function addItemAttributes($item, $attributes) {
        // Item attributes
        $options = $item->getProductOptions();
        if (!is_null($options)) {
            if (isset($options['options']) && is_array($options['options'])) {
                foreach ($options['options'] as $option) {
                    if (isset($option['label']) && isset($option['value'])) {
                        $name = $this->escapeXml($option['label']);
                        $value = $this->escapeXml($option['value']);
                        $attributeDom = new \DOMDocument;
                        $attributeDom->formatOutput = true;
                        $attributeDom->loadXML("<Attribute><Name>$name</Name><Value>$value</Value></Attribute>");
                        $this->addToXML($attributes, $attributeDom->getElementsByTagName('Attribute')->item(0));
                    }
                }
            }

            if (isset($options['attributes_info']) && is_array($options['attributes_info'])) {
                foreach ($options['attributes_info'] as $option) {
                    if (isset($option['label']) && isset($option['value'])) {
                        $name = $this->escapeXml($option['label']);
                        $value = $this->escapeXml($option['value']);
                        $attributeDom = new \DOMDocument;
                        $attributeDom->formatOutput = true;
                        $attributeDom->loadXML("<Attribute><Name>$name</Name><Value>$value</Value></Attribute>");
                        $this->addToXML($attributes, $attributeDom->getElementsByTagName('Attribute')->item(0));
                    }
                }
            }
        }
    }

    /**
     * Adds item gift message to the Attributes dom
     *
     * @param \Magento\Sales\Api\Data\OrderItemInterface $item
     * @param \DOMDocument AttributesDOM
     *
     * @return \DOMNode
     */
    private function addItemGiftMessage($item, $attributes) {
        // Item level gift messages
        if(!is_null($item->getGiftMessageId())) {
            $message = $this->OrderItemGiftMessageRepository->get($item->getOrderId(), $item->getId());

            $recipient = $this->escapeXml($message->getRecipient());
            $giftRecipientDom = new \DOMDocument;
            $giftRecipientDom->formatOutput = true;
            $giftRecipientDom->loadXML("<Attribute><Name>Gift Recipient</Name><Value>$recipient</Value></Attribute>");
            $this->addToXML($attributes, $giftRecipientDom->getElementsByTagName('Attribute')->item(0));

            $giftMessage = $this->escapeXml($message->getMessage());
            $giftMessageDom = new \DOMDocument;
            $giftMessageDom->formatOutput = true;
            $giftMessageDom->loadXML("<Attribute><Name>Gift Message</Name><Value>$giftMessage</Value></Attribute>");
            $this->addToXML($attributes, $giftMessageDom->getElementsByTagName('Attribute')->item(0));

            $sender = $this->escapeXml($message->getSender());
            $giftSenderDom = new \DOMDocument;
            $giftSenderDom->formatOutput = true;
            $giftSenderDom->loadXML("<Attribute><Name>Gift Sender</Name><Value>$sender</Value></Attribute>");
            $this->addToXML($attributes, $giftSenderDom->getElementsByTagName('Attribute')->item(0));
        }
    }

    /**
     * @param \Magento\Sales\Api\Data\OrderInterface $order The order to write notes for
     *
     * @return \DOMNode
     */
    private function writeOrderNotes($order)
    {
        // Create a new document
        $notesDOM = new \DOMDocument;
        $notesDOM->formatOutput = true;

        // Load XML
        $notesDOM->loadXML('<Notes/>');

        if(!is_null($order->getGiftMessageId())) {
            $message = $this->OrderGiftMessageRepository->get($order->getId());

            $sender = $this->escapeXml($message->getSender());
            $giftSenderDom = new \DOMDocument;
            $giftSenderDom->formatOutput = true;
            $giftSenderDom->loadXML("<Note>Gift Sender: $sender</Note>");
            $this->addToXML($notesDOM, $giftSenderDom->getElementsByTagName('Note')->item(0));

            $giftMessage = $this->escapeXml($message->getMessage());
            $giftMessageDom = new \DOMDocument;
            $giftMessageDom->formatOutput = true;
            $giftMessageDom->loadXML("<Note>Gift Message: $giftMessage</Note>");
            $this->addToXML($notesDOM, $giftMessageDom->getElementsByTagName('Note')->item(0));

            $recipient = $this->escapeXml($message->getRecipient());
            $giftRecipientDom = new \DOMDocument;
            $giftRecipientDom->formatOutput = true;
            $giftRecipientDom->loadXML("<Note>Gift Recipient: $recipient</Note>");
            $this->addToXML($notesDOM, $giftRecipientDom->getElementsByTagName('Note')->item(0));
        }

        $history = $order->getStatusHistories();
        foreach ($history as $historyItem) {
            $comment = $historyItem->getComment();
            if ($comment != '') {
                $this->addToXML($notesDOM, $this->writeOrderNote($historyItem));
            }
        }

        return $notesDOM->getElementsByTagName('Notes')->item(0);
    }

    /**
     * @param \Magento\Sales\Api\Data\OrderStatusHistoryInterface $history
     *
     * @return \DOMNode
     */
    private function writeOrderNote($history)
    {
        $comment = $this->escapeXml($history->getComment());
        $note = new \SimpleXMLElement("<Note>$comment</Note>");

        $createdAt = new \DateTime($history->getCreatedAt());

        $note->addAttribute('date', $createdAt->format('Y-m-d\TH:i:s'));
        $note->addAttribute('public', $history->getIsVisibleOnFront() ? "true" : "false");

        $noteDom = new \DOMDocument;
        $noteDom->formatOutput = true;
        $noteDom->loadXML($note->saveXML());

        return $noteDom->getElementsByTagName('Note')->item(0);
    }

    /**
     * @param string[string[]] $statuses
     * @return \DOMNode
     */
    public function writeOrderStatuses($statuses)
    {
        // Create a new document
        $statusDOM = new \DOMDocument;
        $statusDOM->formatOutput = true;

        // Load XML
        $statusDOM->loadXML('<StatusCodes/>');

        foreach ($statuses as $status) {
            $this->addToXML($statusDOM, $this->writeOrderStatus($status));
        }

        return $this->processResult($statusDOM->saveXML(), 'StatusCodes');
    }

    /**
     * @param string[] $status
     * @return \DOMNode
     */
    private function writeOrderStatus($status)
    {
        $statusXml = new \SimpleXMLElement("<StatusCode/>");

        $statusXml->addChild('Code', $this->escapeXml($status['status']));
        $statusXml->addChild('Name', $this->escapeXml($status['label']));


        $statusDOM = new \DOMDocument;
        $statusDOM->formatOutput = true;
        $statusDOM->loadXML($statusXml->saveXML());

        return $statusDOM->getElementsByTagName('StatusCode')->item(0);
    }

    /**
     * @param \ShipWorks\Module\Model\ShipWorks
     * @return string
     */
    public function writeModule($shipworks)
    {
        //Generate the Module XML
        $moduleXML = new \SimpleXMLElement('<Module/>');

        $moduleXML->addChild('Platform', $this->escapeXml($shipworks->Platform));
        $moduleXML->addChild('Developer', $this->escapeXml($shipworks->Developer));

        $moduleXML->addChild('Capabilities');
        $moduleXML->Capabilities->addChild('DownloadStrategy', $this->escapeXml($shipworks->DownloadStrategy));

        $moduleXML->Capabilities->addChild('OnlineCustomerID');
        foreach ($shipworks->OnlineCustomerID as $name => $value) {
            $moduleXML->Capabilities->OnlineCustomerID->addAttribute($name, $value);
        }

        $moduleXML->Capabilities->addChild('OnlineStatus');
        foreach ($shipworks->OnlineStatus as $name => $value) {
            $moduleXML->Capabilities->OnlineStatus->addAttribute($name, $value);
        }

        $moduleXML->Capabilities->addChild('OnlineShipmentUpdate');
        foreach ($shipworks->OnlineShipmentUpdate as $name => $value) {
            $moduleXML->Capabilities->OnlineShipmentUpdate->addAttribute($name, $value);
        }

        return $this->processResult($moduleXML->asXML(), 'Module');
    }

    /**
     * @param \Magento\Framework\DataObject $information
     * @return string
     */
    public function writeStoreInformation($information)
    {
        //Generate the Store XML
        $store = new \SimpleXMLElement('<Store/>');

        $name = $information->getData('name');
        $owner = '';
        $email = '';
        $state = $information->getData('region_id');
        $country = $information->getData('country_id');
        $website = '';

        $store->addChild('Name', $this->escapeXml($name));
        $store->addChild('CompanyOrOwner', $this->escapeXml($owner));
        $store->addChild('Email', $this->escapeXml($email));
        $store->addChild('State', $this->escapeXml($state));
        $store->addChild('Country', $this->escapeXml($country));
        $store->addChild('Website', $this->escapeXml($website));

        return $this->processResult($store->saveXML(), 'Store');
    }

    private function processResult($xmlString, $nodeName)
    {
        $resultDOM = new \DOMDocument();
        $resultDOM->formatOutput = true;
        $resultDOM->loadXML($xmlString);

        $shipWorksRoot = new \DOMDocument();
        $shipWorksRoot->formatOutput = true;
        $shipWorksRoot->loadXML("<ShipWorks moduleVersion='3.0.5.0' schemaVersion='1.1.0'/>");

        //var_dump($storeDOM->getElementsByTagName('Store')->item(0));

        $this->addToXML($shipWorksRoot, $resultDOM->getElementsByTagName($nodeName)->item(0));

        return $shipWorksRoot->saveXML();
    }
    /**
     * @param string
     * @return string
     */
    private function escapeXml($value)
    {
        $char = array('&', '<', '>');
        $entity = array('&amp;', '&lt;', '&gt;');
        return str_replace($char, $entity, $value);
    }
}
