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
class Interapptive_ShipWorks_Model_Store
{

    /**
     * Returns the store information
     *
     * @return string
     */
    public function getStore()
    {
        //Generate the Store XML
        $store = new SimpleXMLElement('<Store/>');

        $state = '';
        $region_model = Mage::getModel('directory/region');
        if (is_object($region_model)) {
            $state = $region_model->load(
                Mage::getStoreConfig('shipping/origin/region_id')
            )->getDefaultName();
        }

        $name = Mage::getStoreConfig('system/store/name');
        $owner = Mage::getStoreConfig('trans_email/ident_general/name');
        $email = Mage::getStoreConfig('trans_email/ident_general/email');
        $country = Mage::getStoreConfig('shipping/origin/country_id');
        $website = Mage::getURL();

        $store->addChild('Name', $name);
        $store->addChild('Owner', $owner);
        $store->addChild('Email', $email);
        $store->addChild('State', $state);
        $store->addChild('Country', $country);
        $store->addChild('Website', $website);

        return $store->asXML();

    }

    /**
     * Returns all of the available statuses in Magento
     *
     * @return string
     */
    public function getStatusCodes()
    {

        $helper = Mage::helper('ShipWorks');

        $statuses_node = Mage::getConfig()->getNode(
            'global/sales/order/statuses'
        );

        // Create a new document
        $statuses = new DOMDocument;
        $statuses->formatOutput = true;

        // statuses Dom
        $statuses->loadXML('<StatusCodes/>');

        foreach ($statuses_node->children() as $status) {
            $statusCode = $status->getName();
            $statusName = $status->label;

            $statusXML = new DOMDocument;
            $statusXML->loadXML(
                "<StatusCode>
                    <Code>$statusCode</Code>
                    <Name>$statusName</Name>
                </StatusCode>"
            );

            // The node we want to import to a new document
            $statusNode = $statusXML->getElementsByTagName('StatusCode')->item(
                0
            );

            $helper->addToXML($statuses, $statusNode);
        }

        return $statuses->saveXML();
    }
}
