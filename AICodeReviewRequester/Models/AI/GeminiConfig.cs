namespace AICodeReviewRequester.Models.AI
{
    public class GeminiConfig
    {
        #region Properties
        public double Temperature { get; }
        public int TopK { get; }
        public int TopP { get; }
        public int MaxOutputTokens { get; }
        public string[] StopSequences { get; }
        #endregion

        #region Consturctor
        public GeminiConfig(double temperature, int topK, int topP, int maxOutputTokens, params string[] stopSquences)
        {
            Temperature = temperature;
            TopK = topK;
            TopP = topP;
            MaxOutputTokens = maxOutputTokens;
            StopSequences = stopSquences;
        }
        #endregion
    }
}
