using Domain.CQRS.Abstractions.Params.Abstractions;

namespace Domain.CQRS.Abstractions;

internal interface IGetEntityQuery<EntityT, ParameterT, KeyT> : IUserQuery<ParameterT, EntityT>
	where ParameterT : IParameter
{
	/// <exception cref="EntityNotFoundException"></exception>
	Task<EntityT?> GetEnityAsync(KeyT id);
}