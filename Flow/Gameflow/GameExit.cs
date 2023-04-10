using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;

using System.Threading.Tasks;

namespace LibraryOfMoira.Flow.Gameflow
{
    public class GameExit : GameflowChunk
    {
        private new LibraryOfMoiraGame Game => (LibraryOfMoiraGame)base.Game;

        public GameExit(Game game) : base(game)
        {
            _state = State.Exit;
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override async void OnEnter()
        {
            await Task.Delay(1000);
            Game.Exit();
        }

        public override void OnExit()
        {
        }
    }
}
