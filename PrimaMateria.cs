using Brimstone;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using Quintessential;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using AtomTypes = class_175;
using BondStyle = class_200;
using BondType = enum_126;
using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using Texture = class_256;

namespace PrimaMateria;

public class PrimaMateria : QuintessentialMod
{
    // Define bond overlaps - Used in conjunction with a hook later
    readonly static Dictionary<BondType, BondType> CustomBondOverlaps = new Dictionary<BondType, BondType>
    {
        {BondType.None, BondType.Standard | BondType.Prisma0 | BondType.Prisma1 | BondType.Prisma2},
        {BondType.Standard, BondType.None},
        {(BondType)Bonds.ETHEREAL_BOND, BondType.None},
        {(BondType)Bonds.CALCIC_BOND, BondType.None},
        {BondType.Prisma0, BondType.Prisma1 | BondType.Prisma2},
        {BondType.Prisma1, BondType.Prisma0 | BondType.Prisma2},
        {BondType.Prisma2, BondType.Prisma0 | BondType.Prisma1},
    };

    //drawing helpers
    public static Vector2 hexGraphicalOffset(HexIndex hex) => class_187.field_1742.method_492(hex);

    public static Texture[] fetchTextureArray(int length, string path)
    {
        var ret = new Texture[length];
        for (int i = 0; i < ret.Length; i++)
        {
            ret[i] = class_235.method_615(path + (i + 1).ToString("0000"));
        }
        return ret;
    }
    public static string contentPath;

    public override void Load()
    {
        Quintessential.Logger.Log("PM - Prima Materia Registered");
    }
    public override void Unload()
    {
        Quintessential.Logger.Log("PM - Prima Materia Unloaded");
        Sounds.Unload();
        Bonds.UnloadHooks();
    }

    public override void LoadPuzzleContent()
    {
        Quintessential.Logger.Log("PM - Prima Materia Loading");
        PrimaMateriaAtoms.AddAtomTypes();
        PrimaMateriaParts.AddPartTypes();
        Wheel_Fabienne.LoadContent();
        Wheel_Tullas.LoadContent();
        contentPath = Brimstone.API.GetContentPath("PrimaMateria").method_1087();
        Sounds.LoadSounds();
        QApi.AddPuzzlePermission("PrimaMateria: Calcic Bonder", "Glyph of Calcic Bonding", "Prima Materia");
        QApi.AddPuzzlePermission("PrimaMateria: Synthesis", "Glyph of Synthesis", "Prima Materia");
        QApi.AddPuzzlePermission("PrimaMateria: Revolution", "Glyph of Revolution", "Prima Materia");
        QApi.AddPuzzlePermission("PrimaMateria: Inversion", "Glyph of Inversion", "Prima Materia");
        QApi.AddPuzzlePermission("PrimaMateria: Subversion", "Glyph of Subversion", "Prima Materia");
        QApi.AddPuzzlePermission("PrimaMateria: Sublimation", "Glyph of Sublimation", "Prima Materia");
        QApi.AddPuzzlePermission("PrimaMateria: Alignment", "Glyph of Alignment", "Prima Materia");
        QApi.AddPuzzlePermission("PrimaMateria: Displacement", "Glyph of Displacement", "Prima Materia");
        QApi.AddPuzzlePermission("PrimaMateria: Ethereal Bonder", "Glyph of Ethereal Bonding", "Prima Materia");
        QApi.AddPuzzlePermission("PrimaMateria: Affluence", "Glyph of Affluence", "Prima Materia");
        QApi.AddPuzzlePermission("PrimaMateria: Volatility", "Glyph of Volatility", "Prima Materia");
        QApi.AddPuzzlePermission("PrimaMateria: Fabienne's Wheel", "Fabienne's Wheel", "Prima Materia");
        QApi.AddPuzzlePermission("PrimaMateria: Tullas' Wheel", "Tullas' Wheel", "Prima Materia");
        API.AddTransmutations();
        Bonds.LoadHooking();
        Bonds.ModInitBondTextures();
        Quintessential.Logger.Log("PM - Prima Materia Loaded");
        //------------------------- HOOKING, stolen from RM -------------------------//
        IL.SolutionEditorBase.method_1984 += drawWheelAtoms;

        // horrible jank hook - stops custom bondtypes from unintentionally stacking with other bondtypes. Taken from Bondlib
        bool MoleculeTryToBond(On.Molecule.orig_method_1112 orig, Molecule self, BondType bonderBondType, HexIndex param_4835, HexIndex param_4836, Maybe<BondEffect> param_4837)
        {
            Molecule.class_348 class_292 = new Molecule.class_348();
            class_292.field_2646 = param_4835;
            class_292.field_2647 = param_4836;
            if (HexIndex.Distance(class_292.field_2646, class_292.field_2647) != 1)
            {
                throw new class_266("Invalid distance between ends of bond");
            }
            List<class_277> field_2643 = typeof(Molecule).GetField("field_2643", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self) as List<class_277>;
            MethodInfo class_292_method_1123 = typeof(Molecule.class_348).GetMethod("method_1123", BindingFlags.Instance | BindingFlags.NonPublic);
            Maybe<class_277> maybe = field_2643.Where(class_292_method_1123.CreateDelegate<Func<class_277, bool>>(class_292)).method_430();
            if (maybe.method_1085())
            {
                Molecule.class_351 class_293 = new Molecule.class_351();
                class_293.field_2650 = maybe.method_1087();
                BondType atomBondType = class_293.field_2650.field_2186;
                if ((atomBondType & bonderBondType) == bonderBondType) return false;
                if (!CustomBondOverlaps.ContainsKey(bonderBondType)) return false;
                if ((atomBondType & CustomBondOverlaps[bonderBondType]) == 0 && atomBondType != BondType.None) return false;
                class_293.field_2650.field_2186 |= bonderBondType;
                MethodInfo class_293_method_1126 = typeof(Molecule.class_351).GetMethod("method_1126", BindingFlags.Instance | BindingFlags.NonPublic);
                param_4837.method_1093(class_293_method_1126.CreateDelegate<Action<BondEffect>>(class_293));
                return true;
            }
            Molecule.class_349 class_294 = new Molecule.class_349();
            class_294.field_2648 = new class_277(bonderBondType, class_292.field_2646, class_292.field_2647);
            MethodInfo class_294_method_1124 = typeof(Molecule.class_349).GetMethod("method_1124", BindingFlags.Instance | BindingFlags.NonPublic);
            param_4837.method_1093(class_294_method_1124.CreateDelegate<Action<BondEffect>>(class_294));
            field_2643.Add(class_294.field_2648);
            return true;
        }
        On.Molecule.method_1112 += MoleculeTryToBond;
    }


