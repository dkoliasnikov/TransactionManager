using System.Runtime.Serialization;

namespace Domain.Exceptions;

internal class TerminatedByUserException : Exception
{
	public TerminatedByUserException()
	{
	}

	public TerminatedByUserException(string? message) : base(message)
	{
	}

	public TerminatedByUserException(string? message, Exception? innerException) : base(message, innerException)
	{
	}

	protected TerminatedByUserException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}