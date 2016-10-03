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
namespace ShipWorks\Module\Model;

use \ShipWorks\Module\Api\ShipWorksInterface;

/**
 * Class Store
 *
 * @category ShipWorks
 * @package  Shipworks\Module
 * @author   ShipWorks <support@shipworks.com>
 * @license  www.shipworks.com Commercial License
 * @link     www.shipworks.com
 */
class ShipWorks implements ShipWorksInterface
{
    const MODULE_VERSION = '3.10.0';

    const SCHEMA_VERSION = '1.1.0';

    public $Platform = 'Magento';

    public $Developer = 'Interapptive, Inc (support@interapptive.com)';

    public $DownloadStrategy = 'ByModifiedTime';

    public $OnlineCustomerID = ['supported' => 'true',
        'dataType' => 'text'];

    public $OnlineStatus = ['supported'    => 'true',
        'dataType' => 'text',
        'downloadOnly' => 'true'];

    public $OnlineShipmentUpdate = ['supported' => 'false'];

    // Instance of \ShipWorks\Module\Model\XMLWriter
    protected $Writer;

    /**
     * @param XMLWriter $writer
     */
    public function __construct(XMLWriter $writer)
    {
        $this->Writer = $writer;
    }

    /**
     * Returns Module configuration
     *
     * @return string
     */
    public function getModule()
    {
        return $this->Writer->writeModule($this);
    }

    /**
     * Returns Module configuration
     *
     * @return string
     */
    public function getSanityCheck()
    {
        return "This is your ShipWorks module URL";
    }
}
