using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Entities;
using MonoGame.Extended.BitmapFonts;

using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

using LibraryOfMoira.DataManagement;
using LibraryOfMoira.Flow;
using LibraryOfMoira.Models;
using System.Threading.Tasks;

namespace LibraryOfMoira
{
    public class LibraryOfMoiraGame : Game
    {
        private GraphicsDeviceManager _graphics;

        public FadeTransition GetFade()
        {
            return new FadeTransition(GraphicsDevice, Color.Black, 1.0f);
        }

        public readonly ScreenManager ScreenManager;

        private SpriteBatch _spriteBatch;
        private World _world;
        private BitmapFont _font;

        private readonly MainMenu _mainMenu;
        private readonly Gameplay _gameplay;

        public LibraryOfMoiraGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            ScreenManager = new ScreenManager();
            Components.Add(ScreenManager);

            _mainMenu = new MainMenu(this);
            _gameplay = new Gameplay(this);
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
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<BitmapFont>("input");
            ScreenManager.LoadScreen(_mainMenu);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _world.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _world.Draw(gameTime);
            base.Draw(gameTime);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, "Hello world", new Vector2(10, 10), Color.White);
            _spriteBatch.End();
        }
    }
}