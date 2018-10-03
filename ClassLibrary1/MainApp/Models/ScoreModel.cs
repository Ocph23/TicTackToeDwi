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
        private long dataTime;
        private TimeSpan _time;

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

    }
}
