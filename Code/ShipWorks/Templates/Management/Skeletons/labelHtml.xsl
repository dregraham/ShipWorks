<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
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
    </head>

    <body>
        <table border="0">
            <tr>
                <td>
                  <b>Name</b>
                </td>
            </tr>
            <tr>
                <td>Address</td>
            </tr>
            <tr>
                <td>City, State ZIP</td>
            </tr>
        </table>
    </body>

    </html>

    </xsl:template>
</xsl:stylesheet>
