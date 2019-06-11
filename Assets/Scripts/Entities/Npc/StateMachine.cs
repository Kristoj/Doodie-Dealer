namespace Doodie.NPC {

    public class StateMachine<T> {

        public State<T> currentState { get; private set; }
        public T Owner;

        // Constructor for the state machine
        public StateMachine(T owner) {
            Owner = owner;
            currentState = null;
        }

        public void ChangeState(State<T> newState) {
            if (currentState != null)
                currentState.ExitState(Owner);
            currentState = newState;
            currentState.EnterState(Owner);
        }

        public void ClearState() {
            currentState = null;
        }

        public void Update() {
            if (currentState != null)
                currentState.UpdateState(Owner);
        }

    }

}