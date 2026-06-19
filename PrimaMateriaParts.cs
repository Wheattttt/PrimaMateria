// Shamelessly stolen from many sources (Complicated Elements, RM helper functions, etc)
using Brimstone;
using MonoMod.Utils;
using Quintessential;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using YamlDotNet.Core.Tokens;
using AtomTypes = class_175;
using BondSite = class_222;
using BondType = enum_126;
using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using Texture = class_256;

namespace PrimaMateria;

internal static class PrimaMateriaParts
{
    #region drawingHelpers
    private static Vector2 hexGraphicalOffset(HexIndex hex) => PrimaMateria.hexGraphicalOffset(hex);
    private static Vector2 textureDimensions(Texture tex) => tex.field_2056.ToVector2();
    private static Vector2 textureCenter(Texture tex) => (textureDimensions(tex) / 2).Rounded();
    private static void drawPartGraphic(class_195 renderer, Texture tex, Vector2 graphicPivot, float graphicAngle, Vector2 graphicTranslation, Vector2 screenTranslation)
    {
        drawPartGraphicScaled(renderer, tex, graphicPivot, graphicAngle, graphicTranslation, screenTranslation, new Vector2(1f, 1f));
    }

    private static void drawPartGraphicScaled(class_195 renderer, Texture tex, Vector2 graphicPivot, float graphicAngle, Vector2 graphicTranslation, Vector2 screenTranslation, Vector2 scaling)
    {
        //for graphicPivot and graphicTranslation, rightwards is the positive-x direction and upwards is the positive-y direction
        //graphicPivot is an absolute position, with (0,0) denoting the bottom-left corner of the texture
        //graphicTranslation is a translation, so (5,-3) means "translate 5 pixels right and 3 pixels down"
        //graphicAngle is measured in radians, and counterclockwise is the positive-angle direction
        //screenTranslation is the final translation applied, so it is not affected by rotations
        Matrix4 matrixScreenPosition = Matrix4.method_1070(renderer.field_1797.ToVector3(0f));
        Matrix4 matrixTranslateOnScreen = Matrix4.method_1070(screenTranslation.ToVector3(0f));
        Matrix4 matrixRotatePart = Matrix4.method_1073(renderer.field_1798);
        Matrix4 matrixTranslateGraphic = Matrix4.method_1070(graphicTranslation.ToVector3(0f));
        Matrix4 matrixRotateGraphic = Matrix4.method_1073(graphicAngle);
        Matrix4 matrixPivotOffset = Matrix4.method_1070(-graphicPivot.ToVector3(0f));
        Matrix4 matrixScaling = Matrix4.method_1074(scaling.ToVector3(0f));
        Matrix4 matrixTextureSize = Matrix4.method_1074(tex.field_2056.ToVector3(0f));

        Matrix4 matrix4 = matrixScreenPosition * matrixTranslateOnScreen * matrixRotatePart * matrixTranslateGraphic * matrixRotateGraphic * matrixPivotOffset * matrixScaling * matrixTextureSize;
        class_135.method_262(tex, Color.White, matrix4);
    }

    private static void drawPartGraphicSpecular(class_195 renderer, Texture tex, Vector2 graphicPivot, float graphicAngle, Vector2 graphicTranslation, Vector2 screenTranslation)
    {
        float specularAngle = (renderer.field_1799 - (renderer.field_1797 + graphicTranslation.Rotated(renderer.field_1798))).Angle() - 1.570796f - renderer.field_1798;
        drawPartGraphic(renderer, tex, graphicPivot, graphicAngle + specularAngle, graphicTranslation, screenTranslation);
    }

    private static void drawPartGloss(class_195 renderer, Texture gloss, Texture glossMask, Vector2 offset)
    {
        drawPartGloss(renderer, gloss, glossMask, offset, new HexIndex(0, 0), 0f);
    }
    private static void drawPartGloss(class_195 renderer, Texture gloss, Texture glossMask, Vector2 offset, HexIndex hexOffset, float angle)
    {
        class_135.method_257().field_1692 = class_238.field_1995.field_1757; // MaskedGlossPS shader
        class_135.method_257().field_1693[1] = gloss;
        var hex = new HexIndex(0, 0);
        Vector2 method2001 = 0.0001f * (renderer.field_1797 + hexGraphicalOffset(hex).Rotated(renderer.field_1798) - 0.5f * class_115.field_1433);
        class_135.method_257().field_1695 = method2001;
        drawPartGraphic(renderer, glossMask, offset, angle, hexGraphicalOffset(hexOffset), Vector2.Zero);
        class_135.method_257().field_1692 = class_135.method_257().field_1696; // previous shader
        class_135.method_257().field_1693[1] = class_238.field_1989.field_71;
        class_135.method_257().field_1695 = Vector2.Zero;
    }
    private static void drawAtomIO(class_195 renderer, AtomType atomType, HexIndex hex, float num)
    {
        Molecule molecule = Molecule.method_1121(atomType);
        Vector2 method1999 = renderer.field_1797 + hexGraphicalOffset(hex).Rotated(renderer.field_1798);
        Editor.method_925(molecule, method1999, new HexIndex(0, 0), 0f, 1f, num, 1f, false, null);
    }
    #endregion




    public static PartType CalcicBonder, Revolution, Synthesis, Inversion, Subversion, Sublimation, Alignment, Displacement, EtherealBonder, Affluence, Volatility;

    public static Texture bowl = class_238.field_1989.field_90.field_170;
    public static Texture metalBowl = class_238.field_1989.field_90.field_255.field_292;
    public static Texture[] glyphFlashAnimation = Brimstone.API.GetAnimation("textures/parts/PrimaMateria/glyph_flash.array", "flash", 10);
    public static Texture[] AtomFlashAnimation = Brimstone.API.GetAnimation("textures/parts/PrimaMateria/atom_flash.array", "flash", 10);

    //Calcic Bonding
    public static Texture calcicBase = Brimstone.API.GetTexture("textures/parts/PrimaMateria/calcic_bonder/base");
    public static Texture calcicSymbol = Brimstone.API.GetTexture("textures/parts/PrimaMateria/calcic_bonder/salt_symbol");
    public static Texture calcicLightmap = Brimstone.API.GetTexture("textures/parts/PrimaMateria/calcic_bonder/lightmap");
    public static Texture calcicCylinder = Brimstone.API.GetTexture("textures/parts/PrimaMateria/calcic_bonder/bond_cylinder_pattern");

    public static Texture calcicIcon = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/calcicbonder");
    public static Texture calcicHover = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/calcicbonder_hover");

    public static readonly HexIndex calcicMiddle = new(0, 0);
    public static readonly HexIndex calcicRight = new(1, 0);
    public static readonly HexIndex calcicLeft = new(-1, 0);


    //Revolution
    public static Texture revolutionBase = Brimstone.API.GetTexture("textures/parts/PrimaMateria/revolution/base");
    public static Texture revolutionTop = Brimstone.API.GetTexture("textures/parts/PrimaMateria/revolution/top");
    public static Texture revolutionGlossmask = Brimstone.API.GetTexture("textures/parts/PrimaMateria/revolution/gloss_mask");
    public static Texture revolutionGloss = Brimstone.API.GetTexture("textures/parts/PrimaMateria/revolution/gloss");
    public static Texture revolutionBowl = Brimstone.API.GetTexture("textures/parts/PrimaMateria/revolution/bowl");
    public static Texture revolutionCWsymbol = Brimstone.API.GetTexture("textures/parts/PrimaMateria/revolution/sulfur_clockwise");
    public static Texture revolutionCCWsymbol = Brimstone.API.GetTexture("textures/parts/PrimaMateria/revolution/sulfur_counterclockwise");

    public static Texture revolutionGlow = Brimstone.API.GetTexture("textures/parts/PrimaMateria/select/revolution_glow");
    public static Texture revolutionStroke = Brimstone.API.GetTexture("textures/parts/PrimaMateria/select/revolution_stroke");

    public static Texture revolutionIcon = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/revolution");
    public static Texture revolutionHover = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/revolution_hover");

    public static readonly HexIndex revBowl = new(0, 0);
    public static readonly HexIndex revClockwise = new(1, 0);
    public static readonly HexIndex revCounterClockwise = new(-1, 0);


    //Synthesis
    public static Texture synthesisBase = Brimstone.API.GetTexture("textures/parts/PrimaMateria/synthesis/base");
    public static Texture synthesisShadow = Brimstone.API.GetTexture("textures/parts/PrimaMateria/synthesis/shadow");
    public static Texture synthesisBottom = Brimstone.API.GetTexture("textures/parts/PrimaMateria/synthesis/bottom");
    public static Texture synthesisTop = Brimstone.API.GetTexture("textures/parts/PrimaMateria/synthesis/top");
    public static Texture synthesisSymbol = Brimstone.API.GetTexture("textures/parts/PrimaMateria/synthesis/prime_symbol");
    public static Texture synthesisGlossmask = Brimstone.API.GetTexture("textures/parts/PrimaMateria/synthesis/gloss_mask");
    public static Texture synthesisGloss = Brimstone.API.GetTexture("textures/parts/PrimaMateria/synthesis/gloss");

    public static Texture synthesisIcon = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/synthesis");
    public static Texture synthesisHover = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/synthesis_hover");

