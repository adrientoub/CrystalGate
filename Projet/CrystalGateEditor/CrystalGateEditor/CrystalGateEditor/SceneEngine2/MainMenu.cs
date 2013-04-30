using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace CrystalGateEditor.SceneEngine2
{
    public class MainMenu : BaseScene
    {
        private ContentManager content;

        private Rectangle mouseRec;

        private Rectangle boutonPlay, boutonOptions, boutonQuitter;

        private Text lancerJeu, optionsJeu, quitterJeu;

        public override void Initialize()
        {

        }

        public override void LoadContent()
        {
            if (content == null)
                content = SceneHandler.content;

            background = content.Load<Texture2D>("background");
            boutons = content.Load<Texture2D>("Boutons");

            lancerJeu = new Text("LaunchGame");
            optionsJeu = new Text("OptionGame");
            quitterJeu = new Text("Quit");

            spriteFont = content.Load<SpriteFont>("sceneengine2font");

            boutonPlay = new Rectangle((Game1.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 100, boutons.Width, boutons.Height);
            boutonOptions = new Rectangle((Game1.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, Game1.graphics.GraphicsDevice.Viewport.Height / 2, boutons.Width, boutons.Height);
            boutonQuitter = new Rectangle((Game1.graphics.GraphicsDevice.Viewport.Width - boutons.Width) / 2, Game1.graphics.GraphicsDevice.Viewport.Height / 2 + 100, boutons.Width, boutons.Height);
        }

        public override void Update(GameTime gameTime)
        {
            // Handle mouse and keyboard to use the menu.
            mouseRec = new Rectangle(mouse.X, mouse.Y, 5, 5);
            if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                if (mouseRec.Intersects(boutonPlay))
                {
                    SceneHandler.gameState = GameState.EditorSettings;
                }
                else if (mouseRec.Intersects(boutonOptions))
                {
                    SceneHandler.gameState = GameState.Setting;
                    MenuOptions.isPauseOption = false;
                }
                else if (mouseRec.Intersects(boutonQuitter))
                {
                    Game1.exit = true;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var fullscene = new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height);
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

            if (mouseRec.Intersects(boutonQuitter))
                spriteBatch.Draw(boutons, boutonQuitter, Color.Gray);
            else
                spriteBatch.Draw(boutons, boutonQuitter, Color.White);

            spriteBatch.DrawString(
                spriteFont,
                lancerJeu.get(),
                new Vector2((Game1.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(lancerJeu.get()).X / 2,
                    Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 90),
                Color.White);
            spriteBatch.DrawString(
                spriteFont,
                optionsJeu.get(),
                new Vector2((Game1.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(optionsJeu.get()).X / 2,
                    Game1.graphics.GraphicsDevice.Viewport.Height / 2 + 10),
                Color.White);
            spriteBatch.DrawString(
                spriteFont,
                quitterJeu.get(),
                new Vector2((Game1.graphics.GraphicsDevice.Viewport.Width) / 2 - spriteFont.MeasureString(quitterJeu.get()).X / 2,
                    Game1.graphics.GraphicsDevice.Viewport.Height / 2 + 110),
                Color.White);

            spriteBatch.End();
        }
    }
}
