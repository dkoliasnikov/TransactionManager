namespace Domain.CQRS.Abstractions.Params.Abstractions;

internal interface IAddEntityParameter<EntityT> : IParameter
{
	EntityT Transaction { get; set; }
}