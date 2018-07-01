using Models;
using NetCore.Apis.Consumer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Access
{
    public class EmployeeAccess
    {

        const string path = "employee";

        public static async Task<ApiConsumedResponse> Post(Employee employee)
            => await ConsumerDefault.Consumer.PostAsync(path, employee);

        public static async Task<ApiConsumedResponse<Employee>> Get(int id)
            => await ConsumerDefault.Consumer.GetAsync<Employee>($"{path}/{id}");

        public static async Task<ApiConsumedResponse> GetNoErrors()
            => await ConsumerDefault.Consumer.GetAsync($"{path}/nop");

    }
}
