using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TicketSystem.Models
{
    public class ShouldNotContainHTML : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            if(valueAsString == null)
            {
                return true;
            }
            Regex tagRegex = new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>");

            if(tagRegex.IsMatch(valueAsString))
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
