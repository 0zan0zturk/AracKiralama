using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AracKiralama.Helpers
{
    public static class SessionHelper
    {
        private static readonly string UserSessionKey = "UserSessionKey";

        public static void SetUserSession(HttpContext httpContext, string userSessionData)
        {
            httpContext.Session.SetString(UserSessionKey, userSessionData);
        }

        public static string GetUserSession(HttpContext httpContext)
        {
            return httpContext.Session.GetString(UserSessionKey);
        }

        public static void ClearUserSession(HttpContext httpContext)
        {
            httpContext.Session.Remove(UserSessionKey);
        }
    }
}
