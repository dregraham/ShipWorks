<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

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

        <xsl:variable name="labels" select="(//Primary | //Supplemental)/Label[@orientation = 'wide']" />
        
        <xsl:for-each select="$labels">
                
            <xsl:variable name="shipment" select="../../../.." />

            <TemplatePartition>
                <center>
                <table height="100%" width="100%" cellspacing="0">
                    <tr>
                        <td valign="middle" align="center">
                            <xsl:choose>
                                <xsl:when test="$shipment/ShipmentType = 'FedEx'">
                                    <img src="{.}" style="width:576; height:384;" />
                                </xsl:when>
                                
                                <xsl:when test="$shipment/ShipmentType = 'UPS'">
                                    <img src="{.}" style="width:576; height:384;" />
                                </xsl:when>
                                
                                <xsl:otherwise>
                                    <img src="{.}" style="width:{@widthInches}in; height:{@heightInches}in;" />
                                </xsl:otherwise>
                            </xsl:choose>
                        </td>
                    </tr>
                </table>
                    

                </center>
            </TemplatePartition>

        </xsl:for-each>

    </body>

    </html>

    </xsl:template>
</xsl:stylesheet>