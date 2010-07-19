using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Security.Mgmt.Controllers;
using Rhino.Security.Mgmt.Dtos;

namespace Rhino.Security.Mgmt.Tests.Controllers
{
	[TestFixture]
	public class OperationControllerTest
	{
		[Test]
		public void Save_should_throw_with_null_input()
		{
			var controller = new OperationController(null, null, null, null, null);
			Assert.Throws<ArgumentNullException>(() => controller.Save(null));
		}

		[Test]
		public void Save_should_return_error_if_operation_name_is_empty()
		{
			var controller = new OperationController(null, MappingEngineBuilder.Build(), null, null, null);
			controller.Save(new OperationDto());

			Assert.That(controller.ViewData.ModelState.IsValid, Is.False);
		}
	}
}
