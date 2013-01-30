using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace CrystalGateEditor
{
    class User
    {
        public KeyboardState keyboardState { get; set; }
        public KeyboardState oldKeyboardState { get; set; }
        public Keys[] pressedKeys { get; set; }
        public Keys[] prevPressedKeys { get; set; }
        public MouseState mouse { get; set; }
       
        public Camera2D camera;

        public User()
        {
            keyboardState = Keyboard.GetState();
            pressedKeys = keyboardState.GetPressedKeys();
            prevPressedKeys = keyboardState.GetPressedKeys();
            camera = new Camera2D(Vector2.Zero);
        }

        // Ecrit dans la string passée en paramètre, le bool verrouille les chiffres
        public bool SaisirTexte(ref string text, bool OnlyChiffre)
        {
            keyboardState = Keyboard.GetState();
            pressedKeys = keyboardState.GetPressedKeys();

            char c = ' ';
            bool shiftPressed;

            shiftPressed = keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift);

            if (keyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))
                c = ' ';

            foreach (Keys key in pressedKeys)
            {
                if (!prevPressedKeys.Contains(key))
                {
                    string keyString = key.ToString();

                    if (!OnlyChiffre)
                    {
                        if (keyString.Length == 1)
                        {
                            c = keyString[0];
                            if (!shiftPressed)
                                c += (char)('a' - 'A');
                        }
                        if (keyString.Length == 7)
                            c = keyString[6];

                        text += c;
                    }
                    // SI WANNACHIFFRE
                    if (keyString.Length == 7 && OnlyChiffre)
                    {
                        c = keyString[6];
                        text += c;
                    }
                }
            }

            // Delete
            if (keyboardState.IsKeyDown(Keys.Back) && oldKeyboardState.IsKeyUp(Keys.Back))
            {
                oldKeyboardState = keyboardState;
                prevPressedKeys = pressedKeys;
                string text2 = "";
                for (int i = 0; i < text.Length - 2; i++)
                    text2 += text[i];

                text = text2;
            }

            // Enter
            if (keyboardState.IsKeyDown(Keys.Enter) && oldKeyboardState.IsKeyUp(Keys.Enter))
            {
                oldKeyboardState = keyboardState;
                prevPressedKeys = pressedKeys;
                string text2 = "";
                for (int i = 0; i < text.Length; i++)
                {
                    if (i == 0 && text[i] == ' ' || i == text.Length - 1 && text[i] == ' ')
                    {

                    }
                    else
                        text2 += text[i];
                }
                text = text2;
                return true;
            }
            else
            {
                oldKeyboardState = keyboardState;
                prevPressedKeys = pressedKeys;
                return false;
            }
        }

        public void Update()
        {
            keyboardState = Keyboard.GetState();
            mouse = Mouse.GetState();
        }
        
    }
}
