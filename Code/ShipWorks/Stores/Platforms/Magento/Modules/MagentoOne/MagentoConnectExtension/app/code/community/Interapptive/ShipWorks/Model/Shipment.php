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
 * Class Interapptive_ShipWorks_Model_Version
 *
 * @category ShipWorks
 * @package  Interapptive\ShipWorks
 * @author   ShipWorks <support@shipworks.com>
 * @license  www.shipworks.com Commercial License
 * @link     www.shipworks.com
 */
class Interapptive_ShipWorks_Model_Shipment
{
    public $Carrier = '';
    public $Service = '';
    public $Tracking = '';
    public $SendEmail = '';


    /**
     * stores shipment data
     *
     * @param string $carrierData
     * @param string $tracking
     * @param int    $sendEmail
     */
    public function createShipment($carrierData, $tracking, $sendEmail)
    {
        if ($carrierData != '' and $tracking != '') {
            $carrierData = preg_split('[\|]', $carrierData);

            $this->Carrier = preg_replace('/[^\da-z ]/i', '', $carrierData[0]);

            $this->Service = preg_replace('/[^\da-z ]/i', '', $carrierData[1]);

            $this->Tracking = preg_replace('/[^\da-z ]/i', '', $tracking);

            $this->SendEmail = (int)$sendEmail;
        }
    }
}