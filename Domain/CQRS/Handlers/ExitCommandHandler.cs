using Domain.CQRS.Abstractions;
using Domain.CQRS.Abstractions.Params.Abstractions;

namespace Domain.CQRS.Handlers;

internal class ExitCommandHandler : IExitCommand
{
	public async Task Handle(IExitAppParameter parameter)
	{
		await TerminateProgram();
	}

	public async Task TerminateProgram()
	{
		Environment.Exit(1);
	}
}