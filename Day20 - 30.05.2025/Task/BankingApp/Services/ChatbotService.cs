using System;
using Microsoft.ML;
using BankingApp.Models;
using BankingApp.Contexts;
using Banking.Models;


namespace BankingApp.Services;

public class ChatbotService
{
    private readonly PredictionEngine<ChatTrainingData, ChatPrediction> _predictionEngine;
    private readonly BankingContext _bankingcontext;

    // private readonly Dictionary<string, string> _answerMap;

    public ChatbotService(BankingContext bankingContext)
    {
        _bankingcontext = bankingContext;

        var mlContext = new MLContext();
        DataViewSchema schema;
        var model = mlContext.Model.Load("ML/ChatbotModel.zip", out schema);
        _predictionEngine = mlContext.Model.CreatePredictionEngine<ChatTrainingData, ChatPrediction>(model);

        
        // _answerMap = new Dictionary<string, string>
        // {
        //     {"create_bank_account", "To create a bank account, please visit our nearest branch or use our online banking portal." },
        //     { "working_hours", "Our working hours are Monday to Friday, 9 AM to 5 PM." },
        //     { "demat_account", "To open a demat account, please install ABCD app in playstore and setup your virtual KYC and after that we will connect you." },
        //     { "contact_support", "You can contact support via email at bank@gmail.com" },
        //     { "default", "Sorry, I didn't understand that." },
        //     {"greeting", "Hellow"}
        // };
    }

    public string GetAnswer(string userQuestion)
    {
        var prediction = _predictionEngine.Predict(new ChatTrainingData { Question = userQuestion });
        var confidence = prediction.Score.Max();
        var log = new ChatPredictionLog
        {
            UserQuestion = userQuestion,
            PredictedLabel = prediction.PredictedLabel,
            Confidence = confidence,
            Timestamp = DateTime.UtcNow
        };
        _bankingcontext.ChatPredictionLogs.Add(log);
        _bankingcontext.SaveChanges();
        // return _answerMap.TryGetValue(prediction.PredictedLabel, out var answer)
        //     ? answer
        //     : "Sorry, I didn't understand that.";
        var response = _bankingcontext.ChatResponses.FirstOrDefault(r => r.Label == prediction.PredictedLabel);

        return response?.AnswerText ?? "Sorry, I didn't understand that.";
    }
}
