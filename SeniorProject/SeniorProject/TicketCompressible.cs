using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeniorProject
{
    public class TicketCompressible : ICompressible
    {
        private int complexity;
        private byte[] data;

        private string itemID; // This tickets ItemID value.
        private TicketCompressible[] similarIDList; // All tickets similar to this.

        /// <summary>
        /// TicketCompressible constructor
        /// </summary>
        /// <param name="summary">The summary field in a ticket.</param>
        /// <param name="ID">The ItemID in the ticket.</param>
        public TicketCompressible(string summary,string ID)
        {
            data = Encoding.ASCII.GetBytes(summary);
            itemID = ID;
        }

        byte[] ICompressible.ToByteArray()
        {
            return data;
        }

        public string ToString()
        {
            return Encoding.ASCII.GetString(data);
        }

        int ICompressible.Complexity
        {
            get { return this.complexity; }
            set { this.complexity = value; }
        }

        public string ItemID
        {
            get { return this.itemID; }
        }

        public TicketCompressible[] SimilarIDList
        {
            get { return this.similarIDList; }
            set { this.similarIDList = value; }
        }
    }
}