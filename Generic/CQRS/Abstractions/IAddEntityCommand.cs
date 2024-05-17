using Generic.CQRS.Abstractions.Params.Abstractions;

namespace Generic.CQRS.Abstractions;

public interface IAddEntityCommand<EntityT, ParameterT> : IUserCommand<ParameterT>
	where ParameterT : IParameter
{
	/// <exception cref="EntityNotFoundException"></exception>
	Task AddOrUpdateAsync(EntityT transaction);
}