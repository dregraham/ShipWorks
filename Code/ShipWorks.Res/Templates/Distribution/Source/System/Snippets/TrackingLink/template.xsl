<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:output method="html" encoding="utf-8" indent="yes" />

  <!-- Start of snippet -->
  <xsl:template name="TrackingLink">
    
  <xsl:choose>
          
      <xsl:when test="TrackingNumber = ''">
        (No Tracking Available)
      </xsl:when>

      <xsl:when test="contains(ServiceUsed, 'UPS')">
        <a href="http://wwwapps.ups.com/WebTracking/processInputRequest?HTMLVersion=5.0&amp;loc=en_US&amp;Requester=UPSHome&amp;tracknum={TrackingNumber}&amp;AgreeToTermsAndConditions=yes&amp;track.x=46&amp;track.y=9">
          <xsl:value-of select="TrackingNumber" />
        </a>
      </xsl:when>

	  <xsl:when test="contains(ServiceUsed, 'DHL SM')">
		  <a href="http://webtrack.dhlglobalmail.com/?mobile=&amp;trackingnumber={TrackingNumber}">
			  <xsl:value-of select="TrackingNumber"/>
		  </a>
	  </xsl:when>

	  <xsl:when test="ShipmentType = 'DHL eCommerce'">
		  <a href="http://webtrack.dhlglobalmail.com/?trackingnumber={TrackingNumber}">
			  <xsl:value-of select="TrackingNumber"/>
		  </a>
	  </xsl:when>

    <xsl:when test="contains(ServiceUsed, 'USPS')">
        <a href="https://tools.usps.com/go/TrackConfirmAction.action?tLabels={TrackingNumber}">
          <xsl:value-of select="TrackingNumber"/>
        </a>
    </xsl:when>

      <xsl:when test="contains(ServiceUsed, 'FedEx')">
        <xsl:choose>
          <xsl:when test="contains(ServiceUsed, 'FIMS')">
            <a href="http://mailviewrecipient.fedex.com/recip_package_summary.aspx?PostalID={TrackingNumber}">
              <xsl:value-of select="TrackingNumber" />
            </a>
          </xsl:when>
          <xsl:otherwise>
            <a href="https://www.fedex.com/fedextrack/?trknbr={TrackingNumber}">
              <xsl:value-of select="TrackingNumber" />
            </a>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
	  
	  <xsl:when test="contains(ServiceUsed, 'OnTrac')">
		  <a href="https://www.ontrac.com/tracking.asp?trackingres=submit&amp;tracking_number={TrackingNumber}">
			  <xsl:value-of select="TrackingNumber"/>
		  </a>
	  </xsl:when>

	  <xsl:when test="contains(ServiceUsed, 'DYNAMEX')">
		  <a href="https://www.ordertracker.com/track/{TrackingNumber}">
			  <xsl:value-of select="TrackingNumber"/>
		  </a>
	  </xsl:when>
	  
      <xsl:otherwise>
        <xsl:value-of select="TrackingNumber" />
      </xsl:otherwise>

    </xsl:choose>

  </xsl:template>

</xsl:stylesheet>