using Domain.CQRS.Abstractions;
using Domain.CQRS.Params.Abstractions;
using Domain.Exceptions;

namespace Domain.CQRS.Handlers;

internal class ExitCommandHandler : IExitCommand
{
	public  Task Handle(IExitAppParameter parameter) => TerminateProgram();

	public async Task TerminateProgram() => throw new TerminatedByUserException();
}