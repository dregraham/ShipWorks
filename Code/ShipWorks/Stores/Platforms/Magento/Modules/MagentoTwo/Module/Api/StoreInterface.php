<?php
/**
 * Copyright © 2015 ShipWorks. All rights reserved.
 * See COPYING.txt for license details.
 */

namespace ShipWorks\Module\Api;

/**
 * Interface providing ShipWorks store related integration
 *
 * @api
 */
interface StoreInterface
{
    /**
     * Returns store information
     *
     * @return string
     */
    public function getStore();

    /**
     * Statuses Available in Magento
     *
     * @return string
     */
    public function getStatusCodes();
}
