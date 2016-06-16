UPDATE Template
	SET Xsl = '<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sw="http://www.interapptive.com/shipworks" extension-element-prefixes="sw">

  <!-- Imports -->
  <xsl:import href="System\Snippets" />

  <xsl:output method="html" encoding="utf-8" indent="yes" />

  <!-- Start of template -->
  <xsl:template match="/">
    <xsl:apply-templates />
  </xsl:template>
  <xsl:template match="ShipWorks">

    <!-- Width defined by the template PageSettings -->
    <xsl:variable name="pageWidth" select="concat(Template/Output/ContentWidth, '' in'')" />

    <html>

      <head>
        <title>Groupon Packing Slip</title>

        <style>
        </style>

      </head>

      <body style="">

        <xsl:variable name="order" select="//Order" />
        <xsl:variable name="shipment" select="$order/Shipment" />

        <div style="width:{$pageWidth}">
          <!--Header-->
          <table width="100%">
            <tr>
              <td width="57%" style="vertical-align:top; font-size:22">
                <div style="font-family:''Arial Black''">GROUPON</div>
                <div style="font-family:''Arial''">Goods</div>
              </td>
              <td style="vertical-align:top;">
                <table>
                  <tr>
                    <td width="8%">
                      <font color="white">.</font>
                    </td>
                    <td width="45%" style="font-family:''Arial''; font-size:10">
                      <b>Groupon Goods Returns</b>
                      <br />
                      3815 Logistics Way<br />
                      Antioch, TN 37013
                    </td>
                    <td style="font-family:''Arial''; font-size:10">
                      groupon.com/goods<br />
                      groupon.com/support<br />
                      facebook.com/GrouponGoods
                    </td>
                  </tr>
                </table>

                <table>
                  <tr>
                    <td>
                      <img src="data:image/png;base64,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"/>
                      <font color="white">_</font>
                    </td>
                    <td style="font-family:''Arial''">
                      <font>
                        <b>Free</b> Shipping | <b>Free</b> Returns
                      </font>
                      <br />
                      <font style="font-size:10">with $24.99 Purchase</font>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
            <tr>
              <td colspan="2" style="font-family:''Arial''; font-size:18; border-bottom:solid black 1pt ">
                Thanks for shopping with Groupon Goods!<br />
                We don''t believe in secrets, so feel free to tell all your friends about us.
              </td>
            </tr>
          </table>

          <!--Order Info-->
          <table width="100%">
            <tr>
              <td width="33%" style="vertical-align:top; font-family:''Arial''; font-size:11">
                <font style="font-family:''Arial'';font-weight:bold">Order Date</font>
                <font color="white">___</font>
                <xsl:value-of select="sw:ToShortDate(//Order/Date)" />
              </td>
              <td width="33%" style="vertical-align:top; font-family:''Arial''; font-size:11">
                <font style="font-family:''Arial'';font-weight:bold">Bill To</font>
                <br />
                <xsl:variable name="address" select="//Order/Address[@type=''bill'']" />
                <xsl:if test="$address/FirstName != '''' or $address/LastName != ''''">
                  <xsl:value-of select="$address/FirstName" />
                  <xsl:text/>
                  <xsl:value-of select="$address/LastName" />
                  <br />
                </xsl:if>

                <xsl:if test="$address/Company != ''''">
                  <xsl:value-of select="$address/Company" />
                  <br />
                </xsl:if>

                <xsl:value-of select="$address/Line1" />
                <br />

                <xsl:if test="$address/Line2 != ''''">
                  <xsl:value-of select="$address/Line2" />
                  <br />
                </xsl:if>

                <xsl:if test="$address/Line3 != ''''">
                  <xsl:value-of select="$address/Line3" />
                  <br />
                </xsl:if>

                <xsl:value-of select="$address/City" />,
                <xsl:value-of select="$address/StateCode" />
                <xsl:text/>
                <xsl:value-of select="$address/PostalCode" />


              </td>
              <td width="33%" style="vertical-align:top; font-family:''Arial''; font-size:11">
                <font style="font-family:''Arial'';font-weight:bold">Ship To</font>
                <br />
                <xsl:variable name="address" select="//Order/Address[@type=''ship'']" />
                <xsl:if test="$address/FirstName != '''' or $address/LastName != ''''">
                  <xsl:value-of select="$address/FirstName" />
                  <xsl:text/>
                  <xsl:value-of select="$address/LastName" />
                  <br />
                </xsl:if>

                <xsl:if test="$address/Company != ''''">
                  <xsl:value-of select="$address/Company" />
                  <br />
                </xsl:if>

                <xsl:value-of select="$address/Line1" />
                <br />

                <xsl:if test="$address/Line2 != ''''">
                  <xsl:value-of select="$address/Line2" />
                  <br />
                </xsl:if>

                <xsl:if test="$address/Line3 != ''''">
                  <xsl:value-of select="$address/Line3" />
                  <br />
                </xsl:if>

                <xsl:value-of select="$address/City" />,
                <xsl:value-of select="$address/StateCode" />
                <xsl:text/>
                <xsl:value-of select="$address/PostalCode" />

              </td>
            </tr>
            <tr>
              <td style="font-family:''Arial''; font-size:11">
                <font style="font-family:''Arial'';font-weight:bold">Order ID</font>
                <font color="white">___</font>
                <xsl:value-of select="//Order/Groupon/GrouponOrderID" />
              </td>
              <td colspan="2" style="font-family:''Arial''; font-size:11">
                <font style="font-family:''Arial'';font-weight:bold">Shipped Via</font>
                <font color="white">___</font>
                <xsl:value-of select="//Order/RequestedShipping" />
              </td>
            </tr>
          </table>

          <!--Items-->
          <table width="100%" style="border-collapse: collapse;font-family:''Arial'';">
            <tr>
              <td style="background:black; color:white; font-size:14">
                <b>Item Number</b>
              </td>
              <td style="background:black; color:white; font-size:14">
                <b>Item Description</b>
              </td>
              <td style="text-align:right;background:black; color:white; font-size:14">
                <b>Quantity</b>
              </td>
            </tr>
            <tr>
              <td style="font-size:14">
                <xsl:value-of select="//Item/SKU" />
              </td>
              <td style="font-size:14">
                <xsl:value-of select="//Item/Name" />
              </td>
              <td style="text-align:right; font-size:14">
                <xsl:value-of select="//Item/Quantity" />
              </td>
            </tr>
          </table>
          <br />
          <!--Gift Message-->
          <table style="font-family:''Arial''; font-size:10">
            <tr>
              <td>
                If you''re looking for a gift message, you''re in the right place:
              </td>
            </tr>
          </table>
          <hr />
          <!--Footer-->
          <table height="30%">
            <tr>
              <td colspan="3" style="font-family:''Arial''; font-size:10">
                If you have any questions about your new goods, please contact us at groupon.com/support.
                <br />
                And, if for any reason your new goods don''t blow your socks right through your shoes, you can return your purchase-unless stated otherwise in the offer''s Fine Print-by following these simple steps:
              </td>
            </tr>
            <tr>
              <td>
                <table height="100%" width="100%">
                  <tr>
                    <td style="vertical-align:top">
                      <img src="data:image/png;base64,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"/>
                    </td>
                    <td style="text-align:left; vertical-align:top; font-family:''Arial''; font-size:18">
                      1
                    </td>
                    <td style="text-align:left; vertical-align:top; font-family:''Arial''; font-size:10">
                      Go to your account <br />
                      (groupon.com/mygroupons)<br />
                      and click on the arrow next to<br />
                      "Track Order", then select "Return"<br />
                      to print a prepaid mailing label*.

                    </td>
                  </tr>
                </table>
              </td>
              <td>
                <table height="100%" width="100%">
                  <tr>
                    <td style="vertical-align:top">
                      <img src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAIAAACRXR/mAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAzHSURBVFhHvVj3V1PLFuYffT+86/MC6ck56VRBeiAQqigXURQLCooghCodEhREuhCKQJCehJAE8H0zc3IIebGy1tuZddacKXt/s9vsk7hv16ZQMDg+7mh6/aalpdXe1X0aCAgTlM7Pz4VeJGEs1rBIvw/rfzj6T/33amulal7F6dIysjzeE2GC0sXFhdCLJIzFGhbpWrCYJk5Pg3dr7ssAS2dKy8z2XoX1Z3Q9I1J8Hq+PwtKqKaz9g0MMikqKbcSf0fVgUdGfpmdy8ixK3qDRm/Xm5JGRsbOLP4ESSdeCxVSyvr5efbdGodZKVVxeQeH8/AKbvQ5dT1thZG63OzMrV83rlpddbJg+/5yuCwvEvGhre2tlZZW+4v3/bkTIJGIvHfni/Dx0fn7GXqIc/M/8HRQbVlSyiXyFHPYSHsQTmL67Hj3xFZ3IqR9QbFjslIxFJK9Q6HxtbePTzPyxx8dGGEWuiaSTgH9y6tPi0nIgGGQj4sqY60X6kRFFFiC3e6f33WD1vVq5SvPXf+Kz8wqaXr2ZX1g6OjpiC3AScTESPaZaWt/mFxXfjE/8O0FSUXWnr3/AvbPLFjDWQj8W/dy3fL6TkdGxqup/eINZquI1OiM6Ck4nU/HZ+ZZHj5/Ozs55PJ7wYt/c/OKzxqbc/CKyRqPlDUmcIUmu5rCruqZ2YGjYe3JF0zHpCqxznELoEgoEgv2DQ3kWq1zFqbTIliYI4I1oZt6YjL6S10sUarWaf/jwEdvY3Nys4bQSmULF6zkDlpH1HHkmYzsGlZwu31rUZu+MUlaU8i5hYUKE5T8NTH6crrhz12BOwWXHBFDugiSIUXB65M/M3Lyu3r6Z+UXHxMTHj9PLK65xx0R+oZWoStxInsm8AY30ZWqOM5rLq+5OTX08O4sdwgRW1FB3z7us3AIYCyZQw2TgSHRjBi8OrI1maO7fNxPTb2fNLSweek/eDY3aKu5AEzIlV1JW3jsw6A0EVtY3imylf0ukCo0W59EYkjSmJMBip1LrzXI1r9bqrdaS3r5+QTAlCub8ihGdzg8Z2dk6U3LYZEw3AixiBa0BGuL0xmGHA+vHxp15RVYtxOiMmNXoTOhojUaLtRhTYL+44jKlpMtUHG5M9SVDqjyYFbt0Rr0xKS/fMu50MgyM4hC6Xza3BwZHlJz+r3iJkgIKa56cDEqCMPiEOTW9wFrc2dvrD51NTH00p966kSCDe7EDELczGTkT8JmhuRvx0rTM24OjY5DhdL4vKi1NvpWhAjiifsJWPHZ4vSQ5NcM58WHn614wFIpb+rySlZ37r79uYk7AYTBrTcnEk6iGEhUqndFcYC3p7OldcrlmFxdLyisT5CpiHcqXYqIdI86DRswNFSo4Pl4qKyuvmp6Zc+/tD4yM2SqqUGJIVRoGTjiPMRmLIUup4m/cTLAW21yutTi3e7eru6+sohpiJEpN+OikQfkKje5B/SPE/OraRkdXt63yDuwoCfsywxTxpN5jMvImE/FuCDaawRMWLCkv7+ju2dja/ry8AoZwA+QXsoUAIh6C+MDKsooqe2f31rZb8K3t7a/dff0YRWZCEKEhKdjKKgaHR/3+wPDIWGX1PY3eKJiMHo6hEWGxDnVqLDBrCCbSMIgtKl6n0Rkqqu8NjzmOvb6JyemiklKpQqXgtEqNTqHmc/IK7d19SNoMzxWX95+ebmxuPXr8HNl8cmp6/+DIOfE+NzdfyXE4DZxAlC2iEZ+XDUFnMKtNJPooStKgSGxHBavktDkWy9DQyOHhMYxgKy2vqr6HK+HY4xVAUIojqQq/iGzm9fqOjz3jDmduXgHylhI+RIOANRw9LOkqmnAj48Q1cQbyGrGe5Ai13ghTYE2epbC3753H6zk6OhYE0+zAcmccut+rjuydXUji8TL4op4ntmM48IwNSGyQiqAhhibIhPUYRINrIlwQd3WPHwtirhLJWlFGDARCOzt7uOYSJYrM27nb7q/nF9/s9q6cgiKIgRMQfCSjkqQaiSOyEW2hgyiGbkzok/UIPaakW1k5TS0twdA5sszDhif/SZQW28o/L7s8Xr8AghKBdXLid62tdXT2lJZXovD9G5lZb5Jr+ASJotBqm5qeWV3fcryfqqt/jM9ARBC+cCJxRDUGi+gGT5gSLqXiUtIyamofOBzv1za2llxrTxpfKHkd7ABWYIhObkFhW3vn8vIKKgMCC6Z9+uwFagHkaKmSIwUC4Y4jJsGlpEgZOlN1TZ1jYurL5tan2fnq2jqkEuR6EpKiewnejVdhBFPwcSyTyFTQx4fJj4eHR0tLy03NLUlpGbh5VFohrzJZAAf7ZmblNDx5dnB0FOc59gwMDeGLKlGuZPEv8sUTrxqdGYkHU3lFxfaeXl8guOxaf1jfgGxJ1tNjsBQlxB1NwphC8FdW38WHUOj8YhrnqbmvpRURDCqehygVMUsyiAFlR26+ZWBgCAEn+Nbm5lZbuz0rv0Cu4XBEtoE9taYU3NDYKachWVZdPTe3cHBwtOxyVd75BwtwDah0BpLMcHVCQ0qNWmsotJWjBkE9uLu71/iyGTcVAUQ1RDmzRs6DggDZJz0zq72jc3Nzm+GJi0wNgbPQ7Nxcsa30RnyiXE3uljA4hg/gzACH7JdfVNLfP4ItvpOT5y8btQZDgkwuUWikUlX9oye41zC1uLx6v74BJ4FgGIhwEDVEMnsyAuhmoiwzO8fhfO/xCH8RUDxnRFussBHxnQYDw6OjMCvOByWJ4Oj5SAzCCsR2BnNhoXVomIDzek9q6x/hOkMGxuva+pf7dQ/15hRgYoCYYgCLcSNW5nQoLkbGHF5fOAYR9gTCGb6jriSIMAn4Ojo6b2Xexn4YhUAxIOBp7qbckcpxYyLC8y3WhcXPbMve/iFMdlMiT0TNL8QESRDMZNSHSIEKxA0NT9mWmBQD1sXFmb2z82H9oy9bW/jCGRl3lJRVmpLT4MKoJagPCY35tcGcZLWVbrvdoVCoprZWRzWBJioJsAAIutebki1WW3df3+7u/vHx8YumV909PX5/jH94YmrrW8ubVjWnS07PaGh8cXB4CBuNjI7nFFiQbIjjC8lamZSWXnO/bnZu3rW6vvFl2+Pzzs0vTH+aRZ5My8xG6AAKbA308NRbt3PsXT0EkMfzrn8w43a2VKl+/aY1GP5Wi6TYsNraOwymFCgcdbc+KaXpVQtiBKn5w9SU1VYWL5WnZ2Z3975zf909OPKAdVrG7RfNr79suvUGc21d/cLnFffXva6ePkuxLUEiy8zKHRoZ9/h8+0fH9u6etMws3LM4IUqStg57MBASpEbQd2EZTakaA6lh4FU4t95kanzRhEoodHa2u7fvOznd+LJ1v+6BOSVVrtZIlMrm1tadnX2ZQoNUCQ2VVtyZnVtAXQD3Pw0EEQqoIpPSM5ALlDzJW8Q79aa2jo7f0la7AItVqtRqnM54Kyu35e1bfD48bWyCiREKMBNmlRr+VWvbzs6BUq3l9TiJAZpGrkIKXVpxDY87cgssvIHkWCQ2+q1BeCKNtXW0B0Pkv9aoz5zYsN6220VYaMzB0UFekKvVqJngK2pOSNbIagoN39phdzgmoVetHlUN/STRGnClKDke6gRK3BbE900oxXAfYAG53AAr6i9gRrFhtb5th28hBTA0LLxZn6XWcJHDBgVYTuckrktY8Oos3YIOGSSA0GhFSdyjtV3QVhTFhvWmtQ3hjG2EL20iMrFReWRQhOVwfAjDurKSrmEr6ZOMEHXCC9+8fRsMEd+KvGxAsWHhQ6D+cYOS19IIp3wpLMY3qoMmwHJOylWXsGIvpjqDU0pkyoqquysrLgbol2CBfD5/R1dnXkERiiJN+A+FyGdYEmmXsODyFFbM9ejAO1F+oRZvftWy85X8gRMFiFFsWGJcIBd3dXWlpKUheZI4IqXppTDaIC8ZRQ6DBe2Siy9iDbU+6cOfUC4jrhtfvHK52F+s36XYsNgJLsF5PH397xD28VLIRQAKUsPgRJefYtpCQgIUUibAh4gbJeEbMEGhetn8ejv8F1dMJYn0XSOCoqy+5XY/fvKcXdsUzSWyS1gqnc4kKIlWL0lq3og4sJZWfHatMkZRbGPSj2Axikp0U1PTSampAIfLmGoFICK1xSOd0kBLUnJ6LEO10NnVLWwmYH6ERqSfwwofDuAEpmfnFwMDg1ZbOfFf+uGPmjbCiKgsDLiu8ixW+DWKcsaE7f1F+jmsMJ1BcUKX0u7ewbPGl6m3MmUqjUylZrAQZQCK2/2f2gdra+sMye9iAv06LPAVWEfK2N3dffq8UW8wtrxpGx1zKpSayjv3XGurFyg1KUWVvr9Ivw4rJhFhfr9/8sPUzMzczOz8yOjY/v4Bm2P0u4AIffv2XzkbNU1e16RAAAAAAElFTkSuQmCC"/>
                    </td>
                    <td style="text-align:left; vertical-align:top; font-family:''Arial''; font-size:18">
                      2
                    </td>
                    <td style="text-align:left; vertical-align:top; font-family:''Arial''; font-size:10">
                      Place the unused item in<br />
                      its original packaging with<br />
                      all parts, accessories, and<br />
                      farewell letters.

                    </td>
                  </tr>
                </table>
              </td>
              <td>
                <table height="100%" width="100%">
                  <tr>
                    <td style="vertical-align:top">
                      <img src="data:image/png;base64,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"/>
                    </td>
                    <td style="text-align:left; vertical-align:top; font-family:''Arial''; font-size:18">
                      3
                    </td>
                    <td style="text-align:left; vertical-align:top; font-family:''Arial''; font-size:10">
                      Attach the prepaid mailing<br />
                      label and ship your goods,<br />
                      postmarked within 14 days<br />
                      of receipt.

                    </td>
                  </tr>
                </table>
              </td>
            </tr>
            <tr>
              <td colspan="3" style="font-family:''Arial''; font-size:10">
                *For Canadian Orders and Gift Returns: Contact us at groupon.com/support to request a Return Authorization form and a prepaid mailing lArial.
              </td>
            </tr>
          </table>
        </div>

      </body>

    </html>

  </xsl:template>
</xsl:stylesheet>'
	WHERE Name = 'Groupon Invoice'