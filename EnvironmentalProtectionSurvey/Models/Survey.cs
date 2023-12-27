using EnvironmentalProtectionSurvey.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnvironmentalProtectionSurvey.Models;

public partial class Survey
{
    public int Id { get; set; }
    [Required]
    public string? Title { get; set; }

    public string? UserType { get; set; }
    [Required]
    public string? Form { get; set; }

    public int? UserPost { get; set; }
    [LessDate]
    public DateTime? CreatedAt { get; set; }
    [Required]
    public DateTime? EndAt { get; set; }

    public bool? IsVisible { get; set; }

    public virtual ICollection<Award> Awards { get; set; } = new List<Award>();

    public virtual ICollection<FilledSurvey> FilledSurveys { get; set; } = new List<FilledSurvey>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
