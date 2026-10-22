using MonoMod.Utils;
using Quintessential;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static Quintessential.PartCycleDelegate;

namespace PrimaMateria;

internal static class Parts
{

    public static PartType CalcicBonder, Revolution, Synthesis, Inversion, Subversion, Sublimation, Alignment, Displacement, EtherealBonder, Affluence, Volatility;

    public static Texture bowl = Assets.textures.parts.calcinator_bowl;
    public static Texture metalBowl = Assets.textures.parts.projection_glyph.metal_bowl;
    public static Texture[] glyphFlashAnimation = API.LoadAnimation("textures/primamateria/parts/glyph_flash.array", "flash", 10);
    public static Texture[] AtomFlashAnimation = API.LoadAnimation("textures/primamateria/parts/atom_flash.array", "flash", 10);

    //Calcic Bonding
    public static Texture calcicBase = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/calcic_bonder/base");
    public static Texture calcicSymbol = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/calcic_bonder/salt_symbol");
    public static Texture calcicLightmap = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/calcic_bonder/lightmap");
    public static Texture calcicCylinder = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/calcic_bonder/bond_cylinder_pattern");

    public static Texture calcicIcon = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/calcicbonder");
    public static Texture calcicHover = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/calcicbonder_hover");

    public static readonly HexIndex calcicMiddle = new(0, 0);
    public static readonly HexIndex calcicRight = new(1, 0);
    public static readonly HexIndex calcicLeft = new(-1, 0);


    //Revolution
    public static Texture revolutionBase = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/revolution/base");
    public static Texture revolutionTop = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/revolution/top");
    public static Texture revolutionGlossmask = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/revolution/gloss_mask");
    public static Texture revolutionGloss = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/revolution/gloss");
    public static Texture revolutionBowl = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/revolution/bowl");
    public static Texture revolutionCWsymbol = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/revolution/sulfur_clockwise");
    public static Texture revolutionCCWsymbol = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/revolution/sulfur_counterclockwise");

    public static Texture revolutionGlow = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/select/revolution_glow");
    public static Texture revolutionStroke = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/select/revolution_stroke");

    public static Texture revolutionIcon = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/revolution");
    public static Texture revolutionHover = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/revolution_hover");

    public static readonly HexIndex revBowl = new(0, 0);
    public static readonly HexIndex revClockwise = new(1, 0);
    public static readonly HexIndex revCounterClockwise = new(-1, 0);


    //Synthesis
    public static Texture synthesisBase = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/synthesis/base");
    public static Texture synthesisShadow = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/synthesis/shadow");
    public static Texture synthesisBottom = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/synthesis/bottom");
    public static Texture synthesisTop = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/synthesis/top");
    public static Texture synthesisSymbol = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/synthesis/prime_symbol");
    public static Texture synthesisGlossmask = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/synthesis/gloss_mask");
    public static Texture synthesisGloss = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/synthesis/gloss");

    public static Texture synthesisIcon = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/synthesis");
    public static Texture synthesisHover = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/synthesis_hover");

    public static readonly HexIndex synthesisInput1 = new(0, 0);
    public static readonly HexIndex synthesisInput2 = new(1, 0);
    public static readonly HexIndex synthesisOutput = new(0, 1);

    //Inversion
    public static Texture inversionBase = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/inversion/base");
    public static Texture inversionShadow = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/inversion/shadow");
    public static Texture inversionBottom = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/inversion/bottom");
    public static Texture inversionTop = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/inversion/top");
    public static Texture inversionSymbol = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/inversion/sulfur_symbol");
    public static Texture inversionGlossmask = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/inversion/gloss_mask");
    public static Texture inversionGloss = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/inversion/gloss");

    public static Texture inversionIcon = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/inversion");
    public static Texture inversionHover = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/inversion_hover");

    public static readonly HexIndex inversionOutput = new(0, 0);
    public static readonly HexIndex inversionInput = new(1, 0);

    //Subversion
    public static Texture subversionBase = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/subversion/base");
    public static Texture subversionShadow = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/subversion/shadow");
    public static Texture subversionSymbol = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/subversion/sulfur_symbol");
    public static Texture subversionBowl = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/subversion/bowl");
    public static Texture subversionBowlInput = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/subversion/bowl_input");
    public static Texture subversionGlossmask = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/subversion/gloss_mask");
    public static Texture subversionGloss = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/subversion/gloss");

    public static Texture[] subversionRingFlashAnimation = API.LoadAnimation("textures/primamateria/parts/subversion_ring_flash.array", "flash", 10);
    public static Texture[] subversionRingFlashGlowAnimation = API.LoadAnimation("textures/primamateria/parts/subversion_ring_flash_glow.array", "flash", 10);
    public static Texture[] subversionBowlFlashAnimation = API.LoadAnimation("textures/primamateria/parts/subversion_bowl_flash.array", "flash", 10);
    public static Texture[] subversionBowlFlashGlowAnimation = API.LoadAnimation("textures/primamateria/parts/subversion_bowl_flash_glow.array", "flash", 10);

