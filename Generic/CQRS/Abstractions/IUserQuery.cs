using Generic.CQRS.Abstractions.Params.Abstractions;

namespace Generic.CQRS.Abstractions;

public interface IUserQuery<ParameterT, ResultT> : IUserRequest
	where ParameterT : IParameter
{
	Task<ResultT?> GetAsync(ParameterT parameter);
}