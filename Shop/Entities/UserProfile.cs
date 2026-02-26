using System;
using System.Collections.Generic;

namespace Shop.Entities;

public partial class UserProfile
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PersonalN { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string PhoneNo { get; set; } = null!;
}
