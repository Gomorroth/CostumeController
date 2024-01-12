namespace gomoru.su.CostumeController
{
    internal static class Singleton<T> where T : class, new()
    {
        public static T Instance { get; } = new();
    }
}
