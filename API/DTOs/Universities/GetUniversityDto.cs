﻿namespace API.DTOs.Universities
{
    public class GetEducationDto
    {
        public Guid Guid { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
    }
}
