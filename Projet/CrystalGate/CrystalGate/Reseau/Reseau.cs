using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace CrystalGate.Reseau
{
    class Reseau
    {
        static byte[] buffer = new byte[1];
        static int tailleObjetEnvoye = 0;
        public static List<Message> discution = new List<Message>();

        static List<byte[]> clientBuffers = new List<byte[]>();

        public static NetworkStream ownStream; // Le stream du TcpClient local

        #region receive
        public static void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                NetworkStream soc = (NetworkStream)result.AsyncState;
                soc.EndRead(result);
                // Traitement : 
                if (buffer[0] == 1) // Si on reçoit une string
                {
                    buffer = new byte[4];

                    soc.BeginRead(buffer, 0, 4, receiveStringCallback, soc);
                    tailleObjetEnvoye = 0;
                }
                else if (buffer[0] == 2)
                {
                    buffer = new byte[4];

                    soc.BeginRead(buffer, 0, 4, receivePlayersCallback, soc);
                    tailleObjetEnvoye = 0;
                }
            }
            catch (Exception)
            {
                discution.Add(new Message(EffetSonore.time.Elapsed, "Connexion perdue"));
                SceneEngine2.CoopConnexionScene.isOnlinePlay = false;
                
                // On essaie de se reconnecter
            }
        }

        #region broadcast
        public static void ReceiveAndBroadcastCallback(IAsyncResult result)
        {
            int id = (int)result.AsyncState;
            try
            {
                NetworkStream soc = Connexion.clientsSoc[id].GetStream();
                soc.EndRead(result);
                // Traitement : 
                clientBuffers[id] = new byte[4];

                soc.BeginRead(clientBuffers[id], 0, 4, ReceiveObjectLengthAndBroadcastCallback, id);
                tailleObjetEnvoye = 0;
                SendServData(clientBuffers[id]);
            }
            catch (Exception)
            {
                discution.Add(new Message(EffetSonore.time.Elapsed, "Connexion perdue avec " + Connexion.joueurs[id]));
                // Supprimer ce client de la liste et essayer de s'y reconnecter
            }
        }

        public static void ReceiveObjectLengthAndBroadcastCallback(IAsyncResult result)
        {
            int id = (int)result.AsyncState;
            NetworkStream soc = Connexion.clientsSoc[id].GetStream();
            soc.EndRead(result);

            SendServData(clientBuffers[id]);
            tailleObjetEnvoye = BitConverter.ToInt32(clientBuffers[id], 0);

            clientBuffers[id] = new byte[tailleObjetEnvoye];
            soc.BeginRead(clientBuffers[id], 0, tailleObjetEnvoye, receiveObjectAndBroadcastCallback, id);
            tailleObjetEnvoye = 0;
        }

        public static void ReceiveObjectAndBroadcastCallback(IAsyncResult result)
        {
            int id = (int)result.AsyncState;
            NetworkStream soc = Connexion.clientsSoc[id].GetStream();
            soc.EndRead(result);

            SendServData(clientBuffers[id]);

            clientBuffers[id] = new byte[1];
            soc.BeginRead(clientBuffers[id], 0, 1, receiveCallback, soc);
        }

        public static void BroadcastData()
        {
            for (int i = 0; i < Connexion.clientsSoc.Count; i++)
            {
                ReceiveDataFromClient(i);
            }
        }

        public static void ReceiveDataFromClient(int id)
        {
            NetworkStream soc;
            soc = Connexion.clientsSoc[id].GetStream();
            clientBuffers[id] = new byte[1];
            soc.BeginRead(clientBuffers[id], 0, 1, receiveAndBroadcastCallback, id); 
        }
        #endregion broadcast

        #region clientReceive
        public static void ReceiveStringLengthCallback(IAsyncResult result)
        {
            NetworkStream soc = (NetworkStream)result.AsyncState;
            soc.EndRead(result);
            // Traitement : 
            tailleObjetEnvoye = BitConverter.ToInt32(buffer, 0);

            buffer = new byte[tailleObjetEnvoye];
            soc.BeginRead(buffer, 0, tailleObjetEnvoye, receiveStringCallback, soc);
            tailleObjetEnvoye = 0;
        }

        public static void ReceiveStringCallback(IAsyncResult result)
        {
            NetworkStream soc = (NetworkStream)result.AsyncState;
            soc.EndRead(result);
            // Traitement :
            discution.Add(new Message(EffetSonore.time.Elapsed, Encoding.UTF8.GetString(buffer)));

            buffer = new byte[1];
            soc.BeginRead(buffer, 0, 1, receiveCallback, soc);
        }

        public static void ReceivePlayerLengthCallback(IAsyncResult result)
        {
            NetworkStream soc = (NetworkStream)result.AsyncState;
            soc.EndRead(result);
            // Traitement : 
            tailleObjetEnvoye = BitConverter.ToInt32(buffer, 0);

            buffer = new byte[tailleObjetEnvoye];
            soc.BeginRead(buffer, 0, tailleObjetEnvoye, receivePlayersCallback, soc);
            tailleObjetEnvoye = 0;
        }

        public static void ReceivePlayersCallback(IAsyncResult result)
        {
            NetworkStream soc = (NetworkStream)result.AsyncState;
            soc.EndRead(result);

            BinaryFormatter formatter = new BinaryFormatter();
            // On ecrit les octets recu dans un flux mémoire
            soc.Position = 0;
            // On Deserialise le flux
            Players joueur = (Players)formatter.Deserialize(soc);

            buffer = new byte[1];
            soc.BeginRead(buffer, 0, 1, receiveCallback, soc);
        }

        public static void ReceiveData()
        {
            buffer = new byte[1];
            ownStream.BeginRead(buffer, 0, 1, receiveCallback, ownStream);
        }
        #endregion clientReceive

        #region callback
        static AsyncCallback receiveCallback = new AsyncCallback(ReceiveCallback);

        static AsyncCallback receiveAndBroadcastCallback = new AsyncCallback(ReceiveAndBroadcastCallback);
        static AsyncCallback receiveObjectLengthAndBroadcastCallback = new AsyncCallback(ReceiveObjectLengthAndBroadcastCallback);
        static AsyncCallback receiveObjectAndBroadcastCallback = new AsyncCallback(ReceiveObjectAndBroadcastCallback);

        static AsyncCallback receiveStringLengthCallback = new AsyncCallback(ReceiveStringLengthCallback);
        static AsyncCallback receiveStringCallback = new AsyncCallback(ReceiveStringCallback);
        static AsyncCallback receivePlayersCallback = new AsyncCallback(ReceivePlayersCallback);
        static AsyncCallback receivePlayersLengthCallback = new AsyncCallback(receivePlayersLengthCallback);
        #endregion callback
        #endregion receive

        #region send
        /// <summary>
        /// On envoie une donnée via le réseau en TCP.
        /// Actuellement seul l'envoi d'un message ou d'un joueur est possible
        /// </summary>
        /// <param name="envoi">La donnée à envoyer</param>
        /// <param name="type">Le type de cette donnée</param>
        public static void SendData(object envoi, int type) // Envoie des données en tant que client.
        {
            TcpClient soc = Connexion.cliSoc;
            if (type == 1)
            {
                string texte = (string)envoi;
                discution.Add(new Message(EffetSonore.time.Elapsed, texte));

                byte[] sendingString = new byte[] { 1 };
                soc.Client.Send(sendingString);

                byte[] messageLength = BitConverter.GetBytes(texte.Length);
                soc.Client.Send(messageLength);

                byte[] messageData = System.Text.Encoding.UTF8.GetBytes(texte);
                soc.Client.Send(messageData);
            }
            else if (type == 2) // On envoie un joueur
            {
                byte[] sendingPlayer = new byte[] { 2 };
                soc.Client.Send(sendingPlayer);

                MemoryStream stream = new MemoryStream();
                BinaryFormatter formater = new BinaryFormatter();

                Players joueur = (Players)envoi;

                formater.Serialize(stream, joueur);
                stream.Position = 0;
                // On lit le stream dans un buffer et on envoie les octets
                byte[] serializedObject = new byte[stream.Length];
                stream.Read(serializedObject, 0, serializedObject.Length);
                
                byte[] objectLength = BitConverter.GetBytes(serializedObject.Length);
                soc.Client.Send(objectLength);

                soc.Client.Send(serializedObject);
            }
        }

        public static void SendServData(byte[] envoi) // Envoie une donnée
        {
            List<TcpClient> clientSoc = Connexion.clientsSoc;
            foreach (var client in clientSoc)
            {
                client.Client.Send(envoi);
            }
        }
        #endregion send
    }
}
