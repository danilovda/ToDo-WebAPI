﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApp.Models
{
    
    public class TodoItem
    {
        
        public long Id { get; set; }

        
        public string Name { get; set; }

        
        public bool IsComplete { get; set; }
    }
}