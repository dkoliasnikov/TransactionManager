namespace Domain.CQRS.Abstractions;

internal interface IExitCommand : IUserCommand
{
    void TerminateProgram();
}