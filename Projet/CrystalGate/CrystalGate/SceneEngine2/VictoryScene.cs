using System;
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
    class VictoryScene : BaseScene
    {
        private ContentManager content;

        private Rectangle mouseRec;
        private Rectangle fullscene;
        private Rectangle boutonQuitter;

        private Texture2D blank;

        private Text victoireT, quitT;

        public override void Initialize()
        {

        }

        public override void LoadContent()
        {
            if (content == null)
                content = SceneHandler.content;

            victoireT = new Text("Win");
            quitT = new Text("QuitGame");
            blank = content.Load<Texture2D>("blank");

            fullscene = new Rectangle(0, 0, CrystalGateGame.graphics.GraphicsDevice.Viewport.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height);
            boutonQuitter = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2, boutons.Width, boutons.Height);
        }

        public override void Update(GameTime gameTime)
        {
            mouseRec = new Rectangle(mouse.X, mouse.Y, 5, 5);
            if (keyboardState.IsKeyDown(Keys.Escape) && !oldKeyboardState.IsKeyDown(Keys.Escape))
            {
                FondSonore.Resume();
                GamePlay.timer.Start();
                SceneHandler.gameState = GameState.Gameplay;
            }

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
                victoireT.get(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(victoireT.get()).X / 2,
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
