using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using Lz4Net;

namespace SeniorProject
{
    public class Similarity : ISimilarity
    {
        // The cutoff value for returning similar/duplicate tickets
        private double threshold;
        
        // Default constructor
        public Similarity()
        {
            threshold = 0.5;
        }

        // Compresses the byte array
        private int compressionSize(byte[] input)
        {
            return compressionSizeGzip(input);
        }

        // Method to determine the size/length of the compressed ticket using Gzip
        private int compressionSizeGzip(byte[] input)
        {
            int compressedSize = 0;             // Variable for return value
            byte[] uncompressedData = input;   

            
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

        // Method to determine the size/length of the compressed ticket using LZ4
        private int compressionSizeLZ4(byte[] input)
        {
            byte[] buffer = input;
            byte[] compressed = Lz4Net.Lz4.CompressBytes(buffer, 0, buffer.Length, Lz4Net.Lz4Mode.Fast);

            return compressed.Length;
        }

        // Method to determine the NCD value between two tickets (ICompressible objects)
        // NCD(x,y) = {Z(xy) - min[Z(x), Z(y)]} / max[Z(x), Z(y)]
        private double getNCD(ICompressible entity1, ICompressible entity2)
        {
            // Compress the tickets
            int compressedEntity1 = GetComplexity(entity1);
            int compressedEntity2 = GetComplexity(entity2);

            // Concatenate the tickets
            byte[] combinedArray = entity1.ToByteArray().Concat(entity2.ToByteArray()).ToArray();

            // NCD_A = compressed size of the two tickets concatenated
            double NCD_A = (double) compressionSize(combinedArray);
            
            // NCD_B = min(compressed size of ticket 1, compressed size of ticket 2)
            // NCD_C = max(compressed size of ticket 1, compressed size of ticket 2)
            double NCD_B, NCD_C;
           
            // Determine the values of NCD_B and NCD_C
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

            // Compute and return the NCD value
            double NCD_result = (NCD_A - NCD_B) / NCD_C;
            return NCD_result;
        }

        // Method to determine the MCD value between two tickets (ICompressible objects)
        // MCD(A,B) = max(|c(AB)-c(AA)|, |c(AB)-c(BB)|)/max(c(AA),c(BB))
        private double getMCD(ICompressible entity1, ICompressible entity2)
        {
            double MCD_numerator;
            double MCD_result;
            
            // Find c(AA) and c(BB)
            double MCD_AA = (double) compressionSize(entity1.ToByteArray().Concat(entity1.ToByteArray()).ToArray());
            double MCD_BB = (double) compressionSize(entity2.ToByteArray().Concat(entity2.ToByteArray()).ToArray());

            // Find c(AB)
            byte[] combinedArray = entity1.ToByteArray().Concat(entity2.ToByteArray()).ToArray();
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

        // Accessor and mutator for the Threshold data member
        public double Threshold
        {
            get { return this.threshold; }
            set { this.threshold = value; }
        }

        // Returns the size of the compressed ticket
        public int GetComplexity(ICompressible entity)
        {
            // Return the complexity of the ticket if its been set (i.e. not zero)
            if (entity.Complexity != 0)
            {
                return entity.Complexity;
            }

            // Set the complexity of the ticket for later use
            entity.Complexity = compressionSize(entity.ToByteArray());

            return entity.Complexity;
        }

        // Determines if two given tickets are similar or not
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

        // Set the complexity of a ticket
        public int SetComplexity(ICompressible entity)
        {
            int complexity = GetComplexity(entity);
            entity.Complexity = complexity;
            return complexity;
        }

        // Return the NCD value between two tickets
        public double GetSimilarity(ICompressible entity1, ICompressible entity2)
        {
            return getNCD(entity1, entity2);
        }

        // Given a test ticket and a set of tickets, return a sorted list of all tickets within the
        // set similar to the test ticket
        public ICompressible[] FindSimilarEntities(ICompressible entity, ICompressible[] dataSet)
        {
            // List to hold all of the similar tickets
            List<Tuple<double, ICompressible>> similarEntities = new List<Tuple<double, ICompressible>>();
           
            // Similary value between the two tickets
            double similarityVal;
            
            // Cutoff value for adding similar tickets to the similarEntities List
            double similarityThreshold = 0.44;

            // Iterate through the set of tickets and add similar tickets to the similarEntites List
            foreach (ICompressible entity2 in dataSet)
            {
                similarityVal = GetSimilarity(entity, entity2);
                if (similarityVal < similarityThreshold)
                {
                    similarEntities.Add(new Tuple<double, ICompressible>(similarityVal,entity2));
                }
            }

            // Sort the similarEntities List by NCD value
            similarEntities.Sort((a, b) => a.Item1.CompareTo(b.Item1));

            return similarEntities.Select(t => t.Item2).ToArray();
        }

        // Similar to FindSimilarEntites but returns a List of StringCompressibles
        // rather than an array of ICompressibles and includes the NCD/MCD value in the return list
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
