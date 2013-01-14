using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class Wave
    {
        List<Vector2> PointsInit;
        List<Vector2> PointsSpawn;
        Unite unite;
        float ite;
        Unite champ;
        int NbWaves;
        int current;
        int Nbalive;
        public bool enabled;

        public Wave(List<Vector2> pointsInit, List<Vector2> pointsSpawn, Unite unite, float ite, int nbWaves, Unite champion)
        {
            this.PointsInit = pointsInit;
            this.PointsSpawn = pointsSpawn;
            this.unite = unite;
            this.ite = ite;
            this.champ = champion;
            this.NbWaves = nbWaves;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Vector2 v in PointsInit)
                if (champ.PositionTile == v)
                {
                    enabled = true;
                    break;
                }

            if (enabled)
                Pop(gameTime);
        }

        public void Pop(GameTime GT)
        {
            if (GT.TotalGameTime.Milliseconds == 1/60 && current < NbWaves)
            {
                foreach (Vector2 v in PointsSpawn)
                {
                    Nbalive = PointsSpawn.Count;
                    unite.Map.unites.Add(new Cavalier(v, unite.Map, unite.packTexture));
                    unite.Map.unites[unite.Map.unites.Count - 1].uniteAttacked = champ;
                }
                current++;
            }
        }
    }
}
