using SeniorProject;

namespace SeniorProjectTests
{
    class MockEntity: ICompressible
    {
        private byte[] data;

        MockEntity(byte[] data)
        {
            this.data = data;
        }

        byte[] ICompressible.ToByteArray()
        {
            byte [] toReturn;

            toReturn = new byte [3] {1,2,3};
            return toReturn;
        }

        void ICompressible.setData(byte[] input)
        {
            this.data = input;
        }
    }
}
