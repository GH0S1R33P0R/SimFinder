using SeniorProject;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeniorProjectWeb.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace SeniorProjectWeb.Controllers
{
    public class HomeController : Controller
    {
        private static List<StringCompressible> DataSet = new List<StringCompressible>();
        private static Similarity simObject = new Similarity();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }

        private void setDataSet()
        {
            //Open the CSV to read in the data set
            var CSVReader = new StreamReader(System.IO.File.OpenRead(Path.Combine(Server.MapPath("~/App_Data"), "IncidentRequest.csv")));

            //Read in the "golden set" and add the entities to DataSet
            while (!CSVReader.EndOfStream)
            {
                var row = CSVReader.ReadLine().Split(',');
                var entity = new StringCompressible(row[0], row[2]);
                entity.ItemID = Regex.Replace(row[1], "IR-0+", "");
                simObject.SetComplexity(entity);
                DataSet.Add(entity);
            }
        }

        [HttpPost]
        public void Init()
        {
            if (DataSet.Count == 0)
            {
                setDataSet();
                SetThreshold("45");
            }
        }

        [HttpGet]
        public string GetSimilarTicketsSummary(string summary)
        {
            Init();

            var ticket = new StringCompressible(summary);
            simObject.SetComplexity(ticket);

            var results = simObject.FindSimilarValAndEntities(ticket, DataSet.ToArray());

            var similarTickets = new List<Ticket>();
            foreach (var element in results)
            {
                similarTickets.Add(new Ticket {
                    oid = element.Item2.OID,
                    itemID = element.Item2.ItemID,
                    rating = element.Item1,
                    summary = element.Item2.Summary
                });
            }

            return JsonConvert.SerializeObject(similarTickets);
        }

        [HttpGet]
        public string GetSimilarTicketsID(string searchID)
        {
            Init();

            searchID = Regex.Replace(searchID, "IR-0+", "");
            var ticket = (from entity in DataSet
                          where entity.ItemID == searchID
                          select entity).First();
            simObject.SetComplexity(ticket);

            var results = simObject.FindSimilarValAndEntities(ticket, DataSet.ToArray());

            var similarTickets = new List<Ticket>();
            foreach (var element in results)
            {
                similarTickets.Add(new Ticket {
                    oid = element.Item2.OID,
                    itemID = element.Item2.ItemID,
                    rating = element.Item1,
                    summary = element.Item2.Summary
                });
            }

            return JsonConvert.SerializeObject(similarTickets);
        }

        [HttpPost]
        public string SetThreshold(string value)
        {
            simObject.Threshold = (double)System.Convert.ToInt64(value) / 100;
            return JsonConvert.SerializeObject(simObject.Threshold);
        }
    }
}