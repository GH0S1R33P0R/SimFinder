using SeniorProject;

namespace SeniorProjectTests
{
    // Mock version of the ICompressible ticket entity
    class MockEntity: ICompressible
    {
        private int complexity;    // Size of the compressed ticket
        private byte[] data;       // Data field of the ticket
        private string itemID;     // Item ID of the ticket

        // Default Constructor
        public MockEntity()
        {
            data = null;
            complexity = 0;
        }

        // Constructor that sets the data field
        public MockEntity(byte[] data)
        {
            this.data = data;
            complexity = 0;
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
            set { this.complexity = value;}
        }

        // Accessor and mutator for item ID value
        public string ItemID
        {
            get { return this.itemID; }
            set { this.itemID = value; }
        }
    }
}
