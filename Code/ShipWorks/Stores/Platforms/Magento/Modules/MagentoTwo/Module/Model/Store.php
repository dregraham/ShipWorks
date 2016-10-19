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

use \ShipWorks\Module\Api\StoreInterface;
use \Magento\Sales\Model\Order\Status;
use \ShipWorks\Module\Api\WriterInterface;
use \Magento\Store\Model\Information;
use \Magento\Store\Api\StoreRepositoryInterface;

/**
 * Class Store
 *
 * @category ShipWorks
 * @package  Shipworks\Module
 * @author   ShipWorks <support@shipworks.com>
 * @license  www.shipworks.com Commercial License
 * @link     www.shipworks.com
 */
class Store implements StoreInterface
{
    // Instance of \ShipWorks\Module\Model\WriterInterface
    protected $Writer;

    // Instance of \Magento\Sales\Model\Order\Status
    protected $Status;

    // Instance of \Magento\Store\Api\StoreRepositoryInterface
    protected $StoreRepository;

    // Instance of \Magento\Store\Model\Information
    protected $Information;

    /**
     * @param WriterInterface          $writer
     * @param Status                   $status
     * @param StoreRepositoryInterface $storeRepository
     * @param Information              $information
     */
    public function __construct(
        WriterInterface $writer,
        Status $status,
        StoreRepositoryInterface $storeRepository,
        Information $information
    ) {
        $this->Status = $status;
        $this->Writer = $writer;
        $this->StoreRepository = $storeRepository;
        $this->Information = $information;
    }

    /**
     * Returns the store information
     *
     * @return string
     */
    public function getStore()
    {
        $store = $this->StoreRepository->getActiveStoreByCode('default');
        $storeInformation = $this->Information->getStoreInformationObject($store);

        return $this->Writer->writeStoreInformation($storeInformation);
    }

    /**
     * Returns all of the available statuses in Magento
     *
     * @return string
     */
    public function getStatusCodes()
    {
        $statusCollection = $this->Status->getResourceCollection();
        $statuses = $statusCollection->getData();

        return $this->Writer->writeOrderStatuses($statuses);
    }
}
