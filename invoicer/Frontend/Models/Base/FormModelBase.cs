namespace Frontend.Models.Base
{
	public abstract class FormModelBase<TFormModel, TDto>
		where TFormModel : FormModelBase<TFormModel, TDto>, new()
		where TDto : class
	{
		public virtual int Id { get; set; }

		public bool IsNewEntity => Id == 0;

		public static TFormModel FromDto(TDto dto)
		{
			TFormModel formModel = new();
			if (dto is not null)
				formModel.PopulateFromDto(dto);
			return formModel;
		}

		public void PopulateFromDto(TDto dto) => MapPropertiesFromDto(dto);

		protected abstract void MapPropertiesFromDto(TDto dto);
		protected abstract void ResetProperties();

		public virtual void ClearModel()
		{
			Id = 0;
			ResetProperties();
		}
	}
}
