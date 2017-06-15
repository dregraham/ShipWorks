<?php
/*
    |
    | This file and the source codes contained herein are the property
    | of Interapptive, Inc.  Use of this file is restricted to the specific
    | terms and conditions in the License Agreement associated with this
    | file.  Distribution of this file or portions of this file for uses
    | not covered by the License Agreement is not allowed without a written
    | agreement signed by an officer of Interapptive, Inc.
    |
    | The code contained herein may not be reproduced, copied or
    | redistributed in any form, as part of another product or otherwise.
    | Modified versions of this code may not be sold or redistributed.
    |
    | Copyright 2009-2017 Interapptive, Inc.  All rights reserved.
    | http://www.interapptive.com/
 */

// flag indicating if we will require SSL connection or not (default is true)
define('REQUIRE_SECURE', FALSE);

//set this constant so we can poke into joomla/virtuemart files
define('_JEXEC', true);
define('DS', DIRECTORY_SEPARATOR);

<<<<<<< Updated upstream
define('JPATH_BASE', __DIR__ );

require_once(JPATH_BASE . DS . 'includes' . DS . 'defines.php');
require_once(JPATH_BASE . DS .   'configuration.php');
require_once(JPATH_BASE . DS .  'includes' . DS . 'framework.php');
require_once(JPATH_BASE . DS .  'libraries' . DS . 'joomla' . DS . 'factory.php');
=======
// Initialize Joomla
define('JPATH_BASE', dirname($_SERVER["SCRIPT_FILENAME"]) );

require_once(JPATH_BASE . DS . 'includes' . DS . 'defines.php');
require_once(JPATH_BASE . DS .  'configuration.php');
require_once(JPATH_BASE . DS .  'includes' . DS . 'framework.php');
require_once(JPATH_BASE . DS .  'libraries' . DS . 'joomla' . DS . 'factory.php');
require_once(JPATH_BASE . DS .  'libraries' . DS . 'joomla' . DS . 'user' . DS . 'authentication.php');

$app = JFactory::getApplication('site');

// Initialize Virtuemart extension
define('VMPATH_ROOT', JPATH_BASE);
define('VMPATH_SITE', VMPATH_ROOT . DS . 'components' . DS . 'com_virtuemart');
define('VMPATH_ADMIN', JPATH_ADMINISTRATOR . DS . 'components' . DS . 'com_virtuemart');

require_once(JPATH_BASE .'/administrator/components/com_virtuemart/helpers/config.php');
VmConfig::loadConfig();

// v3+ supports vmLanguage while older versions don't
if (class_exists( 'vmLanguage' ))
{
	vmLanguage::loadJLang('com_virtuemart', true);
	vmLanguage::loadJLang('com_virtuemart_orders', true);
}
else
{
	VmConfig::loadJLang('com_virtuemart', true);
	VmConfig::loadJLang('com_virtuemart_orders', true);
}
>>>>>>> Stashed changes

$moduleVersion = "3.15.0.0";
$schemaVersion = "1.0.0";

header("Last-Modified: " . gmdate("D, d M Y H:i:s") . " GMT");

// turn off caching - the HTTP/1.1 way
header("Content-Type: text/xml;charset=utf-8");
header("Cache-Control: no-store, no-cache, must-revalidate");
header("Cache-Control: post-check=0, pre-check=0", false);

// turn off caching - the HTTP/1.0 way
header("Pragma: no-cache");

<<<<<<< Updated upstream
global $conn;
global $CONFIG;

// PHP 7 removed the mysql extensions, and replaced them with mysqli.
// We need to check to see which is installed and use the appropriate one,
// so the following wrapper methods are used to execute the one that is installed.

// Wrapper method for connecting to the database.
function sqlConnect()
{
	global $conn;
	global $CONFIG;

	if (function_exists('mysqli_connect')) 
	{
	  //mysqli is installed
	  $conn = mysqli_connect($CONFIG->host, $CONFIG->user, $CONFIG->password);
	} 
	elseif (function_exists('mysql_connect')) 
	{
	  //mysql is installed
	  $conn = mysql_connect($CONFIG->host, $CONFIG->user, $CONFIG->password);
	}
	else
	{
	  echo "Neither mysql_connect or mysqli_connect is installed.";
	}
}

// Wrapper method for escaping text to store in the database.
function sqlEscapeString($textToEscape)
{
	global $conn;

	if (function_exists('mysqli_real_escape_string')) 
	{
	  //mysqli is installed
	  return mysqli_real_escape_string($conn, $textToEscape);
	} 
	elseif (function_exists('mysql_real_escape_string')) 
	{
	  //mysql is installed
	  return mysql_real_escape_string($textToEscape);
	}
	else
	{
	  echo "Neither mysql_real_escape_string or mysqli_real_escape_string is installed.";
	}
}

// Wrapper method for executing a query.
function sqlQuery($sql)
{
	global $conn;

	if (function_exists('mysqli_query')) 
	{
	  //mysqli is installed
	  return mysqli_query($conn, $sql);
	} 
	elseif (function_exists('mysql_query')) 
	{
	  //mysql is installed
	  return mysql_query($conn, $sql);
	}
	else
	{
	  echo "Neither mysql_query or mysqli_query is installed.";
	}
}

// Wrapper method for executing a number of rows query.
function sqlNumRows($sql)
{
	if (function_exists('mysqli_num_rows')) 
	{
	  //mysqli is installed
	  return mysqli_num_rows($sql);
	} 
	elseif (function_exists('mysql_num_rows')) 
	{
	  //mysql is installed
	  return mysql_num_rows($sql);
	}
	else
	{
	  echo "Neither mysql_num_rows or mysqli_num_rows is installed.";
	}
}

// Wrapper method for executing fetch array query.
function sqlFetchArray($query)
{
	if (function_exists('mysqli_fetch_array')) 
	{
	  //mysqli is installed
	  return mysqli_fetch_array($query);
	} 
	elseif (function_exists('mysql_fetch_array')) 
	{
	  //mysql is installed
	  return mysql_fetch_array($query);
	}
	else
	{
	  echo "Neither mysql_fetch_array or mysqli_fetch_array is installed.";
	}
}

// Wrapper method for selecting a database.
function sqlSelectDb($dbName)
{
	global $conn;

	if (function_exists('mysqli_select_db')) 
	{
	  //mysqli is installed
	  return mysqli_select_db($conn, $dbName);
	} 
	elseif (function_exists('mysql_select_db')) 
	{
	  //mysql is installed
	  return mysql_select_db($conn, $dbName);
	}
	else
	{
	  echo "Neither mysql_select_db or mysqli_select_db is installed.";
	}
}

