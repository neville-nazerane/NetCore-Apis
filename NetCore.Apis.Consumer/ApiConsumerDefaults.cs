using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Apis.Consumer
{

    /// <summary>
    /// The default setup that can be assigned to an ApiConsumer
    /// </summary>
    public abstract class ApiConsumerDefaults
    {

        public virtual Task OnSuccess(ApiConsumedResponse response) => Task.CompletedTask;
        public virtual Task OnFailed(ApiConsumedResponse response) => Task.CompletedTask;
        public virtual Task OnBadRequest(ApiConsumedResponse response) => Task.CompletedTask;
        public virtual Task OnUnauthorized(ApiConsumedResponse response) => Task.CompletedTask;
        public virtual Task OnForbidden(ApiConsumedResponse response) => Task.CompletedTask;
        public virtual Task OnNotFound(ApiConsumedResponse response) => Task.CompletedTask;
        public virtual Task OnInternalServerError(ApiConsumedResponse response) => Task.CompletedTask;
        public virtual Task OnBadGateway(ApiConsumedResponse response) => Task.CompletedTask;
        public virtual Task OnGatewayTimeout(ApiConsumedResponse response) => Task.CompletedTask;
        public virtual Task OnServiceUnavailable(ApiConsumedResponse response) => Task.CompletedTask;
        
    }
}
