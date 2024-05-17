using Generic.Abstractions;
using Generic.CQRS.Abstractions.Params.Abstractions;
using Generic.Enums;
using Generic.Exceptions;

namespace Generic.CQRS.Abstractions;

abstract public class AddOrUpdateEntityCommandBaseHandler<EntityT, RepositoryT, ParameterT, KeyT>  :
	IAddEntityCommand<EntityT, ParameterT>
        where RepositoryT : IRepository<EntityT, KeyT>
		where ParameterT : IAddEntityParameter<EntityT>
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

	public async Task Handle(ParameterT parameter)
	{
        var addOrUpdateParameter = parameter as IAddEntityParameter<EntityT>;
        if (addOrUpdateParameter is null)
            throw new ArgumentException($"Parameter should by instantiated from type \"{typeof(EntityT).GetType().FullName}\". Paremeter type: \"{parameter.GetType().FullName}\"");

        await AddOrUpdateAsync(addOrUpdateParameter.Transaction);
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