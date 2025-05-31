using System;
using Microsoft.ML.Data;

namespace BankingApp.Models;

public class ChatPrediction
{
    [ColumnName("PredictedLabel")]
    public string PredictedLabel { get; set; }
    [ColumnName("Score")]
    public float[] Score { get; set; }
}
