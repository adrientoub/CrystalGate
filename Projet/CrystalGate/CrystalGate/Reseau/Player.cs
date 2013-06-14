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
        public Noeud objectifPoint; // Le point ou cherche à aller le joueur
        public byte idUniteAttacked; // L'unité attaqué par le joueur

        public byte idSortCast; // Le sort cast par le joueur
        public float pointSortX;
        public float pointSortY;
        public byte idUniteCibleCast;
    }
}
