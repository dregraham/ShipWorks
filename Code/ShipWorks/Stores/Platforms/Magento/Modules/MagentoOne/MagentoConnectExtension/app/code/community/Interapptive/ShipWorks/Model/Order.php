<?php
/**
 * ShipWorks
 *
 * PHP Version 5
 *
 * @category ShipWorks
 * @package  Interapptive\ShipWorks
 * @author   ShipWorks <support@shipworks.com>
 * @license  www.shipworks.com Commercial License
 * @link     www.shipworks.com
 */

/**
 * Class Interapptive_ShipWorks_Model_Order
 *
 * @category ShipWorks
 * @package  Interapptive\ShipWorks
 * @author   ShipWorks <support@shipworks.com>
 * @license  www.shipworks.com Commercial License
 * @link     www.shipworks.com
 */
class Interapptive_ShipWorks_Model_Order
{
    /**
     * Returns Orders in the ShipWorks Schema 1.1.0 XML Format for the given start date
     *
     * @param string $start    Start updated_at date
     * @param string $storeId  storeId
     * @param string $maxCount string the number of orders to return
     *
     * @return string
     */
    public function getOrders($start, $storeId, $maxCount)
    {
        $helper = Mage::helper("ShipWorks");
        $repo = Mage::getModel('ShipWorksOrder/Repo');

        // only get orders through 2 seconds ago
        $end = date("Y-m-d H:i:s", time() - 2);

        //Build query for orders
        $orders = $repo->getOrders($start, $end, $storeId, (int)$maxCount);

        //Mage::getModel()

        // Create a new document
        $ordersDOM = new DOMDocument;
        $ordersDOM->formatOutput = true;
        $ordersDOM->loadXML('<Orders/>');

        if ($orders->getSize()) {
            $orderWriter = Mage::getModel('ShipWorksOrder/XMLWriter');

            foreach ($orders as $order) {
                //Call OrderXMLWriter to generate the order XML
                $orderNode = $orderWriter->writeOrder($order);

                //Add the Order XML to the Orders document
                $helper->addToXML($ordersDOM, $orderNode);
            }
        }
        return $ordersDOM->saveXML();
    }

    /**
     * Returns the count of orders from $start for the given $storeCode
     *
     * @param string $start   Start date
     * @param string $storeId StoreId
     *
     * @return mixed
     */
    public function getCount($start, $storeId)
    {

        $repo = Mage::getModel('ShipWorksOrder/Repo');

        // only get orders through 2 seconds ago
        $end = date("Y-m-d H:i:s", time() - 2);

        $count = (int)$repo->getCount($start, $end, $storeId);

        $countXML = new SimpleXMLElement("<OrderCount>$count</OrderCount>");

        return $countXML->asXML();
    }

    /**
     * Updates an order with the given parameters
     *
     * @param string                                $orderId
     * @param string                                $command  the status to put the order in
     * @param string                                $comments comments to add to the order
     * @param Interapptive_ShipWorks_Model_Shipment $shipment shipment data to add to the order
     *
     * @return string
     */
    public function updateOrder($orderId, $command, $comments, $shipment)
    {
        $helper = Mage::helper('ShipWorks');
        $repo = Mage::getModel('ShipWorksOrder/Repo');

        $order = $repo->getOrder($orderId);

        // Check to see if the order exists, if not return an error
        if (is_null($order)) {
            return $helper->outputError(
                1010, 'Error Loading Order' . ' ' . $orderId
            );
        }

        try {
            // to change statuses, we need to unhold if necessary
            if ($order->canUnhold()) {
                $order->unhold();
                $repo->saveOrder($order);
            }

            switch (strtolower($command)) {
                case 'complete':
                    return $this->completeOrder($order, $comments, $shipment);
                case 'cancel':
                    $order->cancel();
                    $order->addStatusToHistory($order->getStatus(), $comments);
                    $repo->saveOrder($order);
                    break;
                case 'hold':
                    $order->hold();
                    $order->addStatusToHistory($order->getStatus(), $comments);
                    $repo->saveOrder($order);
                    break;
                default:
                    $helper->outputError(
                        80, "Unknown order command '$command'."
                    );
                    break;
            }
        } catch (Exception $e) {
            return $helper->outputError(
                90, 'Error Executing Command.' . ' ' . $e->getMessage()
            );
        }

        return '<success/>';
    }


    /**
     * Performs the steps needed to complete an order creates invoice and shipment
     *
     * @param Mage_Sales_Model_Order                $order
     * @param string                                $comments
     * @param Interapptive_ShipWorks_Model_Shipment $swShipment
     *
     * @return string
     */
    public function completeOrder($order, $comments, $swShipment)
    {
        $repo = Mage::getModel('ShipWorksOrder/Repo');

        //Check to see if the order already shipments
        if ($order->hasShipments()) {
            //Order already has shipments
            $existingShipments = $order->getShipmentsCollection();

            //Grab the first shipment to add tracking to
            $shipment = $existingShipments->getFirstItem();
        } else {
            //Order has no shipments
            $shipment = $order->prepareShipment();
            $shipment->register();
        }

        //Do Shipment Stuff
        if ($shipment) {
            $shipment->addComment($comments, false);
            $order->setIsInProcess(true);

            // add tracking info if it was supplied
            if (strlen($swShipment->Tracking) > 0) {
                $track = Mage::getModel('sales/order_shipment_track')
                    ->setNumber($swShipment->Tracking);

                # carrier data is of the format code|title
                $track->setCarrierCode($swShipment->Carrier);
                $track->setTitle($swShipment->Service);

                $shipment->addTrack($track);
            }

            $transactionSave = Mage::getModel('core/resource_transaction')
                ->addObject($shipment)
                ->addObject($shipment->getOrder());
            $transactionSave->save();

            // send the email if it's requested
            if ($swShipment->SendEmail == 1) {
                $shipment->sendEmail(true);
            }
        }

        // invoice the order
        if ($order->hasInvoices()) {
            // select the last invoice to attach the note to
            $invoice = $order->getInvoiceCollection()->getLastItem();
        } else {
            // prepare a brand-new invoice
            $invoice = $order->prepareInvoice();
            $invoice->register();
        }

        // capture the invoice if possible
        if ($invoice->canCapture()) {
            $invoice->Capture();
        }

        // some magento versions prevent multiple pay() calls from have impact,
        // but others don't.  If pay is called multiple times, Order.Total Paid is off.
        if ($invoice->getState() != Mage_Sales_Model_Order_Invoice::STATE_PAID
        ) {
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
        $repo->saveOrder($order);

        return '<success/>';
    }
}