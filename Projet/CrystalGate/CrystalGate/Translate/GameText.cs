﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalGate
{
    class GameText
    {
        static string langue = "french";
        const string defaultLanguage = "default";

        static List<string> autorisedLanguages = new List<string> { "french", "english" };
        
        private static List<string> nomDuTexte, texteCorrespondant;
        static bool isLoaded = false;

        public static void initGameText() // lancer cette fonction au lancement du jeu et à chaque changement de langue.
        {
            nomDuTexte = new List<string>();
            texteCorrespondant = new List<string>();
            System.IO.StreamReader file;
            try
            {
                if (autorisedLanguages.Contains(langue))
                    file = new System.IO.StreamReader(@"../../../Languages/" + langue + ".lng"); // Adresse à changer plus tard
                else
                    file = new System.IO.StreamReader(@"../../../Languages/" + defaultLanguage + ".lng"); 
            }
            catch (Exception)
            {
                isLoaded = false;
                return;
            }

            string line;
            string[] lineSplit;
            while ((line = file.ReadLine()) != null)
            {
                lineSplit = line.Split(new char[] { '=' });
                nomDuTexte.Add(lineSplit[0]);
                texteCorrespondant.Add(lineSplit[1]);
            }
            isLoaded = true;
        }

        public static string getText(string textName)
        {
            if (isLoaded)
            {
                int max = nomDuTexte.Count; // trouver une méthode plus jolie

                for (int i = 1; i < max; i++)
                {
                    if (textName == nomDuTexte[i])
                        return texteCorrespondant[i];
                }
                return texteCorrespondant[0];
            }
            else
                return "Error";
        }
    }
}