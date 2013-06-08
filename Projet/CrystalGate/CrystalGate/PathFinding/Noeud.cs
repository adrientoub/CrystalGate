using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
            [Serializable]
    public class Noeud
    {
        public Vector2 Position { get; set; }
        public int CoutG { get; set; }
        public bool IsWalkable { get; set; }
        public int CoutF { get; set; }
        public Noeud Parent { get; set; }

        public Noeud(Vector2 position, bool isWalkable, int cout)
        {
            Position = position;
            IsWalkable = isWalkable;
            CoutG = 1;
            CoutF = cout;
        }

        public Noeud(Vector2 position, bool isWalkable, int cout, Noeud parent)
        {
            Position = position;
            IsWalkable = isWalkable;
            CoutG = 1;
            CoutF = cout;
            Parent = parent;
        }
    }
}
