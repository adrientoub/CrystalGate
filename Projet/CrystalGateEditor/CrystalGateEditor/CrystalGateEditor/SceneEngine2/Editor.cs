using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CrystalGateEditor.SceneEngine2
{
    class Editor : BaseScene
    {
        ContentManager content;

        SpriteFont sp;
        public static string baseDirectory;

        User user;
        UI ui;

        public Editor()
        {
        }

        public override void Initialize()
        {
            
        }

        public override void LoadContent()
        {
            if (content == null)
                content = SceneHandler.content;

            sp = content.Load<SpriteFont>("Police");
            user = new User();
            ui = new UI(user, content.Load<Texture2D>("Palette"), content.Load<Texture2D>("PaletteHiver"), content.Load<Texture2D>("PaletteVolcanique"), sp, content.Load<Texture2D>("writing"));
            ui.mode = UI.Mode.LoadOrCreate;
            if (Game1.isTest)
                baseDirectory = "../../../";
            else
                baseDirectory = "./";
        }

        public override void Update(GameTime gameTime)
        {
            user.Update();
            ui.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Game1.graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(0, null, null, null, null, null, user.camera.CameraMatrix);
            ui.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
