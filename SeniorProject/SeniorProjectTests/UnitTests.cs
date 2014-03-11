using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeniorProject;
using System.Text;
using System.Linq;

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
            ICompressible xx = new MockEntity(doubleArray);
            ICompressible e = new MockEntity();

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
    }
}
