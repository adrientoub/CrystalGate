using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace CrystalGate.Reseau
{
    class Connexion
    {
        /* A faire : 
         * Ajouter l'envoi du joueur au serveur à chaque fois pour que le nom marqué sur le serv soit ok
         * Faire que le serv envoie à tous les clients leurs noms
         * Ajouter une liste de personnes connectés au serveur sur la page de connexion
         * Refaire la page de connexion
         * Rendre le réseau de nouveau fonctionnel
         */

        static bool isServer;

        public static TcpClient cliSoc;
        public static TcpListener serverSoc; // Socket nul sauf dans le cas où on est le serveur
        public static List<TcpClient> clientsSoc;
        public static List<Players> joueurs;
        
        public static IPAddress ip;

        public static Players selfPlayer;
        public static bool isConnected;

        public static void InitializeConnexion()
        {
            clientsSoc = new List<TcpClient>();
            joueurs = new List<Players>();
            isConnected = false;
            selfPlayer = new Players();
        }

        public static void ClientConnected(IAsyncResult result)
        {
            try
            {
                cliSoc.EndConnect(result);
                Reseau.ownStream = cliSoc.GetStream();
                if (joueurs.Count >= 2)
                    SceneEngine2.SceneHandler.coopConnexionScene.lancerJeuActive = true;
                // Ajouter le premier envoi au serveur c'est à dire celui du nom du joueur etc.
                isConnected = true;
            }
            catch (Exception)
            {
                AsyncCallback cc = new AsyncCallback(ClientConnected);
                cliSoc.BeginConnect(ip, 6060, cc, cliSoc);
            }
        }

        public static void ServerConnected(IAsyncResult result)
        {
            clientsSoc.Add(((TcpListener)result.AsyncState).EndAcceptTcpClient(result)); // On ajoute ce client à la liste
            if (clientsSoc.Count >= 2)
                SceneEngine2.SceneHandler.coopConnexionScene.lancerJeuActive = true;

            joueurs.Add(new Players()); // On l'ajoute à la liste des joueurs
            Reseau.ReceiveDataFromClient(joueurs[joueurs.Count - 1].id); // On commence à recevoir des données de ce client

            AsyncCallback sc = new AsyncCallback(ServerConnected);
            serverSoc.BeginAcceptTcpClient(sc, serverSoc);
        }

        public static void Connect()
        {
            isServer = SceneEngine2.SceneHandler.coopSettingsScene.isServer;
            cliSoc = new TcpClient();
            AsyncCallback cc = new AsyncCallback(ClientConnected);

            if (isServer) // Si on est le serveur
            {
                serverSoc = new TcpListener(IPAddress.Any, 6060);
                // serverSoc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                AsyncCallback sc = new AsyncCallback(ServerConnected);
                serverSoc.BeginAcceptTcpClient(sc, serverSoc); // On accepte la connexion
                cliSoc.BeginConnect(IPAddress.Parse("127.0.0.1"), 6060, cc, cliSoc);
                isConnected = true;
            }
            else // Sinon on ne fait que se connecter
            {
                cliSoc.BeginConnect(ip, 6060, cc, cliSoc);
            }
        }

        /// <summary>
        /// Envoi le joueur local
        /// </summary>
        public static void SendPlayer()
        {
            Reseau.SendData(selfPlayer, 2);
        }
    }
}
