using AICodeReviewRequester.Interfaces;
using Newtonsoft.Json;

namespace AICodeReviewRequester.Models.AI
{
    public class GeminiResponse : IAIResponse
    {
        [JsonProperty("candidates")]
        public GeminiCandidate[] Candidates { get; set; }

        [JsonProperty("promptFeedback")]
        public GeminiPromptFeedback PromptFeedback { get; set; }

        public bool TryCheckSuccess(out string message)
        {
            message = string.Empty;

            foreach(var candidate in Candidates)
            {
                var parts = candidate.Content.Parts;

                if (parts == null || !parts.Any())
                    continue;

                foreach (var part in candidate.Content.Parts)
                {
                    if (!string.IsNullOrEmpty(part.Text))
                    {
                        message = part.Text;
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public class GeminiCandidate
    {
        [JsonProperty("content")]
        public GeminiContent Content { get; set; }

        [JsonProperty("finishReason")]
        public string FinishReason { get; set; } // 예: "STOP", "MAX_OUTPUT_TOKENS"

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("safetyRatings")]
        public GeminiSafetyRating[] SafetyRatings { get; set; }
    }

    // 콘텐츠의 실제 내용 (텍스트, 이미지 등)
    public class GeminiContent
    {
        [JsonProperty("parts")]
        public GeminiPart[] Parts { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; } // 예: "model", "user"
    }

    // 콘텐츠의 각 부분 (텍스트, 인라인 데이터 등)
    public class GeminiPart
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        // 다른 타입의 part (예: inline_data, function_call)가 있다면 여기에 추가 가능
        // [JsonProperty("inline_data")]
        // public GeminiInlineData InlineData { get; set; }
    }

    // 안전성 평가
    public class GeminiSafetyRating
    {
        [JsonProperty("category")]
        public string Category { get; set; } // 예: "HARM_CATEGORY_SEXUALLY_EXPLICIT"

        [JsonProperty("probability")]
        public string Probability { get; set; } // 예: "NEGLIGIBLE", "LOW", "MEDIUM", "HIGH"
    }

    // 프롬프트 피드백 (안전성 등)
    public class GeminiPromptFeedback
    {
        [JsonProperty("safetyRatings")]
        public GeminiSafetyRating[] SafetyRatings { get; set; }

        // blockReason이 있을 경우 추가 (안전성 필터링 등으로 응답이 차단될 때)
        [JsonProperty("blockReason")]
        public string BlockReason { get; set; } // 예: "SAFETY"
    }
}
