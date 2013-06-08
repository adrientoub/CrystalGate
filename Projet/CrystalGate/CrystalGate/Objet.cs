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
        [Serializable]
    public class Objet
    {
        public Texture2D Sprite;
        public Texture2D Portrait;
        public Texture2D ProjectileSprite;
        public Projectile Projectile;
        public Vector2 PositionTile;
        public Body body;
        public bool isAChamp;
        public bool isInvisible;
        public bool isApnj;

        public List<Vector2> Animation;
        public List<Text> Dialogue;
        public Vector2 Tiles;

        protected Rectangle SpritePosition;
        protected int AnimationCurrent;
        public PackAnimation packAnimation;

        protected const int AnimationLimite = 5;
        public const int suivrelimite = 20;
        public int suivreactuel = 20;
        public int largeurPhysique;

        protected Direction direction;
        protected bool FlipH;
        public float Scale = 1;
        public bool Mort, CanAttack;
        public List<Noeud> ObjectifListe;

        public float LastAttack;
        public Unite OlduniteAttacked;
        public Unite uniteAttacked;

        public Objet(Vector2 Position)
        {
            // Général
            this.ObjectifListe = new List<Noeud> { };
            CanAttack = true;

            // Physique
            largeurPhysique = 1;
            CreateBody(largeurPhysique, Position);
            
            // Graphique
            this.Sprite = PackTexture.blank;
            this.Animation = new List<Vector2> { };
            this.Dialogue = new List<Text> { };
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
                    List<Noeud> chemin = PathFinding.TrouverChemin(position, objet1.ObjectifListe[objet1.ObjectifListe.Count - 1].Position, Map.Taille, Map.unites, Map.unitesStatic, false);
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

        public void CreateBody(int echelle, Vector2 Position)
        {
            this.body = BodyFactory.CreateRectangle(Map.world, ConvertUnits.ToSimUnits(25 * echelle), ConvertUnits.ToSimUnits(25 * echelle), 100f);
            this.body.Position = ConvertUnits.ToSimUnits(Position * Map.TailleTiles + new Vector2(16, 16));
            this.body.IsStatic = false;
            this.body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            this.body.UserData = this;
            this.body.Mass = 100;
            this.PositionTile = Position;
        }
                    [Serializable]
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
