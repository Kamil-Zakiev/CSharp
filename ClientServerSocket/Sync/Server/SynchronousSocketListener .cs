using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    internal class SynchronousSocketListener
    {
        // Входящие данные от клиента.
        public static string data;

        private static int bufferSize = 32;

        public static void StartListening()
        {
            // буфер для входных данных
            byte[] buffer;

            // Установим локальную конечную точку сокета
            // Dns.GetHostName возвращает имя хоста, выполняющего приложение
            var hostName = Dns.GetHostName();
            var ipHostInfo = Dns.Resolve(hostName);
            var ipAddress = ipHostInfo.AddressList[0]; // 192.168.1.106
            var localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Создание сокета TCP/IP
            var listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Связывание сокета с локальной конечной точкой
                listenerSocket.Bind(localEndPoint);

                // Прослушивание входящих соединений, указываем максимальное количество запросов в очереди обработки
                listenerSocket.Listen(10);

                // Начало прослуживание соединений
                while (true)
                {
                    Console.WriteLine("Ожидание подключения . . . ");

                    // Программа останавливается в ожидании входящего соединения
                    // При получении соединения создается новый сокет для обработки соединения
                    var handlerSocket = listenerSocket.Accept();
                    
                    // Обработка входящего соединения
                    var recievedBytes = new List<byte>();
                    while (true)
                    {
                        buffer = new byte[bufferSize];

                        // заполняем буфер, возвращается количество записанных байтов
                        var bytesRec = handlerSocket.Receive(buffer);
                        recievedBytes.AddRange(buffer.Take(bytesRec));

                        // Если получение магическое окончание слова, то прекращаем считывание
                        if (bytesRec < bufferSize)
                            break;
                    }
                    data = Encoding.UTF8.GetString(recievedBytes.ToArray(), 0, recievedBytes.Count);

                    var message = $"Получено сообщение: {data}";
                    Console.WriteLine(message);
                    Thread.Sleep(3000);

                    // отвечаем клиенту
                    var msg = Encoding.UTF8.GetBytes(data);
                    handlerSocket.Send(msg);
                    handlerSocket.Shutdown(SocketShutdown.Both);
                    handlerSocket.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void Main(string[] args)
        {
            StartListening();
        }
    }
}