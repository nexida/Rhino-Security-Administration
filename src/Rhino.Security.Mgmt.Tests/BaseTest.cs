using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.Facilities.FactorySupport;
using Rhino.Security.Mgmt.Infrastructure;
using Rhino.Security.Mgmt.Data;
using Castle.MicroKernel.Registration;
using Conversation;
using AutoMapper;
using Rhino.Security.Mgmt.Model;
using Rhino.Security.Mgmt.Dtos;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using Castle.MicroKernel;
using Conversation.NHibernate;
using Rhino.Security.Model;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Cfg;
using Rhino.Security.Mgmt.Controllers;
using System.Web.Mvc;
using NUnit.Framework;
using NHibernate.Cfg;
using Environment = NHibernate.Cfg.Environment;
using NHibernate.Driver;
using NHibernate.Dialect;
using NHibernate.ByteCode.Castle;
using NHibernate.Cache;
using NHibernate.Tool.hbm2ddl;

namespace Rhino.Security.Mgmt.Tests
{
	public abstract class BaseTest 
	{
		static NHibernate.Cfg.Configuration _nhConfig;

		protected virtual void SetUp()
		{
			WireThingsUp();
			var currentConversation = ServiceLocator.Current.GetInstance<IConversation>();
			using (currentConversation.SetAsCurrent())
			{
				new SchemaExport(_nhConfig).Execute(false, true, false, ServiceLocator.Current.GetInstance<ISessionFactory>().GetCurrentSession().Connection, null);
				currentConversation.Flush();
			}
		}

		[TearDown]
		protected void TearDown()
		{
			ServiceLocator.Current.GetInstance<IConversation>().Dispose();
		}

		private IWindsorContainer WireThingsUp()
		{
			IWindsorContainer ioc = new WindsorContainer();
			ioc.AddFacility<FactorySupportFacility>();

			ioc.Register(Component.For<ValidatorEngine>().UsingFactoryMethod(CreateValidatorEngine).LifeStyle.Transient);
			ioc.Register(Component.For<Rhino.Security.Mgmt.Infrastructure.IValidator>().ImplementedBy<NHibernateValidator>().LifeStyle.Transient);
			ioc.Register(Component.For<ISessionFactory>().UsingFactoryMethod(CreateSessionFactory));

			// start setup Rhino Security services
			ioc.Register(Component.For<AuthorizationRepositoryFactory>());
			ioc.Register(Component.For<PermissionsServiceFactory>());
			ioc.Register(Component.For<PermissionsBuilderServiceFactory>());
			// end setup Rhino Security services

			ioc.Register(Component.For<SecurityUsersToUsersGroupsAssociationSynchronizer>());

			ioc.Register(Component.For<IConversationFactory>().UsingFactoryMethod(CreateConversationFactory));
			ioc.Register(Component.For<IConversation>().UsingFactoryMethod(CreateConversation));

			ioc.Register(Component.For<IMappingEngine>().UsingFactoryMethod(MappingEngineBuilder.Build));

			ioc.Register(AllTypes.FromAssemblyContaining<UserStringConverter>().BasedOn(typeof(IStringConverter<>)).WithService.Base().Configure(r => r.LifeStyle.Transient));
			ioc.Register(AllTypes.FromAssemblyContaining<UserFactory>().BasedOn(typeof(IFactory<>)).WithService.Base().Configure(r => r.LifeStyle.Transient));
			ioc.Register(AllTypes.FromAssemblyContaining<UserRepository>().BasedOn<IRepository>().Configure(r => r.LifeStyle.Transient));

			ioc.Register(AllTypes.FromAssemblyContaining<UserController>().BasedOn<IController>().Configure(r => r.LifeStyle.Transient));

			ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(ioc));

			return ioc;
		}

		private static ISessionFactory CreateSessionFactory(IKernel kernel)
		{
			var validatorEngine = kernel.Resolve<ValidatorEngine>();

			Assert.NotNull(typeof(System.Data.SQLite.SQLiteConnection));

			_nhConfig = new NHibernate.Cfg.Configuration()
				.SetProperty(NHibernate.Cfg.Environment.CurrentSessionContextClass, typeof(ConversationSessionContext).AssemblyQualifiedName)
				.SetProperty(Environment.ConnectionDriver, typeof(SQLite20Driver).AssemblyQualifiedName)
				.SetProperty(Environment.Dialect, typeof(SQLiteDialect).AssemblyQualifiedName)
				.SetProperty(Environment.ConnectionString, "Data Source=:memory:")
				.SetProperty(Environment.ProxyFactoryFactoryClass, typeof(ProxyFactoryFactory).AssemblyQualifiedName)
				.SetProperty(Environment.ReleaseConnections, "on_close")
				.SetProperty(Environment.UseSecondLevelCache, "true")
				.SetProperty(Environment.UseQueryCache, "true")
				.SetProperty(Environment.CacheProvider, typeof(HashtableCacheProvider).AssemblyQualifiedName)
				.AddAssembly(typeof(User).Assembly);

			Security.Configure<User>(_nhConfig, SecurityTableStructure.Prefix);

			_nhConfig.Initialize(validatorEngine);
			return _nhConfig.BuildSessionFactory();
		}

		private static IConversationFactory CreateConversationFactory(IKernel kernel)
		{
			return new NHibernateConversationFactory(kernel.ResolveAll<ISessionFactory>());
		}

		private static IConversation CreateConversation(IKernel kernel)
		{
			return kernel.Resolve<IConversationFactory>().Open();
		}

		private static ValidatorEngine CreateValidatorEngine()
		{
			var cfg = new XmlConfiguration();
			cfg.Mappings.Add(new MappingConfiguration(typeof(User).Assembly.FullName, null));
			var ve = new ValidatorEngine();
			ve.Configure(cfg);
			return ve;
		}
	}
}
