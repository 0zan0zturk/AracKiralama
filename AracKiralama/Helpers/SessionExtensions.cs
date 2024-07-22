using System.Text.Json;

namespace AracKiralama.Helpers
{
    public static class SessionExtensions
    {
        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
        public static T? GetJson<T>(this ISession session, string key) // hangi tipte olduğunu bilmediğimiz için T? yaptık
        {
            var sessionData = session.GetString(key);
            return sessionData==null ? default(T) : JsonSerializer.Deserialize<T>(sessionData);
        }
        
    }
}
