using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using CrystalGate.SceneEngine2;

namespace CrystalGate
{
        [Serializable]
    public class Unite : Objet
    {
        public int Vie { get { return vie + VieBonus; } set { vie = value - VieBonus; } }
        public int VieMax { get { return vieMax + VieMaxBonus; } set { vieMax = value - VieMaxBonus; } }
        public int Mana { get { return mana + ManaBonus; } set { mana = value - ManaBonus; } }
        public int ManaMax { get { return manaMax + ManaMaxBonus; } set { manaMax = value - ManaMaxBonus; } }
        public int ManaRegen { get { return manaRegen + ManaRegenBonus; } set { manaRegen = value - ManaRegenBonus; } }  // Temps de régération du mana en ms
        public int XPUnite; // Expérience que donne l'unité quand on la tue
        public int XP; // Expérience que possède l'unité
        public int Level;
        public float Vitesse { get { return vitesse + VitesseBonus; } set { vitesse = value - VitesseBonus; } }
        public float Portee { get { return portee + PorteeBonus; } set { portee = value - PorteeBonus; } }
        public int Dommages { get { return dommages + DommagesBonus; } set { dommages = value - DommagesBonus; } }
        public int Puissance { get { return puissance + PuissanceBonus; } set { puissance = value - PuissanceBonus; } }
        public float Vitesse_Attaque { get { return vitesse_Attaque + Vitesse_AttaqueBonus; } set { vitesse_Attaque = value - Vitesse_AttaqueBonus; } }
        public int Defense { get { return defense + DefenseBonus; } set { defense = value - DefenseBonus; } }
        public int DefenseMagique { get { return defenseMagique + DefenseMagiqueBonus; } set { defenseMagique = value - DefenseMagiqueBonus; } } 

        // Stats private
        int vie;
        int vieMax;
        int mana;
        int manaMax;
        int manaRegen; // Temps de régération du mana en ms
        float vitesse;
        float portee;
        int dommages;
        int puissance;
        float vitesse_Attaque;
        int defense;
        int defenseMagique;

        // Stats Bonus
        public int VieBonus;
        public int VieMaxBonus;
        public int ManaBonus;
        public int ManaMaxBonus;
        public int ManaRegenBonus; // Temps de régération du mana en ms
        public float VitesseBonus;
        public float PorteeBonus;
        public int DommagesBonus;
        public int PuissanceBonus;
        public float Vitesse_AttaqueBonus;
        public int DefenseBonus;
        public int DefenseMagiqueBonus;

        public Color color;
        public Vector2 pointCible;
        public bool IsRanged;
        int NiveauDeBrain;

        protected EffetSonore effetUniteAttaque;
        public EffetSonore effetUniteDeath;
        public EffetSonore effetUniteLevelUp;

        protected int nbFrameSonJoue;
        protected double lastManaAdd;

        public List<Spell> spells; // Les sorts disponibles pour cette unités
        public List<Spell> spellsUpdate; // Les sorts qui doivent etre update
        public List<Item> Inventory;
        public List<Item> Stuff;
        public int InventoryCapacity = 64;
        public bool Drawlife;
        public double idWave;
        public bool isCasting; // dit si un sort est lancé, si c'est un champion uniquement

        public float byLevelAdd = 1.05f;

        public static Random rand;

        // Variables pour le reseau, n'affecte pas le local
        public byte idUniteAttacked = 0;

