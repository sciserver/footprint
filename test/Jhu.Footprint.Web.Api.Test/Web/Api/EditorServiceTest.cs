using System;
using System.Linq;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Web.Services;
using Jhu.Graywulf.Web.Services.CodeGen;

namespace Jhu.Footprint.Web.Api.V1
{
    [TestClass]
    public class EditorServiceTest : EditorApiTestBase
    {
        #region Proxy generator tests

        [TestMethod]
        public void GenerateJavascriptTest()
        {
            var r = new RestServiceReflector();
            r.ReflectServiceContract(typeof(IEditorService), "http://localhost/EditorService.svc");
            var cg = new SwaggerJsonGenerator(r.Api);
            var code = cg.Execute();
        }

        [TestMethod]
        public void GenerateSwaggerJsonTest()
        {
            var r = new RestServiceReflector();
            r.ReflectServiceContract(typeof(IEditorService), "http://localhost/EditorService.svc");
            var cg = new SwaggerJsonGenerator(r.Api);
            var code = cg.Execute();
        }

        #endregion
        #region Footprint CRUD operations

        [TestMethod]
        public void GetFootprintTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);

                client.ModifyFootprint(new FootprintRequest(new Footprint() { CombinationMethod = Lib.CombinationMethod.Union }));

                client.CreateRegion("test1", new RegionRequest(TestRegion1));
                client.CreateRegion("test2", new RegionRequest(TestRegion1));

                var res = client.GetFootprint();
                Assert.AreEqual("new_footprint", res.Footprint.Name);
                Assert.AreEqual(Lib.CombinationMethod.Union, res.Footprint.CombinationMethod);
            }
        }

        [TestMethod]
        public void GetEmptyFootprintTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var res = client.GetFootprint();
                Assert.AreEqual("new_footprint", res.Footprint.Name);
            }
        }

        [TestMethod]
        public void ModifyEmptyFootprintTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);

                client.ModifyFootprint(
                    new FootprintRequest()
                    {
                        Footprint = new Footprint()
                        {
                            Name = "test"
                        }
                    });

                var res = client.GetFootprint();
                Assert.AreEqual("test", res.Footprint.Name);
            }
        }

        [TestMethod]
        public void DeleteFootprintTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);

                client.ModifyFootprint(new FootprintRequest(new Footprint() { CombinationMethod = Lib.CombinationMethod.Union }));
                client.CreateRegion("test1", new RegionRequest(TestRegion1));
                client.CreateRegion("test2", new RegionRequest(TestRegion2));

                var r1 = client.ListRegions(null, null, null);
                Assert.AreEqual(2, r1.Regions.Count());

                client.DeleteFootprint();

                var r2 = client.GetFootprint();
                Assert.AreEqual("new_footprint", r2.Footprint.Name);

                var r3 = client.ListRegions(null, null, null);
                Assert.AreEqual(0, r3.Regions.Count());
            }
        }

        [TestMethod]
        public void DeleteEmptyFootprintTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);

                client.ModifyFootprint(
                    new FootprintRequest()
                    {
                        Footprint = new Footprint()
                        {
                            Name = "test"
                        }
                    });

                var res = client.GetFootprint();
                Assert.AreEqual("test", res.Footprint.Name);

                client.DeleteFootprint();

                res = client.GetFootprint();
                Assert.AreEqual("new_footprint", res.Footprint.Name);
            }
        }

        [TestMethod]
        public void DownloadFootprintAsStringTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);

                client.ModifyFootprint(new FootprintRequest(new Footprint() { CombinationMethod = Lib.CombinationMethod.Union }));
                client.CreateRegion("test1", new RegionRequest(TestRegion1));
                client.CreateRegion("test2", new RegionRequest(TestRegion2));

                var url = EditorApiBaseUrl + "/footprint/raw";
                var buffer = session.HttpGet(url, "text/plain");
                var region = System.Text.Encoding.UTF8.GetString(buffer).Trim();

                var gt =
