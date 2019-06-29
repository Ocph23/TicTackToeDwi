using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Models
{
    [TableName("Player")]
    public class PlayerModel
    {
        private ScoreModel score;

        [DbColumn("Id")]
        public int? Id { get; set; }

        [DbColumn("Name")]
        public string Name { get; set; }

        public ScoreModel Score
        {
            get
            {

                if (score == null)
                    score = GetScore();

                return score;
            }
            set { score = value; }
        }

        public string GuidData { get;  set; }

        private ScoreModel GetScore()
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    if (Id>0)
                    {
                        return db.Scores.Where(O => O.PlayerId == Id.Value).FirstOrDefault();
                    }
                    
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        public bool SaveChange()
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    if (Id!=null && Id>0)
                    {
                        int id = Id.Value;
                        if (!db.Players.Update(O => new { O.Name }, this, O => O.Id == id))
                            throw new SystemException("Data Tidak Tersipan");
                    }else
                    {
                        Id = db.Players.InsertAndGetLastID(this);
                        if (Id != null && Id == 0)
                        {
                            Id = null;
                            throw new SystemException("Data Tidak Tersipan");
                        }

                    }
                    return true;
                    
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }



        public bool SaveScore(ScoreModel board3, ScoreModel board4)
        {
            try
            {
                using (var db = new OcphDbContext())
                {

                    if (board3!=null &&  board3.HaveValue)
                    {
                        board3.Id = db.Scores.InsertAndGetLastID(board3);
                        if (board3.Id <= 0)
                        {
                            board3.Id = null;
                            throw new SystemException("Data Tidak Tersipan");
                        }

                    }

                    if (board4 != null &&  board4.HaveValue)
                    {

                        board4.Id = db.Scores.InsertAndGetLastID(board4);
                        if (board4.Id <= 0)
                        {
                            board4.Id = null;
                            throw new SystemException("Data Tidak Tersipan");
                        }

                    }

                    return true;

                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }
    }
}
