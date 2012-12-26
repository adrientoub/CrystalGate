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
        SoundEffectInstance son;
        static System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
        static bool isPlaying = false;
        static TimeSpan duree = new TimeSpan();
        int id;
        static bool hasHP = true;

        public EffetSonore(int i)
        {
            try
            {
                son = CrystalGate.Scenes.GameplayScene._effetsSonores[i].CreateInstance();
                id = i;
            }
            catch (Exception)
            {
                throw new Exception("Impossible de charger le son demandé");
            }
        }
        public void Play()
        {
            if (hasHP)
            {
                if (isPlaying)
                {
                    if (time.Elapsed >= duree)
                    {
                        time.Stop();
                        time.Reset();
                        isPlaying = false;
                    }
                }
                else
                {
                    try
                    {
                        if (!son.IsDisposed)
                        {
                            son.Play();
                            duree = CrystalGate.Scenes.GameplayScene._effetsSonores[id].Duration;
                            time.Start();
                            isPlaying = true;
                        }
                    }
                    catch (NoAudioHardwareException)
                    {
                        hasHP = false;
                    } // On ne lit pas l'audio si il n'y a pas de HP/casque (ça permet d'éviter un crash)
                }
            }
        }
        public void Dispose()
        {
            son.Dispose();
        }
    }
}
