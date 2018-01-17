<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

    <!-- Imports -->
    <xsl:import href="System\Snippets" />
  
    <xsl:output method="html" encoding="utf-8" />

    <!-- Start of template -->
    <xsl:template match="/"><xsl:apply-templates /></xsl:template>
    <xsl:template match="ShipWorks">

    <html>

    <head>
        <title>ShipWorks Template</title>
    </head>

    <body>

        <xsl:variable name="width" select="//Template/Output/ContentWidth" />
        <xsl:variable name="height" select="//Template/Output/ContentHeight" />

        <xsl:choose>
            <xsl:when test="$width &gt; $height">
                
                <xsl:variable name="labels" select="(//Primary | //Supplemental)/Label[@orientation = 'wide']" />
                
                <xsl:for-each select="$labels">
                    <TemplatePartition>
                        <img src="{.}" style="width:100%; height:100%;" />
                    </TemplatePartition>
                </xsl:for-each>
                
			</xsl:when>
            <xsl:otherwise>
                
                <xsl:variable name="labels" select="(//Primary | //Supplemental)/Label[@orientation = 'tall']" />
                
                <xsl:for-each select="$labels">
                    <TemplatePartition>
                        <img src="{.}" style="width:100%; height:100%;" />
                    </TemplatePartition>
                </xsl:for-each>
                
			</xsl:otherwise>
		</xsl:choose>
    </body>

    </html>

    </xsl:template>
</xsl:stylesheet>