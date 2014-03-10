using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeniorProject
{
    public class Similarity: ISimilarity
    {
        public int GetComplexity(ICompressible entity)
        {
            throw new NotImplementedException("TODO");
        }

        public double GetSimilarity(ICompressible entity1, ICompressible entity2)
        {
            throw new NotImplementedException("TODO");
        }

        public ICompressible[] FindSimilarEntities(ICompressible entity, ICompressible[] dataSet)
        {
            throw new NotImplementedException("TODO");
        }
    }
}
