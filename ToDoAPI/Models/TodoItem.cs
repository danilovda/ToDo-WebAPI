using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ToDoAPI.Models
{
    public class TodoItem
    {
        
        public long Id { get; set; }

        [Required(ErrorMessage = "Укажите текст ToDo")]
        public string Name { get; set; }

        [DefaultValue(false)]
        public bool IsComplete { get; set; }
    }
}
