using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace Universe_Simulator
{
    public class Universe
    {
        public List<Energy> Energies = new List<Energy>();
        public int curvature = int.MaxValue;
        public float age;
        public float acceleration = float.NaN;
        public float scaleFactor;

        public float omegaNaught = float.NaN;
        public float qNaught = float.NaN;
        public float criticalDensity = float.NaN;
        public string UltimateDestiny = null;

        public string Name = "";

        public Vector2 Position;


        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(ContentHandler.Font, Name, this.Position, Color.White);
            var stringSize = new Vector2(0, ContentHandler.Font.LineSpacing);

            Vector2 energyPosition = this.Position + new Vector2(0, stringSize.Y);
            foreach (Energy energy in Energies)
            {
                sb.DrawString(ContentHandler.Font, energy.Name + ":  d = " + MathF.Round(energy.density,2) + "  w = " + MathF.Round(energy.stateParameter,2), energyPosition, Color.White);
                energyPosition += new Vector2(0, stringSize.Y);
            }


            if (!float.IsNaN(criticalDensity))
                sb.DrawString(ContentHandler.Font, "critical density: " + criticalDensity.ToString() + "MeV/m^3", this.Position + new Vector2(Game1.WindowSize.X / 2, 0), Color.White);

            if (!float.IsNaN(omegaNaught))
                sb.DrawString(ContentHandler.Font, "density parameter: " + omegaNaught.ToString(), this.Position + new Vector2(Game1.WindowSize.X/2, stringSize.Y), Color.White);

            if (curvature != int.MaxValue)
                sb.DrawString(ContentHandler.Font, "k: " + curvature.ToString(), this.Position + new Vector2(Game1.WindowSize.X / 2, stringSize.Y*2), Color.White);

            if (!float.IsNaN(acceleration))
                sb.DrawString(ContentHandler.Font, "accel: " + acceleration.ToString(), this.Position + new Vector2(Game1.WindowSize.X / 2, stringSize.Y * 3), Color.White);

            if (!float.IsNaN(qNaught))
                sb.DrawString(ContentHandler.Font, "q_0: " + qNaught.ToString(), this.Position + new Vector2(Game1.WindowSize.X / 2, stringSize.Y*4), Color.White);

            if (UltimateDestiny != null)
                sb.DrawString(ContentHandler.Font, "Destiny: " + UltimateDestiny, this.Position + new Vector2(Game1.WindowSize.X / 2, stringSize.Y*5), Color.White);
        }


        public void CalculateOmegaNaught()
        {
            float omega = 0;

            foreach(Energy energy in Energies)
            {
                omega += energy.density;
            }

            this.omegaNaught = omega;
        }

        public void CalculateCurvature()
        {
            if (omegaNaught < 1)
                curvature = -1;
            if (omegaNaught == 1)
                curvature = 0;
            if(omegaNaught > 1)
                curvature = 1;
        }

        public void DetermineDestiny()
        {
            switch (curvature)
            {
                case 0:
                    UltimateDestiny = "Heat Death (delayed scaling)";
                    break;
                case 1:
                    UltimateDestiny = "Big Crunch";
                    break;
                case -1:
                    UltimateDestiny = "Heat Death (linear scaling)";
                    break;
            }

        }


        public void CalculateAcceleration ()
        {
            float grav_constant = 6.6743f * MathF.Pow(10,-11);
            float c = 299792000;

            float e_sum = 0;
            foreach(Energy energy in Energies)
            {
                e_sum += criticalDensity * energy.density * (1 + (3 * energy.stateParameter));
            }

            acceleration = 4 * MathF.PI * grav_constant * e_sum / (3 * c * c);

        }
    }
}
