<?php
/**
 * Copyright © 2015 ShipWorks. All rights reserved.
 * See COPYING.txt for license details.
 */

namespace ShipWorks\Module\Api;

/**
 * Interface providing ShipWorks response writing
 *
 * @api
 */
interface WriterInterface
{
    /**
     * @param $count
     * @return string
     */
    public function writeOrdersCount($count);

    /**
     * @param $orders
     * @return string
     */
    public function writeOrders($orders);

    /**
     * Statuses Available in Magento
     *
     * @param  \ShipWorks\Module\Model\ShipWorks
     * @return string
     */
    public function writeModule($shipworks);

    /**
     * @param string $code
     * @param string $message
     * @return string
     */
    public function writeException($code, $message);


    /**
     * @param \Magento\Sales\Api\Data\OrderInterface $order
     * @param string                     $message
     * @return string
     */
    public function writeSuccess($order, $message);

    /**
     * @param string[string[]] $statuses
     * @return string
     */
    public function writeOrderStatuses($statuses);

    /**
     * @param \Magento\Framework\DataObject $information
     * @return string
     */
    public function writeStoreInformation($information);
}
