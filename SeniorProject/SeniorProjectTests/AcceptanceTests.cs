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
        public bool ContainsMatch(ICompressible testTicket, ICompressible potentialMatch, ICompressible[] dataSet)
        {
            //Similarity object to use for FindSimilarEntities
            Similarity simTest = new Similarity(); 

            //Get the order results and return if they match is present
            ICompressible[] results = simTest.FindSimilarEntities(testTicket, dataSet);

            if(results.Contains(potentialMatch))
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
