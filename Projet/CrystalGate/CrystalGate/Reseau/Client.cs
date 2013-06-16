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
        public static Socket client;
        public static int id;
        public static bool isConnected { get { return client != null && client.Connected; } }
        public static bool Started;
        public static Players ownPlayer;
        public static List<Players> joueursConnectes = new List<Players>();
        public static List<Message> discution = new List<Message>();

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
                SceneHandler.gameState = GameState.CoopConnexion;
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
            try
            {
                // On envoie le type
                client.Send(new byte[4] { (byte)type, 0, 0, 0 });

                // Envoi puis de la taille, puis de l'objet
                client.Send(BitConverter.GetBytes(buffer.Length));
                client.Send(buffer);
            }
            catch
            {
                UI.Error = true;
            }
        }

        public static void Receive()
        {
            try
            {
                Started = true;
                while (true)
                {
                    // Réception du header
                    byte[] buffer = new byte[4];
                    client.Receive(buffer);
                    int header = BitConverter.ToInt32(buffer, 0);

                    if (header == 42)
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
                    else if (header == 1) // On reçoit un joueur
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
                        if (j.id - 1 == joueursConnectes.Count || joueursConnectes.Count == 0)
                        {
                            joueursConnectes.Add(j);
                        }
                        else
                        {
                            joueursConnectes[j.id - 1] = j;
                        }

                    }
                    else if (header == 2) // On reçoit un message du chat
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

                        Message m = (Message)formatter.Deserialize(stream);
                        m.dateEnvoi = EffetSonore.time.Elapsed;
                        discution.Add(m);
                    }
                    else if (header == 3)
                    {
                        SceneHandler.ResetGameplay();
                        SceneHandler.gameState = GameState.Gameplay;
                        FondSonore.Play();
                        GamePlay.timer.Restart();
                    }
                    else
                    {

                    }

                }
        }
            catch
            {
                SceneHandler.coopConnexionScene.Error = true;
                SceneHandler.championSelectionScene.Error = true;
                // Le client s'est deco
                // Attention , risque de rentrer dans ce catch a l'entree du salon,
                // si c'est le cas, ca va planter!
            }
        }

        public static void Unserialize(int IdDuJoueur, byte[] buffer)
        {
            // Traitement
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(buffer);
            stream.Position = 0;
            // mettre un try catch
            Player player = (Player)formatter.Deserialize(stream);
            if (Outil.GetJoueur(IdDuJoueur) != null)
            {
                Joueur joueur = Outil.GetJoueur(IdDuJoueur);
                // Pathfinding
                if (player.Mooved)
                {
                    List<Noeud> path = PathFinding.TrouverChemin(joueur.champion.PositionTile, new Vector2(player.objectifPointX, player.objectifPointY), Map.Taille, Map.unites, Map.unitesStatic, true);
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
                if (player.idSortCast != 0)
                {
                    Unite u = joueur.champion;
                    List<Spell> toutLesSpellsPossibles = new List<Spell> { new Explosion(u), new Soin(u), new Invisibilite(u), new FurieSanguinaire(u), new Polymorphe(u), new Tempete(u) };
                    
                    foreach (Spell s in toutLesSpellsPossibles)
                        if (s.idSort == player.idSortCast)
                        {
                            s.Point = new Vector2(player.pointSortX, player.pointSortY);
                            if (player.idUniteCibleCast != 0)
                            {
                                foreach (Unite un in Map.unites)
                                    if (un.id == player.idUniteCibleCast)
                                        s.UniteCible = un;
                            }
                            joueur.champion.Cast(s, new Vector2(player.pointSortX, player.pointSortY), s.UniteCible);
                            break;
                        }

                }
                if(player.LastDeath != 0)
                    foreach (Unite u in Map.unites)
                        if (u.id == player.LastDeath)
                        {
                            u.Vie = 0;
                            break;
                        }
            }
        }
    }
}
