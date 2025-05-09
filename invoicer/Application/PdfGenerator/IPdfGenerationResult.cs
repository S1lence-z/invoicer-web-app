using Domain.Interfaces;

namespace Application.PdfGenerator
{
    public interface IPdfGenerationResult : IResult<byte[]>
    {
        string FileName { get; }
	}
}
