using System;

namespace SOMIOD.Models
{
    public class Data
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public int Parent { get; set; }

        public Data(int id, string content, DateTime creationDate, int parent)
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
