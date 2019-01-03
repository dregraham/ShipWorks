<!DOCTYPE xsl:stylesheet []>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">
    <!-- Imports -->
   <xsl:import href="System\Snippets" />
    <xsl:output method="html" encoding="utf-8" />
    <!-- This can be changed to false to turn off thumbnails -->
    <xsl:variable name="showThumbnails" select="false()" />

    <xsl:key name="items-specific" match="Item" use="SKU" />
    <xsl:variable name="keyTable" select="'items-specific'" />

    <!-- Start of template -->
    <xsl:template match="/"><xsl:apply-templates /></xsl:template>
    <xsl:template match="ShipWorks">

    <!-- Width defined by the template PageSettings -->
    <xsl:variable name="pageWidth" select="concat(Template/Output/ContentWidth, ' in')" />
    <!-- Default font. Specified as a variable since GMail and Outlook behave differently. -->
    <xsl:variable name="pageFont" select="'font-family: Arial; font-size: 8pt;'" />
    <!-- These styles are used on multiple td's so to avoid copy\paste errors they are defined once here. We have to do this since GMail doesn't support <style> in the <head>. -->
    <xsl:variable name="orderDetailHeaderStyle" select="'border: 1px solid dimgray; background-color: #F3F3F3; font-weight: bold; padding: 3px;'" />
    <xsl:variable name="orderDetailAttributeStyle" select="'color: #808080; padding: 0px 8px 2px 2px;'" />
    <xsl:variable name="orderChargeStyle" select="'white-space: nowrap; text-align: right; padding: 1px 8px 3px 16px;'" />

    <html>
    <head>
           <title><xsl:value-of select="//Template/Name" /></title>
        <style>
            body, table { <xsl:value-of select="$pageFont" /> }
        </style>
    </head>
    <body style="{$pageFont}">
        <h3><xsl:value-of select="//Template/Name" /></h3>

        <table style="width:{$pageWidth}; margin: 0px 0px -6px 0px; border-collapse: collapse;" cellspacing="0">
            <tr>
                <xsl:if test="$showThumbnails">
                    <td style="width: 50px; {$orderDetailHeaderStyle};">Image</td>
                </xsl:if>
                <td style="{$orderDetailHeaderStyle}; white-space: nowrap;">SKU</td>
                <td style="{$orderDetailHeaderStyle}; ">Name</td>
                <td style="{$orderDetailHeaderStyle}; ">Location</td>
                <td style="{$orderDetailHeaderStyle};" align="right">Qty</td>
            </tr>

            <!-- Group by item -->
             <xsl:for-each select="Customer/Order/Item[generate-id(.)=generate-id(key($keyTable, SKU))]">
                <xsl:sort select="concat(Product/Location, Location)" order="ascending" />
                <xsl:call-template name="outputItemGroup" />
            </xsl:for-each>

            <!--
                Totals
            -->
            <tr>
                <xsl:variable name="rowStyle" select="'padding: 4px 8px 4px 8px; border-top: 1px solid rgb(160, 160, 160);'" />
                <xsl:if test="$showThumbnails">
                    <td style="{$rowStyle}"></td>
				</xsl:if>
                <td style="{$rowStyle}"></td>
                <td style="{$rowStyle}"></td>
                <td style="{$rowStyle}" align="right"><b>Totals:</b></td>
                <td style="{$rowStyle}" align="right"><xsl:value-of select="sum(Customer/Order/Item/Quantity)" /></td>
            </tr>
        </table>

    </body>
    </html>
    </xsl:template>

    <!-- -->
    <!-- Outputs totals for a single item -->
    <!-- -->

    <xsl:template name="outputItemGroup">

        <xsl:variable name="groupQuantity" select="sum(key($keyTable,SKU)/Quantity)" />
        <xsl:variable name="groupTotal" select="sum(key($keyTable,SKU)/Total)" />

        <!-- We shouldn't have to conditionally apply the topborder... but IE is broken. -->
        <xsl:variable name="orderDetailContentStyle">
            padding: 4px 8px 4px 8px;
            vertical-align: top;
        <xsl:if test="position() != 1">border-top: 1px solid lightgrey;</xsl:if>
        </xsl:variable>


        <tr>
            <xsl:if test="$showThumbnails">
                <td style="{$orderDetailContentStyle};">
                    <xsl:choose>
                        <xsl:when test="Product/ImageUrl != ''">
                            <img src="{Product/ImageUrl}" alt="" style="height:50; width:50; border:0;" />
						</xsl:when>
                        <xsl:when test="Thumbnail != ''">
                            <img src="{Thumbnail}" alt="" style="height:50; width:50; border:0;" />
						</xsl:when>
					</xsl:choose>
                </td>
            </xsl:if>
            <td style="{$orderDetailContentStyle}; white-space: nowrap;">
                <xsl:choose>
                    <xsl:when test="Product/SKU != ''">
                        <xsl:value-of select="Product/SKU" />
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:value-of select="SKU" />
                    </xsl:otherwise>
                </xsl:choose>

                <br />

                <xsl:if test="Code != '' and SKU != Code">
                    <div style="padding-left:10px">
                        Code: <xsl:value-of select="Code" />
					</div>
				</xsl:if>
            </td>
            <td style="{$orderDetailContentStyle};">
                <xsl:choose>
                    <xsl:when test="Product/Name != ''">
                        <xsl:value-of select="Product/Name" />
                    </xsl:when>
                        <xsl:otherwise><xsl:value-of select="Name" />
                    </xsl:otherwise>
                </xsl:choose>

                <xsl:for-each select="Option">
                    <table style="width: 75%;" cellspacing="0">
                        <tr>
                            <td nowrap="nowrap" style="padding-left:10px"><xsl:value-of select="Name" />: </td>
                            <td style="width: 100%;"><xsl:value-of select="Description" /></td>
                        </tr>
                    </table>
                </xsl:for-each>

                <xsl:for-each select="Product/Attribute">
                    <table style="width: 75%;" cellspacing="0">
                        <tr>
                            <td nowrap="nowrap" style="padding-left:10px"><xsl:value-of select="Name" />: </td>
                            <td style="width: 100%;"><xsl:value-of select="Value" /></td>
                        </tr>
                    </table>
                </xsl:for-each>
            </td>
            <td style="{$orderDetailContentStyle};">
                <xsl:choose>
                    <xsl:when test="Product/Location != ''">
                        <xsl:value-of select="Product/Location" />
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:value-of select="Location" />
                    </xsl:otherwise>
                </xsl:choose>
            </td>
            <td style="{$orderDetailContentStyle};" align="right"><xsl:value-of select="$groupQuantity" /></td>
        </tr>

    </xsl:template>
</xsl:stylesheet>