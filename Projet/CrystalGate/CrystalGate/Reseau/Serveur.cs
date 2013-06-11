using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Collections.Generic;

namespace CrystalGate
{
    public class Serveur
    {
        static Socket serveur;
        static int NbMaxClients = 2;

        public static List<Socket> clients = new List<Socket> { };

        public static void Host()
        {
            serveur = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serveur.Bind(new IPEndPoint(IPAddress.Any, 5035));
            serveur.Listen(42);
            // On attend les clients
            Thread InWaintingClient = new Thread(WaitingClient);
            InWaintingClient.Start();

            // On créer un client qui est soi même et on le connecte à ce serveur
            Client.Connect("127.0.0.1");
        }

        public static void WaitingClient()
        {
            while (true)
            {
                if (clients.Count < NbMaxClients)
                {
                    // Attend qu'un client se connecte et lui fourni un identifiant
                    Socket nouveauClient = serveur.Accept();
                    clients.Add(nouveauClient);
                    // On lance la thread de reception pour ce socket
                    Thread Reception = new Thread(Receive);
                    Reception.Start();
                    // Envoie l'identifiant
                    clients[clients.Count - 1].Send(new byte[] { (byte)clients.Count });
                }
                else
                {
                    Send(new byte[] { 1 });
                    break;
                }
            }
        }

        public static void Send(byte[] buffer)
        {
            // Renvoi pour chaque client les informations reçus
            foreach (Socket s in clients)
                s.Send(buffer);
        }

        public static void Receive()
        {
            Socket c = clients[clients.Count - 1];
            int id = clients.Count;
            while (true)
            {
                // Initialisation des variables
                ASCIIEncoding ascii = new ASCIIEncoding();
                BinaryFormatter formatter = new BinaryFormatter();
                byte[] buffer = new byte[4];
                c.Receive(buffer);
                int header = BitConverter.ToInt32(buffer, 0);

                if (header == 0) // Si on recoit un personnage
                {
                    // Reception de la Taille
                    byte[] buffer2 = new byte[4];
                    c.Receive(buffer2);
                    int Length = BitConverter.ToInt32(buffer2, 0);

                    // Envoi de l'ID du joueur et de la taille
                    Send(BitConverter.GetBytes(id));
                    Send(buffer2);

                    // Données
                    byte[] buffer3 = new byte[Length];
                    c.Receive(buffer3);
                    Send(buffer3); // Envoie les infos reçus aux clients
                }
                else if (true) // Les autres cas
                {

                }
            }
        }
    }
}