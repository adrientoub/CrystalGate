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
        public EffetSonore(int i)
        {
            try
            {
                son = CrystalGate.Scenes.GameplayScene._sons[i].CreateInstance();
            }
            catch (Exception)
            {
                throw new Exception("Impossible de charger le son demandé");
            }
        }
        public void Play()
        {
            try
            {
                son.Play();
            }
            catch (NoAudioHardwareException) // On ne lit pas l'audio si il n'y a pas de HP/casque (ça permet d'éviter un crash)
            {
            }
        }
        public void Dispose()
        {
            son.Dispose();
        }
    }
}
