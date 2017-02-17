<!DOCTYPE xsl:stylesheet[ <!ENTITY nl "&#xd;&#xa;"> ]>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

    <xsl:import href="System\Snippets" />
    
    <xsl:output method="text" encoding="utf-8" />

    <!-- Start of template -->
    <xsl:template match="/"><xsl:apply-templates /></xsl:template>
    <xsl:template match="ShipWorks">
        
        <xsl:text>"OrderID","OrderDate","ItemQTY","ItemName","ItemCode","ItemSKU","ItemTotal","ShippingTotal","ShipAddressName","ShipAddress1","ShipAddress2","ShipAddress3","ShipAddressCity","ShipAddressState","ShipAddressPostalCode","ShipAddressCountryCode","ShipAddressPhoneNumber","ShipAddressEmail","BillAddressName","BillAddress1","BillAddress2","BillAddress3","BillAddressCity","BillAddressState","BillAddressPostalCode","BillAddressCountryCode","BillAddressPhoneNumber","BillAddressEmail"&nl;</xsl:text>
        
        <xsl:for-each select="Customer/Order/Item">
            
        <xsl:variable name="ship" select="../Address[@type='ship']" />   
        <xsl:variable name="bill" select="../Address[@type='bill']" />    
    
            <!-- order id / date -->
    
            <xsl:text>"</xsl:text><xsl:call-template name="OrderNumber"><xsl:with-param name="order" select=".." /></xsl:call-template><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="sw:ToShortDate(../Date)" /><xsl:text>",</xsl:text>
        
            <!-- line item info -->
        
            <xsl:text>"</xsl:text><xsl:value-of select="Quantity" /><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="Name" /><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="Code" /><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="SKU" /><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="Total" /><xsl:text>",</xsl:text>
        
            <!-- shipment info -->
        
            <xsl:text>"</xsl:text><xsl:value-of select="format-number(../Shipment[Status = 'Processed']/TotalCharges, '#,##0.00')" /><xsl:text>",</xsl:text>
            
            <!-- shipping address info -->
        
            <xsl:text>"</xsl:text><xsl:value-of select="$ship/FirstName" /><xsl:text> </xsl:text><xsl:value-of select="$ship/LastName" /><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="$ship/Line1" /><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="$ship/Line2" /><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="$ship/Line3" /><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="$ship/City" /><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="$ship/StateCode" /><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="$ship/PostalCode" /><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="$ship/CountryCode" /><xsl:text>",</xsl:text>
    
            <xsl:text>"</xsl:text><xsl:value-of select="$ship/Phone" /><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="$ship/Email" /><xsl:text>"</xsl:text>
                       
            <!-- billing address info -->
        
            <xsl:text>"</xsl:text><xsl:value-of select="$bill/FirstName" /><xsl:text> </xsl:text><xsl:value-of select="$bill/LastName" /><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="$bill/Line1" /><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="$bill/Line2" /><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="$bill/Line3" /><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="$bill/City" /><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="$bill/StateCode" /><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="$bill/PostalCode" /><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="$bill/CountryCode" /><xsl:text>",</xsl:text>
    
            <xsl:text>"</xsl:text><xsl:value-of select="$bill/Phone" /><xsl:text>",</xsl:text>
            <xsl:text>"</xsl:text><xsl:value-of select="$bill/Email" /><xsl:text>"</xsl:text>
            
            <xsl:text>&nl;</xsl:text>
    
            <xsl:text></xsl:text>
    
        </xsl:for-each>  
        
    </xsl:template>
</xsl:stylesheet>