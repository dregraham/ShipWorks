using System.IO;
using System.Reflection;
using System.Web;
using System.Data;
using SD.LLBLGen.Pro.GeneratorCore;
using SD.LLBLGen.Pro.DBDriverCore;
using System.Text;
using SD.LLBLGen.Pro.ApplicationCore;
using SD.LLBLGen.Pro.ApplicationCore.Entities;
using SD.LLBLGen.Pro.LptParser;
using System.ComponentModel;
using SD.LLBLGen.Pro.ApplicationCore.TypedViews;
using System.Collections.Generic;
using System.Xml;
using SD.LLBLGen.Pro.ApplicationCore.Templates;
using System.Collections;
using SD.LLBLGen.Pro.ApplicationCore.TypedLists;
using System;
using SD.LLBLGen.Pro.ApplicationCore.StoredProcedures;
using SD.LLBLGen.Pro.ApplicationCore.Tasks;


public class SD_PROJECTFILE_V4 : ITemplateClass {
	private StreamWriter __outputWriter;
	private IGenerator _executingGenerator;
	private Dictionary<string, TaskParameter> _parameters;
	private object _activeObject;

	public SD_PROJECTFILE_V4() {
		__outputWriter=null;_executingGenerator=null;_parameters=null;_activeObject=null;
	}


	private Project _currentProject;
	private Dictionary<Type, string> _typeShortcuts;
	private Dictionary<Type, string> _systemTypeShortcuts;
	private Dictionary<string, object> _typeShortcutNames;
	private Dictionary<string, string> _oppositeRelationPerRelation;
	private Dictionary<string, EntityRelation> _entityRelationPerUniqueKey;
	private Dictionary<string, object> _relationshipFullNamesInUse;
	private Dictionary<string, object> _spCallNamesUsed;
	private Dictionary<SPCallDefinition, string> _namePerSPCall;
	private string[] _netSystemNameFragments;
	
	// cache for v3 relationship infos created from the v2 relation pairs. If a key isn't present for a relation, the opposite is used
	// instead (so use _oppositeRelationPerRelation to get that opposite. If the opposite doesn't exist, ignore the relation.
	// For m:n relations, just 1 is used and the other is simply ignored. 
	private Dictionary<string, RelationshipInfo> _normalRelationshipInfoPerUniqueKey;
	private Dictionary<string, IndirectRelationshipInfo> _indirectRelationshipInfoPerUniqueKey;
	private Dictionary<string, List<string>> _systemSequences;


	// adds the type specified to the cache of type shortcuts.
	private void AddTypeToTypeShortcuts(Type toAdd)
	{
		if(_typeShortcuts.ContainsKey(toAdd))
		{
			return;
		}
		string shortcut = toAdd.Name;
		int uniqueMarker = 1;
		while(_typeShortcutNames.ContainsKey(shortcut))
		{
			shortcut = toAdd.Name + uniqueMarker;
			uniqueMarker++;
		}
		_typeShortcuts.Add(toAdd, shortcut);
		_typeShortcutNames[shortcut] = null;
	}
	
	/// <summary>
	/// Converts the discriminator value to XML elements.
	/// </summary>
	/// <param name="discriminatorValue">The discriminator value.</param>
	/// <param name="discriminatorValueTypeAsString">The discriminator value type as the string representation of the DiscriminatorValueType value
	/// which is the enum value converted as int.</param>
	/// <returns>
	/// the discriminatorvalue as string (xml convert)
	/// </returns>
	private string ConvertDiscriminatorValueToXmlElements(object discriminatorValue, out string discriminatorValueTypeAsString)
	{
		string toReturn = string.Empty;
		DiscriminatorValueType typeOfValue = DiscriminatorValueType.Unknown;
		if(discriminatorValue != null)
		{
			switch(Type.GetTypeCode(discriminatorValue.GetType()))
			{
				case TypeCode.Int64:
					typeOfValue = DiscriminatorValueType.Int64;
					toReturn = XmlConvert.ToString(Convert.ToInt64(discriminatorValue));
					break;
				case TypeCode.Int32:
					typeOfValue = DiscriminatorValueType.Int32;
					toReturn = XmlConvert.ToString(Convert.ToInt32(discriminatorValue));
					break;
				case TypeCode.Int16:
					typeOfValue = DiscriminatorValueType.Int16;
					toReturn = XmlConvert.ToString(Convert.ToInt16(discriminatorValue));
					break;
				case TypeCode.Byte:
					typeOfValue = DiscriminatorValueType.Byte;
					toReturn = XmlConvert.ToString(Convert.ToByte(discriminatorValue));
					break;
				case TypeCode.String:
					typeOfValue = DiscriminatorValueType.String;
					toReturn = (string)discriminatorValue;
					break;
				case TypeCode.Decimal:
					typeOfValue = DiscriminatorValueType.Decimal;
					toReturn = XmlConvert.ToString(Convert.ToDecimal(discriminatorValue));
					break;
				case TypeCode.Object:
					if(discriminatorValue.GetType()== typeof(Guid))
					{
						typeOfValue = DiscriminatorValueType.Guid;
						toReturn = XmlConvert.ToString((Guid)discriminatorValue);
					}
					break;
			}
		}
		discriminatorValueTypeAsString = XmlConvert.ToString((int)typeOfValue);
		return toReturn;
	}

	private ParameterDirection ConvertDirection(StoredProcedureParameterDirection originalDirection)
	{
		ParameterDirection toReturn = ParameterDirection.Input;
		switch(originalDirection)
		{
			case StoredProcedureParameterDirection.In:
				toReturn = ParameterDirection.Input;
				break;
			case StoredProcedureParameterDirection.InOut:
				toReturn = ParameterDirection.InputOutput;
				break;
			case StoredProcedureParameterDirection.Out:
				toReturn = ParameterDirection.Output;
				break;
			case StoredProcedureParameterDirection.ReturnValue:
				toReturn = ParameterDirection.ReturnValue;
				break;
		}
		return toReturn;
	}

	private string ConvertTypeDefinitionToXmlAttributes(IDBTypeDefinition typeDefinition)
	{
		int length;
		int precision;
		int scale;
		GetRealTypeSpecifications(typeDefinition.DBTypeAsNETType, typeDefinition.Length, typeDefinition.Precision, typeDefinition.Scale, true, out length, out precision, out scale);
		StringBuilder builder = new StringBuilder();
		builder.AppendFormat(" DbType=\"{0}\"", typeDefinition.DBType);
		builder.AppendFormat(" Length=\"{0}\"", length);
		builder.AppendFormat(" Precision=\"{0}\"", precision);
		builder.AppendFormat(" Scale=\"{0}\"", scale);
		if(typeDefinition.IsOfUserDefinedType)
		{
			builder.AppendFormat(" DbTypeAsNetType=\"{0}\"", GetFullTypeName(typeDefinition.DBTypeAsNETType));
		}
		if(!string.IsNullOrEmpty(typeDefinition.UserDefinedTypeName))
		{
			builder.AppendFormat(" UdtName=\"{0}\"", typeDefinition.UserDefinedTypeName);
			builder.AppendFormat(" UdtOwner=\"{0}\"", typeDefinition.UserDefinedTypeOwner);
			builder.AppendFormat(" UdtCatalog=\"{0}\"", typeDefinition.UserDefinedTypeCatalog);
		}
		return builder.ToString();
	}
	
	private string CreateUniqueKey(EntityRelation relation)
	{
		if(relation.RelationType==EntityRelationType.ManyToMany)
		{
			return relation.ToString() + relation.RelationsToWalk[0].ToString(true) + relation.RelationsToWalk[1].ToString(true) + relation.RelationType.ToString();
		}
		return relation.ToString(true);
	}
	
	private EntityRelation FindRelationToUse(EntityDefinition entity, EntityFieldDefinition fkField)
	{
		EntityRelationCollection relationships = entity.Relations.GetRelationsWithField(fkField, false);
		if(relationships.Count<=0)
		{
			return null;
		}
		// now find the relation which has the fk field indeed as the fk field. As an FK field point to 1 pk field, we can use the first relation we find 
		EntityRelation toReturn = null;
		foreach(EntityRelation toExamine in relationships)
		{
			if(toExamine.RelationIsHidden || toExamine.IsUsedForHierarchy || !_normalRelationshipInfoPerUniqueKey.ContainsKey(CreateUniqueKey(toExamine)))
			{
				continue;
			}
			foreach(EntityFieldRelation fieldRelation in toExamine.FieldRelations)
			{
				if(toExamine.StartEntityIsPkSide)
				{
					if(fieldRelation.RelationEndField==fkField)
					{
						toReturn = toExamine;
						break;
					}
				}
				else
				{
					if(fieldRelation.RelationStartField==fkField)
					{
						toReturn = toExamine;
						break;
					}
				}
			}
			if(toReturn!=null)
			{
				break;
			}
		}
		if((toReturn!=null) && (!toReturn.StartEntityIsPkSide && toReturn.UtilizingPropertyIsHidden))
		{
			// we'll ignore relations which have the FK side's navigator (UtilizingPropertyName) marked as hidden, as the FK field can't refer the pk field
			// that way and should be converted to a normal field instead.
			toReturn = null;
		}
		return toReturn;
	}

	// Determines the UDT types in all fields in the metadata of this project.
	private List<Type> FindUDTsInMetaData()
	{
		List<Type> toReturn = new List<Type>();
		foreach(DBCatalog catalog in _currentProject.Catalogs)
		{
			foreach(DictionaryEntry schemaEntry in catalog.DBSchemas)
			{
				DBSchema schema = (DBSchema)schemaEntry.Value;
				foreach(DictionaryEntry tableEntry in schema.Tables)
				{
					DBTable table = (DBTable)tableEntry.Value;
					foreach(DictionaryEntry fieldEntry in table.Fields)
					{
						DBField field = (DBField)fieldEntry.Value;
						if(field.TypeDefinition.IsOfUserDefinedType && !_typeShortcuts.ContainsKey(field.TypeDefinition.DBTypeAsNETType))
						{
							toReturn.Add(field.TypeDefinition.DBTypeAsNETType);
						}
					}
				}
				foreach(DictionaryEntry viewEntry in schema.Views)
				{
					DBView view = (DBView)viewEntry.Value;
					foreach(DictionaryEntry fieldEntry in view.Fields)
					{
						DBField field = (DBField)fieldEntry.Value;
						if(field.TypeDefinition.IsOfUserDefinedType && !_typeShortcuts.ContainsKey(field.TypeDefinition.DBTypeAsNETType))
						{
							toReturn.Add(field.TypeDefinition.DBTypeAsNETType);
						}
					}
				}
			}
		}
		return toReturn;
	}

	
	// strips off schema name if it's present at the beginning of the sequence name.
	private string GetCleanSequenceName(string sequenceName, string schemaName)
	{
		string schemaPlusDot = schemaName + ".";
		if(sequenceName.StartsWith(schemaPlusDot))
		{
			return sequenceName.Substring(schemaPlusDot.Length);
		}
		return sequenceName;
	}
	
	
	// gets the full type name for the type specified, to be used in the xml output.
	private string GetFullTypeName(Type t)
	{
		if(t==null)
		{
			return string.Empty;
		}
		if(IsNetSystemType(t))
		{
			return t.FullName;
		}
		return (t.Assembly.GetName().GetPublicKeyToken().Length <= 0)? string.Format("{0}, {1}", t.FullName, t.Assembly.GetName().Name)
																	 : t.AssemblyQualifiedName;
	}
	
