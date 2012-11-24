using System;
using System.Threading;
using System.Collections.Generic;
using CrystalGate.Inputs;
using CrystalGate.Scenes.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Common;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;

namespace CrystalGate.Scenes
{
    public class GameplayScene : AbstractGameScene
    {
        private ContentManager content; // Osef
        private SpriteFont gameFont; // Police d'ecriture
        private float pauseAlpha; // Osef
        private Map map; // La map
        Body boundary; // Les limtes du monde physique

        private List<Joueur> joueurs = new List<Joueur> { }; // joueurs sur la map
        private List<Objet> unites = new List<Objet> { }; // unites sur la map
        private List<Effet> effets = new List<Effet> { }; // effes qui seront draw

        public GameplayScene(SceneManager sceneMgr)
            : base(sceneMgr)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5); // temps de transition depuis le menu
            TransitionOffTime = TimeSpan.FromSeconds(0.5); // - vers - 
        }

        protected override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(SceneManager.Game.Services, "Content");

            SpriteBatch spriteBatch = SceneManager.SpriteBatch;
            gameFont = content.Load<SpriteFont>("menufont");

            // Pack de texture (useless pour le moment)
            PackTexture pack = new PackTexture(content.Load<Texture2D>("blank"));
            
            // Creation de la carte
            map = new Map(content.Load<Texture2D>("tile"), 30, new Vector2(32, 32));

            // Creation de la physique de la carte
            var bounds = GetBounds();
            boundary = BodyFactory.CreateLoopShape(map.world, bounds);
            boundary.CollisionCategories = Category.All;
            boundary.CollidesWith = Category.All;

            // ajout joueurs
            joueurs.Add(new Joueur(map));

            // ajout unités
            for (int j = 0; j < 20; j++)
                for (int i = 0; i < 20; i++)
                    if( i  % 2 == 0 && j % 2 == 0)
                        unites.Add(new Unite(content.Load<Texture2D>("knight"), new Vector2(i, j), map, spriteBatch, pack));
        }

        protected override void UnloadContent() // Osef
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool othersceneHasFocus, bool coveredByOtherscene)
        {
            base.Update(gameTime, othersceneHasFocus, false);

            pauseAlpha = coveredByOtherscene 
                ? Math.Min(pauseAlpha + 1f / 32, 1) 
                : Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive) // Si le jeu tourne (en gros)
            {
                KeyboardState k = Keyboard.GetState();
                // On update les infos de la map
                map.Update(unites);
                // On update les infos des unites
                foreach (Objet o in unites)
                    o.Update(unites, effets);
                // On update les infos des joueurs
                foreach (Joueur j in joueurs)
                    j.Update(unites);
                // On update les effets sur la carte
                foreach (Effet e in effets)
                    e.Update();
                // Script temporaire pour se faire chasser par les unites
                if (k.IsKeyDown(Keys.A))
                {
                    foreach (Unite u in unites)
                        if (u != unites[0])
                            u.Attaquer((Unite)unites[0]);
                }
                else
                    foreach (Unite l in unites)
                        if (l != unites[0])
                        {
                            l.body.LinearVelocity = Vector2.Zero;
                            l.ObjectifListe.Clear();
                        }
                // Update de la physique
                map.world.Step(1 / 60f);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SceneManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Orange, 0, 0);
            SpriteBatch spriteBatch = SceneManager.SpriteBatch;

            spriteBatch.Begin();
            // DRAW MAP
            map.Draw(spriteBatch);
            // DRAW EFFETS
            foreach (Effet e in effets)
                e.Draw(spriteBatch);
            // DRAW UNITES
            foreach (Objet o in unites)
                o.Draw();
            // DRAW STRINGS
            //spriteBatch.DrawString(gameFont, PathFinding.Draw(PathFinding.Initialiser(map.Taille,Vector2.Zero, unites)), Vector2.Zero, Color.White);
            spriteBatch.End();

            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);
                SceneManager.FadeBackBufferToBlack(alpha);
            }
        }
        // Gestion des raccourcis des menus
        public override void HandleInput()
        {
            KeyboardState keyboardState = InputState.CurrentKeyboardState;
            GamePadState gamePadState = InputState.CurrentGamePadState;

            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       InputState.GamePadWasConnected;

            if (InputState.IsPauseGame() || gamePadDisconnected)
                new PauseMenuScene(SceneManager, this).Add();
        }
        // Utilisé pour creer le monde physique
        private Vertices GetBounds()
        {
            float width = ConvertUnits.ToSimUnits(1000);
            float height = ConvertUnits.ToSimUnits(1000);

            Vertices bounds = new Vertices(4);
            bounds.Add(new Vector2(0, 0));
            bounds.Add(new Vector2(width, 0));
            bounds.Add(new Vector2(width, height));
            bounds.Add(new Vector2(0, height));

            return bounds;
        }
    }
}
