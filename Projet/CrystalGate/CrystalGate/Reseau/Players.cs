using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace CrystalGate
{
    [Serializable]
    class Players
    {
        // Classe qui permet d'instancier la liste des joueurs connectés
        public string name;
        public int id;

        public Players(string name, int id)
        {
            this.name = name;
            this.id = id;
        }
        public Players()
        {
            name = "";
            this.id = 0;
        }
    }
}