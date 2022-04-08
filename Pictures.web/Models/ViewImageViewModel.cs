using Pictures.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pictures.web.Models
{
    public class ViewImageViewModel
    {
        public Image Image { get; set; }
      
        public List<int> DisabledIds { get; set; }
    }
}
