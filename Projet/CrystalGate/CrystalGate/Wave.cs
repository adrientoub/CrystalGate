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
        Unite champ;
        int NbWaves;
        int current;
        double id;
        public bool enabled;

        public Wave(List<Vector2> pointsInit, List<Vector2> pointsSpawn, Unite unite, int nbWaves, Unite champion)
        {
            this.PointsInit = pointsInit;
            this.PointsSpawn = pointsSpawn;
            this.unite = unite;
            this.champ = champion;
            this.NbWaves = nbWaves;
        }

        public void Update(GameTime gameTime, Unite champion)
        {
            this.champ = champion;
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
            if (id == 0)
                id = GT.TotalGameTime.TotalMilliseconds;

            bool ok = true;
            foreach(Unite u in champ.Map.unites)
                if (u.idWave == id) // Si il y'a encore des unités de la vague précédente
                {
                    ok = false;
                    break;
                }
            // Sinon on pop
            if (ok && current < NbWaves)
            {
                foreach (Vector2 v in PointsSpawn)
                {
                    unite.Map.unites.Add(new Cavalier(v, unite.Map, unite.packTexture));
                    unite.Map.unites[unite.Map.unites.Count - 1].uniteAttacked = champ;
                    ((Unite)unite.Map.unites[unite.Map.unites.Count - 1]).idWave = id;
                }
                current++;
            }
        }
    }
}
