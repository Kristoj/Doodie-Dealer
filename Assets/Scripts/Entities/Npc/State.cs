using UnityEngine;

namespace Doodie.NPC {

    public abstract class State<T> {
        public abstract string StateName { get; }
        public abstract void EnterState(T owner);
        public abstract void ExitState(T owner);
        public abstract void UpdateState(T owner);
    }

}