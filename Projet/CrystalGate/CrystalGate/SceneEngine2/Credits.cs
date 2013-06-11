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
        public Text name;
        public Vector2 position;

        public CreditName(Text name)
        {
            this.name = name;
            this.position = new Vector2();
        }
        public CreditName(string name)
        {
            this.name = new Text(name, true);
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
            credits.Add(new CreditName(new Text("Developer")));
            credits.Add(new CreditName("Adrien Toubiana"));
            credits.Add(new CreditName("Damien Pradier"));
            credits.Add(new CreditName("David Vo"));
            credits.Add(new CreditName("Sullivan Drouard"));
            credits.Add(new CreditName(""));
            credits.Add(new CreditName(new Text("Graphist")));
            credits.Add(new CreditName("Alexandre Ferlet"));
            credits.Add(new CreditName("Kévin Nguyen"));
            
            deplacement = 0;
            UpdatePositions();
            foreach (CreditName objet in credits)
            {
                objet.position = new Vector2(screen.Width, screen.Height);
            }
        }

        public void UpdatePositions()
        {
            screen = new Rectangle(0, 0, CrystalGateGame.graphics.GraphicsDevice.Viewport.Width, CrystalGateGame.graphics.GraphicsDevice.Viewport.Height);
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
            Vector2 tailleI = police.MeasureString("I");
            for (int i = 0; i < credits.Count; i++)
            {
                credits[i].position = new Vector2((screen.Width - police.MeasureString(credits[i].name.get()).X) / 2,
                    screen.Height + i * tailleI.Y - deplacement);
            }
            if (credits[credits.Count - 1].position.Y < - tailleI.Y || (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released))
            {
                SceneHandler.gameState = ecranPrecedent;
                MenuOptions.endFirstClic = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(blank, screen, Color.Black);
            for (int i = 0; i < credits.Count; i++)
            {
                spriteBatch.DrawString(police, credits[i].name.get(), credits[i].position, Color.White);
            }
            spriteBatch.End();
        }

        public void Reinit()
        {
            deplacement = 0;
        }
    }
}
