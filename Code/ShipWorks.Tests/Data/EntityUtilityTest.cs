using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Interapptive.Shared.Collections;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using Xunit;

namespace ShipWorks.Tests.Data
{
    public class EntityUtilityTest
    {
        [Fact]
        public void FindRelationChain_ReturnsCorrectValues_WhenAllowOneToMany_IsTrue()
        {
            var first = EntityUtility.FindRelationChain(EntityType.ShipmentEntity, EntityType.OrderItemEntity, true);
            var second = EntityUtility.FindRelationChain(EntityType.ShipmentEntity, EntityType.OrderItemEntity, true);

            var firstStrings = new List<string>();
            foreach (IRelation relation in first)
            {
                firstStrings.Add(relation.ToString());
            }

            var secondStrings = new List<string>();
            foreach (IRelation relation in second)
            {
                secondStrings.Add(relation.ToString());
            }

            Assert.True(firstStrings.Except(secondStrings).None());
        }

        [Fact]
        public void FindRelationChain_ReturnsCorrectValues_WhenNotKnownRelations()
        {
            var first = EntityUtility.FindRelationChain(EntityType.StoreEntity, EntityType.OrderItemEntity, true);
            var second = EntityUtility.FindRelationChain(EntityType.StoreEntity, EntityType.OrderItemEntity, true);

            var firstStrings = new List<string>();
            foreach (IRelation relation in first)
            {
                firstStrings.Add(relation.ToString());
            }

            var secondStrings = new List<string>();
            foreach (IRelation relation in second)
            {
                secondStrings.Add(relation.ToString());
            }

            Assert.True(firstStrings.Except(secondStrings).None());
        }

        [Fact]
        public void FindRelationChain_ReturnsNull_WhenAllowOneToMany_IsFalse()
        {
            var first = EntityUtility.FindRelationChain(EntityType.ShipmentEntity, EntityType.OrderItemEntity, false);

            var second = EntityUtility.FindRelationChain(EntityType.ShipmentEntity, EntityType.OrderItemEntity, false);

            Assert.Null(first);
            Assert.Null(second);
        }

        /// <summary>
        /// This was just to test caching vs no caching, but leaving it in case we want to use in the future.  But,
        /// not leaving the Fact attribute so that it is no run with the other unit tests.
        /// </summary>
        //[Fact]
        public void FindRelationChain_Timing_And_ReturnsCorrectValues_ForAllCombinations()
        {
            List<EntityType> entityTypes = new List<EntityType>()
            {
                EntityType.CustomerEntity,
                EntityType.StoreEntity,
                EntityType.OrderEntity,
                EntityType.OrderItemEntity,
                EntityType.ShipmentEntity,
                EntityType.UpsShipmentEntity,
                EntityType.UpsPackageEntity,
                EntityType.FedExShipmentEntity,
                EntityType.FedExPackageEntity,
                EntityType.NoteEntity
            };

            var errors = new List<string>();
            var fromEntityTypes = entityTypes.ToList();
            fromEntityTypes.AddRange(entityTypes);
            fromEntityTypes.AddRange(entityTypes);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            foreach (var from in fromEntityTypes)
            {
                foreach (var to in entityTypes)
                {
                    try
                    {
                        var first = EntityUtility.FindRelationChain(from, to, true);
                        var second = EntityUtility.FindRelationChain(from, to, true);

                        var firstStrings = new List<string>();
                        if (first != null)
                        {
                            foreach (IRelation relation in first)
                            {
                                firstStrings.Add(relation.ToString());
                            }
                        }

                        var secondStrings = new List<string>();
                        if (second != null)
                        {
                            foreach (IRelation relation in second)
                            {
                                secondStrings.Add(relation.ToString());
                            }
                        }

                        Assert.True(firstStrings.Except(secondStrings).None());
                    }
                    catch
                    {
                        string err = $"from:{from}, to:{to}";
                        Debug.WriteLine(err);
                        errors.Add(err);
                    }
                }
            }
            sw.Stop();

            errors.ForEach(s => Debug.WriteLine(s));
            Debug.WriteLine("Time to run: " + sw.ElapsedMilliseconds);
        }
    }
}
