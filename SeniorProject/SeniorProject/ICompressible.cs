
namespace SeniorProject
{
    public interface ICompressible
    {
        /// <summary>
        /// Marshals the entity to a byte[].
        /// </summary>
        /// <returns>Returns a byte[] representation of the entity.</returns>
        byte[] ToByteArray();

        /// <summary>
        /// Sets the data of an ICompressible
        /// </summary>
        /// <param name="input"></param>
        void setData(byte[] input);
    }
}
