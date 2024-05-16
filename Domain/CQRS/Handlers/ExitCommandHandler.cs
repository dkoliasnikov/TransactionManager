using Domain.CQRS.Abstractions;

namespace Domain.CQRS.Handlers;

internal class ExitCommandHandler : IExitCommand
{
	public void TerminateProgram()
	{
		Environment.Exit(1);
	}
}