using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnvironmentalProtectionSurvey.Models;

public partial class Support
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? TextMessage { get; set; }
}
