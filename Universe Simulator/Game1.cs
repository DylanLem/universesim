using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Universe_Simulator
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static Vector2 WindowSize;

        public static bool exited = false;
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            WindowSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            ContentHandler.Font = Content.Load<SpriteFont>("Font");
            System.Diagnostics.Debug.WriteLine(ContentHandler.Font);
            ContentHandler.bgSprite = new Texture2D(GraphicsDevice, 1, 1);
            ContentHandler.bgSprite.SetData<Color>(new Color[1] { Color.SlateGray });


            ContentHandler.Init();

            base.Initialize();
            
        }

        

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            
            // TODO: use this.Content to load your game content here
        }

        
        protected override void Update(GameTime gameTime)
        {
            if (exited)
                Exit();

            ContentHandler.Update(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            // TODO: Add your drawing code here
            ContentHandler.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
