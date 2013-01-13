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
        //const string musicName = "GangnamStyle";
        static Song[] _musiqueDeFond = new Song[2];
        static bool _isLoaded = false;
        static bool _isLoadedNext = false;
        static TimeSpan _finDeLaMusique = new TimeSpan();
        static string[] _musicList = new string[] {
            "GangnamStyle",
            "AviciiLevels"
        };
        static int id = 0;
        static int idNext = 1;
        static ContentManager contentRef;
        static int _playingNow = 0; // prend les valeurs 1 et 0
        

        public static void Play() 
        {
            if (_isLoaded)
            {
                MediaPlayer.Volume = 100; // Mets le volume à fond (nécessaire pour éviter un bug)
                MediaPlayer.Play(_musiqueDeFond[_playingNow]);
                _finDeLaMusique = _musiqueDeFond[_playingNow].Duration;
            }
        }
        public static void PlayNext()
        {
            if (!_isLoadedNext)
                LoadNext();

            InvertPlayingNow();
            id = idNext;
            _isLoaded = _isLoadedNext;
            _isLoadedNext = false;
            Play();
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
            else if (EffetSonore.time.Elapsed >= _finDeLaMusique - TimeSpan.FromSeconds(10))
                LoadNext();
        }

        public static void Load(ref ContentManager content)
        {
            contentRef = content;
            if (id == _musicList.Length)
                id = 0;
            
            if (!_isLoaded)
            {
                _musiqueDeFond[_playingNow] = content.Load<Song>(_musicList[id]);
                _isLoaded = true;
            }
            MediaPlayer.IsRepeating = false;
        }

        public static void LoadNext()
        {
            if (id == _musicList.Length)
                idNext = 0;
            else
                idNext = id + 1;

            if (!_isLoadedNext)
            {
                _musiqueDeFond[InvertPlayingNow(_playingNow)] = contentRef.Load<Song>(_musicList[idNext]);
                _isLoadedNext = true;
            }
        }

        static int InvertPlayingNow(int _playingNow)
        {
            if (_playingNow == 0)
                return 1;
            else
                return 0;
        }
        static void InvertPlayingNow()
        {
            if (_playingNow == 0)
                _playingNow = 1;
            else
                _playingNow = 0;
        }
    }
}
