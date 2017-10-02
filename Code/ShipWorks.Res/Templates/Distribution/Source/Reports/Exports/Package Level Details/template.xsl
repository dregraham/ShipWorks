<!DOCTYPE xsl:stylesheet[ <!ENTITY nl "&#xd;&#xa;"> ]>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

    <xsl:import href="System\Snippets" />
    
    <xsl:output method="text" encoding="utf-8" />

    <!-- Start of template -->
    <xsl:template match="/"><xsl:apply-templates /></xsl:template>
    <xsl:template match="ShipWorks">
        
    <xsl:text>"Service", "Origin Zip", "Destination Zip", "Weight", "Length", "Width", "Height"&nl;</xsl:text>
            
    <xsl:for-each select="//Shipment[Status = 'Processed']/Package">
        
        <xsl:variable name="shipment" select=".." />
        <xsl:text>"</xsl:text>  <xsl:value-of select="$shipment/ServiceUsed" /><xsl:text>","</xsl:text>  
        <xsl:value-of select="$shipment/Address[@type='from']/PostalCode" /><xsl:text>","</xsl:text>  
        <xsl:value-of select="$shipment/Address[@type='ship']/PostalCode" /><xsl:text>","</xsl:text>  
        <xsl:value-of select="$shipment/TotalWeight" /><xsl:text>","</xsl:text>  
        <xsl:value-of select="Dimensions/Length" /><xsl:text>","</xsl:text>  
        <xsl:value-of select="Dimensions/Width" /><xsl:text>","</xsl:text>  
        <xsl:value-of select="Dimensions/Height" /><xsl:text>"</xsl:text>  
        <xsl:text>&nl;</xsl:text>

    </xsl:for-each>        
    </xsl:template>
</xsl:stylesheet>