    public static readonly HexIndex synthesisInput1 = new(0, 0);
    public static readonly HexIndex synthesisInput2 = new(1, 0);
    public static readonly HexIndex synthesisOutput = new(0, 1);

    //Inversion
    public static Texture inversionBase = Brimstone.API.GetTexture("textures/parts/PrimaMateria/inversion/base");
    public static Texture inversionShadow = Brimstone.API.GetTexture("textures/parts/PrimaMateria/inversion/shadow");
    public static Texture inversionBottom = Brimstone.API.GetTexture("textures/parts/PrimaMateria/inversion/bottom");
    public static Texture inversionTop = Brimstone.API.GetTexture("textures/parts/PrimaMateria/inversion/top");
    public static Texture inversionSymbol = Brimstone.API.GetTexture("textures/parts/PrimaMateria/inversion/sulfur_symbol");
    public static Texture inversionGlossmask = Brimstone.API.GetTexture("textures/parts/PrimaMateria/inversion/gloss_mask");
    public static Texture inversionGloss = Brimstone.API.GetTexture("textures/parts/PrimaMateria/inversion/gloss");

    public static Texture inversionIcon = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/inversion");
    public static Texture inversionHover = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/inversion_hover");

    public static readonly HexIndex inversionOutput = new(0, 0);
    public static readonly HexIndex inversionInput = new(1, 0);

    //Subversion
    public static Texture subversionBase = Brimstone.API.GetTexture("textures/parts/PrimaMateria/subversion/base");
    public static Texture subversionShadow = Brimstone.API.GetTexture("textures/parts/PrimaMateria/subversion/shadow");
    public static Texture subversionSymbol = Brimstone.API.GetTexture("textures/parts/PrimaMateria/subversion/sulfur_symbol");
    public static Texture subversionBowl = Brimstone.API.GetTexture("textures/parts/PrimaMateria/subversion/bowl");
    public static Texture subversionBowlInput = Brimstone.API.GetTexture("textures/parts/PrimaMateria/subversion/bowl_input");
    public static Texture subversionGlossmask = Brimstone.API.GetTexture("textures/parts/PrimaMateria/subversion/gloss_mask");
    public static Texture subversionGloss = Brimstone.API.GetTexture("textures/parts/PrimaMateria/subversion/gloss");

    public static Texture[] subversionRingFlashAnimation = Brimstone.API.GetAnimation("textures/parts/PrimaMateria/subversion_ring_flash.array", "flash", 10);
    public static Texture[] subversionRingFlashGlowAnimation = Brimstone.API.GetAnimation("textures/parts/PrimaMateria/subversion_ring_flash_glow.array", "flash", 10);
    public static Texture[] subversionBowlFlashAnimation = Brimstone.API.GetAnimation("textures/parts/PrimaMateria/subversion_bowl_flash.array", "flash", 10);
    public static Texture[] subversionBowlFlashGlowAnimation = Brimstone.API.GetAnimation("textures/parts/PrimaMateria/subversion_bowl_flash_glow.array", "flash", 10);

    public static Texture subversionGlow = Brimstone.API.GetTexture("textures/parts/PrimaMateria/select/subversion_glow");
    public static Texture subversionStroke = Brimstone.API.GetTexture("textures/parts/PrimaMateria/select/subversion_stroke");

    public static Texture subversionIcon = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/subversion");
    public static Texture subversionHover = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/subversion_hover");

    public static readonly HexIndex subversionOutputLeft = new(-1, 1);
    public static readonly HexIndex subversionOutputRight = new(1, 0);
    public static readonly HexIndex subversionInput = new(0, -1);
    public static readonly HexIndex blankHex1 = new(-1, 0);
    public static readonly HexIndex blankHex2 = new(0, 1);
    public static readonly HexIndex blankHex3 = new(1, -1);

    //Sublimation
    public static Texture sublimationBase = Brimstone.API.GetTexture("textures/parts/PrimaMateria/sublimation/base");
    public static Texture sublimationGlow = Brimstone.API.GetTexture("textures/parts/PrimaMateria/select/sublimation_glow");
    public static Texture sublimationStroke = Brimstone.API.GetTexture("textures/parts/PrimaMateria/select/sublimation_stroke");
    public static Texture sublimationSymbol = Brimstone.API.GetTexture("textures/parts/PrimaMateria/sublimation/bowlsprite");
    public static Texture[] sublimationFlashGlowAnimation = Brimstone.API.GetAnimation("textures/parts/PrimaMateria/sublimation_flash_glow.array", "flash", 10);
    public static Texture[] sublimationFlashAnimation = Brimstone.API.GetAnimation("textures/parts/PrimaMateria/sublimation_flash.array", "flash", 10);

    public static Texture sublimationIcon = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/sublimation");
    public static Texture sublimationHover = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/sublimation_hover");
    public static class_126 sublimationLightmap = new class_126(class_235.method_615("textures/parts/PrimaMateria/sublimation/left"), class_235.method_615("textures/parts/PrimaMateria/sublimation/right"), class_235.method_615("textures/parts/PrimaMateria/sublimation/bottom"), class_235.method_615("textures/parts/PrimaMateria/sublimation/top"));
    public static readonly HexIndex sublimationCenter = new(0, 0);
    public static readonly HexIndex subEmptyHex1 = new(1, 0);
    public static readonly HexIndex subEmptyHex2 = new(1, -1);
    public static readonly HexIndex subEmptyHex3 = new(0, -1);
    public static readonly HexIndex subEmptyHex4 = new(-1, 0);
    public static readonly HexIndex subEmptyHex5 = new(-1, 1);
    public static readonly HexIndex subEmptyHex6 = new(0, 1);

    //Alignment
    public static Texture alignmentBase = Brimstone.API.GetTexture("textures/parts/PrimaMateria/alignment/base");
    public static Texture alignmentTop = Brimstone.API.GetTexture("textures/parts/PrimaMateria/alignment/top");
    public static Texture alignmentSymbol = Brimstone.API.GetTexture("textures/parts/PrimaMateria/alignment/primae_bowlsymbol");

    public static Texture[] alignmentFlashAnimation = Brimstone.API.GetAnimation("textures/parts/PrimaMateria/alignment_flash.array", "flash", 10);

    public static Texture alignmentGlow = Brimstone.API.GetTexture("textures/parts/PrimaMateria/select/double_glow");
    public static Texture alignmentStroke = Brimstone.API.GetTexture("textures/parts/PrimaMateria/select/double_stroke");

    public static Texture alignmentIcon = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/alignment");
    public static Texture alignmentHover = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/alignment_hover");

    public static readonly HexIndex alignmentPrimeHex = new(0, 0);
    public static readonly HexIndex alignmentPrimaeHex = new(1, 0);

    //Displacement
    public static Texture displacementBase = Brimstone.API.GetTexture("textures/parts/PrimaMateria/displacement/base");
    public static Texture displacementInput = Brimstone.API.GetTexture("textures/parts/PrimaMateria/displacement/input");
    public static Texture displacementSymbol = Brimstone.API.GetTexture("textures/parts/PrimaMateria/displacement/primae_symbol");
    public static Texture displacementTop = Brimstone.API.GetTexture("textures/parts/PrimaMateria/displacement/top");
    public static Texture displacementGlossmask = Brimstone.API.GetTexture("textures/parts/PrimaMateria/displacement/gloss_mask");
    public static Texture displacementGloss = Brimstone.API.GetTexture("textures/parts/PrimaMateria/displacement/gloss");

    public static Texture displacementGlow = Brimstone.API.GetTexture("textures/parts/PrimaMateria/select/displacement_glow");
    public static Texture displacementStroke = Brimstone.API.GetTexture("textures/parts/PrimaMateria/select/displacement_stroke");

    public static Texture displacementIcon = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/displacement");
    public static Texture displacementHover = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/displacement_hover");

    public static readonly HexIndex displacementPrimaeHex = new(0, 0);
    public static readonly HexIndex displacementLeftSwapHex = new(-2, 0);
    public static readonly HexIndex displacementRightSwapHex = new(0, 2);

    //Ethereal Bonding
    public static Texture etherealBase = Brimstone.API.GetTexture("textures/parts/PrimaMateria/ethereal_bonder/base");
    public static Texture etherealLightmap = Brimstone.API.GetTexture("textures/parts/PrimaMateria/ethereal_bonder/lightmap");
    public static Texture etherealGlossmask = Brimstone.API.GetTexture("textures/parts/PrimaMateria/ethereal_bonder/gloss_mask");
    public static Texture etherealGloss = Brimstone.API.GetTexture("textures/parts/PrimaMateria/ethereal_bonder/gloss");

    public static Texture etherealGlow = Brimstone.API.GetTexture("textures/parts/PrimaMateria/select/triple_inv_glow");
    public static Texture etherealStroke = Brimstone.API.GetTexture("textures/parts/PrimaMateria/select/triple_inv_stroke");

    public static Texture etherealIcon = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/etherealbonder");
    public static Texture etherealHover = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/etherealbonder_hover");

    public static readonly HexIndex etherealLeft = new(0, 0);
    public static readonly HexIndex etherealRight = new(1, 0);
    public static readonly HexIndex etherealInput = new(1, -1);

