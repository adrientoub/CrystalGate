﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrystalGate.Unites
{
    class RobertLePNJ : Grunt
    {
        public RobertLePNJ(Vector2 Position)
            : base(Position)
        {
            isApnj = true;
            Dialogue.Add("Dans la prochaine salle t'attends Odin, soit sur tes gardes!");
            Dialogue.Add("Wesh la famille et tout ?");
        }
    }
}
