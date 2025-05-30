using Microsoft.ML;
using Microsoft.ML.Data;
using BankingApp.Models;
using BankingApp.Contexts;

public class ModelTrainer
{
    private readonly BankingContext _bankingContext;

    public ModelTrainer(BankingContext bankingContext)
    {
        _bankingContext = bankingContext;
    }

   public void TrainAndSaveModel()
{
    Console.WriteLine("Starting training...");

    var mlContext = new MLContext();

    var trainingDataFromDb = _bankingContext.ChatTrainingData
        .Select(x => new ChatData { Question = x.Question, Label = x.Label })
        .ToList();

    Console.WriteLine($"Training data count: {trainingDataFromDb.Count}");

    if (trainingDataFromDb.Count == 0)
    {
        Console.WriteLine("No training data found. Cannot train model.");
        return;
    }

    IDataView dataView = mlContext.Data.LoadFromEnumerable(trainingDataFromDb);

    var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label")
        .Append(mlContext.Transforms.Text.FeaturizeText("Features", "Question"))
        .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
        .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

    var model = pipeline.Fit(dataView);

   var modelDir = Path.Combine(Directory.GetCurrentDirectory(), "ML");
    Directory.CreateDirectory(modelDir);

    var modelPath = Path.Combine(modelDir, "ChatbotModel.zip");
    mlContext.Model.Save(model, dataView.Schema, modelPath);

    Console.WriteLine($"Model saved to: {modelPath}");

}

}