@"REGION
    CONVEX
        0 0 1 0
        0 1 0 0
        0.17364817766693055 -0.984807753012208 0 0
        0 0 -1 -0.17364817766693033

    CONVEX
        0.9698463103929541 0.17101007166283433 0.17364817766693033 0.99619469809174555
        -0.17364817766693055 0.984807753012208 0 0

    CONVEX
        0.9698463103929541 0.17101007166283433 0.17364817766693033 0.99619469809174555
        0 0 1 0.17364817766693033
        0.17364817766693055 -0.984807753012208 0 0";

                Assert.AreEqual(gt, region);
            }
        }

        [TestMethod]
        public void DownloadEmptyFootprintAsStringTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var url = EditorApiBaseUrl + "/footprint/raw";
                var buffer = session.HttpGet(url, "text/plain");
                var region = System.Text.Encoding.UTF8.GetString(buffer).Trim();

                Assert.AreEqual("REGION", region);
            }
        }

        [TestMethod]
        public void DownloadEmptyFootprintOutlineAsStringTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var url = EditorApiBaseUrl + "/footprint/outline/raw";
                var buffer = session.HttpGet(url, "text/plain");
                var region = System.Text.Encoding.UTF8.GetString(buffer).Trim();

                Assert.AreEqual("OUTLINE", region);
            }
        }

        [TestMethod]
        public void GetEmptyFootprintOutlinePointsTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var res = client.GetFootprintOutlinePoints(CoordinateSystem.EqJ2000, CoordinateRepresentation.Sexa, null, null, null);
            }
        }

        [TestMethod]
        public void PlotEmptyFootprintTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var url = EditorApiBaseUrl + "/footprint/plot";
                var buffer = session.HttpGet(url, "image/png");

                Assert.IsTrue(buffer.Length > 0);
            }
        }

        [TestMethod]
        public void PlotEmptyFootprintAdvanced()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var url = EditorApiBaseUrl + "/footprint/plot";
                var plot = new PlotRequest()
                {
                    Plot = new Plot()
                };
                var buffer = session.HttpPost(url, "image/png", plot);

                Assert.IsTrue(buffer.Length > 0);
            }
        }

        #endregion
        #region Region CRUD operations

        [TestMethod]
        public void CreateRegionTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var name = GetTestUniqueName();
                var req = new RegionRequest()
                {
                    Region = TestRegion1,
                };
                var res = client.CreateRegion(name, req);

                Assert.IsTrue(res.Region.Area > 0);
            }
        }

        [TestMethod]
        public void CreateRegionWithCoordinateSystemTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var name = GetTestUniqueName();
                var req = new RegionRequest()
                {
                    Region = TestRegion1,
                    CoordinateSystem = CoordinateSystem.GalJ2000
                };
                var res = client.CreateRegion(name, req);

                Assert.IsTrue(res.Region.Area > 0);
            }
        }

        [TestMethod]
        public void CreateRegionWithRotationTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var name = GetTestUniqueName();
                var req = new RegionRequest()
                {
                    Region = TestRegion1,
                    Rotation = TestRotation
                };
                var res = client.CreateRegion(name, req);

                Assert.IsTrue(res.Region.Area > 0);
            }
        }

        [TestMethod]
        public void GetRegionTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var name = GetTestUniqueName();
                var req = new RegionRequest()
                {
                    Region = TestRegion1,
                    CoordinateSystem = CoordinateSystem.GalJ2000,
                    Rotation = TestRotation
                };
                var r1 = client.CreateRegion(name, req);

                var r2 = client.GetRegion(name);

                Assert.AreEqual(r1.Region.Area, r2.Region.Area);
            }
        }

        [TestMethod]
        public void ModifyRegionTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var name = GetTestUniqueName();
                var req = new RegionRequest()
                {
                    Region = TestRegion1,
                    CoordinateSystem = CoordinateSystem.GalJ2000,
                    Rotation = TestRotation
                };

                var r1 = client.CreateRegion(name, req);

                req = new RegionRequest()
                {
                    Region = TestRegion1,
                };

                var r2 = client.ModifyRegion(name, req);

                Assert.AreEqual(r1.Region.Area, r2.Region.Area);
            }
        }

        [TestMethod]
        public void ModifyRegionWithRenameTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var name = GetTestUniqueName();
                var req = new RegionRequest()
                {
                    Region = TestRegion1,
                    CoordinateSystem = CoordinateSystem.GalJ2000,
                    Rotation = TestRotation
                };

                var r1 = client.CreateRegion(name + "_old", req);

                req = new RegionRequest()
                {
                    Region = new Region()
                    {
                        Name = name + "_new"
                    }
                };

                var r2 = client.ModifyRegion(name + "_old", req);

                var r3 = client.GetRegion(name + "_new");

                Assert.AreEqual(r1.Region.Area, r3.Region.Area);
            }
        }

        [TestMethod]
        public void DeleteRegionTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var name = GetTestUniqueName();
                var req = new RegionRequest()
                {
                    Region = TestRegion1,
                    CoordinateSystem = CoordinateSystem.GalJ2000,
                    Rotation = TestRotation
                };

                var r1 = client.CreateRegion(name, req);

                var r2 = client.ListRegions(null, null, null);
                Assert.AreEqual(1, r2.Regions.Count());

                client.DeleteRegion(name);

                var r3 = client.ListRegions(null, null, null);
                Assert.AreEqual(0, r3.Regions.Count());
            }
        }

        [TestMethod]
        public void DeleteRegionsByNameTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var name = GetTestUniqueName();

                for (int i = 0; i < 5; i++)
                {
                    var req = new RegionRequest()
                    {
                        Region = TestRegion1,
                    };
                    client.CreateRegion(name + "_" + i.ToString(), req);
                }

                var names = String.Join(",", name + "_" + 2.ToString(), name + "_" + 3.ToString());
                client.DeleteRegions(names);

                var res = client.ListRegions(null, null, null);

                Assert.AreEqual(3, res.Regions.Count());
            }
        }

        [TestMethod]
        public void ListAllRegionsTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);

                for (int i = 0; i < 5; i++)
                {
                    var name = GetTestUniqueName() + "_" + i.ToString();
                    var req = new RegionRequest()
                    {
                        Region = TestRegion1,
                    };
                    client.CreateRegion(name, req);
                }

                var res = client.ListRegions(null, null, null);

                Assert.AreEqual(5, res.Regions.Count());
            }
        }

        [TestMethod]
        public void ListRegionsByNameTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var name = GetTestUniqueName();

                for (int i = 0; i < 5; i++)
                {
                    var req = new RegionRequest()
                    {
                        Region = TestRegion1,
                    };
                    client.CreateRegion(name + "_" + i.ToString(), req);
                }

                var names = String.Join(",", name + "_" + 2.ToString(), name + "_" + 3.ToString());
                var res = client.ListRegions(names, null, null);

                Assert.AreEqual(2, res.Regions.Count());
            }
        }

        [TestMethod]
        public void ListRegionsWithPatternTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);

                for (int i = 0; i < 11; i++)
                {
                    var name = GetTestUniqueName() + "_" + i.ToString();
                    var req = new RegionRequest()
                    {
                        Region = TestRegion1,
                    };
                    client.CreateRegion(name, req);
                }

                var res = client.ListRegions("*0", null, null);

                Assert.AreEqual(2, res.Regions.Count());
            }
        }

        [TestMethod]
        public void ListRegionsWithPagingTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);

                for (int i = 0; i < 10; i++)
                {
                    var name = GetTestUniqueName() + "_" + i.ToString();
                    var req = new RegionRequest()
                    {
                        Region = TestRegion1,
                    };
                    client.CreateRegion(name, req);
                }

                var res = client.ListRegions(null, null, null);
                Assert.AreEqual(10, res.Regions.Count());

                res = client.ListRegions(null, 0, 3);
                Assert.AreEqual(3, res.Regions.Count());

                res = client.ListRegions(null, 4, 5);
                Assert.AreEqual(5, res.Regions.Count());

                res = client.ListRegions(null, 5, 5);
                Assert.AreEqual(5, res.Regions.Count());

                res = client.ListRegions(null, 8, 5);
                Assert.AreEqual(2, res.Regions.Count());

                // TODO: verify paging links once implemented
            }
        }

        [TestMethod]
        public void DownloadRegionAsStringTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);

                var name = GetTestUniqueName();
                var url = EditorApiBaseUrl + "/footprint/regions/" + name + "/raw";

                var req = new RegionRequest()
                {
                    Region = TestRegion1,
                };
                client.CreateRegion(name, req);


                var buffer = session.HttpGet(url, "text/plain");
                var region = System.Text.Encoding.UTF8.GetString(buffer).Trim();

                var gt =
