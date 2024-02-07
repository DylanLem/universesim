using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Universe_Simulator;

namespace Tuesday
{
    
    public class InputBox
    {
        private static List<Keys> specialKeys = new List<Keys>()
        {
            Keys.Back, Keys.Space, Keys.Enter
        };


        private ConsoleDisplay parentConsole;
        public SpriteFont font;

        private Vector2 position;
        public Vector2 size;


        public string prompt;
        private string contents;

        public bool isActive = false;
        private bool isBlinking;

        private double blinkTimer = 0;
        private double blinkRate = 1.0;
        private double deleteRate = 0.15;
        private double deleteTimer = 0;

        //maps to true if pressed false if not
        List<Keys> prevKeyStates = new List<Keys>();

        public InputBox(ConsoleDisplay console)
        {
            this.font = ContentHandler.Font;
            parentConsole = console;

            contents = "";
            prompt = ">";

        }

        public void Update(GameTime gameTime)
        {
            font = parentConsole.font;
            size = new Vector2(parentConsole.Size.X, font.LineSpacing);
            position = parentConsole.Position + new Vector2(0, parentConsole.Size.Y - size.Y);


            if (!isActive) return;

            ReadKeyboardString();

            deleteTimer += gameTime.ElapsedGameTime.TotalSeconds;

            Blink(gameTime);         
        }

        public void Draw(SpriteBatch sb)
        {
            if (isBlinking) sb.DrawString(font, contents, position, Color.White);
            else sb.DrawString(font, prompt + " ", position, Color.White);
        }

        private void Blink(GameTime gameTime)
        {
            if(contents.Length > 0)
            {
                isBlinking = true;
                return;
            }

            blinkTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if(blinkTimer >= blinkRate)
            {
                blinkTimer = 0;
                isBlinking = !isBlinking;
            }
        }

        private void PushContent()
        {
            parentConsole.ParseInput( contents);
            contents = string.Empty;
            parentConsole.inputRecieved.TrySetResult();
        }
        private void ReadKeyboardString()
        {

            KeyboardState ks = Keyboard.GetState();
            Keys[] keys = ks.GetPressedKeys();

            List<Keys> currentKeyStates = new List<Keys>();


            // the user can input capital letters by holding shift
            bool shiftKeyDown = false;
            if (ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.RightShift)) 
                shiftKeyDown = true;

            if (ks.IsKeyDown(Keys.Escape))
                contents = string.Empty;



            foreach(Keys key in keys)
            {
                System.Diagnostics.Debug.WriteLine(key);
                

                //alphabetical
                if (((int)key >= 65 && (int)key <= 90))
                {
                    currentKeyStates.Add(key);
                    if (prevKeyStates.Contains(key)) continue;
                    
                    
                        if (shiftKeyDown) contents += key.ToString();
                        else contents += key.ToString().ToLower();

                    
                }

               
                //Numeric
                if((int)key >= 48 && (int)key <= 57)
                {
                    currentKeyStates.Add(key);
                    if (prevKeyStates.Contains(key)) continue;


                   
                    contents += key.ToString().ToLower().TrimStart('d');


                }

                //specific symbols
                if (key == Keys.OemMinus)
                {
                    currentKeyStates.Add(key);
                    if (prevKeyStates.Contains(key)) continue;
                    contents += '-';
                    continue;
                }

                if(key == Keys.OemPeriod)
                {
                    currentKeyStates.Add(key);
                    if (prevKeyStates.Contains(key)) continue;
                    contents += '.';
                    continue;
                }

                if (key == Keys.OemQuestion)
                {
                    currentKeyStates.Add(key);
                    if (prevKeyStates.Contains(key)) continue;
                    contents += '/';
                    continue;
                }


                //functional keys
                if ( specialKeys.Contains(key))
                {

                    currentKeyStates.Add(key);

                    if (key != Keys.Back) deleteTimer = 0;
                    else if (deleteTimer >= deleteRate && contents.Length > 0)
                    {
                        contents = contents.Substring(0, contents.Length - 1);
                        deleteTimer = 0;
                    }

                    if (prevKeyStates.Contains(key)) continue;

                    if (key == Keys.Space) contents += " ";


                    if (key == Keys.Enter) PushContent();
                }



            }

            prevKeyStates = currentKeyStates;
            
        }
    }
}
