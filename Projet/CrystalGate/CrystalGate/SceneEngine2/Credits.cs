using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CrystalGate.SceneEngine2
{
    class CreditName
    {
        public string name;
        public Text role;
        public Vector2 position;

        public CreditName(string name, Text role)
        {
            this.name = name;
            this.role = role;
            this.position = new Vector2();
        }
    }

    class Credits : BaseScene
    {
        List<CreditName> credits;
        ContentManager content;
        SpriteFont police;
        int deplacement;
        Rectangle screen;
        public GameState ecranPrecedent;
        
        public override void Initialize()
        {
            credits = new List<CreditName>();
            credits.Add(new CreditName("Adrien Toubiana", new Text("Developer")));
            credits.Add(new CreditName("Damien Pradier", new Text("Developer")));
            credits.Add(new CreditName("David Vo", new Text("Developer")));
            credits.Add(new CreditName("Sullivan Drouard", new Text("Developer")));
            
            deplacement = 0;
            screen = new Rectangle(0, 0, CrystalGateGame.graphics.GraphicsDevice.Viewport.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height);
            foreach (CreditName objet in credits)
            {
                objet.position = new Vector2(screen.Width, screen.Height);
            }
        }

        public override void LoadContent()
        {
            if (content == null)
                content = SceneHandler.content;

            police = content.Load<SpriteFont>("Polices/sceneengine2font");
        }

        public override void Update(GameTime gameTime)
        {
            deplacement++;
            for (int i = 0; i < credits.Count; i++)
            {
                credits[i].position = new Vector2((CrystalGateGame.graphics.PreferredBackBufferWidth - police.MeasureString(credits[i].role.get() + " : " + credits[i].name).X) / 2, CrystalGateGame.graphics.PreferredBackBufferHeight + i * police.MeasureString("I").Y - deplacement);
            }
            if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                SceneHandler.gameState = ecranPrecedent;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(blank, screen, Color.Black);
            for (int i = 0; i < credits.Count; i++)
            {
                spriteBatch.DrawString(police, credits[i].role.get() + " : " + credits[i].name, credits[i].position, Color.White);
            }
            spriteBatch.End();
        }

        public void Reinit()
        {
            deplacement = 0;
        }
    }
}
