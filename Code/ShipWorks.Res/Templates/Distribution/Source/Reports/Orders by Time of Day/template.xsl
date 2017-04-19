<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

    <!-- Imports -->
    <xsl:import href="System\Snippets" />

    <xsl:output method="html" encoding="utf-8" />

    <!-- Start of template -->
    <xsl:template match="/"><xsl:apply-templates /></xsl:template>
    <xsl:template match="ShipWorks">

    <!-- Width defined by the template PageSettings -->
    <xsl:variable name="pageWidth" select="concat(Template/Output/ContentWidth, 'in')" />

    <!-- Default font.  Specified as a variable since GMail and Outlook behave differently. -->
    <xsl:variable name="pageFont" select="'font-family: Arial; font-size: 8pt;'" />

    <!-- These styles are used on multiple td's so to avoid copy\paste errors they are defined once here.  We have to do this since GMail doesn't support <style> in the <head>.  -->
    <xsl:variable name="headerStyle" select="'border: 1px solid dimgray; background-color: #F3F3F3; font-weight: bold; padding: 3px;'" />
    <xsl:variable name="rowStyle" select="'padding: 4px 8px 4px 8px;vertical-align: top; border-top: 1px solid lightgrey;'" />

    <html>

    <head>
        <title>Orders by Time of Day</title>

        <style>
            body, table { <xsl:value-of select="$pageFont" /> }
        </style>

    </head>

    <xsl:variable name="hours">
        <hour code="00" display="12:00 AM to 1:00 AM" />
        <hour code="01" display="1:00 AM to 2:00 AM" />
        <hour code="02" display="2:00 AM to 3:00 AM" />
        <hour code="03" display="3:00 AM to 4:00 AM" />
        <hour code="04" display="4:00 AM to 5:00 AM" />
        <hour code="05" display="5:00 AM to 6:00 AM" />
        <hour code="06" display="6:00 AM to 7:00 AM" />
        <hour code="07" display="7:00 AM to 8:00 AM" />
        <hour code="08" display="8:00 AM to 9:00 AM" />
        <hour code="09" display="9:00 AM to 10:00 AM" />
        <hour code="10" display="10:00 AM to 11:00 AM" />
        <hour code="11" display="11:00 AM to 12:00 PM" />
        <hour code="12" display="12:00 PM to 1:00 PM" />
        <hour code="13" display="1:00 PM to 2:00 PM" />
        <hour code="14" display="2:00 PM to 3:00 PM" />
        <hour code="15" display="3:00 PM to 4:00 PM" />
        <hour code="16" display="4:00 PM to 5:00 PM" />
        <hour code="17" display="5:00 PM to 6:00 PM" />
        <hour code="18" display="6:00 PM to 7:00 PM" />
        <hour code="19" display="7:00 PM to 8:00 PM" />
        <hour code="20" display="8:00 PM to 9:00 PM" />
        <hour code="21" display="9:00 PM to 10:00 PM" />
        <hour code="22" display="10:00 PM to 11:00 PM" />
        <hour code="23" display="11:00 PM to 12:00 AM" />
	</xsl:variable>

    <body style="{$pageFont}">

         <h3 style="font-size: 1.67em; margin-top: 0;">Orders by Time of Day</h3>

         <b>Orders Reported: <xsl:value-of select="count(//Order)" /></b><br />
         <b>Orders Total: $<xsl:value-of select="format-number(sum(//Order/Total), '#,##0.00')" /></b>

         <table style="width:4in; margin: 10px 0px 0px 0px; border-collapse: collapse;" cellspacing="0">
            <tr>
                <td style="{$headerStyle};">Hour</td>
                <td style="{$headerStyle};">Count</td>
                <td style="{$headerStyle};">Total</td>
            </tr>

            <!-- Have to put this in variable since msxsl:node-set changes our root reference -->
            <xsl:variable name="allOrders" select="//Order" />

            <xsl:for-each select="msxsl:node-set($hours)/hour">

                <xsl:variable name="orders" select="$allOrders[sw:FormatDateTime(Date, 'HH')= current()/@code]" />

                <tr>
                    <td style="{$rowStyle};"><xsl:value-of select="@display" /></td>
                    <td style="{$rowStyle};" align="right"><xsl:value-of select="count($orders)" /></td>
                    <td style="{$rowStyle};" align="right">$<xsl:value-of select="format-number(sum($orders/Total), '#,##0.00')" /></td>
                </tr>

			</xsl:for-each>

        </table>

    </body>

    </html>

    </xsl:template>
</xsl:stylesheet>
