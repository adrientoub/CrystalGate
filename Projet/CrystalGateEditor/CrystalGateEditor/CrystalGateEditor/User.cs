using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace CrystalGateEditor
{
    class User
    {
        public KeyboardState keyboardState { get; set; }
        public KeyboardState oldKeyboardState { get; set; }
        public Keys[] pressedKeys { get; set; }
        public Keys[] prevPressedKeys { get; set; }

        public MouseState mouse { get; set; }

        public User()
        {
            keyboardState = Keyboard.GetState();
            pressedKeys = keyboardState.GetPressedKeys();
            prevPressedKeys = keyboardState.GetPressedKeys();
        }

        // Ecrit dans la string passée en paramètre, le bool verrouille les chiffres
        public bool SaisirTexte(ref string text, bool OnlyChiffre)
        {
            keyboardState = Keyboard.GetState();
            pressedKeys = keyboardState.GetPressedKeys();

            bool shiftPressed;

            shiftPressed = keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift);

            if (keyboardState.IsKeyDown(Keys.Space) && oldKeyboardState.IsKeyUp(Keys.Space))
                text += " ";

            foreach (Keys key in pressedKeys)
            {
                if (!prevPressedKeys.Contains(key))
                {
                    string keyString = key.ToString();

                    if (!OnlyChiffre)
                    {
                        char c = ' ';
                        if (keyString.Length == 1)
                        {
                            c = keyString[0];
                            if (!shiftPressed)
                                c += (char)('a' - 'A');
                        }
                        if (keyString.Length == 7)
                            c = keyString[6];

                        text += "" + c;
                    }
                    // SI WANNACHIFFRE
                    if (keyString.Length == 7 && OnlyChiffre)
                    {
                        char c = keyString[6];
                        //
                        // ATTENTION, PRENDRE EN COMPTE LES CHIFFRES!!!!
                        //


                        text += "" + c;
                    }
                }
            }

            if (keyboardState.IsKeyDown(Keys.Enter) && oldKeyboardState.IsKeyUp(Keys.Enter))
            {
                oldKeyboardState = keyboardState;
                prevPressedKeys = pressedKeys;
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
