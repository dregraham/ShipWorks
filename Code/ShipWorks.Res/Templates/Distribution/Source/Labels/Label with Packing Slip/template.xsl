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

    <!-- Width defined by the template PageSettings -->
    <xsl:variable name="pageWidth" select="'100%'" />

    <!-- Default font.  Specified as a variable since GMail and Outlook behave differently. -->
    <xsl:variable name="pageFont" select="'font-family: Arial; font-size: 8pt;'" />

    <!-- These styles are used on multiple td's so to avoid copy\paste errors they are defined once here.  We have to do this since GMail doesn't support <style> in the <head>.  -->
    <xsl:variable name="orderDetailHeaderStyle" select="'border: 1px solid dimgray; background-color: #F3F3F3; font-weight: bold; padding: 3px;'" />
    <xsl:variable name="orderDetailAttributeStyle" select="'color: #808080; padding: 0px 8px 2px 2px;'" />
    <xsl:variable name="orderChargeStyle" select="'white-space: nowrap; text-align: right; padding: 1px 8px 3px 16px;'" />

    <html>

    <head>
        <title>Label with Packing Slip</title>

        <style>
            body, table { <xsl:value-of select="$pageFont" /> }
            table {
              margin-bottom: 4px;
              *margin-bottom: 0px;
            }
            div.shipping-summary {
              border: 1px solid dimgray;
              padding: 3px;
            }
            div.shipping-summary td {
              padding: 2px;
              *padding: 1px;
              vertical-align: top;
            }
            div.shipping-summary td.details {
              white-space: nowrap;
              padding-left: 2px;
            }
            td.address-header {
              font-weight: bold;
              border-bottom: 1px solid dimgray;
              background-color: #F3F3F3;
              padding: 2px 5px;
              *padding: 1px 5px;
            }
        </style>

    </head>
    <body style="{$pageFont}">

        <TemplatePartition>

        <xsl:variable name="order" select="Customer/Order[1]" />
        <table style="width: {$pageWidth};">
            <tr>
                <td style="width:100%; color: white; background-color: #C0C0C0; text-align: center; font-weight: bold; font-size: 12pt; border: 1px solid black; padding: 1px;">
                    Packing Slip
                </td>
            </tr>
        </table>

        <!--
            Store Address \ Order Number
        -->
        <table style="width:{$pageWidth}; margin: 10px 0px; *margin: 6px 0px;">
            <tr>

            <td style="vertical-align: top; width: 100%;">

                <!-- Shared Snippet -->
                <xsl:call-template name="StoreHeading">
                    <xsl:with-param name="address" select="Store/Address" />
                </xsl:call-template>

            </td>

            <td style="vertical-align: top;">
                <div class="shipping-summary">
                  <table style="font-weight: bold;" cellspacing="0">
                      <tr>
                          <td>Order:</td>
                          <td class="details">

                              <!-- Shared Snippet -->
                              <xsl:call-template name="OrderNumber">
                                  <xsl:with-param name="order" select="$order" />
                              </xsl:call-template>

                          </td>
                      </tr>
                      <tr>
                          <td>Placed:</td>
                          <td class="details">

                              <!-- Builtin ShipWorks function -->
                              <xsl:value-of select="sw:ToShortDate($order/Date)" />

                          </td>
                      </tr>
                      <tr>
                          <td>Shipping:</td>
                          <td class="details">

                              <!-- Builtin ShipWorks function -->
                              <xsl:value-of select="$order/RequestedShipping" />

                          </td>
                      </tr>
                  </table>
                </div>
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
                        <td class="address-header">
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
                        <td class="address-header">
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
        <table style="width:{$pageWidth}; margin: 0; border-collapse: collapse;" cellspacing="0">

            <tr>

                <!-- The variables controlling thumbnails are up near the top of the template. -->
                <xsl:if test="$thumbnailsEnabled">
                    <td style="{$orderDetailHeaderStyle}; width: {$thumbnailWidth};">Image</td>
                </xsl:if>

                <td style="{$orderDetailHeaderStyle}; width: 20%;">Item #</td>
                <td style="{$orderDetailHeaderStyle};">Name</td>
                <td style="{$orderDetailHeaderStyle};" align="right">QTY</td>
                
            </tr>

            <xsl:for-each select="$order/Item">

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
                        <xsl:value-of select="Quantity" />
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
                    </tr>
                </xsl:for-each>
            </xsl:for-each>

        </table>




        <!--
            Order Information section
        -->
        <table style="width:{$pageWidth}; border-collapse: collapse;" cellspacing="0">
            <tr>
                <td style="{$orderDetailHeaderStyle};">Order Information</td>
            </tr>
        </table>

        <div style="margin-top: 6px; margin-left: 8px; ">

            <!--
                Shipment information and tracking
            -->
            <xsl:if test="count($order/Shipment[Status = 'Processed']) &gt; 0">

                <b>Shipping</b>
                <div style="margin: 3 0 10 8;">

                    <xsl:for-each select="$order/Shipment[Status = 'Processed']">

                            Shipped on <b><xsl:value-of select="sw:ToShortDate(ShippedDate)" /></b>
                            using <b><xsl:value-of select="ServiceUsed" /></b>:

                            <!-- Shared Snippet -->
                            <b><xsl:call-template name="TrackingLink" /></b>
                            <br />
                    </xsl:for-each>

                </div>

            </xsl:if>


            <!--
                Notes
            -->
            <b>Notes</b>
            <div style="width:{$pageWidth}; margin: 3 0 10 8;">

                <xsl:if test="not(count($order/Note[Visibility='Public']))">
                    <i>None</i>
                </xsl:if>

                <xsl:for-each select="$order/Note[Visibility='Public']">
                    <xsl:value-of select="Text" />
                    <br />
                </xsl:for-each>

            </div>

            <b>Thank you!</b>
            <div style="width:{$pageWidth}; margin: 3 0 10 8;">
                Thank you for your purchase from <xsl:value-of select="Store/Address/Company" />!<br />
                If you have questions about your order please visit us online at <a href="{Store/Address/Website}"><xsl:value-of select="Store/Address/Website" /></a> or email us at <a href="mailto:{Store/Address/Email}"><xsl:value-of select="Store/Address/Email" /></a>.
            </div>
        </div>

        </TemplatePartition>

        <!--
            Ouput shipping label
        -->
        <xsl:variable name="labels" select="(//Primary | //Supplemental)/Label[@orientation = 'wide']" />

        <xsl:for-each select="$labels">

            <xsl:variable name="shipment" select="../../../.." />

            <TemplatePartition>
                <center>
                <table height="100%" width="100%" cellspacing="0">
                    <tr>
                        <td valign="middle" align="center">
                            <xsl:choose>
                                <xsl:when test="$shipment/ShipmentType = 'FedEx'">
                                    <img src="{.}" style="width:576; height:384;" />
                                </xsl:when>

                                <xsl:when test="$shipment/ShipmentType = 'UPS'">
                                    <img src="{.}" style="width:576; height:384;" />
                                </xsl:when>

                                <xsl:otherwise>
                                    <img src="{.}" style="width:{@widthInches}in; height:{@heightInches}in;" />
                                </xsl:otherwise>
                            </xsl:choose>
                        </td>
                    </tr>
                </table>


                </center>
            </TemplatePartition>

        </xsl:for-each>

        <!-- Test for even number of labels and move to a new partition. This ensures the next invoice will start a new page. -->
        <xsl:if test="count($labels) mod 2 = 0">
            <TemplatePartition>

            </TemplatePartition>
        </xsl:if>

</body>

</html>

</xsl:template>
</xsl:stylesheet>
