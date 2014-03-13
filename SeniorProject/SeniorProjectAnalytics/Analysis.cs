using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeniorProject;
using System.IO;

namespace SeniorProjectAnalytics
{
    class Analysis
    {
        private static ISimilarity isSim;
        private static TicketCompressible[] inputs;
        private static ICompressible[] output;

        /// <summary>
        /// Read file "input.txt" and set list of TicketCompressible items.
        /// </summary>
        /// <param name="entitys">Array of Ticket to be overwritten.</param>
        public void ObtainTickets(ref TicketCompressible[] entitys)
        {
            var inputFile = "input.csv";
            var tickets = new List<TicketCompressible>();

            using (var r = new StreamReader(inputFile))
            {
                string line;
                string[] itemsInLine;
                string id;
                string summary;


                // Skip the header line
                r.ReadLine();

                while ((line = r.ReadLine()) != null)
                {
                    // Parse line
                    itemsInLine = line.Split(',');
                    id = itemsInLine[1];
                    summary = itemsInLine[2];

                    TicketCompressible ticket = new TicketCompressible(summary, id);

                    isSim.SetComplexity(ticket);

                    tickets.Add(ticket);
                }
            }

            entitys = tickets.ToArray();
        }

        /// <summary>
        /// Finds all tickets which are similar to the entity. 
        /// </summary>
        /// <param name="entity">Ticket to use for comparison.</param>
        /// <param name="entityList">Array of tickets to compare against.</param>
        /// <returns>An array of tickets similar to entity.</returns>
        public TicketCompressible[] FindSimilars(TicketCompressible entity, TicketCompressible[] entityList)
        {
            // TODO: implement
            return entityList;
        }

        static void Main(string[] args )
        {
            Analysis analytics = new Analysis();
            isSim = new Similarity();

            inputs = new TicketCompressible[0];
            analytics.ObtainTickets(ref inputs);

            // Now each element in inputs has itemID, summary, and complexity.

            // TODO: FindSimilars for each ticket and write to file
        }
    }
}
