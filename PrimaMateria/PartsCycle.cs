using MonoMod.Utils;
using PrimaMateria;
using Quintessential;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using static Quintessential.Internal.LegacyPuzzleModel;
using static Quintessential.PartCycleDelegate;
using static StringLoader;
using static TextureAssets;

namespace UnstableElements;

internal static class PartsCycle
{
    public static void AddAtom(Sim sim, Part part, HexIndex offset, AtomType atomType)
    {
        Molecule molecule = new();
        molecule.AddAtom(new Atom(atomType), part.InFrontBy(offset));
        sim.molecules.Add(molecule);
    }
    public static void RemoveAtom(AtomReference atom) => atom.molecule.RemoveAtom(atom.pos);
    public static void ChangeAtom(AtomReference atom, AtomType newType) => atom.molecule.ReplaceAtom(newType, atom.pos);

    public static bool CalcicBonderCycle(Sim sim, Part part, PartSimState simState, GlyphRecipe _, bool isCycleStart)
    {
        // Is every bowl filled
        if (sim.FindAtomRelative(part, Parts.calcicLeft).GetOrDefault(out AtomReference leftAtom)
        && sim.FindAtomRelative(part, Parts.calcicMiddle).GetOrDefault(out AtomReference middleAtom)
        && sim.FindAtomRelative(part, Parts.calcicRight).GetOrDefault(out AtomReference rightAtom_temp))
        {
            // Is at least one of the atoms salt
            if (leftAtom.atomType == AtomTypes.salt
            || middleAtom.atomType == AtomTypes.salt
            || rightAtom_temp.atomType == AtomTypes.salt)
            {
                BondTexture bondTexture = Bonds.Calcic.bondTexture;
                BondEffect bondEffect = new(sim.solutionEditor, bondTexture.bondAnim, 60f, bondTexture.textureOffset);

                Molecule molecule = leftAtom.molecule;
                // merge the middle atom with the left if it is separate
                if (molecule != middleAtom.molecule)
                {
                    sim.molecules.Remove(molecule);
                    sim.molecules.Remove(middleAtom.molecule);

                    molecule = molecule.MergeWith(middleAtom.molecule);
                    sim.molecules.Add(molecule);
                }
                // merge the right atom with the molecule if it is separate
                sim.FindAtomRelative(part, Parts.calcicRight).GetOrDefault(out AtomReference rightAtom);
                if ((molecule != rightAtom.molecule))
                {
                    sim.molecules.Remove(molecule);
                    sim.molecules.Remove(rightAtom.molecule);

                    molecule = molecule.MergeWith(rightAtom.molecule);
                    sim.molecules.Add(molecule);
                }

                // create left bond
                Maybe<Bond> targeted = molecule.GetBondAt(part.InFrontBy(Parts.calcicLeft), part.InFrontBy(Parts.calcicMiddle));
                if (!targeted.HasValue())
                {
                    if (molecule.AddBond(Bonds.Calcic, part.InFrontBy(Parts.calcicLeft), part.InFrontBy(Parts.calcicMiddle), bondEffect))
                    {
                        Vector2 left = Utils.InterpolateVect(
                            HexGrid.standardGrid.ToPixelCoords(part.InFrontBy(Parts.calcicLeft)),
                            HexGrid.standardGrid.ToPixelCoords(part.InFrontBy(Parts.calcicMiddle)), 0.5f);

                        sim.solutionEditor.field_3936.Add(new GlyphEffect(sim.solutionEditor, EffectTimescaleType.RealTime, left, bondTexture.glyphAnim, 30f, Vector2.Zero, 0f));

                        simState.wasActivated = true;
                    }
                }
                // right bond
                Maybe<Bond> targeted2 = molecule.GetBondAt(part.InFrontBy(Parts.calcicMiddle), part.InFrontBy(Parts.calcicRight));
                if (!targeted2.HasValue())
                {
                    if (molecule.AddBond(Bonds.Calcic, part.InFrontBy(Parts.calcicMiddle), part.InFrontBy(Parts.calcicRight), bondEffect))
                    {
                        Vector2 right = Utils.InterpolateVect(
                            HexGrid.standardGrid.ToPixelCoords(part.InFrontBy(Parts.calcicMiddle)),
                            HexGrid.standardGrid.ToPixelCoords(part.InFrontBy(Parts.calcicRight)), 0.5f);

                        sim.solutionEditor.field_3936.Add(new GlyphEffect(sim.solutionEditor, EffectTimescaleType.RealTime, right, bondTexture.glyphAnim, 30f, Vector2.Zero,0f));

                        simState.wasActivated = true;
                    }
                }
                if (!targeted.HasValue() || (!targeted2.HasValue()))
                {
                    bondTexture.bondSound.PlaySound(sim.solutionEditor.method_506());
                }
                return true;
            }
        }
        return false;
    }

