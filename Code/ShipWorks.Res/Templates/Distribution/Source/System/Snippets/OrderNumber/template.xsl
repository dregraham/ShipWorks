<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

    <xsl:output method="html" encoding="utf-8" indent="yes" />

    <xsl:template name="OrderNumber">
        <xsl:param name="order" />
        
        <xsl:value-of select="$order/Number" />
        
        <xsl:if test="$order/IsManual = 'false'" >
            
            <xsl:variable name="store" select="/ShipWorks/Store[@ID = $order/@storeID]" />
                            
            <xsl:choose>
                
                <!-- Amazon -->
                <xsl:when test="$store/StoreType/Code = 'AMAZON' ">
                    (<xsl:value-of select="$order/Amazon/AmazonOrderID" />)
                </xsl:when>
                
                <!-- ChannelAdvisor  -->
                <xsl:when test="$store/StoreType/Code = 'CHANNELADVISOR' ">
                    (<xsl:value-of select="$order/ChannelAdvisor/OrderID" />)
                </xsl:when>
                
                <!-- eBay -->
                <xsl:when test="$store/StoreType/Code = 'EBAY' and $order/eBay/SellingManager/RecordNumber != '' ">
                    (<xsl:value-of select="$order/eBay/SellingManager/RecordNumber" />)
                </xsl:when>
                
                <!-- MarketplaceAdvisor -->
                <xsl:when test="$store/StoreType/Code = 'MARKETWORKS' ">
                    (<xsl:value-of select="$order/MarketplaceAdvisor/UserOrderNumber" />)
                </xsl:when>                
                
                <!-- PayPal -->
                <xsl:when test="$store/StoreType/Code = 'PAYPAL' ">
                    (<xsl:value-of select="$order/PayPal/PayPalTransactionID" />)
                </xsl:when>                   
                
                <!-- ProStores (Confirmation Number) -->
                <xsl:when test="$store/StoreType/Code = 'PROSTORES' ">
                    (<xsl:value-of select="$order/ProStores/Confirmation" />)
                </xsl:when>
                
                <!-- Yahoo  -->
                <xsl:when test="$store/StoreType/Code = 'YAHOO' ">
                    (<xsl:value-of select="$order/Yahoo/OrderID" />)
                </xsl:when>  
                    
            </xsl:choose>
            
        </xsl:if>
        
    </xsl:template>

</xsl:stylesheet>