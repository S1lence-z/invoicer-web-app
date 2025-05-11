namespace Shared.Interfaces
{
    public interface IPdfGenerationResult : IResult<byte[]>
    {
        string FileName { get; }
	}
}
