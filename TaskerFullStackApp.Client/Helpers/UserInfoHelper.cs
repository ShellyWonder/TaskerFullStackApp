﻿using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace TaskerFullStackApp.Client.Helpers
{
    public static class UserInfoHelper
    {
        public static async Task<UserInfo> GetUserInfoAsync(Task<AuthenticationState> authStateTask)
        {
            if (authStateTask is null) return null!;
            var authState = await authStateTask;
            System.Security.Claims.ClaimsPrincipal user = authState.User;

            try
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var email = user.FindFirst(ClaimTypes.Email)?.Value;
                var firstName = user.FindFirst("FirstName")?.Value;
                var lastName = user.FindFirst("LastName")?.Value;
                return new UserInfo
                {
                    UserId = userId ?? string.Empty,
                    Email = email ?? string.Empty,
                    FirstName = firstName ?? string.Empty,
                    LastName = lastName ?? string.Empty
                };
            }
            catch (Exception)
            {

                return null!;
            }
        }
    }
}
