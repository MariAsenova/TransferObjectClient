using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace TransferObjectClient
{
    public class ClientTransfer
    {
        private NetworkStream networkStream;
        private TcpClient client;

        public void Start()
        {
            Console.WriteLine("Starting client");

            client = new TcpClient("127.0.0.1", 2910);
            networkStream = client.GetStream();
            Console.WriteLine("C# client connected ..");
        }

        public void WriteToServer(Customer customer)
        {
            string serializedCustomer = JsonSerializer.Serialize(customer, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            byte[] dataToServer = Encoding.ASCII.GetBytes(serializedCustomer);
            networkStream.Write(dataToServer, 0, dataToServer.Length);
        }

        public void ReadFromServer()
        {
            if (networkStream.CanRead)
            {
                byte[] readBuffer = new byte[1024];
                StringBuilder message = new StringBuilder();
                int bytesRead = 0;

                do
                {
                    bytesRead = networkStream.Read(readBuffer, 0, readBuffer.Length);
                    message.AppendFormat("{0}", Encoding.ASCII.GetString(readBuffer, 0, bytesRead));
                } while (networkStream.DataAvailable);
                
                Console.WriteLine("C# receives from Java server: " + (message));
                Customer customer = JsonSerializer.Deserialize<Customer>(message.ToString(), new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                Console.WriteLine("Deserialized obj from server: " + customer.Name);
            }
            else
            {
                Console.WriteLine("Cannot read from the network stream");
            }
        }
    }
}