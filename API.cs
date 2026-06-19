using IL;
using Quintessential;
using System.Collections.Generic;
using System.Reflection;
using static Brimstone.API;
using AtomTypes = class_175;
using BondType = enum_126;

namespace PrimaMateria;

public static class API
{
    //stolen from RM for wheel
    public static MethodInfo PrivateMethod<T>(string method) => typeof(T).GetMethod(method, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
    public struct InversionRecipe
    {
        public InversionRecipe(AtomType input, AtomType output)
        {
            this.input = input;
            this.output = output;
        }
        public AtomType input;
        public AtomType output;
    }
    public struct RevolutionRecipe
    {
        public RevolutionRecipe(AtomType input, AtomType ccwoutput, AtomType cwoutput)
        {
            this.input = input;
            this.ccwoutput = ccwoutput;
            this.cwoutput = cwoutput;
        }

        public AtomType input;
        public AtomType ccwoutput;
        public AtomType cwoutput;
    }

    public struct SubversionRecipe
    {
        public SubversionRecipe(AtomType input, AtomType leftoutput, AtomType rightoutput)
        {
            this.input = input;
            this.leftoutput = leftoutput;
            this.rightoutput = rightoutput;
        }
        public AtomType input;
        public AtomType leftoutput;
        public AtomType rightoutput;
    }

    public struct SynthesisRecipe
    {
        public SynthesisRecipe(AtomType input1, AtomType input2, AtomType output)
        {
            this.input1 = input1;
            this.input2 = input2;
            this.output = output;
        }
        public AtomType input1;
        public AtomType input2;
        public AtomType output;
    }

    // I'm not sure why you'd need this, but I'm making these use the recipe format anyways because its easier.
    // If someone else really wants to add their own volatiles, and by extension their own opalescence recipes, then this will allow it.
    public struct AffluenceRecipe
    {
        public AffluenceRecipe(AtomType sulfur, AtomType quicklime, AtomType potash, AtomType magnesia, AtomType antimony, AtomType phosphor, AtomType opalescence)
        {
            this.sulfur = sulfur;
            this.quicklime = quicklime;
            this.potash = potash;
            this.magnesia = magnesia;
            this.antimony = antimony;
            this.phosphor = phosphor;
            this.opalescence = opalescence;
        }
        public AtomType sulfur;
        public AtomType quicklime;
        public AtomType potash;
        public AtomType magnesia;
        public AtomType antimony;
        public AtomType phosphor;
        public AtomType opalescence;
    }
    public struct VolatilityRecipe
    {
        public VolatilityRecipe(AtomType opalescence, AtomType sulfur, AtomType quicklime, AtomType potash, AtomType magnesia, AtomType antimony, AtomType phosphor)
        {
            this.opalescence = opalescence;
            this.sulfur = sulfur;
            this.quicklime = quicklime;
            this.potash = potash;
            this.magnesia = magnesia;
            this.antimony = antimony;
            this.phosphor = phosphor;
        }
        public AtomType sulfur;
        public AtomType quicklime;
        public AtomType potash;
        public AtomType magnesia;
        public AtomType antimony;
        public AtomType phosphor;
        public AtomType opalescence;
    }


    public struct AtomColor
    {
        public AtomColor(AtomType atom, Color color)
        {
            this.atom = atom;
            this.color = color;
        }
        public AtomType atom;
        public Color color;
    }

    public static List<AtomColor> AtomColorsList = new();
    public static List<AtomType> AcceptedVolatileAtoms = new(); // list of all atoms that are considered "Volatiles", Use this for compat with Subversion duping and Revolution turning
    public static List<AtomType> AcceptedPrimeAtoms = new(); // list of all atoms that are considered "Primes", i.e. Salt, Quicksilver, Sulfur. Use this for compat with Sublimation & Alignment
    public static List<AtomType> AcceptedPrimaMateriaAtoms = new(); //list of atoms that should be able to be duplicated to via Alignment.
    public static List<RevolutionRecipe> RevolutionTransmutation = new(); // Input, CCW output, CW output
    public static List<InversionRecipe> InversionTransmutation = new(); // Input, Output
    public static List<SubversionRecipe> SubversionTransmutation = new(); // Input, Left output, Right output
    public static List<SynthesisRecipe> SynthesisTransmutation = new(); // Input 1, Input 2, Output
    public static List<AffluenceRecipe> AffluenceTransmutation = new(); // Input 1, Input 2, Output
    public static List<VolatilityRecipe> VolatilityTransmutation = new(); // Input 1, Input 2, Output

    public static void AddTransmutations()
    {
        //atom colors (This is used for subversion's colored glow)
        AtomColorsList.Add(new(PrimaMateriaAtoms.Sulfur, new Color(255f/255f, 221f/255f, 109f/255f, 1f)));
        AtomColorsList.Add(new(PrimaMateriaAtoms.Phosphor, new Color(162f/255f, 255f/255f, 96f/255f, 1f)));
        AtomColorsList.Add(new(PrimaMateriaAtoms.Antimony, new Color(158f/255f, 117f/255f, 255f/255f, 1f)));
        AtomColorsList.Add(new(PrimaMateriaAtoms.Magnesia, new Color(255f/255f, 140f/255f, 212f/255f, 1f)));
        AtomColorsList.Add(new(PrimaMateriaAtoms.Potash, new Color(255f/255f, 147f/255f, 114f/255f, 1f)));
        AtomColorsList.Add(new(PrimaMateriaAtoms.Quicklime, new Color(255f/255f, 185f/255f, 107f/255f, 1f)));
        AtomColorsList.Add(new(PrimaMateriaAtoms.Opalescence, new Color(255f / 255f, 209f / 255f, 217f / 255f, 1f)));

        //accepted volatiles
        AcceptedVolatileAtoms.Add(PrimaMateriaAtoms.Sulfur);
        AcceptedVolatileAtoms.Add(PrimaMateriaAtoms.Phosphor);
        AcceptedVolatileAtoms.Add(PrimaMateriaAtoms.Antimony);
        AcceptedVolatileAtoms.Add(PrimaMateriaAtoms.Magnesia);
        AcceptedVolatileAtoms.Add(PrimaMateriaAtoms.Potash);
        AcceptedVolatileAtoms.Add(PrimaMateriaAtoms.Quicklime);

        //accepted primes
        AcceptedPrimeAtoms.Add(Brimstone.API.VanillaAtoms.salt);
        AcceptedPrimeAtoms.Add(Brimstone.API.VanillaAtoms.quicksilver);
        AcceptedPrimeAtoms.Add(PrimaMateriaAtoms.Sulfur);

        //accepted prima materia
        AcceptedPrimaMateriaAtoms.Add(PrimaMateriaAtoms.Primae);

        //revolution recipes
        RevolutionTransmutation.Add(new(PrimaMateriaAtoms.Sulfur, PrimaMateriaAtoms.Quicklime, PrimaMateriaAtoms.Phosphor));
        RevolutionTransmutation.Add(new(PrimaMateriaAtoms.Phosphor, PrimaMateriaAtoms.Sulfur, PrimaMateriaAtoms.Antimony));
        RevolutionTransmutation.Add(new(PrimaMateriaAtoms.Antimony, PrimaMateriaAtoms.Phosphor, PrimaMateriaAtoms.Magnesia));
        RevolutionTransmutation.Add(new(PrimaMateriaAtoms.Magnesia, PrimaMateriaAtoms.Antimony, PrimaMateriaAtoms.Potash));
        RevolutionTransmutation.Add(new(PrimaMateriaAtoms.Potash, PrimaMateriaAtoms.Magnesia, PrimaMateriaAtoms.Quicklime));
        RevolutionTransmutation.Add(new(PrimaMateriaAtoms.Quicklime, PrimaMateriaAtoms.Potash, PrimaMateriaAtoms.Sulfur));

        //inversion recipes
        InversionTransmutation.Add(new(PrimaMateriaAtoms.Sulfur, PrimaMateriaAtoms.Magnesia));
        InversionTransmutation.Add(new(PrimaMateriaAtoms.Phosphor, PrimaMateriaAtoms.Potash));
        InversionTransmutation.Add(new(PrimaMateriaAtoms.Antimony, PrimaMateriaAtoms.Quicklime));
        InversionTransmutation.Add(new(PrimaMateriaAtoms.Magnesia, PrimaMateriaAtoms.Sulfur));
        InversionTransmutation.Add(new(PrimaMateriaAtoms.Potash, PrimaMateriaAtoms.Phosphor));
        InversionTransmutation.Add(new(PrimaMateriaAtoms.Quicklime, PrimaMateriaAtoms.Antimony));

        //subversion recipes
        SubversionTransmutation.Add(new(PrimaMateriaAtoms.Sulfur, PrimaMateriaAtoms.Antimony, PrimaMateriaAtoms.Potash));
        SubversionTransmutation.Add(new(PrimaMateriaAtoms.Phosphor, PrimaMateriaAtoms.Magnesia, PrimaMateriaAtoms.Quicklime));
        SubversionTransmutation.Add(new(PrimaMateriaAtoms.Antimony, PrimaMateriaAtoms.Potash, PrimaMateriaAtoms.Sulfur));
        SubversionTransmutation.Add(new(PrimaMateriaAtoms.Magnesia, PrimaMateriaAtoms.Quicklime, PrimaMateriaAtoms.Phosphor));
        SubversionTransmutation.Add(new(PrimaMateriaAtoms.Potash, PrimaMateriaAtoms.Sulfur, PrimaMateriaAtoms.Antimony));
        SubversionTransmutation.Add(new(PrimaMateriaAtoms.Quicklime, PrimaMateriaAtoms.Phosphor, PrimaMateriaAtoms.Magnesia));

        //synthesis recipes
        SynthesisTransmutation.Add(new(PrimaMateriaAtoms.Sulfur, Brimstone.API.VanillaAtoms.salt, Brimstone.API.VanillaAtoms.quicksilver));
        SynthesisTransmutation.Add(new(Brimstone.API.VanillaAtoms.salt, Brimstone.API.VanillaAtoms.quicksilver, PrimaMateriaAtoms.Sulfur));
        SynthesisTransmutation.Add(new(Brimstone.API.VanillaAtoms.quicksilver, PrimaMateriaAtoms.Sulfur, Brimstone.API.VanillaAtoms.salt));
        //inverted synthesis recipes, for putting the inputs in the opposite slots
        SynthesisTransmutation.Add(new(Brimstone.API.VanillaAtoms.salt, PrimaMateriaAtoms.Sulfur, Brimstone.API.VanillaAtoms.quicksilver));
        SynthesisTransmutation.Add(new(Brimstone.API.VanillaAtoms.quicksilver, Brimstone.API.VanillaAtoms.salt, PrimaMateriaAtoms.Sulfur));
        SynthesisTransmutation.Add(new( PrimaMateriaAtoms.Sulfur, Brimstone.API.VanillaAtoms.quicksilver, Brimstone.API.VanillaAtoms.salt));

        //Opalescence
        AffluenceTransmutation.Add(new(
            PrimaMateriaAtoms.Sulfur,
            PrimaMateriaAtoms.Quicklime,
            PrimaMateriaAtoms.Potash,
            PrimaMateriaAtoms.Magnesia,
            PrimaMateriaAtoms.Antimony,
            PrimaMateriaAtoms.Phosphor,
            PrimaMateriaAtoms.Opalescence
        ));
        VolatilityTransmutation.Add(new(
            PrimaMateriaAtoms.Opalescence,
            PrimaMateriaAtoms.Sulfur,
            PrimaMateriaAtoms.Quicklime,
            PrimaMateriaAtoms.Potash,
            PrimaMateriaAtoms.Magnesia,
            PrimaMateriaAtoms.Antimony,
            PrimaMateriaAtoms.Phosphor
        ));
    }
}