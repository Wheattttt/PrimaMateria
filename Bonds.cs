using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.Utils;
using Quintessential;
using Quintessential.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PrimaMateria; // This code is pretty much all from @skyhawk033 on the OM discord.

using BondTextures = class_200;
using BondType = enum_126;
using On_BondM = On.Quintessential.Serialization.PuzzleModel.BondM;
using Texture = class_256;

public static class Bonds
{
    public const byte CALCIC_BOND = 16;
    public const byte ETHEREAL_BOND = 32;
    public static Texture bondTexture0, bondTexture0_gloss, bondTexture1;
    public static BondTextures bondTextures0, bondTextures1;
    public static MethodInfo editorDrawBond = typeof(MoleculeEditorScreen).GetMethod("method_1131", BindingFlags.NonPublic | BindingFlags.Instance);

    internal static void LoadHooking()
    {
        Logger.Log("[PrimaMateria] Hooking Bonds!");
        On_BondM.BondBits += ModBondBits;
        On_BondM.ctor_class_277 += ModBondM;
        On.MoleculeEditorScreen.method_50 += ModAddBondToEditor;
        On.class_283.method_779 += ModGetBondTextures;
    }

    internal static void UnloadHooks()
    {
        Logger.Log("[PrimaMateria] Removing Bond Hooks!");
        On_BondM.BondBits -= ModBondBits;
        On_BondM.ctor_class_277 -= ModBondM;
        On.MoleculeEditorScreen.method_50 -= ModAddBondToEditor;
        On.class_283.method_779 -= ModGetBondTextures;
    }
    internal static void ModInitBondTextures()
    {
        int pre_size = class_167.field_1643.Length;
        Logger.Log($"[PrimaMateria] Initializing Textures. former size: {pre_size}");

        bondTexture0 = class_235.method_615("textures/bonds/PrimaMateria/calcic");
        bondTexture0_gloss = class_235.method_615("textures/bonds/PrimaMateria/calcic_normals");
        bondTexture1 = class_235.method_615("textures/bonds/PrimaMateria/ethereal");
        bondTextures0 = new BondTextures
        {
            field_1814 = (BondType)CALCIC_BOND,
            field_1815 = bondTexture0, // Bond Base Texture
            field_1816 = bondTexture0_gloss, // Bond Normal Map
            field_1817 = class_238.field_1989.field_83.field_147, // Bond animation?
            field_1819 = class_238.field_1989.field_83.field_152, // Bond animation?
            field_1820 = Sounds.CalcicBonder // Bond Sound
        };

        bondTextures1 = new BondTextures
        {
            field_1814 = (BondType)ETHEREAL_BOND,
            field_1815 = bondTexture1, // Bond Base Texture
            field_1816 = class_238.field_1989.field_83.field_146, // Bond Normal Map
            field_1817 = class_238.field_1989.field_83.field_147, // Bond animation?
            field_1819 = class_238.field_1989.field_83.field_152, // Bond animation?
            field_1820 = Sounds.EtherealBonder // Bond Sound
        };

        Array.Resize(ref class_167.field_1643, pre_size + 2);
        class_167.field_1643[pre_size] = bondTextures0;
        class_167.field_1643[pre_size + 1] = bondTextures1;
    }

    internal static byte ModBondBits(On_BondM.orig_BondBits orig, PuzzleModel.BondM self)
    {
        byte b = orig(self);
        if (self.BondTypes.Contains("calcic_bond"))
            b |= CALCIC_BOND;
        if (self.BondTypes.Contains("ethereal_bond"))
            b |= ETHEREAL_BOND;
        return b;
    }

    internal static void ModBondM(On_BondM.orig_ctor_class_277 orig, PuzzleModel.BondM self, class_277 bond)
    {
        orig(self, bond);
        if (((byte)bond.field_2186 & CALCIC_BOND) == CALCIC_BOND)
            self.BondTypes.Add("calcic_bond");
        if (((byte)bond.field_2186 & ETHEREAL_BOND) == ETHEREAL_BOND)
            self.BondTypes.Add("ethereal_bond");
    }

    internal static void ModAddBondToEditor(On.MoleculeEditorScreen.orig_method_50 orig, global::MoleculeEditorScreen self, float unknown)
    {
        // copied vectors from orig_method_50
        Vector2 vector = new(1516f, 922f);
        Vector2 vector2 = (class_115.field_1433 / 2 - vector / 2 + new Vector2(-2f, -11f)).Rounded();

        orig(self, unknown);
        editorDrawBond.Invoke(self, new object[] { vector2 + new Vector2(454f, 231f), CALCIC_BOND, bondTexture0 });
        editorDrawBond.Invoke(self, new object[] { vector2 + new Vector2(549f, 231f), ETHEREAL_BOND, bondTexture1 });
    }

    internal static BondTextures ModGetBondTextures(On.class_283.orig_method_779 orig, BondType bond)
    {
        if ((int)bond == CALCIC_BOND)
            return bondTextures0;
        if ((int)bond == ETHEREAL_BOND)
            return bondTextures1;
        return orig(bond);
    }
}
