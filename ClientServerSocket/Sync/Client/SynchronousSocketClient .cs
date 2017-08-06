using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class SynchronousSocketClient
    {
        public static void StartClient()
        {
            byte[] buffer;

            try
            {
                // Установим удаленную точке для сокета
                // Этот пример используется порт с номером 11000 локального компьютера
                var hostName = Dns.GetHostName();
                var ipHostInfo = Dns.Resolve(hostName);
                var ipAddress = ipHostInfo.AddressList[0];
                var remoteEndPoint = new IPEndPoint(ipAddress, 11000);

                // Создание сокета TCP/IP
                var senderSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Подключаем сокет к удаленной точке
                senderSocket.Connect(remoteEndPoint);

                Console.WriteLine();
                Console.WriteLine("Сокет выполнил соединение с  {0}", senderSocket.RemoteEndPoint);

                var userLine = Console.ReadLine();
                var message = userLine + Environment.NewLine +
                              $"Время на текущий момент: {DateTime.Now.ToLocalTime()}";
                var bytesToSend = Encoding.UTF8.GetBytes(message);
                senderSocket.Send(bytesToSend);

                // Получаем ответ от удаленного устройства
                buffer = new byte[128];
                var bytesCount = senderSocket.Receive(buffer);
                var responseMessage = Encoding.UTF8.GetString(buffer, 0, bytesCount);
                Console.WriteLine("Удаленное устройство ответило следующее: {0}", responseMessage);

                // Закрываем сокет
                senderSocket.Shutdown(SocketShutdown.Both);
                senderSocket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        private static void Main(string[] args)
        {
            while (true)
            {
                StartClient();
            }
        }
    }
}