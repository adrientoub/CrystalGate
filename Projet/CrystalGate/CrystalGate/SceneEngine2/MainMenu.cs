using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrystalGate.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace CrystalGate.SceneEngine2
{
    class MainMenu : BaseScene
    {
        private ContentManager content;

        private Rectangle mouseRec;

        private Rectangle boutonPlay, boutonOptions, boutonCoop, boutonQuitter;

        private Text lancerJeu, optionsJeu, coopT,quitterJeu;

        public override void Initialize()
        {

        }

        public override void LoadContent()
        {
            if (content == null)
                content = SceneHandler.content;

            background = content.Load<Texture2D>("background");
            boutons = content.Load<Texture2D>("Menu/Boutons");
            curseur = content.Load<Texture2D>("Curseur");

            lancerJeu = new Text("LaunchGame");
            optionsJeu = new Text("OptionGame");
            quitterJeu = new Text("Quit");
            coopT = new Text("Coop");

            spriteFont = content.Load<SpriteFont>("Polices/sceneengine2font");

            boutonPlay = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 150, boutons.Width, boutons.Height);
            boutonCoop = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 50, boutons.Width, boutons.Height);
            boutonOptions = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 50, boutons.Width, boutons.Height);
            boutonQuitter = new Rectangle((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 150, boutons.Width, boutons.Height);
        }

        public override void Update(GameTime gameTime)
        {
            // Handle mouse and keyboard to use the menu.
            mouseRec = new Rectangle(mouse.X, mouse.Y, 5, 5);
            if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                if (mouseRec.Intersects(boutonPlay))
                {
                    SceneHandler.gameState = GameState.Gameplay;
                    FondSonore.Play();
                    SceneHandler.gameplayScene.isCoopPlay = false;
                    GamePlay.timer.Restart();
                }
                else if (mouseRec.Intersects(boutonOptions))
                {
                    SceneHandler.gameState = GameState.Setting;
                    MenuOptions.isPauseOption = false;
                }
                else if (mouseRec.Intersects(boutonCoop))
                {
                    SceneHandler.gameState = GameState.CoopSettings;
                }
                else if (mouseRec.Intersects(boutonQuitter))
                {
                    CrystalGateGame.exit = true;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var fullscene = new Rectangle(0, 0, CrystalGateGame.graphics.GraphicsDevice.Viewport.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height);
            spriteBatch.Begin();
            spriteBatch.Draw(background, fullscene, Color.White);

            if (mouseRec.Intersects(boutonPlay))
                spriteBatch.Draw(boutons, boutonPlay, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonPlay, Color.White);
            
            if (mouseRec.Intersects(boutonOptions))
                spriteBatch.Draw(boutons, boutonOptions, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonOptions, Color.White);

            if (mouseRec.Intersects(boutonCoop))
                spriteBatch.Draw(boutons, boutonCoop, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonCoop, Color.White);

            if (mouseRec.Intersects(boutonQuitter))
                spriteBatch.Draw(boutons, boutonQuitter, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonQuitter, Color.White);

            spriteBatch.DrawString(
                spriteFont,
                lancerJeu.get(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(lancerJeu.get()).X / 2,
                    CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 140),
                Color.White);
            spriteBatch.DrawString(
                spriteFont,
                coopT.get(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(coopT.get()).X / 2,
                    CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 - 40),
                Color.White);
            spriteBatch.DrawString(
                spriteFont,
                optionsJeu.get(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(optionsJeu.get()).X / 2,
                    CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 60),
                Color.White);
            spriteBatch.DrawString(
                spriteFont,
                quitterJeu.get(),
                new Vector2((CrystalGateGame.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(quitterJeu.get()).X / 2,
                    CrystalGateGame.graphics.GraphicsDevice.Viewport.Height / 2 + 160),
                Color.White);

            spriteBatch.Draw(curseur, new Vector2(mouse.X, mouse.Y), Color.White);

            spriteBatch.End();
        }
    }
}
