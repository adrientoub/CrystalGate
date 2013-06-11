using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using CrystalGate.SceneEngine2;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class Client
    {
        static Socket client;
        public static int id;
        public static bool isConnected { get { return client != null && client.Connected; } }

        public static void Connect(string ip)
        {
            // On creer le socket et on se connecte
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(new IPEndPoint(IPAddress.Parse(ip), 5035));
            
            // On attend notre identifiant sur le reseau
            byte[] buffer = new byte[1];
            client.Receive(buffer);
            id = buffer[0];

            //On lance la thread de reception

            Thread Reception = new Thread(Receive);
            Reception.Start();
        }

        public static void Send()
        {
            // Initialisation des variables
            ASCIIEncoding ascii = new ASCIIEncoding();
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            
            // Definition du contenu
            Player p = new Player();
            p.noeuds = Outil.GetJoueur(Client.id).champion.ObjectifListe;

            formatter.Serialize(stream, p);
            byte[] buffer = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(buffer, 0, buffer.Length);
            // Envoi de l'id, puis de la taille, puis de l'objet
            client.Send(new byte[] { (byte)Client.id });
            client.Send(BitConverter.GetBytes(buffer.Length));
            client.Send(buffer);
        }

        public static void Receive()
        {
            if (true)
            {
                while (true)
                {
                        ASCIIEncoding ascii = new ASCIIEncoding();
                        BinaryFormatter formatter = new BinaryFormatter();

                        // Reception
                        byte[] buffer = new byte[4];
                        client.Receive(buffer);
                        int IdDuJoueur = BitConverter.ToInt32(buffer, 0);

                        // Taille
                        buffer = new byte[4];
                        client.Receive(buffer);
                        int messageLength = BitConverter.ToInt32(buffer, 0);

                        //Données
                        buffer = new byte[messageLength];
                        client.Receive(buffer);

                        // Traitement
                        MemoryStream stream = new MemoryStream(buffer);
                        stream.Position = 0;

                        Player player = (Player)formatter.Deserialize(stream);
                        if (Outil.GetJoueur(IdDuJoueur) != null)
                        {
                            //Outil.GetJoueur(IdDuJoueur).champion.body.Position = new Vector2(player.positionX, player.positionY);
                            Outil.GetJoueur(IdDuJoueur).champion.ObjectifListe = player.noeuds;
                        }
                }
            }
        }
    }
}
