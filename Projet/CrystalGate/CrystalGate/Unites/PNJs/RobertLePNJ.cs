using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrystalGate.Unites
{
    class Syndra : Assassin
    {
        public Syndra(Vector2 Position)
            : base(Position)
        {
            Vie = 550000;
            isApnj = true;
            Dialogue.Add("Dans la prochaine salle t'attends Odin, soit sur tes gardes!");
            Dialogue.Add("Wesh la famille et tout ?");
        }
    }
}
