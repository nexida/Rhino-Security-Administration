using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Security.Mgmt.Controllers;
using Microsoft.Practices.ServiceLocation;
using Rhino.Security.Mgmt.Infrastructure;
using Rhino.Security.Mgmt.Data;
using Rhino.Security.Model;
using Conversation;
using Rhino.Security.Interfaces;
using Rhino.Security.Services;

namespace Rhino.Security.Mgmt.Tests.Controllers
{
	[TestFixture]
	public class PermissionIntegrationTest : BaseTest
	{
		PermissionController _targetController;
		IAuthorizationRepository _authZRepo;
		UserRepository _userRepository;
		IPermissionsBuilderService _permissionBuilderService;
		IUser _seniorUser, _juniorUser;
		UsersGroup _seniorMgrs, _juniorMgrs;
		Operation _op4Senior, _op4Junior;

		[SetUp]
		protected override void SetUp()
		{
			base.SetUp();

			var currentConversation = ServiceLocator.Current.GetInstance<IConversation>();
			using (currentConversation.SetAsCurrent())
			{
				_authZRepo = ServiceLocator.Current.GetInstance<AuthorizationRepositoryFactory>().Create();
				_userRepository = ServiceLocator.Current.GetInstance<UserRepository>();
				_permissionBuilderService = ServiceLocator.Current.GetInstance<PermissionsBuilderServiceFactory>().Create();

				_seniorUser = _userRepository.Create(new User { Name = "Senior" });
				_juniorUser = _userRepository.Create(new User { Name = "Junior" });
				_seniorMgrs = _authZRepo.CreateUsersGroup("Senior Managers");
				_juniorMgrs = _authZRepo.CreateUsersGroup("Junior Managers");
				_op4Senior = _authZRepo.CreateOperation("/Operation/ForSenior");
				currentConversation.Flush();
				_op4Junior = _authZRepo.CreateOperation("/Operation/ForJunior");

				_authZRepo.AssociateUserWith(_seniorUser, _seniorMgrs);
				_authZRepo.AssociateUserWith(_juniorUser, _juniorMgrs);

				_permissionBuilderService
					.Allow(_op4Junior)
					.For(_juniorMgrs)
					.OnEverything()
					.DefaultLevel()
					.Save();

				_permissionBuilderService
					.Deny(_op4Senior)
					.For(_juniorMgrs)
					.OnEverything()
					.DefaultLevel()
					.Save();

				_permissionBuilderService
					.Allow(_op4Senior)
					.For(_seniorMgrs)
					.OnEverything()
					.DefaultLevel()
					.Save();

				currentConversation.Flush();
			}

			_targetController = ServiceLocator.Current.GetInstance<PermissionController>();
		}

		[Test]
		public void Create_should_throw_with_null_input()
		{
			Assert.Throws<ArgumentNullException>(() => _targetController.Create(null));
		}

		[Test]
		public void Delete_should_throw_with_null_input()
		{
			Assert.Throws<ArgumentNullException>(() => _targetController.Delete(null));
		}

		[Test]
		public void Juniors_should_be_denied_on_senior_operations()
		{
			dynamic result = _targetController.LoadByOperationName(_op4Senior.Name);

			Assert.That(result.Data.allowed, Is.Not.Null);
			Assert.That(result.Data.forbidden, Is.Not.Null);

			//TODO
		}
	}
}
