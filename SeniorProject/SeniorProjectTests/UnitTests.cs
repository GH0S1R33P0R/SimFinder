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
            ICompressible x = new MockEntity();
            ICompressible xx = new MockEntity();
            ICompressible e = new MockEntity();

            x.setData(Encoding.ASCII.GetBytes("Lorem Ipsum Dolor"));

            byte[] doubleArray = x.ToByteArray().Concat(x.ToByteArray()).ToArray();
            xx.setData(doubleArray);

            e.setData(null); // Empty byte array.

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
            ICompressible xy = new MockEntity();
            ICompressible x = new MockEntity();

            x.setData(Encoding.ASCII.GetBytes("Lorem Ipsum Dolor"));

            byte[] appendingString = Encoding.ASCII.GetBytes("sit amet, consectetur adipiscing elit");
            xy.setData(x.ToByteArray().Concat(appendingString).ToArray());

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
