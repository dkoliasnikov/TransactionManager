using Domain.Abstractions;

namespace Startup.Services;

internal class ConsoleInputFetcher : IInputFetcher
{
	public string FetchNext() => Console.ReadLine();
}