namespace Application.DTOs.Pdf
{
    public class PdfGenerationResult : IPdfGenerationResult
	{
        public bool IsSuccess { get; init; }
        public byte[]? Data { get; init; }
		public string FileName { get; init; }
        public string? ErrorMessage { get; init; }
		public int StatusCode { get; init; }

		private PdfGenerationResult(bool isSuccess, byte[]? data, string fileName, string? errorMessage, int statusCode)
		{
			if (isSuccess && data is null)
				throw new InvalidOperationException("Data cannot be null when IsSuccess is true.");

			IsSuccess = isSuccess;
			Data = data;
			FileName = fileName;
			ErrorMessage = errorMessage;
			StatusCode = statusCode;
		}

		public static PdfGenerationResult Success(byte[] pdfFile, string fileName)
		{
			return new PdfGenerationResult(true, pdfFile, fileName, null, 200);
		}

		public static PdfGenerationResult Failure(string errorMessage, int statusCode = 500)
		{
			return new PdfGenerationResult(false, null, string.Empty, errorMessage, statusCode);
		}
	}
}
