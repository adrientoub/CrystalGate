using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class Wave
    {
        List<Vector2> PointsInit; // Les points ou le joueur marche pour initialiser la vague
        List<Vector2> PointsSpawn;
        public List<List<Unite>> unites; // Listes de listes d'unités à pop
        Unite champ; // le champion à focus
        double id; // l'id de cette vague
        public bool enabled; // définit si la vague est activé

        public Wave(List<Vector2> pointsInit, List<Vector2> pointsSpawn, List<List<Unite>> unites, Unite champion)
        {
            this.PointsInit = pointsInit;
            this.PointsSpawn = pointsSpawn;
            this.unites = unites;
            this.champ = champion;
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
            if (id == 0) // si on commence a pop , on assisgne un id à la vague
                id = GT.TotalGameTime.TotalMilliseconds;

            bool NextWave = true;
            foreach(Unite u in champ.Map.unites)
                if (u.idWave == id) // Si il y'a encore des unités de la vague précédente, on ne pop rien
                {
                    NextWave = false;
                    break;
                }
            // Sinon on pop
            if (NextWave && unites.Count > 0)
            {
                while(unites[0].Count != 0) // Tant qu'il y'a des unités a pop
                {
                    foreach (Vector2 v in PointsSpawn) // on les dispatche sur les points de spawn
                    {
                        if (unites[0].Count != 0)
                        {
                            if (unites[0][0] is Cavalier)
                                unites[0][0].Map.unites.Add(new Cavalier(v, unites[0][0].Map, unites[0][0].packTexture));
                            else if (unites[0][0] is Grunt)
                                unites[0][0].Map.unites.Add(new Grunt(v, unites[0][0].Map, unites[0][0].packTexture));
                            else if (unites[0][0] is Archer)
                                unites[0][0].Map.unites.Add(new Archer(v, unites[0][0].Map, unites[0][0].packTexture));
                            else if (unites[0][0] is Troll)
                                unites[0][0].Map.unites.Add(new Troll(v, unites[0][0].Map, unites[0][0].packTexture));
                            else if (unites[0][0] is Demon)
                                unites[0][0].Map.unites.Add(new Demon(v, unites[0][0].Map, unites[0][0].packTexture));
                            else if (unites[0][0] is Ogre)
                                unites[0][0].Map.unites.Add(new Ogre(v, unites[0][0].Map, unites[0][0].packTexture));
                            else
                                throw new Exception("Modif la wave!");

                            unites[0][0].Map.unites[unites[0][0].Map.unites.Count - 1].uniteAttacked = champ;
                            unites[0][0].Map.unites[unites[0][0].Map.unites.Count - 1].idWave = id;
                            unites[0].RemoveAt(0);
                        }
                        else // Si il y'a moins d'unité que de points de spawn, on arrete
                            break;
                    }
                }
                unites.RemoveAt(0);
            }
        }
    }
}
