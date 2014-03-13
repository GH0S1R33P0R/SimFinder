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
            new StringCompressible("IR-0026018", "Unable to start email connector after MR2 install"),
            new StringCompressible("IR-0026036", "When using SQL Scripts to Drop Incident Tables to wipe out test tickets - ON CG restart cg_IncidentRequest is not being recreated"),
            new StringCompressible("IR-0026137", "ChangeGear not starting"),
            new StringCompressible("IR-0026720", "When using SQL Scripts to Drop Incident Tables to wipe out test tickets - ON CG restart cg_IncidentRequest is not being recreated"),
            new StringCompressible("IR-0027472", "Email connector is not starting in dev envrionment"),
            new StringCompressible("IR-0027625", "Email connector service stopped  not restarting"),
            new StringCompressible("IR-0027693","Email connector service not starting"),
            new StringCompressible("IR-0027791", "Error when trying to save - Actual Start Date and End Date being used?"),
            new StringCompressible("IR-0028919", "We recently started using the SLA manager and now newly created after action rules are not being applied"),
            new StringCompressible("IR-0029185", "email connector service will not start after server reboot"),
            new StringCompressible("IR-0029334", "mail Connector Service does not start and has crashed the CGWeb Client")
            };

            //Check each of the 11 tickets against each other and print true if ncd value < threshhold, false otherwise
            int i = 0;
            int j = 0;

            foreach (StringCompressible element1 in goldenSet)
            {
                Console.Write("{0} matches: ", goldenSet[i].ItemID());
                foreach (StringCompressible element2 in goldenSet)
                {
                    if (tester.ContainsMatch(element1, element2, goldenSet) && i != j)
                    {
                        Console.Write("{0}, ", goldenSet[j].ItemID());
                    }
                    j++;
                }
                Console.WriteLine();
                j = 0;
                i++;
            }
           
            Console.Read(); //Prevents the console from closing
        }
    }
}
