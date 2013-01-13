using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class Wave
    {
        int nombre;
        Unite unite;
        float ite;

        public Wave(int nombre, Unite unite, float ite)
        {
            this.nombre = nombre;
            this.unite = unite;
        }

        public void Pop(GameTime GT)
        {
            if (GT.TotalGameTime.TotalMilliseconds % ite == 0)
            {
                unite.PositionTile = new Vector2(10, 10);
                unite.Map.unites.Add(unite);
            }
        }
    }
}
