using Application.Interfaces;

namespace Application.DTOs.Pdf
{
    public interface IPdfGenerationResult : IResult<byte[]>
    {
        string FileName { get; }
	}
}
