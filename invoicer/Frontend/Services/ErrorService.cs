namespace Frontend.Services
{
	public class ErrorService
	{
		public event EventHandler? OnErrorStateChanged;
		private string? _errorMessage;
		private bool _emittedError;

		public string? ErrorMessage
		{
			get => _errorMessage;
			private set
			{
				if (_errorMessage != value)
				{
					_errorMessage = value;
					OnErrorStateChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public bool EmittedError
		{
			get => _emittedError;
			private set
			{
				if (_emittedError != value)
				{
					_emittedError = value;
					OnErrorStateChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public void ShowError(string message)
		{
			ErrorMessage = message;
			EmittedError = true;
		}

		public void ClearError()
		{
			ErrorMessage = null;
			EmittedError = false;
		}
	}
}