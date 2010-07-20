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

namespace Rhino.Security.Mgmt.Tests
{
	class ContainerFactory
	{
		public IWindsorContainer Create()
		{
			IWindsorContainer ioc = new WindsorContainer();
			ioc.AddFacility<FactorySupportFacility>();

			ioc.Register(Component.For<ValidatorEngine>().UsingFactoryMethod(CreateValidatorEngine));
			ioc.Register(Component.For<Rhino.Security.Mgmt.Infrastructure.IValidator>().ImplementedBy<NHibernateValidator>());
			ioc.Register(Component.For<ISessionFactory>().UsingFactoryMethod(CreateSessionFactory));

			// start setup Rhino Security services
			ioc.Register(Component.For<AuthorizationRepositoryFactory>());
			ioc.Register(Component.For<PermissionsServiceFactory>());
			ioc.Register(Component.For<PermissionsBuilderServiceFactory>());
			// end setup Rhino Security services

			ioc.Register(Component.For<SecurityUsersToUsersGroupsAssociationSynchronizer>());

			ioc.Register(Component.For<IConversationFactory>().UsingFactoryMethod(CreateConversationFactory));
			ioc.Register(Component.For<IConversation>().UsingFactoryMethod(CreateConversation).LifeStyle.PerWebRequest);

			ioc.Register(Component.For<IMappingEngine>().UsingFactoryMethod(MappingEngineBuilder.Build));

			ioc.Register(AllTypes.FromAssemblyContaining<UserStringConverter>().BasedOn(typeof(IStringConverter<>)).WithService.Base());
			ioc.Register(AllTypes.FromAssemblyContaining<UserFactory>().BasedOn(typeof(IFactory<>)).WithService.Base());
			ioc.Register(AllTypes.FromAssemblyContaining<UserRepository>().BasedOn<IRepository>());

//			ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());
			ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(ioc));

			return ioc;
		}

		private static ISessionFactory CreateSessionFactory(IKernel kernel)
		{
			var validatorEngine = kernel.Resolve<ValidatorEngine>();

			var nhConfig = new NHibernate.Cfg.Configuration().Configure()
				.SetProperty(NHibernate.Cfg.Environment.CurrentSessionContextClass, typeof(ConversationSessionContext).AssemblyQualifiedName)
				.AddAssembly(typeof(User).Assembly);
			Security.Configure<User>(nhConfig, SecurityTableStructure.Prefix);

			nhConfig.Initialize(validatorEngine);
			return nhConfig.BuildSessionFactory();
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
