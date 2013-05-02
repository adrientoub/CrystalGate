﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace CrystalGate.SceneEngine2
{
    class DefeatScene : BaseScene
    {
        private ContentManager content;

        private Rectangle mouseRec;
        private Rectangle fullscene;
        private Rectangle boutonQuitter;

        private Text defaiteT, quitT;

        public bool firstTime;

        public override void Initialize()
        {
            firstTime = true;
        }

        public override void LoadContent()
        {
            if (content == null)
                content = SceneHandler.content;

            defaiteT = new Text("Lose");
            quitT = new Text("QuitGame");

            fullscene = new Rectangle(0, 0, CrystalGateGame.graphics.GraphicsDevice.Viewport.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height);
            boutonQuitter = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2, boutons.Width, boutons.Height);
        }

        public override void Update(GameTime gameTime)
        {
            if (firstTime)
            {
                FondSonore.PlayDefeat();
                firstTime = false;
            }

            mouseRec = new Rectangle(mouse.X, mouse.Y, 5, 5);

            if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                if (mouseRec.Intersects(boutonQuitter))
                {
                    SceneHandler.ResetGameplay();
                    CrystalGate.FondSonore.Stop();
                    SceneHandler.gameState = GameState.MainMenu;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(blank, fullscene, new Color(0, 0, 0, 127));

            spriteBatch.DrawString(spriteFont,
                defaiteT.get(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(defaiteT.get()).X / 2,
                    boutonQuitter.Top - 90),
                Color.White);
            
            if (mouseRec.Intersects(boutonQuitter))
                spriteBatch.Draw(boutons, boutonQuitter, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonQuitter, Color.White);

            spriteBatch.DrawString(
                spriteFont,
                quitT.get(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(quitT.get()).X / 2,
                    boutonQuitter.Top + 10),
                Color.White);

            spriteBatch.Draw(curseur, new Vector2(mouse.X, mouse.Y), Color.White);

            spriteBatch.End();
        }
    }
} 