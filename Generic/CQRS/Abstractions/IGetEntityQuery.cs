﻿using Generic.CQRS.Abstractions.Params.Abstractions;

namespace Generic.CQRS.Abstractions;

public interface IGetEntityQuery<EntityT, ParameterT, KeyT> : IUserQuery<ParameterT, EntityT>
	where ParameterT : IParameter
{
	/// <exception cref="EntityNotFoundException"></exception>
	Task<EntityT?> GetEnityAsync(KeyT id);
}