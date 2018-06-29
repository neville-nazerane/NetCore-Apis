using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Models;

namespace TestWebApi.Controllers
{

    [ApiController, Route("[controller]")]
    public class EmployeeController : ControllerBase
    {

        [HttpPost]
        public int Post(Employee employee) => 80;

        [HttpGet("{id}")]
        public Employee Get(int id) => new Employee { FirstName = $"For {id}", LastName = "Lasted", Age = 5 };


    }
}
