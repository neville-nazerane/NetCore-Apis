using Models;
using NetCore.Apis.Consumer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MobileUI.Consume
{
    class EmployeeAccess
    {

        const string path = "employee";

        public static async Task<ApiConsumedResponse> Post(Employee employee)
            => await ConsumerDefault.Consumer.PostAsync(path, employee);

    }
}
