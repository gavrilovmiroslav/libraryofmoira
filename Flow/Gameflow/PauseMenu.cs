using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Screens;

namespace LibraryOfMoira.Flow.Gameflow
{
    public class PauseMenu : GameflowChunk
    {
        private new LibraryOfMoiraGame Game => (LibraryOfMoiraGame)base.Game;

        public PauseMenu(Game game) : base(game)
        {
            _state = State.PauseMenu;
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Red);
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Game.Flow.Trigger(Gameflow.Transition.Progress);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F10))
            {
                Game.Flow.Trigger(Gameflow.Transition.Exit);
            }
        }

        public async override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }
    }
}
