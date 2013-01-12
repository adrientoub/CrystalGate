using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CrystalGate
{
    public class Batiment : Objet
    {
        public int Vie { get; set; }
        public int VieMax { get; set; }
        public float Vitesse { get; set; }
        public float Portee { get; set; }
        public int Dommages { get; set; }
        public float Vitesse_Attaque { get; set; }
        public int Defense { get; set; }

        protected EffetSonore effetUnite;
        protected int nbFrameSonJoue;

        public List<Spell> spells { get; set; }
        public bool Drawlife { get; set; }

        public Batiment(Vector2 Position, Map map, SpriteBatch spriteBatch, PackTexture packTexture)
            : base(Position, map, spriteBatch, packTexture)
        {
            // Constructeur par defaut d'un batiment
            Vie = VieMax = 1;
            //effetUnite = new EffetSonore(0);
            nbFrameSonJoue = 0;
            body.IsStatic = true;
            // Graphique par défaut
            Sprite = packTexture.map[0];
            Tiles = new Vector2(32,32);
        }

        public override void Update(List<Objet> unitsOnMap, List<Effet> effets)
        {
            Animer();
            // On rafraichit la propriete suivante, elle est juste indicative et n'affecte pas le draw, mais le pathfinding
            PositionTile = new Vector2((int)(ConvertUnits.ToDisplayUnits(body.Position.X) / 32), (int)(ConvertUnits.ToDisplayUnits(body.Position.Y) / 32));
        }

        public override void Draw()
        {
            spriteBatch.Draw(Sprite, ConvertUnits.ToDisplayUnits(body.Position), new Rectangle(32 * 16 + 16 ,0,32,32), Color.White, 0f, new Vector2(Tiles.X / 2, Tiles.Y / 2), 1f, FlipH ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }

    }
}
