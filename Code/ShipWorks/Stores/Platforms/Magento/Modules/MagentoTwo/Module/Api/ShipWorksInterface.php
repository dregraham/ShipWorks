<?php
/**
 * Copyright © 2015 ShipWorks. All rights reserved.
 * See COPYING.txt for license details.
 */

namespace ShipWorks\Module\Api;

/**
 * Interface providing ShipWorks related integration
 *
 * @api
 */
interface ShipWorksInterface
{
    /**
     * Statuses Available in Magento
     *
     * @return string
     */
    public function getModule();

    /**
     * displays a message to the user that
     * the url for shipworks
     *
     * @return string
     */
    public function getSanityCheck();
}
