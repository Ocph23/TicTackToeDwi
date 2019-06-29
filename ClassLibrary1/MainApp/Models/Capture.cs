using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Models
{
    [TableName("greedy")]
    public class CaptureGreedyModel  :ICapture
    {

        [DbColumn("Id")]
        public int? Id { get; set; }

        [DbColumn("Guid")]
        public string Guid { get; set; }

        [DbColumn("awal")]
        public string awal { get; set; }


        [DbColumn("akhir")]
        public string akhir { get; set; }


        [DbColumn("waktu")]
        public long waktu { get; set; }

        [DbColumn("board")]
        public int Board { get; set; }


        public TimeSpan Time
        {
            get { return TimeSpan.FromTicks(waktu); }
            set
            {
                waktu = value.Ticks;
            }
        }
        public string Awal => AppDomain.CurrentDomain.BaseDirectory + "Captures\\" + awal;
        public string Akhir => AppDomain.CurrentDomain.BaseDirectory + "Captures\\" + akhir;

        public void Save()
        {
            using (var db = new OcphDbContext())
            {
                db.GreedyCapture.Insert(this);
            }
        }
    }


    [TableName("BandB")]
    public class CaptureBandBModel   :ICapture
    {

        [DbColumn("Id")]
        public int? Id { get; set; }

        [DbColumn("Guid")]
        public string Guid { get; set; }

        [DbColumn("awal")]
        public string awal { get; set; }

      

        [DbColumn("akhir")]
        public string akhir { get; set; }
      

        [DbColumn("waktu")]
        public long waktu { get; set; }


        [DbColumn("board")]
        public int Board { get; set; }

        public TimeSpan Time
        {
            get { return TimeSpan.FromTicks(waktu); }
            set
            {
                waktu = value.Ticks;
            }
        }
        public string Awal => AppDomain.CurrentDomain.BaseDirectory + "Captures\\" + awal;
        public string Akhir => AppDomain.CurrentDomain.BaseDirectory + "Captures\\" + akhir;

        public void Save()
        {
            using (var db = new OcphDbContext())
            {
                db.BandBCapture.Insert(this);
            }
        }

    }



    public interface ICapture
    {

        int? Id { get; set; }

        string Guid { get; set; }

        string awal { get; set; }

        string Awal { get;  }
        string Akhir { get;  }


        string akhir { get; set; }


        long waktu { get; set; }

        int Board { get; set; }


        TimeSpan Time { get; set; }

        void Save();
    }
}