    //Affluence
    public static Texture affluenceBase = Brimstone.API.GetTexture("textures/parts/PrimaMateria/affluence/base");
    public static Texture affluenceShadow = Brimstone.API.GetTexture("textures/parts/PrimaMateria/affluence/shadow");
    public static Texture affluenceTop = Brimstone.API.GetTexture("textures/parts/PrimaMateria/affluence/top");
    public static Texture affluenceGlossmask = Brimstone.API.GetTexture("textures/parts/PrimaMateria/affluence/gloss_mask");
    public static Texture affluenceGloss = Brimstone.API.GetTexture("textures/parts/PrimaMateria/affluence/gloss");
    public static Texture affluenceGlow = Brimstone.API.GetTexture("textures/parts/PrimaMateria/select/affluence_glow");
    public static Texture affluenceStroke = Brimstone.API.GetTexture("textures/parts/PrimaMateria/select/affluence_stroke");

    public static Texture affluencePhosphor = Brimstone.API.GetTexture("textures/parts/PrimaMateria/affluence/phosphor");
    public static Texture affluenceAntimony = Brimstone.API.GetTexture("textures/parts/PrimaMateria/affluence/antimony");
    public static Texture affluenceMagnesia = Brimstone.API.GetTexture("textures/parts/PrimaMateria/affluence/magnesia");
    public static Texture affluencePotash = Brimstone.API.GetTexture("textures/parts/PrimaMateria/affluence/potash");
    public static Texture affluenceQuicklime = Brimstone.API.GetTexture("textures/parts/PrimaMateria/affluence/quicklime");

    public static Texture affluenceIcon = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/affluence");
    public static Texture affluenceHover = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/affluence_hover");
   
    public static readonly HexIndex affluenceSulfurHex = new(0, 0);
    public static readonly HexIndex affluencePhosphorHex = new(-1, 0);
    public static readonly HexIndex affluenceAntimonyHex = new(-1, 1);
    public static readonly HexIndex affluenceMagnesiaHex = new(0, 1);
    public static readonly HexIndex affluencePotashHex = new(1, 0);
    public static readonly HexIndex affluenceQuicklimeHex = new(1, -1);

    //Volatility
    public static Texture[] irisSulfur = Brimstone.API.GetAnimation("textures/parts/PrimaMateria/iris_full_sulfur.array", "iris", 16);
    public static Texture[] irisQuicklime = Brimstone.API.GetAnimation("textures/parts/PrimaMateria/iris_full_quicklime.array", "iris", 16);
    public static Texture[] irisPotash = Brimstone.API.GetAnimation("textures/parts/PrimaMateria/iris_full_potash.array", "iris", 16);
    public static Texture[] irisMagnesia = Brimstone.API.GetAnimation("textures/parts/PrimaMateria/iris_full_magnesia.array", "iris", 16);
    public static Texture[] irisAntimony = Brimstone.API.GetAnimation("textures/parts/PrimaMateria/iris_full_antimony.array", "iris", 16);
    public static Texture[] irisPhosphor = Brimstone.API.GetAnimation("textures/parts/PrimaMateria/iris_full_phosphor.array", "iris", 16);

    public static Texture volatilityBase = Brimstone.API.GetTexture("textures/parts/PrimaMateria/volatility/base");
    public static Texture volatilityShadow = Brimstone.API.GetTexture("textures/parts/PrimaMateria/volatility/shadow");
    public static Texture volatilityBottom = Brimstone.API.GetTexture("textures/parts/PrimaMateria/volatility/bottom");
    public static Texture volatilityTop = Brimstone.API.GetTexture("textures/parts/PrimaMateria/volatility/top");
    public static Texture volatilityGlossmask = Brimstone.API.GetTexture("textures/parts/PrimaMateria/volatility/gloss_mask");
    public static Texture volatilityGloss = Brimstone.API.GetTexture("textures/parts/PrimaMateria/volatility/gloss");
    public static Texture volatilityGlow = Brimstone.API.GetTexture("textures/parts/PrimaMateria/select/volatility_glow");
    public static Texture volatilityStroke = Brimstone.API.GetTexture("textures/parts/PrimaMateria/select/volatility_stroke");

    public static Texture volatilityOpalescence = Brimstone.API.GetTexture("textures/parts/PrimaMateria/volatility/opalescence");

    public static Texture volatilityIcon = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/volatility");
    public static Texture volatilityHover = Brimstone.API.GetTexture("textures/parts/PrimaMateria/icons/volatility_hover");

    public static readonly HexIndex volatilityInputHex = new(0, 0);
    public static readonly HexIndex volatilityPhosphorHex = new(0, -2);
    public static readonly HexIndex volatilityAntimonyHex = new(0, -1);
    public static readonly HexIndex volatilityMagnesiaHex = new(-1, 0);
    public static readonly HexIndex volatilityPotashHex = new(2, -2);
    public static readonly HexIndex volatilityQuicklimeHex = new(1, -1);
    public static readonly HexIndex volatilitySulfurHex = new(1, 0);


