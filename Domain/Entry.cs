using Autofac;
using Domain.Abstractions;
using Domain.Comands.Handlers;
using Domain.Enums;
using Domain.Services;

namespace Domain;

public static class Entry
{
	public static ContainerBuilder AddDomain(this ContainerBuilder builder,
		EntityAlreadyExistsBehavior alreadyExistsBehavior = EntityAlreadyExistsBehavior.PropagateException, 
		EnityNotFoundBehavior notFoundBehavior = EnityNotFoundBehavior.PropagateException)
	{
		builder.RegisterType<TransactionManager>().As<ITransactionManager>();
		builder.RegisterType<AddOrUpdateTransactionCommandHandler>().As<IAddOrUpdateTransactionCommandHandler>()
			.WithParameter("alreadyExistsBehavior", alreadyExistsBehavior)
			.WithParameter("notFoundBehavior", notFoundBehavior);

		builder.RegisterType<GetTransactionQueryHandler>().As<IGetTransactionQueryHandler>()
			.WithParameter("notFoundBehavior", notFoundBehavior);

		return builder;
	}
}