    public static bool RevolutionCycle(Sim sim, Part part, PartSimState simState, GlyphRecipe _, bool isCycleStart)
    {
        // Is atom in bowl
        if (sim.FindAtomRelative(part, Parts.revBowl).GetOrDefault(out AtomReference volatileReference))
        {
            if ((sim.FindAtomRelative(part, Parts.revClockwise).GetOrDefault(out AtomReference volatileCW)) | (sim.FindAtomRelative(part, Parts.revCounterClockwise).GetOrDefault(out AtomReference volatileCCW)))
            {
                //check direction so we only do the correct transmutation
                bool isCounterClockwise = false;
                if (volatileCW != null && (volatileCW.inMultiAtomMolecule || volatileCW.isHeldByArm))
                {
                    volatileCW = null;
                }
                if (volatileCCW != null && (volatileCCW.inMultiAtomMolecule || volatileCCW.isHeldByArm))
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
                        if (recipe.input == (volatileReference.atomType))
                        {
                            if (isCounterClockwise)
                            {
                                if (API.AcceptedVolatileAtoms.Contains(volatileCCW.atomType))
                                {
                                    RemoveAtom(volatileCCW);
                                    sim.solutionEditor.consumptionEffects.Add(new ConsumptionEffect(sim.solutionEditor, new AtomReference[] { volatileCCW }));
                                    ChangeAtom(volatileReference, recipe.ccwoutput);
                                    Sounds.glyph_revolution.PlaySound(sim.solutionEditor.method_506());
                                    Color glowcolor = new Color(0f, 0f, 0f, 0f);
                                    foreach (API.AtomColor assignment in API.AtomColorsList)
                                    {
                                        if (assignment.atom == recipe.ccwoutput)
                                        {
                                            glowcolor = assignment.color;
                                        }
                                    }
                                    Vector2 posAsVec = HexGrid.standardGrid.ToPixelCoords(part.InFrontBy(Parts.revBowl));
                                    GlyphEffect glowEffect = new(sim.solutionEditor, (EffectTimescaleType)1, posAsVec, Parts.subversionBowlFlashGlowAnimation, 30f, Vector2.Zero, 0)
                                    {
                                        Color = glowcolor
                                    };
                                    GlyphEffect flashEffect = new(sim.solutionEditor, (EffectTimescaleType)1, posAsVec, Parts.subversionBowlFlashAnimation, 30f, Vector2.Zero, 0);
                                    GlyphEffect atomflashEffect = new(sim.solutionEditor, (EffectTimescaleType)1, posAsVec, Parts.AtomFlashAnimation, 30f, Vector2.Zero, 0);
                                    sim.solutionEditor.glyphEffects.Add(glowEffect);
                                    sim.solutionEditor.glyphEffects.Add(flashEffect);
                                    sim.solutionEditor.glyphEffects.Add(atomflashEffect);
                                    return true;
                                }
                            }
                            else
                            {
                                if (API.AcceptedVolatileAtoms.Contains(volatileCW.atomType))
                                {
                                    RemoveAtom(volatileCW);
                                    sim.solutionEditor.consumptionEffects.Add(new ConsumptionEffect(sim.solutionEditor, new AtomReference[] { volatileCW }));
                                    ChangeAtom(volatileReference, recipe.cwoutput);
                                    Sounds.glyph_revolution.PlaySound(sim.solutionEditor.method_506());
                                    Color glowcolor = new Color(0f, 0f, 0f, 0f);
                                    foreach (API.AtomColor assignment in API.AtomColorsList)
                                    {
                                        if (assignment.atom == recipe.cwoutput)
                                        {
                                            glowcolor = assignment.color;
                                        }
                                    }
                                    Vector2 posAsVec = HexGrid.standardGrid.ToPixelCoords(part.InFrontBy(Parts.revBowl));
                                    GlyphEffect glowEffect = new(sim.solutionEditor, (EffectTimescaleType)1, posAsVec, Parts.subversionBowlFlashGlowAnimation, 30f, Vector2.Zero, 0)
                                    {
                                        Color = glowcolor
                                    }
                                    ;
                                    GlyphEffect flashEffect = new(sim.solutionEditor, (EffectTimescaleType)1, posAsVec, Parts.subversionBowlFlashAnimation, 30f, Vector2.Zero, 0);
                                    GlyphEffect atomflashEffect = new(sim.solutionEditor, (EffectTimescaleType)1, posAsVec, Parts.AtomFlashAnimation, 30f, Vector2.Zero, 0);
                                    sim.solutionEditor.glyphEffects.Add(glowEffect);
                                    sim.solutionEditor.glyphEffects.Add(flashEffect);
                                    sim.solutionEditor.glyphEffects.Add(atomflashEffect);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    public static bool SynthesisCycle(Sim sim, Part part, PartSimState simState, GlyphRecipe _, bool isCycleStart)
    {
        if (isCycleStart)
        {
            // Do both atoms exist
            if (sim.FindAtomRelative(part, Parts.synthesisInput1).GetOrDefault(out AtomReference primeIn1) && sim.FindAtomRelative(part, Parts.synthesisInput2).GetOrDefault(out AtomReference primeIn2))
            {
                if (!sim.FindAtomRelative(part, Parts.synthesisOutput).HasValue())
                {
                    // if atoms aren't being held or consumed
                    if ((!primeIn1.inMultiAtomMolecule && !primeIn1.isHeldByArm) && (!primeIn2.inMultiAtomMolecule && !primeIn2.isHeldByArm))
                    {

                        //check atom type
                        foreach (API.SynthesisRecipe recipe in API.SynthesisTransmutation)
                        {
                            if ((recipe.input1 == (primeIn1.atomType)) && (recipe.input2 == (primeIn2.atomType)))
                            {
                                RemoveAtom(primeIn1);
                                sim.solutionEditor.consumptionEffects.Add(new ConsumptionEffect(sim.solutionEditor, new AtomReference[] { primeIn1 }));
                                RemoveAtom(primeIn2);
                                sim.solutionEditor.consumptionEffects.Add(new ConsumptionEffect(sim.solutionEditor, new AtomReference[] { primeIn2 }));
                                simState.isProcessing = true;
                                simState.processingAtoms = new AtomType[1] { recipe.output };
                                Sounds.glyph_synthesis.PlaySound(sim.solutionEditor.method_506());
                                return true;
                            }
                        }
                    }
                }
            }
        }
        else if (simState.isProcessing)
        {
            AddAtom(sim, part, Parts.synthesisOutput, simState.processingAtoms[0]);
            return true;
        }
        return false;
    }
    public static bool InversionCycle(Sim sim, Part part, PartSimState simState, GlyphRecipe _, bool isCycleStart)
    {
        if (isCycleStart)
        {
            // Does atom exist
            if (sim.FindAtomRelative(part, Parts.inversionInput).GetOrDefault(out AtomReference volatileIn))
            {
                if (!sim.FindAtomRelative(part, Parts.inversionOutput).HasValue())
                {
                    // if atom isn't being held or consumed
                    if (!volatileIn.inMultiAtomMolecule && !volatileIn.isHeldByArm)
                    {

                        //check atom type
                        foreach (API.InversionRecipe recipe in API.InversionTransmutation)
                        {
                            if (recipe.input == (volatileIn.atomType))
                            {
                                RemoveAtom(volatileIn);
                                sim.solutionEditor.consumptionEffects.Add(new ConsumptionEffect(sim.solutionEditor, new AtomReference[] { volatileIn }));
                                simState.isProcessing = true;
                                simState.processingAtoms = new AtomType[1] { recipe.output };
                                Sounds.glyph_inversion.PlaySound(sim.solutionEditor.method_506());
                                return true;
                            }
                        }
                    }
                }
            }
        }
        else if (simState.isProcessing)
        {
            AddAtom(sim, part, Parts.inversionOutput, simState.processingAtoms[0]);
            return true;
        }
        return false;
    }
    public static bool SubversionCycle(Sim sim, Part part, PartSimState simState, GlyphRecipe _, bool isCycleStart)
    {
        bool inputExists = sim.GetAtomReference(part, Parts.subversionInput, new List<Part>(), true).GetOrDefault(out AtomReference volatileReference);
        if (inputExists == true)
        {
            if (sim.FindAtomRelative(part, Parts.subversionOutputLeft).GetOrDefault(out AtomReference volatileLeft) & sim.FindAtomRelative(part, Parts.subversionOutputRight).GetOrDefault(out AtomReference volatileRight))
            {
                //check atom type
                foreach (API.SubversionRecipe recipe in API.SubversionTransmutation)
                {
                    if (recipe.input == (volatileReference.atomType))
                    {
                        if ((recipe.leftoutput != (volatileLeft.atomType)) || (recipe.rightoutput != (volatileRight.atomType)))
                        {
                            if ((API.AcceptedVolatileAtoms.Contains(volatileLeft.atomType)) && (API.AcceptedVolatileAtoms.Contains(volatileRight.atomType)))
                            {
                                IntermediatePartState partInfo = sim.solutionEditor.GetIntermState(part, Vector2.Zero);
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
                                Vector2 leftVec = HexGrid.standardGrid.ToPixelCoords(part.InFrontBy(Parts.subversionOutputLeft));
                                Vector2 centerVec = HexGrid.standardGrid.ToPixelCoords(part.InFrontBy(Parts.sublimationCenter));
                                Vector2 rightVec = HexGrid.standardGrid.ToPixelCoords(part.InFrontBy(Parts.subversionOutputRight));

                                Sounds.glyph_subversion.PlaySound(sim.solutionEditor.method_506());

                                sim.solutionEditor.glyphEffects.Add(new GlyphEffect(sim.solutionEditor, (EffectTimescaleType)1, leftVec, Parts.AtomFlashAnimation, 30f, Vector2.Zero, 0));
                                ChangeAtom(volatileLeft, recipe.leftoutput);
                                sim.solutionEditor.glyphEffects.Add(new GlyphEffect(sim.solutionEditor, (EffectTimescaleType)1, leftVec, Parts.subversionBowlFlashGlowAnimation, 30f, Vector2.Zero, 0){Color = leftcolor});
                                sim.solutionEditor.glyphEffects.Add(new GlyphEffect(sim.solutionEditor, (EffectTimescaleType)1, leftVec, Parts.subversionBowlFlashAnimation, 30f, Vector2.Zero, 0));
                                sim.solutionEditor.glyphEffects.Add(new GlyphEffect(sim.solutionEditor, (EffectTimescaleType)1, centerVec, Parts.subversionRingFlashGlowAnimation, 30f, Vector2.Zero, partInfo.rotation) { Color = leftcolor });
                                sim.solutionEditor.glyphEffects.Add(new GlyphEffect(sim.solutionEditor, (EffectTimescaleType)1, centerVec, Parts.subversionRingFlashAnimation, 30f, Vector2.Zero, partInfo.rotation));
                                
                                sim.solutionEditor.glyphEffects.Add(new GlyphEffect(sim.solutionEditor, (EffectTimescaleType)1, rightVec, Parts.AtomFlashAnimation, 30f, Vector2.Zero, 0));
                                ChangeAtom(volatileRight, recipe.rightoutput);
                                sim.solutionEditor.glyphEffects.Add(new GlyphEffect(sim.solutionEditor, (EffectTimescaleType)1, rightVec, Parts.subversionBowlFlashGlowAnimation, 30f, Vector2.Zero, 0) { Color = rightcolor });
                                sim.solutionEditor.glyphEffects.Add(new GlyphEffect(sim.solutionEditor, (EffectTimescaleType)1, rightVec, Parts.subversionBowlFlashAnimation, 30f, Vector2.Zero, 0));
                                sim.solutionEditor.glyphEffects.Add(new GlyphEffect(sim.solutionEditor, (EffectTimescaleType)1, centerVec, Parts.subversionRingFlashGlowAnimation, 30f, Vector2.Zero, (partInfo.rotation + (sixtyDegrees * 2))) { Color = rightcolor });
                                sim.solutionEditor.glyphEffects.Add(new GlyphEffect(sim.solutionEditor, (EffectTimescaleType)1, centerVec, Parts.subversionRingFlashAnimation, 30f, Vector2.Zero, (partInfo.rotation + (sixtyDegrees * 2))));
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
    public static bool SublimationCycle(Sim sim, Part part, PartSimState simState, GlyphRecipe _, bool isCycleStart)
    {
        // Does atom exist
        if (sim.FindAtomRelative(part, Parts.sublimationCenter).GetOrDefault(out AtomReference PrimeReference))
        {
            if (API.AcceptedPrimeAtoms.Contains(PrimeReference.atomType))
            {
                IntermediatePartState partInfo = sim.solutionEditor.GetIntermState(part, Vector2.Zero);

                Sounds.glyph_sublimation.PlaySound(sim.solutionEditor.method_506());
                ChangeAtom(PrimeReference, Atoms.Primae);
                PrimeReference.atom.transmutationEffect = new TransmutationEffect(sim.solutionEditor, (TransmutationEffectRenderMode)1, PrimeReference.atomType, Assets.textures.atoms.projection_effect, 30f);
                Vector2 centerVec = HexGrid.standardGrid.ToPixelCoords(part.InFrontBy(Parts.revBowl));
                sim.solutionEditor.glyphEffects.Add(new GlyphEffect(sim.solutionEditor, (EffectTimescaleType)1, centerVec, Parts.sublimationFlashAnimation, 30f, Vector2.Zero, partInfo.rotation));
                return true;
            }
        }
        return false;
    }
    public static bool AlignmentCycle(Sim sim, Part part, PartSimState simState, GlyphRecipe _, bool isCycleStart)
    {
        // Does atom exist
        bool inputExists = sim.GetAtomReference(part, Parts.alignmentPrimeHex, new List<Part>(), true).GetOrDefault(out AtomReference PrimeAtomReference);
        if (inputExists == true)
        {
            if (sim.FindAtomRelative(part, Parts.alignmentPrimaeHex).GetOrDefault(out AtomReference PrimaeAtomReference))
            {
                //check atom types
                if (API.AcceptedPrimeAtoms.Contains(PrimeAtomReference.atomType))
                {
                    if (API.AcceptedPrimaMateriaAtoms.Contains(PrimaeAtomReference.atomType))
                    {
                        ChangeAtom(PrimaeAtomReference, PrimeAtomReference.atomType);
                        PrimaeAtomReference.atom.transmutationEffect = new TransmutationEffect(sim.solutionEditor, (TransmutationEffectRenderMode)1, PrimaeAtomReference.atomType, Assets.textures.atoms.projection_effect, 30f);
                        Vector2 centerVec = HexGrid.standardGrid.ToPixelCoords(part.InFrontBy(Parts.alignmentPrimaeHex));
                        sim.solutionEditor.glyphEffects.Add(new GlyphEffect(sim.solutionEditor, (EffectTimescaleType)1, centerVec, Parts.alignmentFlashAnimation, 30f, Vector2.Zero, 0f));
                        Sounds.glyph_alignment.PlaySound(sim.solutionEditor.method_506());
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public static bool DisplacementCycle(Sim sim, Part part, PartSimState simState, GlyphRecipe _, bool isCycleStart)
    {
        // Does primae exist
        if (sim.FindAtomRelative(part, Parts.displacementPrimaeHex).GetOrDefault(out AtomReference PrimaeAtom))
        {
            // Is primae bound or being held
            if (!PrimaeAtom.inMultiAtomMolecule && !PrimaeAtom.isHeldByArm)
            {
                // Are atoms in both bowls
                if ((sim.FindAtomRelative(part, Parts.displacementLeftSwapHex).GetOrDefault(out AtomReference LeftSwapAtom))
                && (sim.FindAtomRelative(part, Parts.displacementRightSwapHex).GetOrDefault(out AtomReference RightSwapAtom)))
                {
                    RemoveAtom(PrimaeAtom);
                    sim.solutionEditor.consumptionEffects.Add(new ConsumptionEffect(sim.solutionEditor, new AtomReference[] { PrimaeAtom }));
                    AtomType LeftSwapAtomTemp = LeftSwapAtom.atomType;
                    ChangeAtom(LeftSwapAtom, RightSwapAtom.atomType);
                    ChangeAtom(RightSwapAtom, LeftSwapAtomTemp);
                    Vector2 leftVec = HexGrid.standardGrid.ToPixelCoords(part.InFrontBy(Parts.displacementLeftSwapHex));
                    Vector2 rightVec = HexGrid.standardGrid.ToPixelCoords(part.InFrontBy(Parts.displacementRightSwapHex));
                    sim.solutionEditor.glyphEffects.Add(new GlyphEffect(sim.solutionEditor, (EffectTimescaleType)1, leftVec, Parts.alignmentFlashAnimation, 30f, Vector2.Zero, 0f));
                    sim.solutionEditor.glyphEffects.Add(new GlyphEffect(sim.solutionEditor, (EffectTimescaleType)1, rightVec, Parts.alignmentFlashAnimation, 30f, Vector2.Zero, 0f));
                    LeftSwapAtom.atom.transmutationEffect = new TransmutationEffect(sim.solutionEditor, (TransmutationEffectRenderMode)1, LeftSwapAtom.atomType, Assets.textures.atoms.projection_effect, 30f);
                    RightSwapAtom.atom.transmutationEffect = new TransmutationEffect(sim.solutionEditor, (TransmutationEffectRenderMode)1, RightSwapAtom.atomType, Assets.textures.atoms.projection_effect, 30f);

                    Sounds.glyph_alignment.PlaySound(sim.solutionEditor.method_506());
                    return true;
                }
            }
        }
        return false;
    }
    public static bool EtherealBonderCycle(Sim sim, Part part, PartSimState simState, GlyphRecipe _, bool isCycleStart)
    {
        // Are both bowls and the input filled
        if (sim.FindAtomRelative(part, Parts.etherealLeft).GetOrDefault(out AtomReference leftAtom)
        && sim.FindAtomRelative(part, Parts.etherealRight).GetOrDefault(out AtomReference rightAtom)
        && sim.FindAtomRelative(part, Parts.etherealInput).GetOrDefault(out AtomReference inputAtom))
        {
            // Is input Primae?
            if (inputAtom.atomType == Atoms.Primae
            // Is Primae not being held or bonded?
            && (!inputAtom.inMultiAtomMolecule && !inputAtom.isHeldByArm)
            // Are bonding atoms the same type?
            && leftAtom.atomType == rightAtom.atomType)
            {
                Maybe<Bond> bondAt = leftAtom.molecule.GetBondAt(leftAtom.pos, rightAtom.pos);
                if (!bondAt.HasValue())
                {
                    RemoveAtom(inputAtom);
                    sim.solutionEditor.consumptionEffects.Add(new ConsumptionEffect(sim.solutionEditor, new AtomReference[] { inputAtom }));
                    BondTexture bondTexture = Bonds.Ethereal.bondTexture;
                    BondEffect bondEffect = new(sim.solutionEditor, bondTexture.bondAnim, 60f, bondTexture.textureOffset);
                    Molecule molecule = leftAtom.molecule;
                    // merge the left atom with the right if it is separate
                    if (molecule != rightAtom.molecule)
                    {
                        sim.molecules.Remove(molecule);
                        sim.molecules.Remove(rightAtom.molecule);

                        molecule = molecule.MergeWith(rightAtom.molecule);
                        sim.molecules.Add(molecule);
                    }
                    // create bond
                    if (molecule.AddBond(Bonds.Ethereal, part.InFrontBy(Parts.etherealLeft), part.InFrontBy(Parts.etherealRight), bondEffect))
                    {
                        Vector2 left = Utils.InterpolateVect(
                            HexGrid.standardGrid.ToPixelCoords(part.InFrontBy(Parts.etherealLeft)),
                            HexGrid.standardGrid.ToPixelCoords(part.InFrontBy(Parts.etherealRight)), 0.5f);

                        sim.solutionEditor.field_3936.Add(new GlyphEffect(sim.solutionEditor, EffectTimescaleType.RealTime, left, bondTexture.glyphAnim, 30f, Vector2.Zero, 0f));

                        simState.wasActivated = true;
                    }
                    bondTexture.bondSound.PlaySound(sim.solutionEditor.method_506());
                    return true;
                }
            }
            
        }
        return false;
    }
    public static bool AffluenceCycle(Sim sim, Part part, PartSimState simState, GlyphRecipe _, bool isCycleStart)
    {
        // are all atoms in the bowls
        if (sim.FindAtomRelative(part, Parts.affluenceSulfurHex).GetOrDefault(out AtomReference bowlAtom) &&
        sim.FindAtomRelative(part, Parts.affluenceQuicklimeHex).GetOrDefault(out AtomReference inputAtom1) &&
        sim.FindAtomRelative(part, Parts.affluencePotashHex).GetOrDefault(out AtomReference inputAtom2) &&
        sim.FindAtomRelative(part, Parts.affluenceMagnesiaHex).GetOrDefault(out AtomReference inputAtom3) &&
        sim.FindAtomRelative(part, Parts.affluenceAntimonyHex).GetOrDefault(out AtomReference inputAtom4) &&
        sim.FindAtomRelative(part, Parts.affluencePhosphorHex).GetOrDefault(out AtomReference inputAtom5))
        {
            bool invalidinput = false; // make sure every input atom is unbound and not being held (except for the center bowl hex!)
            AtomReference[] inputReferences = new AtomReference[5] { inputAtom1, inputAtom2, inputAtom3, inputAtom4, inputAtom5 };
            foreach (AtomReference atom in inputReferences)
            {
                if (atom.inMultiAtomMolecule && atom.isHeldByArm)
                {
                    invalidinput = true;
                    break;
                }
            }
            if (!invalidinput)
            {
                // Check that every atom in a recipe exists
                AtomType[] allTypes = new AtomType[6] { bowlAtom.atomType, inputAtom1.atomType, inputAtom2.atomType, inputAtom3.atomType, inputAtom4.atomType, inputAtom5.atomType };
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
                            RemoveAtom(atom);
                            sim.solutionEditor.consumptionEffects.Add(new ConsumptionEffect(sim.solutionEditor, new AtomReference[] { atom }));
                        }
                        ChangeAtom(bowlAtom, recipe.opalescence);
                        Color glowcolor = new Color(0f, 0f, 0f, 0f);
                        foreach (API.AtomColor assignment in API.AtomColorsList)
                        {
                            if (assignment.atom == recipe.opalescence)
                            {
                                glowcolor = assignment.color;
                            }
                        }
                        Vector2 centerVec = HexGrid.standardGrid.ToPixelCoords(part.InFrontBy(Parts.affluenceSulfurHex));
                        sim.solutionEditor.glyphEffects.Add(new GlyphEffect(sim.solutionEditor, (EffectTimescaleType)1, centerVec, Parts.AtomFlashAnimation, 30f, Vector2.Zero, 0));
                        sim.solutionEditor.glyphEffects.Add(new GlyphEffect(sim.solutionEditor, (EffectTimescaleType)1, centerVec, Parts.subversionBowlFlashGlowAnimation, 30f, Vector2.Zero, 0) { Color = glowcolor });
                        sim.solutionEditor.glyphEffects.Add(new GlyphEffect(sim.solutionEditor, (EffectTimescaleType)1, centerVec, Parts.subversionBowlFlashAnimation, 30f, Vector2.Zero, 0));
                        Sounds.glyph_affluence.PlaySound(sim.solutionEditor.method_506());
                        break;
                    }
                }
            }
        }
        return false;
    }
    public static bool VolatilityCycle(Sim sim, Part part, PartSimState simState, GlyphRecipe _, bool isCycleStart)
    {
        HexIndex[] outputHexes = new HexIndex[6]
                    {
                        Parts.volatilitySulfurHex,
                        Parts.volatilityQuicklimeHex,
                        Parts.volatilityPotashHex,
                        Parts.volatilityMagnesiaHex,
                        Parts.volatilityAntimonyHex,
                        Parts.volatilityPhosphorHex,
                    };
        if (isCycleStart)
        {
            // Does atom exist
            if (sim.FindAtomRelative(part, Parts.volatilityInputHex).GetOrDefault(out AtomReference atomIn))
            {
                bool blocked = false; // check all hexes, from MetalQuintessence
                foreach (HexIndex h in outputHexes)
                {
                    if (sim.FindAtomRelative(part, h).HasValue())
                    {
                        blocked = true;
                        return false;
                    }
                }
                if (!blocked)
                {
                    // if atom isn't being held or consumed
                    if (!atomIn.inMultiAtomMolecule && !atomIn.isHeldByArm)
                    {

                        //check atom type
                        foreach (API.VolatilityRecipe recipe in API.VolatilityTransmutation)
                        {
                            if (recipe.opalescence == (atomIn.atomType))
                            {
                                RemoveAtom(atomIn);
                                sim.solutionEditor.consumptionEffects.Add(new ConsumptionEffect(sim.solutionEditor, new AtomReference[] { atomIn }));
                                simState.isProcessing = true;
                                simState.processingAtoms = new AtomType[6] { recipe.sulfur, recipe.quicklime, recipe.potash, recipe.magnesia, recipe.antimony, recipe.phosphor };
                                Sounds.glyph_volatility.PlaySound(sim.solutionEditor.method_506());
                                return true;
                            }
                        }
                    }
                }
            }
        }
        else if (simState.isProcessing)
        {
            AddAtom(sim, part, Parts.volatilitySulfurHex, simState.processingAtoms[0]);
            AddAtom(sim, part, Parts.volatilityQuicklimeHex, simState.processingAtoms[1]);
            AddAtom(sim, part, Parts.volatilityPotashHex, simState.processingAtoms[2]);
            AddAtom(sim, part, Parts.volatilityMagnesiaHex, simState.processingAtoms[3]);
            AddAtom(sim, part, Parts.volatilityAntimonyHex, simState.processingAtoms[4]);
            AddAtom(sim, part, Parts.volatilityPhosphorHex, simState.processingAtoms[5]);
        }
        return false;
    }
}