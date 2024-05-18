using Domain.Abstractions;

namespace Tests.Helpers;

internal class MockInputFetcher : IInputFetcher
{
	private IEnumerable<string> _values;
	private IEnumerator<string> _enumerator;

	public IEnumerable<string> Values {
		get
		{
			return _values;
		}
		set
		{
			_values = value;
			_enumerator = _values.GetEnumerator();
		}
	}
	
	public MockInputFetcher()
	{
	}

	public string FetchNext()
	{
		if (_enumerator.MoveNext())
			return _enumerator.Current;

		throw new Exception("No more inputs");
	}
}