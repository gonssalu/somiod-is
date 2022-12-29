using System;

namespace SOMIOD.Models
{
    public class Module : Application
    {
        public int Parent { get; set; }

        public Module(int id, string name, DateTime creationDate, int parent) : base(id, name, creationDate)
        {
            Parent = parent;
        }
    }
}
