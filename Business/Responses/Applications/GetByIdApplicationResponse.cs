﻿namespace Business.Responses.Applications;

public class GetByIdApplicationResponse
{
    public int UserId { get; set; }
    public int ApplicantId { get; set; }
    public int BootcampId { get; set; }
    public int ApplicationStateId { get; set; }
}
