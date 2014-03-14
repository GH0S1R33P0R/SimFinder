using System;
using System.Collections.Generic;
using System.IO;

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
    }
}
