using System;
using UnityEngine;

namespace BookAR.Scripts.AssetControl
{
    public interface IStatefulController<TState>
    {
        protected TState _state { get; set; }
        public TState state
        {
            get => _state;
            set
            {
                OnStateChanged(_state,value);
                _state = value;
            }
        }

        protected void OnStateChanged(TState oldState, TState newState);

    }
}