using System.ComponentModel.DataAnnotations.Schema;

namespace AspFromScratch.WebApi.User;

[Table("users")]
public class UserEntity
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("display_name")]
    public string DisplayName { get; set; }

    [Column("username")]
    public string Username { get; set; }

    [Column("password")]
    public string Password { get; set; }
}
