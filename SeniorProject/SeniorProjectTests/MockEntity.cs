using SeniorProject;

namespace SeniorProjectTests
{
    class MockEntity: ICompressible
    {
        private byte[] data;

        public MockEntity()
        {

        }

        public MockEntity(byte[] data)
        {
            this.data = data;
        }

        byte[] ICompressible.ToByteArray()
        {
            return data;
        }

        void ICompressible.setData(byte[] input)
        {
            this.data = input;
        }
    }
}
