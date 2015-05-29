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

    <!-- Default font.  Specified as a variable since GMail and Outlook behave differently. -->
    <xsl:variable name="pageFont" select="'font-family: Arial; font-size: 8pt;'" />

    <!-- These styles are used on multiple td's so to avoid copy\paste errors they are defined once here.  We have to do this since GMail doesn't support <style> in the <head>.  -->
    <xsl:variable name="rowStyle" select="'padding: 4px 8px 4px 8px;vertical-align: top; border-top: 1px solid lightgrey;'" />
        
    <html>

    <head>
        <title>Order Charges Breakdown</title>

        <style>
            body, table { <xsl:value-of select="$pageFont" /> }
        </style>

    </head>

    <body style="{$pageFont}">
        
        <h3>Order Charges Breakdown - <xsl:value-of select="count(//Order)" /> Orders</h3>
        
        <xsl:variable name="total"     select="sum(//Order/Total)" />
        <xsl:variable name="lineItems" select="sum(//Order/Item/Total)" />
        <xsl:variable name="shipping"  select="sum(//Order/Charge[Type='SHIPPING']/Amount)" />
        <xsl:variable name="insurance" select="sum(//Order/Charge[Type='INSURANCE']/Amount)" />
        <xsl:variable name="tax"       select="sum(//Order/Charge[Type='TAX']/Amount)" />
        
        <table style="width:4in; margin: 0px 0px -6px 0px; border-collapse: collapse;" cellspacing="0">

            <tr>
                <td style="{$rowStyle};">Line Items:</td>
                <td style="{$rowStyle};" align="right">$<xsl:value-of select="format-number($lineItems, '#,##0.00')" /></td>
            </tr>

            <tr>
                <td style="{$rowStyle};">Shipping (charged to customers):</td>
                <td style="{$rowStyle};" align="right">$<xsl:value-of select="format-number($shipping, '#,##0.00#')" /></td>
            </tr>

            <tr>
                <td style="{$rowStyle};">Insurance (charged to customers):</td>
                <td style="{$rowStyle};" align="right">$<xsl:value-of select="format-number($insurance, '#,##0.00')" /></td>
            </tr>

            <tr>
                <td style="{$rowStyle};">Tax (charged to customers):</td>
                <td style="{$rowStyle};" align="right">$<xsl:value-of select="format-number($tax, '#,##0.00')" /></td>
            </tr>

            <tr>
                <td style="{$rowStyle};"><b>Total of all Orders:</b></td>
                <td style="{$rowStyle};" align="right"><b>$<xsl:value-of select="format-number($total, '#,##0.00')" /></b></td>
            </tr>

        </table>

    </body>

    </html>

    </xsl:template>
</xsl:stylesheet>