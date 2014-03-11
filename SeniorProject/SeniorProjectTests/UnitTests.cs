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

            ICompressible xy = new MockEntity();
            ICompressible x = new MockEntity();


            ISimilarity simTest = new Similarity();

            Assert.IsTrue(simTest.GetComplexity(xy) >= simTest.GetComplexity(x));
        }

        /// <summary>
        /// Test Symmetry: C(xy) = C(yx).
        /// </summary>
        [TestMethod]
        public void TestSymmetry()
        {
            ICompressible Forward = new MockEntity();
            ICompressible Backward = new MockEntity();

            string testString = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
            string reversedString = new string(testString.ToCharArray().Reverse().ToArray());

            Forward.setData(Encoding.ASCII.GetBytes(testString));
            Backward.setData(Encoding.ASCII.GetBytes(reversedString));

            ISimilarity simTest = new Similarity();

            Assert.IsTrue(simTest.GetComplexity(Forward) == simTest.GetComplexity(Backward));
        }

        /// <summary>
        /// Test Distributivity: C(xy) + C(z) <= C(xz) + C(yz).
        /// </summary>
        [TestMethod]
        public void TestDistributivity()
        {
            Assert.Fail("TODO");
        }
    }
}
