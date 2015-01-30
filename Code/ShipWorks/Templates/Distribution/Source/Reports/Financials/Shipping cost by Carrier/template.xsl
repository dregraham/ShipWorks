<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

  <!-- Imports -->
  <xsl:import href="System\Snippets" />
  <xsl:output method="html" encoding="utf-8" />
  <!-- Start of template -->
  <xsl:template match="/">
    <xsl:apply-templates />
  </xsl:template>
  <xsl:template match="ShipWorks">
    <!-- Default font. Specified as a variable since GMail and Outlook behave differently. -->
    <xsl:variable name="pageFont" select="'font-family: Arial; font-size: 8pt;'" />
    <xsl:variable name="rowStyle" select="'padding: 4px 8px 4px 8px;vertical-align: top; border-top: 1px solid lightgrey;'" />

    <html>
      <head>
        <title>Shipping Cost by Carrier</title>
        <style>
          body, table { <xsl:value-of select="$pageFont" /> }
        </style>
      </head>
      <body style="{$pageFont}">

        <h3>
          Shipping Cost by Carrier - <xsl:value-of select="count(//Order)" /> Orders
        </h3>

        <xsl:variable name="other" select="sum(//Shipment[Status = 'Processed' and ShipmentType='Other']/TotalCharges)" />
        <xsl:variable name="fedex" select="sum(//Shipment[Status = 'Processed' and ShipmentType='FedEx']/TotalCharges)" />
        <xsl:variable name="ups" select="sum(//Shipment[Status = 'Processed' and ShipmentType='UPS']/TotalCharges)" />
        <xsl:variable name="upsWorldShip" select="sum(//Shipment[Status = 'Processed' and ShipmentType='UPS (WorldShip)']/TotalCharges)" />
        <xsl:variable name="uspsWeb" select="sum(//Shipment[Status = 'Processed' and ShipmentType='USPS (w/o Postage)']/TotalCharges)" />
        <xsl:variable name="uspsEndicia" select="sum(//Shipment[Status = 'Processed' and ShipmentType='USPS (Endicia)']/TotalCharges)" />
        <xsl:variable name="usps" select="sum(//Shipment[Status = 'Processed' and ShipmentType='USPS']/TotalCharges)" />
        <xsl:variable name="uspsStamps" select="sum(//Shipment[Status = 'Processed' and ShipmentType='USPS (Stamps.com)']/TotalCharges)" />
        <xsl:variable name="uspsExpress1" select="sum(//Shipment[Status = 'Processed' and ShipmentType='USPS (Express1)']/TotalCharges)" />
        <xsl:variable name="uspsExpress1Endicia" select="sum(//Shipment[Status = 'Processed' and ShipmentType='USPS (Express1 for Endicia)']/TotalCharges)" />
        <xsl:variable name="uspsExpress1Stamps" select="sum(//Shipment[Status = 'Processed' and ShipmentType='USPS (Express1 for Stamps)']/TotalCharges)" />
        <xsl:variable name="onTrac" select="sum(//Shipment[Status = 'Processed' and ShipmentType='OnTrac']/TotalCharges)" />
        <xsl:variable name="iParcel" select="sum(//Shipment[Status = 'Processed' and ShipmentType='i-parcel']/TotalCharges)" />
        <xsl:variable name="total" select="sum(//Shipment[Status = 'Processed']/TotalCharges)" />
        <table style="width:4in; margin: 0px 0px -6px 0px; border-collapse: collapse;" cellspacing="0">
          <tr>
            <td style="{$rowStyle};">'Other'</td>
            <td style="{$rowStyle};" align="right">
              $<xsl:value-of select="format-number($other, '#,##0.00')" />
            </td>
          </tr>
          <tr>
            <td style="{$rowStyle};">FedEx</td>
            <td style="{$rowStyle};" align="right">
              $<xsl:value-of select="format-number($fedex, '#,##0.00')" />
            </td>
          </tr>
          <tr>
            <td style="{$rowStyle};">UPS (Integrated)</td>
            <td style="{$rowStyle};" align="right">
              $<xsl:value-of select="format-number($ups, '#,##0.00')" />
            </td>
          </tr>
          <tr>
            <td style="{$rowStyle};">UPS (WorldShip)</td>
            <td style="{$rowStyle};" align="right">
              $<xsl:value-of select="format-number($upsWorldShip, '#,##0.00')" />
            </td>
          </tr>
          <tr>
            <td style="{$rowStyle};">USPS (w/o Postage)</td>
            <td style="{$rowStyle};" align="right">
              $<xsl:value-of select="format-number($uspsWeb, '#,##0.00')" />
            </td>
          </tr>
          <tr>
            <td style="{$rowStyle};">USPS</td>
            <td style="{$rowStyle};" align="right">
              $<xsl:value-of select="format-number($usps, '#,##0.00')" />
            </td>
          </tr>
          <tr>
            <td style="{$rowStyle};">USPS (Endicia)</td>
            <td style="{$rowStyle};" align="right">
              $<xsl:value-of select="format-number($uspsEndicia, '#,##0.00')" />
            </td>
          </tr>

          <tr>
            <td style="{$rowStyle};">USPS (Express1)</td>
            <td style="{$rowStyle};" align="right">
              $<xsl:value-of select="format-number($uspsExpress1, '#,##0.00')" />
            </td>
          </tr>

          <tr>
            <td style="{$rowStyle};">USPS (Express1 for Endicia)</td>
            <td style="{$rowStyle};" align="right">
              $<xsl:value-of select="format-number($uspsExpress1Endicia, '#,##0.00')" />
            </td>
          </tr>

          <tr>
            <td style="{$rowStyle};">USPS (Express1 for Stamps.com)</td>
            <td style="{$rowStyle};" align="right">
              $<xsl:value-of select="format-number($uspsExpress1Stamps, '#,##0.00')" />
            </td>
          </tr>
          <tr>
            <td style="{$rowStyle};">USPS (Stamps.com)</td>
            <td style="{$rowStyle};" align="right">
              $<xsl:value-of select="format-number($uspsStamps, '#,##0.00')" />
            </td>
          </tr>
          <tr>
            <td style="{$rowStyle};">OnTrac</td>
            <td style="{$rowStyle};" align="right">
              $<xsl:value-of select="format-number($onTrac, '#,##0.00')" />
            </td>
          </tr>
          <tr>
            <td style="{$rowStyle};">i-Parcel</td>
            <td style="{$rowStyle};" align="right">
              $<xsl:value-of select="format-number($iParcel, '#,##0.00')" />
            </td>
          </tr>
          <tr>
            <td style="{$rowStyle};">
              <b>Total:</b>
            </td>
            <td style="{$rowStyle};" align="right">
              <b>
                $<xsl:value-of select="format-number($total, '#,##0.00')" />
              </b>
            </td>
          </tr>

        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
