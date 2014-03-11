using System;
using System.Collections;
using System.Collections.Generic;
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
            int complexityEntity1 = GetComplexity(entity1);
            int complexityEntity2 = GetComplexity(entity2);

            ICompressible combinedEntitys = null; 

            // TODO combinedEntitys = entity1 + entity2 ;
            int NCD_A = GetComplexity(combinedEntitys);
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
            double similarityThreshold = 0.35;

            foreach (ICompressible entity2 in dataSet)
            {
                similarityVal = GetSimilarity(entity, entity2);
                if (similarityVal < similarityThreshold)
                {
                    similarEntities.Add(entity2);
                }
            }
            return similarEntities.ToArray();
        }
    }
}
