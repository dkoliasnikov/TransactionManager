using Domain.Abstractions;

namespace Startup.Services;

internal class ConsoleOutputPrinter : IOutputPrinter
{
	public void Write(string text) => Console.Write(text);

	public void WriteLine(string line) => Console.WriteLine(line);
}