// Wrapper method for getting the last error.
function sqlError()
{
	global $conn;

	if (function_exists('mysqli_error')) 
	{
	  //mysqli is installed
	  return mysqli_error($conn);
	} 
	elseif (function_exists('mysql_error')) 
	{
	  //mysql is installed
	  return mysql_error($conn);
	}
	else
	{
	  echo "Neither mysql_error or mysqli_error is installed.";
	}
}

function toUtf8($string)
{
	return iconv("ISO-8859-1", "UTF-8//TRANSLIT", $string);
}

//basic XML helper functions
function writeXmlDeclaration()
{
	echo "<?xml version=\"1.0\" standalone=\"yes\" ?>";
}

function writeStartTag($tag, $attributes = null)
=======
/**
 * Simple iterator for a query
 *
 * Newer versions of Joomla use an iterator to iterate through database results,
 * while older versions don't. This class lets us treat both the same.
 */
class QueryIterator implements Iterator
>>>>>>> Stashed changes
{
	private $current = null;
	private $index = 0;
	private $items = [];
	private $count = 0;

	/**
	 * Abstract getting an object iterator
	 *
	 * Older versions of Joomla do not support getting an iterator, so we'll
	 * need to wrap the functionality
	 */
	public static function Get($db)
	{
		if (method_exists($db, 'getIterator'))
		{
			return $db->getIterator();
		}

		return new QueryIterator($db);
	}

  public function __construct($db)
	{
		$this->items = $db->loadObjectList();
		$this->count = count($this->items);
  }

  public function rewind()
	{
		$this->index = 0;
  }

  public function current()
	{
		return ($this->index < $this->count) ? $this->items[$this->index] : null;
  }

  public function key()
	{
		return $this->index;
  }

  public function next()
	{
		$this->index++;
  }

  public function valid()
	{
		return $this->index < count($this->items);
  }
}

/**
 * Handle XML output
 */
class XmlOutput
{
	//basic XML helper functions
	public static function writeDeclaration()
	{
		echo("<?xml version=\"1.0\" standalone=\"yes\" ?>");
	}

	public static function writeStartTag($tag, $attributes = null)
	{
		echo(XmlOutput::toUtf8('<' . $tag));

		if ($attributes != null)
		{
			echo(' ');

			foreach ($attributes as $name => $attribValue)
			{
				echo(XmlOutput::toUtf8($name . '="' . htmlspecialchars($attribValue) . '" '));
			}
		}

		echo('>');
	}

	public static function writeCloseTag($tag)
	{
		echo(XmlOutput::toUtf8('</' . $tag . '>'));
	}

	// Output the given tag\value pair
	public static function writeElement($tag, $value)
	{
		XmlOutput::writeStartTag($tag);
		echo(XmlOutput::toUtf8(htmlspecialchars($value)));
		XmlOutput::writeCloseTag($tag);
	}

	// Outputs an xml element with the provided attributes
	public static function writeElementWithAttributes($tag, $value, $attributes)
	{
		echo(XmlOutput::toUtf8('<' . $tag . ' '));

		foreach ($attributes as $name => $attribValue)
		{
			echo(XmlOutput::toUtf8($name . '="' . htmlspecialchars($attribValue) . '" '));
		}
		echo('>');
		echo(XmlOutput::toUtf8(htmlspecialchars($value)));
		XmlOutput::writeCloseTag($tag);
	}

	// Function used to output an error and quit.
	public static function outputError($code, $error)
	{
		XmlOutput::writeStartTag("Error");
		XmlOutput::writeElement("Code", $code);
		XmlOutput::writeElement("Description", $error);
		XmlOutput::writeCloseTag("Error");
	}

	private static function toUtf8($string)
	{
		return iconv("ISO-8859-1", "UTF-8//TRANSLIT", $string);
	}
<<<<<<< Updated upstream
	sqlConnect($CONFIG->host, $CONFIG->user, $CONFIG->password);
=======
>>>>>>> Stashed changes
}

//are we on an SSL connection?
$secure = array_key_exists('HTTPS', $_SERVER) && ($_SERVER['HTTPS'] == 'on' || $_SERVER['HTTPS'] == '1');

// Open the XML output and root
XmlOutput::writeDeclaration();
XmlOutput::writeStartTag(
	"ShipWorks",
	array("moduleVersion" => $moduleVersion, "schemaVersion" => $schemaVersion)
	);

// Enforce SSL
if (!$secure && REQUIRE_SECURE)
{
	XmlOutput::outputError(10, 'Invalid URL, HTTPS is required');
}
else
{
	// verify that the credentials are valid
	if (checkAdminLogin())
	{
		$action = (isset($_REQUEST['action']) ? $_REQUEST['action'] : '');

		switch (strtolower($action)) {
			case 'getmodule':
				Action_GetModule();
				break;
			case 'getstore':
				Action_GetStore();
				break;
			case 'getcount':
				Action_GetCount();
				break;
			case 'getorders':
				Action_GetOrders();
				break;
			case 'getstatuscodes':
				Action_GetStatusCodes();
				break;
			case 'updatestatus':
				Action_UpdateStatus();
				break;
			case 'debug':
				Action_Debug();
				break;
			default:
				XmlOutput::outputError(20, "Invalid action '$action'");
		}
	}
}

// Close the output
XmlOutput::writeCloseTag("ShipWorks");

// Determines the root web location for product images
function GetImageRoot()
{
	$scriptPath = $_SERVER["SCRIPT_NAME"];
	$serverName = $_SERVER["SERVER_NAME"];
	$hostName = $_SERVER["HTTP_HOST"];

	// get the host name to the website since it isn't stored anywhere
	if (strtoupper($serverName) == "LOCALHOST" || $serverName == "127.0.0.1") {
		// default to the http request host used to access the server
		$serverName = $hostName;
	}

	preg_match('/(.*)\//', $scriptPath, $matches);
	$cartRoot = "$serverName$matches[1]";

	return "http://$cartRoot/";
}

// Debugging
function Action_Debug()
{
	$imageRoot = GetImageRoot();

	print "\n\nENCODE_KEY = " . ENCODE_KEY;
	print "\n\nVM_ENCRYPT_FUNCTION = " . @VM_ENCRYPT_FUNCTION;
	print "\n\nVM_DECRYPT_FUNCTION = " . VM_DECRYPT_FUNCTION;
	print "\n\nImage Root = $imageRoot";
}

