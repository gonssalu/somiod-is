using System;

namespace SOMIOD.Models
{
    public class Module : Application
    {
        public Application Parent { get; set; }

        public Module(int id, string name, DateTime creationDate, Application parent) : base(id, name, creationDate)
        {
            Parent = parent;
        }
    }
}
