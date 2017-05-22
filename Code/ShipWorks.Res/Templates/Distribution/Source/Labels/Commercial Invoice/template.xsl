<!DOCTYPE xsl:stylesheet [<!ENTITY nbsp "&#160; &#xa0;">]>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

    <!-- Imports -->
    <xsl:import href="System\Snippets" />

    <xsl:output method="html" encoding="utf-8" indent="yes" />

    <!-- Start of template -->
    <xsl:template match="/"><xsl:apply-templates /></xsl:template>
    <xsl:template match="ShipWorks">

    <!-- Width defined by the template PageSettings -->
    <xsl:variable name="pageWidth" select="concat(Template/Output/ContentWidth, 'in')" />

    <!-- Default font.  Specified as a variable since GMail and Outlook behave differently. -->
    <xsl:variable name="pageFont" select="'font-family: Arial; font-size: 8pt;'" />

    <!-- These styles are used on multiple td's so to avoid copy\paste errors they are defined once here.  We have to do this since GMail doesn't support <style> in the <head>.  -->
    <xsl:variable name="orderDetailHeaderStyle" select="'border: 1px solid dimgray; background-color: #F3F3F3; font-weight: bold; padding: 3px; text-align:center;'" />
    <xsl:variable name="orderDetailAttributeStyle" select="'color: #808080; padding: 0px 8px 2px 2px;'" />
    <xsl:variable name="orderChargeStyle" select="'white-space: nowrap; text-align: right; padding: 1px 8px 3px 16px;'" />

    <html>

    <head>
        <title>Invoice</title>

        <style>
            body, table { <xsl:value-of select="$pageFont" /> }
        </style>

    </head>

    <body style="{$pageFont}">

        <xsl:variable name="orderCount" select="count(Customer/Order)" />
        <xsl:if test="$orderCount = 1">

        <xsl:variable name="order" select="Customer/Order[1]" />

        <table style="width:{$pageWidth};">
            <tr>
                <td style="width:100%; color: white; background-color: #C0C0C0; text-align: center; font-weight: bold; font-size: 12pt; border: 1px solid black; padding: 1px;">
                    Commercial Invoice
                </td>
            </tr>
        </table>

        <!--
            Store Address \ Order Number
        -->
        <table style="width:{$pageWidth}; ">
            <tr>

            <td style="width: 100%; vertical-align:top">

                <span style="font-weight: bold; font-size: 120%;">
                    <xsl:value-of select="Store/Address/Company" />
                </span>

            </td>

            <td style="vertical-align: top;">
                <table style="border: 1px solid dimgray;">
                <tr>
                    <td>

                    <table style="font-weight: bold;" cellspacing="0">
                        <tr>
                            <td style="padding-right: 2px; vertical-align:top">Order:</td>
                            <td style="white-space: nowrap;">

                                <!-- Shared Snippet -->
                                <xsl:call-template name="OrderNumber">
                                    <xsl:with-param name="order" select="$order" />
                                </xsl:call-template>

                            </td>
                        </tr>
                        <tr>
                            <td style="padding-right: 2px">Date:</td>
                            <td style="white-space: nowrap;">

                                <!-- Builtin ShipWorks function -->
                                <xsl:value-of select="sw:ToShortDate(Generated)" />

                            </td>
                        </tr>
                    </table>

                    </td>
                </tr>
                </table>

            </td>
            </tr>
        </table>

        <br />

        <!--
            Ship From \ Ship to
        -->
        <table style="width:{$pageWidth};" cellspacing="0">
            <tr>

            <td style="width: 47.5%; padding: 1px; border: 1px solid dimgray; vertical-align: top;">

                <table style="width: 100%;" cellspacing="0">
                    <tr>
                        <td style="font-weight: bold; border-bottom: 1px solid dimgray; background-color: #F3F3F3; padding: 1px 5px;">
                            Ship From
                        </td>
                    </tr>

                    <tr>
                        <td style="padding: 4px 10px;">

                            <!-- Shared Snippet -->
                            <xsl:call-template name="StoreHeading">
                                <xsl:with-param name="address" select="Store/Address" />
                            </xsl:call-template>

                        </td>
                    </tr>
                </table>

            </td>

            <td style="width: 5%; border:0"> </td>

            <td style="width: 47.5%; padding: 1px; border: 1px solid dimgray; vertical-align: top;">

                <table style="width: 100%;" cellspacing="0">
                    <tr>
                        <td style="font-weight: bold; border-bottom: 1px solid dimgray; background-color: #F3F3F3; padding: 1px 5px;">
                            ShipTo
                        </td>
                    </tr>

                    <tr>
                        <td style="padding: 4px 10px;">

                            <!-- Shared Snippet -->
                            <xsl:call-template name="FormatAddress">
                                <xsl:with-param name="address" select="$order/Shipment/Address" />
                            </xsl:call-template>

                        </td>
                    </tr>
                </table>

            </td>
            </tr>
        </table>

        <br />

        <!--
            Importer \ Check Boxes
        -->
        <table style="width:{$pageWidth};" cellspacing="0">
            <tr>

            <td style="width: 47.5%; padding: 1px; border: 1px solid dimgray; vertical-align: top;">

                <table style="width: 100%;" cellspacing="0">
                    <tr>
                        <td style="font-weight: bold; border-bottom: 1px solid dimgray; background-color: #F3F3F3; padding: 1px 5px;">
                            Importer Other Than Recipient
                        </td>
                    </tr>

                    <tr>
                        <td style="padding: 4px 10px;">
                            Name:<br /><br />
                            Address:<br /><br />
                            City/State/Zip:<br /><br />
                            Phone:<br />

                        </td>
                    </tr>
                </table>

            </td>

            <td style="width: 5%; border:0"> </td>

            <td style="width: 47.5%; padding: 1px; border: 1px solid dimgray; vertical-align: top;">

                <table style="width: 100%;" cellspacing="0">
                    <tr>
                        <td style="font-weight: bold; border-bottom: 1px solid dimgray; background-color: #F3F3F3; padding: 1px 5px;">
                            Shipment Information
                        </td>
                    </tr>

                    <tr>
                        <td style="padding: 4px 10px;">

                        <table style="width:100%">
                            <tr>
                                <td style="border: 1px solid black; width:12; height:12;">   </td><td>CIF</td>
                                <td style="border: 1px solid black; width:12; height:12;">   </td><td>FOB</td>
                                <td style="border: 1px solid black; width:12; height:12;">   </td><td>C&amp;F</td>
                            </tr>
                        </table>

                        <table style="width:100%">
                            <tr>
                                <td>Country of Export: United States</td>
                            </tr>
                            <tr>
                                <td>Country of Destination: <xsl:value-of select="$order/Shipment/Address[@type='ship']/CountryName" /></td>
                            </tr>
                            <tr>
                                <td>Currency: US Dollar</td>
                            </tr>
                            <tr>
                                <td>Package Count: <xsl:value-of select="count($order/Shipment/Labels/Package)" /></td>
                            </tr>
                            <tr>
                                <td>Total Weight: <xsl:value-of select="format-number(sum($order/Shipment/CustomsItem/TotalWeight), '#,##0.00')" /> lbs.</td>
                            </tr>

                        </table>

                        </td>
                    </tr>
                </table>

            </td>
            </tr>
        </table>

        <br />

        <!--
            Line Items
        -->
        <table style="width:{$pageWidth}; margin: 0; border-collapse: collapse;" cellspacing="0">

            <tr>
                <td style="{$orderDetailHeaderStyle}; width: 6%;">Quantity</td>
                <td style="{$orderDetailHeaderStyle};">Unit of<br />Measure</td>
                <td style="{$orderDetailHeaderStyle};">Unit<br />Value</td>
                <td style="{$orderDetailHeaderStyle};">Commodity Description</td>
                <td style="{$orderDetailHeaderStyle};">Weight</td>
                <td style="{$orderDetailHeaderStyle};">Total Value</td>
            </tr>

            <xsl:for-each select="$order/Shipment/CustomsItem">

                <!-- We shouldn't have to conditionally apply the topborder... but IE is broken. -->
                <xsl:variable name="orderDetailContentStyle">
                    padding: 4px 8px 4px 8px;
                    vertical-align:top;
                    <xsl:if test="position() != 1">border-top: 1px solid lightgrey;</xsl:if>
                </xsl:variable>

                <tr>

                    <td style="{$orderDetailContentStyle}; text-align:center;">
                        <xsl:value-of select="Quantity" />
                    </td>
                    <td style="{$orderDetailContentStyle}; text-align:center;">
                        Pounds
                    </td>
                    <td style="{$orderDetailContentStyle}; text-align:center;">
                        <xsl:value-of select="format-number(UnitValue, '#,##0.00')" />
                    </td>
                    <td style="{$orderDetailContentStyle};">
                        <xsl:value-of select="Description" />

                        <xsl:if test="HarmonizedCode != ''">
                            <br />
                            <font style="font-size:8pt;">Harmonized Code: <xsl:value-of select="HarmonizedCode" /></font>
                        </xsl:if>

                        <xsl:if test="OriginCountryName != ''">
                            <br />
                            <font style="font-size:8pt;">Country of Origin: <xsl:value-of select="OriginCountryName" /></font>
                        </xsl:if>

                    </td>
                    <td style="{$orderDetailContentStyle}; text-align:center;">
                        <xsl:value-of select="Weight" />
                    </td>
                    <td style="{$orderDetailContentStyle}; text-align:right;">
                        <xsl:value-of select="format-number(TotalValue, '#,##0.00')" />
                    </td>
              </tr>

            </xsl:for-each>

        </table>

        <br />

        <div style="border-top: 1px solid grey; width:{$pageWidth}; height: 1px; margin: 0px 0px 10px 0px; *margin-bottom: 1;"></div>

        <!--
            Totals
        -->
        <table style="width:{$pageWidth};" cellspacing="0">
            <tr>
                <td style="{$orderChargeStyle}; font-weight: bold;  width: 100%;">Total:</td>

                <td style="{$orderChargeStyle}; font-weight: bold;">
                    <xsl:value-of select="format-number(sum($order/Shipment/CustomsItem/TotalValue), '#,##0.00')" />
                </td>
            </tr>
        </table>


        <!--
            Shipment information and tracking
        -->
        <xsl:if test="count($order/Shipment[Status = 'Processed']) != 0">

            <br />

            <table style="width:{$pageWidth}; border-collapse: collapse;" cellspacing="0">
                <tr>
                    <td colspan="3" style="{$orderDetailHeaderStyle};">Shipment Information</td>
                </tr>

                <xsl:for-each select="$order/Shipment[Status = 'Processed']">
                    <tr>
                        <td>Shipped on <xsl:value-of select="sw:ToShortDate(ShippedDate)" /></td>
                        <td><xsl:value-of select="ServiceUsed" /></td>
                        <td><xsl:value-of select="TrackingNumber" /></td>
                    </tr>
                </xsl:for-each>

            </table>

        </xsl:if>

        <br />
        <br />

        <table>
            <tr>
                <td width="70%" valign="TOP"> <hr size="1" />
                    Signature of shipper/exporter (print name and
                    title and sign)<br />
                    I declare all the information contained in this invoice to be true and correct</td>
                <td width="10%"></td>
                <td width="20%" valign="TOP" align="CENTER">

                <hr size="1" />Date</td>
                </tr>
        </table>


        <!--
            This is the end of the test ensuring a single order
        -->
        </xsl:if>

        <!-- If there is not just a single order its an error -->
        <xsl:if test="$orderCount != 1">
            <p>
                This template is designed to be per-order.  There are currently
                <xsl:value-of select="$orderCount" /> orders in the XML input source.
            </p>
            <p>
                Please adjust the template settings to be processed per-order.
            </p>
        </xsl:if>

    </body>

    </html>

    </xsl:template>
</xsl:stylesheet>
