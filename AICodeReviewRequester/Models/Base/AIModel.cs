using System.Net.Http;
using System.Text;
using AICodeReviewRequester.Interfaces;
using Newtonsoft.Json;

namespace AICodeReviewRequester.Models.Base
{
    public abstract class AIModel<TResponse> : IAIModel
        where TResponse : IAIResponse
    {
        #region Const Fields
        protected const string MESSAGE_WRONG_END_POINT = "Wrong End point";
        protected const string MESSAGE_WRONG_PARAMETER_JSON = "Wrong parameter json";
        protected const string MESSAGE_FAIL_PARSE_JSON = "fail to parse json";
        #endregion

        #region Properties
        public abstract string Name { get; }
        public abstract string ID { get; }
        public abstract string APIKey { get; }
        #endregion

        #region Functions
        protected abstract string GetEndPoint();
        protected virtual HttpClient CreateClient() => new HttpClient();
        protected abstract string CreateJsonBody(string prompt);

        private AIResponse ParseResponse(string response)
        {
            var resp = JsonConvert.DeserializeObject<TResponse>(response);

            return resp.TryCheckSuccess(out var message)
                ? AIResponse.Success(resp, message)
                : AIResponse.Fail(MESSAGE_FAIL_PARSE_JSON);
        }

        public async Task<AIResponse> Post(string prompt)
        {
            var endPoint = GetEndPoint();
            if (string.IsNullOrEmpty(endPoint))
                return AIResponse.Fail(MESSAGE_WRONG_END_POINT);

            var body = CreateJsonBody(prompt);
            if (string.IsNullOrEmpty(body))
                return AIResponse.Fail(MESSAGE_WRONG_PARAMETER_JSON);

            using (var client = CreateClient())
            {
                try
                {
                    // HTTP POST 요청 생성
                    var content = new StringContent(body, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(endPoint, content);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    
                    return response.IsSuccessStatusCode
                        ? ParseResponse(responseBody) // 응답 JSON 파싱 (간단 예시)
                        : AIResponse.Fail(responseBody);
                }
                catch (HttpRequestException ex)
                {
                    return AIResponse.Error(ex);
                }
                catch (Exception ex)
                {
                    return AIResponse.Error(ex);
                }
            }
        }
        #endregion
    }
}

