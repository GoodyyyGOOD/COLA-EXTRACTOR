using System.ComponentModel.DataAnnotations;

namespace Sofos2toDatawarehouseSyncer.Application.DTOs.Identity
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}