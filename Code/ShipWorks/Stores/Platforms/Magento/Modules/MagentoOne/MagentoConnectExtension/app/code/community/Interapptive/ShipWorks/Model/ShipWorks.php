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
 * Class Interapptive_ShipWorks_Model_ShipWorks
 *
 * @category ShipWorks
 * @package  Interapptive\ShipWorks
 * @author   ShipWorks <support@shipworks.com>
 * @license  www.shipworks.com Commercial License
 * @link     www.shipworks.com
 */
class Interapptive_ShipWorks_Model_ShipWorks
{
    const MODULE_VERSION = '3.10.0';
    const SCHEMA_VERSION = '1.1.0';
    const PLATFORM = 'Magento';
    const DEVELOPER = 'Interapptive, Inc (support@interapptive.com)';
    const DOWNLOADSTRATEGY = 'ByModifiedTime';
    private $OnlineCustomerID
        = array('supported' => 'true', 'dataType' => 'text');
    private $OnlineStatus
        = array('supported'    => 'true', 'dataType' => 'text',
           'downloadOnly' => 'true');
    private $OnlineShipmentUpdate = array('supported' => 'false');

    /**
     * Function getModule()
     *
     * Returns a module information as xml
     *
     * @return string
     */
    public function getModule()
    {
        //Generate the Module XML
        $module = new SimpleXMLElement('<Module/>');

        $module->addChild('Platform', self::PLATFORM);
        $module->addChild('Developer', self::DEVELOPER);

        $module->addChild('Capabilities');
        $module->Capabilities->addChild(
            'DownloadStrategy', self::DOWNLOADSTRATEGY
        );

        $module->Capabilities->addChild('OnlineCustomerID');
        foreach ($this->OnlineCustomerID as $name => $value) {
            $module->Capabilities->OnlineCustomerID->addAttribute(
                $name, $value
            );
        }

        $module->Capabilities->addChild('OnlineStatus');
        foreach ($this->OnlineStatus as $name => $value) {
            $module->Capabilities->OnlineStatus->addAttribute($name, $value);
        }

        $module->Capabilities->addChild('OnlineShipmentUpdate');
        foreach ($this->OnlineShipmentUpdate as $name => $value) {
            $module->Capabilities->OnlineShipmentUpdate->addAttribute(
                $name, $value
            );
        }

        return $module->asXML();
    }

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
        return self::MODULE_VERSION;
    }

    /**
     * Function getSchemaVersion()
     *
     * Gets the schema version
     *
     * @return string
     */
    public function getSchemaVersion()
    {
        return self::SCHEMA_VERSION;
    }


    /**
     * XML <ShipWorks/> root element
     *
     * @return DOMDocument
     */
    public function getShipWorksRoot()
    {

        $moduleVersion = self::MODULE_VERSION;
        $schemaVersion = self::SCHEMA_VERSION;

        //All responses to ShipWorks need the root element of <ShipWorks/>
        //Create the ShipWorks XML Document to import the module xml into
        $ShipWorks = new DOMDocument;
        $ShipWorks->LoadXML(
            "<ShipWorks moduleVersion='$moduleVersion' schemaVersion='$schemaVersion'/>"
        );

        return $ShipWorks;
    }
}
