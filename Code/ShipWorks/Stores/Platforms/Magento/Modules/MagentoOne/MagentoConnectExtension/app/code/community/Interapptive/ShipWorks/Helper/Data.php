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
 * Class Interapptive_ShipWorks_Helper_Data
 *
 * @category ShipWorks
 * @package  Interapptive\ShipWorks
 * @author   ShipWorks <support@shipworks.com>
 * @license  www.shipworks.com Commercial License
 * @link     www.shipworks.com
 */
class Interapptive_ShipWorks_Helper_Data extends Mage_Core_Helper_Abstract
{
    /**
     * Takes an DOMDocument and imports DOMNode into it
     *
     * @param DOMDocument $DOM  DOMDocument to import the node into
     * @param DOMNode     $Node DOMNode to be imported
     *
     * @return DOMDocument
     */
    public static function addToXML($DOM, $Node)
    {
        $Node = $DOM->importNode($Node, true);
        $DOM->documentElement->appendChild($Node);

        return $DOM;
    }

    /**
     * Function toLocalSqlDate($sqlUtc)
     *
     * @param string $sqlUtc utc date
     *
     * @return mixed
     */
    public static function toLocalSqlDate($sqlUtc)
    {
        // Try to create a datetime object using the time given
        try {
            //DateTime Object using start in UTC
            $date = new DateTime($sqlUtc, new DateTimeZone('UTC'));
            return $date->format('Y-m-d H:i:s');
        } catch (Exception $e) {
            // it failed, assuming the date/time was invalid
            // return false
            return null;
        }
    }

    /**
     * Outputs the error XML element from the ShipWorks 1.1.0 XSD
     *
     * @param string $code  The error code to output
     * @param string $error The error text to output
     *
     * @return mixed
     */
    public static function outputError($code, $error)
    {
        //Generate the Store XML
        $store = new SimpleXMLElement('<Error/>');

        $store->addChild('Code', $code);
        $store->addChild('Error', $error);

        return $store->asXML();
    }

    /**
     * Returns an array of attribute codes from store config
     * for a given storeId.
     *
     * @param $storeId
     * @return array
     */
    public function getStoreConfig($storeId)
    {
        //Pull Attributes based on what is set in admin
        $location = Mage::getStoreConfig('shipWorksApi/mapping/Location', $storeId);

        $code = Mage::getStoreConfig('shipWorksApi/mapping/Code', $storeId);

        $attributeOne = Mage::getStoreConfig('shipWorksApi/mapping/AttributeOne', $storeId);

        $attributeTwo = Mage::getStoreConfig('shipWorksApi/mapping/AttributeTwo', $storeId);

        $attributeThree = Mage::getStoreConfig('shipWorksApi/mapping/AttributeThree', $storeId);

        $attributes = array("Location" => $location,
            "Code" => $code,
            "AttributeOne" => $attributeOne,
            "AttributeTwo" => $attributeTwo,
            "AttributeThree" => $attributeThree);

        // Check to see if any of values are 'None'
        // update that value to be blank
        foreach ($attributes as $key => $value) {
            if ($value == 'None') {
                $attributes[$key] = '';
            }
        }
        
        return $attributes;
    }
}
