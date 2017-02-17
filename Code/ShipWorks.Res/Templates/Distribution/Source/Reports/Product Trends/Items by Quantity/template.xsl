<!DOCTYPE xsl:stylesheet [
    
    <!-- This is the lookup key value for the grouping table -->
    <!ENTITY itemKey "sw:GetOrderItemKeyValue(., $optionSpecific)">
    
    <!-- This is the collection of items in a single grouping -->
    <!ENTITY itemGroup "key($keyTable, &itemKey;)" >
]>
    
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

    <!-- Imports -->
    <xsl:import href="System\Snippets" />

    <xsl:output method="html" encoding="utf-8" />

    <!-- This can be changed to false to turn off thumbnails -->
    <xsl:variable name="showThumbnails" select="false()" />

    <!-- This can be used to control if items are grouped based on options -->
    <xsl:variable name="optionSpecific" select="false()" />

    <!-- Setup the key table for grouping by item code -->
    <xsl:key name="items-specific" match="Item" use="sw:GetOrderItemKeyValue(., true())" />
    <xsl:key name="items-non-specific" match="Item" use="sw:GetOrderItemKeyValue(., false())" />

    <!-- Determine which key table to use based on if we are option specific or not -->
    <xsl:variable name="keyTable">
        <xsl:if test="$optionSpecific">
            <xsl:value-of select="'items-specific'" />
        </xsl:if>
        <xsl:if test="not($optionSpecific)">
            <xsl:value-of select="'items-non-specific'" />
        </xsl:if>
    </xsl:variable>
    
    <!-- Start of template -->
    <xsl:template match="/"><xsl:apply-templates /></xsl:template>
    <xsl:template match="ShipWorks">
        
    <!-- Width defined by the template PageSettings -->
    <xsl:variable name="pageWidth" select="concat(Template/Output/ContentWidth, ' in')" />

    <!-- Default font.  Specified as a variable since GMail and Outlook behave differently. -->
    <xsl:variable name="pageFont" select="'font-family: Arial; font-size: 8pt;'" />

    <!-- These styles are used on multiple td's so to avoid copy\paste errors they are defined once here.  We have to do this since GMail doesn't support <style> in the <head>.  -->
    <xsl:variable name="orderDetailHeaderStyle" select="'border: 1px solid dimgray; background-color: #F3F3F3; font-weight: bold; padding: 3px;'" />
    <xsl:variable name="orderDetailAttributeStyle" select="'color: #808080; padding: 0px 8px 2px 2px;'" />
    <xsl:variable name="orderChargeStyle" select="'white-space: nowrap; text-align: right; padding: 1px 8px 3px 16px;'" />
        
    <html>

    <head>
        <title>Items by Quantity</title>

        <style>
            body, table { <xsl:value-of select="$pageFont" /> }
        </style>

    </head>

    <body style="{$pageFont}">
        
        <h3>Items by Quantity - <xsl:value-of select="count(Customer/Order)" /> Orders</h3>
        
        <table style="width:{$pageWidth}; margin: 0px 0px -6px 0px; border-collapse: collapse;" cellspacing="0">
        
            <tr>
                <xsl:if test="$showThumbnails">
                    <td style="width: 50px; {$orderDetailHeaderStyle};">Image</td>
                </xsl:if>
                <td style="{$orderDetailHeaderStyle}; white-space: nowrap;">Item Code</td>
                <td style="{$orderDetailHeaderStyle}; ">Name</td>
                <td style="{$orderDetailHeaderStyle};" align="right">Quantity</td>
                <td style="{$orderDetailHeaderStyle};" align="right">Total</td>

            </tr>
                        
            <!-- Group by quantity        -->
            <xsl:for-each select="Customer/Order/Item[generate-id(.)=generate-id(&itemGroup;)]">
                <xsl:sort select="sum(&itemGroup;/Quantity)" order="descending" data-type="number" />
                <xsl:call-template name="outputItemGroup" />
            </xsl:for-each>
            
            <!--
                Totals
            -->
            <tr>
                <xsl:variable name="rowStyle" select="'padding: 4px 8px 4px 8px; border-top: 1px solid rgb(160, 160, 160);'" />
                
                <xsl:if test="$showThumbnails"><td style="{$rowStyle}"></td></xsl:if>
                <td style="{$rowStyle}"></td>
                <td style="{$rowStyle}" align="right"><b>Totals:</b></td>
                <td style="{$rowStyle}" align="right"><xsl:value-of select="sum(Customer/Order/Item/Quantity)" /></td>
                <td style="{$rowStyle}" align="right">$<xsl:value-of select="format-number(sum(Customer/Order/Item/Total), '#,##0.00')" /></td>
            </tr>
            
         </table>
        
    </body>

    </html>

    </xsl:template>
    
    <!--                                                            -->
    <!-- Outputs totals for a single item                           -->
    <!--                                                            -->
    <xsl:template name="outputItemGroup">
        <xsl:variable name="groupQuantity" select="sum(&itemGroup;/Quantity)" />
        <xsl:variable name="groupTotal" select="sum(&itemGroup;/Total)" />
    
        <!-- We shouldn't have to conditionally apply the topborder... but IE is broken. -->
        <xsl:variable name="orderDetailContentStyle">
            padding: 4px 8px 4px 8px;
            vertical-align: top;
        <xsl:if test="position() != 1">border-top: 1px solid lightgrey;</xsl:if>
        </xsl:variable>    
    
        <tr>
            <xsl:if test="$showThumbnails">
                <td style="{$orderDetailContentStyle};">
                    <xsl:if test="Thumbnail != ''">
                    <img src="{Thumbnail}" alt="" style="height:50; width:50; border:0;" />
                    </xsl:if>
                </td>
            </xsl:if>
            
            <td style="{$orderDetailContentStyle}; white-space: nowrap;"><xsl:value-of select="Code" /></td>
            <td style="{$orderDetailContentStyle};"><xsl:value-of select="Name" />
                <xsl:if test="$optionSpecific">
                    <xsl:for-each select="Option">
                        <table style="width: 100%;" cellspacing="0">
                            <tr>
                                <td style="color: DarkGray;" nowrap="nowrap"><xsl:value-of select="Name" />: </td>
                                <td style="color: DarkGray; width: 100%;"><xsl:value-of select="Description" /></td>
                            </tr>
                        </table>
                    </xsl:for-each>
                </xsl:if>
            </td>
            <td style="{$orderDetailContentStyle};" align="right"><xsl:value-of select="$groupQuantity" /></td>
            <td style="{$orderDetailContentStyle};" align="right">$<xsl:value-of select="format-number($groupTotal, '#,##0.00')" /></td>
        </tr>
    
    </xsl:template>    
    
    
</xsl:stylesheet>