	// to be able to re-pair relations, we've to determine the primary and secondary relation for the v3 relationship info.
	private void GetPrimaryAndSecondaryRelations(EntityRelation relation, string uniqueKey, out EntityRelation primary, out EntityRelation secondary,
												 out string primaryUniqueKey, out string secondaryUniqueKey)
	{
		string oppositeUniqueKey;
		_oppositeRelationPerRelation.TryGetValue(uniqueKey, out oppositeUniqueKey);
		EntityRelation oppositeRelation = null;
		if(!string.IsNullOrEmpty(oppositeUniqueKey))
		{
			_entityRelationPerUniqueKey.TryGetValue(oppositeUniqueKey, out oppositeRelation);
		}
		primaryUniqueKey = uniqueKey;
		secondaryUniqueKey = oppositeUniqueKey;
		primary = relation;
		secondary = oppositeRelation;
		// if there's a secondary and the secondary is the fk -> pk variant, swap
		if((secondary!=null) && (secondary.RelationType!=EntityRelationType.ManyToMany) && !secondary.StartEntityIsPkSide)
		{
			string tmpUniqueKey = primaryUniqueKey;
			primaryUniqueKey = secondaryUniqueKey;
			secondaryUniqueKey = tmpUniqueKey;
			EntityRelation tmpPrimary = primary;
			primary = secondary;
			secondary = tmpPrimary;
		}
	}
	
	// get the real type specification values for length, precision, scale based on the .net type specified.
	// length is only used for string/arrays, precision and scale are only used for decimals.
	// precision is always emitted if the values are for meta-data
	private void GetRealTypeSpecifications(Type netType, int length, int precision, int scale, bool forMetaData, 
										   out int realLength, out int realPrecision, out int realScale)
	{
		realLength = length;
		realPrecision = precision;
		realScale = scale;
		
		// length is only set in string and array typed fields
		// precision and scale is only set in decimal typed fields.
		if(netType==null)
		{
			realLength = 0;
			realPrecision = 0;
			realScale = 0;
		}
		else
		{
			if((netType!=typeof(string)) && !typeof(Array).IsAssignableFrom(netType))
			{
				realLength=0;
				if(netType!=typeof(decimal))
				{
					if(!forMetaData)
					{
						realPrecision=0;
						realScale=0;
					}
				}
			}
			else
			{
				// string or array, always clear precision/scale
				realPrecision = 0;
				realScale = 0;
			}
		}
	}
	
	// method which produces the real field definition in case the specified field is a PK field which is in a subtype. In v3 subtypes 
	// don't have pk fields, they're always inherited from the supertype
	internal static EntityFieldDefinition GetRealFieldDefinition(EntityFieldDefinition field)
	{
		EntityFieldDefinition toReturn = field;
		if(field.IsPrimaryKeyField && field.ContainingEntity.IsInHierarchy && !field.ContainingEntity.IsHierarchyRoot)
		{
			// first get the index of the relatedfield in its own entity. Has to be there.
			int fieldIndex = field.ContainingEntity.PrimaryKeyFields.IndexOf(field);
			EntityDefinition root = field.ContainingEntity.GetRootOfHierarchy();
			toReturn = (EntityFieldDefinition)root.PrimaryKeyFields[fieldIndex];
		}
		return toReturn;
	}

	// Gets the related field of the FK field specified in the relationship specified.
	private EntityFieldDefinition GetRelatedFieldOfFkField(EntityRelation relation, EntityFieldDefinition fkField)
	{
		EntityFieldDefinition relatedField = null;
		foreach(EntityFieldRelation fieldRelation in relation.FieldRelations)
		{
			EntityFieldDefinition toCompareWith = fieldRelation.RelationStartField;
			EntityFieldDefinition fieldToConsider = fieldRelation.RelationEndField;
			if(relation.StartEntityIsPkSide)
			{
				toCompareWith = fieldRelation.RelationEndField;
				fieldToConsider = fieldRelation.RelationStartField;
			}
			if(toCompareWith == fkField)
			{
				relatedField = fieldToConsider;
				break;
			}
		}
		return relatedField;
	}
	
	// gets the relationship info for the relation specified. This can be indirectrelationship info if the relation is a m:n relationship.
	// returns null if not found
	private RelationshipInfo GetRelationshipInfo(EntityRelation relation)
	{
		string uniqueKey = CreateUniqueKey(relation);
		RelationshipInfo toReturn;
		if(relation.RelationType==EntityRelationType.ManyToMany)
		{
			IndirectRelationshipInfo mnInfo=null;
			if(!_indirectRelationshipInfoPerUniqueKey.TryGetValue(uniqueKey, out mnInfo))
			{
				// not found, try opposite relation.
				string oppositeUniqueKey;
				if(_oppositeRelationPerRelation.TryGetValue(uniqueKey, out oppositeUniqueKey))
				{
					_indirectRelationshipInfoPerUniqueKey.TryGetValue(oppositeUniqueKey, out mnInfo);
				}
			}
			toReturn = mnInfo;
		}
		else
		{
			if(!_normalRelationshipInfoPerUniqueKey.TryGetValue(uniqueKey, out toReturn))
			{
				// not found, try opposite relation.
				string oppositeUniqueKey;
				if(_oppositeRelationPerRelation.TryGetValue(uniqueKey, out oppositeUniqueKey))
				{
					_normalRelationshipInfoPerUniqueKey.TryGetValue(oppositeUniqueKey, out toReturn);
				}
			}
		}
		
		return toReturn;
	}

	// gets the relation's full name. It first determines the v3 relationship info for this relation and then returns the full name
	// stored in that relationship info, or string.empty if not found.
	private string GetRelationshipName(EntityRelation relation)
	{
		RelationshipInfo info = GetRelationshipInfo(relation);
		if(info==null)
		{
			return string.Empty;
		}
		return info.FullName;
	}
	
	// gets the stored type shortcut string for the type specified, or 'string' if the type isn't known.
	private string GetTypeShortcut(Type t)
	{
		string toReturn;
		if(!_typeShortcuts.TryGetValue(t, out toReturn))
		{
			toReturn = "object";
		}
		return toReturn;
	}
	
	private void InitSystemSequences()
	{
		List<string> sequences = new List<string>();
		sequences.Add("@@IDENTITY");
		_systemSequences.Add("4F77DEE6-918E-4a53-8B3B-A076B00A3E0D", sequences);	// MS Access
		_systemSequences.Add("3FABDE1A-21DF-4fcb-96FD-BBFA8F18B1EA", sequences);	// Sybase ASA
		_systemSequences.Add("A3076322-977C-4e28-BFF4-F25ED096D1DB", sequences);	// Sybase ASE
		sequences = new List<string>();
		sequences.Add("IDENTITY_VAL_LOCAL()");
		_systemSequences.Add("BB438EBA-A0B5-4236-A2B8-64D828A138AF", sequences);	// DB2 
		sequences = new List<string>();
		sequences.Add("LAST_INSERT_ID()");
		_systemSequences.Add("758A392F-06F3-498b-AED9-D85A4C795BDA", sequences);	// MySQL
		sequences = new List<string>();
		sequences.Add("SCOPE_IDENTITY()");
		sequences.Add("@@IDENTITY");
		_systemSequences.Add("2D18D138-1DD2-467E-86CC-4838250611AE", sequences);	// SQL Server
	}
	
	/// <summary>
	/// Determines whether the type specified is a system type of .NET. System types are types in mscorlib, assemblies which start with 'Microsoft.', 'System.'
	/// or the System assembly itself. 
	/// </summary>
	/// <param name="type">The type.</param>
	private bool IsNetSystemType(Type type)
	{
		if(type==null)
		{
			return false;
		}
		AssemblyName nameToCheck = type.Assembly.GetName();
		foreach(string s in _netSystemNameFragments)
		{
			if((s.EndsWith(".") && nameToCheck.Name.StartsWith(s)) || (nameToCheck.Name == s))
			{
				return true;
			}
		}
		return false;
	}
	
	
	// makes sure that the passed in name is a unique relationship name to emit into the output
	private string MakeUniqueRelationshipName(string relationshipName)
	{
		string toReturn = relationshipName;
		if(_relationshipFullNamesInUse.ContainsKey(relationshipName))
		{
			string tmpName = relationshipName;
			toReturn = tmpName;
			int uniqueMaker = 1;
			while(_relationshipFullNamesInUse.ContainsKey(toReturn))
			{
				toReturn = String.Format("{0}_{1}", tmpName, uniqueMaker);
				uniqueMaker++;
			}
		}
		_relationshipFullNamesInUse.Add(toReturn, null);
		return toReturn;
	}

	// preprocesses the project meta-data so relation lookup stores and type shortcuts are filled.
	private void PreProcess()
	{
		// determine the UDT types in the fields of tables/views in the meta-data. For every UDT, a type shortcut will be created.
		List<Type> udtTypesFoundInMetaData = FindUDTsInMetaData();
		foreach(Type t in udtTypesFoundInMetaData)
		{
			AddTypeToTypeShortcuts(t);
		}
		foreach(EntityDefinition entity in _currentProject.Entities)
		{
			foreach(EntityRelation relation in entity.Relations)
			{
				// skip hierarchy relations and hidden relations as we don't need them for the output.
				if(relation.IsUsedForHierarchy || relation.RelationIsHidden)
				{
					continue;
				}
				string uniqueKey = CreateUniqueKey(relation);
				if(_entityRelationPerUniqueKey.ContainsKey(uniqueKey))
				{
					// already processed
					continue;
				}
				_entityRelationPerUniqueKey[uniqueKey] = relation;
				// find opposite relation and if any, store its info.
				EntityRelation mirroredRelation = relation.CreateMirroredCopy();
				int indexOpposite = mirroredRelation.RelationStartPoint.Relations.IndexOf(mirroredRelation);
				if(indexOpposite>=0)
				{
					EntityRelation oppositeRelation = mirroredRelation.RelationStartPoint.Relations[indexOpposite];
					if(oppositeRelation.RelationIsHidden || oppositeRelation.IsUsedForHierarchy)
					{
						_oppositeRelationPerRelation[uniqueKey] = null;
					}
					else
					{
						string oppositeUniqueKey = CreateUniqueKey(oppositeRelation);
						_oppositeRelationPerRelation[uniqueKey] = oppositeUniqueKey;
						_oppositeRelationPerRelation[oppositeUniqueKey] = uniqueKey;
						_entityRelationPerUniqueKey[oppositeUniqueKey] = oppositeRelation;
					}
				}
				else
				{
					_oppositeRelationPerRelation[uniqueKey] = null;
				}
			}
			foreach(EntityFieldDefinition field in entity.Fields)
			{
				AddTypeToTypeShortcuts(field.DotNetType);
			}
		}
		foreach(TypedViewDefinition typedView in _currentProject.TypedViews)
		{
			foreach(TypedViewFieldDefinition field in typedView.Fields)
			{
				AddTypeToTypeShortcuts(field.DotNetType);
			}
		}

		// now re-pair each two opposite relations into 1 relationshipinfo as v3 uses 1 edge which used on both sides. 
		// if a relation doesn't have an opposite relation, just 1 side is used for the info. 
		// we strive for using the fk -> pk relation as the base, and use the opposite relation as source for the navigator.
		// First build the normal relationship cache. 
		Dictionary<string, object> keysAlreadyProcessed = new Dictionary<string, object>();
		foreach(KeyValuePair<string, EntityRelation> kvp in _entityRelationPerUniqueKey)
		{
			if(keysAlreadyProcessed.ContainsKey(kvp.Key) || kvp.Value.RelationType==EntityRelationType.ManyToMany)
			{
				continue;
			}
			string primaryUniqueKey;
			string secondaryUniqueKey;
			EntityRelation primary;
			EntityRelation secondary;
			GetPrimaryAndSecondaryRelations(kvp.Value, kvp.Key, out primary, out secondary, out primaryUniqueKey, out secondaryUniqueKey);

			// produce full name for this pair
			string startEntity = primary.RelationStartName;
			string endEntity = primary.RelationEndName;
			string startNavigator = primary.UtilizingPropertyIsHidden?string.Empty:primary.UtilizingPropertyName;
			string endNavigator = ((secondary==null)||(secondary.UtilizingPropertyIsHidden))?string.Empty:secondary.UtilizingPropertyName;
			string fullName = string.Format("{0}.{1}-{2}.{3}", startEntity, startNavigator, endEntity, endNavigator);
			fullName = MakeUniqueRelationshipName(fullName);
			RelationshipInfo info = new RelationshipInfo(fullName, startEntity, endEntity, startNavigator, endNavigator, primary.RelationType, kvp.Value,
													primary.StartEntityIsPkSide, primary.IsCustomRelation);
			_normalRelationshipInfoPerUniqueKey[primaryUniqueKey] = info;
			keysAlreadyProcessed[primaryUniqueKey] = null;
			if(!string.IsNullOrEmpty(secondaryUniqueKey))
			{
				keysAlreadyProcessed[secondaryUniqueKey] = null;
			}
		}
		
		// then the M:N relations
		keysAlreadyProcessed = new Dictionary<string, object>();
		foreach(KeyValuePair<string, EntityRelation> kvp in _entityRelationPerUniqueKey)
		{
			if(keysAlreadyProcessed.ContainsKey(kvp.Key) || kvp.Value.RelationType!=EntityRelationType.ManyToMany)
			{
				continue;
			}
			string primaryUniqueKey;
			string secondaryUniqueKey;
			EntityRelation primary;
			EntityRelation secondary;
			GetPrimaryAndSecondaryRelations(kvp.Value, kvp.Key, out primary, out secondary, out primaryUniqueKey, out secondaryUniqueKey);

			// produce element names
			string startEntity = primary.RelationStartName;
			string endEntity = primary.RelationEndName;
			string startNavigator = primary.UtilizingPropertyIsHidden?string.Empty:primary.UtilizingPropertyName;
			string endNavigator = ((secondary==null)||(secondary.UtilizingPropertyIsHidden))?string.Empty:secondary.UtilizingPropertyName;
			string firstRelationshipName = GetRelationshipName(primary.RelationsToWalk[0]);
			string secondRelationshipName = GetRelationshipName(primary.RelationsToWalk[1]);
			if(string.IsNullOrEmpty(firstRelationshipName) || string.IsNullOrEmpty(secondRelationshipName))
			{
				continue;
			}
			IndirectRelationshipInfo info = new IndirectRelationshipInfo(startEntity, endEntity, startNavigator, endNavigator, 
																		firstRelationshipName, secondRelationshipName);
			_indirectRelationshipInfoPerUniqueKey[primaryUniqueKey] = info;
			keysAlreadyProcessed[primaryUniqueKey] = null;
			if(!string.IsNullOrEmpty(secondaryUniqueKey))
			{
				keysAlreadyProcessed[secondaryUniqueKey] = null;
			}
		}
	}
	
