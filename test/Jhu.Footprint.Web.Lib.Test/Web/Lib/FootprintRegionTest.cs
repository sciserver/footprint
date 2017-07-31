using System;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;
using Jhu.Graywulf.AccessControl;

namespace Jhu.Footprint.Web.Lib
{
    [TestClass]
    public class FootprintRegionTest : FootprintTestBase
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            InitializeDatabase();
        }

        [TestMethod]
        public void CreateRegionTest()
        {
            int footprintid;

            using (var context = CreateContext())
            {
                var footprint = new Footprint(context)
                {
                    Name = "CreateRegionTest",
                };

                footprintid = (int)footprint.Save();

                var region = new FootprintRegion(footprint)
                {
                    Name = "CreateRegionTest",
                    Type = RegionType.Single,
                    Region = Spherical.Region.Parse("CIRCLE J2000 10 10 10", true, true),
                };

                footprint.Save();
                region.Save();
                region.SaveRegion();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateNameException))]
        public void DuplicateRegionNameTest()
        {
            var name = GetTestUniqueName();

            using (var context = CreateContext())
            {
                var footprint = CreateTestFootprint(context, name);
                CreateTestRegion(footprint, name);
                CreateTestRegion(footprint, name);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AccessDeniedException))]
        public void AccessDeniedCreateRegionTest()
        {
            int regionId;
            int footprintid;

            using (var context = CreateContext())
            {
                var region = CreateTestFootprintAndRegion(context, "AccessDeniedCreateRegionTest");
                regionId = region.Id;
                footprintid = region.FootprintId;

                // grant access to other identity

                var footprint = new Footprint(context);
                footprint.Load(footprintid);
                footprint.Permissions.Grant(CreateOtherPrincipal().Identity.Name, DefaultAccess.Read);
                footprint.Save();
            }

            using (var context = CreateContext())
            {
                context.Principal = CreateOtherPrincipal();

                var footprint = new Footprint(context);
                footprint.Load(footprintid);

                var region = new FootprintRegion(footprint)
                {
                    FootprintId = footprintid,
                    Id = regionId
                };

                region.Load();

                // Now try to modify, this should fail

                region.Save();
            }
        }

        private void UpdateCombinedRegionHelper2(string name, CombinationMethod method)
        {
            using (var context = CreateContext())
            {
                var footprint = CreateTestFootprint(context, name, method);
                var r1 = CreateTestRegion(footprint, name + "_1");

                // reload and see if combined region is correct
                footprint.Load();
                Assert.AreEqual(r1.Id, footprint.CombinedRegionId);

                var r2 = CreateTestRegion(footprint, name + "_2");

                // reload and see if combined region is correct
                footprint.Load();
                Assert.AreNotEqual(0, footprint.CombinedRegionId);
                Assert.AreNotEqual(r1.Id, footprint.CombinedRegionId);
                Assert.AreNotEqual(r2.Id, footprint.CombinedRegionId);
            }
        }

        private void UpdateCombinedRegionHelper3(string name, CombinationMethod method)
        {
            using (var context = CreateContext())
            {
                var footprint = CreateTestFootprint(context, name, method);
                var r1 = CreateTestRegion(footprint, name + "_1");

                // reload and see if combined region is correct
                footprint.Load();
                Assert.AreEqual(r1.Id, footprint.CombinedRegionId);

                var r2 = CreateTestRegion(footprint, name + "_2");
                var r3 = CreateTestRegion(footprint, name + "_3");

                // reload and see if combined region is correct
                footprint.Load();
                Assert.AreNotEqual(0, footprint.CombinedRegionId);
                Assert.AreNotEqual(r1.Id, footprint.CombinedRegionId);
                Assert.AreNotEqual(r2.Id, footprint.CombinedRegionId);
                Assert.AreNotEqual(r3.Id, footprint.CombinedRegionId);
            }
        }

        [TestMethod]
        public void UpdateCombinedUnionRegion2Test()
        {
            var name = GetTestUniqueName();
            UpdateCombinedRegionHelper2(name, CombinationMethod.Union);
        }

        [TestMethod]
        public void UpdateCombinedIntersectRegion2Test()
        {
            var name = GetTestUniqueName();
            UpdateCombinedRegionHelper2(name, CombinationMethod.Intersection);
        }

        [TestMethod]
        public void UpdateCombinedUnionRegion3Test()
        {
            var name = GetTestUniqueName();
            UpdateCombinedRegionHelper3(name, CombinationMethod.Union);
        }

        [TestMethod]
        public void UpdateCombinedIntersectRegion3Test()
        {
            var name = GetTestUniqueName();
            UpdateCombinedRegionHelper3(name, CombinationMethod.Intersection);
        }

        private void RefreshCombinedRegionHelper2(string name, CombinationMethod method)
        {
            using (var context = CreateContext())
            {
                var footprint = CreateTestFootprint(context, name, method);
                var r1 = CreateTestRegion(footprint, name + "_1");
                var r2 = CreateTestRegion(footprint, name + "_2");

                // reload and see if combined region is correct
                footprint.Load();
                Assert.AreNotEqual(0, footprint.CombinedRegionId);
                Assert.AreNotEqual(r1.Id, footprint.CombinedRegionId);
                Assert.AreNotEqual(r2.Id, footprint.CombinedRegionId);

                // Now delete first region, this will force a reload

                r1.Delete();

                // reload and see if combined region is correct
                footprint.Load();

                // TODO: separate tests for union and intersect
            }
        }

        private void RefreshCombinedRegionHelper3(string name, CombinationMethod method)
        {
            using (var context = CreateContext())
            {
                var footprint = CreateTestFootprint(context, name, method);
                var r1 = CreateTestRegion(footprint, name + "_1");
                var r2 = CreateTestRegion(footprint, name + "_2");
                var r3 = CreateTestRegion(footprint, name + "_3");

                // reload and see if combined region is correct
                footprint.Load();
                Assert.AreNotEqual(0, footprint.CombinedRegionId);
                Assert.AreNotEqual(r1.Id, footprint.CombinedRegionId);
                Assert.AreNotEqual(r2.Id, footprint.CombinedRegionId);

                // Now delete first region, this will force a reload

                r1.Delete();

                // reload and see if combined region is correct
                footprint.Load();

                // TODO: separate tests for union and intersect
            }
        }

        [TestMethod]
        public void RefreshCombinedUnionRegion2Test()
        {
            var name = GetTestUniqueName();
            RefreshCombinedRegionHelper2(name, CombinationMethod.Union);
        }

        [TestMethod]
        public void RefreshCombinedIntersectionRegion2Test()
        {
            var name = GetTestUniqueName();
            RefreshCombinedRegionHelper2(name, CombinationMethod.Intersection);
        }

        [TestMethod]
        public void RefreshCombinedUnionRegion3Test()
        {
            var name = GetTestUniqueName();
            RefreshCombinedRegionHelper3(name, CombinationMethod.Union);
        }

        [TestMethod]
        public void RefreshCombinedIntersectionRegion3Test()
        {
            var name = GetTestUniqueName();
            RefreshCombinedRegionHelper3(name, CombinationMethod.Intersection);
        }

        #region Thumbnail and preview tests

        [TestMethod]
        public void CreateThumbnailTest()
        {
            var name = GetTestUniqueName();

            using (var context = CreateContext())
            {
                var footprint = CreateTestFootprint(context, name);
                var r = CreateTestRegion(footprint, name);

                r.CreateThumbnail();

            }
        }

        [TestMethod]
        public void LoadThumbnailTest()
        {
            var name = GetTestUniqueName();
            int footprintId;
            int regionId;
            byte[] t1;
            byte[] t2;

            using (var context = CreateContext())
            {
                //var footprint = CreateTestFootprint(context, name);
                var f = new Lib.Footprint(context)
                {
                    Name = name
                };
                footprintId = (int)f.Save();

                var r = new Lib.FootprintRegion(f)
                {
                    Name = name,
                    Region = Spherical.Region.Parse("CIRCLE J2000 10 10 10")
                };

                regionId = (int)r.Save();
                r.CreateThumbnail();
                t1 = r.ImageThumbnail;
            }

            using (var context = CreateContext())
            {
                var f = new Footprint(context);
                f.Load(footprintId);

                var r = new FootprintRegion(f)
                {
                    Id = regionId
                };

                r.Load(regionId);
                r.LoadThumbnail();
                t2 = r.ImageThumbnail;
            }
            t1.SequenceEqual(t2);

        }

        [TestMethod]
        public void CreatePreviewTest()
        {
            var name = GetTestUniqueName();

            using (var context = CreateContext())
            {
                var footprint = CreateTestFootprint(context, name);
                var r = CreateTestRegion(footprint, name);

                r.CreatePreview();

            }
        }

        [TestMethod]
        public void LoadPreviewTest()
        {
            var name = GetTestUniqueName();
            int footprintId;
            int regionId;
            byte[] t1;
            byte[] t2;

            using (var context = CreateContext())
            {
                //var footprint = CreateTestFootprint(context, name);
                var f = new Lib.Footprint(context)
                {
                    Name = name
                };
                footprintId = (int)f.Save();

                var r = new Lib.FootprintRegion(f)
                {
                    Name = name,
                    Region = Spherical.Region.Parse("CIRCLE J2000 10 10 10")
                };

                regionId = (int)r.Save();
                r.CreatePreview();
                t1 = r.ImagePreview;
            }

            using (var context = CreateContext())
            {
                var f = new Footprint(context);
                f.Load(footprintId);

                var r = new FootprintRegion(f)
                {
                    Id = regionId
                };

                r.Load(regionId);
                r.LoadPreview();
                t2 = r.ImagePreview;
            }

            t1.SequenceEqual(t2);
        }

        #endregion

    }
}
