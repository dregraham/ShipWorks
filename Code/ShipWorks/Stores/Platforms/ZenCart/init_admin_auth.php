<?php
/**
 * @package admin
 * @copyright Copyright 2003-2006 Zen Cart Development Team
 * @copyright Portions Copyright 2003 osCommerce
 * @license http://www.zen-cart.com/license/2_0.txt GNU Public License V2.0
 * @version $Id: init_admin_auth.php 3001 2006-02-09 21:45:06Z wilt $
 */

    if (!defined('IS_ADMIN_FLAG')) 
    {
        die('Illegal Access');
    }

    if (defined('SHIPWORKS_MODULE_ACTIVE') && SHIPWORKS_MODULE_ACTIVE === true) 
    {
        // Bypass extra auth checks. ShipWorks module will check itself.
    }

    else
    {
       // Do standard checks
       require('includes/init_includes/init_admin_auth.php');
    }

?>