	// produces the ready-to-emit custom properties XML for the custom properties container specified, or string.empty if no
	// custom properties are present
	private string ProduceCustomPropertiesXml(Hashtable toEmit, bool wrapInOutputSettingsElement)
	{
		if((toEmit==null) || (toEmit.Count<=0))
		{
			return string.Empty;
		}
		StringBuilder builder = new StringBuilder();
		if(wrapInOutputSettingsElement)
		{
			builder.Append("<OutputSettingValues>");
		}
		builder.Append("<CustomProperties>");
		foreach(DictionaryEntry entry in toEmit)
		{
			string keyAsString = (string)entry.Key;
			if(keyAsString.StartsWith("MS_") && (keyAsString!="MS_Description"))
			{
				continue;
			}
			// use SecurityElement.Escape to replace <, >, ", & and '
			string valueToEmit =  System.Security.SecurityElement.Escape(((string)entry.Value).Replace("\n", string.Empty));
			builder.AppendFormat("<CustomProperty Name=\"{0}\" Value=\"{1}\" />", keyAsString, valueToEmit);
		}
		builder.Append("</CustomProperties>");
		if(wrapInOutputSettingsElement)
		{
			builder.Append("</OutputSettingValues>");
		}
		return builder.ToString();
	}
	
	// Produces the full name of the related field of the field specified. The field which name to return is related over the relation specified
	private string ProduceRelatedFieldName(EntityRelation relation, EntityFieldDefinition fkField)
	{
		EntityFieldDefinition relatedField = GetRelatedFieldOfFkField(relation, fkField);
		string toReturn = string.Empty;
		if(relatedField!=null)
		{
			// if the related field is in a subtype, the PK of that subtype is removed in v3, it's always inherited from the supertype.
			// we therefore have to obtain the real related field from the hierarchy root if the entity containing the related field is 
			// in an inheritance hierarchy.
			relatedField = GetRealFieldDefinition(relatedField);
			toReturn = string.Format(":{0}:{1}", relatedField.ContainingEntityName, relatedField.FieldName);
		}
		return toReturn;
	}
	
	
	// Produces for the spcall specified a unique name. This is required because retrieval and action procs are now in the same bucket in v3.
	private string ProduceUniqueSPCallName(SPCallDefinition spCall)
	{
		string toReturn;
		if(_namePerSPCall.TryGetValue(spCall, out toReturn))
		{
			return toReturn;
		}
		toReturn = spCall.Name;
		while(_spCallNamesUsed.ContainsKey(toReturn))
		{
			toReturn += "_";
		}
		_spCallNamesUsed.Add(toReturn, null);
		_namePerSPCall.Add(spCall, toReturn);
		return toReturn;
	}

	// info class for storing normal relationship information as needed for v3.
	internal class RelationshipInfo
	{
		public RelationshipInfo(string fullName, string startEntity, string endEntity, string startNavigator, string endNavigator,
									  EntityRelationType relationshipType, EntityRelation relation, bool startIsPkSide, bool modelOnly)
		{
			this.FullName = fullName;
			this.StartEntity = startEntity;
			this.EndEntity = endEntity;
			this.StartNavigator = startNavigator;
			this.EndNavigator = endNavigator;
			this.RelationshipType = relationshipType;
			this.FkFieldPkFieldPairs = new List<Pair<string, string>>();
			this.StartIsPkSide = startIsPkSide;
			this.ModelOnly = modelOnly;
			this.StartIsOptional = false;
			this.EndIsOptional = false;
			
			if(relation!=null)
			{
				foreach(EntityFieldRelation fieldRelation in relation.FieldRelations)
				{
					// in v3, subtypes inherit the pk from their supertype. So instead of emiting the subtype's pk field, we've to obtain the hierarchy root's
					// pk field which is mapped onto the same target.
					EntityFieldDefinition pkField;
					EntityFieldDefinition fkField;
					EntityDefinition pkEntity;
					
					// there's a subtle issue in v2 where a relationship with self doesn't supply enough information: StartEntityIsPkSide is always true in 
					// the case of a relationship with self. This means that in the case of a m:1 relationship with self, the start entity is the fk side 
					// but it's also the pk side. This has caused some problems in v2: it has to check whether the fk fields are really the fk fields. This is 
					// easy: it has to check whether there's at least one field in the pk fields reported by the relationship which isn't a pk field, and if
					// that's the case, it has to swap the fields. 
					bool startEntityIsPkSide = relation.StartEntityIsPkSide;
					if(startEntityIsPkSide && (relation.RelationStartPoint == relation.RelationEndPoint))
					{
						// relation with self. Check whether the reported pk fields indeed are all pk fields. If not, negate startEntityIsPkSide
						foreach(EntityFieldDefinition field in relation.PkFields)
						{
							if(!field.IsPrimaryKeyField)
							{
								// issue in v2, see above. Negate startentity flag for next block
								startEntityIsPkSide = false;
								break;
							}
						}
					}				
					if(startEntityIsPkSide)
					{
						pkField = fieldRelation.RelationStartField;
						fkField = fieldRelation.RelationEndField;
						pkEntity = relation.RelationStartPoint;
					}
					else
					{
						pkField = fieldRelation.RelationEndField;
						fkField = fieldRelation.RelationStartField;
						pkEntity = relation.RelationEndPoint;
					}
					if(startIsPkSide)
					{
						this.StartIsOptional |= fkField.IsNullable;
					}
					else
					{
						this.EndIsOptional |= fkField.IsNullable;
					}
					if(pkEntity.IsSubType && (pkEntity.HierarchyType==InheritanceHierarchyType.TargetPerEntity))
					{
						pkField = GetRealFieldDefinition(pkField);
					}
					this.FkFieldPkFieldPairs.Add(new Pair<string, string>(fkField.FieldName, pkField.FieldName));
				}
			}
		}
	
		public bool StartIsPkSide;		// used with 1:1 relationships
		public string FullName;
		public string StartEntity;
		public string EndEntity;
		public string StartNavigator;
		public string EndNavigator;
		public bool ModelOnly;
		public EntityRelationType RelationshipType;
		public bool StartIsOptional;
		public bool EndIsOptional;
		public List<Pair<string, string>> FkFieldPkFieldPairs;		// Value1 is fk field name, Value2 is pk field name
	}
	
	
	// simple pair class 
	internal class Pair<T, U>
	{
		private T _value1;
		private U _value2;
		
		internal Pair(T value1, U value2)
		{
			_value1 = value1;
			_value2 = value2;
		}
	
		public T Value1
		{
			get { return _value1;}
		}
		
