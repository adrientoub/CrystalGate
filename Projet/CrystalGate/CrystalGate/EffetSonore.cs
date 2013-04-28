using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace CrystalGate
{
    public class EffetSonore
    {
        const int nbSonSimult = 20; // Nombre de sons simultanés.
        // A changer si plus de sons voulus.
        SoundEffectInstance son;
        public static System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
        static bool[] isPlaying = new bool[nbSonSimult];
        static TimeSpan[] duree = new TimeSpan[nbSonSimult];
        int id;
        public static float volume = 0.1f;
        static bool hasHP;

        static public void InitEffects()
        {
            for (int i = 0; i < nbSonSimult; i++)
            {
                isPlaying[i] = false;
            }
            time.Start();
            hasHP = true;
        }

        public EffetSonore(int i)
        {
            try
            {
                son = SceneEngine2.GamePlay._effetsSonores[i].CreateInstance();
                id = i;
            }
            catch (Exception)
            {
                throw new Exception("Impossible de charger le son demandé");
            }
        }
        public void Play()
        {
            bool effectLaunch = false;
            if (hasHP)
            {
                for (int i = 0; i < nbSonSimult; i++)
                {
                    if (time.Elapsed >= duree[i])
                    {
                        isPlaying[i] = false;
                    }
                }

                for (int i = 0; i < nbSonSimult && !effectLaunch; i++)
                {
                    if (!isPlaying[i])
                    {
                        try
                        {
                            if (!son.IsDisposed)
                            {
                                son.Volume = (volume >= 0) ? volume : 0; 
                                son.Play();
                                duree[i] = time.Elapsed + SceneEngine2.GamePlay._effetsSonores[id].Duration;
                                isPlaying[i] = true;
                                effectLaunch = true;
                            }
                        }
                        catch (NoAudioHardwareException)
                        {
                            hasHP = false;
                        } // On ne lit pas l'audio si il n'y a pas de HP/casque (ça permet d'éviter un crash)
                    }
                }
            }
        }
        public void Dispose()
        {
            son.Dispose();
        }
    }
}
