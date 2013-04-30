using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalGateEditor
{
    class GameText
    {
        public static string langue = "french";
        const string defaultLanguage = "default";

        static List<string> autorisedLanguages = new List<string> { "french", "english" };
        
        private static List<string> nomDuTexte, texteCorrespondant;
        static bool isLoaded = false;

        public static TimeSpan lastReinit;

        public static System.IO.StreamWriter logWriter;

        public static void initGameText() // lancer cette fonction au lancement du jeu et à chaque changement de langue.
        {
            string baseDirectory;
            if (Game1.isTest)
                baseDirectory = "../../../";
            else
                baseDirectory = "";

            nomDuTexte = new List<string>();
            texteCorrespondant = new List<string>();
            System.IO.StreamReader file;
            try
            {
                if (autorisedLanguages.Contains(langue))
                    file = new System.IO.StreamReader(baseDirectory + "Languages/" + langue + ".lng", System.Text.Encoding.GetEncoding("iso-8859-1")); 
                else
                    file = new System.IO.StreamReader(baseDirectory + "Languages/" + defaultLanguage + ".lng", System.Text.Encoding.GetEncoding("iso-8859-1")); 
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
            file.Close();
            lastReinit = Text.time.Elapsed;
        }

        public static string getText(string textName) // A améliorer en utilisant les algos du cours, recherche dans une liste triée.
        {
            if (isLoaded)
            {
                int max = nomDuTexte.Count; // trouver une méthode plus jolie

                for (int i = 1; i < max; i++)
                {
                    if (textName == nomDuTexte[i])
                        return texteCorrespondant[i];
                }
                logWriter = new System.IO.StreamWriter("logfile.log");
                logWriter.WriteLine("Texte \""+ textName + "\" non trouvé, devrait être ajouté au fichier de langue : " + langue);
                logWriter.Close();
                return texteCorrespondant[0];
            }
            else
                return "Error";
        }
    }
}
