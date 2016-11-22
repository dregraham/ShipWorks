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
 * Class Interapptive_ShipWorks_Model_ItemAttributes
 *
 * @category ShipWorks
 * @package  Interapptive\ShipWorks
 * @author   ShipWorks <support@shipworks.com>
 * @license  www.shipworks.com Commercial License
 * @link     www.shipworks.com
 */
class Interapptive_ShipWorks_Model_ItemAttributes
{
    /**
     * Function toOptionArray()
     *
     * Returns a list of all item attributes available
     *
     * @return array
     */
    public function toOptionArray()
    {
        $attributes = Mage::getSingleton('eav/config')
            ->getEntityType(Mage_Catalog_Model_Product::ENTITY)
            ->getAttributeCollection();

        $optionArray = [];

        $optionArray['None'] = 'None';

        foreach ($attributes as $attribute) {

            if ($attribute->getFrontendLabel() != '') {
                $optionArray[$attribute->getAttributeCode()]
                    = $attribute->getFrontendLabel();
            }
        }
        return $optionArray;
    }
}
