using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeniorProject;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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

            //Open the CSV to read in the data set
            string currentDirectory = Directory.GetCurrentDirectory();
            var CSVReader = new StreamReader(File.OpenRead(Path.Combine(currentDirectory, "IncidentRequest_Gold.csv")));

            //Read in the "golden set" and add the entities to DataSet
            while (!CSVReader.EndOfStream)
            {
                var row = CSVReader.ReadLine();
                var data = row.Split(',');
                DataSet.Add(new StringCompressible(data[0], data[1]));
            }

            //Open log file for writing 
            var logFile = new StreamWriter(Path.Combine(currentDirectory, "TestDetection_Log.txt"));

            //Create the expected outcome 2D list
            List<List<string>> expectedLists = new List<List<string>>();

            //Read the expected outcomes json file into a string
            StreamReader fileReader = new StreamReader(File.OpenRead(Path.Combine(currentDirectory, "expectedOutcomes.json")));
            string jsonText = fileReader.ReadToEnd();
            JsonTextReader JReader = new JsonTextReader(new StringReader(jsonText));

            //Populate the expectedLists 2D list with the expected outcomes
            while (JReader.Read())
            {
                List<string> expectedMatches = new List<string>();
                if (JReader.TokenType.ToString() == "PropertyName" && JReader.Value.ToString() != "expectedOutcomes")
                {
                    expectedMatches.Add(JReader.Value.ToString());
                    JReader.Read();
                    JReader.Read();
                    while (JReader.TokenType.ToString() != "EndArray")
                    {
                        expectedMatches.Add(JReader.Value.ToString());
                        JReader.Read();
                    }
                    expectedLists.Add(expectedMatches);
                }
            }

            //Similarity object to use for FindSimilarEntities
            Similarity simTest = new Similarity();

            int currentList = 0;

            //Get the ordered results and return if the match is present
            foreach(StringCompressible ticket in DataSet)
            {
               ICompressible[] results = simTest.FindSimilarEntities(ticket, DataSet.ToArray());
               logFile.Write("{0} Matches: ", expectedLists[currentList][0]);
               foreach (StringCompressible expectedMatch in results)
               {
                   logFile.Write("{0} ", expectedMatch.ItemID);
                   logFile.Write("({0}), ", expectedLists[currentList].Contains(expectedMatch.ItemID));
               }
               currentList++;
               logFile.WriteLine();
            }
            logFile.Close();
            
            //Read the log file and assert that there are no false positives
            var logReader = new StreamReader(File.OpenRead(Path.Combine(currentDirectory, "TestDetection_Log.txt")));
            string log = logReader.ReadToEnd();
            logReader.Close();
            Assert.IsFalse(log.Contains("False"));
        }
    }
}
