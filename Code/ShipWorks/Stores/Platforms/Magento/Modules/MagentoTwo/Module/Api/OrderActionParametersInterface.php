<?php

/**
 * Copyright © 2015 ShipWorks. All rights reserved.
 * See COPYING.txt for license details.
 */

namespace ShipWorks\Module\Api;

/**
 * Interface providing ShipWorks parameter information
 *
 * @api
 */
interface OrderActionParametersInterface
{
    /**
     * @param \string[] $data
     */
    public function load($data);

    /**
     * Returns the action name
     *
     * @return string
     */
    public function getAction();

    /**
     * Returns the order id
     *
     * @return float
     */
    public function getOrderId();

    /**
     * Returns the tracking
     *
     * @return string
     */
    public function getTrackingNumber();

    /**
     * Returns the comments
     *
     * @return string
     */
    public function getComments();

    /**
     * Returns the SendInvoiceEmail
     *
     * @return bool
     */
    public function getSendInvoiceEmail();

    /**
     * Returns the SendShipmentEmail
     *
     * @return bool
     */
    public function getSendShipmentEmail();

    /**
     * Returns the carrier
     *
     * @return string
     */
    public function getCarrier();

    /**
     * Returns the service
     *
     * @return string
     */
    public function getService();
}
