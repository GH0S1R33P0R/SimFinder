using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SeniorProject
{
    public class Similarity: ISimilarity
    {
        public int GetComplexity(ICompressible entity)
        {
			var bytesIn = entity.ToByteArray ();
			using(var original = new MemoryStream(bytesIn))
			using(var compress = new MemoryStream())
			{
				using(var destination = new GZipStream(compress, CompressionMode.Compress){
					original.CopyTo( destination);
				}
					return (int) compress.Length();
			}
        }

        public double GetSimilarity(ICompressible entity1, ICompressible entity2)
        {
			int complexityEntity1 = this.GetComplexity (entity1);
			int complexityEntity2 = this.GetComplexity (entity2);

			int NCD_A = this.GetComplexity (entity1 + entity2); //TODO: Create a function for '+' operation
			int NCD_B, NCD_C;
			if (complexityEntity1 >= complexityEntity2) {
				NCD_B = complexityEntity2;
				NCD_C = complexityEntity1;
			} else {
				NCD_B = complexityEntity1;
				NCD_C = complexityEntity2;
			}

			double NCD_result = (NCD_A - NCD_B) / NCD_C;
			return NCD_result;
        }

        public ICompressible[] FindSimilarEntities(ICompressible entity, ICompressible[] dataSet)
        {
			List<ICompressible> similarEntities = new List<ICompressible> ();
			double similarityVal;
			foreach (ICompressible entity2 in dataSet) {
				similarityVal	= GetSimilarity (entity, entity2);
				if (similarityVal < 0.35) {
					similarEntities.add(entity2);
				}
			}
			return similarEntities.ToArray ();
        }
    }
}
