using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
	public abstract class ModelBase<TModel> where TModel : class
	{
		private static HashSet<string> GetPrimaryKeyProperties(DbContext context)
		{
			return context.Model?
				.FindEntityType(typeof(TModel))?
				.FindPrimaryKey()?
				.Properties
				.Select(prop => prop.Name)
				.ToHashSet() ?? [];
		}

		public TModel Replace(TModel newModel, DbContext context)
		{
			HashSet<String> primaryKeyProperties = GetPrimaryKeyProperties(context);
			foreach (var property in typeof(TModel).GetProperties())
			{
				if (primaryKeyProperties.Contains(property.Name))
				{
					continue;
				}
				var value = property.GetValue(newModel);
				property.SetValue(this, value);
			}
			return newModel;
		}
	}
}
