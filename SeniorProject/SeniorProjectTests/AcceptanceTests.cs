using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeniorProject;

namespace SeniorProjectTests
{
    public class AcceptanceTests
    {
        //This method tests if an expected match is found for a given ticket and data set
        public bool ContainsMatch(ICompressible testTicket, ICompressible expectedMatch, ICompressible[] dataSet)
        {
            //Similarity object to use for FindSimilarEntities
            Similarity simTest = new Similarity(); 

            //Get the ordered results and return if the match is present
            ICompressible[] results = simTest.FindSimilarEntities(testTicket, dataSet);

            if(results.Contains(expectedMatch))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
