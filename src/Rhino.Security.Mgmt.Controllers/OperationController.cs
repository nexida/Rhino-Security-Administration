using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Rhino.Security.Mgmt.Controllers
{
	public class OperationController : Controller
	{
		private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(Rhino.Security.Mgmt.Controllers.OperationController));
		private readonly Rhino.Security.Mgmt.Data.OperationRepository _repository;
		private readonly AutoMapper.IMappingEngine _mapper;
		private readonly Rhino.Security.Mgmt.Infrastructure.IValidator _validator;
		private readonly Conversation.IConversation _conversation;
		private readonly Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.Operation> _stringConverter;

		public OperationController(Conversation.IConversation conversation, AutoMapper.IMappingEngine mapper, Rhino.Security.Mgmt.Data.OperationRepository repository, Rhino.Security.Mgmt.Infrastructure.IValidator validator, Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.Operation> stringConverter)
		{
			_conversation = conversation;
			_mapper = mapper;
			_repository = repository;
			_validator = validator;
			_stringConverter = stringConverter;
		}

		public ActionResult Save(Rhino.Security.Mgmt.Dtos.OperationDto item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}

			using (_conversation.SetAsCurrent())
			{
				var itemMapped = _mapper.Map<Rhino.Security.Mgmt.Dtos.OperationDto, Rhino.Security.Model.Operation>(item);
				Rhino.Security.Mgmt.Infrastructure.Mvc.ValidationHelpers.AddErrorsToModelState(ModelState, _validator.Validate(itemMapped), "item");
				Rhino.Security.Model.Operation itemToReturn = null;
				if (ModelState.IsValid)
				{
					var isNew = string.IsNullOrEmpty(item.StringId);
					if (isNew)
					{
						try
						{
							itemToReturn = _repository.Create(itemMapped);
						}
						catch (ArgumentException ex)
						{
							// this is hack to re-use the validations in Rhino Security which are returned as ArgumentExceptions
							Rhino.Security.Mgmt.Infrastructure.Mvc.ValidationHelpers.AddErrorToModelState(ModelState, ex.Message, "item", "Name");
						}
					}
					if (!isNew)
					{
						itemToReturn = _repository.Update(itemMapped);
					}
					if (ModelState.IsValid)
					{
						_conversation.Flush();
					}
				}
				var itemToReturnDto = itemToReturn != null ? _mapper.Map<Rhino.Security.Model.Operation, Rhino.Security.Mgmt.Dtos.OperationDto>(itemToReturn) : null;
				return Json(new
				{
					item = itemToReturnDto,
					success = ModelState.IsValid,
					errors = Rhino.Security.Mgmt.Infrastructure.Mvc.ValidationHelpers.BuildErrorDictionary(ModelState),
				});
			}
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
				var itemDto = _mapper.Map<Rhino.Security.Model.Operation, Rhino.Security.Mgmt.Dtos.OperationDto>(item);
				return Json(itemDto);
			}
		}

		public ActionResult Delete(string stringId)
		{
			if (string.IsNullOrEmpty(stringId))
			{
				throw new ArgumentException("stringId cannot be null or empty", "stringId");
			}

			using (_conversation.SetAsCurrent())
			{
				try
				{
					var item = _stringConverter.FromString(stringId);
					_repository.Delete(item);
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

		public ActionResult Search(System.Guid? id, string name, string comment, int start, int limit, string sort, string dir)
		{
			Log.DebugFormat("Search called");
			using (_conversation.SetAsCurrent())
			{
				var set = _repository.Search(id, name, comment);
				var items = set.Skip(start).Take(limit).Sort(sort, dir == "ASC").AsEnumerable();
				var dtos = _mapper.Map<IEnumerable<Rhino.Security.Model.Operation>, Rhino.Security.Mgmt.Dtos.OperationDto[]>(items);
				return Json(new { items = dtos, count = set.Count() });
			}
		}

		public ActionResult GetAllAsTree()
		{
			using (_conversation.SetAsCurrent())
			{
				return Json(BuildTreeViewModel(_repository.GetAll().AsEnumerable().Where(x => x.Parent == null)));
			}
		}

		private List<object> BuildTreeViewModel(IEnumerable<Rhino.Security.Model.Operation> operations)
		{
			var tree = new List<object>();
			foreach (var operation in operations)
			{
				if (operation.Children != null && operation.Children.Count > 0)
				{
					tree.Add(new { id = operation.Name, text = operation.Name, icon = @"images/cog_go.png", children = BuildTreeViewModel(operation.Children) });
				}
				else
				{
					tree.Add(new { id = operation.Name, text = operation.Name, leaf = true, icon = @"images/cog.png" });
				}
			}
			return tree;
		}
	}
}