using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketSystem.Models
{
    public class ShouldNotContainWordBug: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var stringAsvalue = value as string;
            if(string.IsNullOrEmpty(stringAsvalue))
            {
                return false;
            }

            if(stringAsvalue.ToLower().Contains("bug"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