		public U Value2
		{
			get { return _value2;}
		}
	}
	
	
	// info class for storing indirect relationship information as needed for v3.
	internal class IndirectRelationshipInfo : RelationshipInfo
	{
		public IndirectRelationshipInfo(string startEntity, string endEntity, string startNavigator, string endNavigator,
										string firstRelationshipName, string secondRelationshipName)
					: base(string.Empty, startEntity, endEntity, startNavigator, endNavigator, EntityRelationType.ManyToMany, null, false, false)
		{
			this.FirstRelationshipName = firstRelationshipName;
			this.SecondRelationshipName = secondRelationshipName;
		}
		public string FirstRelationshipName;
		public string SecondRelationshipName;
	}

	
	/// <summary>
	/// Enum to store the discriminator value type in the project XML file.
	/// </summary>
	private enum DiscriminatorValueType
	{
		Unknown,
		Int64, 
		Int32,
		Int16,
		Byte,
		String,
		Guid,
		Decimal
	}
	private void __ScriptCode() {
 
	_currentProject = _executingGenerator.ProjectDefinition;
	_typeShortcuts = new Dictionary<Type, string>();
	_systemTypeShortcuts = new Dictionary<Type, string>();
	_relationshipFullNamesInUse = new Dictionary<string, object>();
	_spCallNamesUsed = new Dictionary<string, object>();
	_namePerSPCall = new Dictionary<SPCallDefinition, string>();
	string driverID = _currentProject.DatabaseDriverID;
	if(driverID=="FBAE9964-8A54-4955-921B-827EDD41FFB4")
	{
		// ODP.NET 8i/9i specific driver, convert to ODP.NET driver id
		driverID = "A8034CB0-319D-4cdd-BC7D-1E7CFDBA3B74";
	}	
	_typeShortcuts.Add(typeof(bool), "bool");
	_typeShortcuts.Add(typeof(byte), "byte");
	_typeShortcuts.Add(typeof(byte[]), "byte[]");
	_typeShortcuts.Add(typeof(char), "char");
	_typeShortcuts.Add(typeof(DateTime), "datetime");
	_typeShortcuts.Add(typeof(DateTimeOffset), "datetimeoffset");
	_typeShortcuts.Add(typeof(decimal), "decimal");
	_typeShortcuts.Add(typeof(double), "double");
	_typeShortcuts.Add(typeof(Guid), "guid");
	_typeShortcuts.Add(typeof(int), "int");
	_typeShortcuts.Add(typeof(System.Net.IPAddress), "ipaddress");
	_typeShortcuts.Add(typeof(long), "long");
	_typeShortcuts.Add(typeof(object), "object");
	_typeShortcuts.Add(typeof(sbyte), "sbyte");
	_typeShortcuts.Add(typeof(short), "short");
	_typeShortcuts.Add(typeof(float), "single");
	_typeShortcuts.Add(typeof(string), "string");
	_typeShortcuts.Add(typeof(TimeSpan), "timespan");
	_typeShortcuts.Add(typeof(uint), "uint");
	_typeShortcuts.Add(typeof(ushort), "ushort");
	_typeShortcuts.Add(typeof(ulong), "ulong");
	
	_entityRelationPerUniqueKey = new Dictionary<string, EntityRelation>();
	_oppositeRelationPerRelation = new Dictionary<string, string>();
	_normalRelationshipInfoPerUniqueKey = new Dictionary<string, RelationshipInfo>();
	_indirectRelationshipInfoPerUniqueKey = new Dictionary<string, IndirectRelationshipInfo>();
	_typeShortcutNames = new Dictionary<string, object>();
	_netSystemNameFragments = new string[4];	// .NET 2.0 workaround for test method which originally uses .net 3.5 extension methods.
	_netSystemNameFragments[0]="System";
	_netSystemNameFragments[1]="mscorlib";
	_netSystemNameFragments[2]="System.";
	_netSystemNameFragments[3]="Microsoft.";
	
	_systemSequences = new Dictionary<string, List<string>>();
	InitSystemSequences();
	
	foreach(KeyValuePair<Type, string> kvp in _typeShortcuts)
	{
		_typeShortcutNames[kvp.Value] = null;
		_systemTypeShortcuts.Add(kvp.Key, kvp.Value);
	}
	PreProcess();

		__outputWriter.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!--LLBLGen Pro v4.x project file. Do not manually edit this file. Incorrect references could lead to an unloadable project file.-->\r\n<!--File converted from earlier version LLBLGen Pro project file: ");
__outputWriter.Write(_currentProject.ProjectLocationPathFilename);
		__outputWriter.Write(" -->\r\n<Project Version=\"4.1\" TargetFrameworkName=\"LLBLGen Pro Runtime Framework\">\r\n	<Properties>\r\n");
	if(_currentProject.Properties.Abbreviations.Count<=0)
	{

		__outputWriter.Write("		<Abbreviations/>\r\n");
	}
	else
	{

		__outputWriter.Write("		<Abbreviations>\r\n");
		foreach(KeyValuePair<string, string> kvp in _currentProject.Properties.Abbreviations)
		{

		__outputWriter.Write("			<Abbreviation Name=\"");
__outputWriter.Write(kvp.Key);
		__outputWriter.Write("\" Value=\"");
__outputWriter.Write(kvp.Value);
		__outputWriter.Write("\" />\r\n");
		}

		__outputWriter.Write("		</Abbreviations>\r\n");
	}
	
	ProjectProperties properties = _currentProject.Properties;

		__outputWriter.Write("		<Property Name=\"AdditionalTaskPerformerFolder\" Value=\"");
__outputWriter.Write(properties.AdditionalTaskPerformerFolder);
		__outputWriter.Write("\" />\r\n		<Property Name=\"AdditionalTasksFolder\" Value=\"");
__outputWriter.Write(properties.AdditionalTasksFolder);
		__outputWriter.Write("\" />\r\n		<Property Name=\"AdditionalTemplatesFolder\" Value=\"");
__outputWriter.Write(properties.AdditionalTemplatesFolder);
		__outputWriter.Write("\" />\r\n		<Property Name=\"AdditionalTypeConverterFolder\" Value=\"");
__outputWriter.Write(properties.AdditionalTypeConverterFolder);
		__outputWriter.Write("\" />\r\n		<Property Name=\"AddNewElementsAfterRefresh\" Value=\"");
__outputWriter.Write((int)properties.AddNewElementsAfterRefresh);
		__outputWriter.Write("\" />\r\n		<Property Name=\"AddNewFieldsAfterRefresh\" Value=\"");
__outputWriter.Write((int)properties.AddNewFieldsAfterRefresh);
		__outputWriter.Write("\" />\r\n		<Property Name=\"AddNewViewsAsEntitiesAfterRefresh\" Value=\"");
__outputWriter.Write((int)properties.AddNewViewsAsEntitiesAfterRefresh);
		__outputWriter.Write("\" />\r\n		<Property Name=\"AutoAddManyToManyRelationships\" Value=\"");
__outputWriter.Write(XmlConvert.ToString(properties.HideManyToManyRelationsOnCreation==TriStateBool.True));
		__outputWriter.Write("\" />\r\n		<Property Name=\"AutoAssignSequencesToIntegerPks\" Value=\"true\" />\r\n		<Property Name=\"AutoAssignTypeConverterToFieldMapping\" Value=\"");
__outputWriter.Write(XmlConvert.ToString(properties.AutoAssignTypeConverterToNewField));
		__outputWriter.Write("\" />\r\n		<Property Name=\"CleanUpVsNetProjects\" Value=\"");
__outputWriter.Write(XmlConvert.ToString(properties.CleanUpVsNetProjects));
		__outputWriter.Write("\" />\r\n		<Property Name=\"ConnectionStringKeyNamePattern\" Value=\"");
__outputWriter.Write(properties.ConnectionStringKeyName);
		__outputWriter.Write(".{$ProviderName}\" />\r\n		<Property Name=\"EncodingToUse\" Value=\"");
__outputWriter.Write((int)properties.EncodingToUse);
		__outputWriter.Write("\" />\r\n		<Property Name=\"EnforcePascalCasingAlways\" Value=\"");
__outputWriter.Write(XmlConvert.ToString(properties.EnforcePascalCasingAlways));
		__outputWriter.Write("\" />\r\n		<Property Name=\"EntityFieldNameStripPattern\" Value=\"");
__outputWriter.Write(properties.EntityFieldNameStripPattern);
		__outputWriter.Write("\" />\r\n		<Property Name=\"EntityNameStripPattern\" Value=\"");
__outputWriter.Write(properties.EntityNameStripPattern);
		__outputWriter.Write("\" />\r\n		<Property Name=\"ExcludableOrphanedElementDetectedAction\" Value=\"3\" />\r\n		<Property Name=\"FailCodeGenerationOnWriteError\" Value=\"");
__outputWriter.Write(XmlConvert.ToString(properties.FailCodeGenerationOnWriteError));
		__outputWriter.Write("\" />\r\n		<Property Name=\"FieldMappedOntoRelatedFieldPattern\" Value=\"");
__outputWriter.Write(properties.FieldMappedOnRelatedFieldPattern);
		__outputWriter.Write("\" />\r\n		<Property Name=\"FkFieldsAreNamedAfterTargetField\" Value=\"true\"/>\r\n		<Property Name=\"ForeignKeyFieldPattern\" Value=\"{$NavigatorName}{$RelatedFieldName}\" />\r\n		<Property Name=\"GroupUsage\" Value=\"0\" />\r\n		<Property Name=\"IdentifyingFieldsFollowPrimaryKeys\" Value=\"0\" />\r\n		<Property Name=\"InsertUnderscoreAtWordBreakCaseSensitiveDBs\" Value=\"false\" />\r\n		<Property Name=\"MakeElementNamePascalCasing\" Value=\"");
__outputWriter.Write(XmlConvert.ToString(properties.MakeElementNamePascalCasing));
		__outputWriter.Write("\" />\r\n		<Property Name=\"RelationalModelDataElementNameCasingCaseSensitiveDBs\" Value=\"3\" />\r\n		<Property Name=\"ResetFieldOrderBasedOnTargetOrderAtRefresh\" Value=\"false\"/>\r\n		<Property Name=\"NavigatorMappedOntoManyToManyPattern\" Value=\"");
__outputWriter.Write(properties.FieldMappedOnManyToManyPattern);
		__outputWriter.Write("\" />\r\n		<Property Name=\"NavigatorMappedOntoOneManyToOnePattern\" Value=\"");
__outputWriter.Write(properties.FieldMappedOnOneManyToOnePattern);
		__outputWriter.Write("\" />\r\n		<Property Name=\"NavigatorMappedOntoOneToManyPattern\" Value=\"");
__outputWriter.Write(properties.FieldMappedOnOneToManyPattern);
		__outputWriter.Write("\" />\r\n		<Property Name=\"NonExcludableOrphanedElementDetectedAction\" Value=\"3\" />\r\n		<Property Name=\"PreferDecimalOverCurrencyTypes\" Value=\"true\" />\r\n		<Property Name=\"PreferNaturalCharacterTypes\" Value=\"true\" />\r\n		<Property Name=\"PreferVariableLengthTypes\" Value=\"true\" />\r\n		<Property Name=\"ProjectCreator\" Value=\"");
__outputWriter.Write(HttpUtility.HtmlEncode(properties.ProjectCreator));
		__outputWriter.Write("\" />\r\n		<Property Name=\"ProjectName\" Value=\"");
__outputWriter.Write(properties.ProjectName);
		__outputWriter.Write("\" />\r\n		<Property Name=\"RelationshipsFollowDBForeignKeyConstraints\" Value=\"0\" />\r\n		<Property Name=\"RemoveUnderscoresFromElementName\" Value=\"");
__outputWriter.Write(XmlConvert.ToString(properties.RemoveUnderscoresFromElementName));
		__outputWriter.Write("\" />\r\n		<Property Name=\"RemoveUnmappedElementsAfterRefresh\" Value=\"0\" />\r\n		<Property Name=\"RetrieveDBCustomProperties\" Value=\"");
__outputWriter.Write(XmlConvert.ToString(properties.RetrieveDBCustomProperties));
		__outputWriter.Write("\" />\r\n		<Property Name=\"RootNamespace\" Value=\"\" />\r\n		<Property Name=\"SequencePattern\" Value=\"SEQ_{$EntityName}\" />\r\n		<Property Name=\"SetGroupNameAfterSchemaName\" Value=\"true\" />\r\n		<Property Name=\"StoredProcNameStripPattern\" Value=\"");
__outputWriter.Write(properties.StoredProcNameStripPattern);
		__outputWriter.Write("\" />\r\n		<Property Name=\"StoreTimeLastGeneratedIntoProject\" Value=\"");
__outputWriter.Write(XmlConvert.ToString(properties.StoreTimeLastGeneratedIntoProject));
		__outputWriter.Write("\" />\r\n		<Property Name=\"SyncMappedElementNamesAfterRefresh\" Value=\"");
__outputWriter.Write((int)properties.SyncMappedElementNamesAfterRefresh);
		__outputWriter.Write("\" />\r\n		<Property Name=\"SyncRelationalModelDataElementNameAfterRename\" Value=\"0\" />\r\n		<Property Name=\"SyncRenamedMappedElementNamesAfterRefresh\" Value=\"");
__outputWriter.Write((int)properties.SyncRenamedFieldElementsAfterRefresh);
		__outputWriter.Write("\" />\r\n		<Property Name=\"TargetPerEntityEdgesRequireBackingFkConstraint\" Value=\"true\"/>\r\n		<Property Name=\"TypedViewFieldNameStripPattern\" Value=\"");
__outputWriter.Write(properties.TypedViewFieldNameStripPattern);
		__outputWriter.Write("\" />\r\n		<Property Name=\"TypedViewNameStripPattern\" Value=\"");
__outputWriter.Write(properties.TypedViewNameStripPattern);
		__outputWriter.Write("\" />\r\n		<Property Name=\"UniqueConstraintsFollowDBUniqueConstraints\" Value=\"0\" />\r\n		<Property Name=\"UpdateCustomPropertiesAfterRefresh\" Value=\"");
__outputWriter.Write((int)properties.UpdateCustomPropertiesAfterRefresh);
		__outputWriter.Write("\" />\r\n		<Property Name=\"UseCustomFieldOrderingOnNewElements\" Value=\"true\"/>\r\n	</Properties>\r\n	<TypeShortcuts>\r\n");
	foreach(KeyValuePair<Type, string> typeShortcut in _typeShortcuts)
	{
		if(_systemTypeShortcuts.ContainsKey(typeShortcut.Key))
		{
			continue;
		}

		__outputWriter.Write("		<TypeShortcut Shortcut=\"");
__outputWriter.Write(typeShortcut.Value);
		__outputWriter.Write("\" Type=\"");
__outputWriter.Write(GetFullTypeName(typeShortcut.Key));
		__outputWriter.Write("\" />\r\n");
	}

		__outputWriter.Write("	</TypeShortcuts>\r\n	<CodeGenerationCyclePreferences>\r\n		<OutputType Value=\"3\">\r\n			<LastUsedPreferences>\r\n				<DestinationRootFolder Value=\"");
__outputWriter.Write(_currentProject.DestinationFolder);
		__outputWriter.Write("\" />\r\n				<FrameworkName Value=\"LLBLGen Pro Runtime Framework\" />\r\n				<LanguageName Value=\"");
__outputWriter.Write(_currentProject.LastUsedLanguageName);
		__outputWriter.Write("\" />\r\n				<PlatformName Value=\"");
__outputWriter.Write(_currentProject.LastUsedPlatformName);
		__outputWriter.Write("\" />\r\n				<PresetName Value=\"");
__outputWriter.Write(_currentProject.LastUsedPresetName);
		__outputWriter.Write("\" />\r\n				<RootNamespace Value=\"");
__outputWriter.Write(_currentProject.RootNameSpace);
		__outputWriter.Write("\" />\r\n				<TemplateGroup Value=\"");
__outputWriter.Write(_currentProject.LastUsedTemplateGroup);
		__outputWriter.Write("\" />\r\n				<OutputType Value=\"3\" />\r\n");
	if(_currentProject.LastGenerationStartedOn!=null)
	{

		__outputWriter.Write("				<LastGenerationCycleStartedOn Value=\"");
__outputWriter.Write(XmlConvert.ToString(_currentProject.LastGenerationStartedOn.Value, XmlDateTimeSerializationMode.Utc));
		__outputWriter.Write("\" />\r\n");
	}

		__outputWriter.Write("				<TemplateBindings>\r\n");
	foreach(string templateBinding in _currentProject.LastUsedTemplateBindings)
	{

		__outputWriter.Write("					<Binding Name=\"");
__outputWriter.Write(templateBinding);
		__outputWriter.Write("\" />\r\n");
	}

		__outputWriter.Write("				</TemplateBindings>\r\n			</LastUsedPreferences>\r\n		</OutputType>	\r\n	</CodeGenerationCyclePreferences>\r\n	<OutputSettingValues>\r\n		<SettingValues>\r\n			<SettingValue Name=\"1:AdapterDbGenericProjectFileSuffix\" Value=\"");
__outputWriter.Write(properties.AdapterDbGenericProjectFileSuffix);
		__outputWriter.Write("\" Type=\"18\" />\r\n			<SettingValue Name=\"1:AdapterDbGenericSubFolderName\" Value=\"");
__outputWriter.Write(properties.AdapterDbGenericSubFolderName);
		__outputWriter.Write("\" Type=\"18\" />\r\n			<SettingValue Name=\"1:AdapterDbSpecificNamespaceSuffix\" Value=\"");
__outputWriter.Write(properties.AdapterDbSpecificNamespaceSuffix);
		__outputWriter.Write("\" Type=\"18\" />\r\n			<SettingValue Name=\"1:AdapterDbSpecificProjectFileSuffix\" Value=\"");
__outputWriter.Write(properties.AdapterDbSpecificProjectFileSuffix);
		__outputWriter.Write("\" Type=\"18\" />\r\n			<SettingValue Name=\"1:AdapterDbSpecificSubFolderName\" Value=\"");
__outputWriter.Write(properties.AdapterDbSpecificSubFolderName);
		__outputWriter.Write("\" Type=\"18\" />\r\n			<SettingValue Name=\"1:ConvertNulledReferenceTypesToDefaultValue\" Value=\"");
__outputWriter.Write(XmlConvert.ToString(properties.ConvertNulledReferenceTypesToDefaultValue));
		__outputWriter.Write("\" Type=\"3\" />\r\n			<SettingValue Name=\"1:GenerateAsNullableTypeDefault\" Value=\"");
__outputWriter.Write(XmlConvert.ToString(properties.GenerateNullableFieldsAsNullableTypes));
		__outputWriter.Write("\" Type=\"3\" />\r\n			<SettingValue Name=\"1:LazyLoadingWithoutResultReturnsNew\" Value=\"");
__outputWriter.Write(XmlConvert.ToString(properties.LazyLoadingWithoutResultReturnsNew));
		__outputWriter.Write("\" Type=\"3\" />\r\n			<SettingValue Name=\"1:TdlEmitTimeDateInOutputFiles\" Value=\"");
__outputWriter.Write(XmlConvert.ToString(properties.TdlEmitTimeDateInOutputFiles));
		__outputWriter.Write("\" Type=\"3\" />\r\n		</SettingValues>\r\n		");
__outputWriter.Write(ProduceCustomPropertiesXml(_currentProject.CustomProperties, false));
		__outputWriter.Write("\r\n	</OutputSettingValues>\r\n	<CodeGenerationMetaDataDefaults>\r\n		<TargetElement Type=\"256\">\r\n			<Defaults>\r\n				<Attributes>\r\n					<Attribute Value=\"Browsable($");
__outputWriter.Write(XmlConvert.ToString(properties.HideManyOneToOneRelatedEntityPropertiesFromDataBinding));
		__outputWriter.Write(")\" />\r\n				</Attributes>\r\n			</Defaults>\r\n		</TargetElement>\r\n	</CodeGenerationMetaDataDefaults>\r\n	<EntityModel>\r\n	    <ValueTypeDefinitions />\r\n    	<EntityDefinitions>\r\n");
	foreach(EntityDefinition entity in _currentProject.Entities)	
	{
		string discriminatorValueTypeAsString;
		string discriminatorValueAsString = ConvertDiscriminatorValueToXmlElements(entity.DiscriminatorValue, out discriminatorValueTypeAsString);


		__outputWriter.Write("			<EntityDefinition Name=\"");
__outputWriter.Write(entity.Name);
		__outputWriter.Write("\" ");
 if(entity.IsInHierarchy) {
		__outputWriter.Write(" IsAbstract=\"");
__outputWriter.Write(XmlConvert.ToString(entity.IsAbstract));
		__outputWriter.Write("\" DiscriminatorValue=\"");
__outputWriter.Write(discriminatorValueAsString);
		__outputWriter.Write("\" DiscriminatorValueType=\"");
__outputWriter.Write(discriminatorValueTypeAsString);
		__outputWriter.Write("\" InheritanceType=\"");
__outputWriter.Write((int)entity.HierarchyType);
		__outputWriter.Write("\" ");
 }
		__outputWriter.Write(">\r\n				<Fields>\r\n");
		int fieldIndex = 0;	
		foreach(EntityFieldDefinition field in entity.Fields)
		{
			if((field.MappedField==null) || (entity.IsSubType && field.IsPrimaryKeyField))
			{
				continue;
			}
			bool isFkField = false;
			EntityRelation relationshipToUse = FindRelationToUse(entity, field);
			if(relationshipToUse!=null)
			{
				EntityFieldDefinition relatedField = GetRelatedFieldOfFkField(relationshipToUse, field);
				if(relatedField!=field)
				{
					//  FK field
					isFkField=true;
				}
			}
			bool isReadOnly = (field.IsReadOnly | field.IsDiscriminatorField);
			string customPropertiesXml = ProduceCustomPropertiesXml(field.CustomProperties, false);
			string outputSettingsXml = string.Empty;
			if((field.GenerateAsNullableOfT != properties.GenerateNullableFieldsAsNullableTypes) || !string.IsNullOrEmpty(customPropertiesXml))
			{
				outputSettingsXml = "<OutputSettingValues>";
				if(field.GenerateAsNullableOfT != properties.GenerateNullableFieldsAsNullableTypes)
				{
					outputSettingsXml +="<SettingValues><SettingValue Name=\"64:GenerateAsNullableType\" Value=\"" + XmlConvert.ToString(field.GenerateAsNullableOfT) + "\" Type=\"3\"/></SettingValues>";
				}
				if(!string.IsNullOrEmpty(customPropertiesXml))
				{
					outputSettingsXml += customPropertiesXml;
				}
				outputSettingsXml += "</OutputSettingValues>";
			}
			int length;
			int precision;
			int scale;
			GetRealTypeSpecifications(field.DotNetType, field.MappedFieldLength, field.MappedFieldPrecision, field.MappedFieldScale, false, out length, out precision, out scale);

		__outputWriter.Write("					<Field Name=\"");
__outputWriter.Write(field.FieldName);
		__outputWriter.Write("\" Type=\"");
__outputWriter.Write(GetTypeShortcut(field.DotNetType));
		__outputWriter.Write("\" IsReadOnly=\"");
__outputWriter.Write(XmlConvert.ToString(isReadOnly));
		__outputWriter.Write("\" IsOptional=\"");
__outputWriter.Write(XmlConvert.ToString(field.IsNullable));
		__outputWriter.Write("\" MaxLength=\"");
__outputWriter.Write(length);
		__outputWriter.Write("\" Precision=\"");
__outputWriter.Write(precision);
		__outputWriter.Write("\" Scale=\"");
__outputWriter.Write(scale);
		__outputWriter.Write("\" IsPrimaryKey=\"");
__outputWriter.Write(XmlConvert.ToString(field.IsPrimaryKeyField));
		__outputWriter.Write("\" IsDiscriminator=\"");
__outputWriter.Write(XmlConvert.ToString(field.IsDiscriminatorField));
		__outputWriter.Write("\"");
 if(isFkField) {
		__outputWriter.Write(" IsAutoCreated=\"true\" ");
}
		__outputWriter.Write(" FieldIndex=\"");
__outputWriter.Write(fieldIndex);
		__outputWriter.Write("\">");
__outputWriter.Write(outputSettingsXml);
		__outputWriter.Write("</Field>\r\n");
			fieldIndex++;
		}

		__outputWriter.Write("				</Fields>\r\n");
		if(entity.FieldsMappedOnRelatedFields.Count>0)
		{

		__outputWriter.Write("				<Forfs>\r\n");
			foreach(FieldOnRelatedFieldDefinition forf in entity.FieldsMappedOnRelatedFields)
			{

		__outputWriter.Write("					<Forf Name=\"");
__outputWriter.Write(forf.FieldName);
		__outputWriter.Write("\" MappedField=\":");
__outputWriter.Write(forf.RelatedField.ContainingEntityName);
		__outputWriter.Write(":");
__outputWriter.Write(forf.RelatedField.FieldName);
		__outputWriter.Write("\" RelationshipName=\"");
__outputWriter.Write(GetRelationshipName(forf.RelationUsed));
		__outputWriter.Write("\" ViaStartNavigator=\"");
__outputWriter.Write(XmlConvert.ToString(!forf.RelationUsed.StartEntityIsPkSide));
		__outputWriter.Write("\" IsReadOnly=\"");
__outputWriter.Write(XmlConvert.ToString(forf.IsReadOnly));
		__outputWriter.Write("\" />	\r\n");
			}

		__outputWriter.Write("				</Forfs>\r\n");
		}

		List<ArrayList> ucsUsedByRelations = new List<ArrayList>();
		foreach(EntityFieldDefinition fkField in entity.Fields)
		{
			if((fkField.MappedField==null) || (entity.IsSubType && fkField.IsPrimaryKeyField))
			{
				continue;
			}
			EntityRelation relationshipToUse = FindRelationToUse(entity, fkField);
			if(relationshipToUse==null)
			{
				//  not an FK field
				continue;
			}
			ArrayList ucOfRelation = relationshipToUse.GetUCFormedByFKSide();
			if(ucOfRelation!=null)
			{
				ucsUsedByRelations.Add(ucOfRelation);
			}
		}

		if(entity.UniqueConstraints.Count>0)
		{

		__outputWriter.Write("	    	    <UniqueConstraints>\r\n");
			foreach(DictionaryEntry entry in entity.UniqueConstraints)
			{
				// if the UC is actually the fk side of a 1:1 relationship, we have to ignore it. 
				if(ucsUsedByRelations.Contains((ArrayList)entry.Value))
				{
					continue;
				}
				if(entity.PrimaryKeyFields.Count==((ArrayList)entry.Value).Count)
				{
					// test whether the fields in the UC are all pk fields.
					bool skip = true;
					foreach(EntityFieldDefinition field in (ArrayList)entry.Value)
					{
						skip &= field.IsPrimaryKeyField;
						if(!skip)
						{
							break;
						}
					}
					if(skip)
					{
						continue;
					}
				}
				string ucName = (string)entry.Key;
				ucName = ucName.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("_", "");

		__outputWriter.Write("					<UniqueConstraint Name=\"");
__outputWriter.Write(ucName);
		__outputWriter.Write("\">\r\n");
				foreach(EntityFieldDefinition field in (ArrayList)entry.Value)
				{

		__outputWriter.Write("						<Field Name=\"");
__outputWriter.Write(field.FieldName);
		__outputWriter.Write("\" />\r\n");
				}

		__outputWriter.Write("					</UniqueConstraint>\r\n");
			}

		__outputWriter.Write("				</UniqueConstraints>	\r\n");
		}
		
		string entityCustomPropertiesXml = ProduceCustomPropertiesXml(entity.CustomProperties, false);
		if(!string.IsNullOrEmpty(entityCustomPropertiesXml) || (entity.AdditionalInterfaces.Count>0) || (entity.AdditionalNamespaces.Count>0))
		{

		__outputWriter.Write("				<OutputSettingValues>\r\n");
			if(entity.AdditionalInterfaces.Count>0)
			{

		__outputWriter.Write("					<Interfaces>\r\n");
				foreach(string name in entity.AdditionalInterfaces)
				{

		__outputWriter.Write("						<Interface Name=\"");
__outputWriter.Write(name);
		__outputWriter.Write("\" />\r\n");
				}

		__outputWriter.Write("					</Interfaces>\r\n");
			}
			if(entity.AdditionalNamespaces.Count>0)
			{

		__outputWriter.Write("					<Namespaces>\r\n");
				foreach(string name in entity.AdditionalNamespaces)
				{

		__outputWriter.Write("						<Namespace Name=\"");
__outputWriter.Write(name);
		__outputWriter.Write("\" />\r\n");
				}

		__outputWriter.Write("					</Namespaces>\r\n");
			}

		__outputWriter.Write("			");
__outputWriter.Write(entityCustomPropertiesXml);
		__outputWriter.Write("\r\n				</OutputSettingValues>\r\n");
		}

		__outputWriter.Write("			</EntityDefinition>\r\n");
	}

		__outputWriter.Write("		</EntityDefinitions>\r\n		<Relationships>\r\n");

	foreach(KeyValuePair<string, RelationshipInfo> kvp in _normalRelationshipInfoPerUniqueKey)
	{
		int relationType = 1;
		string startIsPkSideAttribute=string.Empty;
		string modelOnlyAttribute = string.Empty;
		if(kvp.Value.ModelOnly)
		{
			modelOnlyAttribute = " ModelOnly=\"true\"";
		}
		if(kvp.Value.StartIsPkSide)
		{
			startIsPkSideAttribute=" StartIsPkSide=\"true\"";
		}
		string startIsOptionalAttribute = string.Empty;
		if(kvp.Value.StartIsOptional)
		{
			startIsOptionalAttribute = " StartIsOptional=\"true\"";	
		}
		string endIsOptionalAttribute = string.Empty;
		if(kvp.Value.EndIsOptional)
		{
			endIsOptionalAttribute = " EndIsOptional=\"true\"";
		}
		switch(kvp.Value.RelationshipType)
		{
			case EntityRelationType.ManyToOne:
				relationType = 4;
				break;
			case EntityRelationType.OneToMany:
				relationType = 2;
				break;
			case EntityRelationType.OneToOne:
				relationType = 1;
				break;
			case EntityRelationType.ManyToMany:
				continue;
		}

		__outputWriter.Write("			<NormalRelationship Name=\"");
__outputWriter.Write(kvp.Value.FullName);
		__outputWriter.Write("\" Start=\":");
__outputWriter.Write(kvp.Value.StartEntity);
		__outputWriter.Write("\" End=\":");
__outputWriter.Write(kvp.Value.EndEntity);
		__outputWriter.Write("\"");
__outputWriter.Write(startIsOptionalAttribute);
__outputWriter.Write(endIsOptionalAttribute);
		__outputWriter.Write(" StartNavigator=\"");
__outputWriter.Write(kvp.Value.StartNavigator);
		__outputWriter.Write("\" EndNavigator=\"");
__outputWriter.Write(kvp.Value.EndNavigator);
		__outputWriter.Write("\" Type=\"");
__outputWriter.Write(relationType);
		__outputWriter.Write("\"");
__outputWriter.Write(startIsPkSideAttribute);
__outputWriter.Write(modelOnlyAttribute);
		__outputWriter.Write(" >\r\n				<FieldRelationships>\r\n");
		foreach(Pair<string, string> pair in kvp.Value.FkFieldPkFieldPairs)
		{

		__outputWriter.Write("					<FieldRelationship FkField=\"");
__outputWriter.Write(pair.Value1);
		__outputWriter.Write("\" PkField=\"");
__outputWriter.Write(pair.Value2);
		__outputWriter.Write("\"/>\r\n");
		}

		__outputWriter.Write("				</FieldRelationships>\r\n			</NormalRelationship>\r\n");
	}
	foreach(KeyValuePair<string, IndirectRelationshipInfo> kvp in _indirectRelationshipInfoPerUniqueKey)
	{

		__outputWriter.Write("			<IndirectRelationship Start=\":");
__outputWriter.Write(kvp.Value.StartEntity);
		__outputWriter.Write("\" End=\":");
__outputWriter.Write(kvp.Value.EndEntity);
		__outputWriter.Write("\" StartNavigator=\"");
__outputWriter.Write(kvp.Value.StartNavigator);
		__outputWriter.Write("\" EndNavigator=\"");
__outputWriter.Write(kvp.Value.EndNavigator);
		__outputWriter.Write("\" Relationship1=\"");
__outputWriter.Write(kvp.Value.FirstRelationshipName);
		__outputWriter.Write("\" Relationship2=\"");
__outputWriter.Write(kvp.Value.SecondRelationshipName);
		__outputWriter.Write("\" />\r\n");
	}

		__outputWriter.Write("		</Relationships>\r\n		<InheritanceHierarchies>\r\n");
	foreach(EntityDefinition entity in _currentProject.Entities)
	{
		if(entity.SuperType==null)
		{
			continue;
		}

		__outputWriter.Write("			<InheritanceHierarchy SuperType=\":");
__outputWriter.Write(entity.SuperTypeName);
		__outputWriter.Write("\" SubType=\":");
__outputWriter.Write(entity.Name);
		__outputWriter.Write("\" />	\r\n");
	}	

		__outputWriter.Write("		</InheritanceHierarchies>\r\n	</EntityModel>\r\n");
	if(_currentProject.TypedLists.Count>0)
	{

		__outputWriter.Write("	<TypedListDefinitions>\r\n");
		foreach(TypedListDefinition typedList in _currentProject.TypedLists)
		{
			Dictionary<EntityDefinition, Dictionary<string, object>> aliasStringsLookup = new Dictionary<EntityDefinition, Dictionary<string, object>>();

		__outputWriter.Write("		<TypedListDefinition Name=\"");
__outputWriter.Write(typedList.Name);
		__outputWriter.Write("\">\r\n			<EntityAliases>\r\n");
			foreach(EntityAlias alias in typedList.EntityAliases.Keys)
			{
				Dictionary<string, object> aliasStringsForEntity;
				if(!aliasStringsLookup.TryGetValue(alias.AliassedEntity, out aliasStringsForEntity))
				{
					aliasStringsForEntity = new Dictionary<string, object>();
					aliasStringsLookup[alias.AliassedEntity] = aliasStringsForEntity;
				}
				if(aliasStringsForEntity.ContainsKey(alias.Alias))
				{
					// duplicate alias for same entity, skip
					continue;
				}
				aliasStringsForEntity[alias.Alias] = null;

		__outputWriter.Write("				<EntityAlias ");
 if(!string.IsNullOrEmpty(alias.Alias)) { 
		__outputWriter.Write(" Alias=\"");
__outputWriter.Write(alias.Alias);
		__outputWriter.Write("\" ");
}
		__outputWriter.Write(" Entity=\":");
__outputWriter.Write(alias.AliassedEntity.Name);
		__outputWriter.Write("\"/>\r\n");
			}

		__outputWriter.Write("			</EntityAliases>\r\n			<Fields>\r\n");
			foreach(TypedListFieldDefinition field in typedList.Fields)
			{
				string entityAlias = field.AliasNameOfEntity;
				if(entityAlias.Length<=0)
				{
					entityAlias = field.AliasOfEntity.EntityName;
				}

		__outputWriter.Write("				<Field FieldAlias=\"");
__outputWriter.Write(field.Alias);
		__outputWriter.Write("\" TargetField=\"");
__outputWriter.Write(entityAlias);
		__outputWriter.Write(".");
__outputWriter.Write(GetRealFieldDefinition(field.Field).FieldName);
		__outputWriter.Write("\"/>\r\n");
			}

		__outputWriter.Write("			</Fields>\r\n			<Relationships>\r\n");
			foreach(TypedListRelation relation in typedList.RelationsInTypedList)
			{
				bool startAliasIsStartVertex = !relation.WrappedRelation.StartEntityIsPkSide;
				// check whether the wrapped relation is the primary, which we have to check if the wrapped relation has the pkside at the start.
				if(relation.WrappedRelation.StartEntityIsPkSide)
				{
					string oppositeUniqueKey;
					_oppositeRelationPerRelation.TryGetValue(CreateUniqueKey(relation.WrappedRelation), out oppositeUniqueKey);
					// if opposite doesn't exists, the wrapped relation was chosen as primary, so we've to set flag as true
					startAliasIsStartVertex = string.IsNullOrEmpty(oppositeUniqueKey);
				}
				JoinHint hint = relation.Hint;
				if(hint==JoinHint.None)
				{
					hint = JoinHint.Inner;
				}
				// start vertex is joined towards end vertex. In v2.x this is stored in the opposite way

		__outputWriter.Write("				<Relationship Name=\"");
__outputWriter.Write(GetRelationshipName(relation.WrappedRelation));
		__outputWriter.Write("\" StartAlias=\"");
__outputWriter.Write(relation.RelationEndName);
		__outputWriter.Write("\" EndAlias=\"");
__outputWriter.Write(relation.RelationStartName);
		__outputWriter.Write("\" JoinHint=\"");
__outputWriter.Write((int)hint);
		__outputWriter.Write("\" StartAliasIsStartVertex=\"");
__outputWriter.Write(XmlConvert.ToString(!startAliasIsStartVertex));
		__outputWriter.Write("\"/>\r\n");
			}

		__outputWriter.Write("			</Relationships>\r\n			");
__outputWriter.Write(ProduceCustomPropertiesXml(typedList.CustomProperties, true));
		__outputWriter.Write("\r\n		</TypedListDefinition>\r\n");
		}

		__outputWriter.Write("	</TypedListDefinitions>\r\n");
	}

	if(_currentProject.TypedViews.Count>0)
	{

		__outputWriter.Write("	<TypedViewDefinitions>\r\n");
		foreach(TypedViewDefinition typedView in _currentProject.TypedViews)
		{

		__outputWriter.Write("		<TypedViewDefinition Name=\"");
__outputWriter.Write(typedView.Name);
		__outputWriter.Write("\">\r\n			<Fields>\r\n");
			int fieldIndex = 0;
			foreach(TypedViewFieldDefinition field in typedView.Fields)
			{
				int length;
				int precision;
				int scale;
				GetRealTypeSpecifications(field.DotNetType, field.MappedFieldLength, field.MappedFieldPrecision, field.MappedFieldScale, false, out length, out precision, out scale);

		__outputWriter.Write("				<Field Name=\"");
__outputWriter.Write(field.FieldName);
		__outputWriter.Write("\" Type=\"");
__outputWriter.Write(GetTypeShortcut(field.DotNetType));
		__outputWriter.Write("\" IsOptional=\"");
__outputWriter.Write(XmlConvert.ToString(field.MappedField.IsNullable));
		__outputWriter.Write("\" MaxLength=\"");
__outputWriter.Write(length);
		__outputWriter.Write("\" Precision=\"");
__outputWriter.Write(precision);
		__outputWriter.Write("\" Scale=\"");
__outputWriter.Write(scale);
		__outputWriter.Write("\" FieldIndex=\"");
__outputWriter.Write(fieldIndex);
		__outputWriter.Write("\">");
__outputWriter.Write(ProduceCustomPropertiesXml(field.CustomProperties, true));
		__outputWriter.Write("</Field>\r\n");
				fieldIndex++;
			}

		__outputWriter.Write("			</Fields>\r\n			");
__outputWriter.Write(ProduceCustomPropertiesXml(typedView.CustomProperties, true));
		__outputWriter.Write("\r\n		</TypedViewDefinition>\r\n");
		}

		__outputWriter.Write("	</TypedViewDefinitions>\r\n");
	}

	List<SPCallDefinition> spCalls = new List<SPCallDefinition>();
	foreach(SPCallDefinition spCall in _currentProject.ActionSPCalls)
	{
		spCalls.Add(spCall);
	}
	foreach(SPCallDefinition spCall in _currentProject.RetrievalSPCalls)
	{
		spCalls.Add(spCall);
	}
	if(spCalls.Count>0)
	{

		__outputWriter.Write("	<SPCallDefinitions>\r\n");
		foreach(SPCallDefinition spCall in spCalls)
		{
			string uniqueSPCallName = ProduceUniqueSPCallName(spCall);

		__outputWriter.Write("		<SPCallDefinition Name=\"");
__outputWriter.Write(uniqueSPCallName);
		__outputWriter.Write("\">\r\n			<Parameters>\r\n");
			int fieldIndex = 0;
			foreach(SPCallParameterDefinition parameter in spCall.Parameters)
			{
				int length;
				int precision;
				int scale;
				GetRealTypeSpecifications(parameter.DotNetType, parameter.SPParameterLength, parameter.SPParameterPrecision, parameter.SPParameterScale, false, out length, out precision, out scale);

		__outputWriter.Write("				<Parameter Name=\"");
__outputWriter.Write(parameter.ParameterName);
		__outputWriter.Write("\"  Type=\"");
__outputWriter.Write(GetTypeShortcut(parameter.DotNetType));
		__outputWriter.Write("\" IsOptional=\"");
__outputWriter.Write(XmlConvert.ToString(parameter.IsNullable));
		__outputWriter.Write("\" MaxLength=\"");
__outputWriter.Write(length);
		__outputWriter.Write("\" Precision=\"");
__outputWriter.Write(precision);
		__outputWriter.Write("\" Scale=\"");
__outputWriter.Write(scale);
		__outputWriter.Write("\" Direction=\"");
__outputWriter.Write((int)ConvertDirection(parameter.RelatedParameter.Direction));
		__outputWriter.Write("\" FieldIndex=\"");
__outputWriter.Write(fieldIndex);
		__outputWriter.Write("\"/>\r\n");
				fieldIndex++;
			}

		__outputWriter.Write("			</Parameters>\r\n			");
__outputWriter.Write(ProduceCustomPropertiesXml(spCall.CustomProperties, true));
		__outputWriter.Write("\r\n		</SPCallDefinition>\r\n");
		}

		__outputWriter.Write("	</SPCallDefinitions>\r\n");
	}

	// obtain system sequences so we can index from this set in multiple places below.
	List<string> systemSequences;
	_systemSequences.TryGetValue(_currentProject.DatabaseDriverID, out systemSequences);
	if(systemSequences==null)
	{
		systemSequences = new List<string>();
	}

		__outputWriter.Write("	<TargetDatabases>\r\n		<TargetDatabase Type=\"");
__outputWriter.Write(driverID);
		__outputWriter.Write("\">\r\n			<ConnectionElements>\r\n");
	foreach(DictionaryEntry entry in _currentProject.ConnectionElements)
	{
		string connectionElementName = ((ConnectionElement)entry.Key).ToString();
		object value = entry.Value;
		if(value==null)
		{
			continue;
		}
		string valueAsString = value.ToString();
		switch((ConnectionElement)entry.Key)
		{
			case ConnectionElement.CentralUnitNames:
				if(_currentProject.ConnectionElements.ContainsKey(ConnectionElement.CatalogName))
				{
					continue;
				}
				ArrayList centralUnitNames = (ArrayList)value;
				if(_currentProject.DatabaseDriver.RdbmsFunctionalityAspects.Contains(RdbmsFunctionalityAspect.CentralUnitIsCatalog) &&
						(centralUnitNames.Count>0))
				{
					connectionElementName = ConnectionElement.CatalogName.ToString();
					valueAsString = (string)centralUnitNames[0];
				}
				else
				{
					continue;
				}
				break;
			case ConnectionElement.Password:
				valueAsString = string.Empty;
				break;
			default:
				if(value is bool)
				{
					value = XmlConvert.ToString((bool)value);
				}
				break;
		}

		__outputWriter.Write("				<ConnectionElement Name=\"");
__outputWriter.Write(connectionElementName);
		__outputWriter.Write("\" Value=\"");
__outputWriter.Write(valueAsString);
		__outputWriter.Write("\"/>\r\n");
	}

		__outputWriter.Write("			</ConnectionElements>\r\n			<SystemSequences>\r\n");
	foreach(string sequence in systemSequences)
	{

		__outputWriter.Write("				<Sequence Name=\"");
__outputWriter.Write(sequence);
		__outputWriter.Write("\" />\r\n");
	}

		__outputWriter.Write("			</SystemSequences>		\r\n			<Catalogs>\r\n");
	foreach(DBCatalog catalog in _currentProject.Catalogs)
	{
		List<DBForeignKeyConstraint> fkConstraints = new List<DBForeignKeyConstraint>();

		__outputWriter.Write("				<Catalog Name=\"");
__outputWriter.Write(System.Security.SecurityElement.Escape(catalog.CatalogName));
		__outputWriter.Write("\">\r\n					<Schemas>\r\n");
		foreach(DictionaryEntry schemaEntry in catalog.DBSchemas)
		{
			DBSchema schema = (DBSchema)schemaEntry.Value;

		__outputWriter.Write("						<Schema Name=\"");
__outputWriter.Write(System.Security.SecurityElement.Escape(schemaEntry.Key.ToString()));
		__outputWriter.Write("\">\r\n							<Sequences>\r\n");
			foreach(DictionaryEntry sequenceEntry in schema.Sequences)
			{
				DBSequence sequence = (DBSequence)sequenceEntry.Value;
				if(systemSequences.Contains(sequence.SequenceName))
				{
					continue;
				}

		__outputWriter.Write("								<Sequence Name=\"");
__outputWriter.Write(GetCleanSequenceName(sequence.SequenceName, schema.SchemaOwner));
		__outputWriter.Write("\"/>\r\n");
			}

		__outputWriter.Write("							</Sequences>\r\n							<Tables>\r\n");
			foreach(DictionaryEntry tableEntry in schema.Tables)
			{
				DBTable table = (DBTable)tableEntry.Value;
				foreach(DictionaryEntry entry in table.ForeignKeyConstraints)
				{
					fkConstraints.Add((DBForeignKeyConstraint)entry.Value);
				}
				string pkConstraintName = string.Format("PK_{0}", Guid.NewGuid().ToString("N").Substring(5));

		__outputWriter.Write("								<Table Name=\"");
__outputWriter.Write(System.Security.SecurityElement.Escape(table.TableName));
		__outputWriter.Write("\">\r\n									<Fields>\r\n");
				foreach(DictionaryEntry fieldEntry in table.Fields)
				{
					DBTableField field = (DBTableField)fieldEntry.Value;
					string defaultValue = field.DefaultValue;
					if(!string.IsNullOrEmpty(defaultValue))
					{
						// make sure the default values don't contain a hard 0x00 value due to lame Oracle CLI issues.
						defaultValue = defaultValue.Replace("\0",string.Empty);
					}

		__outputWriter.Write("										<Field Name=\"");
__outputWriter.Write(System.Security.SecurityElement.Escape(field.FieldName));
		__outputWriter.Write("\" Ordinal=\"");
__outputWriter.Write(field.OrdinalPosition);
		__outputWriter.Write("\" IsPrimaryKey=\"");
__outputWriter.Write(XmlConvert.ToString(field.IsPrimaryKey));
		__outputWriter.Write("\" ");
 if(field.IsPrimaryKey) {
		__outputWriter.Write(" PkConstraintName=\"");
__outputWriter.Write(pkConstraintName);
		__outputWriter.Write("\" ");
 }
		__outputWriter.Write(" ");
__outputWriter.Write(ConvertTypeDefinitionToXmlAttributes(field.TypeDefinition));
		__outputWriter.Write(" IsComputed=\"");
__outputWriter.Write(XmlConvert.ToString(field.IsComputed));
		__outputWriter.Write("\" IsIdentity=\"");
__outputWriter.Write(XmlConvert.ToString(field.IsIdentity));
		__outputWriter.Write("\" IsOptional=\"");
__outputWriter.Write(XmlConvert.ToString(field.IsNullable));
		__outputWriter.Write("\" IsTimestamp=\"");
__outputWriter.Write(XmlConvert.ToString(field.IsTimeStamp));
		__outputWriter.Write("\" IsRowGUID=\"");
__outputWriter.Write(XmlConvert.ToString(field.IsRowGUID));
		__outputWriter.Write("\" DefaultValue=\"");
__outputWriter.Write(HttpUtility.HtmlEncode(defaultValue));
		__outputWriter.Write("\" DefaultSequence=\"");
__outputWriter.Write(field.SequenceName);
		__outputWriter.Write("\">");
__outputWriter.Write(ProduceCustomPropertiesXml(field.CustomProperties, false));
		__outputWriter.Write("</Field>\r\n");
				}

		__outputWriter.Write("									</Fields>\r\n									<UniqueConstraints>\r\n");
				foreach(DictionaryEntry entry in table.UniqueConstraints)
				{

		__outputWriter.Write("										<UniqueConstraint Name=\"");
__outputWriter.Write(System.Security.SecurityElement.Escape(entry.Key.ToString()));
		__outputWriter.Write("\">\r\n");
					foreach(DBTableField field in ((DBUniqueConstraint)entry.Value).Fields)
					{

		__outputWriter.Write("											<Field Name=\"");
__outputWriter.Write(System.Security.SecurityElement.Escape(field.FieldName));
		__outputWriter.Write("\"/>\r\n");
					}

		__outputWriter.Write("										</UniqueConstraint>\r\n");
				}

		__outputWriter.Write("									</UniqueConstraints>\r\n								</Table>\r\n");
			}

		__outputWriter.Write("							</Tables>\r\n							<Views>\r\n");
			foreach(DictionaryEntry viewEntry in schema.Views)
			{
				DBView view = (DBView)viewEntry.Value;

		__outputWriter.Write("								<View Name=\"");
__outputWriter.Write(System.Security.SecurityElement.Escape(view.ViewName));
		__outputWriter.Write("\">\r\n									<Fields>\r\n");
				foreach(DictionaryEntry fieldEntry in view.Fields)
				{
					DBViewField field = (DBViewField)fieldEntry.Value;

		__outputWriter.Write("										<Field Name=\"");
__outputWriter.Write(System.Security.SecurityElement.Escape(field.FieldName));
		__outputWriter.Write("\" Ordinal=\"");
__outputWriter.Write(field.OrdinalPosition);
		__outputWriter.Write("\" ");
__outputWriter.Write(ConvertTypeDefinitionToXmlAttributes(field.TypeDefinition));
		__outputWriter.Write(" IsIdentity=\"");
__outputWriter.Write(XmlConvert.ToString(field.IsIdentity));
		__outputWriter.Write("\" IsOptional=\"");
__outputWriter.Write(XmlConvert.ToString(field.IsNullable));
		__outputWriter.Write("\" IsTimestamp=\"");
__outputWriter.Write(XmlConvert.ToString(field.IsTimeStamp));
		__outputWriter.Write("\">");
__outputWriter.Write(ProduceCustomPropertiesXml(field.CustomProperties, false));
		__outputWriter.Write("</Field>\r\n");
				}

		__outputWriter.Write("									</Fields>\r\n								</View>\r\n");
			}

		__outputWriter.Write("							</Views>\r\n							<StoredProcedures>\r\n");
			foreach(DictionaryEntry procEntry in schema.StoredProcedures)
			{
				DBStoredProcedure proc = (DBStoredProcedure)procEntry.Value;

		__outputWriter.Write("								<StoredProcedure Name=\"");
__outputWriter.Write(System.Security.SecurityElement.Escape(proc.StoredProcedureName));
		__outputWriter.Write("\" NumberOfResultsets=\"");
__outputWriter.Write(proc.AmountResultsets);
		__outputWriter.Write("\">\r\n									<Parameters>\r\n");
				foreach(DictionaryEntry parameterEntry in proc.Parameters)
				{
					DBStoredProcedureParameter parameter = (DBStoredProcedureParameter)parameterEntry.Value;

		__outputWriter.Write("										<Parameter Name=\"");
__outputWriter.Write(System.Security.SecurityElement.Escape(parameter.ParameterName));
		__outputWriter.Write("\" Ordinal=\"");
__outputWriter.Write(parameter.OrdinalPosition);
		__outputWriter.Write("\" ");
__outputWriter.Write(ConvertTypeDefinitionToXmlAttributes(parameter.TypeDefinition));
		__outputWriter.Write(" IsResultParameter=\"");
__outputWriter.Write(XmlConvert.ToString(parameter.IsResultsetParameter));
		__outputWriter.Write("\" Direction=\"");
__outputWriter.Write((int)ConvertDirection(parameter.Direction));
		__outputWriter.Write("\">");
__outputWriter.Write(ProduceCustomPropertiesXml(parameter.CustomProperties, false));
		__outputWriter.Write("</Parameter>\r\n");
				}

		__outputWriter.Write("									</Parameters>\r\n								</StoredProcedure>\r\n");
			}

		__outputWriter.Write("							</StoredProcedures>\r\n						</Schema>\r\n");
		}

		__outputWriter.Write("					</Schemas>\r\n					<ForeignKeyConstraints>\r\n");
		foreach(DBForeignKeyConstraint fkConstraint in fkConstraints)
		{

		__outputWriter.Write("						<ForeignKeyConstraint Name=\"");
__outputWriter.Write(System.Security.SecurityElement.Escape(fkConstraint.ConstraintName));
		__outputWriter.Write("\">\r\n							<FieldPairs>\r\n");
			for(int i=0;i<fkConstraint.ForeignKeyFields.Count;i++)
			{
				DBTableField pkField = (DBTableField)fkConstraint.PrimaryKeyFields[i];
				DBTableField fkField = (DBTableField)fkConstraint.ForeignKeyFields[i];

		__outputWriter.Write("								<FieldPair PkField=\"");
__outputWriter.Write(System.Security.SecurityElement.Escape(pkField.ParentTable.ContainingSchema.SchemaOwner));
		__outputWriter.Write(":");
__outputWriter.Write(System.Security.SecurityElement.Escape(pkField.ParentTable.TableName));
		__outputWriter.Write(":");
__outputWriter.Write(System.Security.SecurityElement.Escape(pkField.FieldName));
		__outputWriter.Write("\" FkField=\"");
__outputWriter.Write(System.Security.SecurityElement.Escape(fkField.ParentTable.ContainingSchema.SchemaOwner));
		__outputWriter.Write(":");
__outputWriter.Write(System.Security.SecurityElement.Escape(fkField.ParentTable.TableName));
		__outputWriter.Write(":");
__outputWriter.Write(System.Security.SecurityElement.Escape(fkField.FieldName));
		__outputWriter.Write("\" />\r\n");
			}

		__outputWriter.Write("							</FieldPairs>\r\n						</ForeignKeyConstraint>\r\n");
		}

		__outputWriter.Write("					</ForeignKeyConstraints>\r\n				</Catalog>\r\n");
	}

		__outputWriter.Write("			</Catalogs>\r\n		</TargetDatabase>\r\n	</TargetDatabases>\r\n	<MappingStores>\r\n		<MappingStore Type=\"");
__outputWriter.Write(driverID);
		__outputWriter.Write("\">\r\n			<EntityMappings>\r\n");
	foreach(EntityDefinition entity in _currentProject.Entities)
	{
		IEntityMapTargetElement target = entity.Target;

		__outputWriter.Write("				<EntityMapping EntityName=\":");
__outputWriter.Write(entity.Name);
		__outputWriter.Write("\" TargetName=\"");
__outputWriter.Write(System.Security.SecurityElement.Escape(target.ContainingSchema.ParentCatalog.CatalogName));
		__outputWriter.Write(":");
__outputWriter.Write(System.Security.SecurityElement.Escape(target.ContainingSchema.SchemaOwner));
		__outputWriter.Write(":");
__outputWriter.Write(System.Security.SecurityElement.Escape(target.Name));
		__outputWriter.Write("\">\r\n					<FieldMappings>\r\n");
		foreach(EntityFieldDefinition field in entity.Fields)
		{
			string sequenceName = string.Empty;
			if(!entity.IsSubType)
			{
				sequenceName = field.IdentityValueSequenceName;
				if(!string.IsNullOrEmpty(sequenceName))
				{
					if(!systemSequences.Contains(sequenceName))
					{
						// schema sequence
						// a schema sequence can be in another schema. So we've to strip off any schema prefix if present.
						string realSequenceName = sequenceName;
						int firstIndexOfDot = realSequenceName.IndexOf(".");
						if(firstIndexOfDot >0)
						{
							realSequenceName = realSequenceName.Substring(firstIndexOfDot+1);
						}
						sequenceName = string.Format("{0}:{1}:{2}", target.ContainingSchema.ParentCatalog.CatalogName, target.ContainingSchema.SchemaOwner, realSequenceName);
					}
				}
			}
			EntityFieldDefinition realField = GetRealFieldDefinition(field);

		__outputWriter.Write("						<FieldMapping FieldName=\"");
__outputWriter.Write(realField.FieldName);
		__outputWriter.Write("\" TargetFieldName=\"");
__outputWriter.Write(System.Security.SecurityElement.Escape(field.MappedField.FieldName));
		__outputWriter.Write("\" ");
 if(!string.IsNullOrEmpty(sequenceName)) {
		__outputWriter.Write(" SequenceToUse=\"");
__outputWriter.Write(sequenceName);
		__outputWriter.Write("\" ");
} if(field.TypeConverter!=null) {
		__outputWriter.Write(" TypeConverterToUse=\"");
__outputWriter.Write(field.TypeConverterTypeName);
		__outputWriter.Write("\" ");
}
		__outputWriter.Write(" />\r\n");
		}

		__outputWriter.Write("					</FieldMappings>\r\n				</EntityMapping>\r\n");
	}

		__outputWriter.Write("			</EntityMappings>\r\n			<TypedViewMappings>\r\n");
	foreach(TypedViewDefinition typedView in _currentProject.TypedViews)
	{
		IDBView target = typedView.TargetView;

		__outputWriter.Write("				<TypedViewMapping TypedViewName=\":");
__outputWriter.Write(typedView.Name);
		__outputWriter.Write("\" TargetName=\"");
__outputWriter.Write(System.Security.SecurityElement.Escape(target.ContainingSchema.ParentCatalog.CatalogName));
		__outputWriter.Write(":");
__outputWriter.Write(System.Security.SecurityElement.Escape(target.ContainingSchema.SchemaOwner));
		__outputWriter.Write(":");
__outputWriter.Write(System.Security.SecurityElement.Escape(target.ViewName));
		__outputWriter.Write("\">\r\n					<FieldMappings>\r\n");
		foreach(TypedViewFieldDefinition field in typedView.Fields)
		{

		__outputWriter.Write("						<FieldMapping FieldName=\"");
__outputWriter.Write(field.FieldName);
		__outputWriter.Write("\" TargetFieldName=\"");
__outputWriter.Write(System.Security.SecurityElement.Escape(field.MappedField.FieldName));
		__outputWriter.Write("\" ");
if(field.TypeConverter!=null) {
		__outputWriter.Write(" TypeConverterToUse=\"");
__outputWriter.Write(field.TypeConverterTypeName);
		__outputWriter.Write("\" ");
}
		__outputWriter.Write(" />\r\n");
		}

		__outputWriter.Write("					</FieldMappings>\r\n				</TypedViewMapping>\r\n");
	}

		__outputWriter.Write("			</TypedViewMappings>\r\n			<SPCallMappings>\r\n");
	// re-use the list of spcalls we constructed when generating definitions for spcalls
	foreach(SPCallDefinition spCall in spCalls)
	{
		IDBStoredProcedure target = spCall.TargetStoredProcedure;
		string uniqueSPCallName = ProduceUniqueSPCallName(spCall);

		__outputWriter.Write("				<SPCallMapping SPCallName=\":");
__outputWriter.Write(uniqueSPCallName);
		__outputWriter.Write("\" TargetName=\"");
__outputWriter.Write(System.Security.SecurityElement.Escape(target.ContainingSchema.ParentCatalog.CatalogName));
		__outputWriter.Write(":");
__outputWriter.Write(System.Security.SecurityElement.Escape(target.ContainingSchema.SchemaOwner));
		__outputWriter.Write(":");
__outputWriter.Write(System.Security.SecurityElement.Escape(target.StoredProcedureName));
		__outputWriter.Write("\">\r\n					<FieldMappings>\r\n");
		foreach(SPCallParameterDefinition parameter in spCall.Parameters)
		{

		__outputWriter.Write("						<FieldMapping FieldName=\"");
__outputWriter.Write(parameter.ParameterName);
		__outputWriter.Write("\" TargetFieldName=\"");
__outputWriter.Write(System.Security.SecurityElement.Escape(parameter.RelatedParameter.ParameterName));
		__outputWriter.Write("\"/>\r\n");
		}

		__outputWriter.Write("					</FieldMappings>\r\n				</SPCallMapping>\r\n");
	}

		__outputWriter.Write("			</SPCallMappings>\r\n			<TypeConversionDefinitions>\r\n");
	foreach(TypeConversionDefinition definition in _currentProject.TypeConversionDefinitions)
	{

		__outputWriter.Write("				<TypeConversionDefinition TypeConverter=\"");
__outputWriter.Write(definition.ConverterToUse.TypeFullName);
		__outputWriter.Write("\" DbDotNetType=\"");
__outputWriter.Write(GetFullTypeName(definition.DotNetTypeToConvert));
		__outputWriter.Write("\" ");
 if(definition.LengthSet) {
		__outputWriter.Write(" FilterLength=\"");
__outputWriter.Write(definition.Length);
		__outputWriter.Write("\" ");
 } if(definition.DbTypeSet) {
		__outputWriter.Write(" FilterDbType=\"");
__outputWriter.Write(definition.DbType);
		__outputWriter.Write("\" ");
} if(definition.PrecisionSet) {
		__outputWriter.Write(" FilterPrecision=\"");
__outputWriter.Write(definition.Precision);
		__outputWriter.Write("\" ");
} if(definition.ScaleSet) {
		__outputWriter.Write(" FilterScale=\"");
__outputWriter.Write(definition.Scale);
		__outputWriter.Write("\" ");
}
		__outputWriter.Write("/>\r\n");
	}

		__outputWriter.Write("			</TypeConversionDefinitions>\r\n		</MappingStore>\r\n	</MappingStores>\r\n</Project>\r\n\r\n");
		__outputWriter.Write("\r\n");
	}


	public void ___RUN(IGenerator executingGenerator, Dictionary<string, TaskParameter> parameters, StreamWriter outputWriter, object activeObject) {
		__outputWriter = outputWriter; _parameters = parameters; _executingGenerator=executingGenerator; _activeObject = activeObject;
		__ScriptCode();
	}

}
