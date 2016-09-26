<?php
/**
 * ShipWorks
 *
 * PHP Version 5
 *
 * @category ShipWorks
 * @package  Shipworks\Module
 * @author   ShipWorks <support@shipworks.com>
 * @license  www.shipworks.com Commercial License
 * @link     www.shipworks.com
 */
namespace Shipworks\Module\Model;

use \ShipWorks\Module\Api\OrderActionParametersInterface;

/**
 * Class OrderActionParameters
 *
 * @category ShipWorks
 * @package  Shipworks\Module
 * @author   ShipWorks <support@shipworks.com>
 * @license  www.shipworks.com Commercial License
 * @link     www.shipworks.com
 */
class OrderActionParameters implements OrderActionParametersInterface
{
    public $Action;
    public $OrderId;
    public $TrackingNumber;
    public $Comments;
    public $SendInvoiceEmail;
    public $SendShipmentEmail;
    public $Carrier;
    public $Service;

    /**
     * @param \string[] $data
     */
    public function load($data)
    {
        $this->Action = isset($data['command']) ? self::sanitizeString($data['command']) : '';
        $this->OrderId = isset($data['order']) ? self::sanitizeInt($data['order']) : '';
        $this->TrackingNumber = isset($data['tracking']) ? self::sanitizeString($data['tracking']) : '';
        $this->Comments = isset($data['comments']) ?self::sanitizeString($data['comments']) : '';
        
        $sendInvoiceEmail = isset($data['sendInvoiceEmail']) ? self::sanitizeBool($data['sendInvoiceEmail']) : '';
        $this->SendInvoiceEmail = $sendInvoiceEmail;

        $sendShipmentEmail = isset($data['sendShipmentEmail']) ? self::sanitizeBool($data['sendShipmentEmail']) : '';
        $this->SendShipmentEmail = $sendShipmentEmail;
        
        $carrierData = $this->parseCarrier($data['carrier']);

        $this->Carrier = isset($carrierData['0']) ? $carrierData['0'] : '';
        $this->Service = isset($carrierData['1']) ? $carrierData['1'] : '';
    }

    /**
     * Returns the action name
     *
     * @return string
     */
    public function getAction()
    {
        return (string) $this->Action;
    }

    /**
     * Returns the order id
     *
     * @return float
     */
    public function getOrderId()
    {
        return (float) $this->OrderId;
    }

    /**
     * Returns the tracking
     *
     * @return string
     */
    public function getTrackingNumber()
    {
        return (string) $this->TrackingNumber;
    }

    /**
     * Returns the comments
     *
     * @return string
     */
    public function getComments()
    {
        return (string) $this->Comments;
    }

    /**
     * Returns the SendInvoiceEmail
     *
     * @return bool
     */
    public function getSendInvoiceEmail()
    {
        return (bool) $this->SendInvoiceEmail;
    }

    /**
     * Returns the SendShipmentEmail
     *
     * @return bool
     */
    public function getSendShipmentEmail()
    {
        return (bool) $this->SendShipmentEmail;
    }

    /**
     * Returns the carrier
     *
     * @return string
     */
    public function getCarrier()
    {
        return (string) $this->Carrier;
    }

    /**
     * Returns the service
     *
     * @return string
     */
    public function getService()
    {
        return (string) $this->Service;
    }

    /**
     * @param \string $carrierData
     * @return \string[]
     */
    private static function parseCarrier($carrierData)
    {
        return preg_split('[\|]', $carrierData);
    }

    /**
     * @param \string $string
     * @return \string
     */
    private static function sanitizeString($string)
    {
        return preg_replace('/[^\da-z ]/i', '', $string);
    }

    /**
     * @param \string $string
     * @return \int
     */
    private static function sanitizeInt($string)
    {
        return preg_replace('/[^0-9]/', '', $string);
    }

    /**
     * @param \string $string
     * @return \bool
     */
    private static function sanitizeBool($string)
    {
        if (trim(strtolower($string)) == "true") {
            return true;
        } elseif (trim(strtolower($string)) == "false") {
            return false;
        } else {
            return (bool)$string;
        }
    }
}
