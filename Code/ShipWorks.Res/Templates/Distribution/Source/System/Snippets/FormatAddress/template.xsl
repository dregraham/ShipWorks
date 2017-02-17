<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

    <xsl:output method="html" encoding="utf-8" indent="yes" />

    <xsl:template name="FormatAddress">
        <xsl:param name="address" />

        <xsl:if test="$address/FirstName != '' or $address/LastName != ''">
            <xsl:value-of select="$address/FirstName" />
            <xsl:text> </xsl:text>
            <xsl:value-of select="$address/LastName" />
            <br />
        </xsl:if>

        <xsl:if test="$address/Company != ''">
            <xsl:value-of select="$address/Company" />
            <br />
        </xsl:if>

        <xsl:value-of select="$address/Line1" />
        <br />

        <xsl:if test="$address/Line2 != ''">
            <xsl:value-of select="$address/Line2" />
            <br />
        </xsl:if>

        <xsl:if test="$address/Line3 != ''">
            <xsl:value-of select="$address/Line3" />
            <br />
        </xsl:if>

        <xsl:value-of select="$address/City" />,
        <xsl:value-of select="$address/StateName" /><xsl:text> </xsl:text>
        <xsl:value-of select="$address/PostalCode" />
        <br />

        <xsl:if test="$address/CountryCode != 'US'">
            <xsl:value-of select="$address/CountryName" />
            <br />
        </xsl:if>

        <xsl:if test="$address/Phone != ''">
            <xsl:value-of select="$address/Phone" />
            <br />
        </xsl:if>

        <xsl:if test="$address/Email != ''">
            <xsl:value-of select="$address/Email" />
            <br />
        </xsl:if>

    </xsl:template>

</xsl:stylesheet>