using Microsoft.Xna.Framework;

using MonoGame.Extended.Screens;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOfMoira.Flow
{
    public class Gameplay : GameScreen
    {
        private new LibraryOfMoiraGame Game => (LibraryOfMoiraGame)base.Game;

        public Gameplay(Game game) : base(game)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Green);
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
