namespace Generic.CQRS.Abstractions.Params.Abstractions;

public interface IGetEntityParameter<EntityT, KeyT> : IParameter
{
	KeyT Key { get; set; }
}