@"REGION
    CONVEX
        0 0 1 0
        0 1 0 0
        0.17364817766693055 -0.984807753012208 0 0
        0 0 -1 -0.17364817766693033";

                Assert.AreEqual(gt, region);
            }
        }

        [TestMethod]
        public void GetRegionOutlinePointsTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var name = GetTestUniqueName();

                var req = new RegionRequest()
                {
                    Region = TestRegion1,
                };
                client.CreateRegion(name, req);

                var res = client.GetRegionOutlinePoints(name, null, null, null, null, null);

                Assert.AreEqual(1003, res.Count());
            }
        }

        // TODO: test reduction methods

        [TestMethod]
        public void UploadRegionAsStringTest()
        {
            // TODO: fix formatter

            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);

                var name = GetTestUniqueName();
                var url = EditorApiBaseUrl + "/footprint/regions/" + name + "/raw";

                var buffer = System.Text.Encoding.UTF8.GetBytes("CIRCLE J2000 10 10 10");
                session.HttpPut(url, "*/*", "text/plain", buffer);

                var r = client.GetRegion(name);
                Assert.IsTrue(r.Region.Area > 0);
            }
        }

        [TestMethod]
        public void DownloadRegionOutlineAsStringTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);

                var name = GetTestUniqueName();
                var url = EditorApiBaseUrl + "/footprint/regions/" + name + "/outline/raw";

                var req = new RegionRequest()
                {
                    Region = TestRegion1,
                };
                client.CreateRegion(name, req);

                var buffer = session.HttpGet(url, "text/plain");
                var region = System.Text.Encoding.UTF8.GetString(buffer).Trim();

                var gt =
