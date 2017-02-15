<!DOCTYPE xsl:stylesheet[ <!ENTITY nl "&#xd;&#xa;"> ]>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

    <xsl:output method="text" encoding="utf-8" />

    <!-- Start of template -->
    <xsl:template match="/"><xsl:apply-templates /></xsl:template>
    <xsl:template match="ShipWorks">

    <xsl:text>BillFirst,BillLast,BillCompany,BillAddress1,BillAddress2,BillAddress3,BillCity,BillStateCode,BillStateName,BillPostalCode,BillCountryCode,BillCountryName,BillPhone,BillFax,BillEmail,ShipFirst,ShipLast,ShipCompany,ShipAddress1,ShipAddress2,ShipAddress3,ShipCity,ShipStateCode,ShipStateName,ShipPostalCode,ShipCountryCode,ShipCountryName,ShipPhone,ShipFax,ShipEmail&nl;</xsl:text>
            
    <xsl:for-each select="Customer">
        
        <xsl:variable name="ship" select="Address[@type='ship']" />
        <xsl:variable name="bill" select="Address[@type='bill']" />
        
        <!-- Billing information -->
        <xsl:text>"</xsl:text><xsl:value-of select="$bill/FirstName" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$bill/LastName" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$bill/Company" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$bill/Line1" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$bill/Line2" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$bill/Line3" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$bill/City" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$bill/StateCode" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$bill/StateName" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$bill/PostalCode" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$bill/CountryCode" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$bill/CountryName" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$bill/Phone" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$bill/Fax" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$bill/Email" /><xsl:text>",</xsl:text>
        
        <!-- Shipping information -->
        <xsl:text>"</xsl:text><xsl:value-of select="$ship/FirstName" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$ship/LastName" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$ship/Company" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$ship/Line1" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$ship/Line2" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$ship/Line3" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$ship/City" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$ship/StateCode" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$ship/StateName" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$ship/PostalCode" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$ship/CountryCode" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$ship/CountryName" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$ship/Phone" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$ship/Fax" /><xsl:text>",</xsl:text>
        <xsl:text>"</xsl:text><xsl:value-of select="$ship/Email" /><xsl:text>"</xsl:text>
        
        <xsl:text>&nl;</xsl:text>
            
    </xsl:for-each>

    </xsl:template>
</xsl:stylesheet>