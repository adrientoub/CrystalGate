﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CrystalGate
{
    public class Unite : Objet
    {
        public int Vie { get; set; }
        public int VieMax { get; set; }
        public int Mana { get; set; }
        public int ManaMax { get; set; }
        public float Vitesse { get; set; }
        public float Portee { get; set; }
        public int Dommages { get; set; }
        public float Vitesse_Attaque { get; set; }
        public int Defense { get; set; }
        public Color color { get; set; }
        public Vector2 pointCible;

        protected EffetSonore effetUniteAttaque;
        protected EffetSonore effetUniteDeath;
        protected int nbFrameSonJoue;

        public List<Spell> spells { get; set; }
        public bool Drawlife { get; set; }
        public double idWave { get; set; }

        public Unite(Vector2 Position, Map map, PackTexture packTexture)
            : base(Position, map, packTexture)
        {
            // Constructeur par defaut d'une unité
            Vie = VieMax = 1;
            Mana = ManaMax = 200;
            Vitesse = 1.0f;
            Portee = 2; // 2 = Corps à corps
            Dommages = 1;
            effetUniteAttaque = new EffetSonore(0);
            effetUniteDeath = new EffetSonore(1);
            nbFrameSonJoue = 0;
            Vitesse_Attaque = 1.00f;
            Defense = 0;
            // Graphique par defaut
            Sprite = packTexture.blank;
            Tiles = Vector2.One;
            color = Color.White; 
            
            idWave = -1;
            spells = new List<Spell> { new Explosion(this), new Soin(this) };
        }

        public override void Update(List<Unite> unitsOnMap, List<Effet> effets)
        {
            Animer(); 
            Deplacer();
            TestMort(effets);
            if(!isAChamp)
                IA(unitsOnMap);

            // Pour Update et Draw les sorts
            foreach (Spell s in spells)
                if (s.ToDraw)
                    s.Update();

            // On rafraichit la propriete suivante, elle est juste indicative et n'affecte pas le draw, mais le pathfinding
            PositionTile = new Vector2((int)(ConvertUnits.ToDisplayUnits(body.Position.X) / Map.TailleTiles.X), (int)(ConvertUnits.ToDisplayUnits(body.Position.Y) / Map.TailleTiles.Y));

            if (uniteAttacked != null)
                Attaquer(uniteAttacked);
        }

        public void IA(List<Unite> unitsOnMap)
        {
            // Cast un heal si < à la moitié de vie
            if (Vie <= VieMax / 2 && IsCastable(1))
                Cast(1, Vector2.Zero);
        }

        public virtual void TestMort(List<Effet> effets) // aussi la mana
        {
            if (Vie <= 0 && !Mort)
            {
                Vie = 0;
                Mort = true;
                effetUniteDeath.Play();
                effetUniteAttaque.Dispose();
                effets.Add(new Effet(Sprite, ConvertUnits.ToDisplayUnits(body.Position), packAnimation.Mort(), new Vector2(370 / 5, 835 / 11), 1));
                Map.world.RemoveBody(body);
            }
            // TEST MANA
            if (Mana < 0)
                Mana = 0;
        }

        public bool IsCastable(int idSort)
        {
            return Map.gametime.TotalGameTime.TotalMilliseconds - spells[idSort].LastCast > spells[idSort].Cooldown * 1000 && Mana >= spells[idSort].CoutMana && Mana > 0;
        } // Indique si un sort est castable

        public virtual void Attaquer(Unite unite)
        {
            if (Outil.DistanceUnites(this, unite) >= Portee * Map.TailleTiles.X)
                Suivre(unite);
            else
            {
                ObjectifListe.Clear();
                body.LinearVelocity = Vector2.Zero;
                // Fait regarder l'unité vers l'unité attaqué
                if (Animation.Count == 0)
                {
                    AnimationCurrent = AnimationLimite;
                    FlipH = false;
                    float angle = Outil.AngleUnites(this, unite);

                    if (angle >= Math.PI / 4 && angle <= 3 * Math.PI / 4)
                        direction = Direction.Haut;
                    if (angle >= - 3 * Math.PI / 4 && angle <= - Math.PI / 4)
                        direction = Direction.Bas;
                    if (angle >= -Math.PI / 4 && angle <= Math.PI / 4)
                    {
                        direction = Direction.Gauche;
                        FlipH = true;
                    }
                    if (angle >= 3 * Math.PI / 4 || angle <= Math.PI / 4)
                        direction = Direction.Droite;

                }

                if (Map.gametime.TotalGameTime.TotalMilliseconds - LastAttack > Vitesse_Attaque * 1000) // Si le cooldown est fini
                {
                    LastAttack = (float)Map.gametime.TotalGameTime.TotalMilliseconds; // On met à jour "l'heure de la dernière attaque"

                    effetUniteAttaque.Play();

                    unite.Vie -= Dommages - unite.Defense;
                    
                    // Fait regarder l'unité vers l'unité attaqué et l'anime
                    if (Animation.Count == 0)
                    {
                        AnimationCurrent = AnimationLimite;
                        FlipH = false;
                        float angle = Outil.AngleUnites(this, unite);

                        if (angle >= Math.PI / 4 && angle <= 3 * Math.PI / 4)
                        {
                            direction = Direction.Haut;
                            Animation = packAnimation.AttaquerHaut();
                        }
                        if (angle >= -3 * Math.PI / 4 && angle <= -Math.PI / 4)
                        {
                            direction = Direction.Bas;
                            Animation = packAnimation.AttaquerBas();
                        }
                        if (angle >= -Math.PI / 4 && angle <= Math.PI / 4)
                        {
                            direction = Direction.Gauche;
                            FlipH = true;
                            Animation = packAnimation.AttaquerDroite();
                        }
                        if (angle >= 3 * Math.PI / 4 || angle <= Math.PI / 4)
                        {
                            direction = Direction.Droite;
                            Animation = packAnimation.AttaquerDroite();
                        }
                    }
                }
            }
        }

        public virtual void Deplacer()
        {
            if (ObjectifListe.Count > 0)
            {  // Bug, je sais pas pourquoi
                // body.Position = ConvertUnits.ToSimUnits(new Vector2((float)Math.Round(ConvertUnits.ToDisplayUnits(body.Position.X)), (float)Math.Round(ConvertUnits.ToDisplayUnits(body.Position.Y) )));
                Vector2 VecMap = new Vector2(0, 0);
                // HAUT GAUCHE
                if (PositionTile.X > ObjectifListe[0].Position.X && PositionTile.Y > ObjectifListe[0].Position.Y)
                {
                    body.LinearVelocity = new Vector2(-Vitesse / 1.41f, -Vitesse / 1.41f);
                    FlipH = true;

                    if (direction != Direction.HautGauche || Animation.Count == 0)
                        Animation = packAnimation.HautDroite();
                    direction = Direction.HautGauche;
                }
                // HAUT DROITE
                else if (PositionTile.X < ObjectifListe[0].Position.X && PositionTile.Y > ObjectifListe[0].Position.Y)
                {
                    body.LinearVelocity = new Vector2(Vitesse / 1.41f, -Vitesse / 1.41f);
                    FlipH = false;

                    if (direction != Direction.HautDroite || Animation.Count == 0)
                        Animation = packAnimation.HautDroite();
                    direction = Direction.HautDroite;
                }
                // BAS DROITE
                else if (PositionTile.X < ObjectifListe[0].Position.X && PositionTile.Y < ObjectifListe[0].Position.Y)
                {
                    body.LinearVelocity = new Vector2(Vitesse / 1.41f, Vitesse / 1.41f);
                    FlipH = false;

                    if (direction != Direction.BasDroite || Animation.Count == 0)
                        Animation = packAnimation.BasDroite();
                    direction = Direction.BasDroite;
                }
                // BAS GAUCHE
                else if (PositionTile.X > ObjectifListe[0].Position.X && PositionTile.Y < ObjectifListe[0].Position.Y)
                {
                    body.LinearVelocity = new Vector2(-Vitesse / 1.41f, Vitesse / 1.41f);
                    FlipH = true;

                    if (direction != Direction.BasGauche || Animation.Count == 0)
                        Animation = packAnimation.BasDroite();
                    direction = Direction.BasGauche;
                }
                // GAUCHE
                else if (PositionTile.X > ObjectifListe[0].Position.X)
                {
                    body.LinearVelocity = new Vector2(-Vitesse, 0);
                    FlipH = true;

                    if (direction != Direction.Gauche || Animation.Count == 0)
                        Animation = packAnimation.Droite();
                    direction = Direction.Gauche;
                }
                // DROITE
                else if (PositionTile.X < ObjectifListe[0].Position.X)
                {
                    body.LinearVelocity = new Vector2(Vitesse, 0);
                    FlipH = false;

                    if (direction != Direction.Droite || Animation.Count == 0)
                        Animation = packAnimation.Droite();
                    direction = Direction.Droite;
                }
                // HAUT
                else if (PositionTile.Y > ObjectifListe[0].Position.Y)
                {
                    body.LinearVelocity = new Vector2(0, -Vitesse);
                    FlipH = false;

                    if (direction != Direction.Haut || Animation.Count == 0)
                        Animation = packAnimation.Haut();
                    direction = Direction.Haut;
                }
                // BAS
                else if (PositionTile.Y < ObjectifListe[0].Position.Y)
                {
                    body.LinearVelocity = new Vector2(0, Vitesse);
                    FlipH = false;

                    if (direction != Direction.Bas || Animation.Count == 0)
                        Animation = packAnimation.Bas();
                    direction = Direction.Bas;
                }
                else
                    ObjectifListe.RemoveAt(0);

            }
            else
                body.LinearVelocity *= 0.01f;
               
        }

        public void Suivre(Unite unite)
        {
            if (this.id >= Map.compteur && this.id <= Map.compteur + Map.pFParThread) // Si on a la possiblité d'obtnir un pF
            {
                List<Unite> liste = new List<Unite> { };
                double distance = Outil.DistanceUnites(this, unite);
                bool ok = distance > Portee * Map.TailleTiles.X;
                if (ok)
                {
                    foreach (Unite u in Map.unites)
                        if (Outil.DistanceUnites(this, u) <= 1 * Map.TailleTiles.X)
                        {
                            if (u != unite && u != this)
                                liste.Add(u);
                        }

                    suivreactuel = 0;
                    List<Noeud> chemin = PathFinding.TrouverChemin(PositionTile, unite.PositionTile, Map.Taille, liste, Map.unitesStatic, false);
                    if (chemin != null)
                    {
                        ObjectifListe = chemin;
                        ObjectifListe.RemoveAt(0);
                    }
                    else
                    {
                        ObjectifListe.Clear();
                        body.LinearVelocity = Vector2.Zero;
                    }
                }
                else
                    suivreactuel++;
            }
            
        }

        public void Cast(int i, Vector2 point)
        {
            // Cast ou initialise le sort
            spells[i].Begin(point);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, ConvertUnits.ToDisplayUnits(body.Position), SpritePosition, color, 0f, new Vector2(Tiles.X / 2, Tiles.Y / 2), 1f, FlipH ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            DrawVie(spriteBatch);

            foreach (Spell s in spells)
                if (s.ToDraw)
                    s.Draw(spriteBatch);
        }

        private void DrawVie(SpriteBatch spriteBatch)
        {
            if (Drawlife)
            {
                int largeur = 10;
                int longueur = (int)((float)Vie / (float)VieMax * 50);
                spriteBatch.Draw(packTexture.blank, ConvertUnits.ToDisplayUnits(body.Position) + new Vector2(0, -30), new Rectangle(0, 0, longueur, largeur), Color.Green, 0f, new Vector2(longueur / 2, largeur / 2), 1f, SpriteEffects.None, 0);

            }
        }

    }
}
