using System.ComponentModel.DataAnnotations;
using AIMS.SharedKernel;

namespace AIMS.Core.Entities;

public class UserAccount : BaseEntity
{
    [MaxLength(150)]
    public string UserName { get; set; }
    [MaxLength(250)]
    public string Email { get; set; }
    [MaxLength(250)]
    public string FullName { get; set; }
    [MaxLength(250)]
    public string Password { get; set; }

}