using System.Text.Json;
using AICodeReviewRequester.Models.Base;

namespace AICodeReviewRequester.Models.AI
{
    public class GeminiModel : AIModel<GeminiResponse>
    {
        #region Properties
        public override string Name => "Gemini";
        public override string ID { get; }
        public override string APIKey { get; }
        public GeminiConfig Config { get; }
        #endregion

        #region Constructor
        public GeminiModel(string id, string key, GeminiConfig config = null)
        { 
            ID = id;
            APIKey = key;
            Config = config;
        }
        #endregion

        #region Functions
        protected override string CreateJsonBody(string prompt)
        {
            var body = default(object);

            if (Config == null)
            {
                body = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[] 
                            {
                                new { text = prompt }
                            }
                        }
                    }
                };
            }
            else
            {
                body = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    },

                    generationConfig = new
                    {
                        temperature = Config.Temperature,
                        topK = Config.TopK,
                        topP = Config.TopP,
                        maxOutputTokens = Config.MaxOutputTokens,
                        stopSequences = Config.StopSequences
                    }
                };

            }

            return System.Text.Json.JsonSerializer.Serialize(body, new JsonSerializerOptions { WriteIndented = true });
        }

        protected override string GetEndPoint()
        {
            return $"https://generativelanguage.googleapis.com/v1beta/models/{ID}:generateContent?key={APIKey}";
        }
        #endregion

    }
}
