using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.Footprint.Web.Lib.Test.Web.Lib
{
    [TestClass]
    public class FootprintCombinedRegionTest : FootprintTestBase
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testcontext)
        {
            InitializeDatabase();
        }

        private void PlotCombinedRegion(Footprint footprint, int i)
        {
            var p = new Spherical.Visualizer.StereographicProjection();
            var r = new Spherical.Visualizer.RegionsLayer();
            r.DataSource = new Spherical.Visualizer.ObjectListDataSource(new[] { footprint.CombinedRegion.Region });
            Spherical.Visualizer.VisualizerTestBase.PlotZoomedTest(p, r, "_" + footprint.CombinationMethod.ToString() + "_" + i.ToString());
        }

        #region Refresh combined region tests

        /// <summary>
        /// Tests union and intersection calculation for combined regions
        /// </summary>
        /// <param name="method">Union or intersection</param>
        /// <param name="run1">Regions to be combined in a first, all-in-one run</param>
        /// <param name="run2">Regions to be combined in a second, one-by-one run</param>
        /// <param name="convexCount">Ground truth, the number of convexes after each refresh step</param>
        private void RefreshCombinedRegionTestHelper(CombinationMethod method, int run1, int run2, int[] convexCount)
        {
            using (var context = CreateContext())
            {
                int q = 0;
                var region = new FootprintRegion[run1 + run2];

                var footprint = new Footprint(context)
                {
                    Name = GetTestUniqueName() + "_" + method.ToString(),
                    CombinationMethod = method,
                };
                footprint.Save();

                // Add regions one by one, combine them in one shot

                for (int i = 0; i < run1; i++)
                {
                    region[i] = new FootprintRegion(footprint)
                    {
                        Name = "region" + i.ToString(),
                        Region = MakeTestCircle(i)
                    };
                    region[i].Save();
                    region[i].SaveRegion();
                }

                footprint.RefreshCombinedRegion();
                PlotCombinedRegion(footprint, q);
                Assert.AreEqual(convexCount[q++], footprint.CombinedRegion.Region.ConvexList.Count);

                // Add regions one by one, combine after each addition

                for (int i = run1; i < run1 + run2; i++)
                {
                    region[i] = new FootprintRegion(footprint)
                    {
                        Name = "region" + (i).ToString(),
                        Region = MakeTestCircle(i)
                    };
                    region[i].Save();
                    region[i].SaveRegion();

                    footprint.RefreshCombinedRegion();
                    PlotCombinedRegion(footprint, q);
                    Assert.AreEqual(convexCount[q++], footprint.CombinedRegion.Region.ConvexList.Count);
                }

                // Remove region one by one, refresh after each removal

                for (int i = run1 + run2 - 1; i >= 0; i--)
                {
                    region[i].Delete();

                    footprint.RefreshCombinedRegion();

                    if (i > 0)
                    {
                        PlotCombinedRegion(footprint, q);
                        Assert.AreEqual(convexCount[q++], footprint.CombinedRegion.Region.ConvexList.Count);
                    }
                    else
                    {
                        Assert.AreEqual(0, footprint.CombinedRegionId);
                    }
                }
            }
        }

        [TestMethod]
        public void RefreshCombinedRegionTest1()
        {
            // Single region
            RefreshCombinedRegionTestHelper(CombinationMethod.Union, 1, 0, new int[] { 1 });
            RefreshCombinedRegionTestHelper(CombinationMethod.Intersection, 1, 0, new int[] { 1 });
        }

        [TestMethod]
        public void RefreshCombinedRegionTest2()
        {
            // Two regions at once
            RefreshCombinedRegionTestHelper(CombinationMethod.Union, 2, 0, new int[] { 2, 1 });
            RefreshCombinedRegionTestHelper(CombinationMethod.Intersection, 2, 0, new int[] { 1, 1 });
        }
        
        [TestMethod]
        public void RefreshCombinedRegionTest3()
        {
            // Three regions, one by one
            RefreshCombinedRegionTestHelper(CombinationMethod.Union, 1, 2, new int[] { 1, 2, 3, 2, 1 });
            RefreshCombinedRegionTestHelper(CombinationMethod.Intersection, 1, 2, new int[] { 1, 1, 1, 1, 1 });
        }

        [TestMethod]
        public void RefreshCombinedRegionTest4()
        {
            // Two regions at once, then one more
            RefreshCombinedRegionTestHelper(CombinationMethod.Union, 2, 1, new int[] { 2, 3, 2, 1 });
            RefreshCombinedRegionTestHelper(CombinationMethod.Intersection, 2, 1, new int[] { 1, 1, 1, 1 });
        }

        #endregion
        #region Update combined regions test

        /// <summary>
        /// Tests union and intersection calculation for combined regions
        /// </summary>
        /// <param name="method">Union or intersection</param>
        /// <param name="run">Number of regions to be combined</param>
        /// <param name="convexCount">Ground truth, the number of convexes after each update step</param>
        private void UpdateCombinedRegionTestHelper(CombinationMethod method, int run, int[] convexCount)
        {
            using (var context = CreateContext())
            {
                int q = 0;
                var region = new FootprintRegion[run];

                var footprint = new Footprint(context)
                {
                    Name = GetTestUniqueName() + "_" + method.ToString(),
                    CombinationMethod = method,
                };
                footprint.Save();

                // Add regions one by one, combine them in one shot

                for (int i = 0; i < run; i++)
                {
                    region[i] = new FootprintRegion(footprint)
                    {
                        Name = "region" + i.ToString(),
                        Region = MakeTestCircle(i)
                    };
                    region[i].Save();
                    region[i].SaveRegion();

                    footprint.UpdateCombinedRegion(region[i]);
                    PlotCombinedRegion(footprint, q);
                    Assert.AreEqual(convexCount[q++], footprint.CombinedRegion.Region.ConvexList.Count);
                }
            }
        }

        [TestMethod]
        public void UpdateCombinedRegionTest()
        {
            // Two regions at once, then one more
            UpdateCombinedRegionTestHelper(CombinationMethod.Union, 3, new int[] { 1, 2, 3 });
            UpdateCombinedRegionTestHelper(CombinationMethod.Intersection, 3, new int[] { 1, 1, 1 });
        }

        #endregion
    }
}
