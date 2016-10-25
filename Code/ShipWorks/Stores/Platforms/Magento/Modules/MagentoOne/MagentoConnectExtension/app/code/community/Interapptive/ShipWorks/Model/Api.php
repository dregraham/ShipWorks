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
 * Class Interapptive_ShipWorks_Model_ObjectModel_Api
 *
 * @category ShipWorks
 * @package  Interapptive\ShipWorks
 * @author   ShipWorks <support@shipworks.com>
 * @license  www.shipworks.com Commercial License
 * @link     www.shipworks.com
 */
class Interapptive_ShipWorks_Model_Api extends Mage_Api_Model_Resource_Abstract
{

    /**
     * Gets the module capabilities
     *
     * @return mixed
     */
    public function getModule()
    {
        $helper = Mage::helper('ShipWorks');
        //ShipWorks root
        $shipWorks = Mage::getModel('ShipWorks/ShipWorks')->getShipWorksRoot();
        //Get the module XML
        $moduleXml = Mage::getModel('ShipWorks/ShipWorks')->getModule();

        $module = new DOMDocument();
        $module->loadXML($moduleXml);
        $moduleNode = $module->getElementsByTagName('Module')->item(0);

        //Add the Order XML to the Orders document
        $helper->addToXML($shipWorks, $moduleNode);

        //Return the response
        return $moduleXml;
    }

    /**
     * Gets store information
     *
     * @return mixed
     */
    public function getStore()
    {
        $helper = Mage::helper('ShipWorks');
        //ShipWorks root
        $shipWorks = Mage::getModel('ShipWorks/ShipWorks')->getShipWorksRoot();

        //Get the store XML
        $storeXml = Mage::getModel('ShipWorks/Store')->getStore();

        $store = new DOMDocument();
        $store->loadXML($storeXml);
        $storeNode = $store->getElementsByTagName('Store')->item(0);

        //Add the Order XML to the Orders document
        $helper->addToXML($shipWorks, $storeNode);

        //Return the response
        return $shipWorks->saveXML();
    }

    /**
     * Writes order XML for orders newer than the $start belonging to the $storeCode
     *
     * @param string $start     start date
     * @param int    $maxCount  number of orders to return
     * @param int    $storeCode store code
     *
     * @return string
     */
    public function getOrders($start, $maxCount, $storeCode)
    {
        $helper = Mage::helper('ShipWorks');
        //ShipWorks root
        $shipWorks = Mage::getModel('ShipWorks/ShipWorks')->getShipWorksRoot();
        // Convert to local SQL time
        // Returns null of date is invalid
        $start = $helper->toLocalSqlDate($start);

        // Get the storeId
        if ($storeCode == '') {
            $storeId = Mage::app()->getDefaultStoreView()->getId();
        } else {
            $storeId = Mage::app()->getStore($storeCode)->getId();
        }

        // check to see if start is valid
        if (!is_null($start) && !is_null($storeId) && (int)$maxCount) {
            //Get the Orders XML
            $ordersXML = Mage::getModel('ShipWorks/Order')->getOrders(
                $start, $storeId, (int)$maxCount
            );
        } else {
            return $helper->outputError('40', 'Insufficient parameters');
        }

        $orders = new DOMDocument();
        $orders->loadXML($ordersXML);
        $ordersNode = $orders->getElementsByTagName('Orders')->item(0);

        //Add the Order XML to the Orders document
        $helper->addToXML($shipWorks, $ordersNode);

        //Return the response
        return $shipWorks->saveXML();

    }

    /**
     * Returns the number of orders that have been modified after the $start for $storeCode
     *
     * @param string $start     Start date to count orders from
     * @param string $storeCode Store to count orders for
     *
     * @return int
     */
    public function getCount($start, $storeCode)
    {
        $helper = Mage::helper('ShipWorks');
        //ShipWorks root
        $shipWorks = Mage::getModel('ShipWorks/ShipWorks')->getShipWorksRoot();

        // Convert to local SQL time
        // Returns null of date is invalid
        $start = $helper->toLocalSqlDate($start);

        // Get the storeId
        if ($storeCode == '') {
            $storeId = Mage::app()->getDefaultStoreView()->getId();
        } else {
            $storeId = Mage::app()->getStore($storeCode)->getId();
        }

        // check to see if start and storeid are valid
        if (!is_null($start) && !is_null($storeId)) {
            //Get the Count XML
            $countXML = Mage::getModel('ShipWorks/Order')->getCount(
                $start, $storeId
            );
        } else {
            return $helper->outputError('40', 'Insufficient parameters');
        }

        $count = new DOMDocument();
        $count->loadXML($countXML);
        $countNode = $count->getElementsByTagName('OrderCount')->item(0);

        //Add the Order XML to the Orders document
        $helper->addToXML($shipWorks, $countNode);

        //Return the response
        return $shipWorks->saveXML();
    }


    /**
     * Performs the steps needed to change the order status
     *
     * @param string $orderId orders incrementId
     * @param string $command     status to change the order to
     * @param string $comments    comments to add to the order
     * @param string $tracking    tracking number to add
     * @param string $carrierData service and carrier to add
     * @param int    $sendEmail   Notify the customer of the status change
     *
     * @return mixed
     */
    public function updateOrder($orderId, $command, $comments, $tracking,
        $carrierData, $sendEmail
    ) {
        $shipment = Mage::getModel('ShipWorks/Shipment');
        $shipment->createShipment($carrierData, $tracking, $sendEmail);

        // Remove all non alphanumeric or space characters
        $orderId = preg_replace('/[^\da-z ]/i', '', $orderId);
        $command = preg_replace('/[^\da-z ]/i', '', $command);
        $comments = preg_replace('/[^\da-z ]/i', '', $comments);

        $result = Mage::getModel('ShipWorks/Order')->updateOrder(
            $orderId, $command, $comments, $shipment
        );

        return $result;
    }


    /**
     * Generates XML of available order statuses
     *
     * @return string
     */
    public function getStatusCodes()
    {
        $helper = Mage::helper('ShipWorks');
        //ShipWorks root
        $shipWorks = Mage::getModel('ShipWorks/ShipWorks')->getShipWorksRoot();

        //Get the store XML
        $statusCodeXML = Mage::getModel('ShipWorks/Store')->getStatusCodes();

        $status = new DOMDocument();
        $status->loadXML($statusCodeXML);
        $statusNode = $status->getElementsByTagName('StatusCodes')->item(0);

        //Add the status code XML to the ShipWorks document
        $helper->addToXML($shipWorks, $statusNode);

        //Return the response
        return $shipWorks->saveXML();
    }
}