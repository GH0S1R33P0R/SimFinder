using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeniorProject;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;

namespace SeniorProjectTests
{
    [TestClass]
    public class UnitTests
    {
        /// <summary>
        /// Test Idempotency: C(xx) = C(x), and C(e) = 0, where 'e' is the empty string.
        /// </summary>
        [TestMethod]
        public void TestIdempotency()
        {
            ICompressible x = new MockEntity(Encoding.ASCII.GetBytes("Lorem Ipsum Dolor"));

            byte[] doubleArray = x.ToByteArray().Concat(x.ToByteArray()).ToArray();
            byte[] emptyArray = new byte[0];
            ICompressible xx = new MockEntity(doubleArray);
            ICompressible e = new MockEntity(emptyArray);

            ISimilarity simTest = new Similarity();

            Assert.IsTrue(simTest.GetComplexity(x) == simTest.GetComplexity(xx));
            Assert.IsTrue(simTest.GetComplexity(e) == 0);
        }

        /// <summary>
        /// Test Monotonicity: C(xy) >= C(x).
        /// </summary>
        [TestMethod]
        public void TestMonotonicity()
        {

            byte[] xData = Encoding.ASCII.GetBytes("Lorem Ipsum Dolor");
            byte[] appendingData = xData.Concat(Encoding.ASCII.GetBytes("sit amet, consectetur adipiscing elit")).ToArray();

            ICompressible xy = new MockEntity(appendingData);
            ICompressible x = new MockEntity(xData);

            ISimilarity simTest = new Similarity();

            Assert.IsTrue(simTest.GetComplexity(xy) >= simTest.GetComplexity(x));
        }

        /// <summary>
        /// Test Symmetry: C(xy) = C(yx).
        /// </summary>
        [TestMethod]
        public void TestSymmetry()
        {
            byte[] xData = Encoding.ASCII.GetBytes("Lorem ipsum dolor sit amet,");
            byte[] yData = Encoding.ASCII.GetBytes(" consectetur adipiscing elit.");

            ICompressible forward = new MockEntity(xData.Concat(yData).ToArray());
            ICompressible backward = new MockEntity(yData.Concat(xData).ToArray());

            ISimilarity simTest = new Similarity();

            Assert.IsTrue(simTest.GetComplexity(forward) == simTest.GetComplexity(backward));
        }

        /// <summary>
        /// Test Distributivity: C(xy) + C(z) <= C(xz) + C(yz).
        /// </summary>
        [TestMethod]
        public void TestDistributivity()
        {
            byte[] xData = Encoding.ASCII.GetBytes("Lorem ipsum dolor sit amet,");
            byte[] yData = Encoding.ASCII.GetBytes(" consectetur adipiscing elit.");
            byte[] zData = Encoding.ASCII.GetBytes("Suspendisse porttitor lectus");

            ICompressible xy = new MockEntity(xData.Concat(yData).ToArray());
            ICompressible xz = new MockEntity(xData.Concat(zData).ToArray());
            ICompressible z = new MockEntity(zData);
            ICompressible yz = new MockEntity(yData.Concat(zData).ToArray());

            ISimilarity simTest = new Similarity();

            Assert.IsTrue(simTest.GetComplexity(xy) + simTest.GetComplexity(z) <= simTest.GetComplexity(xz) + simTest.GetComplexity(yz));
        }

