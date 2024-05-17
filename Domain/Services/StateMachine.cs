namespace Domain.Services;

internal class StateMachine
{
	class Context
	{
		public State State { get; set; }
		public Context(State state)
		{
			this.State = state;
		}
		public void Request()
		{
			this.State.Handle(this);
		}
	}

	abstract class State
	{
		public abstract void Handle(Context context);
	}

	abstract class FetchCommandState : State
	{

	}

	abstract class InputCommandParameterStateState : State
	{

	}
}