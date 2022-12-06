using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SOMIOD.Models
{
    public class Data
    {
        public int Id { get; set; }
        public String Content { get; set; }
        public DateTime CreationDate { get; set; }
        public Application Parent { get; set; }
        
        public Data(int id, String content, DateTime creationDate, Application parent)
    {
        {
            Id = id;
            Content = content;
            CreationDate = creationDate;
            Parent = parent;
        }
    }
}