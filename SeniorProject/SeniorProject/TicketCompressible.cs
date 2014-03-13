﻿using System;
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
        private string[] similarIDList; // All tickets similar to this.

        /// <summary>
        /// TicketCompressible constructor
        /// </summary>
        /// <param name="summary">The summary field in a ticket.</param>
        /// <param name="ID">The ItemID in the ticket.</param>
        public TicketCompressible(string summary,string ID)
        {
            data = Encoding.ASCII.GetBytes(summary);
            itemID = ID;
            similarIDList = new string[0];
        }

        byte[] ICompressible.ToByteArray()
        {
            return data;
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

        public string[] SimilarIDList
        {
            get { return this.similarIDList; }
            set { this.similarIDList = value; }
        }
    }
}