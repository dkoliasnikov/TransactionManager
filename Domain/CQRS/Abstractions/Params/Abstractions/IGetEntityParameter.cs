namespace Domain.CQRS.Abstractions.Params.Abstractions;

internal interface IGetEntityParameter<EntityT, KeyT> : IParameter
{
	KeyT Key { get; set; }
}