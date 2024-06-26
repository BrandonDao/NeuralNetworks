﻿using SharedLibrary.States;

namespace SharedLibrary.Agents
{

    public class AgentData<TState>(TState state, StateToken<IState> stateToken, AgentData<TState>? founder, float priority, float cumulativeCost)
        where TState : IState
    {
        public TState State { get; } = state;
        public StateToken<IState> StateToken { get; } = stateToken;
        public AgentData<TState>? Predecessor { get; } = founder;
        public float Priority { get; } = priority;
        public float CumulativeCost { get; } = cumulativeCost;
    }
}