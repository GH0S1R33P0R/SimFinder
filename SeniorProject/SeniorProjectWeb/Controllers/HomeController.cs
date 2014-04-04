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
        private List<StringCompressible> DataSet = new List<StringCompressible>();
        private Similarity simObject = new Similarity();
        public ActionResult Index()
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
                var row = CSVReader.ReadLine();
                DataSet.Add(new StringCompressible(row.Substring(0, row.IndexOf(',')), row.Substring(row.IndexOf(',') + 12)));
                DataSet.Last().ItemID = Regex.Replace(row.Substring(row.IndexOf(',')+1, 10),"IR-0+", "");
                //Console.WriteLine("itemID: {0}, summary: {1}", row.Substring(0, row.IndexOf(',')), row.Substring(row.IndexOf(',')+1));
            }
        }

        [HttpGet]
        public string GetSimilarTicketsSummary(string summary)
        {
            if (DataSet.Count == 0)
            {
                setDataSet();
                if (DataSet.Count == 0) return JsonConvert.SerializeObject("error");
                SetThreshold("45");
            }

            StringCompressible ticket = new StringCompressible(summary);
            

            List<Ticket> similarTickets = new List<Ticket>();
           
            List<Tuple<double, StringCompressible>> result = new List<Tuple<double, StringCompressible>>();

            result = simObject.FindSimilarValAndEntities(ticket, DataSet.ToArray());

            foreach (Tuple<double, StringCompressible> element in result)
            {
                similarTickets.Add(new Ticket {oid = element.Item2.OID, itemID = element.Item2.ItemID, rating = element.Item1, summary = element.Item2.Summary });
            }

            return JsonConvert.SerializeObject(similarTickets);

        }
        [HttpGet]
        public string GetSimilarTicketsID(string searchID)
        {
            searchID = Regex.Replace(searchID,"IR-0+", "");

            if (DataSet.Count == 0)
            {
                setDataSet();
                if (DataSet.Count == 0) return JsonConvert.SerializeObject("error");
                SetThreshold("45");
            }

            List<Ticket> similarTickets = new List<Ticket>();
            List<Tuple<double, StringCompressible>> result = new List<Tuple<double, StringCompressible>>();

            foreach (StringCompressible ticket in DataSet)
            {
                if (ticket.ItemID.Equals(searchID))
                {
                    result = simObject.FindSimilarValAndEntities(ticket, DataSet.ToArray());
                }
            }

            foreach (Tuple<double, StringCompressible> element in result)
            {
                similarTickets.Add(new Ticket { oid = element.Item2.OID, itemID = element.Item2.ItemID, rating = element.Item1, summary = element.Item2.Summary });
            }

            return JsonConvert.SerializeObject(similarTickets);

        }

        public string SetThreshold(string value)
        {
            simObject.Threshold = (double)System.Convert.ToInt64(value) / 100;
            return JsonConvert.SerializeObject(simObject.Threshold);
        }
    }
}