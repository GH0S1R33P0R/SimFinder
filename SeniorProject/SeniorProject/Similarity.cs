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

            // http://msdn.microsoft.com/en-us/library/ms182334.aspx LIES
            using (MemoryStream compressionStream = new MemoryStream())
            {
                // Result goes in compressionStream
                using (GZipStream gZipper = new GZipStream(compressionStream, CompressionMode.Compress,true ))
                {
                    // Compress the compressed data.
                    gZipper.Write(uncompressedData, 0, uncompressedData.Length);
                    gZipper.Flush();
                    gZipper.Close();
                    compressedSize = (int)compressionStream.Length;
                }
            }
            return compressedSize;
        }

        private double getNCD(byte[] entity1, byte[] entity2)
        {
            int compressedEntity1 = compressionSize(entity1);
            int compressedEntity2 = compressionSize(entity2);
            
            byte[] combinedArray = entity1.Concat(entity2).ToArray();

            double NCD_A = (double) compressionSize(combinedArray);
            double NCD_B, NCD_C;
            if (compressedEntity1 >= compressedEntity2)
            {
                NCD_B = (double) compressedEntity2;
                NCD_C = (double) compressedEntity1;
            }
            else
            {
                NCD_B = (double) compressedEntity1;
                NCD_C = (double) compressedEntity2;
            }

            double NCD_result = (NCD_A - NCD_B) / NCD_C;
            Console.WriteLine("NCD A: {0}, NCD B: {1}, NCD C: {2}", NCD_A, NCD_B, NCD_C);
            return NCD_result;
        }

        // MCD(A,B) = max( |c(AB)-c(AA)|, |c(AB)-c(BB)|)/max(c(AA),c(BB))
        private double getMCD(byte[] entity1, byte[] entity2)
        {
            int MCD_numerator;
            double MCD_result;
            
            // Find c(AA) and c(BB)
            int MCD_AA = compressionSize(entity1.Concat(entity1).ToArray());
            int MCD_BB = compressionSize(entity2.Concat(entity2).ToArray());

            // Find c(AB)
            byte[] combinedArray = entity1.Concat(entity2).ToArray();
            int MCD_AB = compressionSize(combinedArray);

            // Find max( |c(AB)-c(AA)|, |c(AB)-c(BB)|)
            if (Math.Abs(MCD_AB - MCD_AA) >= Math.Abs(MCD_AB - MCD_BB))
            {
                MCD_numerator = Math.Abs(MCD_AB - MCD_AA);
            }
            else
            {
                MCD_numerator = Math.Abs(MCD_AB - MCD_BB);
            }

            // Find MCD(A,B)
            if (MCD_AA >= MCD_BB)
            {
                MCD_result = (MCD_numerator / MCD_AA);
            }
            else
            {
                MCD_result = (MCD_numerator / MCD_BB);
            }

            return MCD_result;
        }

        double ISimilarity.Threshold
        {
            get { return this.threshold; }
            set { this.threshold = value; }
        }


        public int GetComplexity(ICompressible entity)
        {
            if (entity.Complexity != 0)
            {
                return entity.Complexity;
            }
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
                    Console.WriteLine("NCD value: {0}", similarityVal);
                }
            }

            //similarEntities.Sort();

            return similarEntities.ToArray();
        }
    }
}
