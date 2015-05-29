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

    <!-- Width defined by the template PageSettings -->
    <xsl:variable name="pageWidth" select="concat(Template/Output/ContentWidth, ' in')" />

    <!-- Default font.  Specified as a variable since GMail and Outlook behave differently. -->
    <xsl:variable name="pageFont" select="'font-family: Arial; font-size: 8pt;'" />

    <!-- These styles are used on multiple td's so to avoid copy\paste errors they are defined once here.  We have to do this since GMail doesn't support <style> in the <head>.  -->
    <xsl:variable name="headerStyle" select="'border: 1px solid dimgray; background-color: #F3F3F3; font-weight: bold; padding: 3px;'" />
        
    <html>

    <head>
        <title>Shipping Profit and Loss</title>

        <style>
            body, table { <xsl:value-of select="$pageFont" /> }
        </style>

    </head>
        
    <xsl:variable name="ordersWithShipments" select="//Order[count(Shipment[Status = 'Processed']) > 0]" />

    <body style="{$pageFont}">
        
        <h3>Shipping Profit and Loss - <xsl:value-of select="count($ordersWithShipments)" /> Orders</h3>
        
        <table style="width:{$pageWidth}; margin: 0px 0px -6px 0px; border-collapse: collapse;" cellspacing="0">
        
            <tr>
                <td style="{$headerStyle}; white-space: nowrap;">Order #</td>
                <td style="{$headerStyle};">Requested Shipping</td>
                <td style="{$headerStyle};">Processed Date</td>
                <td style="{$headerStyle};">Service Used</td>
                <td style="{$headerStyle};">Tracking</td>
                <td style="{$headerStyle};">Charged</td>
                <td style="{$headerStyle};">Paid</td>
                <td style="{$headerStyle};">Profit/Loss</td>

            </tr>
            
            <xsl:for-each select="$ordersWithShipments">
            
            <!-- We shouldn't have to conditionally apply the topborder... but IE is broken. -->
            <xsl:variable name="rowStyle">
                padding: 4px 8px 4px 8px;
                vertical-align: top;
            <xsl:if test="position() != 1">border-top: 1px solid lightgrey;</xsl:if>
            </xsl:variable>
                
            <xsl:variable name="shipments" select="Shipment[Status = 'Processed']" />
            <xsl:variable name="shippingCosts" select="sum($shipments/TotalCharges)" />
                
            <tr>            
                <td style="{$rowStyle};"><xsl:call-template name="OrderNumber"><xsl:with-param name="order" select="." /></xsl:call-template></td>
                <td style="{$rowStyle};"><xsl:value-of select="RequestedShipping" /></td>
                
                <xsl:if test="count($shipments) = 1">
                    <td style="{$rowStyle};"><xsl:value-of select="sw:ToShortDate($shipments[1]/ProcessedDate)" /></td>
                    <td style="{$rowStyle};"><xsl:value-of select="$shipments[1]/ServiceUsed" /></td>
                    <td style="{$rowStyle};"><xsl:value-of select="$shipments[1]/TrackingNumber" /></td>
				        </xsl:if>
                
                <xsl:if test="count($shipments) > 1">
                    <td style="{$rowStyle};" colspan="3">(<xsl:value-of select="count($shipments)" /> shipments)</td>
				        </xsl:if>
                
                <td style="{$rowStyle};" align="right">$<xsl:value-of select="format-number(Charge[Type='SHIPPING']/Amount, '#,##0.00')" /></td>
                <td style="{$rowStyle};" align="right">$<xsl:value-of select="format-number($shippingCosts, '#,##0.00#')" /></td>
                <td style="{$rowStyle};" align="right">$<xsl:value-of select="format-number(Charge[Type='SHIPPING']/Amount - $shippingCosts, '#,##0.00#')" /></td>
            </tr>
                
            </xsl:for-each>
            
            <xsl:variable name="totalShippingCharges" select="sum($ordersWithShipments/Charge[Type='SHIPPING']/Amount)" />
            <xsl:variable name="totalShippingCost" select="sum(//Order/Shipment[Status = 'Processed']/TotalCharges)" />
            
            <!--
                Totals
            -->
            <tr>
                <xsl:variable name="rowStyle" select="'padding: 4px 8px 4px 8px; border-top: 1px solid rgb(160, 160, 160);'" />
                <td style="{$rowStyle}"></td>
                <td style="{$rowStyle}"></td>
                <td style="{$rowStyle}"></td>
                <td style="{$rowStyle}"></td>
                <td style="{$rowStyle}" align="right"><b>Totals:</b></td>
                <td style="{$rowStyle}" align="right">$<xsl:value-of select="format-number($totalShippingCharges, '#,##0.00#')" /></td>
                <td style="{$rowStyle}" align="right">$<xsl:value-of select="format-number($totalShippingCost, '#,##0.00#')" /></td>
                <td style="{$rowStyle}" align="right">$<xsl:value-of select="format-number($totalShippingCharges - $totalShippingCost, '#,##0.00#')" /></td>
            </tr>
            
         </table>
        
    </body>

    </html>

    </xsl:template>
</xsl:stylesheet>