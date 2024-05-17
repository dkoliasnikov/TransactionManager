namespace Generic.CQRS.Abstractions.Params.Abstractions;

public interface IAddEntityParameter<EntityT> : IParameter
{
	EntityT Transaction { get; set; }
}