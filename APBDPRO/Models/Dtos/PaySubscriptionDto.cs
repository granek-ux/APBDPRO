using System.ComponentModel.DataAnnotations;

namespace APBDPRO.Models.Dtos;

public class PaySubscriptionDto : PayAgreementDto
{
    [MaxLength(30)] [Required] public string SubscriptionName { get; set; } = null!;
}