using Domain.Abstractions;
using Domain.CQRS.Abstractions;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Comands.Handlers;

abstract internal class AddOrUpdateEntityCommandBaseHandler<EntityT, RepositoryT> : IAddEntityCommand<EntityT>
        where RepositoryT : IRepository<EntityT>
{
    private readonly EntityAlreadyExistsBehavior _alreadyExistsBehavior = EntityAlreadyExistsBehavior.PropagateException;

    private readonly EnityNotFoundBehavior _notFoundBehavior = EnityNotFoundBehavior.PropagateException;
	
    private readonly RepositoryT _repository;

	public AddOrUpdateEntityCommandBaseHandler(RepositoryT repository, EntityAlreadyExistsBehavior alreadyExistsBehavior, EnityNotFoundBehavior notFoundBehavior)
    {
        _repository = repository;
        _alreadyExistsBehavior = alreadyExistsBehavior;
        _notFoundBehavior = notFoundBehavior;
    }

    public async Task AddOrUpdateAsync(EntityT entity)
    {
        var haveToUpdate = await Add(entity);

        if (haveToUpdate)
            await UpdateTransaction(entity);
    }

    private async Task<bool> Add(EntityT transaction)
    {
        var haveToUpdate = false;

        try
        {
            await _repository.AddAsync(transaction);
        }
        catch (EntityAlreadyExistsException)
        {
            switch (_alreadyExistsBehavior)
            {
                case EntityAlreadyExistsBehavior.Update:
                    haveToUpdate = true;
                    break;
                case EntityAlreadyExistsBehavior.PropagateException:
                    throw;
                case EntityAlreadyExistsBehavior.Ignore:
                default:
                    haveToUpdate = false;
                    break;
            }
        }

        return haveToUpdate;
    }

    private async Task UpdateTransaction(EntityT entity)
    {
        try
        {
            await _repository.UpdateAsync(entity);
        }
        catch (EntityNotFoundException)
        {
            switch (_notFoundBehavior)
            {
                case EnityNotFoundBehavior.PropagateException:
                    throw;
                case EnityNotFoundBehavior.Ignore:
                default:
                    break;
            }
        }
    }
}