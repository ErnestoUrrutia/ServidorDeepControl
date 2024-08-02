using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        int port = 5000;
        TcpListener server = new TcpListener(IPAddress.Any, port);
        server.Start();
        Console.WriteLine($"Servidor iniciado en el puerto {port}...");

        Task.Run(() => AcceptClients(server));

        Console.WriteLine("Presiona cualquier tecla para salir...");
        Console.ReadLine();
        server.Stop();
    }

    private static async Task AcceptClients(TcpListener server)
    {
        while (true)
        {
            var client = await server.AcceptTcpClientAsync();
            Console.WriteLine("Cliente conectado.");
            _ = HandleClient(client); // Manejar cliente de forma asíncrona
        }
    }

    private static async Task HandleClient(TcpClient client)
    {
        var stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;

        // Enviar mensajes periódicamente
        while (true)
        {
            
            string message = $"Mensaje del servidor a las {DateTime.Now}";
            byte[] data = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(data, 0, data.Length);
            Console.WriteLine($"Mensaje enviado: {message}");

            await Task.Delay(5000); // Esperar 5 segundos antes de enviar el siguiente mensaje
        }
    }
}

