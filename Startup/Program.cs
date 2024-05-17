using Autofac;
using Domain;
using Domain.Abstractions;
using InMemoryStorage;
using Startup.Services;

var builder = new ContainerBuilder();

builder.AddDomain();
builder.AddInMemoryStorage();
builder.RegisterType<ConsoleInputFetcher>().As<IInputFetcher>();
builder.RegisterType<ConsoleOutputPrinter>().As<IOutputPrinter>();

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