        public Unite(Vector2 Position, int Level = 1)
            : base(Position)
        {
            // Constructeur par defaut d'une unité
            Vie = VieMax = 1;
            Mana = ManaMax = 200;
            ManaRegen = 500;
            Vitesse = 0.1f;
            Portee = 1; // 1 = Corps à corps
            Dommages = 1;
            Puissance = 1;
            effetUniteAttaque = new EffetSonore(PackSon.Epee);
            effetUniteDeath = new EffetSonore(PackSon.GruntDeath);
            effetUniteLevelUp = new EffetSonore(PackSon.LevelUp);
            nbFrameSonJoue = 0;
            Vitesse_Attaque = 1.00f;
            Defense = 0;
            XPUnite = 0;
            this.Level = Level;
            // Graphique par defaut
            Sprite = PackTexture.blank;
            Tiles = Vector2.One;
            color = Color.White; 
            
            idWave = -1;
            spells = new List<Spell> {  new Soin(this)};
            spellsUpdate = new List<Spell> { };
            Inventory = new List<Item> { };
            Stuff = new List<Item> { };

            //Dialogue.Add("Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo.");
        }

        public void statsLevelUpdate()
        {
            defense = (int)(defense * Math.Pow(byLevelAdd, Level - 1));
            defenseMagique = (int)(defenseMagique * Math.Pow(byLevelAdd, Level - 1));
            dommages = (int)(dommages * Math.Pow(byLevelAdd, Level - 1));
            puissance = (int)(puissance * Math.Pow(byLevelAdd, Level - 1));
            vitesse = (float)(vitesse * Math.Pow(byLevelAdd, Level - 1));
            manaMax = (int)(manaMax * Math.Pow(byLevelAdd, Level - 1));
            mana = manaMax;
            vieMax = (int)(vieMax * Math.Pow(byLevelAdd, Level - 1));
            vie = vieMax;
            manaRegen = (int)(manaRegen / Math.Pow(byLevelAdd, Level - 1));
            XPUnite = (int)(XPUnite / Math.Pow(byLevelAdd, Level - 1));
        }

        public override void Update(List<Unite> unitsOnMap, List<Effet> effets)
        {
            Animer(); 
            Deplacer();
            TestMort(effets);
            Debug();
            // Update l'IA
            if(!isAChamp)
                IA(unitsOnMap);

            // Pour Update et Draw les sorts
            try
            {
                foreach (Spell s in spellsUpdate)
                    s.Update();
            }
            catch
            {

            }
            // Retire les sorts qui sont finis
            for (int i = 0; i < spellsUpdate.Count; i++)
                if (!spellsUpdate[i].Activated)
                    spellsUpdate.RemoveAt(i);
            // Pour Update et Draw les items de l'inventaire
            foreach (Item i in Inventory)
                if (i.Activated)
                    i.spell.Update();
            // Pour Update les projectiles
            ProjectileUpdate();
            // On ajoute du mana
            ManaUpdate();
            // On Update l'inventaire de l'unité
            InventoryUpdate();
            // On rafraichit la propriete suivante, elle est juste indicative et n'affecte pas le draw, mais le pathfinding
            PositionTile = new Vector2((int)(ConvertUnits.ToDisplayUnits(body.Position.X) / Map.TailleTiles.X), (int)(ConvertUnits.ToDisplayUnits(body.Position.Y) / Map.TailleTiles.Y));
            // Refresh l'attaque et le pathfinding correspondant
            if (uniteAttacked != null)
                Attaquer(uniteAttacked);
            else
                if (OlduniteAttacked != null && ObjectifListe.Count == 0 && IsRanged) // si l'unité se déplacait pour tirer
                    uniteAttacked = OlduniteAttacked;
            
            // Fait attaquer l'unité la plus proche
            if (!isAChamp && !isApnj)
            {
                float distanceInit = 9000;
                Unite focus = null;
                foreach (Unite u in Map.unites)
                {
                    float distance = 0;

                    if (u.isAChamp && (distance = Outil.DistanceUnites(this, u)) <= distanceInit)
                    {
                        distanceInit = distance;
                        focus = u;
                    }
                }
                uniteAttacked = focus;
            }

            // Reseau
            UpdateReseau();
        }

        void Debug()
        {
            if (Vie > VieMax)
                Vie = VieMax;
            if (Mana > ManaMax)
                Mana = ManaMax;
        }

        void UpdateReseau()
        {
            if (uniteAttacked != null)
                idUniteAttacked = uniteAttacked.id;
            else
                idUniteAttacked = 0;
        }

