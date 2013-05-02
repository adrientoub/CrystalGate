using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace CrystalGate.SceneEngine2
{
    class SplashScreenScene : BaseScene
    {
        private ContentManager content;

        Video vid;
        VideoPlayer videoPlayer;
        Rectangle screen, logoPosition;

        Texture2D logo;

        bool firstTime, videoTime, pictureTime;
        
        public override void Initialize()
        {
            firstTime = true;
            pictureTime = true;
        }

        public override void LoadContent()
        {
            if (content == null)
                content = SceneHandler.content;

            blank = content.Load<Texture2D>("blank");
            vid = content.Load<Video>("video");
            videoPlayer = new VideoPlayer();
            screen = new Rectangle(0, 0, CrystalGateGame.graphics.GraphicsDevice.Viewport.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height);

            logo = content.Load<Texture2D>("logo");
            logoPosition = new Rectangle(screen.Center.X - logo.Width / 2, screen.Center.Y - logo.Height / 2, logo.Width, logo.Height);
        }

        public override void Update(GameTime gameTime)
        {
            if (pictureTime)
            {
                if ((mouse.LeftButton == ButtonState.Pressed & oldMouse.LeftButton == ButtonState.Released) || gameTime.TotalGameTime.Seconds >= 5)
                {
                    pictureTime = false;
                    videoTime = true;
                }
            }
            else if (videoTime)
            {
                if (firstTime)
                {
                    videoPlayer.Play(vid);
                    firstTime = false;
                }

                if (mouse.LeftButton == ButtonState.Pressed & oldMouse.LeftButton == ButtonState.Released)
                {
                    videoPlayer.Stop();
                    SceneHandler.gameState = GameState.MainMenu;
                }
                else if (videoPlayer.State == MediaState.Stopped)
                {
                    SceneHandler.gameState = GameState.MainMenu;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(blank, screen, Color.White);
            if (pictureTime)
            {
                spriteBatch.Draw(logo, logoPosition, Color.White);
            }
            else if (videoTime)
            {
                if (videoPlayer.State != MediaState.Stopped)
                {
                    Texture2D texture = videoPlayer.GetTexture();
                    if (texture != null)
                    {
                        spriteBatch.Draw(texture, screen, Color.White);
                    }
                }
            }
            spriteBatch.End();
        }
    }
}
