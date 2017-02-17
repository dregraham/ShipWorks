<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

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
        <title>Order Status Summary</title>

        <style>
            body, table { <xsl:value-of select="$pageFont" /> }
        </style>

    </head>

    <body style="{$pageFont}">
        
        <h3>Order Status Summary</h3>
        
          <table style="width:{$pageWidth}; margin: 10px 0px -6px 0px; border-collapse: collapse;" cellspacing="0">
            <tr>
                <td style="{$headerStyle};">Order</td>
                <td style="{$headerStyle};">Date</td>
                <td style="{$headerStyle};">Name</td>
                <td style="{$headerStyle};">Status</td>
            </tr>
            
            <!-- Group by order number -->
            <xsl:for-each select="Customer/Order">
                <xsl:sort select="Number" order="descending" data-type="number" />
                 
                <!-- We shouldn't have to conditionally apply the topborder... but IE is broken. -->
                <xsl:variable name="rowStyle">
                    padding: 4px 8px 4px 8px;
                    <xsl:if test="position() != 1">border-top: 1px solid lightgrey;</xsl:if>
                </xsl:variable>   
                
                <tr>
                    <td style="{$rowStyle}">
                        <!-- Shared Snippet -->
                        <xsl:call-template name="OrderNumber">
                            <xsl:with-param name="order" select="." />
                        </xsl:call-template>
                    </td>
                    <td style="{$rowStyle}"><xsl:value-of select="sw:ToShortDate(Date)" /></td>
                    <td style="{$rowStyle}"><xsl:value-of select="Address[@type='bill']/LastName" />, <xsl:value-of select="Address[@type='bill']/FirstName" /></td>
                    <td style="{$rowStyle}"><xsl:value-of select="Status" /></td>
                </tr>
                                                
            </xsl:for-each>
            
        </table>           
       
    </body>

    </html>

    </xsl:template>
</xsl:stylesheet>