    public static Texture subversionGlow = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/select/subversion_glow");
    public static Texture subversionStroke = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/select/subversion_stroke");

    public static Texture subversionIcon = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/subversion");
    public static Texture subversionHover = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/subversion_hover");

    public static readonly HexIndex subversionOutputLeft = new(-1, 1);
    public static readonly HexIndex subversionOutputRight = new(1, 0);
    public static readonly HexIndex subversionInput = new(0, -1);
    public static readonly HexIndex subversionBlankHex1 = new(-1, 0);
    public static readonly HexIndex subversionBlankHex2 = new(0, 1);
    public static readonly HexIndex subversionBlankHex3 = new(1, -1);

    //Sublimation
    public static Texture sublimationBase = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/sublimation/base");
    public static Texture sublimationGlow = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/select/sublimation_glow");
    public static Texture sublimationStroke = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/select/sublimation_stroke");
    public static Texture sublimationSymbol = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/sublimation/bowlsprite");
    public static Texture[] sublimationFlashAnimation = API.LoadAnimation("textures/primamateria/parts/sublimation_flash.array", "flash", 10);

    public static Texture sublimationIcon = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/sublimation");
    public static Texture sublimationHover = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/sublimation_hover");
    public static LightDirTextures sublimationLightmap = new LightDirTextures(AssetLoaderHelper.LoadTexture("textures/primamateria/parts/sublimation/left"), AssetLoaderHelper.LoadTexture("textures/primamateria/parts/sublimation/right"), AssetLoaderHelper.LoadTexture("textures/primamateria/parts/sublimation/bottom"), AssetLoaderHelper.LoadTexture("textures/primamateria/parts/sublimation/top"));
    public static readonly HexIndex sublimationCenter = new(0, 0);
    public static readonly HexIndex subEmptyHex1 = new(1, 0);
    public static readonly HexIndex subEmptyHex2 = new(1, -1);
    public static readonly HexIndex subEmptyHex3 = new(0, -1);
    public static readonly HexIndex subEmptyHex4 = new(-1, 0);
    public static readonly HexIndex subEmptyHex5 = new(-1, 1);
    public static readonly HexIndex subEmptyHex6 = new(0, 1);

    //Alignment
    public static Texture alignmentBase = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/alignment/base");
    public static Texture alignmentTop = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/alignment/top");
    public static Texture alignmentSymbol = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/alignment/primae_bowlsymbol");

    public static Texture[] alignmentFlashAnimation = API.LoadAnimation("textures/primamateria/parts/alignment_flash.array", "flash", 10);

    public static Texture alignmentGlow = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/select/double_glow");
    public static Texture alignmentStroke = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/select/double_stroke");

    public static Texture alignmentIcon = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/alignment");
    public static Texture alignmentHover = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/alignment_hover");

    public static readonly HexIndex alignmentPrimeHex = new(0, 0);
    public static readonly HexIndex alignmentPrimaeHex = new(1, 0);

    //Displacement
    public static Texture displacementBase = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/displacement/base");
    public static Texture displacementInput = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/displacement/input");
    public static Texture displacementSymbol = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/displacement/primae_symbol");
    public static Texture displacementTop = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/displacement/top");
    public static Texture displacementGlossmask = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/displacement/gloss_mask");
    public static Texture displacementGloss = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/displacement/gloss");

    public static Texture displacementGlow = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/select/displacement_glow");
    public static Texture displacementStroke = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/select/displacement_stroke");

    public static Texture displacementIcon = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/displacement");
    public static Texture displacementHover = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/displacement_hover");

    public static readonly HexIndex displacementPrimaeHex = new(0, 0);
    public static readonly HexIndex displacementLeftSwapHex = new(-2, 0);
    public static readonly HexIndex displacementRightSwapHex = new(0, 2);

    //Ethereal Bonding
    public static Texture etherealBase = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/ethereal_bonder/base");
    public static Texture etherealLightmap = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/ethereal_bonder/lightmap");
    public static Texture etherealGlossmask = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/ethereal_bonder/gloss_mask");
    public static Texture etherealGloss = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/ethereal_bonder/gloss");

    public static Texture etherealGlow = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/select/triple_inv_glow");
    public static Texture etherealStroke = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/select/triple_inv_stroke");

    public static Texture etherealIcon = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/etherealbonder");
    public static Texture etherealHover = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/etherealbonder_hover");

    public static readonly HexIndex etherealLeft = new(0, 0);
    public static readonly HexIndex etherealRight = new(1, 0);
    public static readonly HexIndex etherealInput = new(1, -1);

    //Affluence
    public static Texture affluenceBase = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/affluence/base");
    public static Texture affluenceShadow = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/affluence/shadow");
    public static Texture affluenceTop = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/affluence/top");
    public static Texture affluenceGlossmask = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/affluence/gloss_mask");
    public static Texture affluenceGloss = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/affluence/gloss");
    public static Texture affluenceGlow = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/select/affluence_glow");
    public static Texture affluenceStroke = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/select/affluence_stroke");

