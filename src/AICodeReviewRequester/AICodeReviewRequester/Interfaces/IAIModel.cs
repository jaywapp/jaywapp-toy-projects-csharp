using AICodeReviewRequester.Models.Base;

namespace AICodeReviewRequester.Interfaces
{
    public interface IAIModel
    {
        Task<AIResponse> Post(string prompt);
    }
}
