using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universe_Simulator
{
    public class LogString
    {
        public string Content;
        public Color stringColor;

        public LogString(string input, Color color) {
            this.Content = input;
            this.stringColor = color;
        }
    }
}
