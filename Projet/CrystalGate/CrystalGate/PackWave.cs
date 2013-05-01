using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    class PackWave
    {
        static Unite champion = PackMap.j.champion;


        static Wave Level1Wave1()
        {
            List<Vector2> PointsInit = new List<Vector2> { new Vector2(1, 8), new Vector2(1, 9), new Vector2(1, 10) };
            List<Vector2> PointsSpawn = new List<Vector2> { new Vector2(11, 0), new Vector2(31, 0), new Vector2(22, 20) };

            List<List<Unite>> Mobs = new List<List<Unite>> { 
                new List<Unite> { new Ogre(Vector2.Zero), new Demon(Vector2.Zero), new Grunt(Vector2.Zero) },
                new List<Unite> { new Grunt(Vector2.Zero), new Cavalier(Vector2.Zero), new Archer(Vector2.Zero) }
            };

            return new Wave(PointsInit, PointsSpawn, Mobs, champion);
        }

        static Wave Level1Wave2()
        {
            int level = 2;

            List<Vector2> PointsInit = new List<Vector2> { new Vector2(21, 27), new Vector2(22, 27), new Vector2(23, 27), new Vector2(24, 27) };
            List<Vector2> PointsSpawn = new List<Vector2> { new Vector2(0, 39), new Vector2(21, 49), new Vector2(37, 49) };

            List<List<Unite>> Mobs = new List<List<Unite>> { 
                new List<Unite> { new Ogre(Vector2.Zero, level), new Demon(Vector2.Zero, level), new Ogre(Vector2.Zero,level) },
                new List<Unite> { new Grunt(Vector2.Zero, level), new Cavalier(Vector2.Zero, level), new Archer(Vector2.Zero,level) }
            };

            return new Wave(PointsInit, PointsSpawn, Mobs, champion);
        }

        static Wave Level1Wave3()
        {
            int level = 3;

            List<Vector2> PointsInit = new List<Vector2> { new Vector2(52, 38), new Vector2(52, 39), new Vector2(52, 40), new Vector2(52, 41) };
            List<Vector2> PointsSpawn = new List<Vector2> { new Vector2(62, 49), new Vector2(87, 49), new Vector2(99, 34), new Vector2(99, 43) };

            List<List<Unite>> Mobs = new List<List<Unite>> { 
                new List<Unite> { new Ogre(Vector2.Zero,level), new Demon(Vector2.Zero,level), new Ogre(Vector2.Zero,level), new Grunt(Vector2.Zero,level) },
                new List<Unite> { new Grunt(Vector2.Zero,level), new Cavalier(Vector2.Zero,level), new Ogre(Vector2.Zero,level), new Archer(Vector2.Zero,level) }
            };

            return new Wave(PointsInit, PointsSpawn, Mobs, champion);
        }

        static Wave Level1Wave4()
        {
            List<Vector2> PointsInit = new List<Vector2> { new Vector2(66, 22), new Vector2(67, 22), new Vector2(68, 22), new Vector2(83, 22), new Vector2(84, 22), new Vector2(85, 22) };
            List<Vector2> PointsSpawn = new List<Vector2> { new Vector2(66, 0), new Vector2(85, 0) };

            List<List<Unite>> Mobs = new List<List<Unite>> { 
                new List<Unite> { new Ogre(Vector2.Zero), new Demon(Vector2.Zero), new Grunt(Vector2.Zero), new Cavalier(Vector2.Zero), new Archer(Vector2.Zero), new Cavalier(Vector2.Zero), new Archer(Vector2.Zero), new Cavalier(Vector2.Zero), new Archer(Vector2.Zero), new Cavalier(Vector2.Zero), new Archer(Vector2.Zero) },
                new List<Unite> { new Grunt(Vector2.Zero), new Cavalier(Vector2.Zero), new Archer(Vector2.Zero), new Cavalier(Vector2.Zero), new Archer(Vector2.Zero), new Cavalier(Vector2.Zero), new Archer(Vector2.Zero), new Cavalier(Vector2.Zero), new Archer(Vector2.Zero), new Cavalier(Vector2.Zero), new Archer(Vector2.Zero) }
            };

            return new Wave(PointsInit, PointsSpawn, Mobs, champion);
        }

        public static List<Wave> PackWaveLevel1()
        {
            return new List<Wave> { Level1Wave1(), Level1Wave2(), Level1Wave3(), Level1Wave4()};
        }

    }
}
