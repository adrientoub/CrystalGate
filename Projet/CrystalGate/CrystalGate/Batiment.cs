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
        protected Vector2 PositionSprite { get; set; }

        public List<Spell> spells { get; set; }
        public bool Drawlife { get; set; }

        public Batiment(Vector2 Position, Map map, PackTexture packTexture)
            : base(Position, map, packTexture)
        {
            // Constructeur par defaut d'un batiment
            Vie = VieMax = 1;
            //effetUnite = new EffetSonore(0);
            nbFrameSonJoue = 0;
            body.IsStatic = true;
            // Graphique par défaut
            Sprite = packTexture.map[0];
            Tiles = new Vector2(32,32);
            PositionSprite = new Vector2(16,0);
        }

        public override void Update(List<Objet> unitsOnMap, List<Effet> effets)
        {
            Animer();
            // On rafraichit la propriete suivante, elle est juste indicative et n'affecte pas le draw, mais le pathfinding
            PositionTile = new Vector2((int)(ConvertUnits.ToDisplayUnits(body.Position.X) / 32), (int)(ConvertUnits.ToDisplayUnits(body.Position.Y) / 32));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rec = new Rectangle((int)(Tiles.X * PositionSprite.X + PositionSprite.X), (int)(Tiles.Y * PositionSprite.Y + PositionSprite.Y), (int)Tiles.X, (int)Tiles.Y);
            spriteBatch.Draw(Sprite, ConvertUnits.ToDisplayUnits(body.Position), rec, Color.White, 0f, new Vector2(Tiles.X / 2, Tiles.Y / 2), 1f, FlipH ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }

    }
}
