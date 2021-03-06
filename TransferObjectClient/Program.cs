using System;

namespace TransferObjectClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Customer customer = new Customer()
            {
                Id = 2,
                Name = "Maria"
            };

            ClientTransfer client = new ClientTransfer();
            client.Start();
            client.WriteToServer(customer);
            Console.WriteLine("Client send an object to server");
            
            
            client.ReadFromServer();
        }
    }
}