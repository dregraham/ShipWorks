<?php
	
class Interapptive_ShipWorks_Model_Version
{
	// Gets the UI value for version infomation for display in the UI
	public function toOptionArray()
	{
		return array("version" => $this->getModuleVersion());
	}

	// gets the module version
	public function getModuleVersion()
	{
		return "3.1.12";
	}
}
?>
