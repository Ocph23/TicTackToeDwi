﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SharedApp
{
    public delegate void delChangePosition(Position position);
   public class Pion
    {
        public event delChangePosition OnChangePosition;
        public Pion(int id, PlayerPionType type)
        {
            PionType = type;
            Name = id;
        }
      
        public void SetPosition(Position pos)
        {
            if (Position == null)
                Position = pos;
            else
            {
                Position.Row =pos.Row;
                Position.Column = pos.Column;
            }

            OnChangePosition?.Invoke(pos);

        }
        public int Name { get; set; }
        public Position Position { get; set; }
        public PlayerPionType PionType { get; set; }
        public bool OnBoard { get; set; }

    }
}
