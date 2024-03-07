using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SocketBackend.Enumeration;
using SocketBackend.Messages;
using SocketBackend.Context;
using SocketBackend.Models;
using SocketBackend.Repository;

namespace SocketBackend
{
    public class SocketServer
    {
        private TcpListener _listener;
        private readonly int _port;
        private List<TcpClient> _clients = new List<TcpClient>();

        private UserRepository Repository { get; set; }

        public SocketServer(int port)
        {
            _port = port;
            Repository = new UserRepository(new GameOfLifeContext());
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

                    await MessageFilter(message, client);
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

        private async Task SendToClientAsync(string message, TcpClient client)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            NetworkStream stream = client.GetStream();
            await stream.WriteAsync(buffer, 0 , buffer.Length);
        }

        private async Task MessageFilter(string message, TcpClient client)
        {
            TypeMessage msgType;
            var messageSplit = message.Split(';');
            UserMessage msg = null;

            if (Enum.TryParse(messageSplit[2], out msgType))
            {
                switch(msgType)
                {
                    case TypeMessage.USER_REQUEST_CONNECTION:
                        User user = Repository.FindByName(messageSplit[0]);

                        if (user != null)
                        {
                            msg = new UserMessage(user, TypeMessage.USER_VALID_CONNECTION);
                        }
                        else
                        {
                            var usr = new User() { Name = messageSplit[0] };
                            msg = new UserMessage(usr, TypeMessage.USER_INVALID_CONNECTION);
                        }

                        await SendToClientAsync(msg.ToString(), client);
                        break;
                    case TypeMessage.NEW_USER:
                        User newUser = new User() 
                        { 
                            Id = Repository.GetAll().Last().Id + 1,
                            Name = messageSplit[0], // Name
                            Rule = messageSplit[3]  // Rule
                        };

                        if (Repository.FindByName(newUser.Name) != null)
                        {
                            msg = new UserMessage(newUser, TypeMessage.USER_ALREADY_EXIST);
                        }
                        else
                        {
                            Repository.Insert(newUser);
                            msg = new UserMessage(newUser, TypeMessage.USER_REGISTERED);
                        }

                        await SendToClientAsync(msg.ToString(), client);
                        break;
                    case TypeMessage.USER_UPDATE:
                        var userToUpdate = Repository.FindByID(int.Parse(messageSplit[1]));
                        if(userToUpdate != null)
                        {
                            userToUpdate.Name = messageSplit[0];
                            userToUpdate.Rule = messageSplit[3];

                            Repository.Update(userToUpdate);
                        }
                        break;
                    default:
                        await BroadcastAsync(message);
                        break;
                }
            }
        }
    }
}
