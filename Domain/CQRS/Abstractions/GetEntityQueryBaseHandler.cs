using Domain.Abstractions;
using Domain.CQRS.Abstractions.Params.Abstractions;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.CQRS.Abstractions;

abstract internal class GetEntityQueryBaseHandler<EntityT, RepositoryT, ParameterT, KeyT> : IGetEntityQuery<EntityT, ParameterT, KeyT>
        where RepositoryT : IRepository<EntityT, KeyT>
		where ParameterT : IGetEntityParameter<EntityT, KeyT>
{
	private readonly EnityNotFoundBehavior _notFoundBehavior = EnityNotFoundBehavior.PropagateException;

    private readonly RepositoryT _transactionRepository;

    public GetEntityQueryBaseHandler(RepositoryT transactionRepository, EnityNotFoundBehavior notFoundBehavior)
    {
        _transactionRepository = transactionRepository;
        _notFoundBehavior = notFoundBehavior;
    }

	public async Task<EntityT?> GetEnityAsync(KeyT id)
	{
		try
		{
			return await _transactionRepository.GetAsync(id);
		}
		catch (EntityNotFoundException)
		{
			switch (_notFoundBehavior)
			{
				case EnityNotFoundBehavior.PropagateException:
					throw;
				case EnityNotFoundBehavior.Ignore:
				default:
					return default;
			}
		}
	}

	public async Task<EntityT?> GetAsync(ParameterT parameter)
	{
		var getParameter = parameter as IGetEntityParameter<EntityT, KeyT>;
		if (getParameter is null)
			throw new ArgumentException($"Parameter should by instantiated from type \"{typeof(EntityT).GetType().FullName}\". Paremeter type: \"{parameter.GetType().FullName}\"");

		return await GetEnityAsync(getParameter.Key);
	}
}