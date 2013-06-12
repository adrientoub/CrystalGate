﻿using System;
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
        public static Players ownPlayer;
        public static List<Players> joueursConnectes = new List<Players>();

        public static void Connect()
        {
            try
            {
                // On creer le socket et on se connecte
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(new IPEndPoint(CoopSettingsScene.ip, 5035));

                // On attend notre identifiant sur le reseau
                byte[] buffer = new byte[1];
                client.Receive(buffer);
                id = buffer[0];

                ownPlayer = new Players("", id);

                //On lance la thread de reception
                Thread Reception = new Thread(Receive);
                Reception.Start();
            }
            catch
            {
                // Si on arrive pas à se connecter on réessaie mais de manière asynchrone
                Thread t = new Thread(Connect);
                t.Start();
            }
        }

        public static void Send(byte[] buffer, int type)
        {
            // On envoie le type
            client.Send(new byte[] { (byte)type });

            // Envoi puis de la taille, puis de l'objet
            client.Send(BitConverter.GetBytes(buffer.Length));
            client.Send(buffer);
        }

        public static void Receive()
        {
            Started = true;
            while (true)
            {
                // Réception du header
                byte[] buffer = new byte[4];
                client.Receive(buffer);
                int header = BitConverter.ToInt32(buffer, 0);

                if (header == 0)
                {
                    // Réception de l'ID du client
                    byte[] buffer1 = new byte[4];
                    client.Receive(buffer1);
                    int IdDuJoueur = BitConverter.ToInt32(buffer1, 0);

                    // De la taille
                    byte[] buffer2 = new byte[4];
                    client.Receive(buffer2);
                    int messageLength = BitConverter.ToInt32(buffer2, 0);

                    //Des données
                    byte[] buffer3 = new byte[messageLength];
                    client.Receive(buffer3);
                    // On deserialise et modifie le joueur
                    Unserialize(IdDuJoueur, buffer3);
                }
                else if (header == 1)
                {
                    // De la taille
                    byte[] buffer2 = new byte[4];
                    client.Receive(buffer2);
                    int messageLength = BitConverter.ToInt32(buffer2, 0);

                    //Des données
                    byte[] buffer3 = new byte[messageLength];
                    client.Receive(buffer3);

                    BinaryFormatter formatter = new BinaryFormatter();
                    MemoryStream stream = new MemoryStream(buffer3);
                    stream.Position = 0;

                    Players j = (Players)formatter.Deserialize(stream);
                    if (j.id - 1 == joueursConnectes.Count)
                    {
                        joueursConnectes.Add(j);
                    }
                    else 
                    {
                        joueursConnectes[j.id - 1] = j;
                    }
                }
            }
        }

        public static void Unserialize(int IdDuJoueur, byte[] buffer)
        {
            // Traitement
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(buffer);
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
