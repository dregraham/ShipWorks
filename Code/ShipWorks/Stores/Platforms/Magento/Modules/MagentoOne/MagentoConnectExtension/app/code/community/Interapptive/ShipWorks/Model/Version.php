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
class Interapptive_ShipWorks_Model_Version
{
    /**
     * Function toOptionArray()
     *
     * Gets the UI value for version information for display in the UI
     *
     * @return array
     */
    public function toOptionArray()
    {
        return array('version' => $this->getModuleVersion());
    }

    /**
     * Function getModuleVersion()
     *
     * Gets the module version
     *
     * @return string
     */
    public function getModuleVersion()
    {
        return '4.0.3';
    }
}
