using System.Web.Mvc;
using System.Collections.Generic;
using System;
using Rhino.Security.Mgmt.Dtos;
using Rhino.Security.Model;

namespace Rhino.Security.Mgmt.Controllers
{
	public class UserController : Controller
	{
		private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(Rhino.Security.Mgmt.Controllers.UserController));
		private readonly Rhino.Security.Mgmt.Data.UserRepository _repository;
		private readonly AutoMapper.IMappingEngine _mapper;
		private readonly Rhino.Security.Mgmt.Infrastructure.IValidator _validator;
		private readonly Conversation.IConversation _conversation;
		private readonly Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.User> _stringConverter;

		public UserController(Conversation.IConversation conversation, AutoMapper.IMappingEngine mapper, Rhino.Security.Mgmt.Data.UserRepository repository, Rhino.Security.Mgmt.Infrastructure.IValidator validator, Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.User> stringConverter)
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

			filterContext.Result = Json(new  
			{
				success = false,
				errors = Rhino.Security.Mgmt.Infrastructure.Mvc.ValidationHelpers.BuildErrorDictionary(ModelState),
			});
		}

		public ActionResult Save(Rhino.Security.Mgmt.Dtos.UserDto item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}

			Rhino.Security.Mgmt.Dtos.UserDto itemToReturnDto = null;
			using (_conversation.SetAsCurrent())
			{
				Rhino.Security.Model.User itemToReturn = null;
				var itemMapped = _mapper.Map<Rhino.Security.Mgmt.Dtos.UserDto, Rhino.Security.Model.User>(item);
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
				itemToReturnDto = itemToReturn != null ? _mapper.Map<Rhino.Security.Model.User, Rhino.Security.Mgmt.Dtos.UserDto>(itemToReturn) : null;
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

			using (_conversation.SetAsCurrent())
			{
				var item = _stringConverter.FromString(stringId);
				var itemDto = _mapper.Map<User, UserDto>(item);

				return Json(new
				{
					item = itemDto,
					success = ModelState.IsValid,
					errors = Rhino.Security.Mgmt.Infrastructure.Mvc.ValidationHelpers.BuildErrorDictionary(ModelState),
				});
			}
		}

		public ActionResult Delete(string[] stringIds)
		{
			if (stringIds == null)
			{
				throw new ArgumentNullException("stringIds");
			}

			using (_conversation.SetAsCurrent())
			{
				try
				{
					foreach (var s in stringIds)
					{
						var item = _stringConverter.FromString(s);
						_repository.Delete(item);
					}
					_conversation.Flush();
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
		}

		public ActionResult Search(System.Int64? id, string name, int start, int limit, string sort, string dir)
		{
			Log.DebugFormat("Search called");
			using (_conversation.SetAsCurrent())
			{
				var set = _repository.Search(id, name);
				var items = set.Skip(start).Take(limit).Sort(sort, dir == "ASC").AsEnumerable();
				var dtos = _mapper.Map<IEnumerable<Rhino.Security.Model.User>, Rhino.Security.Mgmt.Dtos.UserDto[]>(items);
				return Json(new { items = dtos, count = set.Count() });
			}
		}

		public ActionResult SearchByGroup(Rhino.Security.Mgmt.Dtos.UsersGroupReferenceDto Group, int start, int limit, string sort, string dir)
		{
			Log.DebugFormat("SearchByGroup called");
			using (_conversation.SetAsCurrent())
			{
				var GroupMapped = _mapper.Map<Rhino.Security.Mgmt.Dtos.UsersGroupReferenceDto, Rhino.Security.Model.UsersGroup>(Group);

				var set = _repository.SearchByGroup(GroupMapped);
				var items = set.Skip(start).Take(limit).Sort(sort, dir == "ASC").AsEnumerable();
				var dtos = _mapper.Map<IEnumerable<Rhino.Security.Model.User>, Rhino.Security.Mgmt.Dtos.UserDto[]>(items);
				return Json(new { items = dtos, count = set.Count() });
			}
		}

	}
}