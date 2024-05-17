namespace Generic.Exceptions;

public class EntityAlreadyExistsException : Exception
{
	public EntityAlreadyExistsException(string? message) : base(message) { }
}