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

use \Magento\Framework\ObjectManagerInterface;
use Magento\Sales\Model\Order\Shipment\TrackFactory;
use \Magento\Sales\Model\Order\ShipmentFactory;
use \Magento\Sales\Api\OrderRepositoryInterface;
use \Magento\Framework\Exception\LocalizedException;
use \ShipWorks\Module\Api\OrderInterface;
use \ShipWorks\Module\Api\OrderActionParametersInterface;
use \ShipWorks\Module\Api\WriterInterface;

/**
 * Class Order
 *
 * @category ShipWorks
 * @package  Shipworks\Module
 * @author   ShipWorks <support@shipworks.com>
 * @license  www.shipworks.com Commercial License
 * @link     www.shipworks.com
 */
class Order implements OrderInterface
{
    // Constant order actions ShipWorks
    const COMPLETE = 'complete';
    const CANCEL = 'cancel';
    const HOLD = 'hold';
    
    // Instance of \ShipWorks\Module\Model\XMLWriter
    protected $Writer;

    // Instance of \Magento\Sales\Model\Order\ShipmentFactory
    protected $ShipmentFactory;

    // Instance of \Magento\Framework\ObjectManagerInterface
    protected $ObjectManager;

    // Instance of \Magento\Sales\Api\OrderRepositoryInterface
    protected $OrderRepository;

    // Instance of \ShipWorks\Module\Api\OrderActionParametersInterface;
    protected $OrderActionParameters;

    // Instance of Magento\Sales\Model\Order\Shipment\TrackFactory
    protected $TrackFactory;


    /**
     * Order constructor.
     * @param OrderRepositoryInterface $orderRepository
     * @param WriterInterface $writer
     * @param ShipmentFactory $shipmentFactory
     * @param ObjectManagerInterface $objectManager
     * @param OrderActionParametersInterface $orderActionParameters
     * @param TrackFactory $trackFactory
     */
    public function __construct(
        OrderRepositoryInterface $orderRepository,
        WriterInterface $writer,
        ShipmentFactory $shipmentFactory,
        ObjectManagerInterface $objectManager,
        OrderActionParametersInterface $orderActionParameters,
        TrackFactory $trackFactory
    ) {
        $this->OrderRepository = $orderRepository;
        $this->Writer = $writer;
        $this->ShipmentFactory = $shipmentFactory;
        $this->ObjectManager = $objectManager;
        $this->OrderActionParameters = $orderActionParameters;
        $this->TrackFactory = $trackFactory;
    }

    /**
     * Returns the count of orders from the given start date
     *
     * @param  string $start
     * @return string
     */
    public function getCount($start)
    {
        $startDate = new \DateTime($start, new \DateTimeZone('UTC'));
        $formattedDate = $startDate->format('Y-m-d H:i:s');

        $result = $this->OrderRepository->getList($this->createOrderSearchCriteria($formattedDate));

        return $this->Writer->writeOrdersCount((int) $result->getTotalCount());
    }

    /**
     * Gets orders that match the parameters
     *
     * @api
     * @param  string $start    date to return orders from
     * @param  int    $maxcount number of orders to return
     * @return string
     */
    public function getOrders($start, $maxcount = 50)
    {
        // Get orders from the start date
        $startDate = new \DateTime($start, new \DateTimeZone('UTC'));
        $formattedDate = $startDate->format('Y-m-d H:i:s');

        // Create the order repo and get a list of orders matching our criteria
        $result = $this->OrderRepository->getList($this->createOrderSearchCriteria($formattedDate, $maxcount));

        $orders = null;
        $missingOrders = null;

        if ($result->getTotalCount() > 0) {
            $orders = $result->getItems();

            $processedIds = array();
            $lastModified = '';

            // We have to do this rather than getAllIds() because getAllIds() does not respect the page size
            foreach ($orders as $order) {
                // add orders id to an array
                array_push($processedIds, $order->getEntityId());
                //Grab the last last modified date
                $lastModified = $order->getUpdatedAt();
            }

            //If order 50 and 50+ share the same lastmodified timestamp, we need to get all of them so that
            //The next GetOrders call does not skip those orders
            if (!empty($processedIds) and $lastModified != '') {
                $search = $this->createMissingOrderSearchCriteria($lastModified, $processedIds);
                $result = $this->OrderRepository->getList($search);
                
                if ($result->getTotalCount() > 0) {
                    $orders = array_merge($orders, $result->getItems());
                }
            }
        }

        return $this->Writer->writeOrders($orders);
    }

