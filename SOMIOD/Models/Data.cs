using System;

namespace SOMIOD.Models
{
    public class Data
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public Module Parent { get; set; }

        public Data(int id, string content, DateTime creationDate, Module parent)
        {
            {
                Id = id;
                Content = content;
                CreationDate = creationDate;
                Parent = parent;
            }
        }
    }
}
