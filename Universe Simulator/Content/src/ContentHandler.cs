using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuesday;

namespace Universe_Simulator
{
    public static  class ContentHandler
    {
        public static Texture2D bgSprite;
        public static SpriteFont Font;

        public static ConsoleDisplay Console;

        public static Universe universe;



        public static void Init()
        {
            Console = new ConsoleDisplay();
          
        }


        public static void Update(GameTime gameTime)
        {
            Console.Update(gameTime);
            if(universe != null)
            {
                universe.Update(gameTime);
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            Console.Draw(spriteBatch);

            if(universe != null)
            {
                universe.Draw(spriteBatch);
            }
        }



        public static float ParseFraction(string fraction)
        {
            string[] frac = fraction.Split('/');

            return float.Parse(frac[0])/float.Parse(frac[1]);
        }
    }
}
