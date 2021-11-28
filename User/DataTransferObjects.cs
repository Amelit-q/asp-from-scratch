﻿using System.ComponentModel.DataAnnotations;

namespace AspFromScratch.WebApi.User;

public class ReadUserDto
{
    /// <summary>User identifier</summary>
    /// <example>xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx</example>
    public Guid Id { get; set; }

    /// <summary>Display name for user</summary>
    /// <example>Sylwester Asia</example>
    public string DisplayName { get; set; }

    /// <summary>Username for authorization</summary>
    /// <example>Sylwester Asia</example>
    public string Username { get; set; }
}

public class RegisterUserDto
{
    /// <summary>Display name for user</summary>
    /// <example>Sylwester Asia</example>
    [Required]
    [MaxLength(50)]
    [RegularExpression(@"^[\w\s]*$")]
    public string DisplayName { get; set; }

    /// <summary>Username for authorization</summary>
    /// <example>Sylwester Asia</example>
    [Required]
    [MaxLength(30)]
    [RegularExpression(@"^[\w\s\d]*$")]
    public string Username { get; set; }

    /// <summary>Password for authorization.</summary>
    /// <example>QWERTY123</example>
    [Required]
    [MinLength(6)]
    [MaxLength(20)]
    [RegularExpression(@"^[\w\s\d]*$")]
    public string Password { get; set; }
}