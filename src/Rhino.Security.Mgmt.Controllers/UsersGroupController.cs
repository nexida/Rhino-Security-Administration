using System.Web.Mvc;
using System.Collections.Generic;
using System;
using System.Reflection;
using Rhino.Security.Mgmt.Dtos;
using Rhino.Security.Model;

namespace Rhino.Security.Mgmt.Controllers
{
	public class UsersGroupController : Controller
	{
		private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(Rhino.Security.Mgmt.Controllers.UsersGroupController));
		private readonly Rhino.Security.Mgmt.Data.UsersGroupRepository _repository;
		private readonly AutoMapper.IMappingEngine _mapper;
		private readonly Rhino.Security.Mgmt.Infrastructure.IValidator _validator;
		private readonly Conversation.IConversation _conversation;
		private readonly Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.UsersGroup> _stringConverter;

		public UsersGroupController(Conversation.IConversation conversation, AutoMapper.IMappingEngine mapper, Rhino.Security.Mgmt.Data.UsersGroupRepository repository, Rhino.Security.Mgmt.Infrastructure.IValidator validator, Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.UsersGroup> stringConverter)
		{
			_conversation = conversation;
			_mapper = mapper;
			_repository = repository;
			_validator = validator;
			_stringConverter = stringConverter;
		}

		protected override void OnException(ExceptionContext filterContext)
		{
			var inner = filterContext.Exception.GetBaseException() as NHibernate.ObjectNotFoundException;
			if (inner == null)
			{
				return;
			}

			filterContext.ExceptionHandled = true;
			ModelState.AddModelError("operationError", Resources.StaleObjectExceptionMessage);

			Log.Warn(inner.Message);

			filterContext.Result = Json(new
			{
				success = false,
				errors = Rhino.Security.Mgmt.Infrastructure.Mvc.ValidationHelpers.BuildErrorDictionary(ModelState),
			});
		}

		public ActionResult Save(Rhino.Security.Mgmt.Dtos.UsersGroupDto item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (string.IsNullOrEmpty(item.Name))
			{
				Rhino.Security.Mgmt.Infrastructure.Mvc.ValidationHelpers.AddErrorToModelState(ModelState, "may not be null or empty", "item", "Name");
			}

			UsersGroupDto itemToReturnDto = null;
			using (_conversation.SetAsCurrent())
			{
				Rhino.Security.Model.UsersGroup itemToReturn = null;
				var itemMapped = _mapper.Map<UsersGroupDto, UsersGroup>(item);
				Rhino.Security.Mgmt.Infrastructure.Mvc.ValidationHelpers.AddErrorsToModelState(ModelState, _validator.Validate(itemMapped), "item");
				if (ModelState.IsValid)
				{
					var isNew = string.IsNullOrEmpty(item.StringId);
					if (isNew)
					{
						itemToReturn = _repository.Create(itemMapped);
					}
					if (!isNew)
					{
						itemToReturn = _repository.Update(itemMapped);
					}
					_conversation.Flush();
				}
				itemToReturnDto = itemToReturn != null ? _mapper.Map<UsersGroup, UsersGroupDto>(itemToReturn) : null;
			}

			return Json(new
			{
				item = itemToReturnDto,
				success = ModelState.IsValid,
				errors = Rhino.Security.Mgmt.Infrastructure.Mvc.ValidationHelpers.BuildErrorDictionary(ModelState),
			});
		}

		public ActionResult Load(string stringId)
		{
			if (string.IsNullOrEmpty(stringId))
			{
				throw new ArgumentException("stringId cannot be null or empty", "stringId");
			}

			UsersGroupDto itemDto = null;

			using (_conversation.SetAsCurrent())
			{
				var item = _stringConverter.FromString(stringId);
				itemDto = _mapper.Map<Rhino.Security.Model.UsersGroup, Rhino.Security.Mgmt.Dtos.UsersGroupDto>(item);
			}

			return Json(new
			{
				item = itemDto,
				success = ModelState.IsValid,
				errors = Rhino.Security.Mgmt.Infrastructure.Mvc.ValidationHelpers.BuildErrorDictionary(ModelState),
			});
		}

		public ActionResult Delete(string[] stringIds)
		{
			if (stringIds == null)
			{
				throw new ArgumentNullException("stringIds");
			}

			try
			{
				using (_conversation.SetAsCurrent())
				{
					foreach (var s in stringIds)
					{
						var item = _stringConverter.FromString(s);
						_repository.Delete(item);
					}
					_conversation.Flush();
				}
			}
			catch (InvalidOperationException ex)
			{
				ModelState.AddModelError("operationError", ex.Message);
			}

			return Json(new
			{
				success = ModelState.IsValid,
				errors = Rhino.Security.Mgmt.Infrastructure.Mvc.ValidationHelpers.BuildErrorDictionary(ModelState),
			});
		}

		public ActionResult Search(string name, int start, int limit, string sort, string dir)
		{
			Log.DebugFormat("Search called");
			using (_conversation.SetAsCurrent())
			{
				var set = _repository.Search(name);
				var items = set.Skip(start).Take(limit).Sort(sort, dir == "ASC").AsEnumerable();
				var dtos = _mapper.Map<IEnumerable<Rhino.Security.Model.UsersGroup>, Rhino.Security.Mgmt.Dtos.UsersGroupDto[]>(items);
				return Json(new { items = dtos, count = set.Count() });
			}
		}

	}
}