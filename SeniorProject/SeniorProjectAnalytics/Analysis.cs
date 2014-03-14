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
            var matches = new List<TicketCompressible>();

            foreach (TicketCompressible ticket in entityList)
            {
                if (isSim.IsSimilar(entity, ticket))
                {
                    matches.Add(ticket);
                }
            }
            return matches.ToArray();
        }

        static void Main(string[] args)
        {
            Analysis analytics = new Analysis();
            isSim = new Similarity();
            isSim.Threshold = 0.5;

            bool toConsole = true; // If we want to see the output live.

            inputs = new TicketCompressible[0];
            analytics.ObtainTickets(ref inputs);


            // Now each element in inputs has itemID, summary, and complexity.

            using (var w = new StreamWriter("output.txt"))
            {
                foreach (TicketCompressible ticket in inputs)
                {
                    ticket.SimilarIDList = analytics.FindSimilars(ticket, inputs);

                    w.Write("\n{0}: ", ticket.ItemID);
                    if (toConsole)
                    {
                        Console.Write("\n{0}: ", ticket.ItemID);
                    }

                    // Display all the matches to this ticket
                    bool first = true;
                    foreach (TicketCompressible match in ticket.SimilarIDList)
                    {
                        // A hack to get the commas to appear correctly
                        if (!first)
                        {
                            if (toConsole)
                            {
                                Console.Write(", {0}", match.ItemID);
                            }
                            w.Write(", {0}", match.ItemID);
                        }
                        else
                        {
                            Console.Write(" {0}", match.ItemID);
                            w.Write(" {0}", match.ItemID);
                            first = false;
                        }
                    }
                }
            }

            if (toConsole)
            {
                Console.ReadLine();
            }
        }
    }
}
