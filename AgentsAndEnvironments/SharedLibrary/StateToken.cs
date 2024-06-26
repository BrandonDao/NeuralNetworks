﻿using SharedLibrary.States;

namespace SharedLibrary
{
    public class StateToken<TState>(TState state)
        where TState : IState
    {
        public TState State { get; } = state;
    }
}