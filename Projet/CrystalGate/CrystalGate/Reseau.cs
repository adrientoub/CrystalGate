using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace CrystalGate
{
    class Reseau
    {
        static List<ArraySegment<byte>> buffer = new List<ArraySegment<byte>>();
        static int tailleDeLaString = 0;
        

        public static void ReceiveCallback(IAsyncResult result)
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
            UI.messageRecu = Encoding.UTF8.GetString(buffer[0].Array);

            buffer.Clear();
            buffer.Add(new ArraySegment<byte>(new byte[4]));
            soc.BeginReceive(buffer, SocketFlags.None, receiveCallback, soc);
        }

        static AsyncCallback receiveCallback = new AsyncCallback(ReceiveCallback);
        static AsyncCallback receiveStringCallback = new AsyncCallback(ReceiveStringCallback);

        public static void SendData(string texte)
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
            byte[] messageLength = BitConverter.GetBytes(texte.Length);
            soc.Send(messageLength);

            byte[] messageData = System.Text.Encoding.UTF8.GetBytes(texte);
            soc.Send(messageData);
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
            buffer.Add(new ArraySegment<byte>(new byte[4]));
            soc.BeginReceive(buffer, SocketFlags.None, receiveCallback, soc);
        }
    }
}