        /// <summary>
        /// Test if algorithm identifies known matches
        /// </summary>
        [TestMethod]
        public void TestDetection()
        {
            //Create the data set as a list of StringCompressible objects
            List<StringCompressible> DataSet = new List<StringCompressible>();

            //Create the golden data set
            DataSet.Add(new StringCompressible("IR-0026018", "Unable to start email connector after MR2 install"));
            DataSet.Add(new StringCompressible("IR-0026036", "When using SQL Scripts to Drop Incident Tables to wipe out test tickets - ON CG restart cg_IncidentRequest is not being recreated"));
            DataSet.Add(new StringCompressible("IR-0026137", "ChangeGear not starting"));
            DataSet.Add(new StringCompressible("IR-0026720", "When using SQL Scripts to Drop Incident Tables to wipe out test tickets - ON CG restart cg_IncidentRequest is not being recreated"));
            DataSet.Add(new StringCompressible("IR-0027472", "Email connector is not starting in dev envrionment"));
            DataSet.Add(new StringCompressible("IR-0027625", "Email connector service stopped  not restarting"));
            DataSet.Add(new StringCompressible("IR-0027693", "Email connector service not starting"));
            DataSet.Add(new StringCompressible("IR-0027791", "Error when trying to save - Actual Start Date and End Date being used?"));
            DataSet.Add(new StringCompressible("IR-0028919", "We recently started using the SLA manager and now newly created after action rules are not being applied"));
            DataSet.Add(new StringCompressible("IR-0029185", "email connector service will not start after server reboot"));
            DataSet.Add(new StringCompressible("IR-0029334", "mail Connector Service does not start and has crashed the CGWeb Client"));

            //Create the expected outcome 2D list
            List<List<string>> expectedLists = new List<List<string>>();

            //Create the list of tickets that match ticket 1
            List<string> expectedMatch1 = new List<string>();
            expectedMatch1.Add("IR-0026018");
            expectedMatch1.Add("IR-0029185");
            expectedMatch1.Add("IR-0027693");
            expectedMatch1.Add("IR-0027472");
            expectedMatch1.Add("IR-0027625");
            expectedMatch1.Add("IR-0029334");
            expectedLists.Add(expectedMatch1);

            //Create the list of tickets that match ticket 2
            List<string> expectedMatch2 = new List<string>();
            expectedMatch2.Add("IR-0026036");
            expectedMatch2.Add("IR-0026720");
            expectedLists.Add(expectedMatch2);

            //Create the list of tickets that match ticket 3
            List<string> expectedMatch3 = new List<string>();
            expectedMatch3.Add("IR-0026137");
            expectedLists.Add(expectedMatch3);

            //Create the list of tickets that match ticket 4
            List<string> expectedMatch4 = new List<string>();
            expectedMatch4.Add("IR-0026720");
            expectedMatch4.Add("IR-0026036");
            expectedLists.Add(expectedMatch4);

            //Create the list of tickets that match ticket 5
            List<string> expectedMatch5 = new List<string>();
            expectedMatch5.Add("IR-0027472");
            expectedMatch5.Add("IR-0029185");
            expectedMatch5.Add("IR-0027693");
            expectedMatch5.Add("IR-0026018");
            expectedMatch5.Add("IR-0027625");
            expectedMatch5.Add("IR-0029334");
            expectedLists.Add(expectedMatch5);

            //Create the list of tickets that match ticket 6
            List<string> expectedMatch6 = new List<string>();
            expectedMatch6.Add("IR-0027625");
            expectedMatch6.Add("IR-0029185");
            expectedMatch6.Add("IR-0027693");
            expectedMatch6.Add("IR-0027472");
            expectedMatch6.Add("IR-0026018");
            expectedMatch6.Add("IR-0029334");
            expectedLists.Add(expectedMatch6);

            //Create the list of tickets that match ticket 7
            List<string> expectedMatch7 = new List<string>();
            expectedMatch7.Add("IR-0027693");
            expectedMatch7.Add("IR-0029185");
            expectedMatch7.Add("IR-0026018");
            expectedMatch7.Add("IR-0027472");
            expectedMatch7.Add("IR-0027625");
            expectedMatch7.Add("IR-0029334");
            expectedLists.Add(expectedMatch7);

            //Create the list of tickets that match ticket 8
            List<string> expectedMatch8 = new List<string>();
            expectedMatch8.Add("IR-0027791");
            expectedLists.Add(expectedMatch8);

            //Create the list of tickets that match ticket 9
            List<string> expectedMatch9 = new List<string>();
            expectedMatch9.Add("IR-0028919");
            expectedLists.Add(expectedMatch9);

            //Create the list of tickets that match ticket 10
            List<string> expectedMatch10 = new List<string>();
            expectedMatch10.Add("IR-0029185");
            expectedMatch10.Add("IR-0026018");
            expectedMatch10.Add("IR-0027693");
            expectedMatch10.Add("IR-0027472");
            expectedMatch10.Add("IR-0027625");
            expectedMatch10.Add("IR-0029334");
            expectedLists.Add(expectedMatch10);

            //Create the list of tickets that match ticket 11
            List<string> expectedMatch11 = new List<string>();
            expectedMatch11.Add("IR-0029334");
            expectedMatch11.Add("IR-0029185");
            expectedMatch11.Add("IR-0027693");
            expectedMatch11.Add("IR-0027472");
            expectedMatch11.Add("IR-0027625");
            expectedMatch11.Add("IR-0026018");
            expectedLists.Add(expectedMatch11);
        }
    }
}
