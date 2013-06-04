<?xml version="1.0" encoding="utf-8"?>
<!DOCTYPE xsl:stylesheet[
    <!ENTITY nl "&#xd;&#xa;">
]>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw" >

    <xsl:output method="text" encoding="utf-8" />

    <!-- Start of template -->
    <xsl:template match="/"><xsl:apply-templates /></xsl:template>
    <xsl:template match="ShipWorks">

    <xsl:text>ShipWorks template: &nl;</xsl:text>
    <xsl:value-of select="Template/Folder" />\<xsl:value-of select="Template/Name" />

    </xsl:template>
</xsl:stylesheet>
