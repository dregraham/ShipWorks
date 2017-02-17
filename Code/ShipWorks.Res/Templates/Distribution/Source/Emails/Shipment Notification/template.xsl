<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

    <!-- Imports -->
    <xsl:import href="System\Snippets" />

    <xsl:output method="html" encoding="utf-8" />

    <!-- Start of template -->
    <xsl:template match="/"><xsl:apply-templates /></xsl:template>
    <xsl:template match="ShipWorks">

    <!-- Default font.  Specified as a variable since GMail and Outlook behave differently. -->
    <xsl:variable name="pageFont" select="'font-family: Tahoma; font-size: 10pt;'" />

    <html>

    <head>
        <title>Shipment Notification</title>

        <style>
            body, table { <xsl:value-of select="$pageFont" /> }
        </style>

    </head>

    <body style="{$pageFont}">
        
        <xsl:variable name="order" select="Customer/Order[1]" />
        <xsl:variable name="address" select="$order/Address[@type='bill']" />
    
        <xsl:value-of select="$address/FirstName" />,
    
        <p>
            Your order       
            
            <!-- Shared Snippet -->
            #<xsl:call-template name="OrderNumber">
                <xsl:with-param name="order" select="$order" />
            </xsl:call-template> 
            
            has shipped and the tracking information is below.  Thank you for your business!
        </p>
    
        <p>
            <xsl:if test="count($order/Shipment[Status = 'Processed']) = 0">
                <i>(No shipments)</i>
            </xsl:if>
        
            <xsl:for-each select="$order/Shipment[Status = 'Processed']">
                Shipped on <b><xsl:value-of select="sw:ToShortDate(ShippedDate)" /></b>
                using <b><xsl:value-of select="ServiceUsed" /></b>: <b><xsl:call-template name="TrackingLink" /></b>
                <br />
            </xsl:for-each>
         </p>
     
        <p> 
            Sincerely,<br />
            <xsl:value-of select="Store/Address/Company" />
        </p>
    </body>

    </html>

</xsl:template>
</xsl:stylesheet>