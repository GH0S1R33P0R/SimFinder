using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeniorProject;
using System.IO;
using System.Diagnostics;

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

        public static void RunAnalysis( )
        {
            Analysis analytics = new Analysis();
            isSim = new Similarity();
            isSim.Threshold = 0.25;

            var topN = 500;

            bool toConsole = true; // If we want to see the output live.

            inputs = new TicketCompressible[0];
            analytics.ObtainTickets(ref inputs);


            // Now each element in inputs has itemID, summary, and complexity.

            using (var w = new StreamWriter("output.txt"))
            {
                var sw = new Stopwatch();
                sw.Start();
                foreach (TicketCompressible ticket in inputs.Take(topN))
                {
                    ticket.SimilarIDList = analytics.FindSimilars(ticket, inputs);

                    // Display all the matches to this ticket
                    var dupItemIDs = new List<string>();
                    foreach (TicketCompressible match in ticket.SimilarIDList)
                    {
                        if (match.ItemID != ticket.ItemID)
                        {
                            dupItemIDs.Add(match.ItemID);
                        }
                    }

                    if (dupItemIDs.Count > 0 && dupItemIDs.Count <= 3)
                    {
                        var matches = string.Format("{0}: {1}", ticket.ItemID, string.Join(", ", dupItemIDs));
                        if (toConsole)
                        {
                            Console.WriteLine(matches);
                        }
                        w.WriteLine(matches);
                    }
                }

                sw.Stop();
                var avgTimePer = String.Format("Average Time (ms): {0}", (double)sw.Elapsed.TotalMilliseconds / topN);
                if (toConsole)
                {
                    Console.WriteLine();
                    Console.WriteLine(avgTimePer);
                }
                w.WriteLine();
                w.WriteLine(avgTimePer);

            }

            if (toConsole)
            {
                Console.ReadLine();
            }
        }
    }
}
