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
    class PauseScene : BaseScene
    {
        private ContentManager content;

        private Rectangle mouseRec;
        private Rectangle fullscene;
        private Rectangle boutonRetour, boutonOption, boutonMenuPrincipal;

        private Text retourJeuT, optionsT, quitT;

        public override void Initialize()
        {
            
        }

        public override void LoadContent()
        {
            if (content == null)
                content = SceneHandler.content;

            retourJeuT = new Text("BackToGame");
            optionsT = new Text("OptionGame");
            quitT = new Text("QuitGame");

            UpdatePositions();
        }

        public void UpdatePositions()
        {
            fullscene = new Rectangle(0, 0, CrystalGateGame.graphics.GraphicsDevice.Viewport.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height);
            boutonRetour = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 100, boutons.Width, boutons.Height);
            boutonOption = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2, boutons.Width, boutons.Height);
            boutonMenuPrincipal = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 100, boutons.Width, boutons.Height);
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
                if (mouseRec.Intersects(boutonRetour))
                {
                    SceneHandler.gameState = GameState.Gameplay;
                    GamePlay.timer.Start();
                    CrystalGate.FondSonore.Resume();
                }
                else if (mouseRec.Intersects(boutonOption))
                {
                    SceneHandler.gameState = GameState.Setting;
                    MenuOptions.isPauseOption = true;
                }
                else if (mouseRec.Intersects(boutonMenuPrincipal))
                {
                    SceneHandler.ResetGameplay();
                    CrystalGate.FondSonore.Stop();
                    SceneHandler.gameState = GameState.MainMenu;
                    // Deconnecte du reseau
                    if (Serveur.clients.Count > 0) // Si on etait le serveur
                        Serveur.Shutdown();
                    if (Client.client != null) // Si on etait un client
                        Client.client.Close();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(blank, fullscene, new Color(0,0,0,127));

            if (mouseRec.Intersects(boutonRetour))
                spriteBatch.Draw(boutons, boutonRetour, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonRetour, Color.White);

            if (mouseRec.Intersects(boutonOption))
                spriteBatch.Draw(boutons, boutonOption, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonOption, Color.White);
            if (mouseRec.Intersects(boutonMenuPrincipal))
                spriteBatch.Draw(boutons, boutonMenuPrincipal, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonMenuPrincipal, Color.White);
            
            spriteBatch.DrawString(
                spriteFont,
                retourJeuT.get(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(retourJeuT.get()).X / 2,
                    boutonRetour.Top + 10),
                Color.White);

            spriteBatch.DrawString(
                spriteFont,
                optionsT.get(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(optionsT.get()).X / 2,
                    boutonOption.Top + 10),
                Color.White);

            spriteBatch.DrawString(
                spriteFont,
                quitT.get(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(quitT.get()).X / 2,
                    boutonMenuPrincipal.Top + 10),
                Color.White);
            
            spriteBatch.Draw(curseur, new Vector2(mouse.X, mouse.Y), Color.White);

            spriteBatch.End();
        }

    }
}
