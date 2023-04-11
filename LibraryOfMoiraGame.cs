using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Entities;
using MonoGame.Extended.BitmapFonts;

using LibraryOfMoira.DataManagement;
using LibraryOfMoira.Models;
using LibraryOfMoira.Flow.Gameflow;

namespace LibraryOfMoira
{
    public class LibraryOfMoiraGame : Game
    {
        public SpriteBatch SpriteBatch;
        public GameData Data;
        public GameflowManager Flow;

        private GraphicsDeviceManager _graphics;                
        private World _world;
        private BitmapFont _font;
        
        public GraphicsDevice GetGraphicsDevice()
        {
            return _graphics.GraphicsDevice;
        }

        public void DrawString(string text, Vector2 pos, Color color)
        {
            SpriteBatch.Begin();
            SpriteBatch.DrawString(_font, text, pos, color);
            SpriteBatch.End();
        }

        public LibraryOfMoiraGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Data = new GameData();
            Flow = new GameflowManager(this);
        }

        protected override void Initialize()
        {
            base.Initialize();

            var notion = new NotionDataPlugin();
            _world = new WorldBuilder()
//                .AddSystem(new PlayerSystem())
//                .AddSystem(new RenderSystem(GraphicsDevice))
                .Build();
        }


        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<BitmapFont>("input");

            Flow.Start(State.Loading);
        }

        protected override void Update(GameTime gameTime)
        {
            _world.Update(gameTime);
            Flow.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _world.Draw(gameTime);
            Flow.Draw(gameTime);
            base.Draw(gameTime);

            // Debug
            Flow.DebugDraw(gameTime);
        }
    }
}