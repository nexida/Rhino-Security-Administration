using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Microsoft.Web.Mvc
{
    // http://haacked.com/archive/2010/04/15/sending-json-to-an-asp-net-mvc-action-method-argument.aspx
    // This class should be included in MVC 3
	public class JsonValueProviderFactory : ValueProviderFactory
	{
		private static void AddToBackingStore(Dictionary<string, object> backingStore, string prefix, object value)
		{
			{
				// dictionary?
				var d = value as IDictionary<string, object>;
				if(d != null)
				{
					foreach(var entry in d)
					{
						AddToBackingStore(backingStore, MakePropertyKey(prefix, entry.Key), entry.Value);
					}
					return;
				}
			}

			{
				// list?
				var l = value as IList;
				if(l != null)
				{
					for(int i = 0; i < l.Count; i++)
					{
						AddToBackingStore(backingStore, MakeArrayKey(prefix, i), l[i]);
					}
					return;
				}
			}

			// primitive
			backingStore[prefix] = value;
		}

		private static object GetDeserializedJson(ControllerContext controllerContext)
		{
			if(!controllerContext.HttpContext.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
			{
				// not JSON request
				return null;
			}

			var reader = new StreamReader(controllerContext.HttpContext.Request.InputStream);
			string bodyText = reader.ReadToEnd();
			if(String.IsNullOrEmpty(bodyText))
			{
				// no JSON data
				return null;
			}

			var serializer = new JavaScriptSerializer();
			object jsonData = serializer.DeserializeObject(bodyText);
			return jsonData;
		}

		public override IValueProvider GetValueProvider(ControllerContext controllerContext)
		{
			object jsonData = GetDeserializedJson(controllerContext);
			if(jsonData == null)
			{
				return null;
			}

			var backingStore = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			AddToBackingStore(backingStore, String.Empty, jsonData);
			return new DictionaryValueProvider<object>(backingStore, CultureInfo.CurrentCulture);
		}

		private static string MakeArrayKey(string prefix, int index)
		{
			return prefix + "[" + index.ToString(CultureInfo.InvariantCulture) + "]";
		}

		private static string MakePropertyKey(string prefix, string propertyName)
		{
			return (String.IsNullOrEmpty(prefix)) ? propertyName : prefix + "." + propertyName;
		}
	}
}