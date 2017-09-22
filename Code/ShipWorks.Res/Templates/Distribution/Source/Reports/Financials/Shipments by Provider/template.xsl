<!DOCTYPE xsl:stylesheet>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">
    <!-- Imports -->
   <xsl:import href="System\Snippets" />
    <xsl:output method="html" encoding="utf-8" />
    <!-- Setup the key table for grouping by item code -->
    <xsl:key name="processed-shipments" match="Shipment[Status = 'Processed']" use="concat(ShipmentType, ServiceUsed)" />
   
    <!-- Start of template -->
    <xsl:template match="/"><xsl:apply-templates /></xsl:template>
    <xsl:template match="ShipWorks">
       
    <!-- Width defined by the template PageSettings -->
    <xsl:variable name="pageWidth" select="concat(Template/Output/ContentWidth, ' in')" />
    <!-- Default font. Specified as a variable since GMail and Outlook behave differently. -->
    <xsl:variable name="pageFont" select="'font-family: Arial; font-size: 8pt;'" />
        
    <!-- These styles are used on multiple td's so to avoid copy\paste errors they are defined once here. We have to do this since GMail doesn't support <style> in the <head>. -->
    <xsl:variable name="orderDetailHeaderStyle" select="'border: 1px solid dimgray; background-color: #F3F3F3; font-weight: bold; padding: 3px;'" />

    <html>
    <head>
        <title>Types of Shipments</title>
        <style>
            body, table { <xsl:value-of select="$pageFont" /> }
        </style>
    </head>
    <body style="{$pageFont}">

        <h4>Date: <xsl:value-of select="sw:ToShortDate(//Generated)" /><br />
        Orders Selected: <xsl:value-of select="count(//Order)" /></h4>
        
        <xsl:variable name="ordersWithShipments" select="//Order[count(Shipment[Status = 'Processed']) &gt; 0]" />
        <xsl:value-of select="count($ordersWithShipments)" /> orders processed with <xsl:value-of select="count(//Shipment[Status = 'Processed'])"/> shipments
        
        <table style="width:{$pageWidth}; margin: 0px 0px -6px 0px; border-collapse: collapse;" cellspacing="0">
       
            <tr>
                <td style="{$orderDetailHeaderStyle}; white-space: nowrap;">Provider</td>
                <td style="{$orderDetailHeaderStyle}; ">Service Used</td>
                <td style="{$orderDetailHeaderStyle};">Quantity</td>
                <td style="{$orderDetailHeaderStyle}; ">Paid</td>
               
            </tr>
                       
            <!-- Group by processed shipments shipmenttype and service used -->
             <xsl:for-each select="Customer/Order/Shipment[Status = 'Processed'][generate-id(.)=generate-id(key('processed-shipments', concat(ShipmentType,ServiceUsed)))]">
                <xsl:sort select="key('processed-shipments', concat(ShipmentType,ServiceUsed))" />
                <xsl:call-template name="outputItemGroup" />
            </xsl:for-each>
           
            <!--
                Totals
            -->
            <tr>
                <xsl:variable name="rowStyle" select="'padding: 4px 8px 4px 8px; border-top: 1px solid rgb(160, 160, 160);'" />
                <xsl:variable name="Shipping" select="sum(//Order/Shipment[Status = 'Processed']/TotalCharges)" />

                <td style="{$rowStyle}"></td>
                <td style="{$rowStyle}" align="right"><b>Totals:</b></td>
                <td style="{$rowStyle}" align="left"><xsl:value-of select="count(//Order/Shipment[Status = 'Processed'])" /></td>
                <td style="{$rowStyle}" align="right">$<xsl:value-of select="format-number($Shipping, '#,##0.00')" /></td>
            </tr>
           
        </table>
       
    </body>
    </html>
    </xsl:template>
   
    <!-- Outputs totals for a single shipment -->
    <xsl:template name="outputItemGroup">
   
        <xsl:variable name="groupQuantity" select="count(key('processed-shipments', concat(ShipmentType,ServiceUsed)))" />
        <xsl:variable name="ShippingCost" select="sum(key('processed-shipments', concat(ShipmentType,ServiceUsed))/TotalCharges)" />
      
        <!-- We shouldn't have to conditionally apply the topborder... but IE is broken. -->
        <xsl:variable name="orderDetailContentStyle">
            padding: 4px 8px 4px 8px;
            vertical-align: top;
        <xsl:if test="position() != 1">border-top: 1px solid lightgrey;</xsl:if>
        </xsl:variable>
   
        <tr>           
            <td style="{$orderDetailContentStyle}; white-space: nowrap;">
            <xsl:value-of select="ShipmentType" />
            </td>
            <td style="{$orderDetailContentStyle}; white-space: nowrap;">
                <xsl:value-of select="ServiceUsed" />
			</td>
            <td style="{$orderDetailContentStyle};" align="left">
                <xsl:value-of select="$groupQuantity" />
            </td>
            <td style="{$orderDetailContentStyle}; white-space: nowrap;" align="right">
                <xsl:value-of select="format-number($ShippingCost, '#,##0.00')" />
			</td>

        </tr>
   
    </xsl:template>
</xsl:stylesheet>