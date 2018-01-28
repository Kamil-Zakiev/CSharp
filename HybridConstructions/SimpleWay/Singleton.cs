namespace SimpleWay
{
    internal class Singleton
    {
        // �������� �����������, ����� ����� �� ��� ������� ��������� ����������� new
        private Singleton() { }

        private static readonly Singleton _singleton = new Singleton();
        
        public static Singleton GetSingleton()
        {
            return _singleton;
        }
    }
}