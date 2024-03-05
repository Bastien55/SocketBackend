using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SocketBackend.Enumeration;
using SocketBackend.Messages;

namespace SocketBackend
{
    public class SocketClient
    {
        private TcpClient _client;
        private readonly string _serverIp;
        private readonly int _port;

        public event EventHandler<Message> OnMessageReceived;
        public event EventHandler<UserMessage> OnUserMessageReceived;

        public SocketClient(string serverIp, int port)
        {
            _serverIp = serverIp;
            _port = port;
        }

        public async Task ConnectAsync()
        {
            _client = new TcpClient();
            await _client.ConnectAsync(_serverIp, _port);
            Console.WriteLine("Connecté au serveur.");

            await Task.Run(() => ReceiveMessages());
        }

        private void ReceiveMessages()
        {
            try
            {
                NetworkStream stream = _client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Message reçu : " + message);
                    var msgSplit = message.Split(';');

                    if (!(msgSplit.Length == 4))
                    {
                        OnMessageReceived?.Invoke(this, new Message(msgSplit[0],
                                            msgSplit[1],
                                            (TypeMessage)Enum.Parse(typeof(TypeMessage), msgSplit[2])));
                    }
                    else
                    {
                        OnUserMessageReceived?.Invoke(this, new UserMessage(msgSplit[0], msgSplit[1], (TypeMessage)Enum.Parse(typeof(TypeMessage), msgSplit[2]), msgSplit[3]));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur est survenue lors de la réception des messages : " + ex.Message);
            }
        }

        public async Task SendMessageAsync(string message)
        {
            try
            {
                NetworkStream stream = _client.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(buffer, 0, buffer.Length);
                Console.WriteLine("Message envoyé : " + message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur est survenue lors de l'envoi du message : " + ex.Message);
            }
        }

        public async Task SendMessageAsync(Message message)
        {
            await SendMessageAsync(message.ToString());
        }
    }
}
