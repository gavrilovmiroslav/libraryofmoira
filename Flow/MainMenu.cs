using LibraryOfMoira.Models;

using Microsoft.Xna.Framework;

using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOfMoira.Flow
{
    public class MainMenu : GameScreen
    {
        private new LibraryOfMoiraGame Game => (LibraryOfMoiraGame)base.Game;        

        public MainMenu(Game game) : base(game)
        {            
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
