//using System.Collections.Generic;
//using Newtonsoft.Json;

//namespace ShipWorks.OrderLookup.FieldManager
//{
//    /// <summary>
//    /// Section layout definition
//    /// </summary>
//    public interface ISectionLayout
//    {
//        /// <summary>
//        /// Name
//        /// </summary>
//        string Name { get; set; }

//        /// <summary>
//        /// Unique ID of the section
//        /// </summary>
//        string Id { get; set; }

//        /// <summary>
//        /// Is this section selected?
//        /// </summary>
//        bool Selected { get; set; }

//        /// <summary>
//        /// Is this section expanded?
//        /// </summary>
//        bool Expanded { get; set; }

//        /// <summary>
//        /// List of fields in this section
//        /// </summary>
//        List<ISectionFieldLayout> SectionFields { get; set; }

//        /// <summary>
//        /// Does the section have children
//        /// </summary>
//        bool HasChildren { get; }

//        /// <summary>
//        /// Copy the given SectionLayout to this instance.  SectionFields are NOT copied.
//        /// </summary>
//        void Copy(ISectionLayout toCopy);

//        /// <summary>
//        /// Create a clone of this SectionLayout
//        /// </summary>
//        ISectionLayout Clone();
//    }
//}