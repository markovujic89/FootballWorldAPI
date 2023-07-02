﻿using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PlayerDTO
    {
        public Guid Id { get; set; }
        public int Age { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Position { get; set; }
    }
}
