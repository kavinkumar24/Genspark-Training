using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterBackendApp.Models;

public class TweetHashTag
{
    [Key]
    public int TweetId { get; set; }
    [ForeignKey("TweetId")]
    public Tweet? Tweet { get; set; }
    public int HashTagId { get; set; }
    [ForeignKey("HashTagId")]
    public HashTag? HashTag { get; set; }
}
