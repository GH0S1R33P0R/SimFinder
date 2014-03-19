using System;
using System.Collections.Generic;
using System.IO;
using SeniorProject;
using System.Diagnostics;

namespace SeniorProjectAnalytics
{
    class Program
    {
        /* static void Main(string[] args)
         {
            
             const double ncdThreshold = 0.1;
             var inputFile = "input.csv";
             var outputFile = "output.txt";

             using (var r = new StreamReader(inputFile))
             {
                 using (var w = new StreamWriter(outputFile))
                 {
                     string line;
                     int lineNumber = 0;
                     while ((line = r.ReadLine()) != null)
                     {
                         lineNumber++;
                         var ncds = line.Split(',');
                         var dups = new List<double>();
                         for (int i = 0; i < ncds.Length; ++i)
                         {
                             if (!string.IsNullOrEmpty(ncds[i]))
                             {
                                 var ncd = Convert.ToDouble(ncds[i]);
                                 if (ncd < ncdThreshold && (i + 1) != lineNumber)
                                 {
                                     dups.Add(i + 1);
                                 }
                             }
                         }
                         if (dups.Count > 0)
                         {
                             w.WriteLine("{0}: {1}", lineNumber, string.Join(", ", dups));
                         }
                     }
                 }
             
             }
         }
         * */

        static void Main(string[] args)
        {
            //Timer to record search time
            Stopwatch timer;

            //Similarity object to use for the FindSimilarEntities function
            Similarity simObject = new Similarity();

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

            //Get the item ID to search for and store it in searchID
            Console.WriteLine("Enter the itemID of the ticket you would like to find similarities for: ");
            searchID = Console.ReadLine();

            //Start the timer before searching
            timer = Stopwatch.StartNew();

            foreach (StringCompressible ticket in DataSet)
            {
                if (ticket.ItemID.Equals(searchID))
                {
                    results = simObject.FindSimilarValAndEntities(ticket, DataSet.ToArray());
                }
            }

            int counter = 1;

            foreach (Tuple<double, StringCompressible> ticket in results)
            {
                if (counter > 20)
                {
                    break;
                }
                Console.WriteLine("{0}.\tTicket ID: {1}\tConfidence Rating: {2}", counter, ticket.Item2.ItemID, ticket.Item1);
                counter++;
            }

            timer.Stop();
            Console.WriteLine();
            Console.WriteLine("Searched 5,000 tickets and produced results in {0} ms", timer.ElapsedMilliseconds);
            Console.ReadLine();
        }
    }
}
