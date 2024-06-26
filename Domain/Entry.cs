﻿using Autofac;
using Domain.Abstractions;
using Domain.CQRS.Abstractions;
using Domain.CQRS.Handlers;
using Domain.Services;
using Generic.Enums;

namespace Domain;

public static class Entry
{
	public static ContainerBuilder AddDomain(this ContainerBuilder builder,
		EntityAlreadyExistsBehavior alreadyExistsBehavior = EntityAlreadyExistsBehavior.PropagateException, 
		EnityNotFoundBehavior notFoundBehavior = EnityNotFoundBehavior.PropagateException)
	{
		builder.RegisterType<TransactionManager>().As<ITransactionManager>();
		builder.RegisterType<AddOrUpdateTransactionCommandHandler>().As<IAddOrUpdateTransactionCommand>()
			.WithParameter("alreadyExistsBehavior", alreadyExistsBehavior)
			.WithParameter("notFoundBehavior", notFoundBehavior);

		builder.RegisterType<GetTransactionQueryHandler>().As<ITransactionQuery>()
			.WithParameter("notFoundBehavior", notFoundBehavior);

		builder.RegisterType<ExitCommandHandler>().As<IExitCommand>();

		return builder;
	}
}
