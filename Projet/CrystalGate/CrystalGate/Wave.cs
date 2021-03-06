﻿using System;
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
        double id; // l'id de cette vague
        public bool enabled; // définit si la vague est activé
        Random rand = new Random();

        public Wave(List<Vector2> pointsInit, List<Vector2> pointsSpawn, List<List<Unite>> unites)
        {
            this.PointsInit = pointsInit;
            this.PointsSpawn = pointsSpawn;
            this.unites = unites;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Vector2 v in PointsInit)
                foreach(Joueur j in PackMap.joueurs)
                if (j.champion.PositionTile == v)
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
            foreach (Unite u in Map.unites)
                if (u.idWave == id) // Si il y'a encore des unités de la vague précédente, on ne pop rien
                {
                    NextWave = false;
                    break;
                }
            // Sinon on pop
            if (NextWave && unites.Count > 0)
            {
                Map.waveNumber++;
                while(unites[0].Count != 0) // Tant qu'il y'a des unités a pop
                {
                    foreach (Vector2 v in PointsSpawn) // on les dispatche sur les points de spawn
                    {
                        if (unites[0].Count != 0)
                        {
                            if (unites[0][0] is Cavalier)
                                Map.unites.Add(new Cavalier(v, unites[0][0].Level));
                            else if (unites[0][0] is Grunt)
                                Map.unites.Add(new Grunt(v, unites[0][0].Level));
                            else if (unites[0][0] is Archer)
                                Map.unites.Add(new Archer(v, unites[0][0].Level));
                            else if (unites[0][0] is Troll)
                                Map.unites.Add(new Troll(v, unites[0][0].Level));
                            else if (unites[0][0] is Demon)
                                Map.unites.Add(new Demon(v, unites[0][0].Level));
                            else if (unites[0][0] is Ogre)
                                Map.unites.Add(new Ogre(v, unites[0][0].Level));
                            else if (unites[0][0] is Odin)
                                Map.unites.Add(new Odin(v, unites[0][0].Level));
                            else if (unites[0][0] is Voleur)
                                Map.unites.Add(new Voleur(v, unites[0][0].Level));
                            else
                                throw new Exception("Modif la wave!");
                            
                            // Fait attaquer l'unité la plus proche
                            if (!Map.unites[Map.unites.Count - 1].isAChamp && !Map.unites[Map.unites.Count - 1].isApnj)
                            {
                                float distanceInit = 9000;
                                Unite focus = null;
                                foreach (Unite u in Map.unites)
                                {
                                    float distance = 0;

                                    if (u.isAChamp && (distance = Outil.DistanceUnites(Map.unites[Map.unites.Count - 1], u)) <= distanceInit)
                                    {
                                        distanceInit = distance;
                                        focus = u;
                                    }
                                }
                                Map.unites[Map.unites.Count - 1].uniteAttacked = focus;
                            }

                            //Map.unites[Map.unites.Count - 1].uniteAttacked = focus;
                            Map.unites[Map.unites.Count - 1].idWave = id;
                            unites[0].RemoveAt(0);
                        }
                        else // Si il y'a moins d'unité que de points de spawn, on arrete
                            break;
                    }
                }
                unites.RemoveAt(0);
                Map.nombreDeVaguesPop++;
            }
        }
    }
}
