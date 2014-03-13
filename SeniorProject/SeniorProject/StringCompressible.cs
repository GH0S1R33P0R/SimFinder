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

        int ICompressible.Complexity
        {
            get { return this.complexity; }
            set { this.complexity = value; }
        }

        public string ItemID
        {
            get {return this.itemID; }
            set { this.itemID = value; }
        }
    }
}
