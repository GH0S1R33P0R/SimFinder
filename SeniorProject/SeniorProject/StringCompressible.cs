using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeniorProject
{
    public class StringCompressible : ICompressible
    {
        private int complexity;
        private byte[] data;
        private string itemID;

        //Default Constructor
        public StringCompressible()
        {
            complexity = 0;
            data = new byte[0];
        }
       
        //Constructor that takes one string
        public StringCompressible(string summary)
        {
            data = Encoding.ASCII.GetBytes(summary);
        }

        //Constructor that takes two strings
        public StringCompressible(string ID, string summary)
        {
            data = Encoding.ASCII.GetBytes(summary);
            itemID = ID;
        }

        byte[] ICompressible.ToByteArray()
        {
            return data;
        }

        public string ItemID()
        {
            return itemID;
        }

        int ICompressible.Complexity
        {
            get { return this.complexity; }
            set { this.complexity = value; }
        }

        string ICompressible.ItemID
        {
            get {return this.itemID; }
            set { this.itemID = value; }
        }
    }
}
