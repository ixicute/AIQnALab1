using Microsoft.Extensions.Configuration;
using Azure;
using System;
using System.Threading.Tasks;

namespace AIQnALab1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //We create an instance of the configuration service
            ConfigService configService = new ConfigService();

            //Calling the method from the instance to load as the IConfiguration.
            IConfiguration configuration = configService.LoadConfiguration();

            //Loading variables from the appsettings.json file.
            Uri endpoint = new Uri(configuration["END-POINT"]);
            AzureKeyCredential credential = new AzureKeyCredential(configuration["CREDENTIAL"]);
            string projectName = configuration["PROJECTNAME"];
            string translatorEndpoint = configuration["TranslatorEndpoint"];
            string cogSrvKey = configuration["CogSrvKey"];
            string cogSrvRegion = configuration["CogSrvRegion"];

            //Creating instances for the translator and the qna service.
            TranslationService translatorService = new TranslationService();
            QnAService qnaService = new QnAService();

            string question = "";

            //Will continue running until user types "exit"
            while (question != "exit")
            {
                Console.WriteLine("Ask me anything or type \"exit\" to leave the chat!");
                question = Console.ReadLine();

                //Checking the request language by calling on the method DetectLanguage.
                string language = await translatorService.DetectLanguage(question, translatorEndpoint, cogSrvKey, cogSrvRegion);

                //if the language used by the user is not English, we will translate the question to English.
                if (language != "en")
                {
                    question = await translatorService.TranslateText(question, language, "en", translatorEndpoint, cogSrvKey, cogSrvRegion);
                }

                //checks if user wrote "exit", in which case the program will exit.
                if (question.ToLower() == "exit")
                {
                    Console.WriteLine("Thank you for chatting, have a great day!");
                    break;
                }

                //Calling the GetAnswer method, which handles the API-request to get an answer from our QnA service.
                string answer = await qnaService.GetAnswer(question, endpoint, credential, projectName, "production");

                //Here we check again for the language that the user had. If it was not english, we will translate the answer we got.
                if (language != "en")
                {
                    answer = await translatorService.TranslateText(answer, "en", language, translatorEndpoint, cogSrvKey, cogSrvRegion);
                }


                //The answer is presented to the user.
                Console.WriteLine($"A: {answer}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}