using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Models;
using Microsoft.Extensions.Logging;

namespace TestWebApi.Controllers
{

    [ApiController, Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        public ILogger<EmployeeController> Logger { get; }

        public EmployeeController(ILogger<EmployeeController> logger){
            Logger = logger;
        }

        [HttpPost]
        public string Post(Employee e)
            => $"Fname: {e.FirstName}, Lname: {e.LastName}, Age: {e.Age}, fire: {e.ToBeFiredOn}, death: {e.IsDead}";

        [HttpGet("{id}")]
        public Employee Get(int id) => new Employee { FirstName = $"Made in {id}", LastName = "Blasted", Age = 25 };


        [HttpGet("nop")]
        public IActionResult Get() => BadRequest("invalid error");

    }
}
