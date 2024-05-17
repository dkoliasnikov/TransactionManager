using Autofac;
using Domain.Abstractions;
using Generic.CQRS.Abstractions.Params.Abstractions;

namespace Domain.Services;

internal class StateMachine
{
	public class Context
	{
		private State _state { get; set; }

		public readonly ILifetimeScope Scope;
		public readonly IInputFetcher InputFetcher;
		public readonly IOutputPrinter OutputPrinter;

        public (Type Handler, IParameter Parameter)? Result { get; set; }

        public Context(State state)
		{
			_state = state;
		}

		public Context(State state, ILifetimeScope scope, IInputFetcher inputFetcher, IOutputPrinter outputPrinter) : this(state)
		{
			Scope = scope;
			InputFetcher = inputFetcher;
			OutputPrinter = outputPrinter;
		}

		public void Request()
		{
			_state.Handle(this);
		}

		public void TransitionTo(State state)
		{
			_state = state;
			_state.SetContext(this);
		}
	}

	public class ExitCommandContext : Context
	{
		public ExitCommandContext(State state) : base(state)
		{
		}

		public ExitCommandContext(State state, ILifetimeScope scope, IInputFetcher inputFetcher, IOutputPrinter outputPrinter) : base(state, scope, inputFetcher, outputPrinter)
		{
		}
	}

	public abstract class State
	{
		protected Context _context;

		public void SetContext(Context context)
		{
			_context = context;
		}

		public abstract void Handle(Context context);
	}

	public class FetchCommandState : State
	{
		public override void Handle(Context context)
		{
			context.InputFetcher.FetchNext();
		}
	}

	public class FetchExitCommandState : State
	{
		public override void Handle(Context context)
		{
			context.InputFetcher.FetchNext();
		}
	}

	public class InputCommandParameterStateState : State
	{
		public override void Handle(Context context)
		{
			throw new NotImplementedException();
		}
	}
}