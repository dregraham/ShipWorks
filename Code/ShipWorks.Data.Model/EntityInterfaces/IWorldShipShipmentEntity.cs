///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 5.0
// Code is generated on: 
// Code is generated using templates: ShipWorks
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'WorldShipShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IWorldShipShipmentEntity
    {
        
        /// <summary> The ShipmentID property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The OrderNumber property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."OrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OrderNumber { get; }
        /// <summary> The FromCompanyOrName property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromCompanyOrName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FromCompanyOrName { get; }
        /// <summary> The FromAttention property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromAttention"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FromAttention { get; }
        /// <summary> The FromAddress1 property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromAddress1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FromAddress1 { get; }
        /// <summary> The FromAddress2 property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromAddress2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FromAddress2 { get; }
        /// <summary> The FromAddress3 property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromAddress3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FromAddress3 { get; }
        /// <summary> The FromCountryCode property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FromCountryCode { get; }
        /// <summary> The FromPostalCode property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FromPostalCode { get; }
        /// <summary> The FromCity property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromCity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FromCity { get; }
        /// <summary> The FromStateProvCode property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromStateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FromStateProvCode { get; }
        /// <summary> The FromTelephone property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromTelephone"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FromTelephone { get; }
        /// <summary> The FromEmail property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FromEmail { get; }
        /// <summary> The FromAccountNumber property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromAccountNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FromAccountNumber { get; }
        /// <summary> The ToCustomerID property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToCustomerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ToCustomerID { get; }
        /// <summary> The ToCompanyOrName property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToCompanyOrName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ToCompanyOrName { get; }
        /// <summary> The ToAttention property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToAttention"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ToAttention { get; }
        /// <summary> The ToAddress1 property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToAddress1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ToAddress1 { get; }
        /// <summary> The ToAddress2 property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToAddress2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ToAddress2 { get; }
        /// <summary> The ToAddress3 property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToAddress3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ToAddress3 { get; }
        /// <summary> The ToCountryCode property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ToCountryCode { get; }
        /// <summary> The ToPostalCode property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ToPostalCode { get; }
        /// <summary> The ToCity property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToCity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ToCity { get; }
        /// <summary> The ToStateProvCode property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToStateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ToStateProvCode { get; }
        /// <summary> The ToTelephone property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToTelephone"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ToTelephone { get; }
        /// <summary> The ToEmail property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ToEmail { get; }
        /// <summary> The ToAccountNumber property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToAccountNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ToAccountNumber { get; }
        /// <summary> The ToResidential property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToResidential"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ToResidential { get; }
        /// <summary> The ServiceType property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ServiceType"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 3<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ServiceType { get; }
        /// <summary> The BillTransportationTo property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."BillTransportationTo"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BillTransportationTo { get; }
        /// <summary> The SaturdayDelivery property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."SaturdayDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SaturdayDelivery { get; }
        /// <summary> The QvnOption property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."QvnOption"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String QvnOption { get; }
        /// <summary> The QvnFrom property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."QvnFrom"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String QvnFrom { get; }
        /// <summary> The QvnSubjectLine property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."QvnSubjectLine"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 18<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String QvnSubjectLine { get; }
        /// <summary> The QvnMemo property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."QvnMemo"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String QvnMemo { get; }
        /// <summary> The Qvn1ShipNotify property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn1ShipNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Qvn1ShipNotify { get; }
        /// <summary> The Qvn1DeliveryNotify property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn1DeliveryNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Qvn1DeliveryNotify { get; }
        /// <summary> The Qvn1ExceptionNotify property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn1ExceptionNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Qvn1ExceptionNotify { get; }
        /// <summary> The Qvn1ContactName property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn1ContactName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Qvn1ContactName { get; }
        /// <summary> The Qvn1Email property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn1Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Qvn1Email { get; }
        /// <summary> The Qvn2ShipNotify property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn2ShipNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Qvn2ShipNotify { get; }
        /// <summary> The Qvn2DeliveryNotify property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn2DeliveryNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Qvn2DeliveryNotify { get; }
        /// <summary> The Qvn2ExceptionNotify property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn2ExceptionNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Qvn2ExceptionNotify { get; }
        /// <summary> The Qvn2ContactName property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn2ContactName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Qvn2ContactName { get; }
        /// <summary> The Qvn2Email property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn2Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Qvn2Email { get; }
        /// <summary> The Qvn3ShipNotify property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn3ShipNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Qvn3ShipNotify { get; }
        /// <summary> The Qvn3DeliveryNotify property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn3DeliveryNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Qvn3DeliveryNotify { get; }
        /// <summary> The Qvn3ExceptionNotify property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn3ExceptionNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Qvn3ExceptionNotify { get; }
        /// <summary> The Qvn3ContactName property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn3ContactName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Qvn3ContactName { get; }
        /// <summary> The Qvn3Email property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn3Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Qvn3Email { get; }
        /// <summary> The CustomsDescriptionOfGoods property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."CustomsDescriptionOfGoods"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String CustomsDescriptionOfGoods { get; }
        /// <summary> The CustomsDocumentsOnly property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."CustomsDocumentsOnly"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String CustomsDocumentsOnly { get; }
        /// <summary> The ShipperNumber property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ShipperNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipperNumber { get; }
        /// <summary> The PackageCount property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."PackageCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 PackageCount { get; }
        /// <summary> The DeliveryConfirmation property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."DeliveryConfirmation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String DeliveryConfirmation { get; }
        /// <summary> The DeliveryConfirmationAdult property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."DeliveryConfirmationAdult"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String DeliveryConfirmationAdult { get; }
        /// <summary> The InvoiceTermsOfSale property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."InvoiceTermsOfSale"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 3<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String InvoiceTermsOfSale { get; }
        /// <summary> The InvoiceReasonForExport property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."InvoiceReasonForExport"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 2<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String InvoiceReasonForExport { get; }
        /// <summary> The InvoiceComments property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."InvoiceComments"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 200<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String InvoiceComments { get; }
        /// <summary> The InvoiceCurrencyCode property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."InvoiceCurrencyCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 3<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String InvoiceCurrencyCode { get; }
        /// <summary> The InvoiceChargesFreight property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."InvoiceChargesFreight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Decimal> InvoiceChargesFreight { get; }
        /// <summary> The InvoiceChargesInsurance property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."InvoiceChargesInsurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Decimal> InvoiceChargesInsurance { get; }
        /// <summary> The InvoiceChargesOther property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."InvoiceChargesOther"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Decimal> InvoiceChargesOther { get; }
        /// <summary> The ShipmentProcessedOnComputerID property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ShipmentProcessedOnComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ShipmentProcessedOnComputerID { get; }
        /// <summary> The UspsEndorsement property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."UspsEndorsement"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String UspsEndorsement { get; }
        /// <summary> The CarbonNeutral property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."CarbonNeutral"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String CarbonNeutral { get; }
        
        
        
        IEnumerable<IWorldShipGoodsEntity> Goods { get; }
        IEnumerable<IWorldShipPackageEntity> Packages { get; }
        IEnumerable<IWorldShipProcessedEntity> WorldShipProcessed { get; }

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IWorldShipShipmentEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IWorldShipShipmentEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'WorldShipShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial class WorldShipShipmentEntity : IWorldShipShipmentEntity
    {
        
        
        IEnumerable<IWorldShipGoodsEntity> IWorldShipShipmentEntity.Goods => Goods;
        IEnumerable<IWorldShipPackageEntity> IWorldShipShipmentEntity.Packages => Packages;
        IEnumerable<IWorldShipProcessedEntity> IWorldShipShipmentEntity.WorldShipProcessed => WorldShipProcessed;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IWorldShipShipmentEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IWorldShipShipmentEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IWorldShipShipmentEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyWorldShipShipmentEntity(this, objectMap);
        }

        
    }
}
