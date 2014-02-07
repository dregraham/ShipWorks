<!DOCTYPE xsl:stylesheet []>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

  <!-- Imports -->
  <xsl:import href="System\Snippets" />

  <xsl:output method="html" encoding="utf-8" />

  <!-- Setup the key table for grouping by Shipment/User -->
  <xsl:key name="employee" match="ProcessedUser" use="@ID" />

  <!-- Start of template -->
  <xsl:template match="/">
    <xsl:apply-templates />
  </xsl:template>
  <xsl:template match="ShipWorks">

    <!-- Width defined by the template PageSettings -->
    <xsl:variable name="pageWidth" select="concat(Template/Output/ContentWidth, ' in')" />

    <!-- Default font.  Specified as a variable since GMail and Outlook behave differently. -->
    <xsl:variable name="pageFont" select="'font-family: Arial; font-size: 8pt;'" />

    <!-- These styles are used on multiple td's so to avoid copy\paste errors they are defined once here.  We have to do this since GMail doesn't support <style> in the <head>.  -->
    <xsl:variable name="headerStyle" select="'border: 1px solid dimgray; background-color: #F3F3F3; font-weight: bold; padding: 3px;'" />

    <html>

      <head>
        <title>Shipper Productivity (by User)</title>

        <style>
          body, table { <xsl:value-of select="$pageFont" /> }
        </style>

      </head>

      <body style="{$pageFont}">

        <h3>
          Shipper Productivity (by User) - <xsl:value-of select="count(//Shipment[Processed = 'true'])" /> Shipments
        </h3>

        <table style="width:{$pageWidth}; margin: 0px 0px -6px 0px; border-collapse: collapse;" cellspacing="0">

          <tr>
            <td style="{$headerStyle}; white-space: nowrap;">User</td>
            <td style="{$headerStyle};" align='right'># Processed</td>
            <td style="{$headerStyle};" align='right'># Voided (by any user)</td>
            <td style="{$headerStyle};" align='right'>Cost of Shipments</td>
          </tr>

          <!-- Group by quantity        -->
          <xsl:for-each select="//Shipment/ProcessedUser[generate-id(.)=generate-id(key('employee',@ID))]">
            <xsl:call-template name="outputUserGroup" />
          </xsl:for-each>

        </table>

      </body>

    </html>

  </xsl:template>

  <!--                                                            -->
  <!-- Outputs totals for a single User                           -->
  <!--                                                            -->
  <xsl:template name="outputUserGroup">
    <!-- We shouldn't have to conditionally apply the topborder... but IE is broken. -->
    <xsl:variable name="detailContentStyle">
      padding: 4px 8px 4px 8px;
      vertical-align: top;
      <xsl:if test="position() != 1">border-top: 1px solid lightgrey;</xsl:if>
    </xsl:variable>
    <xsl:variable name="username" select="Username"/>

    <tr>

      <td style="{$detailContentStyle}; white-space: nowrap;">
        <xsl:value-of select="Username" />
      </td>
      <td style="{$detailContentStyle}; white-space: nowrap;" align='right'>
        <xsl:value-of select="count(//Shipment[ProcessedUser/Username=$username and (Status='Processed' or Status='Voided')])" />
      </td>
      <td style="{$detailContentStyle}; white-space: nowrap;" align='right'>
        <xsl:value-of select="count(//Shipment[ProcessedUser/Username=$username and Status='Voided'])" />
      </td>
      <td style="{$detailContentStyle}; white-space: nowrap;" align='right'>
        $ <xsl:value-of select="format-number(sum(//Shipment[ProcessedUser/Username=$username and Status='Processed']/TotalCharges), '#,##0.00')" />
      </td>
    </tr>

  </xsl:template>


</xsl:stylesheet>