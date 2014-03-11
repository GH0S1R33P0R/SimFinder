using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace SeniorProject
{
    public class Similarity : ISimilarity
    {
        public static int GetComplexity(ICompressible entity)
        {
            int compressedSize; // Used to hold the result

            byte[] uncompressedData = entity.ToByteArray();

            using (MemoryStream compressionStream = new MemoryStream())
            {
                // Result goes in compressionStream
                using (GZipStream gZipper = new GZipStream(compressionStream, CompressionMode.Compress))
                {
                    // Compress the compressed data.
                    gZipper.Write(uncompressedData, 0, uncompressedData.Length);
                }
                compressedSize = (int) compressionStream.Length;
            }

            return compressedSize;
        }

        public double GetSimilarity(ICompressible entity1, ICompressible entity2)
        {
            int complexityEntity1 = this.GetComplexity(entity1);
            int complexityEntity2 = this.GetComplexity(entity2);

            byte[] combinedEntitys;
            combinedEntitys = entity1.ToByteArray().Concat(entity2.ToByteArray());
            int NCD_A = this.GetComplexity(entity1.ToByteArray() + entity2.ToByteArray); //TODO: Create a function for '+' operation
            int NCD_B, NCD_C;
            if (complexityEntity1 >= complexityEntity2)
            {
                NCD_B = complexityEntity2;
                NCD_C = complexityEntity1;
            }
            else
            {
                NCD_B = complexityEntity1;
                NCD_C = complexityEntity2;
            }

            double NCD_result = (NCD_A - NCD_B) / NCD_C;
            return NCD_result;
        }

        public ICompressible[] FindSimilarEntities(ICompressible entity, ICompressible[] dataSet)
        {
            List<ICompressible> similarEntities = new List<ICompressible>();
            double similarityVal;
            foreach (ICompressible entity2 in dataSet)
            {
                similarityVal = GetSimilarity(entity, entity2);
                if (similarityVal < 0.35)
                {
                    similarEntities.add(entity2);
                }
            }
            return similarEntities.ToArray();
        }
    }
}
