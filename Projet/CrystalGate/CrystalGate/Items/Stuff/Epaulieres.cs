using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class Epaulieres : Stuff
    {

        public Epaulieres(Vector2 position, PackTexture pack)
            : base(position, pack)
        {
            Icone = pack.boutons[8];
            type = Type.Epaulieres;
        }
    }
}
