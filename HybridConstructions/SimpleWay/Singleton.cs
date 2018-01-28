namespace SimpleWay
{
    internal class Singleton
    {
        // закрытый конструктор, чтобы никто не мог создать экземпл€р последством new
        private Singleton() { }

        private static readonly Singleton _singleton = new Singleton();
        
        public static Singleton GetSingleton()
        {
            return _singleton;
        }
    }
}