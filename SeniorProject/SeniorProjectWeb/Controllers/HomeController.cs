using SeniorProject;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeniorProjectWeb.Models;
using Newtonsoft.Json;

namespace SeniorProjectWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string GetSimilarTickets(string summary)
        {
            StringCompressible ticket = new StringCompressible(summary);

            Similarity simObject = new Similarity();
            simObject.Threshold = 0.45;

            List<StringCompressible> DataSet = new List<StringCompressible>();

            //Open the CSV to read in the data set
            var CSVReader = new StreamReader(System.IO.File.OpenRead(Path.Combine(Server.MapPath("~/App_Data"), "IncidentRequest_Gold5k.csv")));

            //Read in the "golden set" and add the entities to DataSet
            while (!CSVReader.EndOfStream)
            {
                var row = CSVReader.ReadLine();
                DataSet.Add(new StringCompressible(row.Substring(0, row.IndexOf(',')), row.Substring(row.IndexOf(',') + 1)));
                //Console.WriteLine("itemID: {0}, summary: {1}", row.Substring(0, row.IndexOf(',')), row.Substring(row.IndexOf(',')+1));
            }

            List<Ticket> similarTickets = new List<Ticket>();
            List<Tuple<double, StringCompressible>> result = new List<Tuple<double, StringCompressible>>();

            result = simObject.FindSimilarValAndEntities(ticket, DataSet.ToArray());

            foreach (Tuple<double, StringCompressible> element in result)
            {
                similarTickets.Add(new Ticket {id = element.Item2.ItemID, rating = element.Item1 });
            }
            
            return JsonConvert.SerializeObject(similarTickets);

        }
    }
}