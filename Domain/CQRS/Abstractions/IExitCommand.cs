using Domain.CQRS.Abstractions.Params.Abstractions;

namespace Domain.CQRS.Abstractions;

internal interface IExitCommand : IUserCommand<IExitAppParameter>
{
    Task TerminateProgram();
}