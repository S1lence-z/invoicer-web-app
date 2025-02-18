using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
	public abstract class ModelBase<TModel> where TModel : ModelBase<TModel>
	{
		public static void ConfigureEntity(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TModel>(entity =>
			{
				TModel instance = (TModel)Activator.CreateInstance(typeof(TModel));
				instance!.SetUp(modelBuilder);
			});
		}

		protected virtual void SetUp(ModelBuilder modelBuilder)
		{
			throw new NotImplementedException("SetUp method not implemented");
		}
	}
}