        protected virtual void IA(List<Unite> unitsOnMap)
        {
            // Cast un heal si < à la moitié de vie
            if (Vie <= VieMax / 2 && IsCastable(0))
                Cast( spells[0], Vector2.Zero, null);

            if (uniteAttacked != null)
            {
                if (IsRanged && NiveauDeBrain >= 3)
                {
                    NiveauDeBrain = 0;
                    // represente tous les noeuds possibles pour se rapprocher de la cible
                    List<Noeud> Chemin = PathFinding.TrouverChemin(PositionTile, uniteAttacked.PositionTile, Map.Taille, new List<Unite> { }, Map.unitesStatic, false);
                    Projectile = new Projectile(this, uniteAttacked);
                    if (Chemin != null)
                    {
                        foreach (Noeud n in Chemin) // Pour chaque noeud possible
                        {
                            if (Projectile.CanReach(n.Position)) // Si on peut atteindre sa cible depuis ce noeud
                            {
                                ObjectifListe = new List<Noeud> { };
                                foreach (Noeud n2 in Chemin) // Alors on se déplace sur ce noeud
                                {
                                    if (n2 != n)
                                        ObjectifListe.Add(n2);
                                    else
                                    {
                                        ObjectifListe.Add(n2);
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }   
                    OlduniteAttacked = uniteAttacked;
                    uniteAttacked = null;
                }
            }
        }

        public virtual void TestMort(List<Effet> effets) // aussi la mana
        {
            if (Vie <= 0 && !Mort)
            {
                Vie = 0;
                Mort = true;
                if (PackMap.joueurs[0].champion.XP + XPUnite < PackMap.joueurs[0].champion.Level * 1000)
                    PackMap.joueurs[0].champion.XP += XPUnite;
                else
                {
                    PackMap.joueurs[0].champion.XP = PackMap.joueurs[0].champion.XP + XPUnite - PackMap.joueurs[0].champion.Level * 1000;
                    PackMap.joueurs[0].champion.newLevel();
                }
                effetUniteDeath.Play();
                effetUniteAttaque.Dispose();
                effets.Add(new Effet(Sprite, ConvertUnits.ToDisplayUnits(body.Position), packAnimation.Mort(this), Tiles, 1));
                Map.world.RemoveBody(body);
                Drop();
                // Reseau
                for (int i = 0; i < Serveur.LastDead.Length; i++)
                {
                    if (Serveur.LastDead[i] == 0)
                    {
                        if (i == Serveur.LastDead.Length - 1)
                            for (int j = 0; j < Serveur.LastDead.Length; j++)
                                Serveur.LastDead[j] = 0;
                        Serveur.LastDead[i] = id;
                        break;
                    }
                }
                // Interface De GameOver pour le joueur
                if (isAChamp) // Si un champion se fait tuer
                    foreach (Joueur j in PackMap.joueurs) // On regarde si c'est le joueur local
                            j.Interface.Lost = true;
            }
            // TEST MANA
            if (Mana < 0)
                Mana = 0;
        }

        public void newLevel()
        {
            effetUniteLevelUp.Play();
            Level++;
            if ((int)(defense * byLevelAdd) == defense)
                defense++;
            else
                defense = (int)(defense * byLevelAdd);
            if ((int)(defenseMagique * byLevelAdd) == defenseMagique)
                defenseMagique++;
            else
                defenseMagique = (int)(defenseMagique * byLevelAdd);
            dommages = (int)(dommages * byLevelAdd);
            puissance = (int)(puissance * byLevelAdd);
            vitesse = vitesse * byLevelAdd;
            manaMax = (int)(manaMax * byLevelAdd);
            vieMax = (int)(vieMax * byLevelAdd);
            manaRegen = (int)(manaRegen / byLevelAdd);
        }

        public bool IsCastable(int idSort)
        {
            return Map.gametime.TotalGameTime.TotalMilliseconds - spells[idSort].LastCast > spells[idSort].Cooldown * 1000 && Mana >= spells[idSort].CoutMana && Mana > 0;
        } // Indique si un sort est castable

        void ProjectileUpdate()
        {
            if (Projectile != null)
            {
                // On verifie si le projectile n'est pas dans un mur, on augmente la variable si c'est le cas
                if (Projectile.IsInWall())
                {
                    Projectile = null;
                    NiveauDeBrain++;
                }
                else
                {
                    Projectile.Update();
                    if (Projectile.Timer <= 0) // quand le projectile atteint sa cible
                    {
                        Projectile = null;
                        if (uniteAttacked != null)// Si la cible n'est pas morte entre temps
                            if (Dommages - uniteAttacked.Defense <= 0)
                                uniteAttacked.Vie -= 1;
                            else
                                uniteAttacked.Vie -= Dommages - uniteAttacked.Defense;
                    }
                }
            }
        }

        void InventoryUpdate()
        {
            for (int i = 0; i < Inventory.Count; i++)
                if (Inventory[i].Activated && !Inventory[i].spell.Activated)
                    Inventory.Remove(Inventory[i]);
        }
       
        void ManaUpdate()
        {
            if (Map.gametime.TotalGameTime.TotalMilliseconds - lastManaAdd >= ManaRegen && Mana + 1 <= ManaMax)
            {
                lastManaAdd = Map.gametime.TotalGameTime.TotalMilliseconds;
                Mana++;
            }
        }

        public virtual void Attaquer(Unite unite)
        {
            float calcul = Outil.DistanceUnites(this, unite) - (26 * (unite.largeurPhysique + this.largeurPhysique) / 2);
            if (calcul >= Portee * Map.TailleTiles.X)
                Suivre(unite);
            else
            {
                if (CanAttack)
                {
                    if (!uniteAttacked.isInvisible)
                    {
                        OlduniteAttacked = uniteAttacked;
                        ObjectifListe.Clear();
                        body.LinearVelocity = Vector2.Zero;
                        // Fait regarder l'unité vers l'unité attaqué
                        if (Animation.Count == 0)
                        {
                            FlipH = false;
                            float angle = Outil.AngleUnites(this, unite);

                            if (angle >= Math.PI / 4 && angle <= 3 * Math.PI / 4)
                                direction = Direction.Haut;
                            else if (angle >= -3 * Math.PI / 4 && angle <= -Math.PI / 4)
                                direction = Direction.Bas;
                            else if (angle >= -Math.PI / 4 && angle <= Math.PI / 4)
                            {
                                direction = Direction.Gauche;
                                FlipH = true;
                            }
                            else
                                direction = Direction.Droite;

                        }

                        if (Map.gametime.TotalGameTime.TotalMilliseconds - LastAttack > Vitesse_Attaque * 1000) // Si le cooldown est fini
                        {
                            LastAttack = (float)Map.gametime.TotalGameTime.TotalMilliseconds; // On met à jour "l'heure de la dernière attaque"
                            // projectile
                            if (IsRanged) // Si l'unité attaque à distance, on creer un projectile, sinon on attaque direct
                                Projectile = new Projectile(this, uniteAttacked);
                            else
                            {
                                if (Dommages - unite.Defense <= 0) // Si armure > Dommages , degats = 1
                                {
                                    // On ne prend en compte les degats que si on est en local autrement ils sont effectués via le serveur

                                    unite.Vie -= 1;

                                }
                                else
                                {
                                    unite.Vie -= Dommages - unite.Defense;

                                }
                            }
                            // son
                            effetUniteAttaque.Play();

                            // Fait regarder l'unité vers l'unité attaqué et l'anime

                            AnimationCurrent = AnimationLimite;
                            FlipH = false;
                            float angle = Outil.AngleUnites(this, unite);

                            if (angle >= Math.PI / 4 && angle <= 3 * Math.PI / 4)
                            {
                                direction = Direction.Haut;
                                Animation = packAnimation.AttaquerHaut();
                            }
                            else if (angle >= -3 * Math.PI / 4 && angle <= -Math.PI / 4)
                            {
                                direction = Direction.Bas;
                                Animation = packAnimation.AttaquerBas();
                            }
                            else if (angle >= -Math.PI / 4 && angle <= Math.PI / 4)
                            {
                                direction = Direction.Gauche;
                                FlipH = true;
                                Animation = packAnimation.AttaquerDroite();
                            }
                            else
                            {
                                direction = Direction.Droite;
                                Animation = packAnimation.AttaquerDroite();
                            }

                        }
                    }
                }
            }
        }

        public virtual void Deplacer()
        {
            if (ObjectifListe.Count > 0)
            {  // Bug, je sais pas pourquoi
                // body.Position = ConvertUnits.ToSimUnits(new Vector2((float)Math.Round(ConvertUnits.ToDisplayUnits(body.Position.X)), (float)Math.Round(ConvertUnits.ToDisplayUnits(body.Position.Y) )));
                // HAUT GAUCHE
                if (PositionTile.X > ObjectifListe[0].Position.X && PositionTile.Y > ObjectifListe[0].Position.Y)
                {
                    body.LinearVelocity = new Vector2(-Vitesse / 1.41f, -Vitesse / 1.41f) * Map.gametime.ElapsedGameTime.Milliseconds;
                    FlipH = true;

                    if (direction != Direction.HautGauche || Animation.Count == 0)
                        Animation = packAnimation.HautDroite();
                    direction = Direction.HautGauche;
                }
                // HAUT DROITE
                else if (PositionTile.X < ObjectifListe[0].Position.X && PositionTile.Y > ObjectifListe[0].Position.Y)
                {
                    body.LinearVelocity = new Vector2(Vitesse / 1.41f, -Vitesse / 1.41f) * Map.gametime.ElapsedGameTime.Milliseconds;
                    FlipH = false;

                    if (direction != Direction.HautDroite || Animation.Count == 0)
                        Animation = packAnimation.HautDroite();
                    direction = Direction.HautDroite;
                }
                // BAS DROITE
                else if (PositionTile.X < ObjectifListe[0].Position.X && PositionTile.Y < ObjectifListe[0].Position.Y)
                {
                    body.LinearVelocity = new Vector2(Vitesse / 1.41f, Vitesse / 1.41f) * Map.gametime.ElapsedGameTime.Milliseconds;
                    FlipH = false;

                    if (direction != Direction.BasDroite || Animation.Count == 0)
                        Animation = packAnimation.BasDroite();
                    direction = Direction.BasDroite;
                }
                // BAS GAUCHE
                else if (PositionTile.X > ObjectifListe[0].Position.X && PositionTile.Y < ObjectifListe[0].Position.Y)
                {
                    body.LinearVelocity = new Vector2(-Vitesse / 1.41f, Vitesse / 1.41f) * Map.gametime.ElapsedGameTime.Milliseconds;
                    FlipH = true;

                    if (direction != Direction.BasGauche || Animation.Count == 0)
                        Animation = packAnimation.BasDroite();
                    direction = Direction.BasGauche;
                }
                // GAUCHE
                else if (PositionTile.X > ObjectifListe[0].Position.X)
                {
                    body.LinearVelocity = new Vector2(-Vitesse, 0) * Map.gametime.ElapsedGameTime.Milliseconds;
                    FlipH = true;

                    if (direction != Direction.Gauche || Animation.Count == 0)
                        Animation = packAnimation.Droite();
                    direction = Direction.Gauche;
                }
                // DROITE
                else if (PositionTile.X < ObjectifListe[0].Position.X)
                {
                    body.LinearVelocity = new Vector2(Vitesse, 0) * Map.gametime.ElapsedGameTime.Milliseconds;
                    FlipH = false;

                    if (direction != Direction.Droite || Animation.Count == 0)
                        Animation = packAnimation.Droite();
                    direction = Direction.Droite;
                }
                // HAUT
                else if (PositionTile.Y > ObjectifListe[0].Position.Y)
                {
                    body.LinearVelocity = new Vector2(0, -Vitesse) * Map.gametime.ElapsedGameTime.Milliseconds;
                    FlipH = false;

                    if (direction != Direction.Haut || Animation.Count == 0)
                        Animation = packAnimation.Haut();
                    direction = Direction.Haut;
                }
                // BAS
                else if (PositionTile.Y < ObjectifListe[0].Position.Y)
                {
                    body.LinearVelocity = new Vector2(0, Vitesse) * Map.gametime.ElapsedGameTime.Milliseconds;
                    FlipH = false;

                    if (direction != Direction.Bas || Animation.Count == 0)
                        Animation = packAnimation.Bas();
                    direction = Direction.Bas;
                }
                else
                    ObjectifListe.RemoveAt(0);

            }
            else
                body.LinearVelocity *= 0.01f;
               
        }

        public void Suivre(Unite unite)
        {
            List<Unite> liste = new List<Unite> { };
            double distance = Outil.DistanceUnites(this, unite);
            bool ok = distance > Portee * Map.TailleTiles.X;
            if (ok)
            {
                foreach (Unite u in Map.unites)
                    if (Outil.DistanceUnites(this, u) <= 1 * Map.TailleTiles.X)
                    {
                        if (u != unite && u != this)
                            liste.Add(u);
                    }

                suivreactuel = 0;
                List<Noeud> chemin = PathFinding.TrouverChemin(PositionTile, unite.PositionTile, Map.Taille, liste, Map.unitesStatic, false);
                if (chemin != null)
                {
                    ObjectifListe = chemin;
                    ObjectifListe.RemoveAt(0);
                }
                else
                {
                    ObjectifListe.Clear();
                    body.LinearVelocity = Vector2.Zero;
                }
            }
            else
                suivreactuel++;           
        }

        public void Drop()
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (id % 2 == 0)
                {
                    Inventory[i].Position = PositionTile;
                    Map.items.Add(Inventory[i]);
                    Inventory.RemoveAt(i);
                }
            }
        }

        public void Cast(Spell s, Vector2 point, Unite unit, bool fromOnline = false)
        {
                // Cast ou initialise le sort
                s.Point = point;
                s.UniteCible = unit;
            if(SceneHandler.gameplayScene.isCoopPlay && !fromOnline)
                isCasting = true;
            else
            {
                spellsUpdate.Add(s);
                spellsUpdate[spellsUpdate.Count - 1].Begin(point, unit);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, ConvertUnits.ToDisplayUnits(body.Position), SpritePosition, color, 0f, new Vector2(Tiles.X / 2, Tiles.Y / 2), Scale, FlipH ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            DrawVie(spriteBatch);
            // Draw les sorts
            try
            {
                foreach (Spell s in spellsUpdate)
                    s.Draw(spriteBatch);
            }
            catch
            {

            }
            foreach (Item i in Inventory)
                if (i.Activated)
                    i.spell.Draw(spriteBatch);
            // Draw projectile
            if(Projectile != null)
                Projectile.Draw(spriteBatch);
            // Pour voir le pathfinding
            /*foreach (Noeud n in ObjectifListe)
                spriteBatch.Draw(PackTexture.blank, new Rectangle((int)n.Position.X * 32, (int)n.Position.Y * 32, 32, 32), Color.White);
        */}

        void DrawVie(SpriteBatch spriteBatch)
        {
            if (Drawlife)
            {
                int largeur = 10;
                int longueur = (int)((float)Vie / (float)VieMax * 50);
                spriteBatch.Draw(PackTexture.blank, ConvertUnits.ToDisplayUnits(body.Position) + new Vector2(0, -30), new Rectangle(0, 0, longueur, largeur), Color.Green, 0f, new Vector2(longueur / 2, largeur / 2), 1f, SpriteEffects.None, 0);

            }
        }

    }
}
