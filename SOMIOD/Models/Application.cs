using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOMIOD.Models
{
    public class Application
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public DateTime CreationDate { get; set; }

        public Application(int id, String name, DateTime creationDate)
        {
            Id = id;
            Name = name;
            CreationDate = creationDate;
        }
    }
}