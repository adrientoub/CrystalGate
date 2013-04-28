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
        static TimeSpan _finDeLaMusique = new TimeSpan();
        static string[] _musicList = new string[] {
            "Musiques/CelticMusic-FortheKing",
            "Musiques/CelticMusic-Ode to the Fallen",
            "Musiques/CelticBallad-Riversong",
            "Musiques/CelticMusic-Prophecy",
            "Musiques/HOM4"
        };
        static Song[] _musiqueDeFond = new Song[_musicList.Length];
        static int _playingNow = 0;
        public static float volume = 0.1f;
        static bool isLoaded = false;


        public static void Play()
        {
            if (isLoaded)
            {
                MediaPlayer.Volume = volume; // Mets le volume à fond (nécessaire pour éviter un bug)
                MediaPlayer.Play(_musiqueDeFond[_playingNow]);
                _finDeLaMusique = _musiqueDeFond[_playingNow].Duration;
            }
        }
        public static void PlayNext()
        {
            if (isLoaded)
            {
                _playingNow++;
                _playingNow = _playingNow % _musicList.Length;
                MediaPlayer.Play(_musiqueDeFond[_playingNow]);
                _finDeLaMusique = _musiqueDeFond[_playingNow].Duration + EffetSonore.time.Elapsed;
            }
        }
        public static void UpdateVolume()
        {
            MediaPlayer.Volume = volume;
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
        public static void Update()
        {
            if (EffetSonore.time.Elapsed >= _finDeLaMusique)
                PlayNext();
        }

        public static void Load(ContentManager content)
        {
            for (int i = 0; i < _musicList.Length; i++)
            {
                _musiqueDeFond[i] = content.Load<Song>(_musicList[i]);
            }
            MediaPlayer.IsRepeating = false;
            isLoaded = true;
        }
    }
}
