using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Universe_Simulator;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Tuesday
{
    public class ConsoleDisplay { 

        
       

        public InputBox input;

        private List<LogString> log;
        public SpriteFont font;

        private Texture2D bgSprite;
        public Color bgColour;
        public Color fontColour;

        private Vector2 stringSize;


        public Vector2 Position;
        public Vector2 Size;

        public TaskCompletionSource inputRecieved;



        public ConsoleDisplay()
        {
            log = new List<LogString>();
            bgColour = Color.SlateGray;
            fontColour = Color.White;
            Size = new Vector2(Game1.WindowSize.X, Game1.WindowSize.Y/2);
            Position = Vector2.Zero;
            this.font = ContentHandler.Font;

            input = new InputBox(this);
            

            bgSprite = ContentHandler.bgSprite;
            System.Diagnostics.Debug.WriteLine(bgSprite);


            ShowStartup();
            
        }

        public void Draw(SpriteBatch sb)
        {
            stringSize = new Vector2(0, font.LineSpacing);


            sb.Draw(bgSprite, new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)this.Size.X, (int)this.Size.Y), bgColour);
            
            //prints the log contents from newest to oldest
            for (int i = log.Count - 1; i>=0; i--)
            {
                
                sb.DrawString(font, log[i].Content, new Vector2(Position.X, Position.Y + stringSize.Y * i), log[i].stringColor);
            }


            input.Draw(sb);
           

        }

        public void AddString(string input, Color color)
        {
            log.Add(new LogString(input,color));
        }

        public void ParseInput(string s)
        {
            this.AddString(input.prompt + s, fontColour);
            

            string[] arguments = s.Split();

            for (int i = 0; i < arguments.Length; i++)
            {
                arguments[i] = arguments[i].ToLower();
            }
        }


        public void Update(GameTime gameTime)
        {
            if (stringSize.Y * log.Count > this.Size.Y)
                this.log.Remove(this.log[0]);

            var rand = new Random();
            fontColour = new Color(fontColour.R + rand.Next(-1,2) % 255, fontColour.G + rand.Next(-1, 2) % 255, fontColour.B + rand.Next(-1, 2) % 255);

            input.Update(gameTime);
        }


        public string GetLastInput()
        {
            return log[log.Count - 1].Content.TrimStart('>').ToLower();
        }


        public async void ShowStartup()
        {
            this.AddString("HELLO - Welcome to Universe Simulator v8.81 (released 1976).", Color.CornflowerBlue);
            await AwaitInput();

            await Task.Delay(1000);
            this.AddString("Please wait for the simulation to initialize.", Color.Red);
            await Task.Delay(800);
            this.AddString("Allocating memory for:", Color.DarkRed);
            await Task.Delay(100);
            
            this.AddString("mathematical principles", Color.Gray);
            this.AddString("spacetime", Color.Gray);
            this.AddString("fields", Color.Gray);
            this.AddString("higher dimensionality", Color.Gray);
            this.AddString("radiation", Color.Gray);
            this.AddString("matter", Color.Gray);
            this.AddString("Science", Color.Gray);
            this.AddString("Magic", Color.Pink);
            await Task.Delay(100);
            this.AddString("Wii Sports: Resort", Color.Gray);
            this.AddString("Electromagnetic Forces", Color.Gray);
            this.AddString("Nuclear cohesion", Color.Gray);
            this.AddString("Non-deterministic events", Color.Gray);
            this.AddString("Broken symmetries", Color.Gray);

            await Task.Delay(1000);
            this.AddString("Task Complete.", Color.White);
            await Task.Delay(200);
            this.AddString("Initializing universal calibrator...", Color.DarkRed);
            await Task.Delay(500);
            this.AddString("Finished.", Color.White);
            await Task.Delay(500);
            


            AwaitUniverse();
        }

        public async void AwaitUniverse()
        {
            this.AddString("Simulator ready! Would you like to create a new universe? y/n", Color.Coral);
            await AwaitInput();
            string input = GetLastInput();

   

            if (input == "y")
            {
                this.AddString("yay! let's start.", Color.Wheat);
                CreateUniverse();
            }

            else if(input == "n")
            {
                this.AddString("Are you sure? type q to quit.", Color.DarkOrchid);
                await AwaitInput();
                input = GetLastInput();

                if (input == "q")
                {
                    Game1.exited = true;
                    return;
                }

                AwaitUniverse();     

            }

            else
            {
                this.AddString("hey, I don't know what that means. Try again.", Color.Red);
                AwaitUniverse();
            }
        }


        public async void CreateUniverse()
        {
            
            this.AddString("Allocating asymptotic amounts of virtual energy...", Color.DarkRed);
            await Task.Delay(500);
            Universe uni = new Universe();
            

            uni.Position = this.Position + new Vector2(0, this.Size.Y);

            ContentHandler.universe = uni;

            this.AddString("Please provide a name for this universe: ", Color.Coral);
            await AwaitInput();
            uni.Name = GetLastInput();


            this.AddString("Initializing " + uni.Name + ".", Color.DarkRed);

            await Task.Delay(150);

            bool AddingEnergies = true;

            this.AddString("Your universe needs energy to prosper. Beginning energy distribution.", Color.DarkOrchid);

            await Task.Delay(350);

            AddString("What is the critical energy density of your universe? (leave blank for default 4870 MeV/m^3)", Color.Coral);
            await AwaitInput();
            if(GetLastInput() == "")
            {
                uni.criticalDensity = 4870;
            }
            else
            {
                uni.criticalDensity = float.Parse(GetLastInput());
            }


            while (AddingEnergies)
            {
                Energy e = new Energy();

                AddString("Provide a name for your energy type (leave blank for numeric designation):", Color.Coral);
                await AwaitInput();
                e.Name = GetLastInput();


                
                AddString("What is the density parameter? (0.0-1.0)", Color.Coral);
                await AwaitInput();
                float d;
                if (GetLastInput().Contains('/'))
                    e.density = ContentHandler.ParseFraction(GetLastInput());
                else
                    e.density = float.Parse(GetLastInput());

                

                AddString("Provide an equation of state parameter, w: ", Color.Coral);
                await AwaitInput();
                if (GetLastInput().Contains('/'))
                    e.stateParameter = ContentHandler.ParseFraction(GetLastInput());
                else
                    e.stateParameter = float.Parse(GetLastInput());





                AddString("Energy creation complete.", Color.CornflowerBlue);
                await Task.Delay(200);
                AddString("----", Color.Red);
                AddString("Name: " + e.Name, Color.LightGray);
                AddString("Density: " + e.density, Color.LightGray);
                AddString("Equation of state parameter: " + e.stateParameter, Color.LightGray);

                AddString("----", Color.Red);

                AddString("Confirm properties. y/n", Color.Coral);
                await AwaitInput();
                string input = GetLastInput();

                if (input == "y")
                    uni.Energies.Add(e);


                else if (input == "n")
                    continue;

                AddEnergies:
                AddString("Add more energies? y/n", Color.Coral);
                await AwaitInput();
                input = GetLastInput();
                if (input == "y")
                    continue;


                else if (input == "n")
                    AddingEnergies = false;

                else
                {
                    AddString("I dont understand.", Color.Red);
                    goto AddEnergies;
                }

            }

            AddString("Compiling universal parameters... please wait...", Color.Gray);
            uni.CalculateAcceleration();
            uni.CalculateOmegaNaught();
            uni.CalculateCurvature();
            uni.DetermineDestiny();

        }



        private async Task AwaitInput()
        {
            input.isActive = true;
            inputRecieved = new TaskCompletionSource();
            await inputRecieved.Task;
            input.isActive = false;
        }

    }
}
