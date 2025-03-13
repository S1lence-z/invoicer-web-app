using Domain.Interfaces;

namespace Backend.Utils.InvoicePdfGenerator
{
    public class PdfGenerationResult : IPdfGenerationResult
	{
        public bool IsSuccess { get; init; }
        public byte[]? Data { get; init; }
        public string? ErrorMessage { get; init; }
		public int StatusCode { get; init; }

		private PdfGenerationResult(bool isSuccess, byte[]? data, string? errorMessage, int statusCode)
		{
			if (isSuccess && data is null)
				throw new InvalidOperationException("Data cannot be null when IsSuccess is true.");

			IsSuccess = isSuccess;
			Data = data;
			ErrorMessage = errorMessage;
			StatusCode = statusCode;
		}

		public static PdfGenerationResult Success(byte[] pdfFile)
		{
			return new PdfGenerationResult(true, pdfFile, null, 200);
		}

		public static PdfGenerationResult Failure(string errorMessage, int statusCode = 500)
		{
			return new PdfGenerationResult(false, null, errorMessage, statusCode);
		}
	}
}
