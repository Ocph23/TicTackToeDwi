using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Models
{

    [TableName("Score")]
    public class ScoreModel
    {
      
        [DbColumn("Id")]
        public int? Id { get; set; }

        [DbColumn("PlayerId")]
        public int PlayerId { get; set; }

        [DbColumn("PlayerWin")]
        public int PlayerWin { get; set; }

        [DbColumn("ComputerWin")]
        public int ComputerWin { get; set; }

        [DbColumn("Draw")]
        public int Draw { get; set; }

        [DbColumn("Board")]
        public int Board { get; set; }


        [DbColumn("guid")]
        public string GuidData { get; set; }

        [DbColumn("DataTime")]
        public long DataTime{
            get {
                return dataTime; }
            set
            {
                dataTime = value;
            }
        }


        public TimeSpan Time
        {
            get { return TimeSpan.FromTicks(DataTime); }
            set
            {
                dataTime = value.Ticks;
            }
        }


        public int Game
        {
            get
            {
                return PlayerWin + ComputerWin + Draw;
            }
        }


        public bool HaveValue
        {
            get
            {
                if (Time.Ticks > 0)
                    return true;
                return false;
            }
        }

    

        private long dataTime;
        private TimeSpan _time;


    }
}
