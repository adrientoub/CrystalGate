using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using CrystalGate.SceneEngine2;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CrystalGate
{
    public class Client
    {
        static Socket client;
        public static int id;
        public static bool isConnected { get { return client != null && client.Connected; } }
        public static bool Started;

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

        public static void Send(byte[] buffer, int type)
        {
            if (type == 0) //envoyer un perso
                client.Send(new byte[] { 0 });

            // Envoi puis de la taille, puis de l'objet
            client.Send(BitConverter.GetBytes(buffer.Length));
            client.Send(buffer);
        }

        public static void Receive()
        {
            byte[] buffer = new byte[1];
            client.Receive(buffer);
            if (BitConverter.ToBoolean(buffer, 0)) // le start
            {
                Started = true;
                while (true)
                {
                        ASCIIEncoding ascii = new ASCIIEncoding();
                        BinaryFormatter formatter = new BinaryFormatter();

                        // Reception
                        byte[] buffer1 = new byte[4];
                        client.Receive(buffer1);
                        int IdDuJoueur = BitConverter.ToInt32(buffer1, 0);

                        // Taille
                        byte[] buffer2 = new byte[4];
                        client.Receive(buffer2);
                        int messageLength = BitConverter.ToInt32(buffer2, 0);

                        //Données
                        byte[] buffer3 = new byte[messageLength];
                        client.Receive(buffer3);

                        // Traitement
                        MemoryStream stream = new MemoryStream(buffer3);
                        stream.Position = 0;

                        Player player = (Player)formatter.Deserialize(stream);
                        if (Outil.GetJoueur(IdDuJoueur) != null)
                        {
                            Joueur joueur = Outil.GetJoueur(IdDuJoueur);
                            // Pathfinding
                            if (player.objectifPoint != null)
                            {
                                List<Noeud> path = PathFinding.TrouverChemin(joueur.champion.PositionTile, player.objectifPoint.Position, Map.Taille, Map.unites, Map.unitesStatic, true);
                                if (path != null)
                                    joueur.champion.ObjectifListe = path;
                            }
                            //Stats
                            if (player.idUniteAttacked != 0)
                            {
                                foreach (Unite u in Map.unites)
                                    if (u.id == player.idUniteAttacked)
                                        joueur.champion.uniteAttacked = u;
                            }
                            else
                                joueur.champion.uniteAttacked = null;
                        }
                }
            }
        }
    }
}
