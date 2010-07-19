using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Rhino.Security.Mgmt.Infrastructure.Mvc
{
	public class ValidationHelpers
	{
		public static void AddErrorsToModelState(ModelStateDictionary modelState, IEnumerable<ValidationError> errors, string parameterName)
		{
			foreach(ValidationError invalidValue in errors)
			{
				modelState.AddModelError(parameterName + "." + invalidValue.PropertyPath, invalidValue.Message);
			}
		}

		public static void AddErrorToModelState(ModelStateDictionary modelState, string errorMsg, string parameterName, string propertyPath)
		{
			modelState.AddModelError(parameterName + "." + propertyPath, errorMsg);
		}

		public static object BuildErrorDictionary(ModelStateDictionary modelState)
		{
			return BuildErrorDictionary(MakeHierarchical(modelState));
		}

		public static PropertyError MakeHierarchical(ModelStateDictionary modelStateDictionary)
		{
			var root = new PropertyError();
			foreach(var kvp in modelStateDictionary.Where(kvp => kvp.Value.Errors.Count > 0))
			{
				PropertyError current = root;
				foreach(string property in kvp.Key.Split('.', '[', ']').Where(k => k != ""))
				{
					int index;
					current = int.TryParse(property, out index) ? current[index] : current[property];
				}
				foreach(ModelError modelError in kvp.Value.Errors)
				{
					current.Errors.Add(modelError);
				}
			}
			return root;
		}

		public static object BuildErrorDictionary(PropertyError root)
		{
			if(root.Indexes.Count > 0)
			{
				var ary = new object[root.Indexes.Keys.Max() + 1];
				foreach(var kvp in root.Indexes)
				{
					ary[kvp.Key] = BuildErrorDictionary(kvp.Value);
				}
				return ary;
			}
			if(root.Properties.Count > 0)
			{
				return root.Properties.ToDictionary(kvp => kvp.Key, kvp => BuildErrorDictionary(kvp.Value));
			}
			return root.BuildMessage();
		}

		#region Nested type: PropertyError

		public class PropertyError
		{
			public PropertyError()
			{
				Errors = new ModelErrorCollection();
				Properties = new Dictionary<string, PropertyError>();
				Indexes = new Dictionary<int, PropertyError>();
			}

			public ModelErrorCollection Errors { get; private set; }
			public Dictionary<string, PropertyError> Properties { get; private set; }
			public Dictionary<int, PropertyError> Indexes { get; private set; }

			public PropertyError this[string property]
			{
				get
				{
					if(!Properties.ContainsKey(property))
					{
						Properties.Add(property, new PropertyError());
					}
					return Properties[property];
				}
			}

			public PropertyError this[int index]
			{
				get
				{
					if(!Indexes.ContainsKey(index))
					{
						Indexes.Add(index, new PropertyError());
					}
					return Indexes[index];
				}
			}

			public string BuildMessage()
			{
				return string.Join(". ", Errors.Select(e => e.Exception == null ? e.ErrorMessage : e.Exception.Message).ToArray());
			}
		}

		#endregion
	}
}