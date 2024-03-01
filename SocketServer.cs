using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocketBackend
{
    public class SocketServer
    {
        private TcpListener _listener;
        private readonly int _port;
        private List<TcpClient> _clients = new List<TcpClient>();

        public SocketServer(int port)
        {
            _port = port;
        }

        public async Task StartAsync()
        {
            _listener = new TcpListener(IPAddress.Any, _port);
            _listener.Start();
            Console.WriteLine("Serveur démarré.");

            try
            {
                while (true)
                {
                    TcpClient client = await _listener.AcceptTcpClientAsync();
                    Console.WriteLine("Client connecté.");
                    _clients.Add(client);

                    Task.Run(async () => await HandleClientAsync(client));
                }
            }
            finally
            {
                _listener.Stop();
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Message reçu : " + message);

                    // Broadcast du message à tous les clients connectés
                    await BroadcastAsync(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur est survenue lors de la gestion du client : " + ex.Message);
            }
            finally
            {
                _clients.Remove(client);
                client.Close();
                Console.WriteLine("Client déconnecté.");
            }
        }

        private async Task BroadcastAsync(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            foreach (TcpClient client in _clients)
            {
                NetworkStream stream = client.GetStream();
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
        }
    }
}
