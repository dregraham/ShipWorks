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
        <title>ShipWorks Template</title>

        <!-- CSS -->
        <style>
            body {font-family: Tahoma; font-size: 10pt;}
            span.example {color: blue; font-weight: bold;}
        </style>
    </head>

    <body>
        ShipWorks template
        <span class="example">
            <xsl:value-of select="Template/Folder" />\<xsl:value-of select="Template/Name" />
        </span>
    </body>

    </html>

    </xsl:template>
</xsl:stylesheet>
