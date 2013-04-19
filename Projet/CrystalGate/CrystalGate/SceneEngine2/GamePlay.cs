using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using CrystalGate.Inputs;
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

namespace CrystalGate.SceneEngine2
{
    public class GamePlay : BaseScene
    {
        private ContentManager content;
        private SpriteFont gameFont; // Police d'ecriture
        private PackTexture pack; // Toutes les textures
        private Map map; // La map
        public static System.Diagnostics.Stopwatch timer;

        public static List<SoundEffect> _effetsSonores = new List<SoundEffect> { }; // Tous les effets sonores.

        public override void LoadContent()
        {
            if (content == null)
                content = SceneHandler.content;

            FondSonore.Load(content);
            FondSonore.Play();

            gameFont = content.Load<SpriteFont>("Polices/gamefont");

            // Pack de texture (Contient toutes les sprites des unites et des sorts)
            pack = new PackTexture(content.Load<Texture2D>("blank"));
            pack.tresor = content.Load<Texture2D>("tresor");
            pack.unites = new List<Texture2D> { content.Load<Texture2D>("Unites/knight"), content.Load<Texture2D>("Unites/grunt"), content.Load<Texture2D>("Unites/archer"), content.Load<Texture2D>("Unites/troll"), content.Load<Texture2D>("Unites/demon"), content.Load<Texture2D>("Unites/ogre"), content.Load<Texture2D>("Unites/champion") };
            pack.sorts.Add(content.Load<Texture2D>("Spells/Explosion"));
            pack.sorts.Add(content.Load<Texture2D>("Spells/Soin"));
            pack.sorts.Add(content.Load<Texture2D>("Spells/ManaRegen"));
            pack.boutons = new List<Texture2D> { content.Load<Texture2D>("Boutons/Explosion"), content.Load<Texture2D>("Boutons/Soin"), content.Load<Texture2D>("Boutons/Invisibility"), content.Load<Texture2D>("Boutons/PotionDeVie"), content.Load<Texture2D>("Boutons/PotionMana"), content.Load<Texture2D>("Boutons/BloodLust") };
            pack.projectiles = new List<Texture2D> { content.Load<Texture2D>("Projectiles/arrow"), content.Load<Texture2D>("Projectiles/axe"), content.Load<Texture2D>("Projectiles/fireball") };
            pack.map.Add(content.Load<Texture2D>("summertiles"));

            // Chargement de la carte
            Outil.OuvrirMap("level1", ref map, pack);

            // Chargement sons
            Outil.LoadSounds(_effetsSonores, content);

            // Ajout joueurs
            map.joueurs.Add(new Joueur(new Guerrier(new Vector2(0, 9), map, pack)));
            map.unites.Add(map.joueurs[0].champion);

            // Ajout Interface
            UI Interface = new UI(map.joueurs[0], content.Load<Texture2D>("UI/barre des sorts"), content.Load<Texture2D>("Curseur"), content.Load<Texture2D>("UI/curseurRouge"), content.Load<Texture2D>("UI/GuerrierIcone"), content.Load<Texture2D>("UI/inventaire"), content.Load<Texture2D>("blank"), SceneHandler.spriteBatch, gameFont, content.Load<SpriteFont>("Polices/SpellFont"));
            map.joueurs[0].Interface = Interface;

            Wave.waveNumber = 0;
            // La vague
            PackWave packWave = new PackWave(map, pack, map.joueurs[0].champion);
            map.waves.Add(packWave.Level1Wave1());
            map.waves.Add(packWave.Level1Wave2());
            map.waves.Add(packWave.Level1Wave3());
            map.waves.Add(packWave.Level1Wave4());
            // Ajout des items
            map.items.Add(new PotionDeVie(new Vector2(22, 24), pack));
            map.items.Add(new PotionDeVie(new Vector2(23, 24), pack));

            timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            //map.items.Add(new PotionDeVie(new Vector2(40, 32), pack));
        }

        public override void Update(GameTime gameTime)
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
            foreach (Wave w in map.waves)
                w.Update(gameTime, map.joueurs[0].champion);
            // Update de la physique
            map.world.Step(1 / 60f);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CrystalGateGame.graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);
            spriteBatch.Begin(0, null, null, null, null, null, map.joueurs[0].camera.CameraMatrix);
            // DRAW MAP
            map.Draw(spriteBatch);
            // DRAW EFFETS
            foreach (Effet e in map.effets)
                e.Draw(spriteBatch);
            // DRAW ITEMS
            foreach (Item i in map.items)
                spriteBatch.Draw(pack.tresor, i.Position * map.TailleTiles, Color.White);
            // DRAW UNITES
            foreach (Unite o in map.unites)
                o.Draw(spriteBatch);
            // DRAW INTERFACE
            map.joueurs[0].Interface.Draw();
            // DRAW STRINGS
            /**/
            spriteBatch.End();
        }
        // Gestion des raccourcis des menus
        public void HandleInput()
        {
            KeyboardState keyboardState = InputState.CurrentKeyboardState;
            GamePadState gamePadState = InputState.CurrentGamePadState;

            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       InputState.GamePadWasConnected;

            if (InputState.IsPauseGame() || gamePadDisconnected)
                SceneHandler.gameState = GameState.Pause;
        }

        public void Initialize()
        {

        }
    }
}
