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
        public static System.Diagnostics.Stopwatch timer;

        public bool isCoopPlay;
        public bool isServer;

        public override void LoadContent()
        {
            if (content == null)
                content = SceneHandler.content;

            gameFont = content.Load<SpriteFont>("Polices/gamefont");

            // Chargement des textures : Pack de texture (Contient toutes les sprites des unites et des sorts)
            PackTexture.Initialize(content);

            // Chargement sons
            PackSon.Initialize(content);
            FondSonore.Load(content);

            // Chargement de la carte
            Outil.OuvrirMap(SceneHandler.level); // On initialise la carte avec les tuiles
            if(PackMap.j == null)
                PackMap.Initialize();
            PackMap.LoadLevel(SceneHandler.level); // On injecte les donnés (unités, joueurs)
            // Ajout joueur

          
            // Bug de l'espace, si on ne reinit pas le body, il passe en statique!
            foreach (Unite u in Map.unites)
            {
                u.body = BodyFactory.CreateRectangle(Map.world, ConvertUnits.ToSimUnits(25 * u.largeurPhysique), ConvertUnits.ToSimUnits(25 * u.largeurPhysique), 100f);
                u.body.Position = ConvertUnits.ToSimUnits(u.PositionTile * Map.TailleTiles + new Vector2(16, 16));
                u.body.IsStatic = false;
            }

             /*Map.unites.Add(new Grunt(Vector2.One));
             Map.unites[Map.unites.Count - 1].isApnj = true;*/

            // Ajout Interface
            UI Interface = new UI(Map.joueurs[0], SceneHandler.spriteBatch, gameFont, content.Load<SpriteFont>("Polices/SpellFont"));
            Map.joueurs[0].Interface = Interface;

            Wave.waveNumber = 0;
            // La vague
            /*PackWave packWave = new PackWave(Map.joueurs[0].champion);
            Map.waves.Add(packWave.Level1Wave1());
            Map.waves.Add(packWave.Level1Wave2());
            Map.waves.Add(packWave.Level1Wave3());
            Map.waves.Add(packWave.Level1Wave4());*/
            // Ajout des items
            /*Map.items.Add(new PotionDeVie(null, new Vector2(22, 24)));
            Map.items.Add(new PotionDeVie(null, new Vector2(23, 24)));
            Map.items.Add(new EpeeSolari(null, Vector2.One));
            Map.items.Add(new GantsDeDevotion(null, Vector2.One));
            Map.items.Add(new BottesDacier(null, Vector2.One));
            Map.items.Add(new Epaulieres(null, Vector2.One));
            Map.items.Add(new HelmetPurple(null, Vector2.One));
            Map.items.Add(new RingLionHead(null, Vector2.One));*/

            timer = new System.Diagnostics.Stopwatch();
            timer.Start();
        }

        public override void Update(GameTime gameTime)
        {
            FondSonore.Update();

            // On update les infos de la Map
            Map.Update(gameTime);
            // On update les infos des items
            foreach (Item i in Map.items)
                i.Update(Map.unites);
            // On update les infos des unites
            foreach (Unite u in Map.unites)
                u.Update(Map.unites, Map.effets);
            // On update les infos des joueurs
            foreach (Joueur j in Map.joueurs)
                j.Update(Map.unites);
            // On update les effets sur la carte
            foreach (Effet e in Map.effets)
                e.Update();
            // On update les infos des vagues
            foreach (Wave w in Map.waves)
                w.Update(gameTime, Map.joueurs[0].champion);
            // Update de la physique
            Map.world.Step(1 / 60f);

            if (SceneEngine2.BaseScene.keyboardState.IsKeyDown(Keys.Escape) && !SceneEngine2.BaseScene.oldKeyboardState.IsKeyDown(Keys.Escape))
            {
                FondSonore.Pause();
                GamePlay.timer.Stop();
                SceneHandler.gameState = GameState.Pause;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CrystalGateGame.graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);
            spriteBatch.Begin(0, null, null, null, null, null, Map.joueurs[0].camera.CameraMatrix);
            // DRAW Map
            Map.Draw(spriteBatch);
            // DRAW EFFETS
            foreach (Effet e in Map.effets)
                e.Draw(spriteBatch);
            // DRAW ITEMS
            foreach (Item i in Map.items)
                spriteBatch.Draw(PackTexture.tresor, i.Position * Map.TailleTiles, Color.White);
            // DRAW UNITES
            foreach (Unite o in Map.unites)
                o.Draw(spriteBatch);
            // DRAW INTERFACE
            Map.joueurs[0].Interface.Draw();
            // DRAW STRINGS
            /*if(SceneHandler.joueur != null)
            spriteBatch.DrawString(gameFont, SceneHandler.joueur.champion.body.Position.ToString() + SceneHandler.joueur.champion.body.IsStatic.ToString(), Vector2.Zero, Color.White);*/
            spriteBatch.End();
        }

        public override void Initialize()
        {
            isCoopPlay = false;
            isServer = false;
        }
    }
}
