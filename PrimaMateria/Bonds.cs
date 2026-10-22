using Quintessential;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PrimaMateria
{
    internal class Bonds
    {
        public static BondType Calcic, Ethereal;
        public static void AddBondTypes()
        {
            var CalcicTextures = new BondTexture
            {
                texture = AssetLoaderHelper.LoadTexture("textures/primamateria/bonds/calcic"),
                normalMap = AssetLoaderHelper.LoadTexture("textures/primamateria/bonds/calcic_normals"),
                bondAnim = API.LoadAnimation("textures/primamateria/bonds/bond_calcic.array", "bond", 10),
                glyphAnim = BondTextures.standard.glyphAnim,
                bondSound = Sounds.glyph_calcicbonder,
                textureOffset = new Vector2(1, 0),
            };
            Calcic = new(
              "primamateria:calcic",
              CalcicTextures,
              BondTypes.GetBondType("om:standard").unbondAnim,
              [],
              0,
              BondOverlapMode.OnlyListed
            );
            PrimaMateria.Instance.AddBondType(Calcic);

            var EtherealTextures = new BondTexture
            {
                texture = AssetLoaderHelper.LoadTexture("textures/primamateria/bonds/ethereal"),
                normalMap = AssetLoaderHelper.LoadTexture("textures/primamateria/bonds/ethereal_normals"),
                bondAnim = API.LoadAnimation("textures/primamateria/bonds/bond_ethereal.array", "bond", 10),
                glyphAnim = BondTextures.standard.glyphAnim,
                bondSound = Sounds.glyph_etherealbonder,
                textureOffset = new Vector2(1, 0),
            };
            Ethereal = new(
              "primamateria:ethereal",
              EtherealTextures,
              BondTypes.GetBondType("om:standard").unbondAnim,
              [],
              0,
              BondOverlapMode.OnlyListed
            );
            PrimaMateria.Instance.AddBondType(Ethereal);
        }
    }
}
