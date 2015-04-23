<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

  <!-- Imports -->
  <xsl:import href="System\Snippets" />

  <xsl:output method="html" encoding="utf-8" indent="yes" />

  <!-- Start of template -->
  <xsl:template match="/">
    <xsl:apply-templates />
  </xsl:template>
  <xsl:template match="ShipWorks">
    <xsl:variable name="order" select="Customer/Order[1]" />

    <html>
      <head>
        <title>
          Packing Slip
        </title>
        <style>
          body {
          margin: auto;
          width: 775px;
          font-family: Arial, Helvetica, sans-serif;
          font-size: 12pt;
          color: #000000;
          }

          a {
          margin: 0;
          padding: 0;
          border: 0;
          text-decoration:none;
          color: #000000;
          }

          #wrapper {
          width: 732px;
          border-right: 1px solid #bebdbd;
          border-left: 1px solid #bebdbd;
          border-bottom: 1px solid #bebdbd;
          margin: 0 21px;
          padding: 0;

          }
          table {
          width: 732px;
          margin: 0px 0px 0px 0px;
          }



          #table2, #table1 {
          width: 702px;
          margin-left: 15px;
          }

          #table3 {
          border-collapse:collapse;
          }

          #table3 td, th{
          text-align: center;

          }

          #greybg {
          background-color:black;
          font-weight: bold;
          height: 1px;
          vertical-align: middle;
          }

          .ty {
          margin: auto;
          width: 100%;
          text-align: center;
          font-size: 11pt;
          padding: 15px;
          }

          #bigbold {
          font-size: 16pt;
          }

          .9pt {
          font-size: 9pt;
          }

          .pages {
          font-size: 10pt;
          margin: auto;
          width: 733px;
          text-align: right;
          padding: 10px 0;
          border-bottom: 1px dashed #7f8181;
          }

          .spacer {
          width: 100%;
          height: 20px;

          }
        </style>
      </head>
      <body>
        <div>
          <img alt="Choxi packing slip" src="http://static.choxicdn.com/images/customer_service/slip_header_4x6.png" />
          <br />
          <div id="wrapper">
            <table id="table1" width="100%">
              <tr>
                <td style="width: 70%;">
                  <strong>
                    CHOXI ORDER NUMBER:
                    <xsl:value-of select="$order/Number" />
                  </strong>
                </td>
                <td style="width: 30%; text-align: right;">
                  Date:
                  <!-- Builtin ShipWorks function -->
                  <xsl:value-of select="sw:ToShortDate($order/Date)" />
                </td>
                <td> </td>
              </tr>
            </table>

            <table id="table2" width="100%">

              <tr>
                <td style="width:50%;">Bill to:</td>
                <td style="width:50%;">Ship to:</td>
              </tr>
              <tr>
                <td style="width:50%;">
                  <!-- Shared Snippet -->
                  <xsl:call-template name="InvoiceBillTo">
                    <xsl:with-param name="order" select="$order" />
                  </xsl:call-template>
                </td>
                <td style="width:50%;">
                  <!-- Shared Snippet -->
                  <xsl:call-template name="InvoiceShipTo">
                    <xsl:with-param name="order" select="$order" />
                  </xsl:call-template>
                </td>
              </tr>

              <tr>
                <td style="width:50%;"> </td>
                <td style="width:50%;"> </td>
              </tr>
            </table>

            <table id="table3" style="font-size: 12px; text-align: center;" width="100%">
              <tr >
                <th>SKU</th>
                <th>Product</th>
                <th>Details</th>
                <th>Quantity</th>
                <th>Unit Price</th>
                <th>Total Price</th>
              </tr>
              <tr id="greybg">
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
              </tr>
              <xsl:for-each select="$order/Item">
                <tr>
                  <td>
                    <!-- Shared Snippet -->
                    <xsl:call-template name="OrderItemCode">
                      <xsl:with-param name="item" select="." />
                    </xsl:call-template>
                  </td>
                  <td>
                    <xsl:value-of select="Name" />
                  </td>
                  <td>
                    <!-- Displays any item attribuets that may exists -->
                    <xsl:for-each select="Option">
                      <xsl:value-of select="Name" />:
                      <xsl:value-of select="Description" />
                      <br/>
                    </xsl:for-each>
                  </td>
                  <td>
                    <xsl:value-of select="Quantity" />
                  </td>
                  <td>
                    $<xsl:value-of select="format-number(UnitPrice, '#,##0.00')" />
                  </td>
                  <td>
                    $<xsl:value-of select="format-number(TotalPrice, '#,##0.00')" />
                  </td>
                </tr>


              </xsl:for-each>
            </table>
            <div class="ty">
              <p>
                Total Savings:
                $<xsl:value-of select="format-number(//Payment/Detail[Label='TotalSavings']/Value, '#,##0.00')"/>
              </p>
              <p>
                <span id="bigbold">Thank you for shopping with choxi.com!</span>
                <br />
                We want you to be satisfied with your shopping experience,
                <br />
                let us know if there is anything we need to make better.
              </p>
            </div>
            <div>
              <img alt="Get $10 to shop with us!Share, Give, Get!Earn $10 each time you refer a friend and they make a purchase. They get a $10 gift card too! For details, visit www.choxi.com/promotions/friend_program" src="http://content.choxi.com/images/customer_service/share.png" title="Get $10 to shop with us!Share, Give, Get!Earn $10 each time you refer a friend and they make a purchase. They get a $10 gift card too! For details, visit www.choxi.com/promotions/friend_program" />
            </div>
            <div class="ty">
              <p style="font-size: 9pt;">
                <center>
                  Please inspect your items within 24 hours of receiving them. For information about our return policy and returns please reach out to
                  <br />Choxi© Customer Care
                  <a href="mailto:customercare@choxi.com">customercare@choxi.com</a>
                </center>
              </p>
            </div>
          </div>
        </div>


      </body>

    </html>

  </xsl:template>
</xsl:stylesheet>