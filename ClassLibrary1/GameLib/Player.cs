using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib
{
   public class Player:BaseNotify
    {
        public Player(string name, PlayerType playerType,PlayerPionType pionType)
        {
            Pions = new List<Pion>();
            this.Name = name;
            this.PlayerType = playerType;
            this.PionType = pionType;
        }

        public int Id { get; set; }
        private string name;

        public string Name
        {
            get { return name; }
            set {SetProperty(ref name ,value); }
        }

        public PlayerType PlayerType { get; set; }
        public PlayerPionType PionType { get; set; }
        public bool IsPlay { get; internal set; }
        public List<Pion> Pions { get; private set; }
        public Pion Selected { get; set; }

        public Task CreateNewPion(int count)
        {
           for(var i=0;i<count;i++)
            {
                Pions.Add(new Pion(i, PionType));
            }
            return Task.FromResult(0);
        }



        public Task<Pion> CreatePion()
        {
            var count = Pions.Count + 1;
            var pion = new Pion(count, PionType);
            Pions.Add(pion);
            return Task.FromResult(pion);
        }
    }
}
