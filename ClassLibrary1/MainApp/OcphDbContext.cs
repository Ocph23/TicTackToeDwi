using MainApp.Models;
using Ocph.DAL;
using Ocph.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp
{
    public class OcphDbContext : Ocph.DAL.Provider.SQLite.SQLiteDbConnection
    {
        public OcphDbContext()
        {
            if (File.Exists("dbgame.db"))
            {
                this.ConnectionString = "Data Source=dbgame.db";
            }
            else
            {
                this.ConnectionString = "Data Source=dbgame.db;Version=3;New=True;Compress=True;";
                this.Open();

                IDbCommand cmd = CreateCommand();

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS Player (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name VARCHAR(100))";

                IDataReader reader = cmd.ExecuteReader();

                reader.Close();


                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Score (Id INTEGER PRIMARY KEY AUTOINCREMENT, PlayerId INTEGER NOT NULL DEFAULT 0, PlayerWin INTEGER,
                                    ComputerWin INTEGER, Draw INTEGER, DataTime long )";

                reader = cmd.ExecuteReader();

                reader.Close();
            }
        }

        public IRepository<PlayerModel> Players { get { return new Repository<PlayerModel>(this); } }
        public IRepository<ScoreModel> Scores { get { return new Repository<ScoreModel>(this); } }

        internal void IsExist<T>()
        {
            var name = typeof(T).Name;
        }

    }

}