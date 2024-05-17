using Autofac;
using Domain.Abstractions;
using InMemoryStorage.Services;

namespace InMemoryStorage;

public static class Entry
{
	public static ContainerBuilder AddInMemoryStorage(this ContainerBuilder builder)
	{
		builder.RegisterInstance<ITransactionRepository>(new InMemoryTransactionRepository());
		return builder;
	}
}