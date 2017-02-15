<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

    <xsl:output method="html" encoding="utf-8" indent="yes" />

    <xsl:template name="OrderItemCode">
        <xsl:param name="item" />

        <xsl:choose>
            <xsl:when test="$item/../IsManual = 'true' or /ShipWorks/Store[@ID = $item/../@storeID]/StoreType != 'eBay'">
                <xsl:value-of select="$item/Code" />
            </xsl:when>
            <xsl:otherwise>
                <a href="http://cgi.ebay.com/ws/eBayISAPI.dll?ViewItem&amp;item={$item/Code}">
                    <xsl:value-of select="$item/Code" />
                </a>
            </xsl:otherwise>
        </xsl:choose>

    </xsl:template>

</xsl:stylesheet>