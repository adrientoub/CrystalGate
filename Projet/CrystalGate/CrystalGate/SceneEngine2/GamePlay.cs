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

            PackMap.LoadLevel(SceneHandler.level); // On injecte les donnés (unités, joueurs)
        
            // Bug de l'espace, si on ne reinit pas le body, il passe en statique!
            foreach (Unite u in Map.unites)
            {
                u.body = BodyFactory.CreateRectangle(Map.world, ConvertUnits.ToSimUnits(25 * u.largeurPhysique), ConvertUnits.ToSimUnits(25 * u.largeurPhysique), 100f);
                u.body.Position = ConvertUnits.ToSimUnits(u.PositionTile * Map.TailleTiles + new Vector2(16, 16));
                if(!u.isApnj)
                    u.body.IsStatic = false;
            }

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
            spriteBatch.Begin(0, null, null, null, null, null, PackMap.j.camera.CameraMatrix);
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
            /*spriteBatch.DrawString(gameFont, SceneHandler.level, Vector2.Zero, Color.White);*/
            spriteBatch.End();
        }

        public override void Initialize()
        {
            isCoopPlay = false;
            isServer = false;
        }
    }
}
