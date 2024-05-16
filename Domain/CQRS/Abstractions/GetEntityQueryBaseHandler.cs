using Domain.Abstractions;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Comands.Handlers;

abstract internal class GetEntityQueryBaseHandler<EntityT, RepositoryT> : IGetEntityQuery<EntityT>, IGetTransactionQueryHandler
		where RepositoryT : IRepository<EntityT>
{
    private readonly EnityNotFoundBehavior _notFoundBehavior = EnityNotFoundBehavior.PropagateException;

    private readonly RepositoryT _transactionRepository;

    public GetEntityQueryBaseHandler(RepositoryT transactionRepository, EnityNotFoundBehavior notFoundBehavior)
    {
        _transactionRepository = transactionRepository;
        _notFoundBehavior = notFoundBehavior;
    }

    public async Task<EntityT?> GetAsync(int id)
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
}