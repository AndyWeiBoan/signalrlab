using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SignalrLab {
    public class DomainRestrictedRequirement :
    AuthorizationHandler<DomainRestrictedRequirement, HubInvocationContext>,
    IAuthorizationRequirement {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            DomainRestrictedRequirement requirement,
            HubInvocationContext resource) {
            var httpCtx = resource.Context.GetHttpContext();
            httpCtx.Request.Headers.Add("test", "sdfasdfdasfas");
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
