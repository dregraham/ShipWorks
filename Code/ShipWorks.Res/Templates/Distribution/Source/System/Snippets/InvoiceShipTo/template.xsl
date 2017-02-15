<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

    <xsl:import href="System\Snippets\FormatAddress" />

    <xsl:output method="html" encoding="utf-8" indent="yes" />

    <xsl:template name="InvoiceShipTo">
        <xsl:param name="order" />

        <xsl:variable name="showAddress" select="$order/IsManual = 'true' or /ShipWorks/Store[@ID = $order/@storeID]/StoreType != 'eBay' or $order/eBay/CheckoutComplete = 'true'" />

        <xsl:if test="$showAddress">
            <xsl:call-template name="FormatAddress">
                <xsl:with-param name="address" select="$order/Address[@type='ship']" />
            </xsl:call-template>
        </xsl:if>

        <xsl:if test="not($showAddress)">
            <font color="#FF0000">
                <b>Checkout incomplete.</b>
            </font>
            <br />
            Winning Buyer: <xsl:value-of select="$order/eBay/BuyerID" />
        </xsl:if>

    </xsl:template>

</xsl:stylesheet>