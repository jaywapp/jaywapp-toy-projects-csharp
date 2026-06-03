namespace AICodeReviewRequester.Interfaces
{
    public interface IAIResponse
    {
        bool TryCheckSuccess(out string message);
    }
}
