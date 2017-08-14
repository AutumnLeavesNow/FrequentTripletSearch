using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FrequentTripletSearch
{

    class FrequentTripletFinder
    {
        private static object syncRoot = new object();
        public string MostFrequentTriplet(string str, CancellationToken ct)
        {
            string[] strings = str.Split(',');
            Dictionary<string, int> triplets = new Dictionary<string, int>();
            Parallel.ForEach(strings, word => WorkOnWord(word, triplets, ct));
            int value = 0;
            StringBuilder result = new StringBuilder();
            foreach (KeyValuePair<string, int> pair in triplets)
            {
                if (ct.IsCancellationRequested)
                    return null;
                if (pair.Value > value)
                {
                    result = new StringBuilder(pair.Key);
                    value = pair.Value;
                }
                else if (pair.Value == value)
                {
                    result.Append(',');
                    result.Append(pair.Key);
                }
            }
            return string.Format("{0}\t{1}", result.ToString(), value);
        }
        private void WorkOnWord(string word, Dictionary<string, int> dict, CancellationToken ct)
        {
            
            string triplet;
            for (int i = 0; i < word.Length - 2; i++)
            {
                if (ct.IsCancellationRequested)
                return;
                triplet = word.Substring(i, 3);
                lock (syncRoot)
                {
                    if (dict.ContainsKey(triplet))
                        dict[triplet]++;
                    else
                        dict.Add(triplet, 1);
                }

            }
        }
    }
}
