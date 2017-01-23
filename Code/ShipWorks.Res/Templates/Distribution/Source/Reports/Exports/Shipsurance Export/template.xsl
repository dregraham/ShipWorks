<!DOCTYPE xsl:stylesheet[ <!ENTITY nl "&#xd;&#xa;"> ]>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

    <xsl:import href="System\Snippets" />
    
    <xsl:output method="text" encoding="utf-8" />

    <!-- Start of template -->
    <xsl:template match="/"><xsl:apply-templates /></xsl:template>
    <xsl:template match="ShipWorks">
        
    
    <xsl:text>ORDER_NUMBER,SHIP_DATE,CARRIER,SERVICE,TRACKING,VALUE&nl;</xsl:text>
    
    <xsl:for-each select="//Shipment[Status = 'Processed']">
        <xsl:call-template name="OrderNumber"><xsl:with-param name="order" select=".." /></xsl:call-template>
        <xsl:text>,</xsl:text>
        <xsl:value-of select="sw:ToShortDate(../Date)" /><xsl:text>,</xsl:text>
        
        <xsl:if test="contains(ShipmentType, 'UPS')">UPS</xsl:if>
        
        <xsl:if test="contains(ShipmentType, 'USPS')">USPS</xsl:if>
        
        <xsl:if test="ShipmentType = 'FedEx'">FedEx</xsl:if>
        
        <xsl:if test="ShipmentType = 'Other'">Other</xsl:if>
        
        <xsl:text>,</xsl:text>

        <xsl:value-of select="ServiceUsed" /><xsl:text>,</xsl:text>
        <xsl:value-of select="TrackingNumber" /><xsl:text>,</xsl:text>
        <xsl:value-of select="format-number(../Total, '#,##0.00')" />
        <xsl:text>&nl;</xsl:text>
        
    </xsl:for-each>
        
    </xsl:template>
</xsl:stylesheet>