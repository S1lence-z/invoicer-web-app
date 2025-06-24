using Application.Interfaces;

namespace Application.DTOs.Pdf
{
    public interface IPdfGenerationResult : IApiResult<byte[]>
    {
        string FileName { get; }
	}
}