    public static Texture affluencePhosphor = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/affluence/phosphor");
    public static Texture affluenceAntimony = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/affluence/antimony");
    public static Texture affluenceMagnesia = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/affluence/magnesia");
    public static Texture affluencePotash = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/affluence/potash");
    public static Texture affluenceQuicklime = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/affluence/quicklime");

    public static Texture affluenceIcon = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/affluence");
    public static Texture affluenceHover = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/affluence_hover");

    public static readonly HexIndex affluenceSulfurHex = new(0, 0);
    public static readonly HexIndex affluencePhosphorHex = new(-1, 0);
    public static readonly HexIndex affluenceAntimonyHex = new(-1, 1);
    public static readonly HexIndex affluenceMagnesiaHex = new(0, 1);
    public static readonly HexIndex affluencePotashHex = new(1, 0);
    public static readonly HexIndex affluenceQuicklimeHex = new(1, -1);

    //Volatility
    public static Texture[] irisSulfur = API.LoadAnimation("textures/primamateria/parts/iris_full_sulfur.array", "iris", 16);
    public static Texture[] irisQuicklime = API.LoadAnimation("textures/primamateria/parts/iris_full_quicklime.array", "iris", 16);
    public static Texture[] irisPotash = API.LoadAnimation("textures/primamateria/parts/iris_full_potash.array", "iris", 16);
    public static Texture[] irisMagnesia = API.LoadAnimation("textures/primamateria/parts/iris_full_magnesia.array", "iris", 16);
    public static Texture[] irisAntimony = API.LoadAnimation("textures/primamateria/parts/iris_full_antimony.array", "iris", 16);
    public static Texture[] irisPhosphor = API.LoadAnimation("textures/primamateria/parts/iris_full_phosphor.array", "iris", 16);

    public static Texture volatilityBase = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/volatility/base");
    public static Texture volatilityShadow = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/volatility/shadow");
    public static Texture volatilityBottom = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/volatility/bottom");
    public static Texture volatilityTop = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/volatility/top");
    public static Texture volatilityGlossmask = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/volatility/gloss_mask");
    public static Texture volatilityGloss = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/volatility/gloss");
    public static Texture volatilityGlow = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/select/volatility_glow");
    public static Texture volatilityStroke = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/select/volatility_stroke");

    public static Texture volatilityOpalescence = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/volatility/opalescence");

