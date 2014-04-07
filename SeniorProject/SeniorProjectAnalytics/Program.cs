using System;
using System.Collections.Generic;
using System.IO;
using SeniorProject;
using System.Diagnostics;

namespace SeniorProjectAnalytics
{
    // Driver program for the NCD/MCD library
    class Program
    {
        static void Main(string[] args)
        {
            //Timer to record search time
            Stopwatch timer;

            //Similarity object to use for the FindSimilarEntities function
            Similarity simObject = new Similarity();
            simObject.Threshold = 0.45;

            //The ID of the ticket that you want to find similar tickets for
            string searchID;

            //List of tickets and their NCD values that match the search ticket
            List<Tuple<double, StringCompressible>> results = new List<Tuple<double, StringCompressible>>();

            //Create the data set as a list of StringCompressible objects
            List<StringCompressible> DataSet = new List<StringCompressible>();

            //Open the CSV to read in the data set
            string currentDirectory = Directory.GetCurrentDirectory();
            var CSVReader = new StreamReader(File.OpenRead(Path.Combine(currentDirectory, "IncidentRequest_Gold5k.csv")));

            //Read in the "golden set" and add the entities to DataSet
            while (!CSVReader.EndOfStream)
            {
                var row = CSVReader.ReadLine();
                DataSet.Add(new StringCompressible(row.Substring(0, row.IndexOf(',')), row.Substring(row.IndexOf(',') + 1)));
                //Console.WriteLine("itemID: {0}, summary: {1}", row.Substring(0, row.IndexOf(',')), row.Substring(row.IndexOf(',')+1));
            }

            while (true)
            {
                //Get the item ID to search for and store it in searchID
                Console.WriteLine("Enter the itemID of the search ticket ('q' to quit): ");
                searchID = Console.ReadLine();

                //REPL control
                if (searchID.Equals("q"))
                {
                    break;
                }

                //Start the timer before searching
                timer = Stopwatch.StartNew();

                // Find the itemID matching the requested ticket and populate the results List with similar tickets
                foreach (StringCompressible ticket in DataSet)
                {
                    if (ticket.ItemID.Equals(searchID))
                    {
                        results = simObject.FindSimilarValAndEntities(ticket, DataSet.ToArray());
                    }
                }

                int counter = 1;   // Counter for the number of tickets to return

                // Print output formatting
                Console.WriteLine();
                Console.WriteLine("-----Similar Ticket List-----");
                Console.WriteLine();

                // Print the similar tickets with their ID and NCD/MCD value
                foreach (Tuple<double, StringCompressible> ticket in results)
                {
                    //if (counter > 20)
                    //{
                    //    break;
                    //}
                    Console.WriteLine("{0}.\tTicket ID: {1}\tConfidence Rating: {2}", counter, ticket.Item2.ItemID, ticket.Item1);
                    counter++;
                }

                timer.Stop();  // Stop the timer

                // Output the time it took to return the results
                Console.WriteLine();
                Console.WriteLine("Searched 5,000 tickets and produced results in {0} ms", timer.ElapsedMilliseconds);
                Console.WriteLine();
            }
        }
    }
}
