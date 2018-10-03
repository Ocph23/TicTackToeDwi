using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Models
{
    public class PlayerCollection 
    {
        public PlayerCollection(PlayerModel player)
        {
            Player = player;
        }


        private ScoreModel score;

        public ScoreModel Score
        {
            get {

                if (score == null)
                    score = GetScore();

                return score; }
            set { score = value; }
        }

        private ScoreModel GetScore()
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                   if(Player!=null)
                        return db.Scores.Where(O => O.PlayerId == Player.Id).FirstOrDefault();
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public PlayerModel Player { get; }
    }
}
