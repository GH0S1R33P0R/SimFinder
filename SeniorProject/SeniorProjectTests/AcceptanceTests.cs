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

        //Main function to run the acceptance tests
        static void Main(string[] args)
        {
            AcceptanceTests tester = new AcceptanceTests();

            //Create the golden set as an array of StringCompressible objects
            StringCompressible[] goldenSet = new StringCompressible[11] {
            new StringCompressible("Unable to start email connector after MR2 install"),
            new StringCompressible("When using SQL Scripts to Drop Incident Tables to wipe out test tickets - ON CG restart cg_IncidentRequest is not being recreated"),
            new StringCompressible("ChangeGear not starting"),
            new StringCompressible("When using SQL Scripts to Drop Incident Tables to wipe out test tickets - ON CG restart cg_IncidentRequest is not being recreated"),
            new StringCompressible("Email connector is not starting in dev envrionment"),
            new StringCompressible("Email connector service stopped  not restarting"),
            new StringCompressible("Email connector service not starting"),
            new StringCompressible("Error when trying to save - Actual Start Date and End Date being used?"),
            new StringCompressible("We recently started using the SLA manager and now newly created after action rules are not being applied"),
            new StringCompressible("email connector service will not start after server reboot"),
            new StringCompressible("mail Connector Service does not start and has crashed the CGWeb Client")
            };

            //Check the known duplicates for ticket #1 in goldenSet (IR-0026018)
            //Console.Write(tester.ContainsMatch(goldenSet[0], goldenSet[9], goldenSet));
            //Console.Write(tester.ContainsMatch(goldenSet[0], goldenSet[6], goldenSet));
            //Console.Write(tester.ContainsMatch(goldenSet[0], goldenSet[4], goldenSet));
            //Console.Write(tester.ContainsMatch(goldenSet[0], goldenSet[5], goldenSet));
            //Console.Write(tester.ContainsMatch(goldenSet[0], goldenSet[10], goldenSet));

            //Check the known duplicates for ticket #1 in goldenSet (IR-0026018)

            int i = 0;
            int j = 0;

            foreach (StringCompressible element1 in goldenSet)
            {
                foreach (StringCompressible element2 in goldenSet)
                {
                    Console.Write("Ticket {0} vs ticket {1}: ", i, j);
                    Console.WriteLine(tester.ContainsMatch(element1, element2, goldenSet));
                    j++;
                }
                j = 0;
                i++;
            }

            Console.Read();
        }
    }
}
