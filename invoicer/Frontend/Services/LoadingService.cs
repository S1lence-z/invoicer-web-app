namespace Frontend.Services
{
	public class LoadingService
	{
		public event EventHandler? OnLoadingStateChanged;

		private bool _isLoading;

		public bool IsLoading
		{
			get => _isLoading;
			set
			{
				if (_isLoading != value)
				{
					_isLoading = value;
					OnLoadingStateChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public void StartLoading()
		{
			IsLoading = true;
		}

		public void StopLoading()
		{
			IsLoading = false;
		}
	}
}
