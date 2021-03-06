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
            byte[] dataFromServer = new byte[1024];
            int bytesRead = networkStream.Read(dataFromServer, 0, dataFromServer.Length);
            string messageFromServer = Encoding.ASCII.GetString(dataFromServer, 0, bytesRead);
            Customer customer = JsonSerializer.Deserialize<Customer>(messageFromServer);
            Console.WriteLine("Reading customer from server: " + customer.Id);
        }
    }
}