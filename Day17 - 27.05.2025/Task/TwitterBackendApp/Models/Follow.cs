using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace TwitterBackendApp.Models;

public class Follow
{
    public int Id { get; set; }
    public int FollowerId { get; set; }
    [ForeignKey(nameof(FollowerId))]
    [InverseProperty(nameof(User.Following))]
    public User? Follower { get; set; }
    public int FollowingId { get; set; }
    [ForeignKey(nameof(FollowingId))]
    [InverseProperty(nameof(User.Followers))]
    public User? Following { get; set; }
}