// Check to see if admin functions exist.  And if so, determine if the user
// has access.
function checkAdminLogin()
{
<<<<<<< Updated upstream
	global $dbPrefix;
	global $conn;
	$loginOK = false;

	if (isset($_REQUEST['username']) && isset($_REQUEST['password'])) {
		$username = $_REQUEST['username'];
		$password = $_REQUEST['password'];

		$sql = "SELECT id, password"
			. " FROM " . $dbPrefix . "users"
			. " WHERE username='" . sqlEscapeString($username) . "'";
		
		$check_adminquery = sqlQuery($sql);
		
		//$check_adminquery is false...
		//Joomla < 2.5.18 method of auth
		if (sqlNumRows($check_adminquery)) {
			$check_admin = sqlFetchArray($check_adminquery);

			// Check that password is good
			$parts = explode(':', $check_admin['password']);
			$crypt = $parts[0];
			$salt = @$parts[1];
			$testcrypt = getCryptedPassword($password, $salt);

			if ($crypt == $testcrypt) {
				$loginOK = true;
			}
		}

		//Attempt this only if $loginOK is still false
		//Joomla > 2.5.17 method of auth
		if (!$loginOK) {
			$db = JFactory::getDBO();

			$query = "SELECT id,username, password FROM " . $dbPrefix
				. "users WHERE username = '$username'";
			$db->setQuery($query);

			if ($result = $db->loadObject()) {
				$match = JUserHelper::verifyPassword(
					$password, $result->password, $result->id
					);
=======
	if (!isset($_REQUEST['username']) || !isset($_REQUEST['password']))
	{
		outputError(40, "Insufficient parameters");
		return false;
	}
>>>>>>> Stashed changes

	$authenticate = JAuthentication::getInstance();
	$response = $authenticate->authenticate([
		'username' => $_REQUEST['username'],
		'password' => $_REQUEST['password']
	]);

	if ($response->status !== JAuthentication::STATUS_SUCCESS)
	{
		XmlOutput::outputError(50, "Username or password is incorrect");
		return false;
	}

	return true;
}

// Get module data
function Action_GetModule()
{
	XmlOutput::writeStartTag("Module");

	XmlOutput::writeElement("Platform", "VirtueMart");
	XmlOutput::writeElement("Developer", "Interapptive, Inc. (support@interapptive.com)");

	XmlOutput::writeStartTag("Capabilities");
	XmlOutput::writeElement("DownloadStrategy", "ByModifiedTime");
	XmlOutput::writeElementWithAttributes(
		"OnlineCustomerID", "", array("supported" => "true")
		);
	XmlOutput::writeElementWithAttributes(
		"OnlineStatus", "",
		array("supported" => "true", "supportsComments" => "true")
		);
	XmlOutput::writeElementWithAttributes(
		"OnlineShipmentUpdate", "", array("supported" => "false")
		);
	XmlOutput::writeCloseTag("Capabilities");

	XmlOutput::writeCloseTag("Module");
}

/**
 * Get the 2 letter code of a country given its id
 */
function GetCountryCodeFromId($id)
{
	$db = JFactory::getDBO();
	$query = $db->getQuery(true)
		->select('country_2_code')
		->from('#__virtuemart_countries')
		->where("virtuemart_country_id = $id");
	$db->setQuery($query, 0, 1);
	return GetValue($db->loadObject(), 'country_2_code');
}

/**
 * Get the name of a state given its id
 */
function GetStateNameFromId($id)
{
	$db = JFactory::getDBO();
	$query = $db->getQuery(true)
		->select('state_name')
		->from('#__virtuemart_states')
		->where("virtuemart_state_id = $id");
	$db->setQuery($query, 0, 1);
	return GetValue($db->loadObject(), 'state_name');
}

// Write store data
function Action_GetStore()
{
<<<<<<< Updated upstream
	global $dbPrefix;
	global $conn;
	$sql = "SELECT * FROM " . $dbPrefix . "virtuemart_vmusers vmu" .
		" inner join " . $dbPrefix . "users u on u.id=vmu.virtuemart_user_id" .
		" inner join " . $dbPrefix
		. "virtuemart_userinfos ui on ui.virtuemart_user_id = vmu.virtuemart_user_id"
		.
		" inner join " . $dbPrefix
		. "virtuemart_vendors_en_gb en on en.virtuemart_vendor_id = vmu.virtuemart_vendor_id"
		.
		" inner join " . $dbPrefix
		. "virtuemart_states s on s.virtuemart_state_id = ui.virtuemart_state_id"
		.
		" inner join " . $dbPrefix
		. "virtuemart_countries c on c.virtuemart_country_id = ui.virtuemart_country_id"
		.
		" WHERE user_is_vendor=1 " .
		" ORDER BY vmu.virtuemart_vendor_id LIMIT 1";

	$vendorquery = sqlQuery($sql);
	$vendor = sqlFetchArray($vendorquery);

	writeStartTag("Store");
	writeElement("Name", str_replace("\\", "", $vendor['name']));
	writeElement("CompanyOrOwner", str_replace("\\", "", $vendor['company']));
	writeElement("Email", $vendor['email']);
	writeElement("Street1", $vendor['address_1']);
	writeElement("Street2", $vendor['address_2']);
	writeElement("City", $vendor['city']);
	writeElement("State", $vendor['state_name']);
	writeElement("PostalCode", $vendor['zip']);
	writeElement("Country", substr($vendor['country_2_code'], 0, 2));
	writeElement("Phone", $vendor['phone_1']);
	writeElement("Website", $vendor['vendor_url']);

	writeCloseTag("Store");
=======
	$vendorModel = VmModel::getModel('vendor');

	$vendor = $vendorModel->getVendor();
	$email = $vendorModel->getVendorEmail($vendor->virtuemart_vendor_id);
	$address = $vendorModel->getVendorAdressBT($vendor->virtuemart_vendor_id);

	$country = GetCountryCodeFromId($address->virtuemart_country_id);
	$stateName = GetStateNameFromId($address->virtuemart_state_id);

	VmModel::getModel('country')->getCountryByCode($address->virtuemart_country_id);

	XmlOutput::writeStartTag("Store");

	XmlOutput::writeElement("Name", str_replace("\\", "", GetValue($vendor, 'vendor_store_name')));
	XmlOutput::writeElement("CompanyOrOwner", str_replace("\\", "", GetValue($address, 'company')));
	XmlOutput::writeElement("Email", $email);
	XmlOutput::writeElement("Street1", GetValue($address, 'address_1'));
	XmlOutput::writeElement("Street2", GetValue($address, 'address_2'));
	XmlOutput::writeElement("City", GetValue($address, 'city'));
	XmlOutput::writeElement("State", $stateName);
	XmlOutput::writeElement("PostalCode", GetValue($address, 'zip'));
	XmlOutput::writeElement("Country", $country);
	XmlOutput::writeElement("Phone", GetValue($address, 'phone_1'));
	XmlOutput::writeElement("Website", GetValue($vendor, 'vendor_url'));

	XmlOutput::writeCloseTag("Store");
}

/**
 * Get a value from an object if it's set
 */
function GetValue($obj, $prop)
{
	return $obj != null && property_exists($obj, $prop) ? $obj->$prop : '';
>>>>>>> Stashed changes
}

// Get the count of orders greater than the start ID
function Action_GetCount()
{
<<<<<<< Updated upstream
	global $dbPrefix;
	global $conn;
	$start = GetStart();

	// Write the params for easier diagnostics
	writeStartTag("Parameters");
	writeElement("Start", $start);
	writeCloseTag("Parameters");

	$ordersQuery = sqlQuery(
		"select count(*) as count " .
		" from " . $dbPrefix . "virtuemart_orders o " .
		" where modified_on > '$start'"
		);
	$count = 0;

	if (sqlNumRows($ordersQuery)) {
		$rows = sqlFetchArray($ordersQuery);
		$count = $rows['count'];
	}

	writeElement("OrderCount", $count);
=======
	$start = GetStart();

	// Write the params for easier diagnostics
	XmlOutput::writeStartTag("Parameters");
	XmlOutput::writeElement("Start", $start);
	XmlOutput::writeCloseTag("Parameters");

	$db = JFactory::getDBO();
	$startParam = $db->quote($start, true);
	$query = $db->getQuery(true)
		->select('count(*) as count')
		->from('#__virtuemart_orders')
		->where("modified_on > $startParam");
	$db->setQuery($query);
	$count = $db->loadObject()->count;

	XmlOutput::writeElement("OrderCount", $count);
>>>>>>> Stashed changes
}

// Get all orders greater than the given start id, limited by max count
function Action_GetOrders()
{
<<<<<<< Updated upstream
	global $dbPrefix;
	global $secure;
	global $conn;

	$maxcount = 50;

=======
	$maxcount = isset($_POST['maxcount']) ? $_POST['maxcount'] : 50;
>>>>>>> Stashed changes
	$start = GetStart();

	// Only get orders through 2 seconds ago.
	$end = toFormattedDateFromObject(time() - 2);

	// Write the params for easier diagnostics
<<<<<<< Updated upstream
	writeStartTag("Parameters");
	writeElement("Start", $start);
	writeElement("MaxCount", $maxcount);
	writeCloseTag("Parameters");


	writeStartTag("Orders");

	$sql = "select * " .
		" from " . $dbPrefix . "virtuemart_orders " .
		" where modified_on > '$start'  " .
		"    and modified_on <= '$end' " .
		" order by modified_on asc " .
		" limit 0, " . $maxcount;

	$ordersQuery = sqlQuery($sql);


	$lastModified = null;
	$processedIds = "";

	while ($row = sqlFetchArray($ordersQuery)) {
		// keep track of the most current processed modified time
		$lastModified = $row['modified_on'];
=======
	XmlOutput::writeStartTag("Parameters");
	XmlOutput::writeElement("Start", $start);
	XmlOutput::writeElement("MaxCount", $maxcount);
	XmlOutput::writeCloseTag("Parameters");

	XmlOutput::writeStartTag("Orders");

	$db = JFactory::GetDBO();
	$query = $db->getQuery(true)
		->select('*')
		->from("#__virtuemart_orders")
		->where('modified_on > ' . $db->quote($start, true) . ' and modified_on <= ' . $db->quote($end, true))
		->order('modified_on ASC');
	$db->setQuery($query, 0, $maxcount);

	$orderIterator = QueryIterator::Get($db);

	$lastModified = null;
	$processedIds = [];
>>>>>>> Stashed changes

	foreach ($orderIterator as $order)
	{
		$lastModified = $order->modified_on;
		$processedIds[] = $order->virtuemart_order_id;

		WriteOrder($order);
	}

<<<<<<< Updated upstream
	// If we processed some orders, we may have to get some more
	if ($processedIds != "") {
		// This make sure we don't split a page between orders of the same modified time
		// If there's any that didn't make the maxcount cutoff with the same last modified time
		// as the greatest last modified time we already processed, this will get them
		$moreQuery = sqlQuery(
			"select * " .
			"from " . $dbPrefix . "virtuemart_orders o " .
			"where virtuemart_order_id not in ($processedIds) " .
			"and modified_on = '$lastModified'"
			);

		while ($row = sqlFetchArray($ordersQuery)) {
			WriteOrder($row);
		}
	}
=======
	WriteExtraOrders($db, $processedIds, $lastModified);
>>>>>>> Stashed changes

	XmlOutput::writeCloseTag("Orders");
}

/**
 * Write any extra orders
 *
 * This make sure we don't split a page between orders of the same modified time
 * If there's any that didn't make the maxcount cutoff with the same last modified time
 * as the greatest last modified time we already processed, this will get them
 */
function WriteExtraOrders($db, $processedIds, $lastModified)
{
	if (count($processedIds) === 0)
	{
		return;
	}

	$idList = implode(',', $processedIds);
	$modified = $db->quote($lastModified, true);

	$query = $db->getQuery(true)
		->select('*')
		->from("#__virtuemart_orders")
		->where("virtuemart_order_id not in ($idList) and modified_on = $modified")
		->order('modified_on ASC');
	$db->setQuery($query);

	$orderIterator = QueryIterator::Get($db);

	foreach ($orderIterator as $order)
	{
		WriteOrder($order);
	}
}

function GetStart()
{
	return (isset($_POST['start'])) ? $_POST['start'] : "1900-01-01T00:00:00";
}

/**
 * Write an individual order
 */
function WriteOrder($order)
{
<<<<<<< Updated upstream
	global $dbPrefix;
	global $conn;

	writeStartTag("Order");
	writeElement("OrderNumber", $row['virtuemart_order_id']);
	writeElement("OrderDate", toFormattedDateFromString($row['created_on']));
	writeElement(
		"LastModified", toFormattedDateFromString($row['modified_on'])
=======
	XmlOutput::writeStartTag("Order");
	XmlOutput::writeElement("OrderNumber", $order->virtuemart_order_id);
	XmlOutput::writeElement("OrderDate", toFormattedDateFromString($order->created_on));
	XmlOutput::writeElement(
		"LastModified", toFormattedDateFromString($order->modified_on)
>>>>>>> Stashed changes
		);
	XmlOutput::writeElement(
		"ShippingMethod",
		GetShippingMethod($order->virtuemart_shipmentmethod_id)
		);
	XmlOutput::writeElement("StatusCode", ord($order->order_status));
	XmlOutput::writeElement("CustomerID", $order->virtuemart_user_id);

<<<<<<< Updated upstream
	$sqlAddressWithoutType
		= "select u.*, s.state_name, c.country_name from " . $dbPrefix
		. "virtuemart_order_userinfos u" .
		" left outer join " . $dbPrefix
		. "virtuemart_states s on s.virtuemart_state_id = u.virtuemart_state_id"
		.
		" inner join " . $dbPrefix
		. "virtuemart_countries c on c.virtuemart_country_id = u.virtuemart_country_id"
		.
		" where virtuemart_order_id = " . $row['virtuemart_order_id']
		. " AND address_type=";

	$customerBTQuery = sqlQuery($sqlAddressWithoutType . "'BT'");
	$customerBT = sqlFetchArray($customerBTQuery);

	$customerSTQuery = sqlQuery($sqlAddressWithoutType . "'ST'");
	$customerST = sqlFetchArray($customerSTQuery);

	if ($customerST == null) {
		$customerST = $customerBT;
	}

	writeStartTag("ShippingAddress");
	writeElement("FirstName", $customerST['first_name']);
	writeElement("LastName", $customerST['last_name']);
	writeElement("Company", $customerST['company']);
	writeElement("Street1", $customerST['address_1']);
	writeElement("Street2", $customerST['address_2']);
	writeElement("City", $customerST['city']);
	writeElement("State", $customerST['state_name']);
	writeElement("PostalCode", $customerST['zip']);
	writeElement("Country", $customerST['country_name']);
	writeElement("Phone", $customerST['phone_1']);
	writeElement("Email", $customerST['email']);
	writeCloseTag("ShippingAddress");

	writeStartTag("BillingAddress");
	writeElement("FirstName", $customerBT['first_name']);
	writeElement("LastName", $customerBT['last_name']);
	writeElement("Company", $customerBT['company']);
	writeElement("Street1", $customerBT['address_1']);
	writeElement("Street2", $customerBT['address_2']);
	writeElement("City", $customerBT['city']);
	writeElement("State", $customerBT['state_name']);
	writeElement("PostalCode", $customerBT['zip']);
	writeElement("Country", $customerBT['country_name']);
	writeElement("Phone", $customerBT['phone_1']);
	writeElement("Email", $customerBT['email']);
	writeCloseTag("BillingAddress");

	writeStartTag("Payment");
	writeElement(
		"Method", GetPaymentMethod($row['virtuemart_paymentmethod_id'])
		);
	writeCloseTag("Payment");
=======
	$billingAddress = WriteAddress($order->virtuemart_order_id, "BT", "BillingAddress", null);
	WriteAddress($order->virtuemart_order_id, "ST", "ShippingAddress", $billingAddress);

	XmlOutput::writeStartTag("Payment");
	XmlOutput::writeElement(
		"Method",
		GetPaymentMethod($order->virtuemart_paymentmethod_id));
	XmlOutput::writeCloseTag("Payment");

	WriteOrderItems($order->virtuemart_order_id);
	WriteOrderTotals($order);
	WriteNotes($order);
>>>>>>> Stashed changes

	XmlOutput::writeCloseTag("Order");
}

/**
 * Write notes for an order
 */
function WriteNotes($order)
{
	if (isset($order->customer_note))
	{
		XmlOutput::writeStartTag("Notes");
		XmlOutput::writeElementWithAttributes("Note", $order->customer_note, ['public' => 'true']);
		XmlOutput::writeCloseTag("Notes");
	}
}

/**
 * Write an order address
 */
function WriteAddress($orderId, $addressType, $elementName, $fallbackAddress)
{
	$db = JFactory::getDBO();
	$type = $db->quote($addressType, true);

	$query = $db->getQuery(true)
		->select("u.*, s.state_name, c.country_name")
		->from("#__virtuemart_order_userinfos u")
		->leftJoin("#__virtuemart_states s ON s.virtuemart_state_id = u.virtuemart_state_id")
		->innerJoin("#__virtuemart_countries c ON c.virtuemart_country_id = u.virtuemart_country_id")
		->where("virtuemart_order_id = $orderId AND address_type = $type");

	$db->setQuery($query);
	$customerAddress = $db->loadObject();

	if ($customerAddress == null)
	{
		$customerAddress = $fallbackAddress;
	}

	XmlOutput::writeStartTag($elementName);
	XmlOutput::writeElement("FirstName", $customerAddress->first_name);
	XmlOutput::writeElement("LastName", $customerAddress->last_name);
	XmlOutput::writeElement("Company", $customerAddress->company);
	XmlOutput::writeElement("Street1", $customerAddress->address_1);
	XmlOutput::writeElement("Street2", $customerAddress->address_2);
	XmlOutput::writeElement("City", $customerAddress->city);
	XmlOutput::writeElement("State", $customerAddress->state_name);
	XmlOutput::writeElement("PostalCode", $customerAddress->zip);
	XmlOutput::writeElement("Country", $customerAddress->country_name);
	XmlOutput::writeElement("Phone", $customerAddress->phone_1);
	XmlOutput::writeElement("Email", $customerAddress->email);
	XmlOutput::writeCloseTag($elementName);

	return $customerAddress;
}

// returns the corresponding shipping name
function GetShippingMethod($shipping_method_id)
{
	$db = JFactory::getDBO();
	$query = $db->getQuery(true)
		->select("shipment_name")
		->from("#__virtuemart_shipmentmethods_" . VMLANG)
		->where("virtuemart_shipmentmethod_id = $shipping_method_id");
	$db->setQuery($query);
	$result = $db->loadObject();

	return ($result == null) ? "" : $result->shipment_name;
}

// returns the corresponding payment method row
function GetPaymentMethod($payment_method_id)
{
	$db = JFactory::getDBO();
	$query = $db->getQuery(true)
		->select("payment_name")
		->from("#__virtuemart_paymentmethods_" . VMLANG)
		->where("virtuemart_paymentmethod_id = $payment_method_id");
	$db->setQuery($query);
	$result = $db->loadObject();

	return ($result == null) ? "" : $result->payment_name;
}

<<<<<<< Updated upstream
	global $dbPrefix;
	global $conn;
=======
// Write all totals lines for the order
function WriteOrderTotals($order)
{
	XmlOutput::writeStartTag("Totals");

	WriteTotalIfGreaterThanZero("Sub-Total", $order->order_subtotal, "subtotal", "none");
	WriteTotalIfGreaterThanZero("Total", $order->order_total, "total", "none");
	WriteTotalIfGreaterThanZero("Shipping", $order->order_shipment, "shipping");
	WriteTotalIfGreaterThanZero('Tax', $order->order_tax, 'tax');
	WriteTotalIfGreaterThanZero('Shipping Tax', $order->order_shipment_tax, 'shipping tax');

	if ($order->order_discountAmount != 0)
	{
		$title = ($order->order_discountAmount > 0) ? 'Fee' : 'Discount';
		WriteTotal($title, $order->order_discountAmount, $title, "add");
	}
>>>>>>> Stashed changes

	$title = "Coupon ($order->coupon_code)";
	WriteTotalIfGreaterThanZero($title, $order->coupon_discount, $title, "subtract");

<<<<<<< Updated upstream
	$q = sqlQuery($sql);

	if ($q) {
		$row = sqlFetchArray($q);
		return $row['shipment_name'];
	} else {
		return "";
=======
	XmlOutput::writeCloseTag("Totals");
}

/**
 * Write a total element if the value is greater than zero
 */
function WriteTotalIfGreaterThanZero($title, $value, $class, $impact = "add")
{
	if ($value > 0)
	{
		WriteTotal($title, $value, $class, $impact);
>>>>>>> Stashed changes
	}
}

/**
 * Write a total element
 */
function WriteTotal($title, $value, $class, $impact = "add")
{
	XmlOutput::writeElementWithAttributes(
		"Total", $value,
		array("name" => $title, "class" => $class, "impact" => $impact)
		);
}

/**
 * Write order items
 */
function WriteOrderItems($orderID)
{
<<<<<<< Updated upstream
	global $dbPrefix;
	global $conn;
=======
	$imageRoot = GetImageRoot();
>>>>>>> Stashed changes

	$db = JFactory::getDBO();
	$query = $db->getQuery(true)
		->select("*")
		->from("#__virtuemart_order_items")
		->where("virtuemart_order_id = $orderID");
	$db->setQuery($query);
	$itemIterator = QueryIterator::Get($db);

<<<<<<< Updated upstream
	$q = sqlQuery($sql);
	if ($q) {
		$row = sqlFetchArray($q);
		return $row['payment_name'];
	} else {
		return "";
=======
	XmlOutput::writeStartTag("Items");

	foreach ($itemIterator as $item)
	{
		WriteOrderItem($item, $imageRoot);
>>>>>>> Stashed changes
	}

	XmlOutput::writeCloseTag("Items");
}

/**
 * Write an individual order item
 */
function WriteOrderItem($item, $imageRoot)
{
	$db = JFactory::getDBO();
	$query = $db->getQuery(true)
		->select("p.*, m.file_url_thumb")
		->from("#__virtuemart_products p")
		->leftJoin("#__virtuemart_product_medias pm on pm.virtuemart_product_id = p.virtuemart_product_id")
		->leftJoin("#__virtuemart_medias m on m.virtuemart_media_id = pm.virtuemart_media_id")
		->where("p.virtuemart_product_id = $item->virtuemart_product_id");
	$db->setQuery($query);
	$product = $db->loadObject();

	$imageUrl = $product->file_url_thumb;
	if (isset($imageUrl) and strlen($imageUrl) > 0) {
		$imageUrl = $imageRoot . $imageUrl;
	}

	XmlOutput::writeStartTag("Item");
	XmlOutput::writeElement("ItemID", $item->virtuemart_order_item_id);
	XmlOutput::writeElement("ProductID", $item->virtuemart_product_id);
	XmlOutput::writeElement("Code", $item->order_item_sku);
	XmlOutput::writeElement("SKU", $item->order_item_sku);
	XmlOutput::writeElement("Name", $item->order_item_name);
	XmlOutput::writeElement("Quantity", $item->product_quantity);
	XmlOutput::writeElement("UnitPrice", $item->product_item_price);
	XmlOutput::writeElement("Image", $imageUrl);

	// assuming everything is in pounds (the default)
	$weightFactor = 1;
	$uom = $product->product_weight_uom;
	if (preg_match('/^(oz|ounce)/i', $uom)) {
		$weightFactor = 1 / 16;
	}

	XmlOutput::writeElement("Weight", $product->product_weight * $weightFactor);

	WriteItemAttributes($item);

	XmlOutput::writeCloseTag("Item");
}

/**
 * Write attributes (variants) for an item
 */
function WriteItemAttributes($item)
{
	XmlOutput::writeStartTag("Attributes");

	if (strlen($item->product_attribute) > 0) {
		$isArrayBased = version_compare(vmVersion::$RELEASE, '3') >= 0;

		if ($isArrayBased)
		{
			WriteItemAttributesFromArrayValues($item);
		}
		else
		{
			$attributeDefinitions = json_decode($item->product_attribute, true);
			WriteItemAttributesFromStringValues($attributeDefinitions);
		}
	}

	XmlOutput::writeCloseTag("Attributes");
}

/**
 * Write attributes (variants) for an item using the embedded array
 *
 * This method and its children were adapted from
 * VirtueMartCustomFieldRenderer::renderCustomfieldsCart(product, html, trigger)
 */
function WriteItemAttributesFromArrayValues($item)
{
	$variantmods = $item->product_attribute;
	if (!is_array($variantmods))
	{
		$variantmods = json_decode($variantmods,true);
	}

	$productDB = VmModel::getModel('product')->getProduct($item->virtuemart_product_id);
	$productCustoms = array();

	foreach ((array)$productDB->customfields as $prodcustom)
	{
		//We just add the customfields to be shown in the cart to the variantmods
		if (is_object($prodcustom))
		{
			if ($prodcustom->is_cart_attribute or $prodcustom->is_input)
			{
				if (!isset($variantmods[$prodcustom->virtuemart_custom_id]) or
					!is_array($variantmods[$prodcustom->virtuemart_custom_id]))
				{
					$variantmods[$prodcustom->virtuemart_custom_id] = array();
				}
			}

			$productCustoms[$prodcustom->virtuemart_customfield_id] = $prodcustom;
		}
	}

	foreach ((array)$variantmods as $custom_id => $customfield_ids)
	{
		if (!is_array($customfield_ids))
		{
			$customfield_ids = array( $customfield_ids =>false);
		}

		foreach ($customfield_ids as $customfield_id => $params)
		{
			if (empty($productCustoms) or !isset($productCustoms[$customfield_id]))
			{
				continue;
			}

			$productCustom = $productCustoms[$customfield_id];
			//vmdebug('displayProductCustomfieldSelected ',$customfield_id,$productCustom);
			//The stored result in vm2.0.14 looks like this {"48":{"textinput":{"comment":"test"}}}
			//and now {"32":[{"invala":"100"}]}

			if (!empty($productCustom))
			{
				switch($productCustom->field_type)
				{
					case 'E':
						WriteAttributeForFieldTypeE($productCustom, $item);
						break;
					case 'G':
						WriteAttributeForFieldTypeG($productCustom);
						break;
					case 'A':
						WriteAttributeForFieldTypeA($productCustom, $item);
						break;
					case 'C':
						WriteAttributeForFieldTypeC($productCustom, $item);
						break;
					case 'M':
						WriteAttributeForFieldTypeM($productCustom);
						break;
					case 'S':
						WriteAttributeForFieldTypeS($productCustom, $params);
						break;
					default:
						break;
				}
			}
		}
	}
}

/**
 * Write element for attributes with field_type of 'E'
 *
 * This method and its children were adapted from
 * VirtueMartCustomFieldRenderer::renderCustomfieldsCart(product, html, trigger)
 */
function WriteAttributeForFieldTypeE($productCustom, $product)
{
	if (!class_exists ('vmCustomPlugin'))
		require(VMPATH_PLUGINLIBS . DS . 'vmcustomplugin.php');
	JPluginHelper::importPlugin('vmcustom');
	$dispatcher = JDispatcher::getInstance();
	$dispatcher->trigger($trigger.'VM3', array(&$product, &$productCustom, &$tmp));
}

/**
 * Write element for attributes with field_type of 'G'
 *
 * This method and its children were adapted from
 * VirtueMartCustomFieldRenderer::renderCustomfieldsCart(product, html, trigger)
 */
function WriteAttributeForFieldTypeG($productCustom)
{
	$db = JFactory::getDBO();
	$query = $db->getQuery(true)
		->select('product_name')
		->from('#__virtuemart_products_' . VMLANG)
		->where("virtuemart_product_id = $productCustom->customfield_value");
	$db->setQuery($query);
	$child = $db->loadObject();
	WriteAttributeElement($child->product_name, '');
}

/**
 * Write element for attributes with field_type of 'M'
 *
 * This method and its children were adapted from
 * VirtueMartCustomFieldRenderer::renderCustomfieldsCart(product, html, trigger)
 */
function WriteAttributeForFieldTypeM($productCustom)
{
<<<<<<< Updated upstream
	global $dbPrefix;
	global $conn;
	$imageRoot = GetImageRoot();

	$itemQuery = sqlQuery(
		"select * from " . $dbPrefix . "virtuemart_order_items" .
		" where virtuemart_order_id = " . $orderID
		);

	writeStartTag("Items");

	while ($item = sqlFetchArray($itemQuery)) {
		// Get product info
		$sql = "select p.*,m.file_url_thumb" .
			" from " . $dbPrefix . "virtuemart_products p" .
			" inner join " . $dbPrefix
			. "virtuemart_product_medias pm on pm.virtuemart_product_id = p.virtuemart_product_id"
			.
			" inner join " . $dbPrefix
			. "virtuemart_medias m on m.virtuemart_media_id = pm.virtuemart_media_id"
			.
			" where p.virtuemart_product_id = "
			. $item['virtuemart_product_id'];
		$productQuery = sqlQuery($sql);

		$product = sqlFetchArray($productQuery);

		// Build fully qualified image url
		$imageUrl = $product['file_url_thumb'];
		if (isset($imageUrl) and strlen($imageUrl) > 0) {
			$imageUrl = $imageRoot . $imageUrl;
		}
=======
	$customFieldModel = VmModel::getModel('customfields');
	$value = $customFieldModel->displayCustomMedia(
		$productCustom->customfield_value,
		'product',
		$productCustom->width,
		$productCustom->height,
		VirtueMartModelCustomfields::$useAbsUrls);

	WriteAttributeElement('Media', $value);
}

/**
 * Write element for attributes with field_type of 'S'
 *
 * This method and its children were adapted from
 * VirtueMartCustomFieldRenderer::renderCustomfieldsCart(product, html, trigger)
 */
function WriteAttributeForFieldTypeS($productCustom)
{
	if ($productCustom->is_list and $productCustom->is_input)
	{
		$value = ($productCustom->is_list == 2) ? $productCustom->customfield_value : $params;
	}
	else
	{
		$value = $productCustom->customfield_value;
	}

	WriteAttributeElement('Custom', vmText::_($value));
}
>>>>>>> Stashed changes

/**
 * Write element for attributes with field_type of 'A'
 *
 * This method and its children were adapted from
 * VirtueMartCustomFieldRenderer::renderCustomfieldsCart(product, html, trigger)
 */
function WriteAttributeForFieldTypeA($productCustom, $product)
{
	if (!property_exists($product, $productCustom->customfield_value))
	{
		$productDB = VmModel::getModel('product')->getProduct($product->virtuemart_product_id);

		if($productDB)
		{
			$attr = $productCustom->customfield_value;
			$product->$attr = $productDB->$attr;
		}
	}

	$value = vmText::_( $product->{$productCustom->customfield_value} );
	WriteAttributeElement('Custom', $value);
}

/**
 * Write element for attributes with field_type of 'C'
 *
 * This method and its children were adapted from
 * VirtueMartCustomFieldRenderer::renderCustomfieldsCart(product, html, trigger)
 */
function WriteAttributeForFieldTypeC($productCustom, $item)
{
	$optionId = $item->virtuemart_product_id;
	foreach($productCustom->options->$optionId as $key => $option)
	{
		$name = '';
		$value = '';

		if (!empty($productCustom->selectoptions[$key]->clabel) and
				in_array($productCustom->selectoptions[$key]->voption, VirtueMartModelCustomfields::$dimensions))
		{
			$name = vmText::_('COM_VIRTUEMART_' . $productCustom->selectoptions[$key]->voption);

			$rd = $productCustom->selectoptions[$key]->clabel;

			if (is_numeric($rd) and is_numeric($option))
			{
				$value = number_format(round((float)$option, (int)$rd), $rd);
			}
		}
		else
		{
			if (!empty($productCustom->selectoptions[$key]->clabel))
			{
				$name = vmText::_($productCustom->selectoptions[$key]->clabel);
			}

			$value = vmText::_($option);
		}

		WriteAttributeElement($name, $value);
	}
}

/**
 * Write attributes (variants) for an item using the strings contained
 * in the order item
 *
 * We're just going to output each attribute without a price because the
 * price in the database includes various taxes, while the subtotal does not.
 * This would cause shipworks to incorrectly display values
 */
function WriteItemAttributesFromStringValues($attributeDefinitions)
{
	foreach ($attributeDefinitions as $customfield_id => $valueHtml) {
		$xml = new SimpleXMLElement("<root>" . $valueHtml . "</root>");

		$nameElement = $xml->xpath("/root/span[@class='costumTitle']");
		$valueElement = $xml->xpath("/root/span[@class='costumValue']");

		$name = (string)$nameElement[0];
		$value = (string)$valueElement[0];

		WriteAttributeElement($name, $value);
	}
}

/**
 * Write an attribute element
 */
function WriteAttributeElement($name, $value, $price = 0) {
	XmlOutput::writeStartTag("Attribute");
	XmlOutput::writeElement("Name", trim($name));
	XmlOutput::writeElement("Value", trim($value));
	XmlOutput::writeElement("Price", $price);
	XmlOutput::writeCloseTag("Attribute");
}

// Return the collection of valid status codes
function Action_GetStatusCodes()
{
<<<<<<< Updated upstream
	global $dbPrefix;
	global $conn;
	writeStartTag("StatusCodes");

	$codesQuery = sqlQuery(
		"SELECT * FROM " . $dbPrefix . "virtuemart_orderstates"
		);

	while ($row = sqlFetchArray($codesQuery)) {
		writeStartTag("StatusCode");
		writeElement("Code", ord($row['order_status_code']));
		writeElement("Name", $row['order_status_name']);
		writeCloseTag("StatusCode");
=======
	$db = JFactory::getDBO();
	$query = $db->getQuery(true)
		->select('order_status_code, order_status_name')
		->from('#__virtuemart_orderstates');
	$db->setQuery($query);
	$codesIterator = QueryIterator::Get($db);

	XmlOutput::writeStartTag("StatusCodes");

	foreach ($codesIterator as $code)
	{
		XmlOutput::writeStartTag("StatusCode");
		XmlOutput::writeElement("Code", ord($code->order_status_code));
		XmlOutput::writeElement("Name", vmText::_($code->order_status_name));
		XmlOutput::writeCloseTag("StatusCode");
>>>>>>> Stashed changes
	}

	XmlOutput::writeCloseTag("StatusCodes");
}

// Update order status
function Action_UpdateStatus()
{
<<<<<<< Updated upstream
	global $dbPrefix;
	global $conn;

	if (!isset($_POST['order']) || !isset($_POST['status'])
		|| !isset($_POST['comments'])
	) {
=======
	if (!isset($_POST['order']) ||
		!isset($_POST['status']) ||
		!isset($_POST['comments']))
	{
>>>>>>> Stashed changes
		outputError(40, "Insufficient parameters");
		return;
	}

<<<<<<< Updated upstream
	echo "<UpdateSuccess>";
	$orderID = (int)$_POST['order'];
	$code = chr($_POST['status']);
	$comments = sqlEscapeString($_POST['comments']);

	if (!sqlQuery(
		"insert into " . $dbPrefix . "virtuemart_order_histories " .
		" (virtuemart_order_id, order_status_code, created_on, modified_on, locked_on, customer_notified, comments) "
		.
		" values (" . $orderID . ", '$code' , now(), now(), now(), 0, '" . $comments . "')"
		))
	{
  		echo "<ErrorInserting>" . sqlError() . "</ErrorInserting>";
  	}

	if (!sqlQuery(
		"update " . $dbPrefix . "virtuemart_orders " .
		" set order_status = '$code' " .
		" where virtuemart_order_id = " . $orderID
		))
	{
		
  		echo "<ErrorUpdating>" . sqlError() . "</ErrorUpdating>";
  	}

	echo "</UpdateSuccess>";
=======
	$db = JFactory::getDBO();

	$orderID = (int) $_POST['order'];
	$code = $db->quote(chr($_POST['status']));
	$comments = $db->quote($_POST['comments']);

	XmlOutput::writeStartTag("UpdateSuccess");

	AddOrderHistory($db, $orderID, $code, $comments);
	UpdateOrderStatus($db, $orderID, $code);

	XmlOutput::writeCloseTag("UpdateSuccess");
}

/**
 * Add order history
 */
function AddOrderHistory($db, $orderID, $code, $comments)
{
	$insertQuery = "insert into #__virtuemart_order_histories " .
		"(virtuemart_order_id, order_status_code, created_on, modified_on, locked_on, customer_notified, comments) " .
		"values ($orderID, $code, now(), now(), now(), 0, $comments)";
	$db->setQuery($insertQuery);
	$db->query();

	$err = $db->getErrorMsg();

	if (!empty($err))
	{
		XmlOutput::writeElement("ErrorUpdating", $err);
	}
}

/**
 * Update the order status
 */
function UpdateOrderStatus($db, $orderID, $code)
{
	$order = (object) ['order_status' => $code, 'virtuemart_order_id' => $orderID];
	$result = $db->updateObject('#__virtuemart_orders', $order, 'virtuemart_order_id');

	if (!$result)
	{
		XmlOutput::writeElement("ErrorUpdating", $db->getErrorMsg());
	}
>>>>>>> Stashed changes
}

// Convert the date string to xml date string
function toFormattedDateFromString($sqlDateString)
{
	$sqlDate = date("Y-m-d\TH:i:s", strtotime($sqlDateString));

	return $sqlDate;
}

// Convert the date to string
function toFormattedDateFromObject($sqlDateString)
{
	return gmdate("Y-m-d\TH:i:s", $sqlDateString);
}

<<<<<<< Updated upstream
//virtue mart functions
function db_connect()
{
	global $conn;

	if (class_exists("JConfig")) {
		$CONFIG = new JConfig();
	        sqlConnect($CONFIG->host, $CONFIG->user, $CONFIG->password);
		return sqlSelectDb($CONFIG->db);
	} else {
		// Joomla 1.0
		global $mosConfig_host;
		global $mosConfig_user;
		global $mosConfig_password;
		global $mosConfig_db;

	        sqlConnect($mosConfig_host, $mosConfig_user, $mosConfig_password);
		return sqlSelectDb($mosConfig_db);
	}
}

function not_null($value)
{
	if (is_array($value)) {
		if (sizeof($value) > 0) {
			return true;
		} else {
			return false;
		}
	} else {
		if ((is_string($value) || is_int($value)) && ($value != '')
			&& ($value != 'NULL')
			&& (strlen(trim($value)) > 0)
		) {
			return true;
		} else {
			return false;
		}
	}
}

=======
>>>>>>> Stashed changes
// encrypts a string using the encryption method that virtuemart does
function getCryptedPassword($plaintext, $salt = '', $encryption = 'md5-hex')
{
	$encrypted = ($salt) ? md5($plaintext . $salt) : md5($plaintext);
	return $encrypted;
}

// converts a date string in utc format to one in local time, sql format
function toLocalSqlDate($sqlUtc)
{
	$pattern = "/^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2})$/i";

	if (preg_match($pattern, $sqlUtc, $dt)) {
		$unixUtc = gmmktime($dt[4], $dt[5], $dt[6], $dt[2], $dt[3], $dt[1]);

		return date("Y-m-d H:i:s", $unixUtc);
	}

	return $sqlUtc;
}

?>
