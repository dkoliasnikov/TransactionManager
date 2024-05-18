namespace Generic.Abstractions;

public interface IHaveKey<KeyT>
{
	KeyT Id { get; }
}