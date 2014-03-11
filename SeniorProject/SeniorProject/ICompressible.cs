
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
        /// Stored complexity value.
        /// </summary>
        double Complexity { get; set; }
    }
}
