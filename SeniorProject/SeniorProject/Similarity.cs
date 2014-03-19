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
        
        //Default constructor
        public Similarity()
        {
            threshold = 0.5;
        }

        private int compressionSize(byte[] input)
        {
            int compressedSize = 0;
            byte[] uncompressedData = input;

            // http://msdn.microsoft.com/en-us/library/ms182334.aspx LIES
            using (MemoryStream compressionStream = new MemoryStream())
            {
                // Result goes in compressionStream
                using (DeflateStream gZipper = new DeflateStream(compressionStream, CompressionLevel.Fastest, true))
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

        private double getNCD(ICompressible entity1, ICompressible entity2)
        {
            int compressedEntity1 = GetComplexity(entity1);
            int compressedEntity2 = GetComplexity(entity2);

            byte[] combinedArray = entity1.ToByteArray().Concat(entity2.ToByteArray()).ToArray();

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
            return NCD_result;
        }

        // MCD(A,B) = max(|c(AB)-c(AA)|, |c(AB)-c(BB)|)/max(c(AA),c(BB))
        private double getMCD(byte[] entity1, byte[] entity2)
        {
            double MCD_numerator;
            double MCD_result;
            
            // Find c(AA) and c(BB)
            double MCD_AA = (double)compressionSize(entity1.Concat(entity1).ToArray());
            double MCD_BB = (double)compressionSize(entity2.Concat(entity2).ToArray());

            // Find c(AB)
            byte[] combinedArray = entity1.Concat(entity2).ToArray();
            double MCD_AB = (double)compressionSize(combinedArray);

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

        public int SetComplexity(ICompressible entity)
        {
            int complexity = GetComplexity(entity);
            entity.Complexity = complexity;
            return complexity;
        }

        public double GetSimilarity(ICompressible entity1, ICompressible entity2)
        {
            return getNCD(entity1, entity2);;
        }

        public ICompressible[] FindSimilarEntities(ICompressible entity, ICompressible[] dataSet)
        {
            List<Tuple<double, ICompressible>> similarEntities = new List<Tuple<double, ICompressible>>();
            double similarityVal;
            double similarityThreshold = 0.44;

            foreach (ICompressible entity2 in dataSet)
            {
                similarityVal = GetSimilarity(entity, entity2);
                if (similarityVal < similarityThreshold)
                {
                    similarEntities.Add(new Tuple<double, ICompressible>(similarityVal,entity2));
                }
            }

            similarEntities.Sort((a, b) => a.Item1.CompareTo(b.Item1));

            return similarEntities.Select(t => t.Item2).ToArray();
        }

        public List<Tuple<double, StringCompressible>> FindSimilarValAndEntities(StringCompressible entity, StringCompressible[] dataSet)
        {
            List<Tuple<double, StringCompressible>> similarEntities = new List<Tuple<double, StringCompressible>>();
            double similarityVal;

            foreach (StringCompressible entity2 in dataSet)
            {
                similarityVal = GetSimilarity(entity, entity2);
                if (similarityVal < threshold)
                {
                    similarEntities.Add(new Tuple<double, StringCompressible>(similarityVal, entity2));
                }
            }

            similarEntities.Sort((a, b) => a.Item1.CompareTo(b.Item1));

            return similarEntities;
        }
    }
}
