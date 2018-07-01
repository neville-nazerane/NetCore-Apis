using Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NetCore.Apis.Consumer.Test
{
    public class EmployeeTest : TesterAbstract
    {

        const string path = "employee";

        [Fact]
        public async Task Post()
        {
            var res = await Consumer.PostAsync(path, new Employee { });
            Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
            res = await Consumer.PostAsync(path, new Employee { FirstName = "Batman"  });
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
            res = await Consumer.PostAsync(path, new Employee { FirstName = "nananananananana Batman" });
            Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
            res = await Consumer.PostAsync(path, new Employee { LastName = "nananananananananananananananananananananananana Batman" });
            Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
            res = await Consumer.PostAsync(path, null);
            Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
        }

        [Fact]
        public async Task Get()
        {
            var res = await Consumer.GetAsync<Employee>($"{path}/55");
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }

        [Fact]
        public async Task GetNoErr()
        {
            var res = await Consumer.GetAsync($"{path}/nop");
            Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
        }

    }
}
