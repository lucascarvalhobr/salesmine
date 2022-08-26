using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace SalesMine.Core.Users
{
    public interface IAspNetUser
    {
        string Name { get; }
        Guid GetUserId();
        string GetUserEmail();
        string GetUserToken();
        string GetUserRefreshToken();
        bool IsAuthenticated();
        bool IsInRole(string role);
        IEnumerable<Claim> ObterClaims();
        HttpContext GetHttpContext();
    }
}