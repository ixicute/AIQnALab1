using Azure;
using Azure.AI.Language.QuestionAnswering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIQnALab1
{
    internal class QnAService
    {
        public async Task<string> GetAnswer(string question, Uri endpoint, AzureKeyCredential credential, string projectName, string deploymentName)
        {
            QuestionAnsweringClient client = new QuestionAnsweringClient(endpoint, credential);
            QuestionAnsweringProject project = new QuestionAnsweringProject(projectName, deploymentName);
            Response<AnswersResult> response = client.GetAnswers(question, project);

            return response.Value.Answers[0].Answer;
            
        }
    }
}
