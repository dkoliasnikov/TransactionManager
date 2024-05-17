using Domain.CQRS.Abstractions.Params.Abstractions;

namespace Domain.CQRS.Abstractions;

internal interface IAddEntityCommand<EntityT, ParameterT> : IUserCommand<ParameterT>
	where ParameterT : IParameter
{
	/// <exception cref="EntityNotFoundException"></exception>
	Task AddOrUpdateAsync(EntityT transaction);
}