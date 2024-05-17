using Domain.CQRS.Abstractions.Params.Abstractions;

namespace Domain.CQRS.Abstractions;

internal interface IUserCommand<ParameterT> : IUserRequest
	where ParameterT : IParameter
{
	Task Handle(ParameterT parameter);
}