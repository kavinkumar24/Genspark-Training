namespace BankingApp.Models
{
    public class ChatPredictionLog
{
    public int Id { get; set; }
    public string UserQuestion { get; set; }
    public string PredictedLabel { get; set; }
    public float Confidence { get; set; }
    public DateTime Timestamp { get; set; }
}

}