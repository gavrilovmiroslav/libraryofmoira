using Microsoft.Xna.Framework;

using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

namespace LibraryOfMoira.Flow.Gameflow
{
    public enum State
    {
        Loading,
        MainMenu,
        Gameplay,
        PauseMenu,
        Exit,
    }

    public enum Transition
    {
        Loaded,
        Progress,
        Pause,
        Exit,
    }

    public abstract class GameflowChunk : GameScreen, IFlowChunk<State>
    {
        protected State _state;
        public State GetChunkState() => _state;

        public GameflowChunk(Game game) : base(game)
        { }

        public virtual void OnEnter() { }
        public virtual void OnExit() { }
    }

    public class GameflowManager : GenericFlow<State, Transition, GameflowChunk>
    {
        private readonly LibraryOfMoiraGame _game;
        private readonly ScreenManager _manager;

        public GameflowManager(Game game)
        {
            _game = (LibraryOfMoiraGame)game;
            _manager = new ScreenManager();
            _game.Components.Add(_manager);

            Add(new Loading(game));
            Add(new MainMenu(game));
            Add(new Gameplay(game));
            Add(new PauseMenu(game));
            Add(new GameExit(game));

            From(State.Loading).On(Transition.Loaded).Goto(State.MainMenu);
            From(State.MainMenu).On(Transition.Progress).Goto(State.Gameplay);
            From(State.Gameplay).On(Transition.Pause).Goto(State.PauseMenu);
            From(State.Gameplay).On(Transition.Exit).Goto(State.Exit);
            From(State.PauseMenu).On(Transition.Progress).Goto(State.Gameplay);
            From(State.PauseMenu).On(Transition.Exit).Goto(State.Exit);
        }

        public override void OnTransition(State state)
        {
            _manager.LoadScreen((GameScreen)Get(state));//, new FadeTransition(_game.GetGraphicsDevice(), Color.Black, 2.0f));
        }

        public string StringifyCurrentState
        {
            get => CurrentState switch
            {
                State.Loading => "intro",
                State.MainMenu => "main menu",
                State.Gameplay => "gameplay",
                State.PauseMenu => "pause",
                _ => "exit",
            };
        }

        public void Update(GameTime gameTime)
        {
            _manager.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _manager.Draw(gameTime);
        }

        public void DebugDraw(GameTime gameTime)
        {
            _game.DrawString(StringifyCurrentState, new Vector2(10, 10), Color.White);
        }
    }
}
