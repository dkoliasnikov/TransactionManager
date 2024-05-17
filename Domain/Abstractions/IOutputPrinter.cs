namespace Domain.Abstractions;

public interface IOutputPrinter
{
	void WriteLine(string line);
	void Write(string text);
}