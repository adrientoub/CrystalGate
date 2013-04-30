using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CrystalGate.Reseau
{
    class Reseau
    {
        static List<ArraySegment<byte>> buffer = new List<ArraySegment<byte>>();
        static int tailleDeLaString = 0;
        public static List<Message> discution = new List<Message>();

        public static void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                Socket soc = (Socket)result.AsyncState;
                soc.EndReceive(result);
                // Traitement : 
                if (buffer[0].Array[0] == 1) // Si on reçoit une string
                {
                    buffer.Clear();
                    buffer.Add(new ArraySegment<byte>(new byte[4]));
                    soc.BeginReceive(buffer, SocketFlags.None, receiveStringLengthCallback, soc);
                    tailleDeLaString = 0;
                }
                else // On reçoit autre chose :)
                {

                }
            }
            catch (Exception)
            {
                discution.Add(new Message(EffetSonore.time.Elapsed, "Connexion perdue"));
                SceneEngine2.CoopConnexionScene.isOnlinePlay = false;
                
                // On essaie de se reconnecter
            }
        }

        public static void ReceiveStringLengthCallback(IAsyncResult result)
        {
            Socket soc = (Socket)result.AsyncState;
            soc.EndReceive(result);
            // Traitement : 
            tailleDeLaString = BitConverter.ToInt32(buffer[0].Array, 0);

            buffer.Clear();
            buffer.Add(new ArraySegment<byte>(new byte[tailleDeLaString]));
            soc.BeginReceive(buffer, SocketFlags.None, receiveStringCallback, soc);
            tailleDeLaString = 0;
        }

        public static void ReceiveStringCallback(IAsyncResult result)
        {
            Socket soc = (Socket)result.AsyncState;
            soc.EndReceive(result);
            // Traitement :
            discution.Add(new Message(EffetSonore.time.Elapsed ,Encoding.UTF8.GetString(buffer[0].Array)));

            buffer.Clear();
            buffer.Add(new ArraySegment<byte>(new byte[4]));
            soc.BeginReceive(buffer, SocketFlags.None, receiveCallback, soc);
        }

        static AsyncCallback receiveCallback = new AsyncCallback(ReceiveCallback);
        static AsyncCallback receiveStringLengthCallback = new AsyncCallback(ReceiveStringLengthCallback);
        static AsyncCallback receiveStringCallback = new AsyncCallback(ReceiveStringCallback);

        public static void SendData(object envoi, int type)
        {
            Socket soc;
            if (SceneEngine2.SceneHandler.coopSettingsScene.isServer)
            {
                soc = SceneEngine2.SceneHandler.coopConnexionScene.clientSoc;
            }
            else
            {
                soc = SceneEngine2.SceneHandler.coopConnexionScene.soc;
            }
            if (type == 1)
            {
                string texte = (string)envoi;
                discution.Add(new Message(EffetSonore.time.Elapsed, texte));

                byte[] sendingString = new byte[] { 1 };
                soc.Send(sendingString);

                byte[] messageLength = BitConverter.GetBytes(texte.Length);
                soc.Send(messageLength);

                byte[] messageData = System.Text.Encoding.UTF8.GetBytes(texte);
                soc.Send(messageData);
            }
        }

        public static void ReceiveData()
        {
            Socket soc;
            if (SceneEngine2.SceneHandler.coopSettingsScene.isServer)
            {
                soc = SceneEngine2.SceneHandler.coopConnexionScene.clientSoc;
            }
            else
            {
                soc = SceneEngine2.SceneHandler.coopConnexionScene.soc;
            }
            buffer.Add(new ArraySegment<byte>(new byte[1]));
            soc.BeginReceive(buffer, SocketFlags.None, receiveCallback, soc);
        }
    }
}
