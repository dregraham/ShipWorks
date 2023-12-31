﻿<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

  <!-- Imports -->
  <xsl:import href="System\Snippets" />

  <xsl:output method="html" encoding="utf-8" indent="yes" />

  <!-- Start of template -->
  <xsl:template match="/">
    <xsl:apply-templates />
  </xsl:template>
  <xsl:template match="ShipWorks">

    <!-- Controls display of thumbnail images -->
    <xsl:variable name="thumbnailsEnabled" select="false()" />

    <!-- Width defined by the template PageSettings -->
    <xsl:variable name="pageWidth" select="concat(Template/Output/ContentWidth, 'in')" />

    <!-- Default font.  Specified as a variable since GMail and Outlook behave differently. -->
    <xsl:variable name="pageFont" select="'font-family: Arial; font-size: 8pt;'" />

    <!-- These styles are used on multiple td's so to avoid copy\paste errors they are defined once here.  We have to do this since GMail doesn't support <style> in the <head>.  -->
    <xsl:variable name="orderDetailHeaderStyle" select="'border: 1px solid dimgray; background-color: #F3F3F3; font-weight: bold; padding: 3px;'" />
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
                Invoice
              </td>
            </tr>
          </table>

          <!--
            Store Address \ Order Number
        -->
          <table style="width:{$pageWidth}; margin: 6px 0px;">
            <tr>

              <td style="width: 100%;">

                <!-- Shared Snippet -->
                <xsl:call-template name="StoreHeading">
                  <xsl:with-param name="address" select="Store/Address" />
                </xsl:call-template>

              </td>

              <td style="vertical-align: top;">
                <table style="border: 1px solid dimgray;">
                  <tr>
                    <td>

                      <table style="font-weight: bold;" cellspacing="0">
                        <tr>
                          <td style="padding-right: 2px; vertical-align: top;">Order:</td>
                          <td style="white-space: nowrap;">

                            <!-- Shared Snippet -->
                            <xsl:call-template name="OrderNumber">
                              <xsl:with-param name="order" select="$order" />
                            </xsl:call-template>

                          </td>
                        </tr>
                        <tr>
                          <td style="padding-right: 2px;">Placed:</td>
                          <td style="white-space: nowrap;">

                            <!-- Builtin ShipWorks function -->
                            <xsl:value-of select="sw:ToShortDate($order/Date)" />

                          </td>
                        </tr>
                        <tr>
                          <td style="padding-right: 2px; vertical-align:top;">Shipping:</td>
                          <td style="white-space: nowrap;">

                            <!-- Builtin ShipWorks function -->
                            <xsl:value-of select="$order/RequestedShipping" />

                          </td>
                        </tr>
                      </table>

                    </td>
                  </tr>
                </table>

              </td>
            </tr>
          </table>

          <!--
            Bill To \ Ship To
        -->
          <table style="width:{$pageWidth}; margin: 12px 0px;" cellspacing="0">
            <tr>

              <td style="width: 47.5%; padding: 1px; border: 1px solid dimgray; vertical-align: top;">

                <table style="width: 100%;" cellspacing="0">
                  <tr>
                    <td style="font-weight: bold; border-bottom: 1px solid dimgray; background-color: #F3F3F3; padding: 1px 5px;">
                      Ship To
                    </td>
                  </tr>

                  <tr>
                    <td style="padding: 4px 10px;">

                      <!-- Shared Snippet -->
                      <xsl:call-template name="InvoiceShipTo">
                        <xsl:with-param name="order" select="$order" />
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
                      Bill To
                    </td>
                  </tr>

                  <tr>
                    <td style="padding: 4px 10px;">

                      <!-- Shared Snippet -->
                      <xsl:call-template name="InvoiceBillTo">
                        <xsl:with-param name="order" select="$order" />
                      </xsl:call-template>

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
          <xsl:call-template name="ItemGroup">
            <xsl:with-param name="order" select="$order" />
            <xsl:with-param name="optionSpecific" select="true()" />
            <xsl:with-param name="showThumbnailImages" select="$thumbnailsEnabled" />
            <xsl:with-param name="showPrices" select="true()" />
          </xsl:call-template>

          <div style="border-top: 1px solid grey; width:{$pageWidth}; height: 1px; margin: 0px 0px 10px 0px; *margin-bottom: 1;"></div>

          <!--
            Totals
        -->
          <table style="width:{$pageWidth};" cellspacing="0">

            <!--
                Subtotal
            -->

            <tr>
              <td style="{$orderChargeStyle}; width: 100%;">
                Subtotal:
              </td>
              <td style="{$orderChargeStyle};">
                <xsl:value-of select="format-number(sum($order/Item/Total), '#,##0.00')" />
              </td>
            </tr>

            <!--
                Order Charges
            -->

            <xsl:for-each select="$order/Charge">
              <tr>
                <td style="{$orderChargeStyle}; width: 100%;">
                  <xsl:value-of select="Description" />:
                </td>
                <td style="{$orderChargeStyle};">
                  <xsl:value-of select="format-number(Amount, '#,##0.00')" />
                </td>
              </tr>
            </xsl:for-each>

            <!--
                Order Total
            -->

            <tr>
              <td style="{$orderChargeStyle}; font-weight: bold;  width: 100%;">Order Total:</td>
              <td style="{$orderChargeStyle}; font-weight: bold;">
                <xsl:value-of select="format-number($order/Total, '#,##0.00')" />
              </td>
            </tr>
          </table>

          <br />

          <!--
            Order Information section
        -->
          <table style="width:{$pageWidth}; border-collapse: collapse;" cellspacing="0">
            <tr>
              <td style="{$orderDetailHeaderStyle};">Order Information</td>
            </tr>
          </table>

          <div style="width:{$pageWidth};">
            <div style="margin-top: 6px; margin-left: 8px; ">

              <!--
                  Shipment information and tracking
              -->
              <xsl:if test="count($order/Shipment[Status = 'Processed']) &gt; 0">

                <b>Shipping</b>
                <div style="margin: 3 0 10 8;">

                  <xsl:for-each select="$order/Shipment[Status = 'Processed']">

                    Shipped on <b>
                      <xsl:value-of select="sw:ToShortDate(ShippedDate)" />
                    </b>
                    using <b>
                      <xsl:value-of select="ServiceUsed" />
                    </b>:

                    <!-- Shared Snippet -->
                    <b>
                      <xsl:call-template name="TrackingLink" />
                    </b>
                    <br />
                  </xsl:for-each>

                </div>

              </xsl:if>


              <!--
                  Notes
              -->
              <b>Notes</b>
              <div style="margin: 3 0 10 8;">

                <xsl:if test="not(count($order/Note[Visibility='Public']))">
                  <i>None</i>
                </xsl:if>

                <xsl:for-each select="$order/Note[Visibility='Public']">
                  <xsl:value-of select="Text" />
                  <br />
                </xsl:for-each>

              </div>

              <b>Thank you!</b>
              <div style="margin: 3 0 10 8;">
                Thank you for your purchase from <xsl:value-of select="Store/Address/Company" />!<br />
                If you have questions about your order please visit us online at <a href="{Store/Address/Website}">
                  <xsl:value-of select="Store/Address/Website" />
                </a> or email us at <a href="mailto:{Store/Address/Email}">
                  <xsl:value-of select="Store/Address/Email" />
                </a>.
              </div>
            </div>
          </div>
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