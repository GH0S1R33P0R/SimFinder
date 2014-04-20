using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeniorProject
{
    // String version of the ICompressible ticket entity
    public class StringCompressible : ICompressible
    {
        private int complexity;    // Size of the compressed ticket
        private byte[] data;       // Data field of the ticket
        private string itemID;     // Item ID of the ticket
        private string summary;
        private string oid;

        //Default Constructor
        public StringCompressible()
        {
            complexity = 0;
            data = new byte[0];
        }
       
        //Constructor that takes one string to set the data field
        public StringCompressible(string summaryValue)
        {
            data = Encoding.ASCII.GetBytes(summaryValue.ToUpper());
            summary = summaryValue;
        }

        //Constructor that takes two strings to set the data field and item ID
        public StringCompressible(string ID, string summaryValue)
        {
            data = Encoding.ASCII.GetBytes(summaryValue.ToUpper());
            oid = ID;
            summary = summaryValue;
        }

        // Return the data as a byte array
        byte[] ICompressible.ToByteArray()
        {
            return data;
        }

        // Accessor and mutator for Complexity value
        int ICompressible.Complexity
        {
            get { return this.complexity; }
            set { this.complexity = value; }
        }

        // Accessor and mutator for item ID value
        public string ItemID
        {
            get {return this.itemID; }
            set { this.itemID = value; }
        }

        public string OID
        {
            get { return this.oid; }
        }

        public string Summary
        {
            get { return this.summary; }
        }
    }
}
