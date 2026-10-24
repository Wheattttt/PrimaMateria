using Quintessential;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PrimaMateria
{
    internal class Sounds
    {
        public static Sound glyph_calcicbonder, glyph_etherealbonder, glyph_affluence, glyph_alignment, glyph_inversion, glyph_revolution, glyph_sublimation, glyph_subversion, glyph_synthesis, glyph_volatility;
        public static void AddSounds()
        {
            glyph_calcicbonder = AssetLoaderHelper.LoadSound("sounds/primamateria/glyph_calcicbonder");
            glyph_etherealbonder = AssetLoaderHelper.LoadSound("sounds/primamateria/glyph_etherealbonder");
            glyph_affluence = AssetLoaderHelper.LoadSound("sounds/primamateria/glyph_affluence");
            glyph_alignment = AssetLoaderHelper.LoadSound("sounds/primamateria/glyph_alignment");
            glyph_inversion = AssetLoaderHelper.LoadSound("sounds/primamateria/glyph_inversion");
            glyph_revolution = AssetLoaderHelper.LoadSound("sounds/primamateria/glyph_revolution");
            glyph_sublimation = AssetLoaderHelper.LoadSound("sounds/primamateria/glyph_sublimation");
            glyph_subversion = AssetLoaderHelper.LoadSound("sounds/primamateria/glyph_subversion");
            glyph_synthesis = AssetLoaderHelper.LoadSound("sounds/primamateria/glyph_synthesis");
            glyph_volatility = AssetLoaderHelper.LoadSound("sounds/primamateria/glyph_volatility");

            Volume.SetVolume(glyph_calcicbonder, 0.25f);
            Volume.SetVolume(glyph_etherealbonder, 0.3f);
            Volume.SetVolume(glyph_affluence, 0.35f);
            Volume.SetVolume(glyph_alignment, 0.2f);
            Volume.SetVolume(glyph_inversion, 0.25f);
            Volume.SetVolume(glyph_revolution, 0.15f);
            Volume.SetVolume(glyph_sublimation, 0.2f);
            Volume.SetVolume(glyph_subversion, 0.25f);
            Volume.SetVolume(glyph_synthesis, 0.3f);
            Volume.SetVolume(glyph_volatility, 0.35f);
        }
    }
}
