using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Net.Download
{
    public class DownloadStatistics
    {
        [NonSerialized]
        private StUtil.Data.Generic.FixedCapacityQueue<int> samples = new StUtil.Data.Generic.FixedCapacityQueue<int>(1000);

        private List<double> averages = new List<double>();

        public DateTime DateAdded { get; private set; }

        public double SessionAverageSpeed
        {
            get
            {
                return samples.Average();
            }
        }

        public double HistoricAverageSpeed
        {
            get
            {
                return (averages.Sum() + SessionAverageSpeed) / (averages.Count + 1);
            }
        }

        public DownloadStatistics()
        {
            DateAdded = DateTime.Now;
        }
    }
}
