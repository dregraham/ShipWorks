<?php
/**
 * Copyright © 2015 ShipWorks. All rights reserved.
 * See COPYING.txt for license details.
 */

namespace ShipWorks\Module\Api;

/**
 * Interface providing ShipWorks order related integration
 *
 * @api
 */
interface OrderInterface
{
    /**
     * Gets orders that match the parameters
     *
     * @api
     * @param  string $start    date to return orders from
     * @param  int    $maxcount number of orders to return
     * @return string order xml
     */
    public function getOrders($start, $maxcount);

    /**
     * Gets orders count that match the parameters
     *
     * @api
     * @param  string $start date to return count from
     * @return string count
     */
    public function getCount($start);

    /**
     * Updates order information
     *
     * @api
     * @param  string[] $data
     * @return string
     */
    public function updateOrders($data);
}
