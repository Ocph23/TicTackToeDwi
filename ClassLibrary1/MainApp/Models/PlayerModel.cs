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
                    if (Id!=null)
                    {
                        int id = Id.Value;
                        if (!db.Players.Update(O => new { O.Name }, this, O => O.Id == id))
                            throw new SystemException("Data Tidak Tersipan");

                        if(Score!=null)
                        {
                            if(Score.Id==null)
                            {
                               Score.Id = db.Scores.InsertAndGetLastID(Score);
                                if(Score.Id<=0)
                                {
                                    Score.Id = null;
                                    throw new SystemException("Data Tidak Tersipan");
                                }
                            }
                            else
                            {
                                if (!db.Scores.Update(O => new { O.ComputerWin,O.Draw,O,O.PlayerWin,O.DataTime}, Score, O => O.Id == Score.Id))
                                    throw new SystemException("Data Tidak Tersipan");
                            }
                        }

                    }else
                    {
                        var result = db.Players.Select();
                        foreach (var item in result.Where(O => O.Name.Contains(Name)))
                        {
                            if (item.Name.ToUpper() == Name.ToUpper())
                            {
                                throw new SystemException("Data Sudah Ada");
                            }
                        }

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
    }
}
