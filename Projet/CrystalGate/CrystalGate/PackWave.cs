using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    class PackWave
    {
        Map map;
        PackTexture pack;
        Unite champion;


        public PackWave(Map _map, PackTexture _pack, Unite _champion)
        {
            map = _map;
            pack = _pack;
            champion = _champion;
        }

        public Wave Level1Wave1()
        {
            List<Vector2> PointsInit = new List<Vector2> { new Vector2(1, 8), new Vector2(1, 9), new Vector2(1, 10) };
            List<Vector2> PointsSpawn = new List<Vector2> { new Vector2(11, 0), new Vector2(31, 0), new Vector2(22, 20) };

            List<List<Unite>> Mobs = new List<List<Unite>> { 
                new List<Unite> { new Ogre(Vector2.Zero, map, pack), new Demon(Vector2.Zero, map, pack), new Troll(Vector2.Zero, map, pack) },
                new List<Unite> { new Grunt(Vector2.Zero, map, pack), new Cavalier(Vector2.Zero, map, pack), new Archer(Vector2.Zero, map, pack) }
            };

            return new Wave(PointsInit, PointsSpawn, Mobs, champion);
        }
    }
}
