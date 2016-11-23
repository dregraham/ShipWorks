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
 * Class Interapptive_ShipWorks_Model_Order_Repo
 *
 * @category ShipWorks
 * @package  Interapptive\ShipWorks
 * @author   ShipWorks <support@shipworks.com>
 * @license  www.shipworks.com Commercial License
 * @link     www.shipworks.com
 */
class Interapptive_ShipWorks_Model_Order_Repo
{
    /**
     * Returns the orders with the given params
     *
     * @param string $start   Start updated_at date
     * @param string $end     end updated_at date
     * @param string $storeId storeId of the store
     * @param string $maxCount
     *
     * @return Mage_Sales_Model_Resource_Order_Collection
     */
    public function getOrders($start, $end, $storeId, $maxCount)
    {

        $orders = Mage::getModel('sales/order')->getCollection();
        $orders->addFieldToFilter('updated_at', array('gt' => $start));
        $orders->addFieldToFilter('updated_at', array('lteq' => $end));
        $orders->addFieldToFilter('store_id', $storeId);
        $orders->setOrder('updated_at', 'asc');

        // configure paging and load orders
        $orders->setCurPage(1)
            ->setPageSize($maxCount)
            ->loadData();


        $processedIds = array();
        $lastModified = '';

        // We have to do this rather than getAllIds() because getAllIds() does not respect the page size
        foreach ($orders as $order) {
            // add orders id to an array
            array_push($processedIds, $order->getId());
            //Grab the last last modified date
            $lastModified = $order->getUpdatedAt();
        }

        //If order 50 and 50+ share the same lastmodified timestamp, we need to get all of them so that
        //The next GetOrders call does not skip those orders
        if (!empty($processedIds) and $lastModified != '') {
            $missingOrders = Mage::getModel('sales/order')->getCollection()
                ->addFieldToFilter('updated_at', array('eq' => $lastModified))
                ->addFieldToFilter('entity_id', array('nin' => $processedIds))
                ->addFieldToFilter('store_id', $storeId)
                ->loadData();

            foreach ($missingOrders as $missingOrder) {
                $orders->addItem($missingOrder);
            }
        }

        return $orders;
    }

    /**
     * Returns the number of orders between the start and end date for a given storeId
     *
     * @param string $start   Start updated_at date
     * @param string $end     End updated_at date
     * @param string $storeId storeId
     *
     * @return int            number of orders
     */
    public function getCount($start, $end, $storeId)
    {

        $orders = Mage::getModel('sales/order')->getCollection()
            ->addFieldToFilter('updated_at', array('gt' => $start))
            ->addFieldToFilter('updated_at', array('lteq' => $end))
            ->addFieldToFilter('store_id', $storeId);

        $count = $orders->getSize();

        return (int)$count;
    }

    /**
     * Returns the order for the given incrementId
     *
     * @param string $id the orders incrementId
     *
     * @return Mage_Sales_Model_Order
     */
    public function getOrder($id)
    {
        $order = Mage::getModel('sales/order')->Load($id);

        if ($order->getId() != '') {
            return $order;
        } else {
            return null;
        }
    }

    /**
     * Saves an order
     *
     * @param Mage_Sales_Model_Order $order
     */
    public function saveOrder($order)
    {
        $order->save();
    }
}