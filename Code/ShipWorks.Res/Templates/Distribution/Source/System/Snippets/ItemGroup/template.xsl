<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">
  <!-- Imports -->
  <xsl:import href="System\Snippets\OrderItemCode" />

  <xsl:output method="html" encoding="utf-8" indent="yes" />

  <!-- Setup the key table for grouping by item code -->
  <xsl:key name="items-specific" match="Item" use="sw:GetOrderItemKeyValue(., true())" />
  <xsl:key name="items-non-specific" match="Item" use="sw:GetOrderItemKeyValue(., false())" />

  <!-- Start of template -->
  <xsl:template name="ItemGroup">
    <xsl:param name="order" />
    <xsl:param name="optionSpecific" />
    <xsl:param name="thumbnailsEnabled" />
    <xsl:param name="showPrices" />

    <!-- Determine which key table to use based on if we are option specific or not -->
    <xsl:variable name="keyTable">
      <xsl:if test="$optionSpecific">
        <xsl:value-of select="'items-specific'" />
      </xsl:if>
      <xsl:if test="not($optionSpecific)">
        <xsl:value-of select="'items-non-specific'" />
      </xsl:if>
    </xsl:variable>

    <!-- Controls display of thumbnail images -->
    <xsl:variable name="thumbnailWidth" select="'50px'" />
    <xsl:variable name="thumbnailHeight" select="'50px'" />

    <!-- Width defined by the template PageSettings -->
    <xsl:variable name="pageWidth" select="concat(Template/Output/ContentWidth, 'in')" />

    <!-- Default font.  Specified as a variable since GMail and Outlook behave differently. -->
    <xsl:variable name="pageFont" select="'font-family: Arial; font-size: 8pt;'" />

    <!-- These styles are used on multiple td's so to avoid copy\paste errors they are defined once here.  We have to do this since GMail doesn't support <style> in the <head>.  -->
    <xsl:variable name="orderDetailHeaderStyle" select="'border: 1px solid dimgray; background-color: #F3F3F3; font-weight: bold; padding: 3px;'" />
    <xsl:variable name="orderDetailAttributeStyle" select="'color: #808080; padding: 0px 8px 2px 2px;'" />
    <xsl:variable name="orderChargeStyle" select="'white-space: nowrap; text-align: right; padding: 1px 8px 3px 16px;'" />


    <!--
            Line Items
        -->
    <table style="width:{$pageWidth}; margin: 0; border-collapse: collapse;" cellspacing="0">
      <tr>
        <!-- The variables controlling thumbnails are up near the top of the template. -->
        <xsl:if test="$thumbnailsEnabled">
          <td style="{$orderDetailHeaderStyle}; width: {$thumbnailWidth};">Image</td>
        </xsl:if>

        <td style="{$orderDetailHeaderStyle}; width: 20%;">Item #</td>
        <td style="{$orderDetailHeaderStyle};">Name</td>
        <td style="{$orderDetailHeaderStyle};" align="right">QTY</td>
        <xsl:if test="$showPrices">
          <td style="{$orderDetailHeaderStyle};" align="right">Price</td>
          <td style="{$orderDetailHeaderStyle};" align="right">Total</td>
        </xsl:if>
      </tr>

      <xsl:for-each select="$order/Item[generate-id(.)=generate-id(key($keyTable, sw:GetOrderItemKeyValue(., $optionSpecific)))]">

        <xsl:variable name="groupQuantity" select="sum(key($keyTable, sw:GetOrderItemKeyValue(., $optionSpecific))/Quantity)" />
        <xsl:variable name="groupTotal" select="sum(key($keyTable, sw:GetOrderItemKeyValue(., $optionSpecific))/Total)" />

        <!-- We shouldn't have to conditionally apply the topborder... but IE is broken. -->
        <xsl:variable name="orderDetailContentStyle">
          padding: 4px 8px 4px 8px;
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
            <xsl:value-of select="$groupQuantity" />
          </td>
          <xsl:if test="$showPrices">
            <td style="{$orderDetailContentStyle};" align="right">
              <xsl:value-of select="format-number(UnitPrice, '#,##0.00')" />
            </td>
            <td style="{$orderDetailContentStyle};" align="right">
              <xsl:value-of select="format-number($groupTotal * Quantity, '#,##0.00')" />
            </td>
          </xsl:if>
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
            <td></td>
            <td></td>
          </tr>
        </xsl:for-each>
      </xsl:for-each>
    </table>

  </xsl:template>

</xsl:stylesheet>