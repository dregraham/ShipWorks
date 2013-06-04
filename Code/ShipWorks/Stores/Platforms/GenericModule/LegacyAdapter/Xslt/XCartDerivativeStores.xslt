<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
    exclude-result-prefixes="msxsl" 
    extension-element-prefixes="sw"
    xmlns:sw="http://www.interapptive.com/shipworks/extensions"
>
  <xsl:param name="moduleAction"/>
  
  <!-- Stylesheet for transforming X-Cart V2 Communications to conform to the ShipWorks V3 schema.  -->
  <xsl:output method="xml" indent="yes"/>
  <xsl:template match="/">

    <!-- see if we have a ShipWorks root, indicating it's likely a V2 response -->
    <xsl:variable name="shipworksDocument" select="count(/ShipWorks) = 1"/>
    <xsl:choose>
      <xsl:when test="$shipworksDocument">
        <ShipWorks>

          <!-- need to write Module Version -->
          <xsl:variable name="moduleVersion" select="string(/ShipWorks/ModuleVersion)"/>
          <xsl:choose>
            <xsl:when test="$moduleVersion = ''">
              <xsl:attribute name="moduleVersion">2.9.0</xsl:attribute>
            </xsl:when>
            <xsl:otherwise>
              <xsl:attribute name="moduleVersion">
                <xsl:value-of select="$moduleVersion"/>
              </xsl:attribute>
            </xsl:otherwise>
          </xsl:choose>

          <xsl:attribute name="schemaVersion">0.0.0</xsl:attribute>
          
          <!-- pull over verbatim any Parameters -->
          <xsl:apply-templates select="/ShipWorks/Parameters"/>

          <xsl:choose>
            <!-- Test for Errors -->
            <xsl:when test="count(//Error) = 1">
              <xsl:apply-templates select="//Error"/>
            </xsl:when>

            <!-- Test if this is a GetStore response -->
            <xsl:when test="$moduleAction='getstore'">
              <xsl:apply-templates select="/ShipWorks/Store"/>
            </xsl:when>

            <!-- Test if this is a GetStatusCodes reponse -->
            <xsl:when test="$moduleAction = 'getstatuscodes'">
              <xsl:choose>
                <xsl:when test="count(/ShipWorks/StatusCodes) = 0">
                  <xsl:call-template name="outputDefaultStatusCodes"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:apply-templates select="/ShipWorks/StatusCodes"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>

            <!-- Test for GetCount response -->
            <xsl:when test="$moduleAction = 'getcount'">
              <xsl:apply-templates select="/ShipWorks/OrderCount"/>
            </xsl:when>

            <!-- Test for GetOrders response -->
            <xsl:when test="$moduleAction = 'getorders'">
              <xsl:apply-templates select="/ShipWorks/Orders"/>
            </xsl:when>

            <!-- Default to outputting the store data -->
            <xsl:otherwise>
              <xsl:call-template name="outputStore"/>
            </xsl:otherwise>

          </xsl:choose>

        </ShipWorks>
      </xsl:when>
      <xsl:otherwise>
        <!-- no chance of being a valid V2 response -->
        <xsl:call-template name="outputError">
          <xsl:with-param name="outputRoot" select="true"/>
          <xsl:with-param name="errorString">
            The original response was not a Version 2 response.  Aborting.
          </xsl:with-param>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="Store" name="outputStore">
    <Store>
      <Name>
        <xsl:value-of select="Company"/>
      </Name>
      <CompanyOrOwner>
        <xsl:value-of select="Company"/>
      </CompanyOrOwner>
      <Email>
        <xsl:value-of select="Email"/>
      </Email>
      <Street1>
        <xsl:value-of select="Address1" />
      </Street1>
      <City>
        <xsl:value-of select="City"/>
      </City>
      <State>
        <xsl:value-of select="State"/>
      </State>
      <PostalCode>
        <xsl:value-of select="ZipCode"/>
      </PostalCode>
      <Country>
        <xsl:value-of select="Country"/>
      </Country>
      <Phone>
        <xsl:value-of select="Phone"/>
      </Phone>
      <Website>
        <xsl:value-of select="Website"/>
      </Website>

    </Store>
  </xsl:template>

  <xsl:template match="OrderCount">
    <OrderCount>
      <xsl:value-of select="text()"/>
    </OrderCount>

  </xsl:template>

  <!-- Translate the old StatusCodes to the new -->
  <xsl:template match="StatusCodes">
    <StatusCodes>
      <xsl:for-each select="StatusCode">
        <StatusCode>
          <Code>
            <xsl:value-of select="Code"/>
          </Code>
          <Name>
            <xsl:value-of select="Name"/>
          </Name>
        </StatusCode>
      </xsl:for-each>
    </StatusCodes>
  </xsl:template>

  <!-- XCart initially didn't support order statuses, there was just this list of hard coded ones. This template
  outputs the default statuses -->
  <xsl:template name="outputDefaultStatusCodes">
    <StatusCodes>
      <StatusCode>
        <Code>Queued</Code>
        <Name>Queued</Name>
      </StatusCode>
      <StatusCode>
        <Code>Processed</Code>
        <Name>Processed</Name>
      </StatusCode>
      <StatusCode>
        <Code>Complete</Code>
        <Name>Complete</Name>
      </StatusCode>
      <StatusCode>
        <Code>Backordered</Code>
        <Name>Backordered</Name>
      </StatusCode>
      <StatusCode>
        <Code>Declined</Code>
        <Name>Declined</Name>
      </StatusCode>
      <StatusCode>
        <Code>Failed</Code>
        <Name>Failed</Name>
      </StatusCode>
      <StatusCode>
        <Code>Not Finished</Code>
        <Name>Not Finished</Name>
      </StatusCode>
    </StatusCodes>
  </xsl:template>

  <xsl:template match="Orders">
    <Orders>
      <xsl:apply-templates select="Order"/>
    </Orders>
  </xsl:template>

  <xsl:template match="Order">
    <Order>
      <OrderNumber>
        <xsl:value-of select="OrderNumber"/>
      </OrderNumber>
      <OrderDate>
        <xsl:call-template name="FormatDate">
          <xsl:with-param name="dateString">
            <xsl:value-of select="OrderDate"/>
          </xsl:with-param>
        </xsl:call-template>
      </OrderDate>

      <ShippingMethod>
        <xsl:value-of select="ShippingMethod"/>
      </ShippingMethod>

      <StatusCode>
        <xsl:value-of select="Status"/>
      </StatusCode>

      <CustomerID>
        <xsl:value-of select="Customer/CustomerID"/>
      </CustomerID>

      <Notes>
        <xsl:apply-templates select="CustomerComments"/>
      </Notes>

      <ShippingAddress>
        <xsl:apply-templates select="ShipAddress"/>
      </ShippingAddress>

      <BillingAddress>
        <xsl:apply-templates select="BillAddress"/>
      </BillingAddress>

      <Payment>
        <xsl:apply-templates select="Payment"/>
      </Payment>

      <Items>
        <xsl:apply-templates select="Items"/>
      </Items>

      <Totals>
        <xsl:apply-templates select="Totals"/>
      </Totals>

      <!-- copy all debug nodes even though they don't appear in v2 -->
      <Debug>
        <xsl:copy-of select="Debug/*"/>
      </Debug>
    </Order>
  </xsl:template>

  <xsl:template match="Totals">
    <xsl:for-each select="Total[Class != 'Total' and Class != 'SubTotal']">
      <Total>
        <xsl:attribute name="id">
          <xsl:call-template name="getNumber">
            <xsl:with-param name="value" select="TotalID"/>
            <xsl:with-param name="default" select="0"/>
          </xsl:call-template>
        </xsl:attribute>
        <xsl:attribute name="name">
          <xsl:value-of select="Name"/>
        </xsl:attribute>
        <xsl:attribute name="class">
          <xsl:value-of select="Class"/>
        </xsl:attribute>
        <xsl:choose>
          <xsl:when test="(number(Value) &lt; 0) or (Class = 'GiftCert' or Class = 'Coupon')">
            <xsl:attribute name="impact">subtract</xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="impact">add</xsl:attribute>
          </xsl:otherwise>
        </xsl:choose>
        <xsl:call-template name="getNumber">
          <xsl:with-param name="value" select="Value"/>
          <xsl:with-param name="default" select="0"/>
        </xsl:call-template>
      </Total>
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="Items">
    <xsl:for-each select="Item">
      
      <!-- calculate the base price -->
      <xsl:variable name="basePrice">
        <xsl:call-template name="calculateItemBasePrice">
          <xsl:with-param name="unitPrice">
            <xsl:call-template name="getNumber">
              <xsl:with-param name="value" select="UnitPrice"/>
              <xsl:with-param name="default" select="0"/>
            </xsl:call-template>
          </xsl:with-param>
        </xsl:call-template>
      </xsl:variable>

      <Item>
        <ItemID>
          <xsl:call-template name="getNumber">
            <xsl:with-param name="value" select="ItemID"/>
            <xsl:with-param name="default" select="0"/>
          </xsl:call-template>
        </ItemID>
        <ProductID>
          <xsl:value-of select="ProductID"/>
        </ProductID>
        <Code>
          <xsl:value-of select="Code"/>
        </Code>
        <SKU>
          <xsl:value-of select="Code"/>
        </SKU>
        <Name>
          <xsl:value-of select="Name"/>
        </Name>
        <Quantity>
          <xsl:call-template name="getNumber">
            <xsl:with-param name="value" select="Quantity"/>
            <xsl:with-param name="default" select="0"/>
          </xsl:call-template>
        </Quantity>
        <UnitPrice>
          <xsl:value-of select="$basePrice"/>
        </UnitPrice>
        <Image>
          <xsl:value-of select="Image"/>
        </Image>
        <Weight>
          <!-- Get the weight and convert if necessary -->
          <xsl:variable name="weight">
            <xsl:call-template name="getNumber">
              <xsl:with-param name="value" select="Weight"/>
              <xsl:with-param name="default" select="0"/>
            </xsl:call-template>
          </xsl:variable>
          <xsl:variable name="format">
            <xsl:value-of select="translate(@symbol, 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')"/>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$format = 'LBS'">
              <xsl:value-of select="$weight"/>
            </xsl:when>
            <xsl:when test="$format = 'KG'">
              <xsl:value-of select="$weight * 2.204"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$weight"/>
            </xsl:otherwise>
          </xsl:choose>
        </Weight>

        <!-- Item Attributes -->
        <Attributes>
          <xsl:apply-templates select="Attributes">
            <xsl:with-param name="basePrice" select="$basePrice"/>
          </xsl:apply-templates>
        </Attributes>
      </Item>
    </xsl:for-each>
  </xsl:template>

  <!-- Item Attributes -->
  <xsl:template match="Attributes">
    <xsl:param name="basePrice" select="0"/>
    
    <xsl:for-each select="Attribute">
      <Attribute>
        <AttributeID>
          <xsl:value-of select="AttributeID"/>
        </AttributeID>

        <Name>
          <xsl:value-of select="Name"/>
        </Name>

        <Value>
          <xsl:value-of select="Value"/>
        </Value>

        <Price>
          <xsl:choose>
            <xsl:when test="Modifier/@type = '$' or Modifier/@type = ''">
              <xsl:call-template name="getNumber">
                <xsl:with-param name="value" select="Modifier"/>
                <xsl:with-param name="default" select="0"/>
              </xsl:call-template>
            </xsl:when>
            <xsl:when test="Modifier/@type = '%'">
              <xsl:call-template name="calculateAttributePrice">
                <xsl:with-param name="itemBasePrice" select="$basePrice"/>
              </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
              <!-- a modifier type we don't understand, output 0 -->
              <xsl:value-of select="0"/>
            </xsl:otherwise>
          </xsl:choose>
        </Price>

        <!-- copy all debug nodes even though they don't appear in v2 -->
        <Debug>
          <xsl:copy-of select="Debug/*"/>
        </Debug>
      </Attribute>
    </xsl:for-each>
  </xsl:template>

  <!-- Calculates the dollar price of an attribute priced by percent -->
  <xsl:template name="calculateAttributePrice">
    <xsl:param name="itemBasePrice"></xsl:param>
    <xsl:value-of select="$itemBasePrice * (number(Modifier) div 100)"/>
  </xsl:template>

  <!-- Calculates the adjusted unit price of an item, taking into account percentage based attribute prices -->
  <xsl:template name="calculateItemBasePrice">
    <xsl:param name="unitPrice" select="0"/>
    
    <!-- subtract out all "absolute" priced oprations, those that are $ based -->
    <xsl:variable name="minusAbsolute">
      <xsl:value-of select="$unitPrice - sum(Attributes/Attribute/Modifier[@type='$' or @type = ''])"/>
    </xsl:variable>

    <xsl:variable name="percentRepresented">
      <xsl:value-of select="sum(Attributes/Attribute/Modifier[@type='%'])"/>
    </xsl:variable>

    <xsl:value-of select="$minusAbsolute div (1 + ($percentRepresented div 100))"/>
  </xsl:template>

  <!-- payment -->
  <xsl:template match="Payment">
    <Method>
      <xsl:value-of select="Method"/>
    </Method>
    <xsl:if test="string(CreditCard/Type) != ''">
      <CreditCard>
        <Type>
          <xsl:value-of select="CreditCard/Type"/>
        </Type>
        <Owner>
          <xsl:value-of select="CreditCard/Owner"/>
        </Owner>
        <Number>
          <xsl:value-of select="CreditCard/Number"/>
        </Number>
        <Expires>
          <xsl:value-of select="CreditCard/Expires"/>
        </Expires>
        <CCV>
          <xsl:value-of select="CCV"/>
        </CCV>
      </CreditCard>
    </xsl:if>
  </xsl:template>

  <!-- Method call parameters -->
  <xsl:template match="Parameters">
    <Parameters>
      <xsl:copy-of select="*"/>
    </Parameters>
  </xsl:template>

  <!-- Billing and Shipping Address -->
  <xsl:template match="ShipAddress|BillAddress">
    <FirstName>
      <xsl:value-of select="FirstName"/>
    </FirstName>

    <LastName>
      <xsl:value-of select="LastName"/>
    </LastName>
    
    <Company>
      <xsl:value-of select="Company"/>
    </Company>

    <Street1>
      <xsl:value-of select="Street1"/>
    </Street1>

    <Street2>
      <xsl:value-of select="Street2"/>
    </Street2>

    <Street3>
      <xsl:value-of select="Street3"/>
    </Street3>

    <City>
      <xsl:value-of select="City"/>
    </City>

    <State>
      <xsl:value-of select="State"/>
    </State>

    <PostalCode>
      <xsl:value-of select="PostalCode"/>
    </PostalCode>

    <Country>
      <xsl:value-of select="Country"/>
    </Country>

    <Residential>true</Residential>

    <!-- Phone may or may not be used from the Customer node-->
    <Phone>
      <xsl:choose>
        <xsl:when test="name() = 'ShipAddress'">
          <xsl:call-template name="ShippingPhone"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="../Customer/Phone"/>
        </xsl:otherwise>
      </xsl:choose>
    </Phone>

    <Fax>
      <xsl:value-of select="Fax"/>
    </Fax>

    <!-- Phone may or may not be used from the Customer node-->
    <Email>
      <xsl:choose>
        <xsl:when test="name() = 'ShipAddress'">
          <xsl:call-template name="ShippingEmail"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="../Customer/Email"/>
        </xsl:otherwise>
      </xsl:choose>
    </Email>
  </xsl:template>

  <xsl:template match="CustomerComment">
    <xsl:variable name="commentText" select="text()"/>
    <xsl:if test="$commentText != ''">
      <Note>
        <xsl:value-of select="$commentText"/>
      </Note>
    </xsl:if>
  </xsl:template>

  <!-- Returns the value to be used for shipping address phone number.  If the Billing and Shipping 
  information match at a glance, use the customer information. -->
  <xsl:template name="ShippingPhone">
    <xsl:variable name="billName" select="string(../BillAddress/Name)"/>
    <xsl:variable name="billStreet" select="string(../BillAddress/Street1)"/>
    <xsl:variable name="billPostal" select="string(../BillAddress/PostalCode)"/>

    <!-- if the ship and billing information seem to match, use the data from the customer node -->
    <xsl:choose>
      <xsl:when test="(string(Name) = $billName and string(Street1) = $billStreet and string(PostalCode) = $billPostal) or 
                       ($billName = '' and $billStreet = '' and billPostal = '')">
        <xsl:value-of select="../Customer/Phone"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:variable name="emptyString"/>
        <xsl:value-of select="$emptyString"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- Returns the value to be used for shipping address email.  If the Billing and Shipping 
  information match at a glance, use the customer information. -->
  <xsl:template name="ShippingEmail">
    <!-- if the ship and billing information seem to match, use the data from the customer node -->
    <xsl:variable name="billName" select="string(../BillAddress/Name)"/>
    <xsl:variable name="billStreet" select="string(../BillAddress/Street1)"/>
    <xsl:variable name="billPostal" select="string(../BillAddress/PostalCode)"/>
    <xsl:choose>
      <xsl:when test="(string(Name) = $billName and string(Street1) = $billStreet and string(PostalCode) = $billPostal) or 
                       ($billName = '' and $billStreet = '' and billPostal = '')">
        <xsl:value-of select="../Customer/Email"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:variable name="emptyString"/>
        <xsl:value-of select="$emptyString"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template match="Error">
    <Error>
      <Code>
        <xsl:value-of select="Code"/>
      </Code>
      <Description>
        <xsl:value-of select="Description"/>
      </Description>
    </Error>
  </xsl:template>

  <!-- Converts a value to a number if it is numeric, default otherwise -->
  <xsl:template name="getNumber">
    <xsl:param name="value"/>
    <xsl:param name="default"/>
    <xsl:choose>
      <xsl:when test="number($value) != number($value)">
        <xsl:value-of select="$default"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="number($value)"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- Formats a Date string so it valides -->
  <xsl:template name="FormatDate">
    <xsl:param name="dateString"/>

    <xsl:value-of select="sw:ToUtcDateTime($dateString)"/>
  </xsl:template>

  <!-- Template for outputting an error string -->
  <xsl:template name="outputError">
    <!-- error string -->
    <xsl:param name="errorString">Unknown</xsl:param>

    <!-- output a root document element or not, default to no -->
    <xsl:param name="outputRoot" select="false"></xsl:param>

    <xsl:choose>
      <xsl:when test="$outputRoot">
        <ShipWorks>
          <LegacyTransformError>
            <xsl:value-of select="$errorString"></xsl:value-of>
          </LegacyTransformError>
        </ShipWorks>
      </xsl:when>
      <xsl:otherwise>
        <LegacyTransformError>
          <xsl:value-of select="$errorString"></xsl:value-of>
        </LegacyTransformError>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
