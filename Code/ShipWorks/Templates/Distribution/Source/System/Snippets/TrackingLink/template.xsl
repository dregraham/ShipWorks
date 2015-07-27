<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:output method="html" encoding="utf-8" indent="yes" />

  <!-- Start of snippet -->
  <xsl:template name="TrackingLink">
    <xsl:variable name="UpsMI" select="ShipmentType/@code = '1' and (starts-with(ServiceUsed, 'Priority Mail') or starts-with(ServiceUsed, 'First Class') 
                or starts-with(ServiceUsed, 'UPS Expedited Mail Innovations')  or starts-with(ServiceUsed, 'UPS Economy Mail Innovations') 
                or starts-with(ServiceUsed, 'UPS Priority Mail Innovations'))"/>

    <xsl:choose>

      <xsl:when test="TrackingNumber = ''">
        (No Tracking Available)
      </xsl:when>

      <xsl:when test="$UpsMI">
        <a href="http://www.ups-mi.net/packageID/PackageID.aspx?PID={TrackingNumber}">
          <xsl:value-of select="TrackingNumber" />
        </a>
      </xsl:when>

      <xsl:when test="starts-with(ServiceUsed, 'UPS') and not($UpsMI)">
        <a href="http://wwwapps.ups.com/WebTracking/processInputRequest?HTMLVersion=5.0&amp;loc=en_US&amp;Requester=UPSHome&amp;tracknum={TrackingNumber}&amp;AgreeToTermsAndConditions=yes&amp;track.x=46&amp;track.y=9">
          <xsl:value-of select="TrackingNumber" />
        </a>
      </xsl:when>

      <xsl:when test="starts-with(ServiceUsed, 'USPS') or contains(ServiceUsed, 'SmartPost') or contains(ServiceUsed, 'DHL SM')">

        <xsl:if test="contains(ServiceUsed, 'DHL SM')">
          <a href="http://webtrack.dhlglobalmail.com/?mobile=&amp;trackingnumber={TrackingNumber}">
            <xsl:value-of select="TrackingNumber"/>
          </a>
        </xsl:if>

        <xsl:if test="not(contains(ServiceUsed, 'DHL SM'))">
          <a href="https://tools.usps.com/go/TrackConfirmAction.action?tLabels={TrackingNumber}">
            <xsl:value-of select="TrackingNumber"/>
          </a>
        </xsl:if>

      </xsl:when>


      <xsl:when test="ShipmentType = 'FedEx'">
        <xsl:if test="not(contains(ServiceUsed, 'DHL SM'))">
          <a href="http://www.fedex.com/Tracking?language=english&amp;cntry_code=us&amp;tracknumbers={TrackingNumber}">
            <xsl:value-of select="TrackingNumber" />
          </a>
        </xsl:if>
        <xsl:if test="contains(ServiceUsed, 'FIMS')">
          <a href="http://mailviewrecipient.fedex.com/recip_package_summary.aspx?PostalID={TrackingNumber}">
            <xsl:value-of select="TrackingNumber" />
          </a>
        </xsl:if>
      </xsl:when>

      <xsl:otherwise>
        <xsl:value-of select="TrackingNumber" />
      </xsl:otherwise>

    </xsl:choose>

  </xsl:template>

</xsl:stylesheet>