using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LibraryOfMoira.Flow.Gameflow
{
    public class MainMenu : GameflowChunk
    {
        private new LibraryOfMoiraGame Game => (LibraryOfMoiraGame)base.Game;

        public MainMenu(Game game) : base(game)
        {
            _state = State.MainMenu;
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Game.Flow.Trigger(Gameflow.Transition.Progress);
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
