using AICodeReviewRequester.Interfaces;
using Newtonsoft.Json;

namespace AICodeReviewRequester.Models.AI
{
    public class ClaudeResponse : IAIResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("role")]
        public string Role { get; set; }
        [JsonProperty("content")]
        public List<ClaudeContent> Contents { get; set; }
        [JsonProperty("model")]
        public string Model { get; set; }
        [JsonProperty("stop_reason")]
        public string StopReason { get; set; }
        [JsonProperty("stop_sequence")]
        public string StopSequence { get; set; }
        [JsonProperty("usage")]
        public ClaudeUsage Usage { get; set; }

        public bool TryCheckSuccess(out string message)
        {
            message = string.Empty;

            foreach (var cont in Contents)
            {
                if(cont.Type == "text")
                {
                    if (!string.IsNullOrEmpty(cont.Text))
                    {
                        message = cont.Text;
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public class ClaudeContent
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("text")]
        public string? Text { get; set; }
        [JsonProperty("id")]
        public string? Id { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("input")]
        public object? Input { get; set; }
        [JsonProperty("source")]
        public string? Source { get; set; }
    }

    public class ClaudeUsage
    {
        [JsonProperty("input_tokens")]
        public int InputTokens { get; set; }
        [JsonProperty("output_tokens")]
        public int OutputTokens { get; set; }
        [JsonProperty("cache_creation_input_tokens")]
        public int CacheCreationInputTokens { get; set; }
        [JsonProperty("cache_read_input_tokens")]
        public int CacheReadInputTokens { get; set; }
    }
}
