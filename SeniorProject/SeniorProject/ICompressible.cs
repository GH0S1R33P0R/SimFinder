
namespace SeniorProject
{
    public interface ICompressible
    {
        /// <summary>
        /// Marshals the entity to a byte[].
        /// </summary>
        /// <returns>Returns a byte[] representation of the entity.</returns>
        byte[] ToByteArray();

        // TODO: Can this be nullable?
        /// <summary>
        /// Stored complexity value.
        /// </summary>
        void Complexity { get; set; }
    }
}
