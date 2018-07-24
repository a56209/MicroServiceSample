using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.Api.ViewModels
{
    public class TagContactInputViewModel
    {
        public TagContactInputViewModel()
        {
            Tags = new List<string>();
        }

        public int ContactId { get; set; }
        public List<string> Tags { get; set; }
    }
}