    private static void drawWheelAtoms(ILContext il)
    {
        ILCursor cursor = new ILCursor(il);
        // skip ahead to roughly where method_2015 is called
        cursor.Goto(658);

        // jump ahead to just after the method_2015 for-loop
        if (!cursor.TryGotoNext(MoveType.After, instr => instr.Match(OpCodes.Ldarga_S))) return;

        // load the SolutionEditorBase self and the class423 local onto the stack so we can use it
        cursor.Emit(OpCodes.Ldarg_0);
        cursor.Emit(OpCodes.Ldloc_0);
        // then run the new code
        cursor.EmitDelegate<Action<SolutionEditorBase, SolutionEditorBase.class_423>>((seb_self, class423) =>
        {
            if (seb_self.method_503() != enum_128.Stopped)
            {
                var partList = seb_self.method_502().field_3919;
                foreach (var fabienne in partList.Where(x => x.method_1159() == Wheel_Fabienne.Fabienne))
                {
                    Wheel_Fabienne.drawFabienneAtoms(seb_self, fabienne, class423.field_3959);
                }
                foreach (var tullas in partList.Where(x => x.method_1159() == Wheel_Tullas.Tullas))
                {
                    Wheel_Tullas.drawTullasAtoms(seb_self, tullas, class423.field_3959);
                }
            }
        });
    }

    public static Texture periodicTableOverlay;
    ILHook ilhook_method_926;
    public override void PostLoad()
    {
        periodicTableOverlay = Brimstone.API.GetTexture("textures/periodic_table/PrimaMateria/overlay");
        On.class_177.method_50 += OnMethod50;
        On.SolutionEditorBase.method_1997 += DrawPartSelectionGlows;
        // Hook for enabling the gloss effect on custom bondTypes, thank you Iris! https://gist.github.com/Iris-xii/e57c4ab7032e7e49748695fff16c59f9
        ilhook_method_926 = new ILHook(
            typeof(Editor).GetMethod("method_926", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static),
            static ilcontext => {
                try
                {
                    Mono.Cecil.Cil.VariableDefinition literalStore =
                        new(ilcontext.Import(typeof(int)));
                    ilcontext.Method.Body.Variables.Add(literalStore);
                    Mono.Cecil.Cil.VariableDefinition fieldStore =
                        new(ilcontext.Import(typeof(int)));
                    ilcontext.Method.Body.Variables.Add(fieldStore);
                    ILCursor c = new(ilcontext);
                    c.GotoNext(MoveType.After,
                        x => { return x.MatchCallOrCallvirt(typeof(Matrix4).GetMethod("op_Multiply")); }
                    );
                    c.GotoNext(MoveType.After,
                        x => { return x.MatchLdcI4(1); }
                    );
                    c.Emit(Mono.Cecil.Cil.OpCodes.Stloc, literalStore.Index); // store literal '1'
                    c.Emit(Mono.Cecil.Cil.OpCodes.Stloc, fieldStore.Index); // store field_1814
                    c.Emit(Mono.Cecil.Cil.OpCodes.Ldloc, fieldStore.Index); // emit field_1814
                    c.Emit(Mono.Cecil.Cil.OpCodes.Ldloc, literalStore.Index); // emit literal '1'
                    c.EmitDelegate(
                        //returns either a new literal for the comparison
                        //or just literal_one unmodified if field_1814 != <cond>
                        (int field_1814, int literal_one) => {
                            if (field_1814 == 16)
                            {  // 16 = Calcic Bond
                                return 16;
                            }
                            else
                            {
                                return literal_one;
                            }
                        }
                    );
                    //stack re-shuffling
                    c.Emit(Mono.Cecil.Cil.OpCodes.Stloc, literalStore.Index); // store literal '1/x' that has been returned
                                                                              //pretend I did absolutely nothing:
                    c.Emit(Mono.Cecil.Cil.OpCodes.Ldloc, fieldStore.Index); // emit field_1814
                    c.Emit(Mono.Cecil.Cil.OpCodes.Ldloc, literalStore.Index); // emit literal '1', or modified one
                }
                catch (Exception e)
                {
                    Logger.Log($"[bond hook] {e}");
                }
            }
        );
    }

