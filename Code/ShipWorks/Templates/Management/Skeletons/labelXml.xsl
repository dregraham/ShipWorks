<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

    <xsl:output method="xml" encoding="utf-8" indent="yes"/>

    <!-- Start of template -->
    <xsl:template match="/"><xsl:apply-templates /></xsl:template>
    <xsl:template match="ShipWorks">

    <ShipWorks>
        <Template>
            <xsl:value-of select="Template/Folder"></xsl:value-of>\<xsl:value-of select="Template/Name" />
        </Template>
    </ShipWorks>

    </xsl:template>
</xsl:stylesheet>