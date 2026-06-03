using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AICodeReviewRequester.Interfaces;
using AICodeReviewRequester.Models.AI;

namespace AICodeReviewRequester.Services
{
    public static class AIFactory
    {
        public static IEnumerable<IAIModel> GetModels()
        {
            // Gemini 
            yield return new GeminiModel("gemini-2.5-flash", GetGeminiKey());

            // Claude
            yield return new ClaudeModel("claude-3-5-sonnet-20241022", GetClaudeKey(), "2023-06-01", 1024);
        }

        private static string GetGeminiKey()
        {
            var path = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "AI",
                "gemini_key.txt");

            return File.ReadAllText(path);
        }

        private static string GetClaudeKey()
        {
            var path = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "AI",
                "claude_key.txt");

            return File.ReadAllText(path);
        }
    }
}
