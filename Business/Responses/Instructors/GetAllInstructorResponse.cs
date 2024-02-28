﻿using System.Globalization;

namespace Business.Responses.Instructors;

public class GetAllInstructorResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string CompanyName { get; set; }
}