using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    [Serializable]
    class Player
    {
        public bool Mooved;
        public float objectifPointX; // Le point ou cherche à aller le joueur
        public float objectifPointY;
        public byte idUniteAttacked; // L'unité attaqué par le joueur

        public byte idSortCast; // Le sort cast par le joueur
        public float pointSortX;
        public float pointSortY;
        public byte idUniteCibleCast;

        public byte[] LastDeath = new byte[10];
        public int LastItemUsed = -1;
        public int LastStuffUsed = -1;
        public string level;
    }
}
