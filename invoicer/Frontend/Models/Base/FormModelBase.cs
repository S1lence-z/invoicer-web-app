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
				formModel.LoadFromDto(dto);
			return formModel;
		}

		public abstract TDto ToDto();
		protected abstract void LoadFromDto(TDto dto);
		protected abstract void ResetProperties();

		public virtual void ClearModel()
		{
			Id = 0;
			ResetProperties();
		}

		public override string ToString()
		{
			return $"FormModel: {typeof(TFormModel).Name}, Dto: {typeof(TDto).Name}, Id: {Id}";
		}
	}
}
