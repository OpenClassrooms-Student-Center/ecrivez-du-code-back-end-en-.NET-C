using System.ComponentModel.DataAnnotations;

namespace Hamac.Api.Dtos;

public sealed class CreateLeaveRequestDto : IValidatableObject
{
    [Range(1, int.MaxValue)]
    public int EmployeeId { get; init; }

    [Required]
    public DateTime? StartDate { get; init; }

    [Required]
    public DateTime? EndDate { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndDate < StartDate)
        {
            yield return new ValidationResult(
                "La date de fin doit être postérieure ou égale à la date de début.",
                [nameof(EndDate)]);
        }
    }
}
