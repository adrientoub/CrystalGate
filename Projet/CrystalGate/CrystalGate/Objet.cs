﻿using System;
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
        public Vector2 PositionTile { get; set; }
        public Map Map { get; set; }
        public Body body { get; set; }

        public List<Vector2> Animation { get; set; }
        public PackTexture packTexture { get; set; }
        public Vector2 Tiles { get; set; }

        protected SpriteBatch spriteBatch { get; set; }
        protected Rectangle SpritePosition { get; set; }
        protected int AnimationCurrent { get; set; }

        protected const int AnimationLimite = 4;
        public const int suivrelimite = 20;
        public int suivreactuel = 20;
        public int debug = 20;

        protected Direction direction { get; set; }
        protected bool FlipH { get; set; }
        public bool Mort { get; set; }
        public List<Noeud> ObjectifListe { get; set; }

        public Unite uniteSuivi { get; set; }
        public Unite uniteAttacked { get; set; }

        public Objet(Texture2D Sprite, Vector2 Position, Map Map, SpriteBatch spriteBatch, PackTexture packTexture)
        {
            // Général
            this.Map = Map;
            this.ObjectifListe = new List<Noeud> { };

            // Physique
            this.body = BodyFactory.CreateRectangle(this.Map.world, ConvertUnits.ToSimUnits(32), ConvertUnits.ToSimUnits(32), 10f);
            this.body.Position = ConvertUnits.ToSimUnits(Position * Map.TailleTiles + new Vector2(16,16));
            this.body.IsStatic = false;
            this.body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            this.body.UserData = this;
            this.body.Mass = 100;
            
            // Graphique
            this.Sprite = Sprite;
            this.Animation = new List<Vector2> { };
            this.spriteBatch = spriteBatch;
            this.packTexture = packTexture;
            this.direction = Direction.Bas;
            this.FlipH = false;

            //* GRUNT */ this.Tiles = new Vector2( 380 / 5, 600 / 11);
            /* KNIGHT */ this.Tiles = new Vector2(370 / 5, 835 / 11);
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            Unite objet1 = (Unite)(fixtureA.Body.UserData);
            Unite objet2 = (Unite)(fixtureB.Body.UserData);

            if (objet1.uniteSuivi == null && debug > 20)
            {
                if (objet1.ObjectifListe.Count > 0)
                {
                    Vector2 position = new Vector2((int)(ConvertUnits.ToDisplayUnits(objet1.body.Position.X) / 32), (int)(ConvertUnits.ToDisplayUnits(objet1.body.Position.Y) / 32));
                    List<Noeud> chemin = PathFinding.TrouverChemin(position, objet1.ObjectifListe[objet1.ObjectifListe.Count - 1].Position, objet1.Map.Taille, Map.unites, false);
                    if (chemin != null)
                        objet1.ObjectifListe = chemin;
                    debug = 0;
                }
            }
            else
                debug++;


                return true;
        }

        public virtual void Update(List<Objet> unitsOnMap, List<Effet> effets)
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
                case Direction.Haut: Animation = PackAnimation.StandHaut();
                    break;
                case Direction.Bas: Animation = PackAnimation.StandBas();
                    break;
                case Direction.Gauche: Animation = PackAnimation.StandGauche();
                    break;
                case Direction.Droite: Animation = PackAnimation.StandDroite();
                    break;
                case Direction.BasDroite: Animation = PackAnimation.StandDroite();
                    break;
                case Direction.BasGauche: Animation = PackAnimation.StandDroite();
                    break;
                case Direction.HautDroite: Animation = PackAnimation.StandDroite();
                    break;
                case Direction.HautGauche: Animation = PackAnimation.StandDroite();
                    break;
                default: Animation = PackAnimation.StandBas();
                    break;
            }
        }

        public virtual void Draw()
        {
            spriteBatch.Draw(Sprite, ConvertUnits.ToDisplayUnits(body.Position), SpritePosition, Color.White, 0f, new Vector2(Tiles.X / 2, Tiles.Y / 2), 1f, FlipH ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
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
