using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Collections.Generic;
using CrystalGate.SceneEngine2;

namespace CrystalGate
{
    public class Serveur
    {
        public static Socket serveur;
        static int NbMaxClients = 2;
        
        public static bool IsRunning;

        public static List<Socket> clients = new List<Socket> { };
        public static List<Players> joueurs = new List<Players>();
        public static List<Thread> threads = new List<Thread>();

        public static int current = 0; // client que le serveur ecoute , commence a 0

        public static int LastDead; // represente la deniere unité qui vient de mourir selon le serveur

        public static void Host()
        {
            try
            {
                serveur = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serveur.Bind(new IPEndPoint(IPAddress.Any, 5035));
                serveur.Listen(42);
                IsRunning = true;
                // On attend les clients
                Thread InWaintingClient = new Thread(WaitingClient);
                InWaintingClient.Start();

                // On créer un client qui est soi même et on le connecte à ce serveur
                SceneEngine2.CoopSettingsScene.ip = IPAddress.Parse("127.0.0.1");
                Client.Connect();
                SceneHandler.gameState = GameState.CoopConnexion;
            }
            catch
            {
                SceneHandler.coopSettingsScene.Error = true;
            }
        }

        public static void WaitingClient()
        {
            while (IsRunning)
            {
                if (clients.Count < NbMaxClients)
                {
                    // Attend qu'un client se connecte et lui fourni un identifiant
                    Socket nouveauClient = serveur.Accept();
                    clients.Add(nouveauClient);
                    // On lance la thread de reception pour ce socket
                    threads.Add(new Thread(Receive));
                    threads[threads.Count - 1].Start();
                    // Envoie l'identifiant
                    clients[clients.Count - 1].Send(new byte[] { (byte)clients.Count });
                }
                else // Si tout le monde est la , on arrete d'attendre les clients
                    break;
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
            try
            {

                Socket c = clients[clients.Count - 1];
                int id = clients.Count;
                byte[] buffer = new byte[4];
                while (IsRunning)
                {
                    // Initialisation des variables
                    ASCIIEncoding ascii = new ASCIIEncoding();
                    BinaryFormatter formatter = new BinaryFormatter();

                    c.Receive(buffer);

                    if (id - 1 == current)
                    {
                        int header = BitConverter.ToInt32(buffer, 0);

                        if (header == 42) // Si on recoit un personnage
                        {
                            //header
                            Send(buffer);
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
                        else if (header == 1) // Si on reçoit une personne 
                        {

                            // Reception de la Taille
                            byte[] buffer2 = new byte[4];
                            c.Receive(buffer2);
                            int Length = BitConverter.ToInt32(buffer2, 0);


                            // Données
                            byte[] buffer3 = new byte[Length];
                            c.Receive(buffer3);
                            //header
                            Send(buffer);
                            Send(buffer2);
                            Send(buffer3); // Envoie les infos reçus aux clients
                        }
                        else if (header == 2) // Si on reçoit un message 
                        {
                            //header
                            Send(buffer);
                            // Reception de la Taille
                            byte[] buffer2 = new byte[4];
                            c.Receive(buffer2);
                            int Length = BitConverter.ToInt32(buffer2, 0);

                            Send(buffer2);

                            // Données
                            byte[] buffer3 = new byte[Length];
                            c.Receive(buffer3);
                            Send(buffer3); // Envoie les infos reçus aux clients
                        }
                        else // Si on a recu un header incorrect, on attend de recevoir un header correct
                        {
                            // header incorrect
                        }
                        current = (current + 1) % 2;
                    }
                }
            }
            catch
            {
                UI.Error = true;
            }

        }

        public static void Shutdown()
        {
           /* if (serveur != null)
                serveur.Close();
            IsRunning = false; // Arrete les threads
            clients.Clear();*/
        }
    }
}