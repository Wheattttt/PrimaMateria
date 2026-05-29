using System.Collections.Generic;
using System.Reflection;

namespace PrimaMateria
{
    internal static class Sounds
    {
        public static Sound Synthesis;
        public static Sound Revolution;
        public static Sound Inversion;
        public static Sound Subversion;
        public static Sound Sublimation;
        public static Sound Alignment;
        public static Sound EtherealBonder;
        public static Sound CalcicBonder;

        public static void LoadSounds()
        {
            Synthesis = Brimstone.API.GetSound(PrimaMateria.contentPath, "sounds/glyph_synthesis").method_1087();
            Revolution = Brimstone.API.GetSound(PrimaMateria.contentPath, "sounds/glyph_revolution").method_1087();
            Inversion = Brimstone.API.GetSound(PrimaMateria.contentPath, "sounds/glyph_inversion").method_1087();
            Subversion = Brimstone.API.GetSound(PrimaMateria.contentPath, "sounds/glyph_subversion").method_1087();
            Sublimation = Brimstone.API.GetSound(PrimaMateria.contentPath, "sounds/glyph_sublimation").method_1087();
            Alignment = Brimstone.API.GetSound(PrimaMateria.contentPath, "sounds/glyph_alignment").method_1087();
            EtherealBonder = Brimstone.API.GetSound(PrimaMateria.contentPath, "sounds/etherealbonder").method_1087();
            CalcicBonder = Brimstone.API.GetSound(PrimaMateria.contentPath, "sounds/calcicbonder").method_1087();

            FieldInfo field = typeof(class_11).GetField("field_52", BindingFlags.Static | BindingFlags.NonPublic);
            Dictionary<string, float> volumeDictionary = (Dictionary<string, float>)field.GetValue(null);

            volumeDictionary.Add("glyph_synthesis", 0.5f);
            volumeDictionary.Add("glyph_revolution", 0.33f);
            volumeDictionary.Add("glyph_inversion", 0.5f);
            volumeDictionary.Add("glyph_subversion", 0.5f);
            volumeDictionary.Add("glyph_sublimation", 0.33f);
            volumeDictionary.Add("glyph_alignment", 0.33f);
            volumeDictionary.Add("etherealbonder", 0.4f);
            volumeDictionary.Add("calcicbonder", 0.4f);

            On.class_201.method_540 += Sounds.Method_540;
        }

        public static void Unload()
        {
            On.class_201.method_540 -= Sounds.Method_540;
        }

        public static void Method_540(On.class_201.orig_method_540 orig, class_201 self)
        {
            orig(self);
            Synthesis.field_4062 = false;
            Revolution.field_4062 = false;
            Inversion.field_4062 = false;
            Subversion.field_4062 = false;
            Sublimation.field_4062 = false;
            Alignment.field_4062 = false;
            EtherealBonder.field_4062 = false;
            CalcicBonder.field_4062 = false;
        }
    }
}