@"OUTLINE
    LOOP
        ARC 0 0 1 0 1 0 0 0.984807753012208 0.17364817766693055 0
        ARC 0.17364817766693055 -0.984807753012208 0 0 0.984807753012208 0.17364817766693055 0 0.9698463103929541 0.17101007166283455 0.17364817766693033
        ARC 0 0 -1 -0.17364817766693033 0.9698463103929541 0.17101007166283455 0.17364817766693033 0.98480775301220813 0 0.17364817766693033
        ARC 0 1 0 0 0.98480775301220813 0 0.17364817766693033 1 0 0";

                Assert.AreEqual(gt, region);
            }
        }

        [TestMethod]
        public void PlotRegionTest()
        {
            using (var session = new RestClientSession())
            {
                var name = GetTestUniqueName();
                var client = CreateClient(session, TestUser);

                var req = new RegionRequest()
                {
                    Region = TestRegion1,
                };
                client.CreateRegion(name, req);

                var url = EditorApiBaseUrl + "/footprint/regions/" + name + "/plot";
                var buffer = session.HttpGet(url, "image/png");

                Assert.IsTrue(buffer.Length > 0);
            }
        }

        [TestMethod]
        public void PlotAllRegionsTest()
        {
            using (var session = new RestClientSession())
            {
                var name = GetTestUniqueName();
                var client = CreateClient(session, TestUser);

                var req = new RegionRequest()
                {
                    Region = TestRegion1,
                };
                client.CreateRegion(name, req);

                var url = EditorApiBaseUrl + "/footprint/regions/*/plot";
                var buffer = session.HttpGet(url, "image/png");

                Assert.IsTrue(buffer.Length > 0);
            }
        }

        [TestMethod]
        public void PloRegionAdvancedTest()
        {
            using (var session = new RestClientSession())
            {
                var name = GetTestUniqueName();
                var client = CreateClient(session, TestUser);

                var req = new RegionRequest()
                {
                    Region = TestRegion1,
                };
                client.CreateRegion(name, req);

                var plot = new PlotRequest()
                {
                    Plot = new Plot()
                };

                var url = EditorApiBaseUrl + "/footprint/regions/" + name + "/plot";
                var buffer = session.HttpPost(url, "image/png", plot);

                Assert.IsTrue(buffer.Length > 0);
            }
        }

        #endregion

