using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Employee
    {

        [Required, MaxLength(20)]
        public string FirstName { get; set; }

        [MaxLength(25)]
        public string LastName { get; set; }

        public int Age { get; set; }

        public DateTime ToBeFiredOn { get; set; }

    }

}
