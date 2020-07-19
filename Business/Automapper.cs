namespace Business
{
    public static class Automapper
    {
        public static T Map<T>(object obj) where T : new()
        {
            return new T();
        }
    }
}