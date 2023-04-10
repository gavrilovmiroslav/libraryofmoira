using LibraryOfMoira.DataManagement;
using LibraryOfMoira.Models;

using Microsoft.Xna.Framework;

using System.Threading.Tasks;

namespace LibraryOfMoira.Flow.Gameflow
{
    public class Loading : GameflowChunk
    {
        private new LibraryOfMoiraGame Game => (LibraryOfMoiraGame)base.Game;
        private bool _allLoaded = false;

        public Loading(Game game) : base(game)
        {
            _state = State.Loading;
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
            if (_allLoaded)
            {
                Game.Flow.Trigger(Gameflow.Transition.Loaded);
            }
        }

        public override void OnEnter()
        {
            var notion = new NotionDataPlugin();

            Task.Run(async () =>
            {
                Game.Data.Abilities?.Clear();
                Game.Data.Abilities = await notion.GetAsAsync<Ability>("Abilities");
                await Task.Delay(2000);
                _allLoaded = true;
            });
        }

        public override void OnExit()
        { }
    }
}
