using Domain.CQRS.Params.Abstractions;
using Generic.CQRS.Abstractions;

namespace Domain.CQRS.Abstractions;

public interface IExitCommand : IUserCommand<IExitAppParameter>
{
    Task TerminateProgram();
}