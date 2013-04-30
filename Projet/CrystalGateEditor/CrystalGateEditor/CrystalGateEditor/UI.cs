using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace CrystalGateEditor
{
    public class UI
    {
        Texture2D Palette, PaletteEte, PaletteHiver, PaletteVolcanique, fondTexte;
        SpriteFont SpriteFont;

        Vector2 PalettePosition;
        Vector2 Selection = new Vector2(0, 0);

        bool ShowCurrent = false;
        bool ShowPalette;
        bool ShowMap;
        bool ShowHelp;
        bool ShowAlert;

        public TextureStart textureStart;

        public User user;

        string MenuString = "";
        public string MapName = "";
        string current = "";
        string textBase = "";

        // Sert a debug
        int thread;
        int threadActuel;

        // La Map
        public Vector2[,] Map;
        int width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        int height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

        Stack<Vector4> stack;

        public UI(User user, Texture2D PaletteEte, Texture2D PaletteHiver, Texture2D PaletteVolcanique, SpriteFont spriteFont, Texture2D fondTexte)
        {
            this.user = user;

            this.Palette = PaletteEte;
            this.PaletteEte = PaletteEte;
            this.PaletteHiver = PaletteHiver;
            this.PaletteVolcanique = PaletteVolcanique;
            this.fondTexte = fondTexte;
            this.SpriteFont = spriteFont;

            this.PalettePosition = new Vector2(width - Palette.Width, 0);
            stack = new Stack<Vector4> { };
            ShowMap = true;

            textureStart = TextureStart.Desert;
        }

        public void Update()
        {
            CameraCheck();

            if (user.mouse.LeftButton == ButtonState.Pressed)
            {
                if (user.mouse.X < width - Palette.Width && ShowPalette || !ShowPalette)
                {
                    if (user.mouse.X + user.camera.Position.X < Map.GetLength(0) * 32 && user.mouse.Y + user.camera.Position.Y < Map.GetLength(1) * 32)
                    {
                        int x = (int)(user.camera.Position.X + user.mouse.X) / 32;
                        int y = (int)(user.camera.Position.Y + user.mouse.Y) / 32;
                        if (Selection.X != Map[x, y].X || Selection.Y != Map[x, y].Y)
                        {
                            stack.Push(new Vector4(x, y, Map[x, y].X, Map[x, y].Y));
                            Map[x, y] = Selection;
                        }
                    }
                }
            }
            // Controle utilisateur
            // ShowPalette
            if (user.keyboardState.IsKeyDown(Keys.P) && user.oldKeyboardState.IsKeyUp(Keys.P))
                ShowPalette = !ShowPalette;

            // Selection tile
            if (ShowPalette && user.mouse.LeftButton == ButtonState.Pressed && user.mouse.X + user.camera.Position.X >= PalettePosition.X && user.mouse.Y <= Palette.Height)
            {
                int varx = (int)(user.mouse.X + user.camera.Position.X - PalettePosition.X);
                int vary = (int)(user.mouse.Y + user.camera.Position.Y - PalettePosition.Y);

                int x = (int)(varx - varx / 32) / 32;
                int y = (int)(vary - vary / 32) / 32;
                Selection = new Vector2(x, y);
            }
            // ShowHelp
            if (user.keyboardState.IsKeyDown(Keys.OemComma))
                ShowHelp = true;
            else
                ShowHelp = false;
            // SaveMap
            if (user.keyboardState.IsKeyDown(Keys.LeftControl) && user.keyboardState.IsKeyDown(Keys.S))
                SaveMap();
            // Control Z
            if (user.keyboardState.IsKeyDown(Keys.Z))
            {
                if (stack.Count > 0)
                {
                    Vector4 sommet = stack.Pop();
                    Map[(int)sommet.X, (int)sommet.Y] = new Vector2(sommet.Z, sommet.W);
                }
            }
            // Restart
            if (user.keyboardState.IsKeyDown(Keys.R))
            {
                SceneEngine2.SceneHandler.gameState = SceneEngine2.GameState.EditorSettings;
                MapName = "";
                stack = new Stack<Vector4>();
                ShowCurrent = false;
                Game1.scene.ReinitilizeEditor();
            }

            if (thread - threadActuel > 100 && current == "Carte sauvegarde")
            {
                current = "";
                ShowCurrent = false;
            }

            thread++;
        }

        public void Initialiser()
        {
            Random rand = new Random();
            for (int i = 0; i < Map.GetLength(0); i++)
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    int x = 0;
                    int y = 0;
                    if (textureStart == TextureStart.Herbe)
                    {
                        x = rand.Next(14, 18);
                        y = 18;
                        this.Palette = PaletteEte;
                    }
                    if (textureStart == TextureStart.Volcanique)
                    {
                        x = rand.Next(4, 11);
                        y = 18;
                        this.Palette = PaletteVolcanique;
                    }
                    if (textureStart == TextureStart.Desert)
                    {
                        x = rand.Next(11, 14);
                        y = 17;
                        this.Palette = PaletteEte;
                    }
                    if (textureStart == TextureStart.Hiver)
                    {
                        x = rand.Next(7, 13);
                        y = 18;
                        this.Palette = PaletteHiver;
                    }

                    Map[i, j] = new Vector2(x, y);
                }

        }

        public bool OuvrirMap(string MapName)
        {
            try
            {
                // Read the file and display it line by line.
                string line;
                int longueur = 0;
                int hauteur = 0;

                StreamReader file = new StreamReader(SceneEngine2.Editor.baseDirectory + "Maps/" + MapName + ".txt");
                // On regarde quel type de sol c'est grace au header

                switch (int.Parse(file.ReadLine()))
                {
                    case 1: textureStart = TextureStart.Herbe;
                        break;
                    case 2: textureStart = TextureStart.Desert;
                        break;
                    case 3: textureStart = TextureStart.Hiver;
                        break;
                    case 4: textureStart = TextureStart.Volcanique;
                        break;
                    default: textureStart = TextureStart.Herbe;
                        break;
                }

                // On établit la longueur et la hauteur
                while ((line = file.ReadLine()) != null)
                {
                    char[] splitchar = { '|' };

                    if (line != null)
                        longueur = line.Split(splitchar).Length - 1;
                    hauteur++;
                }
                // Creation de la carte
                Map = new Vector2[longueur, hauteur];
                // Reset
                file.Close();
                file = new StreamReader(SceneEngine2.Editor.baseDirectory + "Maps/" + MapName + ".txt");
                int j = 0;
                file.ReadLine(); // pour passer le header
                while ((line = file.ReadLine()) != null)
                {
                    char[] splitchar = { '|' };
                    string[] tiles = line.Split(splitchar);

                    for (int i = 0; i < longueur; i++)
                    {
                        char[] splitchar2 = { ',' };
                        int x = int.Parse((tiles[i].Split(splitchar2))[0]);
                        int y = int.Parse((tiles[i].Split(splitchar2))[1]);
                        Map[i, j] = new Vector2(x, y);
                    }
                    j++;
                }
                file.Close();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public void SaveMap()
        {
            StreamWriter stream = new StreamWriter(SceneEngine2.Editor.baseDirectory + "Maps/" + MapName + ".txt");

            switch (this.textureStart)
            {
                case TextureStart.Herbe: stream.WriteLine(1);
                    break;
                case TextureStart.Desert: stream.WriteLine(2);
                    break;
                case TextureStart.Hiver: stream.WriteLine(3);
                    break;
                case TextureStart.Volcanique: stream.WriteLine(4);
                    break;
                default: stream.WriteLine(1);
                    break;
            }

            for (int j = 0; j < Map.GetLength(1); j++)
            {
                for (int i = 0; i < Map.GetLength(0); i++)
                    stream.Write(Map[i, j].X + "," + Map[i, j].Y + "|");
                stream.WriteLine();
            }
            stream.Close();
            current = "Carte sauvegarde";
            ShowCurrent = true;
            thread = threadActuel;
        }

        public void CameraCheck()
        {
            int width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            int height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            int vitesse = 10;
            Vector2 vec = new Vector2();

            // Si on déplace la caméra hors des bords de l'écran
            if (user.mouse.X >= width - 1)
                vec.X += vitesse;
            if (user.mouse.X <= 1)
                vec.X -= vitesse;
            if (user.mouse.Y >= height - 15)
                vec.Y += vitesse;
            if (user.mouse.Y <= 1)
                vec.Y -= vitesse;

            // Si on sort de la map
            if (user.camera.Position.X >= Map.GetLength(0) * 32 - width)
                user.camera.Position = new Vector2(Map.GetLength(0) * 32 - width, user.camera.Position.Y);
            if (user.camera.Position.X <= 0)
                user.camera.Position = new Vector2(0, user.camera.Position.Y);
            if (user.camera.Position.Y <= 0)
                user.camera.Position = new Vector2(user.camera.Position.X, 0);
            if (user.camera.Position.Y >= Map.GetLength(1) * 32 - height)
                user.camera.Position = new Vector2(user.camera.Position.X, Map.GetLength(1) * 32 - height);

            //Update de la position de la caméra et de l'interface
            user.camera.Position = new Vector2(user.camera.Position.X, user.camera.Position.Y) + vec;
            PalettePosition = new Vector2(user.camera.Position.X + width - Palette.Width, user.camera.Position.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (ShowMap)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                    for (int i = 0; i < Map.GetLength(0); i++)
                        spriteBatch.Draw(Palette, new Vector2(i * 32, j * 32), new Rectangle((int)Map[i, j].X * 32 + (int)Map[i, j].X, (int)Map[i, j].Y * 32 + (int)Map[i, j].Y, 32, 32), Color.White);
            }
            if (ShowPalette)
                spriteBatch.Draw(Palette, PalettePosition, Color.White);
            if (ShowCurrent)
            {
                spriteBatch.Draw(fondTexte, new Vector2(user.camera.Position.X + width / 2 - 300, user.camera.Position.Y + height / 1.5f - 15), Color.White);
                spriteBatch.DrawString(SpriteFont, current, new Vector2(user.camera.Position.X + width / 2, user.camera.Position.Y + height / 1.5f), Color.White, 0, SpriteFont.MeasureString(current) / 2, 1, SpriteEffects.None, 0);
            }
            if (ShowHelp)
            {
                string str = "    Raccourcis claviers: \n Ctrl + S - Sauvegarde la carte \n P - Affiche la palette \n R - Redemarrer l'editeur \n ? - Affiche l'aide \n";
                spriteBatch.DrawString(SpriteFont, str, new Vector2(user.camera.Position.X + width / 2, user.camera.Position.Y + height / 2), Color.White, 0, SpriteFont.MeasureString(str) / 2, 1, SpriteEffects.None, 0);
            }
            if (ShowAlert)
            {
                string str = "Nom de carte incorrect!";
                spriteBatch.DrawString(SpriteFont, str, new Vector2(user.camera.Position.X + width / 2, user.camera.Position.Y + height / 2 + 20), Color.Red, 0, SpriteFont.MeasureString(str) / 2, 1, SpriteEffects.None, 0);
            }

            spriteBatch.DrawString(SpriteFont, MenuString, new Vector2(user.camera.Position.X + width / 2, user.camera.Position.Y + height / 2), Color.White, 0, SpriteFont.MeasureString(MenuString) / 2, 1, SpriteEffects.None, 0);
        }

        public enum TextureStart
        {
            Herbe,
            Volcanique,
            Desert,
            Hiver
        }
    }
}
