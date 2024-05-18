using Domain.CQRS.Params.Abstractions;
using Generic.CQRS.Abstractions;

namespace Domain.CQRS.Abstractions;

internal interface IExitCommand : IUserCommand<IExitAppParameter>
{
    Task TerminateProgram();
}