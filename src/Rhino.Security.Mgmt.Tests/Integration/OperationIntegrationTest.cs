using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Security.Mgmt.Controllers;
using Rhino.Security.Mgmt.Dtos;
using Microsoft.Practices.ServiceLocation;
using System.Web.Script.Serialization;
using System.Web.Mvc;

namespace Rhino.Security.Mgmt.Tests.Controllers
{
	[TestFixture]
	public class OperationIntegrationTest : BaseTest
	{
		OperationController _targetController;

		[SetUp]
		protected override void SetUp()
		{
			base.SetUp();

			_targetController = ServiceLocator.Current.GetInstance<OperationController>();
		}

		[Test]
		public void Save_should_throw_with_null_input()
		{
			Assert.Throws<ArgumentNullException>(() => _targetController.Save(null));
		}

		[Test]
		public void Load_should_throw_with_null_input()
		{
			Assert.Throws<ArgumentException>(() => _targetController.Load(null));
		}

		[Test]
		public void Delete_should_throw_with_null_input()
		{
			Assert.Throws<ArgumentException>(() => _targetController.Delete(null));
		}

		[Test]
		public void Load_should_throw_with_empty_input()
		{
			Assert.Throws<ArgumentException>(() => _targetController.Load(""));
		}

		[Test]
		public void Delete_should_throw_with_empty_input()
		{
			Assert.Throws<ArgumentException>(() => _targetController.Delete(""));
		}

		[Test]
		public void Save_should_return_error_if_operation_name_is_empty()
		{
			dynamic result = _targetController.Save(new OperationDto());

			Assert.That(_targetController.ViewData.ModelState.IsValid, Is.False);
			Assert.That(result.Data.errors.Count > 0);
		}

		[Test]
		public void Save_should_return_error_if_operation_name_does_not_start_with_slash()
		{
			dynamic result = _targetController.Save(new OperationDto { Name = "test" });

			Assert.That(_targetController.ViewData.ModelState.IsValid, Is.False);
			Assert.That(result.Data.errors.Count > 0);
		}

		[Test]
		public void Save_should_throw_if_entering_same_operation_twice()
		{
			dynamic firstResult = _targetController.Save(new OperationDto { Name = "/test" });
			Assert.Throws<NHibernate.Exceptions.GenericADOException>(() => _targetController.Save(new OperationDto { Name = "/test" }));
		}

		[Test]
		public void Create_should_succeed_with_correct_input()
		{
			var testName = "/Customer";
			dynamic result = _targetController.Save(new OperationDto { Name = testName });

			Assert.That(result.Data.success, Is.True);
			Assert.That(result.Data.item.Name, Is.EqualTo(testName));
			Assert.That(result.Data.item.Id, Is.Not.EqualTo(Guid.Empty));
			Assert.That(result.Data.item.StringId, Is.Not.EqualTo(Guid.Empty));
		}

		[Test]
		public void Update_should_succeed_with_correct_input()
		{
			var originalName = "/Customer";
			var newName = "/Employee";

			dynamic newOperation = _targetController.Save(new OperationDto { Name = originalName });
			dynamic updateOperation = _targetController.Save(new OperationDto { StringId = Convert.ToString(newOperation.Data.item.Id), Id = newOperation.Data.item.Id, Name = newName });

			Assert.That(updateOperation.Data.success, Is.True);
			Assert.That(updateOperation.Data.item.Name, Is.EqualTo(newName));
			Assert.That(updateOperation.Data.item.Id, Is.EqualTo(newOperation.Data.item.Id));
			Assert.That(updateOperation.Data.item.StringId, Is.EqualTo(Convert.ToString(newOperation.Data.item.Id)));
		}

		[Test]
		public void Load_should_return_correctly()
		{
			var name = "/Customer";

			dynamic newOperation = _targetController.Save(new OperationDto { Name = name });
			dynamic readOperation = _targetController.Load(newOperation.Data.item.StringId);

			Assert.That(readOperation.Data.Name, Is.EqualTo(name));
			Assert.That(readOperation.Data.Id, Is.EqualTo(newOperation.Data.item.Id));
			Assert.That(readOperation.Data.StringId, Is.EqualTo(Convert.ToString(newOperation.Data.item.Id)));
		}

		[Test]
		public void Search_should_return_correctly()
		{
			var name = "/Customer";

			dynamic newOperation = _targetController.Save(new OperationDto { Name = name });
			dynamic searchOperation = _targetController.Search(null, "/C", null, 0, 20, null, null);

			Assert.That(searchOperation.Data.count, Is.EqualTo(1));
			Assert.That(searchOperation.Data.items.Length, Is.EqualTo(1));
			Assert.That(searchOperation.Data.items[0].Name, Is.EqualTo(name));
			Assert.That(searchOperation.Data.items[0].Id, Is.EqualTo(newOperation.Data.item.Id));
			Assert.That(searchOperation.Data.items[0].StringId, Is.EqualTo(Convert.ToString(newOperation.Data.item.Id)));
		}
	}
}
