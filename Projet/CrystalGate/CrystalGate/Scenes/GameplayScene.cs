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
        Body boundary; // Les limtes du monde physique
        private Wave wave;

        public static List<SoundEffect> _effetsSonores = new List<SoundEffect> { }; // Tous les effets sonores.
        private List<Joueur> joueurs = new List<Joueur> { }; // joueurs sur la map
        private List<Objet> unites = new List<Objet> { }; // unites sur la map
        private List<Batiment> batiments = new List<Batiment> { };
        private List<Effet> effets = new List<Effet> { }; // effets qui seront draw

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
            pack.unites = new List<Texture2D> { content.Load<Texture2D>("knight"), content.Load<Texture2D>("grunt") };
            pack.sorts.Add(content.Load<Texture2D>("explosion"));
            pack.boutons.Add(content.Load<Texture2D>("Boutons/Explosion"));
            pack.map.Add(content.Load<Texture2D>("summertiles"));
            //
            // Chargement de la carte
            int counter = 0;
            string line; // X
            // Read the file and display it line by line.
            StreamReader file = new StreamReader(@"../../../Map/Map1.txt");

            line = file.ReadLine();
            string line2;
            file = new StreamReader(@"../../../Map/Map1.txt");
            while ((line2 = file.ReadLine()) != null)
                counter++;

            // Creation de la carte
            Texture2D SpriteMap = content.Load<Texture2D>("summertiles");
            map = new Map(SpriteMap, new Vector2(line.Length, counter) , new Vector2(32, 32));

            counter = 0;
            file = new StreamReader(@"../../../Map/Map1.txt");
            while ((line2 = file.ReadLine()) != null)
            {
                for (int i = 0; i < line2.Length; i++)
                {
                    if (line2[i] == '1') // Mur
                    {
                        batiments.Add(new Mur(new Vector2(i, counter), map, pack));
                        map.unitesStatic[i, counter] = new Noeud(new Vector2(i, counter), false, 1);
                    }
                    if (line2[i] == '2') // Arbre ( pas encore implanté)
                    {
                        map.Cellules[i, counter] = new Vector2(12, 6);
                    }
                }
                counter++;
            }
            file.Close();

            // Creation de la physique de la carte
            var bounds = GetBounds();
            boundary = BodyFactory.CreateLoopShape(map.world, bounds);
            boundary.CollisionCategories = Category.All;
            boundary.CollidesWith = Category.All;
            
            // Les sons.
            _effetsSonores.Add(content.Load<SoundEffect>("Sons/sword3")); // Attaque cavalier
            _effetsSonores.Add(content.Load<SoundEffect>("Sons/Cavalierquimeurt"));
            _effetsSonores.Add(content.Load<SoundEffect>("Sons/GruntAttack")); 
            _effetsSonores.Add(content.Load<SoundEffect>("Sons/Gruntquimeurt"));
            EffetSonore.InitEffects();
            // ajout joueurs
            joueurs.Add(new Joueur(new Grunt(new Vector2(2, 8), map, pack)));
            unites.Add(joueurs[0].champion);

            // Interface
            UI Interface = new UI(joueurs[0], content.Load<Texture2D>("UI/Perso and stats"), content.Load<Texture2D>("UI/barre des sorts"), content.Load<Texture2D>("Curseur"), content.Load<Texture2D>("gruntIcone"), spriteBatch, gameFont);
            joueurs[0].Interface = Interface;

            // fixe l'id de toutes les unités
            for (int i = 0; i < unites.Count; i++)
                unites[i].id = i;

            unites.Add(new Unite(Vector2.One, map, pack));
            // La vague
            wave = new Wave(new List<Vector2>{new Vector2(8, 7), new Vector2(8, 8)}, new List<Vector2> { new Vector2(22,0), new Vector2(39,7), new Vector2(23,17) }, new Cavalier(Vector2.Zero, map, pack), 6000, 3, joueurs[0].champion);
        }

        protected override void UnloadContent() 
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
                FondSonore.Update();

                joueurs[0].Update(unites);
                // On update les infos de la map
                map.Update(unites, batiments, gameTime);
                // On update les infos des unites
                foreach (Unite u in unites)
                    u.Update(unites, effets);
                // On update les infos des batiments
                foreach (Batiment b in batiments)
                    b.Update(unites, effets);
                // On update les infos des joueurs
                foreach (Joueur j in joueurs)
                    j.Update(unites);
                // On update les effets sur la carte
                foreach (Effet e in effets)
                    e.Update();

                wave.Update(gameTime);
                // Update de la physique
                map.world.Step(1 / 60f);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SceneManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);
            SpriteBatch spriteBatch = SceneManager.SpriteBatch;

            spriteBatch.Begin(0, null, null, null, null, null, joueurs[0].camera.CameraMatrix);
            // DRAW MAP
            map.Draw(spriteBatch);
            // DRAW EFFETS
            foreach (Effet e in effets)
                e.Draw(spriteBatch);
            // DRAW UNITES
            foreach (Unite o in unites)
                o.Draw(spriteBatch);
            // DRAW BATIMENTS
            foreach (Batiment b in batiments)
                b.Draw(spriteBatch);
            // DRAW INTERFACE
            joueurs[0].Interface.Draw();
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
        // Utilisé pour creer le monde physique
        private Vertices GetBounds()
        {
            float width = ConvertUnits.ToSimUnits(map.Taille.X * map.TailleTiles.X);
            float height = ConvertUnits.ToSimUnits(map.Taille.Y * map.TailleTiles.Y);

            Vertices bounds = new Vertices(4);
            bounds.Add(new Vector2(0, 0));
            bounds.Add(new Vector2(width, 0));
            bounds.Add(new Vector2(width, height));
            bounds.Add(new Vector2(0, height));

            return bounds;
        }
    }
}
