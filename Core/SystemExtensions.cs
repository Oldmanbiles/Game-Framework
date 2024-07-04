namespace GameFramework
{
    public static class SystemExtensions
    {
        public static T GetSystem<T>(this ICanGetSystem self) where T : class, ISystem
        {
            return self.GetArchitecture().GetSystem<T>();
        }

        public static T GetMono<T>(this ICanGetSystem self) where T : MonoSystem
        {
            return self.GetArchitecture().GetMono<T>();
        }
    }
}