    //Modify Periodic Table
    private static void OnMethod50(
    On.class_177.orig_method_50 orig,
    class_177 self,
    float param_3780)
    {
        orig(self, param_3780);
        Vector2 vector = new Vector2(1516f, 922f);
        Vector2 vector2 = (class_115.field_1433 / 2 - vector / 2 + new Vector2(-2f, -11f)).Rounded();
        class_135.method_272(periodicTableOverlay, vector2 + new Vector2(83f, 94f));
        // Volatiles
        class_135.method_290("_Magnesia_", vector2 + new Vector2(281f, 675f), class_238.field_1990.field_2151, DocumentScreen.field_2410, (enum_0)1, 1f, 0.6f, float.MaxValue, float.MaxValue, 0, default(Color), null, int.MaxValue, param_3473: false, param_3474: true);
        class_135.method_290("_Antimony_", vector2 + new Vector2(172f, 612f), class_238.field_1990.field_2151, DocumentScreen.field_2410, (enum_0)1, 1f, 0.6f, float.MaxValue, float.MaxValue, 0, default(Color), null, int.MaxValue, param_3473: false, param_3474: true);
        class_135.method_290("_Phosphor_", vector2 + new Vector2(172f, 488f), class_238.field_1990.field_2151, DocumentScreen.field_2410, (enum_0)1, 1f, 0.6f, float.MaxValue, float.MaxValue, 0, default(Color), null, int.MaxValue, param_3473: false, param_3474: true);
        class_135.method_290("_Sulfur_", vector2 + new Vector2(281f, 427f), class_238.field_1990.field_2151, DocumentScreen.field_2410, (enum_0)1, 1f, 0.6f, float.MaxValue, float.MaxValue, 0, default(Color), null, int.MaxValue, param_3473: false, param_3474: true);
        class_135.method_290("_Quicklime_", vector2 + new Vector2(389f, 488f), class_238.field_1990.field_2151, DocumentScreen.field_2410, (enum_0)1, 1f, 0.6f, float.MaxValue, float.MaxValue, 0, default(Color), null, int.MaxValue, param_3473: false, param_3474: true);
        class_135.method_290("_Potash_", vector2 + new Vector2(389f, 612f), class_238.field_1990.field_2151, DocumentScreen.field_2410, (enum_0)1, 1f, 0.6f, float.MaxValue, float.MaxValue, 0, default(Color), null, int.MaxValue, param_3473: false, param_3474: true);

        // Titles
        class_135.method_292("_The Volatiles_", vector2 + new Vector2(281f, 805f), class_238.field_1990.field_2148, DocumentScreen.field_2410, (enum_0)1, 1f, 0.6f, 300f, float.MaxValue, 0, default(Color), null, int.MaxValue);
        class_135.method_292("_The Cardinals_", vector2 + new Vector2(762f, 805f), class_238.field_1990.field_2148, DocumentScreen.field_2410, (enum_0)1, 1f, 0.6f, 300f, float.MaxValue, 0, default(Color), null, int.MaxValue);
        class_135.method_292("_The Metals_", vector2 + new Vector2(1233f, 805f), class_238.field_1990.field_2148, DocumentScreen.field_2410, (enum_0)1, 1f, 0.6f, 300f, float.MaxValue, 0, default(Color), null, int.MaxValue);
    }
    public void DrawPartSelectionGlows(On.SolutionEditorBase.orig_method_1997 orig, SolutionEditorBase seb_self, Part part, Vector2 pos, float alpha)
    {
        if (part.method_1159() == Wheel_Fabienne.Fabienne) Wheel_Fabienne.drawSelectionGlow(seb_self, part, pos, alpha);
        if (part.method_1159() == Wheel_Tullas.Tullas) Wheel_Tullas.drawSelectionGlow(seb_self, part, pos, alpha);
        orig(seb_self, part, pos, alpha);
    }
}
