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
        private double threshold;

        private int compressionSize(byte[] input)
        {
            int compressedSize = 0;
            byte[] uncompressedData = input;

            // TODO http://msdn.microsoft.com/en-us/library/ms182334.aspx
            using (MemoryStream compressionStream = new MemoryStream())
            {
                // Result goes in compressionStream
                using (GZipStream gZipper = new GZipStream(compressionStream, CompressionMode.Compress))
                {
                    // Compress the compressed data.
                    gZipper.Write(uncompressedData, 0, uncompressedData.Length);
                }
                compressedSize = (int)compressionStream.Length;
            }

            return compressedSize;
        }

        private double getNCD(byte[] entity1, byte[] entity2)
        {
            int compressedEntity1 = compressionSize(entity1);
            int compressedEntity2 = compressionSize(entity2);
            
            byte[] combinedArray = entity1.Concat(entity2).ToArray();

            int NCD_A = compressionSize(combinedArray);
            int NCD_B, NCD_C;
            if (compressedEntity1 >= compressedEntity2)
            {
                NCD_B = compressedEntity2;
                NCD_C = compressedEntity1;
            }
            else
            {
                NCD_B = compressedEntity1;
                NCD_C = compressedEntity2;
            }

            double NCD_result = (NCD_A - NCD_B) / NCD_C;
            return NCD_result;
        }

        double ISimilarity.Threshold
        {
            get { return this.threshold; }
            set { this.threshold = value; }
        }

        public int GetComplexity(ICompressible entity)
        {
            int compressedSize; // Used to hold the result

            compressedSize = compressionSize(entity.ToByteArray());

            return compressedSize;
        }

        public bool IsSimilar(ICompressible entity1, ICompressible entity2)
        {
            if (GetSimilarity(entity1, entity2) <= threshold)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int SetComplexity(ref ICompressible entity)
        {
            int complexity = GetComplexity(entity);
            entity.Complexity = complexity;
            return complexity;
        }

        public double GetSimilarity(ICompressible entity1, ICompressible entity2)
        {
            double ncdResult;
            ncdResult = getNCD(entity1.ToByteArray(), entity2.ToByteArray());
            return ncdResult;
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

            similarEntities.Sort();

            return similarEntities.ToArray();
        }
    }
}
