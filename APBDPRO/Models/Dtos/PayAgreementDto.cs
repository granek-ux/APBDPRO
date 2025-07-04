﻿using System.ComponentModel.DataAnnotations;

namespace APBDPRO.Models.Dtos;

public class PayAgreementDto
{
    [Required] [Length(10, 11)] public string PeselOrKrs { get; set; } = null!;
    
    [Required][MaxLength(50)] public string SoftwareName { get; set; } = null!;
    
    [Required] public double Amount { get; set; }
    
}