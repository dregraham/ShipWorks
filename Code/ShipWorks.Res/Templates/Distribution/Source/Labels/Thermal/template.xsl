<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

  <xsl:output method="xml" encoding="utf-8" />

    <!-- Start of template -->
    <xsl:template match="/"><xsl:apply-templates /></xsl:template>
    <xsl:template match="ShipWorks">
                  
    <Root>
        <xsl:variable name="labels" select="(//Primary | //Supplemental)/Label[@thermal]" />
        
        <xsl:for-each select="$labels">
            <ThermalLabel format="{@format}" ><xsl:value-of select="." /></ThermalLabel>
        </xsl:for-each>
    </Root>
                  
</xsl:template>
</xsl:stylesheet>