    public static void AddPartTypes()
    {
        CalcicBonder = new()
        {
            field_1528 = "prima-materia-calcicbonder", // ID
            field_1529 = class_134.method_253("Glyph of Calcic Bonding", string.Empty), // Name
            field_1530 = class_134.method_253("The glyph of calcic bonding creates a special bond across three atoms at once, as long as at least one of them is neutral salt. ", string.Empty), // Description
            field_1531 = 20, // Cost
            field_1539 = true, // Is a glyph
            field_1549 = revolutionGlow,
            field_1550 = revolutionStroke,
            field_1547 = calcicIcon, // Panel icon
            field_1548 = calcicHover, // Hovered panel icon
            // Bonding Sites - handle bonding manually instead
            //field_1538 = new BondSite[2]
            //    {
            //        new BondSite(new HexIndex(0, 0), new HexIndex(-1, 0), BondType.Prisma0, (Maybe<AtomType>) struct_18.field_1431),
            //        new BondSite(new HexIndex(0, 0), new HexIndex(1, 0), BondType.Prisma0, (Maybe<AtomType>) struct_18.field_1431),
            //    },
            field_1540 = new HexIndex[]
            {
                calcicMiddle,
                calcicRight,
                calcicLeft
            },
            field_1551 = Permissions.None,
            CustomPermissionCheck = perms => perms.Contains("PrimaMateria: Calcic Bonder")
        };
        QApi.AddPartType(CalcicBonder, static (part, pos, editor, renderer) =>
        {
            class_256 field1287 = calcicBase;
            Vector2 vector2 = new Vector2(125f, 48f);
            renderer.method_523(field1287, new Vector2(0.0f, -1f), vector2, 0.0f);
            foreach (HexIndex hexIndex in part.method_1159().field_1540)
            {
                renderer.method_530(class_238.field_1989.field_90.field_164 /*bonder_shadow*/, hexIndex, 4f);
                renderer.method_528(class_238.field_1989.field_90.field_163 /*bonder_ring*/, hexIndex, Vector2.Zero);
                renderer.method_529(calcicSymbol /*symbol*/, hexIndex, new Vector2(0f, 1f));
            }
            for (int i = 0; i < part.method_1159().field_1540.Length; i++)
            {
                HexIndex hexIndex = part.method_1159().field_1540[i];
                if (hexIndex != new HexIndex(0, 0))
                {
                    // render a bonder thingy to it
                    int index = i - 1;
                    float num = new HexRotation(index * 3).ToRadians();
                    renderer.method_522(class_238.field_1989.field_90.field_161 /*bonder_bond*/, new Vector2(-28f, 22f), num);
                    renderer.method_531(calcicLightmap, calcicCylinder, new HexIndex(0, 0), num);
                }
            }
        });
        Revolution = new()
        {
            field_1528 = "prima-materia-revolution", // ID
            field_1529 = class_134.method_253("Glyph of Revolution", string.Empty), // Name
            field_1530 = class_134.method_253("The glyph of revolution accepts a volatile in either the clockwise or counter-clockwise slots to revolve the volatile element in the center in that direction along the volatile wheel. ", string.Empty), // Description
            field_1531 = 20, // Cost
            field_1539 = true, // Is a glyph
            field_1549 = revolutionGlow,
            field_1550 = revolutionStroke,
            field_1547 = revolutionIcon, // Panel icon
            field_1548 = revolutionHover, // Hovered panel icon
            field_1540 = new HexIndex[]
            {
                revBowl,
                revClockwise,
                revCounterClockwise
            },
            field_1551 = Permissions.None,
            CustomPermissionCheck = perms => perms.Contains("PrimaMateria: Revolution")
        };
        QApi.AddPartType(Revolution, static (part, pos, editor, renderer) =>
        {
            Vector2 offset = new(128f, 52f);
            renderer.method_523(revolutionBase, Vector2.Zero, offset, 0f);
            renderer.method_523(revolutionTop, Vector2.Zero, offset, 0f);
            renderer.method_528(revolutionBowl, revBowl, Vector2.Zero);
            renderer.method_529(revolutionCWsymbol, revClockwise, Vector2.Zero);
            renderer.method_529(revolutionCCWsymbol, revCounterClockwise, Vector2.Zero);
            renderer.method_529(subversionSymbol, revBowl, Vector2.Zero);
            drawPartGloss(renderer, revolutionGloss, revolutionGlossmask, offset);
        });
        Synthesis = new()
        {
            field_1528 = "prima-materia-synthesis", // ID
            field_1529 = class_134.method_253("Glyph of Synthesis", string.Empty), // Name
            field_1530 = class_134.method_253("The glyph of synthesis accepts two of the tria prima (Salt, Quicksilver, and Sulfur), outputting the third. ", string.Empty), // Description
            field_1531 = 20, // Cost
            field_1539 = true, // Is a glyph
            field_1549 = class_238.field_1989.field_97.field_386,// triple_glow
            field_1550 = class_238.field_1989.field_97.field_387, // triple_stroke
            field_1547 = synthesisIcon, // Panel icon
            field_1548 = synthesisHover, // Hovered panel icon
            field_1540 = new HexIndex[]
            {
                synthesisInput1,
                synthesisInput2,
                synthesisOutput
            },
            field_1551 = Permissions.None,
            CustomPermissionCheck = perms => perms.Contains("PrimaMateria: Synthesis")
        };
        QApi.AddPartType(Synthesis, static (part, pos, editor, renderer) =>
        {
            PartSimState pss = editor.method_507().method_481(part);
            class_236 uco = editor.method_1989(part, pos);
            float time = editor.method_504();

            Vector2 offset = new(42f, 49f);
            renderer.method_523(synthesisBase, Vector2.Zero, offset, 0f);
            renderer.method_523(synthesisShadow, Vector2.Zero, offset, 0f);
            renderer.method_523(synthesisBottom, Vector2.Zero, offset, 0f);

            int irisFrame = 15;
            bool afterIrisOpens = false;
            Molecule risingMetal = null;
            Vector2 risingOffset = uco.field_1984 + class_187.field_1742.method_492(synthesisOutput).Rotated(uco.field_1985);
            if (pss.field_2743)
            {
                irisFrame = class_162.method_404((int)(class_162.method_411(1f, -1f, time) * 16f), 0, 15);
                afterIrisOpens = time > 0.5f;
                risingMetal = Molecule.method_1121(pss.field_2744[0]);
                if (!afterIrisOpens)
                {
                    // show atom rising behind iris
                    Editor.method_925(risingMetal, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                }
            }
            renderer.method_529(class_238.field_1989.field_90.field_246[irisFrame], synthesisOutput, Vector2.Zero);
            renderer.method_523(synthesisTop, Vector2.Zero, offset, 0f);
            renderer.method_529(synthesisSymbol, synthesisInput1, Vector2.Zero);
            renderer.method_529(synthesisSymbol, synthesisInput2, Vector2.Zero);
            drawPartGloss(renderer, synthesisGloss, synthesisGlossmask, offset);
            if (pss.field_2743 && afterIrisOpens)
            {
                // show atom rising infront of iris
                Editor.method_925(risingMetal, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }
        });
        Inversion = new()
        {
            field_1528 = "prima-materia-inversion", // ID
            field_1529 = class_134.method_253("Glyph of Inversion", string.Empty), // Name
            field_1530 = class_134.method_253("The glyph of inversion accepts a volatile-type atom and outputs its inverse form. ", string.Empty), // Description
            field_1531 = 20, // Cost
            field_1539 = true, // Is a glyph
            field_1549 = class_238.field_1989.field_97.field_374, // Shadow/glow
            field_1550 = class_238.field_1989.field_97.field_375, // Stroke/outline
            field_1547 = inversionIcon, // Panel icon
            field_1548 = inversionHover, // Hovered panel icon
            field_1540 = new HexIndex[]
            {
                inversionOutput,
                inversionInput
            },
            field_1551 = Permissions.None,
            CustomPermissionCheck = perms => perms.Contains("PrimaMateria: Inversion")
        };
        QApi.AddPartType(Inversion, static (part, pos, editor, renderer) =>
        {
            PartSimState pss = editor.method_507().method_481(part);
            class_236 uco = editor.method_1989(part, pos);
            float time = editor.method_504();

            Vector2 offset = new(46f, 52f);
            renderer.method_523(inversionBase, Vector2.Zero, offset, 0f);
            renderer.method_523(inversionShadow, Vector2.Zero, offset, 0f);
            renderer.method_523(inversionBottom, Vector2.Zero, offset, 0f);

            int irisFrame = 15;
            bool afterIrisOpens = false;
            Molecule risingMetal = null;
            Vector2 risingOffset = uco.field_1984 + class_187.field_1742.method_492(inversionOutput).Rotated(uco.field_1985);
            if (pss.field_2743)
            {
                irisFrame = class_162.method_404((int)(class_162.method_411(1f, -1f, time) * 16f), 0, 15);
                afterIrisOpens = time > 0.5f;
                risingMetal = Molecule.method_1121(pss.field_2744[0]);
                if (!afterIrisOpens)
                {
                    // show atom rising behind iris
                    Editor.method_925(risingMetal, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                }
            }
            renderer.method_529(class_238.field_1989.field_90.field_246[irisFrame], inversionOutput, Vector2.Zero);
            renderer.method_523(inversionTop, Vector2.Zero, offset, 0f);
            renderer.method_529(inversionSymbol, inversionInput, Vector2.Zero);
            drawPartGloss(renderer, inversionGloss, inversionGlossmask, offset);
            if (pss.field_2743 && afterIrisOpens)
            {
                // show atom rising infront of iris
                Editor.method_925(risingMetal, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }
        });
        Subversion = new()
        {
            field_1528 = "prima-materia-subversion", // ID
            field_1529 = class_134.method_253("Glyph of Subversion", string.Empty), // Name
            field_1530 = class_134.method_253("The glyph of subversion copies a volatile's two triadic partners onto another two volatile atoms. ", string.Empty), // Description
            field_1531 = 30, // Cost
            field_1539 = true, // Is a glyph
            field_1549 = subversionGlow, // Shadow/glow
            field_1550 = subversionStroke, // Stroke/outline
            field_1547 = subversionIcon, // Panel icon
            field_1548 = subversionHover, // Hovered panel icon
            field_1540 = new HexIndex[]
        {
                subversionOutputLeft,
                subversionOutputRight,
                subversionInput,
                blankHex1,
                blankHex2,
                blankHex3
        },
            field_1551 = Permissions.None,
            CustomPermissionCheck = perms => perms.Contains("PrimaMateria: Subversion")
        };
        QApi.AddPartType(Subversion, static (part, pos, editor, renderer) =>
        {
            PartSimState pss = editor.method_507().method_481(part);
            class_236 uco = editor.method_1989(part, pos);
            float time = editor.method_504();

            Vector2 offset = new(124f, 120f);
            renderer.method_523(subversionBase, Vector2.Zero, offset, 0f);
            renderer.method_523(subversionShadow, Vector2.Zero, offset, 0f);
            renderer.method_528(subversionBowlInput, subversionInput, Vector2.Zero);
            renderer.method_529(subversionSymbol, subversionInput, Vector2.Zero);
            renderer.method_529(subversionBowl, subversionOutputLeft, Vector2.Zero);
            renderer.method_529(subversionBowl, subversionOutputRight, Vector2.Zero);
            drawPartGloss(renderer, subversionGloss, subversionGlossmask, offset);
        });
        Sublimation = new()
        {
            field_1528 = "prima-materia-sublimation", // ID
            field_1529 = class_134.method_253("Glyph of Sublimation", string.Empty), // Name
            field_1530 = class_134.method_253("The glyph of sublimation is capable of unimpressing atoms of the tria prima into atoms of pure primae. ", string.Empty), // Description
            field_1531 = 50, // Cost
            field_1539 = true, // Is a glyph
            field_1549 = sublimationGlow,
            field_1550 = sublimationStroke,
            field_1547 = sublimationIcon, // Panel icon
            field_1548 = sublimationHover, // Hovered panel icon
            field_1552 = true, // Only one allowed
            field_1540 = new HexIndex[]
            {
                sublimationCenter,
                subEmptyHex1,
                subEmptyHex2,
                subEmptyHex3,
                subEmptyHex4,
                subEmptyHex5,
                subEmptyHex6
            },
            field_1551 = Permissions.None,
            CustomPermissionCheck = perms => perms.Contains("PrimaMateria: Sublimation")
        };
        QApi.AddPartType(Sublimation, static (part, pos, editor, renderer) =>
        {
            Vector2 offset = new(124f, 119f);
            renderer.method_523(sublimationBase, Vector2.Zero, offset, 0f);
            renderer.method_527(sublimationLightmap, sublimationCenter, Vector2.Zero, offset, 0f);
            renderer.method_528(bowl, sublimationCenter, Vector2.Zero);
            renderer.method_529(sublimationSymbol, sublimationCenter, Vector2.Zero);
        });
        Alignment = new()
        {
            field_1528 = "prima-materia-alignment", // ID
            field_1529 = class_134.method_253("Glyph of Alignment", string.Empty), // Name
            field_1530 = class_134.method_253("The glyph of alignment copies the impression of any of the tria prima onto pure unimpressed primae. ", string.Empty), // Description
            field_1531 = 30, // Cost
            field_1539 = true, // Is a glyph
            field_1549 = class_238.field_1989.field_97.field_374, // Shadow/glow
            field_1550 = class_238.field_1989.field_97.field_375, // Stroke/outline
            field_1547 = alignmentIcon, // Panel icon
            field_1548 = alignmentHover, // Hovered panel icon
            field_1540 = new HexIndex[]
        {
                alignmentPrimaeHex,
                alignmentPrimeHex
        },
            field_1551 = Permissions.None,
            CustomPermissionCheck = perms => perms.Contains("PrimaMateria: Alignment")
        };
        QApi.AddPartType(Alignment, static (part, pos, editor, renderer) =>
        {
            PartSimState pss = editor.method_507().method_481(part);
            class_236 uco = editor.method_1989(part, pos);
            float time = editor.method_504();

            Vector2 offset = new(42f, 48f);
            renderer.method_523(alignmentBase, Vector2.Zero, offset, 0f);
            renderer.method_528(bowl, alignmentPrimeHex, Vector2.Zero);
            renderer.method_529(metalBowl, alignmentPrimaeHex, Vector2.Zero);
            renderer.method_529(alignmentSymbol, alignmentPrimaeHex, Vector2.Zero);
            renderer.method_529(sublimationSymbol, alignmentPrimeHex, Vector2.Zero);
            renderer.method_523(alignmentTop, Vector2.Zero, offset, 0f);
        });
        Displacement = new()
        {
            field_1528 = "prima-materia-displacement", // ID
            field_1529 = class_134.method_253("Glyph of Displacement", string.Empty), // Name
            field_1530 = class_134.method_253("The glyph of displacement accepts primae to switch the positions of the atoms in the two inputs, without destroying their bonds. ", string.Empty), // Description
            field_1531 = 40, // Cost
            field_1539 = true, // Is a glyph
            field_1549 = displacementGlow, // Shadow/glow
            field_1550 = displacementStroke, // Stroke/outline
            field_1547 = displacementIcon, // Panel icon
            field_1548 = displacementHover, // Hovered panel icon
            field_1540 = new HexIndex[]
        {
                displacementPrimaeHex,
                displacementLeftSwapHex,
                displacementRightSwapHex,
                new HexIndex(-1, 0),
                new HexIndex(0, 1)
        },
            field_1551 = Permissions.None,
            CustomPermissionCheck = perms => perms.Contains("PrimaMateria: Displacement")
        };
        QApi.AddPartType(Displacement, static (part, pos, editor, renderer) =>
        {
            PartSimState pss = editor.method_507().method_481(part);
            class_236 uco = editor.method_1989(part, pos);
            float time = editor.method_504();

            Vector2 offset = new(210f, 52f);
            renderer.method_523(displacementBase, Vector2.Zero, offset, 0f);
            renderer.method_528(subversionBowl, displacementLeftSwapHex, Vector2.Zero);
            renderer.method_528(subversionBowl, displacementRightSwapHex, Vector2.Zero);
            renderer.method_529(displacementInput, displacementPrimaeHex, Vector2.Zero);
            renderer.method_529(displacementSymbol, displacementPrimaeHex, Vector2.Zero);
            renderer.method_523(displacementTop, Vector2.Zero, offset, 0f);
            drawPartGloss(renderer, displacementGloss, displacementGlossmask, offset);
        });
        EtherealBonder = new()
        {
            field_1528 = "prima-materia-etherealbonder", // ID
            field_1529 = class_134.method_253("Glyph of Ethereal Bonding", string.Empty), // Name
            field_1530 = class_134.method_253("The glyph of ethereal bonding takes one atom of Primae to create a special kind of bond between two atoms of the same type. ", string.Empty), // Description
            field_1531 = 30, // Cost
            field_1539 = true, // Is a glyph
            field_1549 = etherealGlow,
            field_1550 = etherealStroke,
            field_1547 = etherealIcon, // Panel icon
            field_1548 = etherealHover, // Hovered panel icon
            // Bonding Sites - handle bonding manually instead
            //field_1538 = new BondSite[2]
            //    {
            //        new BondSite(new HexIndex(0, 0), new HexIndex(-1, 0), BondType.Prisma0, (Maybe<AtomType>) struct_18.field_1431),
            //        new BondSite(new HexIndex(0, 0), new HexIndex(1, 0), BondType.Prisma0, (Maybe<AtomType>) struct_18.field_1431),
            //    },
            field_1540 = new HexIndex[]
            {
                etherealLeft,
                etherealRight,
                etherealInput
            },
            field_1551 = Permissions.None,
            CustomPermissionCheck = perms => perms.Contains("PrimaMateria: Ethereal Bonder")
        };
        QApi.AddPartType(EtherealBonder, static (part, pos, editor, renderer) =>
        {
            renderer.method_523(etherealBase, Vector2.Zero, new Vector2(42f, 119f), 0f);
            Vector2 vector2 = new Vector2(125f, 48f);
            renderer.method_530(class_238.field_1989.field_90.field_164 /*bonder_shadow*/, etherealLeft, 4f);
            renderer.method_528(class_238.field_1989.field_90.field_163 /*bonder_ring*/, etherealLeft, Vector2.Zero);
            renderer.method_530(class_238.field_1989.field_90.field_164 /*bonder_shadow*/, etherealRight, 4f);
            renderer.method_528(class_238.field_1989.field_90.field_163 /*bonder_ring*/, etherealRight, Vector2.Zero);
            renderer.method_529(displacementInput, etherealInput, Vector2.Zero);
            renderer.method_529(displacementSymbol, etherealInput, Vector2.Zero);
            drawPartGloss(renderer, etherealGloss, etherealGlossmask, new Vector2(42f, 119f));
            //renderer.method_529(calcicSymbol /*symbol*/, hexIndex, new Vector2(0f, 1f));
            for (int i = 0; i < part.method_1159().field_1540.Length; i++)
            {
                HexIndex hexIndex = part.method_1159().field_1540[i];
                if (hexIndex == etherealRight)
                {
                    // render a bonder thingy to it
                    int index = i - 1;
                    float num = new HexRotation(index * 3).ToRadians();
                    renderer.method_522(class_238.field_1989.field_90.field_161 /*bonder_bond*/, new Vector2(-28f, 22f), num);
                    renderer.method_531(etherealLightmap, class_238.field_1989.field_90.field_166, new HexIndex(0, 0), num);
                }
            }
        });

        Affluence = new()
        {
            field_1528 = "prima-materia-affluence", // ID
            field_1529 = class_134.method_253("Glyph of Affluence", string.Empty), // Name
            field_1530 = class_134.method_253("The glyph of affluence transmutes one atom of each of the six volatile elements into one atom of opalescence.", string.Empty), // Description
            field_1531 = 20, // Cost
            field_1539 = true, // Is a glyph
            field_1549 = affluenceGlow,
            field_1550 = affluenceStroke,
            field_1547 = affluenceIcon, // Panel icon
            field_1548 = affluenceHover, // Hovered panel icon
            field_1540 = new HexIndex[]
            {
                affluenceSulfurHex,
                affluencePhosphorHex,
                affluenceAntimonyHex,
                affluenceMagnesiaHex,
                affluencePotashHex,
                affluenceQuicklimeHex
            },
            field_1551 = Permissions.None,
            CustomPermissionCheck = perms => perms.Contains("PrimaMateria: Affluence")
        };
        QApi.AddPartType(Affluence, static (part, pos, editor, renderer) =>
        {
            Vector2 offset = new(124f, 119f);
            renderer.method_523(affluenceBase, Vector2.Zero, offset, 0f);
            renderer.method_523(affluenceShadow, Vector2.Zero, offset, 0f);
            renderer.method_528(subversionBowl, affluenceSulfurHex, Vector2.Zero);
            renderer.method_523(affluenceTop, Vector2.Zero, offset, 0f);
            renderer.method_529(affluenceQuicklime, affluenceQuicklimeHex, Vector2.Zero);
            renderer.method_529(affluencePotash, affluencePotashHex, Vector2.Zero);
            renderer.method_529(affluenceMagnesia, affluenceMagnesiaHex, Vector2.Zero);
            renderer.method_529(affluenceAntimony, affluenceAntimonyHex, Vector2.Zero);
            renderer.method_529(affluencePhosphor, affluencePhosphorHex, Vector2.Zero);
            renderer.method_529(subversionSymbol, affluenceSulfurHex, Vector2.Zero);
            drawPartGloss(renderer, affluenceGloss, affluenceGlossmask, offset);
        });
        Volatility = new()
        {
            field_1528 = "prima-materia-volatility", // ID
            field_1529 = class_134.method_253("Glyph of Volatility", string.Empty), // Name
            field_1530 = class_134.method_253("The glyph of volatility transmutes one atom of opalescence into one atom of each of the six volatile elements. ", string.Empty), // Description
            field_1531 = 20, // Cost
            field_1539 = true, // Is a glyph
            field_1549 = volatilityGlow,
            field_1550 = volatilityStroke,
            field_1547 = volatilityIcon, // Panel icon
            field_1548 = volatilityHover, // Hovered panel icon
            field_1540 = new HexIndex[]
            {
                volatilityInputHex,
                volatilitySulfurHex,
                volatilityQuicklimeHex,
                volatilityPotashHex,
                volatilityMagnesiaHex,
                volatilityAntimonyHex,
                volatilityPhosphorHex,
            },
            field_1551 = Permissions.None,
            CustomPermissionCheck = perms => perms.Contains("PrimaMateria: Volatility")
        };
        QApi.AddPartType(Volatility, static (part, pos, editor, renderer) =>
        {
            PartSimState pss = editor.method_507().method_481(part);
            class_236 uco = editor.method_1989(part, pos);
            float time = editor.method_504();

            Vector2 offset = new(125f, 190f);
            renderer.method_523(volatilityBase, Vector2.Zero, offset, 0f);
            renderer.method_523(volatilityShadow, Vector2.Zero, offset, 0f);
            renderer.method_523(volatilityBottom, Vector2.Zero, offset, 0f);

            int irisFrame = 15;
            bool afterIrisOpens = false;
            Molecule risingSulfur = null;
            Molecule risingQuicklime = null;
            Molecule risingPotash = null;
            Molecule risingMagnesia = null;
            Molecule risingAntimony = null;
            Molecule risingPhosphor = null;
            Vector2 risingOffsetSulfur = uco.field_1984 + class_187.field_1742.method_492(volatilitySulfurHex).Rotated(uco.field_1985);
            Vector2 risingOffsetQuicklime = uco.field_1984 + class_187.field_1742.method_492(volatilityQuicklimeHex).Rotated(uco.field_1985);
            Vector2 risingOffsetPotash = uco.field_1984 + class_187.field_1742.method_492(volatilityPotashHex).Rotated(uco.field_1985);
            Vector2 risingOffsetMagnesia = uco.field_1984 + class_187.field_1742.method_492(volatilityMagnesiaHex).Rotated(uco.field_1985);
            Vector2 risingOffsetAntimony = uco.field_1984 + class_187.field_1742.method_492(volatilityAntimonyHex).Rotated(uco.field_1985);
            Vector2 risingOffsetPhosphor = uco.field_1984 + class_187.field_1742.method_492(volatilityPhosphorHex).Rotated(uco.field_1985);
            if (pss.field_2743)
            {
                irisFrame = class_162.method_404((int)(class_162.method_411(1f, -1f, time) * 16f), 0, 15);
                afterIrisOpens = time > 0.5f;
                risingSulfur = Molecule.method_1121(pss.field_2744[0]);
                risingQuicklime = Molecule.method_1121(pss.field_2744[1]);
                risingPotash = Molecule.method_1121(pss.field_2744[2]);
                risingMagnesia = Molecule.method_1121(pss.field_2744[3]);
                risingAntimony = Molecule.method_1121(pss.field_2744[4]);
                risingPhosphor = Molecule.method_1121(pss.field_2744[5]);
                if (!afterIrisOpens)
                {
                    // show atom rising behind iris
                    Editor.method_925(risingSulfur, risingOffsetSulfur, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                    Editor.method_925(risingQuicklime, risingOffsetQuicklime, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                    Editor.method_925(risingPotash, risingOffsetPotash, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                    Editor.method_925(risingMagnesia, risingOffsetMagnesia, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                    Editor.method_925(risingAntimony, risingOffsetAntimony, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                    Editor.method_925(risingPhosphor, risingOffsetPhosphor, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                }
            }
            renderer.method_529(irisSulfur[irisFrame], volatilitySulfurHex, Vector2.Zero); //Render Irises
            renderer.method_529(irisQuicklime[irisFrame], volatilityQuicklimeHex, Vector2.Zero);
            renderer.method_529(irisPotash[irisFrame], volatilityPotashHex, Vector2.Zero);
            renderer.method_529(irisMagnesia[irisFrame], volatilityMagnesiaHex, Vector2.Zero);
            renderer.method_529(irisAntimony[irisFrame], volatilityAntimonyHex, Vector2.Zero);
            renderer.method_529(irisPhosphor[irisFrame], volatilityPhosphorHex, Vector2.Zero);
            renderer.method_523(volatilityTop, Vector2.Zero, offset, 0f);
            renderer.method_529(volatilityOpalescence, volatilityInputHex, Vector2.Zero);
            drawPartGloss(renderer, volatilityGloss, volatilityGlossmask, offset);
            if (pss.field_2743 && afterIrisOpens)
            {
                // show atom rising infront of iris
                Editor.method_925(risingSulfur, risingOffsetSulfur, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                Editor.method_925(risingQuicklime, risingOffsetQuicklime, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                Editor.method_925(risingPotash, risingOffsetPotash, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                Editor.method_925(risingMagnesia, risingOffsetMagnesia, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                Editor.method_925(risingAntimony, risingOffsetAntimony, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                Editor.method_925(risingPhosphor, risingOffsetPhosphor, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }
        });


        QApi.AddPartTypeToPanel(CalcicBonder, false);
        QApi.AddPartTypeToPanel(Revolution, false);
        QApi.AddPartTypeToPanel(Synthesis, false);
        QApi.AddPartTypeToPanel(Inversion, false);
        QApi.AddPartTypeToPanel(Subversion, false);
        QApi.AddPartTypeToPanel(Sublimation, false);
        QApi.AddPartTypeToPanel(Alignment, false);
        QApi.AddPartTypeToPanel(Displacement, false);
        QApi.AddPartTypeToPanel(EtherealBonder, false);
        QApi.AddPartTypeToPanel(Affluence, false);
        QApi.AddPartTypeToPanel(Volatility, false);

        QApi.RunAfterCycle((sim, first) => {
            var seb = sim.field_3818;
            List<Part> allParts = seb.method_502().field_3919;
            var simStates = sim.field_3821;
            Maybe<AtomReference> maybeFindAtom(Part part, HexIndex hex, List<Part> list, bool checkWheels = false)
            {
                return (Maybe<AtomReference>)API.PrivateMethod<Sim>("method_1850").Invoke(sim, new object[] { part, hex, list, checkWheels });
            }

            foreach (var part in allParts)
            {
                PartSimState pss = simStates[part];
                var type = part.method_1159();
                if (type == Synthesis)
                {
                    if (first)
                    {
                        // Do both atoms exist
                        if (sim.FindAtomRelative(part, synthesisInput1).method_99(out AtomReference primeIn1) && sim.FindAtomRelative(part, synthesisInput2).method_99(out AtomReference primeIn2))
                        {
                            if (!sim.FindAtomRelative(part, synthesisOutput).method_1085())
                            {
                                // if atoms aren't being held or consumed
                                if ((!primeIn1.field_2281 && !primeIn1.field_2282) && (!primeIn2.field_2281 && !primeIn2.field_2282))
                                {

                                    //check atom type
                                    foreach (API.SynthesisRecipe recipe in API.SynthesisTransmutation)
                                    {
                                        if ((recipe.input1 == (primeIn1.field_2280)) && (recipe.input2 == (primeIn2.field_2280)))
                                        {
                                            Brimstone.API.RemoveAtom(primeIn1);
                                            seb.field_3937.Add(new(seb, primeIn1.field_2278, recipe.input1));
                                            Brimstone.API.DrawFallingAtom(seb, primeIn1);
                                            Brimstone.API.RemoveAtom(primeIn2);
                                            seb.field_3937.Add(new(seb, primeIn2.field_2278, recipe.input2));
                                            Brimstone.API.DrawFallingAtom(seb, primeIn2);
                                            pss.field_2743 = true;
                                            pss.field_2744 = new AtomType[1] { recipe.output };
                                            Brimstone.API.PlaySound(sim, Sounds.Synthesis);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (pss.field_2743)
                    {
                        Brimstone.API.AddAtom(sim, part, synthesisOutput, pss.field_2744[0]);
                    }
                }
                else if (type == Revolution)
                {
                    // Is atom in bowl
                    if (sim.FindAtomRelative(part, revBowl).method_99(out AtomReference volatileReference))
                    {

                        if ((sim.FindAtomRelative(part, revClockwise).method_99(out AtomReference volatileCW)) | (sim.FindAtomRelative(part, revCounterClockwise).method_99(out AtomReference volatileCCW)))
                        {
                            //check direction so we only do the correct transmutation
                            bool isCounterClockwise = false;
                            if (volatileCW != null && (volatileCW.field_2281 || volatileCW.field_2282))
                            {
                                volatileCW = null;
                            }
                            if (volatileCCW != null && (volatileCCW.field_2281 || volatileCCW.field_2282))
                            {
                                volatileCCW = null;
                            }
                            if (volatileCW == null)
                            {
                                isCounterClockwise = true;
                            }
                            if (volatileCW != null ^ volatileCCW != null)
                            {
                                foreach (API.RevolutionRecipe recipe in API.RevolutionTransmutation)
                                {
                                    //check atom type
                                    if (recipe.input == (volatileReference.field_2280))
                                    {
                                        if (isCounterClockwise)
                                        {
                                            if (API.AcceptedVolatileAtoms.Contains(volatileCCW.field_2280))
                                            {
                                                Brimstone.API.RemoveAtom(volatileCCW);
                                                seb.field_3937.Add(new(seb, volatileCCW.field_2278, PrimaMateriaAtoms.Sulfur));
                                                Brimstone.API.DrawFallingAtom(seb, volatileCCW);
                                                Brimstone.API.ChangeAtom(volatileReference, recipe.ccwoutput);
                                                Brimstone.API.PlaySound(sim, Sounds.Revolution);
                                                Color glowcolor = new Color(0f, 0f, 0f, 0f);
                                                foreach (API.AtomColor assignment in API.AtomColorsList)
                                                {
                                                    if (assignment.atom == recipe.ccwoutput)
                                                    {
                                                        glowcolor = assignment.color;
                                                    }
                                                }
                                                seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(revBowl)), subversionBowlFlashGlowAnimation, 30f, Vector2.Zero, 0f).WithColor(glowcolor));
                                                seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(revBowl)), subversionBowlFlashAnimation, 30f, Vector2.Zero, 0f));
                                                seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(revBowl)), AtomFlashAnimation, 30f, Vector2.Zero, 0f));
                                            }
                                        }
                                        else
                                        {
                                            if (API.AcceptedVolatileAtoms.Contains(volatileCW.field_2280))
                                            {
                                                Brimstone.API.RemoveAtom(volatileCW);
                                                seb.field_3937.Add(new(seb, volatileCW.field_2278, PrimaMateriaAtoms.Sulfur));
                                                Brimstone.API.DrawFallingAtom(seb, volatileCW);
                                                Brimstone.API.ChangeAtom(volatileReference, recipe.cwoutput);
                                                Brimstone.API.PlaySound(sim, Sounds.Revolution);
                                                Color glowcolor = new Color(0f, 0f, 0f, 0f);
                                                foreach (API.AtomColor assignment in API.AtomColorsList)
                                                {
                                                    if (assignment.atom == recipe.cwoutput)
                                                    {
                                                        glowcolor = assignment.color;
                                                    }
                                                }
                                                seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(revBowl)), subversionBowlFlashGlowAnimation, 30f, Vector2.Zero, 0f).WithColor(glowcolor));
                                                seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(revBowl)), subversionBowlFlashAnimation, 30f, Vector2.Zero, 0f));
                                                seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(revBowl)), AtomFlashAnimation, 30f, Vector2.Zero, 0f));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (type == Inversion)
                {
                    if (first)
                    {
                        // Does atom exist
                        if (sim.FindAtomRelative(part, inversionInput).method_99(out AtomReference volatileIn))
                        {
                            if (!sim.FindAtomRelative(part, inversionOutput).method_1085())
                            {
                                // if atom isn't being held or consumed
                                if (!volatileIn.field_2281 && !volatileIn.field_2282)
                                {

                                    //check atom type
                                    foreach (API.InversionRecipe recipe in API.InversionTransmutation)
                                    {
                                        if (recipe.input == (volatileIn.field_2280))
                                        {
                                            Brimstone.API.RemoveAtom(volatileIn);
                                            seb.field_3937.Add(new(seb, volatileIn.field_2278, recipe.input));
                                            Brimstone.API.DrawFallingAtom(seb, volatileIn);
                                            pss.field_2743 = true;
                                            pss.field_2744 = new AtomType[1] { recipe.output };
                                            Brimstone.API.PlaySound(sim, Sounds.Inversion);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (pss.field_2743)
                    {
                        Brimstone.API.AddAtom(sim, part, inversionOutput, pss.field_2744[0]);
                    }
                }
                else if (type == Subversion)
                {
                    // Does atom exist
                    bool inputExists = false;
                    if (sim.FindAtomRelative(part, subversionInput).method_99(out AtomReference volatileReference))
                    {
                        inputExists = true;
                    }
                    else if (Wheel_Fabienne.MaybeFindFabienneWheelAtom(sim, part, subversionInput).method_99(out volatileReference))
                    {
                        inputExists = true;
                    }
                    else if (Wheel_Tullas.MaybeFindTullasWheelAtom(sim, part, subversionInput).method_99(out volatileReference))
                    {
                        inputExists = true;
                    }
                    if (inputExists == true)
                    {
                        if (sim.FindAtomRelative(part, subversionOutputLeft).method_99(out AtomReference volatileLeft) & sim.FindAtomRelative(part, subversionOutputRight).method_99(out AtomReference volatileRight))
                        {
                            //check atom type
                            foreach (API.SubversionRecipe recipe in API.SubversionTransmutation)
                            {
                                if (recipe.input == (volatileReference.field_2280))
                                {
                                    if ((recipe.leftoutput != (volatileLeft.field_2280)) || (recipe.rightoutput != (volatileRight.field_2280)))
                                    {
                                        if ((API.AcceptedVolatileAtoms.Contains(volatileLeft.field_2280)) && (API.AcceptedVolatileAtoms.Contains(volatileRight.field_2280)))
                                        {
                                            class_236 partInfo = seb.method_1989(part, Vector2.Zero);
                                            float sixtyDegrees = 60f * (float)Math.PI / 180f;
                                            Color leftcolor = new Color(0f, 0f, 0f, 0f);
                                            Color rightcolor = new Color(0f, 0f, 0f, 0f);
                                            foreach (API.AtomColor assignment in API.AtomColorsList)
                                            {
                                                if (assignment.atom == recipe.leftoutput)
                                                {
                                                    leftcolor = assignment.color;
                                                }
                                                if (assignment.atom == recipe.rightoutput)
                                                {
                                                    rightcolor = assignment.color;
                                                }
                                            }
                                            seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(subversionOutputLeft)), AtomFlashAnimation, 30f, Vector2.Zero, 0f));
                                            Brimstone.API.ChangeAtom(volatileLeft, recipe.leftoutput);
                                            seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(subversionOutputLeft)), subversionBowlFlashGlowAnimation, 30f, Vector2.Zero, 0f).WithColor(leftcolor));
                                            seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(subversionOutputLeft)), subversionBowlFlashAnimation, 30f, Vector2.Zero, 0f));
                                            seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(sublimationCenter)), subversionRingFlashGlowAnimation, 30f, Vector2.Zero, partInfo.field_1985).WithColor(leftcolor));
                                            seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(sublimationCenter)), subversionRingFlashAnimation, 30f, Vector2.Zero, partInfo.field_1985));

                                            seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(subversionOutputRight)), AtomFlashAnimation, 30f, Vector2.Zero, 0f));
                                            Brimstone.API.ChangeAtom(volatileRight, recipe.rightoutput);
                                            seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(subversionOutputRight)), subversionBowlFlashGlowAnimation, 30f, Vector2.Zero, 0f).WithColor(rightcolor));
                                            seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(subversionOutputRight)), subversionBowlFlashAnimation, 30f, Vector2.Zero, 0f));
                                            seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(sublimationCenter)), subversionRingFlashGlowAnimation, 30f, Vector2.Zero, (partInfo.field_1985 + (sixtyDegrees * 2))).WithColor(rightcolor));
                                            seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(sublimationCenter)), subversionRingFlashAnimation, 30f, Vector2.Zero, (partInfo.field_1985 + (sixtyDegrees * 2))));
                                            Brimstone.API.PlaySound(sim, Sounds.Subversion);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (type == Sublimation)
                {
                    // Does atom exist
                    if (sim.FindAtomRelative(part, sublimationCenter).method_99(out AtomReference PrimeReference))
                    {
                        if (API.AcceptedPrimeAtoms.Contains(PrimeReference.field_2280))
                        {
                            class_236 partInfo = seb.method_1989(part, Vector2.Zero);

                            Brimstone.API.PlaySound(sim, Sounds.Sublimation);
                            Brimstone.API.ChangeAtom(PrimeReference, PrimaMateriaAtoms.Primae);
                            PrimeReference.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, PrimeReference.field_2280, class_238.field_1989.field_81.field_614, 30f);
                            seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(revBowl)), sublimationFlashAnimation, 30f, Vector2.Zero, partInfo.field_1985));
                        }
                    }
                }
                else if (type == Alignment)
                {
                    // Does atom exist
                    bool inputExists = maybeFindAtom(part, alignmentPrimeHex, new List<Part>(), true).method_99(out AtomReference PrimeAtomReference);
                    if (inputExists == true)
                    {
                        if (sim.FindAtomRelative(part, alignmentPrimaeHex).method_99(out AtomReference PrimaeAtomReference))
                        {
                            //check atom types
                            if (API.AcceptedPrimeAtoms.Contains(PrimeAtomReference.field_2280))
                            {
                                if (API.AcceptedPrimaMateriaAtoms.Contains(PrimaeAtomReference.field_2280))
                                {
                                    Brimstone.API.ChangeAtom(PrimaeAtomReference, PrimeAtomReference.field_2280);
                                    PrimaeAtomReference.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, PrimaeAtomReference.field_2280, class_238.field_1989.field_81.field_614, 30f);
                                    seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(alignmentPrimaeHex)), alignmentFlashAnimation, 30f, Vector2.Zero, 0f));
                                    Brimstone.API.PlaySound(sim, Sounds.Alignment);
                                }
                            }
                        }
                    }
                }
                else if (type == Displacement)
                {
                    // Does primae exist
                    if (sim.FindAtomRelative(part, displacementPrimaeHex).method_99(out AtomReference PrimaeAtom))
                    {
                        // Is primae bound or being held
                        if (!PrimaeAtom.field_2281 && !PrimaeAtom.field_2282)
                        {
                            // Are atoms in both bowls
                            if ((sim.FindAtomRelative(part, displacementLeftSwapHex).method_99(out AtomReference LeftSwapAtom))
                            && (sim.FindAtomRelative(part, displacementRightSwapHex).method_99(out AtomReference RightSwapAtom)))
                            {
                                Brimstone.API.RemoveAtom(PrimaeAtom);
                                seb.field_3937.Add(new(seb, PrimaeAtom.field_2278, PrimaeAtom.field_2280));
                                Brimstone.API.DrawFallingAtom(seb, PrimaeAtom);
                                AtomType LeftSwapAtomTemp = LeftSwapAtom.field_2280;
                                Brimstone.API.ChangeAtom(LeftSwapAtom, RightSwapAtom.field_2280);
                                LeftSwapAtom.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, LeftSwapAtom.field_2280, class_238.field_1989.field_81.field_614, 30f);
                                seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(displacementLeftSwapHex)), alignmentFlashAnimation, 30f, Vector2.Zero, 0f));
                                Brimstone.API.ChangeAtom(RightSwapAtom, LeftSwapAtomTemp);
                                RightSwapAtom.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, RightSwapAtom.field_2280, class_238.field_1989.field_81.field_614, 30f);
                                seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(displacementRightSwapHex)), alignmentFlashAnimation, 30f, Vector2.Zero, 0f));
                                Brimstone.API.PlaySound(sim, Sounds.Alignment);
                            }
                        }
                    }
                }
                else if (type == CalcicBonder)
                {
                    // Is every bowl filled
                    if (sim.FindAtomRelative(part, calcicLeft).method_99(out AtomReference leftAtom)
                    && sim.FindAtomRelative(part, calcicMiddle).method_99(out AtomReference middleAtom)
                    && sim.FindAtomRelative(part, calcicRight).method_99(out AtomReference rightAtom))
                    {
                        // Is at least one of the atoms salt
                        if (leftAtom.field_2280 == Brimstone.API.VanillaAtoms.salt
                        || middleAtom.field_2280 == Brimstone.API.VanillaAtoms.salt
                        || rightAtom.field_2280 == Brimstone.API.VanillaAtoms.salt)
                        {
                            Brimstone.API.JoinMoleculesAtHexes(sim, part, calcicLeft, calcicMiddle);
                            Brimstone.API.JoinMoleculesAtHexes(sim, part, calcicMiddle, calcicRight);
                            Brimstone.API.AddBond(sim, part, calcicLeft, calcicMiddle, (BondType)Bonds.CALCIC_BOND, true, true);
                            Brimstone.API.AddBond(sim, part, calcicRight, calcicMiddle, (BondType)Bonds.CALCIC_BOND, true, true);
                        }
                    }
                }
                else if (type == EtherealBonder)
                {
                    // Are both bowls and the input filled
                    if (sim.FindAtomRelative(part, etherealLeft).method_99(out AtomReference leftAtom)
                    && sim.FindAtomRelative(part, etherealRight).method_99(out AtomReference rightAtom)
                    && sim.FindAtomRelative(part, etherealInput).method_99(out AtomReference inputAtom))
                    {
                        // Is input Primae?
                        if (inputAtom.field_2280 == PrimaMateriaAtoms.Primae
                        // Is Primae not being held or bonded?
                        && (!inputAtom.field_2281 && !inputAtom.field_2282)
                        // Are bonding atoms the same type?
                        && leftAtom.field_2280 == rightAtom.field_2280
                        && Brimstone.API.FindBondTypeRelative(sim, part, etherealLeft, etherealRight) == BondType.None)
                        {
                            Brimstone.API.RemoveAtom(inputAtom);
                            seb.field_3937.Add(new(seb, inputAtom.field_2278, inputAtom.field_2280));
                            Brimstone.API.DrawFallingAtom(seb, inputAtom);
                            Brimstone.API.JoinMoleculesAtHexes(sim, part, etherealLeft, etherealRight);
                            Brimstone.API.AddBond(sim, part, etherealLeft, etherealRight, (BondType)Bonds.ETHEREAL_BOND, true, true);
                        }
                    }
                }
                else if (type == Affluence)
                {
                    // are all atoms in the bowls
                    if (sim.FindAtomRelative(part, affluenceSulfurHex).method_99(out AtomReference bowlAtom) &&
                    sim.FindAtomRelative(part, affluenceQuicklimeHex).method_99(out AtomReference inputAtom1) &&
                    sim.FindAtomRelative(part, affluencePotashHex).method_99(out AtomReference inputAtom2) &&
                    sim.FindAtomRelative(part, affluenceMagnesiaHex).method_99(out AtomReference inputAtom3) &&
                    sim.FindAtomRelative(part, affluenceAntimonyHex).method_99(out AtomReference inputAtom4) &&
                    sim.FindAtomRelative(part, affluencePhosphorHex).method_99(out AtomReference inputAtom5))
                    {
                        bool invalidinput = false; // make sure every input atom is unbound and not being held (except for the center bowl hex!)
                        AtomReference[] inputReferences = new AtomReference[5]{inputAtom1,inputAtom2,inputAtom3,inputAtom4,inputAtom5};
                        foreach (AtomReference atom in inputReferences)
                        {
                            if (atom.field_2281 && atom.field_2282)
                            {
                                invalidinput = true;
                                break;
                            }
                        }
                        if (!invalidinput)
                        {
                            // Check that every atom in a recipe exists
                            AtomType[] allTypes = new AtomType[6] { bowlAtom.field_2280, inputAtom1.field_2280, inputAtom2.field_2280, inputAtom3.field_2280, inputAtom4.field_2280, inputAtom5.field_2280 };
                            foreach (API.AffluenceRecipe recipe in API.AffluenceTransmutation)
                            {
                                if (allTypes.Contains(recipe.sulfur) &&
                                allTypes.Contains(recipe.quicklime) &&
                                allTypes.Contains(recipe.potash) &&
                                allTypes.Contains(recipe.magnesia) &&
                                allTypes.Contains(recipe.antimony) &&
                                allTypes.Contains(recipe.phosphor))
                                {
                                    foreach (AtomReference atom in inputReferences)
                                    {
                                        Brimstone.API.RemoveAtom(atom);
                                        seb.field_3937.Add(new(seb, atom.field_2278, atom.field_2280));
                                        Brimstone.API.DrawFallingAtom(seb, atom);
                                    }
                                    Brimstone.API.ChangeAtom(bowlAtom, recipe.opalescence);
                                    Color glowcolor = new Color(0f, 0f, 0f, 0f);
                                    foreach (API.AtomColor assignment in API.AtomColorsList)
                                    {
                                        if (assignment.atom == recipe.opalescence)
                                        {
                                            glowcolor = assignment.color;
                                        }
                                    }
                                    seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(affluenceSulfurHex)), AtomFlashAnimation, 30f, Vector2.Zero, 0f));
                                    seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(affluenceSulfurHex)), subversionBowlFlashGlowAnimation, 30f, Vector2.Zero, 0f).WithColor(glowcolor));
                                    seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(affluenceSulfurHex)), subversionBowlFlashAnimation, 30f, Vector2.Zero, 0f));
                                    Brimstone.API.PlaySound(sim, Sounds.Affluence);
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (type == Volatility)
                {
                    HexIndex[] outputHexes = new HexIndex[6]
                    {
                        volatilitySulfurHex,
                        volatilityQuicklimeHex,
                        volatilityPotashHex,
                        volatilityMagnesiaHex,
                        volatilityAntimonyHex,
                        volatilityPhosphorHex,
                    };
                    if (first)
                    {
                        // Does atom exist
                        if (sim.FindAtomRelative(part, volatilityInputHex).method_99(out AtomReference atomIn))
                        {
                            bool blocked = false; // check all hexes, from MetalQuintessence
                            foreach (HexIndex h in outputHexes)
                            {
                                if (sim.FindAtomRelative(part, h).method_1085())
                                {
                                    blocked = true;
                                    break;
                                }
                            }
                            if (!blocked)
                            {
                                // if atom isn't being held or consumed
                                if (!atomIn.field_2281 && !atomIn.field_2282)
                                {

                                    //check atom type
                                    foreach (API.VolatilityRecipe recipe in API.VolatilityTransmutation)
                                    {
                                        if (recipe.opalescence == (atomIn.field_2280))
                                        {
                                            Brimstone.API.RemoveAtom(atomIn);
                                            seb.field_3937.Add(new(seb, atomIn.field_2278, recipe.opalescence));
                                            Brimstone.API.DrawFallingAtom(seb, atomIn);
                                            pss.field_2743 = true;
                                            pss.field_2744 = new AtomType[6] { recipe.sulfur, recipe.quicklime, recipe.potash, recipe.magnesia, recipe.antimony, recipe.phosphor };
                                            Brimstone.API.PlaySound(sim, Sounds.Volatility);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (pss.field_2743)
                    {
                        Brimstone.API.AddAtom(sim, part, volatilitySulfurHex, pss.field_2744[0]);
                        Brimstone.API.AddAtom(sim, part, volatilityQuicklimeHex, pss.field_2744[1]);
                        Brimstone.API.AddAtom(sim, part, volatilityPotashHex, pss.field_2744[2]);
                        Brimstone.API.AddAtom(sim, part, volatilityMagnesiaHex, pss.field_2744[3]);
                        Brimstone.API.AddAtom(sim, part, volatilityAntimonyHex, pss.field_2744[4]);
                        Brimstone.API.AddAtom(sim, part, volatilityPhosphorHex, pss.field_2744[5]);
                    }
                }
            }
        });
    }
}