﻿using System.ComponentModel.DataAnnotations;

namespace KidsApi.Models.Requests
{
        public class UserLoginRequest
        {
            [Required]
            public string Username { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }

