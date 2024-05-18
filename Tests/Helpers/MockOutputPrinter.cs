using Domain.Abstractions;

namespace Tests.Helpers;

internal class MockOutputPrinter : IOutputPrinter
{
	public List<string> Values { get; set; } = new();
	
	public MockOutputPrinter()
	{
	}
	
	public void WriteLine(string line)
	{
		Values.Add(line);
	}

	public void Write(string text)
	{
		if (Values.Any())
			Values[Values.Count - 1] = Values[Values.Count - 1] + text;
		else
			Values.Add(text);
	}
}