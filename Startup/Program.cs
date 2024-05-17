using Autofac;
using Domain;
using Domain.Abstractions;
using InMemoryStorage;

var builder = new ContainerBuilder();

builder.AddDomain();
builder.AddInMemoryStorage();

try
{
	var scoped = builder.Build().BeginLifetimeScope();
	var manager = scoped.Resolve<ITransactionManager>();
	await manager.Run();
}
catch (Exception ex)
{
	Console.WriteLine(ex.ToString());
}