    /**
     * create a search criteria with the given field and direction
     *
     * @param  \string   $date
     * @param  \string[] $excludeOrders
     * @return \Magento\Framework\Api\SearchCriteria
     */
    private function createMissingOrderSearchCriteria($date, $excludeOrders)
    {
        // Create a search criteria and add the sort order and filter group to our search criteria
        $searchCriteria = $this->ObjectManager->create('\Magento\Framework\Api\SearchCriteria');

        // create the filter
        $dateFilter = $this->createFilter('updated_at', 'eq', $date);

        // create the filter
        $idFilter = $this->createFilter('entity_id', 'nin', $excludeOrders);

        // add filter and sort to the search
        $searchCriteria->setFilterGroups($this->createFilterGroup(array($dateFilter, $idFilter)));

        return $searchCriteria;
    }

    /**
     * create a search criteria with the given field and direction
     *
     * @param  \string $date
     * @param  \int    $maxcount
     * @return \Magento\Framework\Api\SearchCriteria
     */
    private function createOrderSearchCriteria($date, $maxcount = 0)
    {
        // Create a search criteria and add the sort order and filter group to our search criteria
        $searchCriteria = $this->ObjectManager->create('\Magento\Framework\Api\SearchCriteria');

        // create the filter
        $filter = $this->createFilter('updated_at', 'gt', $date);

        // add filter and sort to the search
        $searchCriteria->setFilterGroups($this->createFilterGroup(array($filter)))
            ->setSortOrders($this->createSortOrder('updated_at', 'desc'));

        // if there is a maxcount add it as page size
        if ($maxcount > 0) {
            $searchCriteria->setPageSize((int)$maxcount);
        }

        return $searchCriteria;
    }

    /**
     * create a sort order with the given field and direction
     *
     * @param  \string $field
     * @param  \string $direction
     * @return \Magento\Framework\Api\SortOrder
     */
    private function createSortOrder($field, $direction)
    {
        // Create a sort order
        $sortOrder = $this->ObjectManager->create('\Magento\Framework\Api\SortOrder');
        $sortOrder->setField($field)
            ->setDirection($direction);

        return array($sortOrder);
    }

    /**
     * create a filter using the given field, operator and value
     *
     * @param  \string $field
     * @param  \string $operator
     * @param  \string $value
     * @return \Magento\Framework\Api\Filter
     */
    private function createFilter($field, $operator, $value)
    {
        // Create a filter
        $filter = $this->ObjectManager->create('\Magento\Framework\Api\Filter');
        $filter->setField($field)
            ->setConditionType($operator)
            ->setValue($value);

        return $filter;
    }

    /**
     * create a filter group using the given filters
     *
     * @param  \Magento\Framework\Api\Filter[] $filters
     * @return \Magento\Framework\Api\Search\FilterGroup
     */
    private function createFilterGroup($filters)
    {
        // Add the filters to a filter group
        $filterGroup = $this->ObjectManager->create('\Magento\Framework\Api\Search\FilterGroup');
        $filterGroup->setFilters($filters);

        return array($filterGroup);
    }