#if false

        [TestMethod]
        public void UnionTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var r1 = GetTestRegion();
                client.New(r1);
                var r2 = GetTestRegion("CIRCLE J2000 15 10 15");
                client.Union(r2);
                var res = client.GetShape();
                Assert.AreEqual(2, res.ConvexList.Count);
            }
        }

        [TestMethod]
        public void IntersectTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var r1 = GetTestRegion();
                client.New(r1);
                var r2 = GetTestRegion("CIRCLE J2000 15 10 15");
                client.Intersect(r2);
                var res = client.GetShape();
                Assert.IsTrue(res.ConvexList.Count >= 1);
            }

        }

        [TestMethod]
        public void SubtractTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var r1 = GetTestRegion();
                client.New(r1);
                var r2 = GetTestRegion("CIRCLE J2000 15 10 15");
                client.Subtract(r2);
                var res = client.GetShape();
                Assert.IsTrue(res.ConvexList.Count >= 1);
            }

        }

        [TestMethod]
        public void GrowTest()
        {
            using (var session = new RestClientSession())
            {
                var client = CreateClient(session, TestUser);
                var r1 = GetTestRegion();
                client.New(r1);
                client.Grow(10);
                var res = client.GetShape();

                Assert.IsTrue(res.Area > r1.Region.GetRegion().Area);
            }

        }

        [TestMethod]
        public void SaveLoadTest()
        {

            using (var session = new RestClientSession())
            {
                var name = GetTestUniqueName();
                var client = CreateClient(session, TestUser);
                var r1 = GetTestRegion();
                client.New(r1);
                client.Save(TestUser, name, name, "");

                client.Load(TestUser, name, name);

                var res = client.GetShape();
                Assert.AreEqual(res.ToString(), r1.Region.GetRegion().ToString());
            }
        }

        [TestMethod]
        public void GetTestOutline()
        {
            using (var session = new RestClientSession())
            {
                var name = GetTestUniqueName();
                var client = CreateClient(session, TestUser);
                var r1 = GetTestRegion();
                client.New(r1);
                var url = EditorApiBaseUrl + "/outline";
                var buffer = session.HttpGet(url);
                Assert.IsTrue(buffer != null && buffer.Length > 0);
            }
        }

        [TestMethod]
        public void PlotTestFootprintRegionAdvanced()
        {
            using (var session = new RestClientSession())
            {
                var name = GetTestUniqueName();
                var client = CreateClient(session, TestUser);
                var r1 = GetTestRegion();
                client.New(r1);
                var url = EditorApiBaseUrl + "/plot";
                var json = "{ \"ra\": 10, \"dec\": 10 }";
                var data = System.Text.ASCIIEncoding.Default.GetBytes(json);
                var buffer = session.HttpPost(url, "image/png", "application/json", data);
                Assert.IsTrue(buffer != null && buffer.Length > 0);
            }
        }
#endif
    }
}
