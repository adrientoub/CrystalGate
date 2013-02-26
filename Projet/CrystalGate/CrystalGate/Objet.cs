using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace CrystalGate
{
    public class Objet
    {
        public Texture2D Sprite { get; set; }
        public Texture2D ProjectileSprite { get; set; }
        public Projectile Projectile { get; set; }
        public Vector2 PositionTile { get; set; }
        public Map Map { get; set; }
        public Body body { get; set; }
        public bool isAChamp { get; set; }

        public List<Vector2> Animation { get; set; }
        public PackTexture packTexture { get; set; }
        public Vector2 Tiles { get; set; }

        protected Rectangle SpritePosition { get; set; }
        protected int AnimationCurrent { get; set; }
        public PackAnimation packAnimation { get; set; }

        protected const int AnimationLimite = 5;
        public const int suivrelimite = 20;
        public int suivreactuel = 20;

        protected Direction direction { get; set; }
        protected bool FlipH { get; set; }
        public float Scale = 1;
        public bool Mort { get; set; }
        public List<Noeud> ObjectifListe { get; set; }

        public float LastAttack { get; set; }
        public Unite uniteAttacked { get; set; }

        public Objet(Vector2 Position, Map Map, PackTexture packTexture)
        {
            // Général
            this.Map = Map;
            this.ObjectifListe = new List<Noeud> { };

            // Physique
            this.body = BodyFactory.CreateRectangle(this.Map.world, ConvertUnits.ToSimUnits(25), ConvertUnits.ToSimUnits(25), 100f);
            this.body.Position = ConvertUnits.ToSimUnits(Position * Map.TailleTiles + new Vector2(16,16));
            this.body.IsStatic = false;
            this.body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            this.body.UserData = this;
            this.body.Mass = 100;
            this.PositionTile = Position;
            
            // Graphique
            this.Sprite = packTexture.blank;
            this.Animation = new List<Vector2> { };
            this.packTexture = packTexture;
            this.packAnimation = new PackAnimation();
            this.direction = Direction.Bas;
            this.FlipH = false;
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            Unite objet1 = (Unite)(fixtureA.Body.UserData);

            if (objet1.isAChamp)
            {
                if (objet1.ObjectifListe.Count > 0)
                {
                    Vector2 position = new Vector2((int)(ConvertUnits.ToDisplayUnits(objet1.body.Position.X) / 32), (int)(ConvertUnits.ToDisplayUnits(objet1.body.Position.Y) / 32));
                    List<Noeud> chemin = PathFinding.TrouverChemin(position, objet1.ObjectifListe[objet1.ObjectifListe.Count - 1].Position, objet1.Map.Taille, Map.unites, Map.unitesStatic, false);
                    if (chemin != null)
                        objet1.ObjectifListe = chemin;
                }
            }

            return true;
        }

        public virtual void Update(List<Unite> unitsOnMap, List<Effet> effets)
        {
            Animer();
            PositionTile = new Vector2((int)(ConvertUnits.ToDisplayUnits(body.Position.X) / 32), (int)(ConvertUnits.ToDisplayUnits(body.Position.Y) / 32));
        }

        public void Animer()
        {
            if (AnimationCurrent >= AnimationLimite)
            {
                if (Animation.Count == 0)
                    Stand();
                AnimationCurrent = 0;
                SpritePosition = new Rectangle((int)Animation[0].X * (int)Tiles.X, (int)Animation[0].Y * (int)Tiles.Y, (int)Tiles.X, (int)Tiles.Y);
                Animation.RemoveAt(0);
            }

            else
                AnimationCurrent++;
        }

        public void Stand()
        {
            switch (direction)
            {
                case Direction.Haut: Animation = packAnimation.StandHaut();
                    break;
                case Direction.Bas: Animation = packAnimation.StandBas();
                    break;
                case Direction.Gauche: Animation = packAnimation.StandGauche();
                    break;
                case Direction.Droite: Animation = packAnimation.StandDroite();
                    break;
                case Direction.BasDroite: Animation = packAnimation.StandDroite();
                    break;
                case Direction.BasGauche: Animation = packAnimation.StandDroite();
                    break;
                case Direction.HautDroite: Animation = packAnimation.StandDroite();
                    break;
                case Direction.HautGauche: Animation = packAnimation.StandDroite();
                    break;
                default: Animation = packAnimation.StandBas();
                    break;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, ConvertUnits.ToDisplayUnits(body.Position), SpritePosition, Color.White, 0f, new Vector2(Tiles.X / 2, Tiles.Y / 2), Scale, FlipH ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }

        protected enum Direction
        {
            Haut,
            Bas,
            Gauche,
            Droite,
            HautDroite,
            BasDroite,
            HautGauche,
            BasGauche
        }
    }
}
