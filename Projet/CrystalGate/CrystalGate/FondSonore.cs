using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace CrystalGate
{
    class FondSonore
    {
        public static void Play() // Joue aléatoirement un son dans la bibliothèque de l'utilisateur
        {
            MediaPlayer.Volume = 100; // Mets le volume à fond (nécessaire pour éviter un bug)
            MediaLibrary sampleMediaLibrary = new MediaLibrary();
            Random rand = new Random();
            // generate a random valid index into Albums
            int i = rand.Next(0, sampleMediaLibrary.Albums.Count - 1);
            int j = rand.Next(0, sampleMediaLibrary.Albums[i].Songs.Count - 1);

            // play the first track from the album
            MediaPlayer.Play(sampleMediaLibrary.Albums[i].Songs[j]);
        }
        public static void Pause() // Pause pour quand dans les menus
        {
            MediaPlayer.Pause();
        }
        public static void Resume() // Reprise
        {
            MediaPlayer.Resume();
        }
        public static void Stop() // Arrête le massacre
        {
            MediaPlayer.Volume = 0; // Mets le volume à 0 avant d'arrêter la musique (pour éviter un bug)
            MediaPlayer.Stop();
        }
    }
}
