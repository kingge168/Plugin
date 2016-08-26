using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Spider.Common
{
    public abstract class Voter<T>
    {
        protected IList<T> Votors
        {
            get;
            private set;
        }
        protected readonly object _syncObject = new object();
        protected Voter(IList<T> votors)
        {
            if (votors != null)
            {
                Votors = votors;
            }
            else
            {
                Votors = new List<T>();
            }
        }

        public abstract T Vote();
    }

    public class WeightVoter<T> : Voter<T>
    {
        private Random random;
        private int totalWeight;
        private IList<int> Weights { get; set; }
        public WeightVoter(IList<T> votors, IList<int> weights)
            : base(votors)
        {
            Weights = weights ?? new List<int>();
            totalWeight = weights.Sum();
            random = new Random();
        }

        public override T Vote()
        {
            if (Votors.Count > 0)
            {
                int r = random.Next(totalWeight) + 1;
                int weight = 0;
                for (int i = 0; i < Votors.Count; i++)
                {
                    weight += Weights.Count > i ? Weights[i] : 0;
                    if (weight >= r)
                    {
                        return Votors[i];
                    }
                }
            }
            return default(T);
        }
    }

    public class RollVoter<T> : Voter<T>
    {
        private int index = 0;
        public RollVoter(IList<T> votors)
            : base(votors)
        {

        }

        public override T Vote()
        {
            if (Votors.Count > 0)
            {
                var votor = Votors[index];
                if (index < Votors.Count-1)
                {
                    Interlocked.Increment(ref index);
                }
                else
                {
                    index = 0;
                }
                return votor;
            }
            return default(T);
        }
    }
}
