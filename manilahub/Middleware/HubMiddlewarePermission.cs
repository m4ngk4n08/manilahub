using Microsoft.AspNetCore.Authorization;

namespace manilahub.Middleware
{
    public class HubMiddlewarePermission : IAuthorizationRequirement
    {
        public HubMiddlewarePermission(bool isAuthorize)
        {
            IsAuthorize = isAuthorize;
        }

        public bool IsAuthorize { get; set;  }
    }
}
