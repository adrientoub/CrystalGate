using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using CrystalGate.Inputs;
using CrystalGate.Scenes.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using FarseerPhysics.Common;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using AForge;
using System.IO;

namespace CrystalGate.Scenes
{
    public class GameplayScene : AbstractGameScene
    {
        private ContentManager content;
        private SpriteFont gameFont; // Police d'ecriture
        private PackTexture pack; // Toutes les textures
        private float pauseAlpha;
        private Map map; // La map

        public static List<SoundEffect> _effetsSonores = new List<SoundEffect> { }; // Tous les effets sonores.

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

            FondSonore.Load(ref content);
            FondSonore.Play();

            SpriteBatch spriteBatch = SceneManager.SpriteBatch;
            gameFont = content.Load<SpriteFont>("Polices/gamefont");

            // Pack de texture (Contient toutes les sprites des unites et des sorts)
            pack = new PackTexture(content.Load<Texture2D>("blank"));
            pack.unites = new List<Texture2D> { content.Load<Texture2D>("Unites/champion"), content.Load<Texture2D>("Unites/grunt"), content.Load<Texture2D>("Unites/archer"), content.Load<Texture2D>("Unites/troll"), content.Load<Texture2D>("Unites/demon"), content.Load<Texture2D>("Unites/ogre") };
            pack.sorts.Add(content.Load<Texture2D>("Spells/Explosion"));
            pack.sorts.Add(content.Load<Texture2D>("Spells/Soin"));
            pack.boutons = new List<Texture2D> { content.Load<Texture2D>("Boutons/Explosion"), content.Load<Texture2D>("Boutons/Soin"), content.Load<Texture2D>("Boutons/Invisibility"), content.Load<Texture2D>("Boutons/PotionDeVie") };
            pack.projectiles = new List<Texture2D> { content.Load<Texture2D>("Projectiles/arrow"), content.Load<Texture2D>("Projectiles/axe"), content.Load<Texture2D>("Projectiles/fireball") };
            pack.map.Add(content.Load<Texture2D>("summertiles"));

            // Chargement de la carte
            Outil.OuvrirMap("level1", ref map, pack);

            // Chargement sons
            Outil.LoadSounds(_effetsSonores, content);

            // Ajout joueurs
            map.joueurs.Add(new Joueur(new Cavalier(new Vector2(0, 9), map, pack)));
            map.unites.Add(map.joueurs[0].champion);

            // Ajout Interface
            UI Interface = new UI(map.joueurs[0], content.Load<Texture2D>("UI/barre des sorts"), content.Load<Texture2D>("Curseur"), content.Load<Texture2D>("UI/archerIcone"), content.Load<Texture2D>("UI/inventaire"), content.Load<Texture2D>("blank"), spriteBatch, gameFont);
            map.joueurs[0].Interface = Interface;

            // La vague
            map.waves.Add(new Wave(new List<Vector2> { new Vector2(1, 9), new Vector2(1, 10) }, new List<Vector2> { new Vector2(11, 0), new Vector2(30, 1), new Vector2(23, 21) }, new Ogre(Vector2.Zero, map, pack), 3, map.joueurs[0].champion));

            // Ajout des items
            for(int i = 0; i < 9; i++)
                map.items.Add(new PotionDeVie(new Vector2(1, 3), pack));
        }

        public override void Update(GameTime gameTime, bool othersceneHasFocus, bool coveredByOtherscene)
        {
            base.Update(gameTime, othersceneHasFocus, false);

            pauseAlpha = coveredByOtherscene 
                ? Math.Min(pauseAlpha + 1f / 32, 1) 
                : Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive) // Si le jeu tourne (en gros)
            {
                FondSonore.Update();
                // On update les infos de la map
                map.Update(map.unites, gameTime);
                // On update les infos des joueurs
                map.joueurs[0].Update(map.unites);
                // On update les infos des items
                foreach (Item i in map.items)
                    i.Update(map.unites);
                // On update les infos des unites
                foreach (Unite u in map.unites)
                    u.Update(map.unites, map.effets);
                // On update les infos des joueurs
                foreach (Joueur j in map.joueurs)
                    j.Update(map.unites);
                // On update les effets sur la carte
                foreach (Effet e in map.effets)
                    e.Update();
                // On update les infos des vagues
                foreach(Wave w in map.waves)
                    w.Update(gameTime, map.joueurs[0].champion);
                // Update de la physique
                map.world.Step(1 / 60f);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SceneManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);
            SpriteBatch spriteBatch = SceneManager.SpriteBatch;

            spriteBatch.Begin(0, null, null, null, null, null, map.joueurs[0].camera.CameraMatrix);
            // DRAW MAP
            map.Draw(spriteBatch);
            // DRAW EFFETS
            foreach (Effet e in map.effets)
                e.Draw(spriteBatch);
            // DRAW ITEMS
            foreach (Item i in map.items)
                spriteBatch.Draw(pack.boutons[0], i.Position * map.TailleTiles, Color.White);
            // DRAW UNITES
            foreach (Unite o in map.unites)
                o.Draw(spriteBatch);
            // DRAW INTERFACE
            map.joueurs[0].Interface.Draw();
            // DRAW STRINGS
            /**/
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

    }
}
