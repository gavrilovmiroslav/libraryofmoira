using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Screens;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOfMoira.Flow.Gameflow
{
    public class Gameplay : GameflowChunk
    {
        private new LibraryOfMoiraGame Game => (LibraryOfMoiraGame)base.Game;

        public Gameplay(Game game) : base(game)
        {
            _state = State.Gameplay;
        }

        public override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Green);
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Game.Flow.Trigger(Gameflow.Transition.Pause);                
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F10))
            {
                Game.Flow.Trigger(Gameflow.Transition.Exit);
            }
        }

        public override void OnEnter()
        {
        }

        public override void OnExit()
        {
        }
    }
}
