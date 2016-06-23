using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public interface IEntity
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}