    /**
     * @param \string[] $data
     * @throws \Magento\Framework\Exception\LocalizedException
     * @return \string
     */
    public function updateOrders($data)
    {
        $this->OrderActionParameters->load($data);

        $order = $this->OrderRepository->get($this->OrderActionParameters->getOrderId());

        try {
            if ($order->canUnhold()) {
                $order->unhold();
            }

            $action = strtolower(trim($this->OrderActionParameters->getAction()));

            switch ($action) {
                case self::COMPLETE:
                    // Try to add tracking first
                    $this->addTracking($order, $this->OrderActionParameters);
                    // assume that tracking added successfully so now we can invoice
                    $this->invoice($order, $this->OrderActionParameters);
                    $order->setStatus(self::COMPLETE);
                    break;
                case self::CANCEL:
                    $this->cancelOrder($order);
                    break;
                case self::HOLD:
                    $this->holdOrder($order);
                    break;
                default:
                    throw new LocalizedException(__("Unknown Order action $action"));
            }
        } catch (LocalizedException $e) {
            $message = $e->getMessage();

            if ($message != "We cannot create an empty shipment.") {
                return $this->Writer->writeException(100, $e->getMessage());
            }
        }
        return $this->Writer->writeSuccess($order, "");
    }

    /**
     * places an order on hold the order
     *
     * @param  \Magento\Sales\Api\Data\OrderInterface $order
     * @throws \Magento\Framework\Exception\LocalizedException
     */
    private function holdOrder($order)
    {
        if (!$order->canHold()) {
            throw new LocalizedException(__('Order cannot be placed on Hold'));
        }

        $order->hold();
        $order->save();
    }

    /**
     * cancels an order
     *
     * @param   \Magento\Sales\Api\Data\OrderInterface $order
     * @returns \bool
     * @throws  \Magento\Framework\Exception\LocalizedException
     */
    private function cancelOrder($order)
    {
        if (!$order->canCancel()) {
            throw new LocalizedException(__('Order cannot be Canceled'));
        }

        $order->cancel();
        $order->save();
    }

    /**
     * Add Tracking
     *
     * @param  \Magento\Sales\Api\Data\OrderInterface                    $order
     * @param  \ShipWorks\Module\Api\OrderActionParametersInterface $params
     * @throws \Magento\Framework\Exception\LocalizedException
     */
    private function addTracking($order, $params)
    {
        if ($order->canShip()) {
            // No Shipments exist so we need to create one
            $shipment = $this->ShipmentFactory->create($order);
        } else {
            // Get the latest shipment
            $shipment = $order->getShipmentsCollection()
                ->addFieldToFilter('order_id', array('eq' => $params->getOrderId()))
                ->setPage(1, 1)
                ->getLastItem();
        }

        if (!$shipment) {
            throw new LocalizedException(__('Unable to load Shipment'));
        }

        $track = $this->TrackFactory->create()
        ->setNumber(
            $params->getTrackingNumber()
        )->setCarrierCode(
            $params->getCarrier()
        )->setTitle(
            $params->getService()
        );

        $shipment->addTrack($track);
        
        foreach ($order->getAllItems() as $item) {
            $item->setQtyShipped($item->getQtyToShip());
            $item->save();
        }

        $transactionSave = $this->ObjectManager->create('Magento\Framework\DB\Transaction')
            ->addObject($shipment)
            ->addObject($order);

        $transactionSave->save();
    }

    /**
     * Invoices the order
     *
     * @param  \Magento\Sales\Api\Data\OrderInterface       $order
     * @param  \ShipWorks\Module\Model\OrderActionParameters
     * @throws \Magento\Framework\Exception\LocalizedException
     */
    private function invoice($order, $params)
    {
        if (!$order->canInvoice()) {
            throw new LocalizedException(__('Order cannot be Invoiced'));
        }

        $invoice = $order->prepareInvoice();

        if (!$invoice) {
            throw new LocalizedException(__('Unable to load Invoice'));
        }

        $invoice->register();

        if ($params->getComments() != '') {
            $invoice->addComment($params->getComments(), $params->getSendInvoiceEmail());
        }


        if ($invoice->canCapture()) {
            $invoice->capture();
        }

        $transactionSave = $this->ObjectManager->create('Magento\Framework\DB\Transaction')
            ->addObject($invoice)
            ->addObject($invoice->getOrder());

        $transactionSave->save();
    }
}
