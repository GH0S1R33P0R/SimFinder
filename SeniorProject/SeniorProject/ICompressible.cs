
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
        /// Returns a new ICompressible that contains the first ICompressible followed by the second.
        /// </summary>
        /// <param name="input">ICompressible to be appended.</param>
        /// <returns>The resulting ICompressible.</returns>
        ICompressible append(ICompressible input );
    }
}
