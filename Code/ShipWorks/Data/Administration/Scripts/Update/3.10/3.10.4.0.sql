-- Reset the contract type to unknown, so we get updated information (and don't show the
-- footnote to users that have already converted their accounts outside of ShipWorks)
UPDATE StampsAccount SET ContractType = 0