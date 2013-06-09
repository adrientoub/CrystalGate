using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace CrystalGate.Reseau
{
    class Players
    {
        // Classe qui permet d'instancier la liste des joueurs connectés
        public string name;
        public int id;
        public IPAddress ip;

        public static int idAct = 0;

        public Players(string name)
        {
            this.name = name;
            this.id = idAct;
            idAct++;
        }
        public Players()
        {
            name = "";
            this.id = idAct;
            idAct++;
        }
    }
}
