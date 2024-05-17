using Generic.CQRS.Abstractions.Params.Abstractions;

namespace Generic.CQRS.Abstractions;

public interface IUserCommand<ParameterT> : IUserRequest
	where ParameterT : IParameter
{
	Task Handle(ParameterT parameter);
}