using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TwitterBackendApp.Models;

public class HashTag
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int HashTagId { get; set; }
    public string TagName { get; set; } = string.Empty;
    public ICollection<TweetHashTag>? TweetHashtags { get; set; }
}
