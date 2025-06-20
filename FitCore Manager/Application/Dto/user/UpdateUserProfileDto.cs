﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.user
{
    public class UpdateUserProfileDto
    {
        public string? UserName { get; set; }
        public string? Phone { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