    public static Texture volatilityIcon = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/volatility");
    public static Texture volatilityHover = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/volatility_hover");

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
            cost = 20, // Cost
            isFullHexCover = true, // Is a glyph
            glowTexture = revolutionGlow,
            strokeTexture = revolutionStroke,
            baseTexture = calcicIcon, // Panel icon
            hoverTexture = calcicHover, // Hovered panel icon
            glyphHexes = new HexIndex[]
            {
                calcicMiddle,
                calcicRight,
                calcicLeft
            },
            permissionCategory = PuzzlePermissions.None,
            CustomPermissionCheck = perms => perms.Contains(PrimaMateria.Instance.GetIdentifier("calcicbonder"))
        };
        PrimaMateria.Instance.AddPartType(CalcicBonder, "calcicbonder", (part, pos, editor, renderer) =>
        {
            Texture field1287 = calcicBase;
            Vector2 vector2 = new Vector2(125f, 48f);
            renderer.RenderBase(field1287, new Vector2(0.0f, -1f), vector2, 0.0f);
            foreach (HexIndex hexIndex in part.GetType().glyphHexes)
            {
                renderer.RenderShadowCircle(Assets.textures.parts.bonder_shadow, hexIndex, 4f);
                renderer.RenderRotating(Assets.textures.parts.bonder_ring, hexIndex, Vector2.Zero);
                renderer.RenderUpright(calcicSymbol /*symbol*/, hexIndex, new Vector2(0f, 1f));
            }
            for (int i = 0; i < part.GetType().glyphHexes.Length; i++)
            {
                HexIndex hexIndex = part.GetType().glyphHexes[i];
                if (hexIndex != new HexIndex(0, 0))
                {
                    // render a bonder thingy to it
                    int index = i - 1;
                    float num = new HexRotation(index * 3).ToRadians();
                    renderer.RenderBase(Assets.textures.parts.bonder_bond, new Vector2(-28f, 22f), num);
                    renderer.RenderBonderCylinder(calcicLightmap, calcicCylinder, new HexIndex(0, 0), num);
                }
            }
        });
        Revolution = new()
        {
            cost = 20,
            isFullHexCover = true,
            glowTexture = revolutionGlow,
            strokeTexture = revolutionStroke,
            baseTexture = revolutionIcon,
            hoverTexture = revolutionHover,
            glyphHexes = new HexIndex[]
            {
                revBowl,
                revClockwise,
                revCounterClockwise
            },
            permissionCategory = PuzzlePermissions.None,
            CustomPermissionCheck = perms => perms.Contains(PrimaMateria.Instance.GetIdentifier("revolution"))
        };
        PrimaMateria.Instance.AddPartType(Revolution, "revolution", (part, pos, editor, renderer) =>
        {
            Vector2 offset = new(128f, 52f);
            renderer.RenderBase(revolutionBase, Vector2.Zero, offset, 0f);
            renderer.RenderBase(revolutionTop, Vector2.Zero, offset, 0f);
            renderer.RenderRotating(revolutionBowl, revBowl, Vector2.Zero);
            renderer.RenderUpright(revolutionCWsymbol, revClockwise, Vector2.Zero);
            renderer.RenderUpright(revolutionCCWsymbol, revCounterClockwise, Vector2.Zero);
            renderer.RenderUpright(subversionSymbol, revBowl, Vector2.Zero);
            API.drawPartGloss(renderer, revolutionGloss, revolutionGlossmask, offset);
        });
        Synthesis = new()
        {
            cost = 20,
            isFullHexCover = true,
            glowTexture = Assets.textures.select.triple_glow,
            strokeTexture = Assets.textures.select.triple_stroke,
            baseTexture = synthesisIcon,
            hoverTexture = synthesisHover,
            glyphHexes = new HexIndex[]
            {
                synthesisInput1,
                synthesisInput2,
                synthesisOutput
            },
            permissionCategory = PuzzlePermissions.None,
            CustomPermissionCheck = perms => perms.Contains(PrimaMateria.Instance.GetIdentifier("synthesis"))
        };
        PrimaMateria.Instance.AddPartType(Synthesis, "synthesis", (part, pos, editor, renderer) =>
        {
            PartSimState pss = editor.GetSimulation().GetSimState(part);
            IntermediatePartState uco = editor.GetIntermState(part, pos);
            float time = editor.GetCycleTime();

            Vector2 offset = new(42f, 49f);
            renderer.RenderBase(synthesisBase, Vector2.Zero, offset, 0f);
            renderer.RenderBase(synthesisShadow, Vector2.Zero, offset, 0f);
            renderer.RenderBase(synthesisBottom, Vector2.Zero, offset, 0f);

            int irisFrame = 15;
            bool afterIrisOpens = false;
            Molecule risingMetal = null;
            Vector2 risingOffset = uco.pos + HexGrid.standardGrid.ToPixelCoords(synthesisOutput).Rotated(uco.rotation);
            if (pss.isProcessing)
            {
                irisFrame = Utils.Clamp((int)(Utils.InterpolateLinear(1f, -1f, time) * 16f), 0, 15);
                afterIrisOpens = time > 0.5f;
                risingMetal = Molecule.CreateMonoatomic(pss.processingAtoms[0]);
                if (!afterIrisOpens)
                {
                    // show atom rising behind iris
                    Editor.RenderMolecule(risingMetal, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                }
            }
            renderer.RenderUpright(Assets.textures.parts.iris_full[irisFrame], synthesisOutput, Vector2.Zero);
            renderer.RenderBase(synthesisTop, Vector2.Zero, offset, 0f);
            renderer.RenderUpright(synthesisSymbol, synthesisInput1, Vector2.Zero);
            renderer.RenderUpright(synthesisSymbol, synthesisInput2, Vector2.Zero);
            API.drawPartGloss(renderer, synthesisGloss, synthesisGlossmask, offset);
            if (pss.isProcessing && afterIrisOpens)
            {
                // show atom rising infront of iris
                Editor.RenderMolecule(risingMetal, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }
        });
        Inversion = new()
        {
            cost = 20,
            isFullHexCover = true,
            glowTexture = Assets.textures.select.double_glow,
            strokeTexture = Assets.textures.select.double_stroke,
            baseTexture = inversionIcon,
            hoverTexture = inversionHover,
            glyphHexes = new HexIndex[]
            {
                inversionOutput,
                inversionInput
            },
            permissionCategory = PuzzlePermissions.None,
            CustomPermissionCheck = perms => perms.Contains(PrimaMateria.Instance.GetIdentifier("inversion"))
        };
        PrimaMateria.Instance.AddPartType(Inversion, "inversion", (part, pos, editor, renderer) =>
        {
            PartSimState pss = editor.GetSimulation().GetSimState(part);
            IntermediatePartState uco = editor.GetIntermState(part, pos);
            float time = editor.GetCycleTime();

            Vector2 offset = new(46f, 52f);
            renderer.RenderBase(inversionBase, Vector2.Zero, offset, 0f);
            renderer.RenderBase(inversionShadow, Vector2.Zero, offset, 0f);
            renderer.RenderBase(inversionBottom, Vector2.Zero, offset, 0f);

            int irisFrame = 15;
            bool afterIrisOpens = false;
            Molecule risingMetal = null;
            Vector2 risingOffset = uco.pos + HexGrid.standardGrid.ToPixelCoords(inversionOutput).Rotated(uco.rotation);
            if (pss.isProcessing)
            {
                irisFrame = Utils.Clamp((int)(Utils.InterpolateLinear(1f, -1f, time) * 16f), 0, 15);
                afterIrisOpens = time > 0.5f;
                risingMetal = Molecule.CreateMonoatomic(pss.processingAtoms[0]);
                if (!afterIrisOpens)
                {
                    // show atom rising behind iris
                    Editor.RenderMolecule(risingMetal, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                }
            }
            renderer.RenderUpright(Assets.textures.parts.iris_full[irisFrame], inversionOutput, Vector2.Zero);
            renderer.RenderBase(inversionTop, Vector2.Zero, offset, 0f);
            renderer.RenderUpright(inversionSymbol, inversionInput, Vector2.Zero);
            API.drawPartGloss(renderer, inversionGloss, inversionGlossmask, offset);
            if (pss.isProcessing && afterIrisOpens)
            {
                // show atom rising infront of iris
                Editor.RenderMolecule(risingMetal, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }
        });
        Subversion = new()
        {
            cost = 30,
            isFullHexCover = true,
            glowTexture = subversionGlow,
            strokeTexture = subversionStroke,
            baseTexture = subversionIcon,
            hoverTexture = subversionHover,
            glyphHexes = new HexIndex[]
        {
                subversionOutputLeft,
                subversionOutputRight,
                subversionInput,
                subversionBlankHex1,
                subversionBlankHex2,
                subversionBlankHex3
        },
            permissionCategory = PuzzlePermissions.None,
            CustomPermissionCheck = perms => perms.Contains(PrimaMateria.Instance.GetIdentifier("subversion"))
        };
        PrimaMateria.Instance.AddPartType(Subversion, "subversion", (part, pos, editor, renderer) =>
        {
            PartSimState pss = editor.GetSimulation().GetSimState(part);
            IntermediatePartState uco = editor.GetIntermState(part, pos);
            float time = editor.GetCycleTime();

            Vector2 offset = new(124f, 120f);
            renderer.RenderBase(subversionBase, Vector2.Zero, offset, 0f);
            renderer.RenderBase(subversionShadow, Vector2.Zero, offset, 0f);
            renderer.RenderRotating(subversionBowlInput, subversionInput, Vector2.Zero);
            renderer.RenderUpright(subversionSymbol, subversionInput, Vector2.Zero);
            renderer.RenderUpright(subversionBowl, subversionOutputLeft, Vector2.Zero);
            renderer.RenderUpright(subversionBowl, subversionOutputRight, Vector2.Zero);
            API.drawPartGloss(renderer, subversionGloss, subversionGlossmask, offset);
        });
        Sublimation = new()
        {
            cost = 50,
            isFullHexCover = true,
            glowTexture = sublimationGlow,
            strokeTexture = sublimationStroke,
            baseTexture = sublimationIcon,
            hoverTexture = sublimationHover,
            isUnique = true,
            glyphHexes = new HexIndex[]
            {
                sublimationCenter,
                subEmptyHex1,
                subEmptyHex2,
                subEmptyHex3,
                subEmptyHex4,
                subEmptyHex5,
                subEmptyHex6
            },
            permissionCategory = PuzzlePermissions.None,
            CustomPermissionCheck = perms => perms.Contains(PrimaMateria.Instance.GetIdentifier("sublimation"))
        };
        PrimaMateria.Instance.AddPartType(Sublimation, "sublimation", (part, pos, editor, renderer) =>
        {
            Vector2 offset = new(124f, 119f);
            renderer.RenderBase(sublimationBase, Vector2.Zero, offset, 0f);
            renderer.RenderTrackLights(sublimationLightmap, sublimationCenter, Vector2.Zero, offset, 0f);
            renderer.RenderRotating(bowl, sublimationCenter, Vector2.Zero);
            renderer.RenderUpright(sublimationSymbol, sublimationCenter, Vector2.Zero);
        });
        Alignment = new()
        {
            cost = 30,
            isFullHexCover = true,
            glowTexture = Assets.textures.select.double_glow,
            strokeTexture = Assets.textures.select.double_stroke,
            baseTexture = alignmentIcon,
            hoverTexture = alignmentHover,
            glyphHexes = new HexIndex[]
        {
                alignmentPrimaeHex,
                alignmentPrimeHex
        },
            permissionCategory = PuzzlePermissions.None,
            CustomPermissionCheck = perms => perms.Contains(PrimaMateria.Instance.GetIdentifier("alignment"))
        };
        PrimaMateria.Instance.AddPartType(Alignment, "alignment", (part, pos, editor, renderer) =>
        {
            PartSimState pss = editor.GetSimulation().GetSimState(part);
            IntermediatePartState uco = editor.GetIntermState(part, pos);
            float time = editor.GetCycleTime();

            Vector2 offset = new(42f, 48f);
            renderer.RenderBase(alignmentBase, Vector2.Zero, offset, 0f);
            renderer.RenderRotating(bowl, alignmentPrimeHex, Vector2.Zero);
            renderer.RenderUpright(metalBowl, alignmentPrimaeHex, Vector2.Zero);
            renderer.RenderUpright(alignmentSymbol, alignmentPrimaeHex, Vector2.Zero);
            renderer.RenderUpright(sublimationSymbol, alignmentPrimeHex, Vector2.Zero);
            renderer.RenderBase(alignmentTop, Vector2.Zero, offset, 0f);
        });
        Displacement = new()
        {
            cost = 40,
            isFullHexCover = true,
            glowTexture = displacementGlow,
            strokeTexture = displacementStroke,
            baseTexture = displacementIcon,
            hoverTexture = displacementHover,
            glyphHexes = new HexIndex[]
        {
                displacementPrimaeHex,
                displacementLeftSwapHex,
                displacementRightSwapHex,
                new HexIndex(-1, 0),
                new HexIndex(0, 1)
        },
            permissionCategory = PuzzlePermissions.None,
            CustomPermissionCheck = perms => perms.Contains(PrimaMateria.Instance.GetIdentifier("displacement"))
        };
        PrimaMateria.Instance.AddPartType(Displacement, "displacement", (part, pos, editor, renderer) =>
        {
            PartSimState pss = editor.GetSimulation().GetSimState(part);
            IntermediatePartState uco = editor.GetIntermState(part, pos);
            float time = editor.GetCycleTime();

            Vector2 offset = new(210f, 52f);
            renderer.RenderBase(displacementBase, Vector2.Zero, offset, 0f);
            renderer.RenderRotating(subversionBowl, displacementLeftSwapHex, Vector2.Zero);
            renderer.RenderRotating(subversionBowl, displacementRightSwapHex, Vector2.Zero);
            renderer.RenderUpright(displacementInput, displacementPrimaeHex, Vector2.Zero);
            renderer.RenderUpright(displacementSymbol, displacementPrimaeHex, Vector2.Zero);
            renderer.RenderBase(displacementTop, Vector2.Zero, offset, 0f);
            API.drawPartGloss(renderer, displacementGloss, displacementGlossmask, offset);
        });
        EtherealBonder = new()
        {
            cost = 30,
            isFullHexCover = true,
            glowTexture = etherealGlow,
            strokeTexture = etherealStroke,
            baseTexture = etherealIcon,
            hoverTexture = etherealHover,
            glyphHexes = new HexIndex[]
            {
                etherealLeft,
                etherealRight,
                etherealInput
            },
            permissionCategory = PuzzlePermissions.None,
            CustomPermissionCheck = perms => perms.Contains(PrimaMateria.Instance.GetIdentifier("etherealbonder"))
        };
        PrimaMateria.Instance.AddPartType(EtherealBonder, "etherealbonder", (part, pos, editor, renderer) =>
        {
            renderer.RenderBase(etherealBase, Vector2.Zero, new Vector2(42f, 119f), 0f);
            Vector2 vector2 = new Vector2(125f, 48f);
            renderer.RenderShadowCircle(Assets.textures.parts.bonder_shadow, etherealLeft, 4f);
            renderer.RenderRotating(Assets.textures.parts.bonder_ring, etherealLeft, Vector2.Zero);
            renderer.RenderShadowCircle(Assets.textures.parts.bonder_shadow, etherealRight, 4f);
            renderer.RenderRotating(Assets.textures.parts.bonder_ring, etherealRight, Vector2.Zero);
            renderer.RenderUpright(displacementInput, etherealInput, Vector2.Zero);
            renderer.RenderUpright(displacementSymbol, etherealInput, Vector2.Zero);
            API.drawPartGloss(renderer, etherealGloss, etherealGlossmask, new Vector2(42f, 119f));
            for (int i = 0; i < part.GetType().glyphHexes.Length; i++)
            {
                HexIndex hexIndex = part.GetType().glyphHexes[i];
                if (hexIndex == etherealRight)
                {
                    // render a bonder thingy to it
                    int index = i - 1;
                    float num = new HexRotation(index * 3).ToRadians();
                    renderer.RenderBase(Assets.textures.parts.bonder_bond, new Vector2(-28f, 22f), num);
                    renderer.RenderBonderCylinder(etherealLightmap, Assets.textures.parts.bond_cylinder_pattern, new HexIndex(0, 0), num);
                }
            }
        });

        Affluence = new()
        {
            cost = 20,
            isFullHexCover = true,
            glowTexture = affluenceGlow,
            strokeTexture = affluenceStroke,
            baseTexture = affluenceIcon,
            hoverTexture = affluenceHover,
            glyphHexes = new HexIndex[]
            {
                affluenceSulfurHex,
                affluencePhosphorHex,
                affluenceAntimonyHex,
                affluenceMagnesiaHex,
                affluencePotashHex,
                affluenceQuicklimeHex
            },
            permissionCategory = PuzzlePermissions.None,
            CustomPermissionCheck = perms => perms.Contains(PrimaMateria.Instance.GetIdentifier("affluence"))
        };
        PrimaMateria.Instance.AddPartType(Affluence, "affluence", (part, pos, editor, renderer) =>
        {
            Vector2 offset = new(124f, 119f);
            renderer.RenderBase(affluenceBase, Vector2.Zero, offset, 0f);
            renderer.RenderBase(affluenceShadow, Vector2.Zero, offset, 0f);
            renderer.RenderRotating(subversionBowl, affluenceSulfurHex, Vector2.Zero);
            renderer.RenderBase(affluenceTop, Vector2.Zero, offset, 0f);
            renderer.RenderUpright(affluenceQuicklime, affluenceQuicklimeHex, Vector2.Zero);
            renderer.RenderUpright(affluencePotash, affluencePotashHex, Vector2.Zero);
            renderer.RenderUpright(affluenceMagnesia, affluenceMagnesiaHex, Vector2.Zero);
            renderer.RenderUpright(affluenceAntimony, affluenceAntimonyHex, Vector2.Zero);
            renderer.RenderUpright(affluencePhosphor, affluencePhosphorHex, Vector2.Zero);
            renderer.RenderUpright(subversionSymbol, affluenceSulfurHex, Vector2.Zero);
            API.drawPartGloss(renderer, affluenceGloss, affluenceGlossmask, offset);
        });
        Volatility = new()
        {
            cost = 20, // Cost
            isFullHexCover = true, // Is a glyph
            glowTexture = volatilityGlow,
            strokeTexture = volatilityStroke,
            baseTexture = volatilityIcon, // Panel icon
            hoverTexture = volatilityHover, // Hovered panel icon
            glyphHexes = new HexIndex[]
            {
                volatilityInputHex,
                volatilitySulfurHex,
                volatilityQuicklimeHex,
                volatilityPotashHex,
                volatilityMagnesiaHex,
                volatilityAntimonyHex,
                volatilityPhosphorHex,
            },
            permissionCategory = PuzzlePermissions.None,
            CustomPermissionCheck = perms => perms.Contains(PrimaMateria.Instance.GetIdentifier("volatility"))
        };
        PrimaMateria.Instance.AddPartType(Volatility, "volatility", (part, pos, editor, renderer) =>
        {
            PartSimState pss = editor.GetSimulation().GetSimState(part);
            IntermediatePartState uco = editor.GetIntermState(part, pos);
            float time = editor.GetCycleTime();

            Vector2 offset = new(125f, 190f);
            renderer.RenderBase(volatilityBase, Vector2.Zero, offset, 0f);
            renderer.RenderBase(volatilityShadow, Vector2.Zero, offset, 0f);
            renderer.RenderBase(volatilityBottom, Vector2.Zero, offset, 0f);

            int irisFrame = 15;
            bool afterIrisOpens = false;
            Molecule risingSulfur = null;
            Molecule risingQuicklime = null;
            Molecule risingPotash = null;
            Molecule risingMagnesia = null;
            Molecule risingAntimony = null;
            Molecule risingPhosphor = null;
            Vector2 risingOffsetSulfur = uco.pos + HexGrid.standardGrid.ToPixelCoords(volatilitySulfurHex).Rotated(uco.rotation);
            Vector2 risingOffsetQuicklime = uco.pos + HexGrid.standardGrid.ToPixelCoords(volatilityQuicklimeHex).Rotated(uco.rotation);
            Vector2 risingOffsetPotash = uco.pos + HexGrid.standardGrid.ToPixelCoords(volatilityPotashHex).Rotated(uco.rotation);
            Vector2 risingOffsetMagnesia = uco.pos + HexGrid.standardGrid.ToPixelCoords(volatilityMagnesiaHex).Rotated(uco.rotation);
            Vector2 risingOffsetAntimony = uco.pos + HexGrid.standardGrid.ToPixelCoords(volatilityAntimonyHex).Rotated(uco.rotation);
            Vector2 risingOffsetPhosphor = uco.pos + HexGrid.standardGrid.ToPixelCoords(volatilityPhosphorHex).Rotated(uco.rotation);
            if (pss.isProcessing)
            {
                irisFrame = Utils.Clamp((int)(Utils.InterpolateLinear(1f, -1f, time) * 16f), 0, 15);
                afterIrisOpens = time > 0.5f;
                risingSulfur = Molecule.CreateMonoatomic(pss.processingAtoms[0]);
                risingQuicklime = Molecule.CreateMonoatomic(pss.processingAtoms[1]);
                risingPotash = Molecule.CreateMonoatomic(pss.processingAtoms[2]);
                risingMagnesia = Molecule.CreateMonoatomic(pss.processingAtoms[3]);
                risingAntimony = Molecule.CreateMonoatomic(pss.processingAtoms[4]);
                risingPhosphor = Molecule.CreateMonoatomic(pss.processingAtoms[5]);
                if (!afterIrisOpens)
                {
                    // show atom rising behind iris
                    Editor.RenderMolecule(risingSulfur, risingOffsetSulfur, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                    Editor.RenderMolecule(risingQuicklime, risingOffsetQuicklime, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                    Editor.RenderMolecule(risingPotash, risingOffsetPotash, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                    Editor.RenderMolecule(risingMagnesia, risingOffsetMagnesia, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                    Editor.RenderMolecule(risingAntimony, risingOffsetAntimony, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                    Editor.RenderMolecule(risingPhosphor, risingOffsetPhosphor, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                }
            }
            renderer.RenderUpright(irisSulfur[irisFrame], volatilitySulfurHex, Vector2.Zero); //Render Irises
            renderer.RenderUpright(irisQuicklime[irisFrame], volatilityQuicklimeHex, Vector2.Zero);
            renderer.RenderUpright(irisPotash[irisFrame], volatilityPotashHex, Vector2.Zero);
            renderer.RenderUpright(irisMagnesia[irisFrame], volatilityMagnesiaHex, Vector2.Zero);
            renderer.RenderUpright(irisAntimony[irisFrame], volatilityAntimonyHex, Vector2.Zero);
            renderer.RenderUpright(irisPhosphor[irisFrame], volatilityPhosphorHex, Vector2.Zero);
            renderer.RenderBase(volatilityTop, Vector2.Zero, offset, 0f);
            renderer.RenderUpright(volatilityOpalescence, volatilityInputHex, Vector2.Zero);
            API.drawPartGloss(renderer, volatilityGloss, volatilityGlossmask, offset);
            if (pss.isProcessing && afterIrisOpens)
            {
                // show atom rising infront of iris
                Editor.RenderMolecule(risingSulfur, risingOffsetSulfur, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                Editor.RenderMolecule(risingQuicklime, risingOffsetQuicklime, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                Editor.RenderMolecule(risingPotash, risingOffsetPotash, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                Editor.RenderMolecule(risingMagnesia, risingOffsetMagnesia, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                Editor.RenderMolecule(risingAntimony, risingOffsetAntimony, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                Editor.RenderMolecule(risingPhosphor, risingOffsetPhosphor, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
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

        PrimaMateria.Instance.AddPuzzlePermission("calcicbonder");
        PrimaMateria.Instance.AddPuzzlePermission("revolution");
        PrimaMateria.Instance.AddPuzzlePermission("synthesis");
        PrimaMateria.Instance.AddPuzzlePermission("inversion");
        PrimaMateria.Instance.AddPuzzlePermission("subversion");
        PrimaMateria.Instance.AddPuzzlePermission("sublimation");
        PrimaMateria.Instance.AddPuzzlePermission("alignment");
        PrimaMateria.Instance.AddPuzzlePermission("displacement");
        PrimaMateria.Instance.AddPuzzlePermission("etherealbonder");
        PrimaMateria.Instance.AddPuzzlePermission("affluence");
        PrimaMateria.Instance.AddPuzzlePermission("volatility");

        QApi.BindGlyphCycle(CalcicBonder, new(PartCycleExecutionType.Normal, PartsCycle.CalcicBonderCycle));
        QApi.BindGlyphCycle(Revolution, new(PartCycleExecutionType.Normal, PartsCycle.RevolutionCycle));
        QApi.BindGlyphCycle(Synthesis, new(PartCycleExecutionType.Normal, PartsCycle.SynthesisCycle));
        QApi.BindGlyphCycle(Inversion, new(PartCycleExecutionType.Normal, PartsCycle.InversionCycle));
        QApi.BindGlyphCycle(Subversion, new(PartCycleExecutionType.Normal, PartsCycle.SubversionCycle));
        QApi.BindGlyphCycle(Sublimation, new(PartCycleExecutionType.Normal, PartsCycle.SublimationCycle));
        QApi.BindGlyphCycle(Alignment, new(PartCycleExecutionType.Normal, PartsCycle.AlignmentCycle));
        QApi.BindGlyphCycle(Displacement, new(PartCycleExecutionType.Normal, PartsCycle.DisplacementCycle));
        QApi.BindGlyphCycle(EtherealBonder, new(PartCycleExecutionType.Normal, PartsCycle.EtherealBonderCycle));
        QApi.BindGlyphCycle(Affluence, new(PartCycleExecutionType.Normal, PartsCycle.AffluenceCycle));
        QApi.BindGlyphCycle(Volatility, new(PartCycleExecutionType.Normal, PartsCycle.VolatilityCycle));

        PrimaMateria.Instance.AddRecipe(new(), CalcicBonder.Id, CalcicBonder.Id);
        PrimaMateria.Instance.AddRecipe(new(), Revolution.Id, Revolution.Id);
        PrimaMateria.Instance.AddRecipe(new(), Synthesis.Id, Synthesis.Id);
        PrimaMateria.Instance.AddRecipe(new(), Inversion.Id, Inversion.Id);
        PrimaMateria.Instance.AddRecipe(new(), Subversion.Id, Subversion.Id);
        PrimaMateria.Instance.AddRecipe(new(), Sublimation.Id, Sublimation.Id);
        PrimaMateria.Instance.AddRecipe(new(), Alignment.Id, Alignment.Id);
        PrimaMateria.Instance.AddRecipe(new(), Displacement.Id, Displacement.Id);
        PrimaMateria.Instance.AddRecipe(new(), EtherealBonder.Id, EtherealBonder.Id);
        PrimaMateria.Instance.AddRecipe(new(), Affluence.Id, Affluence.Id);
        PrimaMateria.Instance.AddRecipe(new(), Volatility.Id, Volatility.Id);


    }
}