<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
    exclude-result-prefixes="msxsl" 
    extension-element-prefixes="sw"
    xmlns:sw="http://www.interapptive.com/shipworks/extensions" 
>
  <xsl:param name="moduleAction"/>
  
  <!-- Stylesheet for transforming OsCommerce's V2 Communications to conform to the ShipWorks V3 schema.  -->
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
            <xsl:when test="$moduleAction = 'getstore'">
              <xsl:choose>
                <xsl:when test="count(/ShipWorks/Store) = 1">
                  <xsl:apply-templates select="/ShipWorks/Store"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:call-template name="outputStore"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
            
            <!-- Test if this is a GetStatusCodes reponse -->
            <xsl:when test="$moduleAction = 'getstatuscodes'">
              <xsl:apply-templates select="/ShipWorks/StatusCodes"/>
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
        <xsl:value-of select="Name"/>
      </Name>
      <CompanyOrOwner>
        <xsl:value-of select="Owner"/>
      </CompanyOrOwner>
      <Email>
        <xsl:value-of select="Email"/>
      </Email>
      <State>
        <xsl:value-of select="State"/>
      </State>
      <Country>
        <xsl:value-of select="Country"/>
      </Country>
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

  <xsl:template match="Orders">
    <Orders>
      <xsl:apply-templates select="Order"/>
    </Orders>
  </xsl:template>

  <xsl:template match="Order">
    <Order>
      <!-- ClickCartPro's order number isn't numeric and is passed to shipworks via the untyped Debug node-->
      <OrderNumber>-1</OrderNumber>
      <OrderDate>
        <xsl:call-template name="FormatDate">
          <xsl:with-param name="dateString">
            <xsl:value-of select="OrderDate"/>
          </xsl:with-param>
        </xsl:call-template>
      </OrderDate>
      
      <LastModified>
        <xsl:call-template name="FormatDate">
          <xsl:with-param name="dateString">
            <xsl:value-of select="LastModified"/>
          </xsl:with-param>
        </xsl:call-template>
      </LastModified>

      <ShippingMethod>
        <xsl:value-of select="ShippingMethod"/>
      </ShippingMethod>

      <StatusCode>
        <xsl:value-of select="StatusCode"/>
      </StatusCode>

      <CustomerID>
        <xsl:value-of select="Customer/CustomerID"/>
      </CustomerID>

      <Notes>
        <xsl:apply-templates select="Customer/CustomerComment"/>
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
          <!-- Passing the order number to ShipWorks V3 via the Debug node -->
          <OrderID>
            <xsl:value-of select="OrderNumber"/>
          </OrderID>
          <xsl:copy-of select="Debug/*"/>
        </Debug>
    </Order>
  </xsl:template>

 

  <xsl:template match="Totals">
    <xsl:for-each select="Total">
      <xsl:choose>
        <xsl:when test="msxsl:string-compare(Class, 'ot_total', 'en-US', 'i') = 0 or msxsl:string-compare(Class, 'ot_subtotal', 'en-US', 'i') = 0"></xsl:when>
        <xsl:otherwise>
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
              <xsl:when test="number(Value) &lt; 0">
                <xsl:attribute name="impact">subtract</xsl:attribute>
              </xsl:when>
              <xsl:otherwise>
                <xsl:attribute name="impact">add</xsl:attribute>
              </xsl:otherwise>
            </xsl:choose>
            <xsl:call-template name="getNumberAbsolute">
              <xsl:with-param name="value" select="Value"/>
              <xsl:with-param name="default" select="0"/>
            </xsl:call-template>
          </Total>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="Items">
    <xsl:for-each select="Item">
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
          <xsl:call-template name="getNumber">
            <xsl:with-param name="value" select="UnitPrice"/>
            <xsl:with-param name="default" select="0"/>
          </xsl:call-template>
        </UnitPrice>
        <Image>
          <xsl:value-of select="Image"/>
        </Image>
        <ThumbnailImage>
          <xsl:value-of select="ThumbnailImage"/>
        </ThumbnailImage>
        <Weight>
          <xsl:call-template name="getNumber">
            <xsl:with-param name="value" select="Weight"/>
            <xsl:with-param name="default" select="0"/>
          </xsl:call-template>
        </Weight>
        
        <!-- Item Attributes -->
        <Attributes>
          <xsl:apply-templates select="Attributes"/>
        </Attributes>
      </Item>
    </xsl:for-each>
  </xsl:template>

  <!-- Item Attributes -->
  <xsl:template match="Attributes">
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
          <xsl:call-template name="getNumber">
            <xsl:with-param name="value" select="Price"/>
            <xsl:with-param name="default" select="0"/>
          </xsl:call-template>
        </Price>

        <!-- copy all debug nodes even though they don't appear in v2 -->
        <Debug>
          <xsl:copy-of select="Debug/*"/>
        </Debug>
      </Attribute>
    </xsl:for-each>
  </xsl:template>

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
    <FullName>
      <xsl:value-of select="Name"/>
    </FullName>

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

    <Website>
      <xsl:value-of select="Website"/>
    </Website>
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

  <!-- Converts a value to its absoluate value if it is numeric, default otherwise -->
  <xsl:template name="getNumberAbsolute">
    <xsl:param name="value"/>
    <xsl:param name="default"/>
    <xsl:choose>
      <xsl:when test="number($value) != number($value)">
        <xsl:value-of select="$default"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="format-number(number($value), '#########0.00;#########0.00')"/>
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
