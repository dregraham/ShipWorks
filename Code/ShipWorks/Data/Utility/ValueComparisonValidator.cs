using System;
using System.Data;
using Interapptive.Shared.Utility;
using Microsoft.XmlDiffPatch;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;

namespace ShipWorks.Data.Utility
{
    /// <summary>
    /// Validator used to determine if two values of a column truly are semantically the same, to avoid sending values to the database for complex types
    /// </summary>
    [DependencyInjectionInfo(typeof(IEntity2), "Validator", ContextType = DependencyInjectionContextType.Singleton)]
    [Serializable]
    public class ValueComparisonValidator : ValidatorBase
    {
        /// <summary>
        /// Validate the value of the given entity
        /// </summary>
        public override bool ValidateFieldValue(IEntityCore involvedEntity, int fieldIndex, object value)
        {
            IEntityField2 field = ((EntityBase2) involvedEntity).Fields[fieldIndex];
            IFieldPersistenceInfo persistInfo = DataAccessAdapter.GetPersistenceInfo(field);

            // XML
            if (persistInfo.SourceColumnDbType == SqlDbType.Xml.ToString())
            {
                string newXml = (string) value;
                string oldXml = (string) involvedEntity.GetCurrentFieldValue(fieldIndex);

                return !XmlUtility.CompareXml(oldXml, newXml, XmlDiffOptions.IgnoreXmlDecl | XmlDiffOptions.IgnoreWhitespace);
            }

            // Binary
            if (persistInfo.SourceColumnDbType == SqlDbType.VarBinary.ToString())
            {
                byte[] newBytes = (byte[]) value;
                byte[] oldBytes = (byte[]) involvedEntity.GetCurrentFieldValue(fieldIndex);

                return !ByteUtility.AreEqual(oldBytes, newBytes);
            }

            return true;
        }
    }
}
