using AICodeReviewRequester.Interfaces;

namespace AICodeReviewRequester.Models.Base
{
    public class AIResponse
    {
        #region Properties
        public bool Result { get; private set; }
        public IAIResponse Response { get; private set; }
        public string Message { get; private set; }
        public Exception Exception { get; private set; }
        #endregion

        #region Constructor
        private AIResponse()
        {
        }
        #endregion

        #region Functions
        public static AIResponse Success(IAIResponse response, string message)
        {
            return new AIResponse()
            {
                Result = true,  
                Response = response,
                Message = message
            };
        }

        public static AIResponse Fail(string message)
        {
            return new AIResponse()
            {
                Result = false,
                Message = message,
            };
        }


        public static AIResponse Error(Exception exception)
        {
            return new AIResponse()
            {
                Result = false,
                Message = exception.Message,
                Exception = exception
            };
        }
        #endregion

    }
}
