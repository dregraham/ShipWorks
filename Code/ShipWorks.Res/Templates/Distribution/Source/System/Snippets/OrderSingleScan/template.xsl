<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

  <xsl:output method="html" encoding="utf-8" indent="yes" />

  <xsl:template name="OrderSingleScan">
    <xsl:param name="order" />
    <font style="font-family:Free 3 of 9 Extended; font-size:48pt;  font-weight:normal;">
      *SWO<xsl:value-of select="$order/@ID"/>*
    </font>
  </xsl:template>

</xsl:stylesheet>