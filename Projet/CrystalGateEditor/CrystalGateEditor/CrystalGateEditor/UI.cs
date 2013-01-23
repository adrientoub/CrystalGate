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
    class UI
    {
        Texture2D Palette;
        SpriteFont SpriteFont;

        Vector2 PalettePosition;
        Vector2 Selection = new Vector2(0, 0);

        bool ShowCurrent = true;
        bool ShowPalette;
        bool ShowMap;
        bool ShowHelp;
        
        public Mode mode;
        public SousMode sousmode;
        public TextureStart textureStart;
        
        User user;

        string MenuString = "";
        string MapName = "";
        string longueur = "";
        string hauteur = "";
        string current = "";
        string textBase = "";

        // Sert a debug
        int thread;
        int threadActuel;

        // La Map
        Vector2[,] Map;
        int width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        int height =  System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

        public UI(User user, Texture2D Palette, SpriteFont spriteFont)
        {
            this.user = user;

            this.Palette = Palette;
            this.SpriteFont = spriteFont;

            this.PalettePosition = new Vector2(width - Palette.Width,0);
            this.sousmode = SousMode.Undone;
        }

        public void Update()
        {
            // NouvelleMap ou on Charge ?
            if (mode == Mode.LoadOrCreate)
            {
                MenuString = "    Creer/Ouvrir: \n 1 - Creer une nouvelle carte \n 2 - Charge la carte specifie";

                if (user.keyboardState.IsKeyDown(Keys.D1))
                    mode = Mode.NouvelleMap;
                if (user.keyboardState.IsKeyDown(Keys.D2))
                    mode = Mode.ChargerMap;
            }

            // Si NouvelleMap
            if (mode == Mode.NouvelleMap)
            {
                if(sousmode == SousMode.Undone)
                    sousmode = SousMode.Nom;

                    if (sousmode == SousMode.Done)
                    {
                        // Creer la carte
                        Map = new Vector2[int.Parse(longueur), int.Parse(hauteur)];
                        Initialiser(new Vector2(14, 13));
                        MenuString = "";
                        current = "";
                        mode = Mode.Draw;
                        ShowMap = true;
                    }
            }

            // Si ChargerMap
            if (mode == Mode.ChargerMap)
            {
                
            }
            // Dessiner
            if (mode == Mode.Draw)
            {
                if (user.mouse.LeftButton == ButtonState.Pressed)
                {
                    if (user.mouse.X < width - Palette.Width && ShowPalette || !ShowPalette)
                    {
                        if (user.mouse.X < Map.GetLength(0) * 32 && user.mouse.Y < Map.GetLength(1) * 32)
                        {
                            int x = user.mouse.X / 32;
                            int y = user.mouse.Y / 32;
                            Map[x, y] = Selection;
                        }
                    }
                }
                    // Controle utilisateur
                // ShowPalette
                if (user.keyboardState.IsKeyDown(Keys.P))
                {
                    ShowPalette = true;
                    if (user.mouse.LeftButton == ButtonState.Pressed && user.mouse.X >= PalettePosition.X && user.mouse.Y <= Palette.Height)
                    {
                        int x = (int)(user.mouse.X - PalettePosition.X) / 32;
                        int y = (int)(user.mouse.Y - PalettePosition.Y) / 32;
                        Selection = new Vector2(x, y);
                    }
                }
                else
                    ShowPalette = false;
                // ShowHelp
                if (user.keyboardState.IsKeyDown(Keys.OemComma))
                    ShowHelp = true;
                else
                    ShowHelp = false;
                // SaveMap
                if (user.keyboardState.IsKeyDown(Keys.LeftControl) && user.keyboardState.IsKeyDown(Keys.S) && user.oldKeyboardState.IsKeyUp(Keys.LeftControl) && user.oldKeyboardState.IsKeyUp(Keys.S))
                    SaveMap();
                // Restart
                if (user.keyboardState.IsKeyDown(Keys.R))
                {
                    mode = Mode.LoadOrCreate;
                    sousmode = SousMode.Undone;
                    MapName = "";
                    longueur = "";
                    hauteur = "";
                    textBase = "";
                }
            }

            // SousMode
            if (sousmode == SousMode.Nom)
            {
                MenuString = "Entrez le nom de la carte :";
                bool b = user.SaisirTexte(ref MapName, false);
                current = MapName;

                if (b)
                {
                    sousmode = SousMode.TailleX;
                    threadActuel = thread;
                }
            }

            if (sousmode == SousMode.TailleX)
            {
                MenuString = "Entrez la longueur de la carte :";
                bool b = user.SaisirTexte(ref longueur, true);
                current = longueur;

                if (b && threadActuel != thread)
                {
                    sousmode = SousMode.TailleY;
                    threadActuel = thread;
                }
            }

            if (sousmode == SousMode.TailleY)
            {
                MenuString = "Entrez la largeur de la carte :";
                bool b = user.SaisirTexte(ref hauteur, true);
                current = hauteur;

                if (b && threadActuel != thread)
                    sousmode = SousMode.TextureBase;
            }

            if (sousmode == SousMode.TextureBase)
            {
                MenuString = "Choisissez la sprite par defaut du sol :";
                bool b = user.SaisirTexte(ref textBase, true);
                current = textBase;
                
                if (b)
                {
                    switch (int.Parse(textBase))
                    {
                        case 1: textureStart = TextureStart.Herbe;
                            break;
                        case 2: textureStart = TextureStart.Desert;
                            break;
                        default: textureStart = TextureStart.Herbe;
                            break;
                    }
                    sousmode = SousMode.Done;
                }
            }

            if (thread - threadActuel > 100 && current == "Carte sauvegarde")
                current = "";

            thread++;
        }

        public void Initialiser(Vector2 tile)
        {
            for (int i = 0; i < Map.GetLength(0); i++)
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    int x = 0;
                    int y = 0;
                    if (textureStart == TextureStart.Herbe)
                    {
                        Random rand = new Random(i * j);
                        x = rand.Next(14, 18);
                        y = 18;
                    }

                    if (textureStart == TextureStart.Desert)
                    {
                        Random rand = new Random(i * j);
                        x = rand.Next(11, 14);
                        y = 17;
                    }

                    Map[i, j] = new Vector2(x, y);
                }

        }

        public void SaveMap()
        {
            StreamWriter stream = new StreamWriter("../../../Maps/" + MapName + ".txt");
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                for (int i = 0; i < Map.GetLength(0); i++)
                    stream.Write(Map[i,j].X + "," + Map[i,j].Y + "|");
                stream.WriteLine();
            }
            stream.Close();
            current = "Carte sauvegarde";
            thread = threadActuel;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (ShowMap)
            {
                for(int j = 0; j < Map.GetLength(1); j++)
                    for (int i = 0; i < Map.GetLength(0); i++)
                        spriteBatch.Draw(Palette, new Vector2(i * 32, j * 32), new Rectangle((int)Map[i, j].X * 32 + (int)Map[i, j].X, (int)Map[i, j].Y * 32 + (int)Map[i, j].Y, 32, 32), Color.White);
            }
            if (ShowPalette)
                spriteBatch.Draw(Palette, PalettePosition, Color.White);
            if (ShowCurrent)
                spriteBatch.DrawString(SpriteFont, current, new Vector2(width / 2, height / 1.5f), Color.White, 0, SpriteFont.MeasureString(current) / 2, 1, SpriteEffects.None, 0);
            if (ShowHelp)
            {
                string str = "    Raccourcis claviers: \n Ctrl + S - Sauvegarde la carte \n P - Affiche la palette \n R - Redemarrer l'editeur \n ? - Affiche l'aide \n";
                spriteBatch.DrawString(SpriteFont, str, new Vector2(width / 2, height / 2), Color.White, 0, SpriteFont.MeasureString(str) / 2, 1, SpriteEffects.None, 0);
            }
            spriteBatch.DrawString(SpriteFont, MenuString, new Vector2(width / 2, height / 2), Color.White, 0, SpriteFont.MeasureString(MenuString) / 2, 1, SpriteEffects.None, 0);
        }

        public enum Mode
        {
            LoadOrCreate,
            NouvelleMap,
            ChargerMap,
            Draw
        }

        public enum SousMode
        {
            Undone,
            Nom,
            TailleX,
            TailleY,
            TextureBase,
            Done
        }

        public enum TextureStart
        {
            Herbe,
            Desert
        }
    }
}
