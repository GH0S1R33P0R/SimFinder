using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeniorProject
{
    public class StringCompressible : ICompressible
    {
        private double complexity;
        private byte[] data;

        //Default Constructor
        public StringCompressible()
        {
            complexity = 0;
            data = null;
        }
       
        //Constructor that takes string data
        public StringCompressible(string data)
        {
            this.data = Encoding.ASCII.GetBytes(data);
        }

        byte[] ICompressible.ToByteArray()
        {
            return data;
        }

        double ICompressible.Complexity
        {
            get { return this.complexity; }
            set { this.complexity = value; }
        }
    }
}
