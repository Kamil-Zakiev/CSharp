using System.Threading;

namespace DoubleCheckLocking
{
    internal class Singleton
    {
        // �������� �����������, ����� ����� �� ��� ������� ��������� ����������� new
        private Singleton() { }

        private static Singleton _singleton;

        private static readonly object LockObj = new object();

        public static Singleton GetSingleton()
        {
            // �������� �1
            if (_singleton != null)
            {
                return _singleton;
            }

            Monitor.Enter(LockObj);
            // ������ ����� ����� ����� ����� ����������� ������

            // �������� �2
            if (_singleton != null)
            {
                #region ���������
                // ���� ������ ����� �������� ��������� �������, ��� ����� ����� ����������, ������� ������ � ������� ���������� 
                // ����� �������� �1, �� ������ ��� ��������� ������ �� ����� 
                #endregion
                Monitor.Exit(LockObj);
                return _singleton;
            }

            #region ���������
            // ��� ��� ���������� ����� ������� �������� ������ ��� �������, � ����� ������� ��� �����������,
            // �� ������ ����� ����� �������� ���������� ������, � �������� ����������� �� �������� ������.
            // ������� - ����������� ������, ������� ����� ���������� ������ � ������ ������ ������� Volatile.Write
            // ��� ���� �������������, ��� �������� ������ � ���������� temp ���������� �� ������ � _singleton 
            #endregion
            var temp = new Singleton();
            Volatile.Write(ref _singleton, temp);
            
            Monitor.Exit(LockObj);

            return _singleton;
        }
    }
}