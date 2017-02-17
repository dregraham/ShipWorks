<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

    <!-- Imports -->
    <xsl:import href="System\Snippets" />

    <xsl:output method="html" encoding="utf-8" indent="yes" />

    <!-- Start of template -->
    <xsl:template match="/"><xsl:apply-templates /></xsl:template>
    <xsl:template match="ShipWorks">

    <!-- Controls display of thumbnail images -->
    <xsl:variable name="thumbnailsEnabled" select="false()" />
    <xsl:variable name="thumbnailWidth" select="'50px'" />
    <xsl:variable name="thumbnailHeight" select="'50px'" />

    <!-- Default font.  Specified as a variable since GMail and Outlook behave differently. -->
    <xsl:variable name="pageFont" select="'font-family: Arial; font-size: 8pt;'" />

    <!-- These styles are used on multiple td's so to avoid copy\paste errors they are defined once here.  We have to do this since GMail doesn't support <style> in the <head>.  -->
    <xsl:variable name="orderDetailAttributeStyle" select="'color: #808080; padding: 0px 8px 2px 2px;'" />

    <html>

    <head>
        <title>Line Items</title>

        <style>
            body, table { <xsl:value-of select="$pageFont" /> }
        </style>

    </head>

    <body style="{$pageFont}; margin: 0; padding: 0;" >

        <xsl:variable name="order" select="Customer/Order[1]" />
        
        <!--
            Line Items
        -->
        <table style="margin: 0px 0px 20px 30px; border-collapse: collapse;" cellspacing="0">

            <xsl:for-each select="$order/Item">

                <!-- We shouldn't have to conditionally apply the topborder... but IE is broken. -->
                <xsl:variable name="orderDetailContentStyle">
                    padding: 4px 8px 4px 8px;
                    white-space: nowrap;
                    color: #505050;
                    <xsl:if test="position() != 1">border-top: 1px solid lightgrey;</xsl:if>
                </xsl:variable>
                
                <tr>

                    <!-- The variables controlling thumbnails are up near the top of the template. -->
                    <xsl:if test="$thumbnailsEnabled">
                        <td style="{$orderDetailContentStyle};">
                            <xsl:if test="Thumbnail != ''">
                                <img src="{Thumbnail}" alt="" style="height:{$thumbnailWidth}; width:{$thumbnailHeight}; border:0;" />
                            </xsl:if>
                        </td>
                    </xsl:if>

                    <td style="{$orderDetailContentStyle};">
                        <!-- Shared Snippet -->
                        <xsl:call-template name="OrderItemCode">
                            <xsl:with-param name="item" select="." />
                        </xsl:call-template>
                    </td>
                    <td style="{$orderDetailContentStyle};">
                        <xsl:value-of select="Name" />
                    </td>
                    <td style="{$orderDetailContentStyle};" align="right">
                        <xsl:value-of select="Quantity" />
                    </td>
                                        
                    <td style="{$orderDetailContentStyle};" align="right">
                        $<xsl:value-of select="format-number(UnitPrice, '#,##0.00')" />
                    </td>
                    
                </tr>

                <!-- Displays any item attribuets that may exists -->
                <xsl:for-each select="Option">
                    <tr>
                        <xsl:if test="$thumbnailsEnabled">
                            <td></td>
                        </xsl:if>
                        <td></td>
                        <td>
                            <table style="width: 100%; margin-left: 30px;" cellspacing="0">
                                <tr>
                                    <td style="{$orderDetailAttributeStyle}; white-space: nowrap;">
                                        <xsl:value-of select="Name" />:
                                    </td>
                                    <td style="{$orderDetailAttributeStyle}; width: 100%;">
                                        <xsl:value-of select="Description" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td></td>
                        <xsl:choose>
                            <xsl:when test="UnitPrice != 0">
                                <td style="{$orderDetailAttributeStyle};" align="right">
                                    <xsl:value-of select="format-number(UnitPrice, '#,##0.00')" />
                                </td>
                            </xsl:when>
                            <xsl:otherwise>
                                <td></td>
                            </xsl:otherwise>
                        </xsl:choose>
                    </tr>
                </xsl:for-each>
            </xsl:for-each>

        </table>

    </body>

    </html>

    </xsl:template>
</xsl:stylesheet>