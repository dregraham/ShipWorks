<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

    <!-- Imports -->
    <xsl:import href="System\Snippets" />

    <xsl:output method="html" encoding="utf-8" />

    <!-- Start of template -->
    <xsl:template match="/"><xsl:apply-templates /></xsl:template>
    <xsl:template match="ShipWorks">

    <!-- Width defined by the template PageSettings -->
    <xsl:variable name="pageWidth" select="concat(Template/Output/ContentWidth, 'in')" />

    <!-- Default font.  Specified as a variable since GMail and Outlook behave differently. -->
    <xsl:variable name="pageFont" select="'font-family: Arial; font-size: 8pt;'" />

    <!-- These styles are used on multiple td's so to avoid copy\paste errors they are defined once here.  We have to do this since GMail doesn't support <style> in the <head>.  -->
    <xsl:variable name="orderDetailHeaderStyle" select="'border: 1px solid dimgray; background-color: #F3F3F3; font-weight: bold; padding: 3px;'" />
    <xsl:variable name="orderDetailAttributeStyle" select="'color: #808080;'" />

    <html>

    <head>
        <title>Order Summary Report</title>

        <!-- CSS -->
        <style>
            body, table { <xsl:value-of select="$pageFont" /> }
        </style>
    </head>

    <body style="{$pageFont}" >

        <h3 style="font-size: 1.67em; margin-top: 0;">Order Summary</h3>

        <table style="width:{$pageWidth}; margin: 0px 0px 0px 0px; border-collapse: collapse;" cellspacing="0">

            <tr>
                <td style="{$orderDetailHeaderStyle};">Order</td>
                <td style="{$orderDetailHeaderStyle};">Date</td>
                <td style="{$orderDetailHeaderStyle};">Name</td>
                <td style="{$orderDetailHeaderStyle};">Items</td>
                <td style="{$orderDetailHeaderStyle};">Shipping</td>
                <td style="{$orderDetailHeaderStyle};">Total</td>
            </tr>

            <!-- Group by item code -->
            <xsl:for-each select="Customer/Order">

                <xsl:sort select="Number" order="descending" data-type="number" />

                <!-- We shouldn't have to conditionally apply the topborder... but IE is broken. -->
                <xsl:variable name="summaryDetailContentStyle">
                    padding: 4px 8px 4px 8px;
                    vertical-align: top;
                    <xsl:if test="position() != 1">border-top: 1px solid lightgrey;</xsl:if>
                </xsl:variable>

                <xsl:variable name="shipAddress" select="Address[@type='ship']" />
                <xsl:variable name="billAddress" select="Address[@type='bill']" />

                <tr>
                    <td style="{$summaryDetailContentStyle};">
                        <!-- Shared Snippet -->
                        <xsl:call-template name="OrderNumber">
                            <xsl:with-param name="order" select="." />
                        </xsl:call-template>
                    </td>

                    <td style="{$summaryDetailContentStyle};">
                        <xsl:value-of select="sw:ToShortDate(Date)" />
                    </td>

                    <td style="{$summaryDetailContentStyle};">
                        <b>Ship To:</b>
                        <div style="color: rgb(140, 140, 140);">
                            <xsl:value-of select="$shipAddress/LastName" />, <xsl:value-of select="$shipAddress/FirstName" /><br />
                            <xsl:value-of select="$shipAddress/Line1" /><br />
                            <xsl:value-of select="$shipAddress/City" /><xsl:text>, </xsl:text><xsl:value-of select="$shipAddress/StateCode" /><xsl:text> </xsl:text><xsl:value-of select="$shipAddress/PostalCode" />
                        </div>

                        <b>Bill To:</b>
                        <div style="color: rgb(140, 140, 140);">
                            <xsl:value-of select="$billAddress/LastName" />, <xsl:value-of select="$billAddress/FirstName" /><br />
                            <xsl:value-of select="$billAddress/Line1" /><br />
                            <xsl:value-of select="$billAddress/City" /><xsl:text>, </xsl:text><xsl:value-of select="$billAddress/StateCode" /><xsl:text> </xsl:text><xsl:value-of select="$billAddress/PostalCode" />
                        </div>
                    </td>

                    <td style="{$summaryDetailContentStyle};">

                        <!--                                                            -->
                        <!-- Outputs a table with all order items and options           -->
                        <!--                                                            -->

                        <xsl:for-each select="Item">

                            <xsl:variable name="tableBorder">
                                <xsl:if test="position() != last()">
                                    border-bottom: 1px dashed darkgray;
                                    margin-bottom: 4px;
                                </xsl:if>
                            </xsl:variable>

                            <table style="{$tableBorder}" width="100%" cellspacing="0" border="0">

                                <tr>
                                    <td style="width: 75; white-space: nowrap; vertical-align: top; ">

                                        <b>Code:</b><br />
                                        <xsl:choose>
                                            <xsl:when test="/ShipWorks/Store[@ID = current()/../@storeID]/StoreType = 'eBay'">
                                                <a href="http://cgi.ebay.com/ws/eBayISAPI.dll?ViewItem&amp;item={Code}"><xsl:value-of select="Code" /></a>
                                            </xsl:when>

                                            <xsl:otherwise>
                                                <xsl:value-of select="Code" />
                                            </xsl:otherwise>
                                        </xsl:choose>

                                        <!-- output the SKU if it exists -->
                                        <xsl:if test="SKU != ''">
                                            <br />
                                            <b>SKU:</b><br /><xsl:value-of select="SKU" />
                                        </xsl:if>

                                        <!-- output inventory location if it exists -->
                                        <xsl:if test="Location != ''">
                                            <br />
                                            <b>LOC:</b><br /><xsl:value-of select="Location" />
                                        </xsl:if>
                                    </td>

                                    <td style="vertical-align: top; padding: 0;">
                                        <table style="margin: 0;" cellspacing="0">
                                            <tr>
                                                <td width="300" style="vertical-align: top;" ><b>Name:</b><br /><xsl:value-of select="Name" /></td>
                                                <td width="50" align="right" style="vertical-align: top;" ><b>QTY:</b><br /><xsl:value-of select="Quantity" /> </td>
                                                <td width="50" align="right" style="vertical-align: top;" ><b>Price:</b><br />$<xsl:value-of select="format-number(UnitPrice, '#,##0.00')" /></td>
                                            </tr>

                                            <xsl:for-each select="Option">
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%;" cellspacing="0">
                                                            <tr>
                                                                <td style="{$orderDetailAttributeStyle};" nowrap="nowrap"><xsl:value-of select="Name" />: </td>
                                                                <td style="{$orderDetailAttributeStyle}; width: 100%;"><xsl:value-of select="Description" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>

                                                    <xsl:if test="UnitPrice != 0">
                                                        <td></td>
                                                        <td style="{$orderDetailAttributeStyle};" align="right">$<xsl:value-of select="format-number(UnitPrice, '#,##0.00')" /></td>
                                                    </xsl:if>

                                                    <xsl:if test="UnitPrice = 0">
                                                        <td></td>
                                                        <td></td>
                                                    </xsl:if>

                                                </tr>
                                            </xsl:for-each>

                                        </table>
                                    </td>

                                </tr>

                            </table>
                        </xsl:for-each>

                    </td>

                    <td valign="top" style="{$summaryDetailContentStyle};"><xsl:value-of select="RequestedShipping" /></td>
                    <td valign="top" style="{$summaryDetailContentStyle};">$<xsl:value-of select="format-number(Total, '#,##0.00')" /></td>

                </tr>
            </xsl:for-each>
        </table>
    </body>
    </html>

</xsl:template>
</xsl:stylesheet>
