using Domain.CQRS.Abstractions.Params.Abstractions;

namespace Domain.CQRS.Abstractions;

internal interface IUserQuery<ParameterT, ResultT> : IUserRequest
	where ParameterT : IParameter
{
	Task<ResultT?> GetAsync(ParameterT parameter);
}