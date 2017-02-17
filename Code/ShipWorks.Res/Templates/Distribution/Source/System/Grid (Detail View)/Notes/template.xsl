<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" 
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
    xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

    <!-- Imports -->
    <xsl:import href="System\Snippets" />

    <xsl:output method="html" encoding="utf-8" />

    <!-- Start of template -->
    <xsl:template match="/"><xsl:apply-templates /></xsl:template>
    <xsl:template match="ShipWorks">

    <html>

    <head>
        <style>
            body {font-family: Tahoma; font-size: 10pt; color: gray; }
        </style>
    </head>

    <body style="margin: 0; padding: 0 0 4px 20px;">
        <xsl:for-each select="//Note">
           Note <xsl:value-of select="position()" />: <xsl:value-of select="Text" /><br/>
		</xsl:for-each>
    </body>

    </html>

    </xsl:template>
</xsl:stylesheet>