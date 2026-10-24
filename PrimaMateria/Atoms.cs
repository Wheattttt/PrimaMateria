using Quintessential;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;

namespace PrimaMateria
{
    internal static class Atoms
    {
        public static AtomType Sulfur, Quicklime, Potash, Magnesia, Antimony, Phosphor, Opalescence, Primae;
        public static void AddAtomTypes()
        {
            // make quicksilver gray
            AtomTypes.quicksilver.matteTextures.diffuse = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/quicksilver_diffuse");
            AtomTypes.quicksilver.matteTextures.shade = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/quicksilver_shade");
            AtomTypes.quicksilver.symbol = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/quicksilver_symbol");
            Sulfur = new()
            {
                shadow = Assets.textures.atoms.shadow,
                symbol = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/sulfur_symbol"),
                matteTextures = new()
                {
                    diffuse = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/sulfur_diffuse"),
                    shade = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/sulfur_shade"),
                }
            };
            PrimaMateria.Instance.AddAtomType(Sulfur, "sulfur");

            Quicklime = new()
            {
                shadow = Assets.textures.atoms.shadow,
                symbol = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/quicklime_symbol"),
                matteTextures = new()
                {
                    diffuse = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/quicklime_diffuse"),
                    shade = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/quicklime_shade"),
                }
            };
            PrimaMateria.Instance.AddAtomType(Quicklime, "quicklime");

            Potash = new()
            {
                shadow = Assets.textures.atoms.shadow,
                symbol = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/potash_symbol"),
                matteTextures = new()
                {
                    diffuse = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/potash_diffuse"),
                    shade = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/potash_shade"),
                }
            };
            PrimaMateria.Instance.AddAtomType(Potash, "potash");

            Magnesia = new()
            {
                shadow = Assets.textures.atoms.shadow,
                symbol = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/magnesia_symbol"),
                matteTextures = new()
                {
                    diffuse = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/magnesia_diffuse"),
                    shade = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/magnesia_shade"),
                }
            };
            PrimaMateria.Instance.AddAtomType(Magnesia, "magnesia");

            Antimony = new()
            {
                shadow = Assets.textures.atoms.shadow,
                symbol = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/antimony_symbol"),
                matteTextures = new()
                {
                    diffuse = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/antimony_diffuse"),
                    shade = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/antimony_shade"),
                }
            };
            PrimaMateria.Instance.AddAtomType(Antimony, "antimony");

            Phosphor = new()
            {
                shadow = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/phosphorus_shadow"),
                symbol = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/phosphorus_symbol"),
                matteTextures = new()
                {
                    diffuse = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/phosphorus_diffuse"),
                    shade = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/phosphorus_shade"),
                }
            };
            PrimaMateria.Instance.AddAtomType(Phosphor, "phosphor");

            Opalescence = new()
            {
                shadow = Assets.textures.atoms.shadow,
                symbol = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/opalescence_symbol"),
                matteTextures = new()
                {
                    diffuse = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/opalescence_diffuse"),
                    shade = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/opalescence_shade"),
                }
            };
            PrimaMateria.Instance.AddAtomType(Opalescence, "opalescence");

            Primae = new()
            {
                shadow = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/primae/primae_shadow"),
                symbol = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/primae/primae_symbol"),
                isPrismatic = true,
                prismaticTextures = new()
                {
                    base1 = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/primae/primae_base"),
                    colors = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/primae/primae_colors"),
                    colorsMask = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/primae/primae_mask"),
                    rimlight = AssetLoaderHelper.LoadTexture("textures/primamateria/atoms/primae/primae_rimlight"),
                }
            };
            PrimaMateria.Instance.AddAtomType(Primae, "primae");
        }
    }
}
