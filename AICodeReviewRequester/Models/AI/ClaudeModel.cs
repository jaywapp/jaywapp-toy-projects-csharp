using System.Net.Http;
using AICodeReviewRequester.Models.Base;
using Newtonsoft.Json;

namespace AICodeReviewRequester.Models.AI
{
    public class ClaudeModel : AIModel<ClaudeResponse>
    {
        #region Properties
        public override string Name => "Claude";
        public override string ID { get; }
        public override string APIKey { get; }
        public string AnthropicVersion { get; }
        public int MaxTokens { get; }
        #endregion

        #region Constructor
        public ClaudeModel(string id, string key, string anthropicVersion, int maxTokens = 1000)
        {
            ID = id;
            APIKey = key;
            AnthropicVersion = anthropicVersion;
            MaxTokens = maxTokens;
        }
        #endregion

        #region Functions
        protected override string CreateJsonBody(string prompt)
        {
            var requestBody = new
            {
                model = ID,
                max_tokens = MaxTokens,
                messages = new[]
                {
                    new { role = "user", content = prompt },
                }
            };

            return JsonConvert.SerializeObject(requestBody);
        }

        protected override HttpClient CreateClient()
        {
            var client = base.CreateClient();

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("x-api-key", APIKey);
            client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");

            return client;
        }

        protected override string GetEndPoint()
        {
            return "https://api.anthropic.com/v1/messages";
        }
        #endregion
    }
}
