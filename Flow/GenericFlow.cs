using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using Appccelerate.StateMachine.Syntax;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOfMoira.Flow
{
    public interface IFlowChunk<StateT>
    where StateT : IComparable
    {
        StateT GetChunkState();

        void OnEnter();
        void OnExit();
    }

    public class GenericFlow<StateT, TransitionT, ChunkT>
        where StateT : IComparable
        where TransitionT : IComparable
        where ChunkT : IFlowChunk<StateT>
    {
        public StateT CurrentState;

        public PassiveStateMachine<StateT, TransitionT> FSM;
        public readonly StateMachineDefinitionBuilder<StateT, TransitionT> Builder = new();
        public readonly Dictionary<StateT, ChunkT> Chunks = new();

        public ChunkT Get(StateT state)
        {
            return Chunks[state];
        }

        public void Add(ChunkT chunk)
        {
            var state = chunk.GetChunkState();
            Chunks.Add(state, chunk);

            From(state)
                .ExecuteOnEntry(() =>
                {
                    chunk.OnEnter();
                    this.CurrentState = state;
                    this.OnTransition(state);
                })
                .ExecuteOnExit(chunk.OnExit);
        }

        public virtual void OnTransition(StateT state) { }

        public void Start(StateT state)
        {
            Builder.WithInitialState(state);
            FSM = Builder.Build().CreatePassiveStateMachine();
            FSM.Start();
        }

        protected IEntryActionSyntax<StateT, TransitionT> From(StateT state)
        {
            return Builder.In(state);
        }

        public void Trigger(TransitionT transition)
        {
            FSM.Fire(transition);
        }
    }
}
