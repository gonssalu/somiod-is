﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SOMIOD.Models
{
    [XmlRoot(ElementName = "Module")]
    public class ModuleWithData : Module
    {
        [XmlElement(ElementName = "Data")]
        public List<Data> Data { get; set; }

        public ModuleWithData() { }

        public ModuleWithData(int id, string name, DateTime creationDate, int parent, List<Data> data) : base(id, name, creationDate, parent)
        {
            Data = data;
        }
    }
}