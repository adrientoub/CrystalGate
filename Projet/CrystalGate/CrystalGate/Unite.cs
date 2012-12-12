using System;
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
        public float Vitesse { get; set; }
        public float Portee { get; set; }
        public int Dommages { get; set; }
        protected EffetSonore effetUnite;
        protected int nbFrameSonJoue;

        public bool Drawlife { get; set; }

        public Unite(Vector2 Position, Map map, SpriteBatch spriteBatch, PackTexture packTexture)
            : base(Position, map, spriteBatch, packTexture)
        {
            // Constructeur par default d'une unité
            Vie = VieMax = 1;
            Vitesse = 1.0f;
            Portee = 1; // 1 = Corps à corps
            Dommages = 1;
            effetUnite = new EffetSonore(0);
            nbFrameSonJoue = 0;
        }

        public override void Update(List<Objet> unitsOnMap, List<Effet> effets)
        {
            Animer(); 
            Deplacer();
            TestMort(effets);
            // On rafraichit la propriete suivante, elle est juste indicative et n'affecte pas le draw, mais le pathfinding
            PositionTile = new Vector2((int)(ConvertUnits.ToDisplayUnits(body.Position.X) / 32), (int)(ConvertUnits.ToDisplayUnits(body.Position.Y) / 32));
        }

        public virtual void TestMort(List<Effet> effets)
        {
            if (Vie <= 0 && !Mort)
            {
                Mort = true;
                effetUnite.Dispose();
                effets.Add(new Effet(Sprite, ConvertUnits.ToDisplayUnits(body.Position), PackAnimation.Mort(), new Vector2(370 / 5, 835 / 11), 1));
                Map.world.RemoveBody(body);
            }
        }

        public virtual void Attaquer(Unite unite)
        {
            if (Outil.DistanceUnites(this, unite) >= Portee * Map.TailleTiles.X)
                Suivre(unite);
            else
            {
                // uniteSuivi = null;  Source de lags

                /* Ce if else permet que les sons ne soient pas joués trop fréquemment. 
                 * Mais je pense pas que les unités devraient attaquer aussi souvent.
                 * Chaque attaque aura un cooldown (ou un temps en tout cas). Qui fera que le son sera joué (beaucoup) moins souvent.
                 * Actuellement les unités attaquent en continu...
                 */
                if (nbFrameSonJoue == 0 || nbFrameSonJoue == 50) 
                {
                    effetUnite.Play();
                    nbFrameSonJoue = 0;
                }
                else
                    nbFrameSonJoue++;

                unite.Vie -= Dommages;
                
                if (Animation.Count == 0)
                {
                    AnimationCurrent = AnimationLimite;
                    FlipH = false;
                    float angle = Outil.AngleUnites(this, unite);
                    
                     if (unite.PositionTile.Y < this.PositionTile.Y)
                        Animation = PackAnimation.AttaquerHaut();
                     if (unite.PositionTile.X < this.PositionTile.X)
                    {
                        FlipH = true;
                        Animation = PackAnimation.AttaquerDroite();
                    }
                     if (unite.PositionTile.Y >= this.PositionTile.Y)
                        Animation = PackAnimation.AttaquerBas();
                     if (unite.PositionTile.Y >= this.PositionTile.Y)
                         Animation = PackAnimation.AttaquerDroite();
                }
            }
        }

        public virtual void Deplacer()
        {
            if (ObjectifListe.Count > 0)
            {
                body.Position = ConvertUnits.ToSimUnits(new Vector2((float)Math.Round(ConvertUnits.ToDisplayUnits(body.Position.X)), (float)Math.Round(ConvertUnits.ToDisplayUnits(body.Position.Y) )));
                Vector2 VecMap = new Vector2(0, 0);
                // HAUT GAUCHE
                if (PositionTile.X > ObjectifListe[0].Position.X && PositionTile.Y > ObjectifListe[0].Position.Y)
                {
                    body.LinearVelocity = new Vector2(-Vitesse / 1.41f, -Vitesse / 1.41f);
                    FlipH = true;

                    if (direction != Direction.HautGauche || Animation.Count == 0)
                        Animation = PackAnimation.HautDroite();
                    direction = Direction.HautGauche;
                }
                // HAUT DROITE
                else if (PositionTile.X < ObjectifListe[0].Position.X && PositionTile.Y > ObjectifListe[0].Position.Y)
                    {
                        body.LinearVelocity = new Vector2(Vitesse / 1.41f, -Vitesse / 1.41f);
                        FlipH = false;

                        if (direction != Direction.HautDroite || Animation.Count == 0)
                            Animation = PackAnimation.HautDroite();
                        direction = Direction.HautDroite;
                    }
                    // BAS DROITE
                else if (PositionTile.X < ObjectifListe[0].Position.X && PositionTile.Y < ObjectifListe[0].Position.Y)
                {
                    body.LinearVelocity = new Vector2(Vitesse / 1.41f, Vitesse / 1.41f);
                    FlipH = false;

                    if (direction != Direction.BasDroite || Animation.Count == 0)
                        Animation = PackAnimation.BasDroite();
                    direction = Direction.BasDroite;
                }
                // BAS GAUCHE
                else if (PositionTile.X > ObjectifListe[0].Position.X && PositionTile.Y < ObjectifListe[0].Position.Y)
                {
                    body.LinearVelocity = new Vector2(-Vitesse / 1.41f, Vitesse / 1.41f);
                    FlipH = true;

                    if (direction != Direction.BasGauche || Animation.Count == 0)
                        Animation = PackAnimation.BasDroite();
                    direction = Direction.BasGauche;
                }
                // GAUCHE
                else if (PositionTile.X > ObjectifListe[0].Position.X)
                {
                    body.LinearVelocity = new Vector2(-Vitesse, 0);
                    FlipH = true;

                    if (direction != Direction.Gauche || Animation.Count == 0)
                        Animation = PackAnimation.Droite();
                    direction = Direction.Gauche;
                }
                // DROITE
                else if (PositionTile.X < ObjectifListe[0].Position.X)
                {
                    body.LinearVelocity = new Vector2(Vitesse, 0);
                    FlipH = false;

                    if (direction != Direction.Droite || Animation.Count == 0)
                        Animation = PackAnimation.Droite();
                    direction = Direction.Droite;
                }
                // HAUT
                else if (PositionTile.Y > ObjectifListe[0].Position.Y)
                {
                    body.LinearVelocity = new Vector2(0, -Vitesse);
                    FlipH = false;

                    if (direction != Direction.Haut || Animation.Count == 0)
                        Animation = PackAnimation.Haut();
                    direction = Direction.Haut;
                }
                // BAS
                else if (PositionTile.Y < ObjectifListe[0].Position.Y)
                {
                    body.LinearVelocity = new Vector2(0, Vitesse);
                    FlipH = false;

                    if (direction != Direction.Bas || Animation.Count == 0)
                        Animation = PackAnimation.Bas();
                    direction = Direction.Bas;
                }
                else
                    ObjectifListe.RemoveAt(0);

            }
            else
                body.LinearVelocity = Vector2.Zero;
               
        }

        public void Suivre(Unite unite)
        {
            List<Objet> liste = new List<Objet> { };

                double distance = Outil.DistanceUnites(this, unite);
                bool ok = distance > Portee * Map.TailleTiles.X;
                if (ok)
                {
                    foreach (Unite u in Map.unites)
                        if (Outil.DistanceUnites(this, u) <= 2 * Map.TailleTiles.X)
                        {
                            if (u != unite && u != this)
                                liste.Add(u);
                        }

                    suivreactuel = 0;
                    List<Noeud> chemin = PathFinding.TrouverChemin(PositionTile, unite.PositionTile, Map.Taille, liste, false);
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

                uniteSuivi = unite;
            
        }

        public void Patrouiller(Vector2 point1, Vector2 point2)
        {

        }

        public override void Draw()
        {
            Vector2 lol = ConvertUnits.ToDisplayUnits(body.Position);
            spriteBatch.Draw(Sprite, ConvertUnits.ToDisplayUnits(body.Position), SpritePosition, Color.White, 0f, new Vector2(Tiles.X / 2, Tiles.Y / 2), 1f, FlipH ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            DrawVie();
        }

        private void DrawVie()
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
