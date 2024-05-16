using Autofac;
using Domain;
using InMemoryStorage;

var builder = new ContainerBuilder();

builder.AddDomain();
builder.AddInMemoryStorage();

try
{
	var scoped = builder.Build().BeginLifetimeScope();

}
catch (Exception ex)
{
	Console.WriteLine(ex.ToString());
}