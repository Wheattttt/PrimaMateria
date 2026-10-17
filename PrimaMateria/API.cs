using Quintessential;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PrimaMateria;

public static class API
{
    public static Texture[] LoadAnimation(string containingFolder, string frameBaseName, int frameCount, int padding = 4)
    {
        Texture[] anim = new Texture[frameCount];
        for (int i = 0; i < frameCount; i++)
        {
            anim[i] = AssetLoaderHelper.LoadTexture(Path.Combine(containingFolder, frameBaseName) + "_" + (i + 1).ToString().PadLeft(padding, '0'));
        }
        return anim;
    }

    public static Vector2 hexGraphicalOffset(HexIndex hex) => HexGrid.standardGrid.ToPixelCoords(hex);
    public static void drawPartGraphic(PartRenderer renderer, Texture tex, Vector2 graphicPivot, float graphicAngle, Vector2 graphicTranslation, Vector2 screenTranslation)
    {
        drawPartGraphicScaled(renderer, tex, graphicPivot, graphicAngle, graphicTranslation, screenTranslation, new Vector2(1f, 1f));
    }

    public static void drawPartGraphicScaled(PartRenderer renderer, Texture tex, Vector2 graphicPivot, float graphicAngle, Vector2 graphicTranslation, Vector2 screenTranslation, Vector2 scaling)
    {
        //for graphicPivot and graphicTranslation, rightwards is the positive-x direction and upwards is the positive-y direction
        //graphicPivot is an absolute position, with (0,0) denoting the bottom-left corner of the texture
        //graphicTranslation is a translation, so (5,-3) means "translate 5 pixels right and 3 pixels down"
        //graphicAngle is measured in radians, and counterclockwise is the positive-angle direction
        //screenTranslation is the final translation applied, so it is not affected by rotations
        Matrix4 matrixScreenPosition = Matrix4.GetTranslation(renderer.partPos.ToVector3(0f));
        Matrix4 matrixTranslateOnScreen = Matrix4.GetTranslation(screenTranslation.ToVector3(0f));
        Matrix4 matrixRotatePart = Matrix4.RotXY(renderer.partRotation);
        Matrix4 matrixTranslateGraphic = Matrix4.GetTranslation(graphicTranslation.ToVector3(0f));
        Matrix4 matrixRotateGraphic = Matrix4.RotXY(graphicAngle);
        Matrix4 matrixPivotOffset = Matrix4.GetTranslation(-graphicPivot.ToVector3(0f));
        Matrix4 matrixScaling = Matrix4.GetScale(scaling.ToVector3(0f));
        Matrix4 matrixTextureSize = Matrix4.GetScale(tex.size.ToVector3(0f));

        Matrix4 matrix4 = matrixScreenPosition * matrixTranslateOnScreen * matrixRotatePart * matrixTranslateGraphic * matrixRotateGraphic * matrixPivotOffset * matrixScaling * matrixTextureSize;
        TextureRenderer.Render(tex, Color.White, matrix4);
    }
    public static void drawPartGloss(PartRenderer renderer, Texture gloss, Texture glossMask, Vector2 offset)
    {
        drawPartGloss(renderer, gloss, glossMask, offset, new HexIndex(0, 0), 0f);
    }
    public static void drawPartGloss(PartRenderer renderer, Texture gloss, Texture glossMask, Vector2 offset, HexIndex hexOffset, float angle)
    {
        TextureRenderer.GetBatcher().shader = Assets.shaderAssets.maskedGloss;
        TextureRenderer.GetBatcher().textures[1] = gloss;
        var hex = new HexIndex(0, 0);
        Vector2 method2001 = 0.0001f * (renderer.partPos + hexGraphicalOffset(hex).Rotated(renderer.partRotation) - 0.5f * InputManager.screenSize);
        TextureRenderer.GetBatcher().texCoords = method2001;
        drawPartGraphic(renderer, glossMask, offset, angle, hexGraphicalOffset(hexOffset), Vector2.Zero);
        TextureRenderer.GetBatcher().shader = TextureRenderer.GetBatcher().defaultSpriteShader;
        TextureRenderer.GetBatcher().textures[1] = Assets.textures.white;
        TextureRenderer.GetBatcher().texCoords = Vector2.Zero;
    }

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
        AtomColorsList.Add(new(Atoms.Sulfur, new Color(255f / 255f, 221f / 255f, 109f / 255f, 1f)));
        AtomColorsList.Add(new(Atoms.Phosphor, new Color(162f / 255f, 255f / 255f, 96f / 255f, 1f)));
        AtomColorsList.Add(new(Atoms.Antimony, new Color(158f / 255f, 117f / 255f, 255f / 255f, 1f)));
        AtomColorsList.Add(new(Atoms.Magnesia, new Color(255f / 255f, 140f / 255f, 212f / 255f, 1f)));
        AtomColorsList.Add(new(Atoms.Potash, new Color(255f / 255f, 147f / 255f, 114f / 255f, 1f)));
        AtomColorsList.Add(new(Atoms.Quicklime, new Color(255f / 255f, 185f / 255f, 107f / 255f, 1f)));
        //AtomColorsList.Add(new(Atoms.Opalescence, new Color(255f / 255f, 209f / 255f, 217f / 255f, 1f)));

        //accepted volatiles
        AcceptedVolatileAtoms.Add(Atoms.Sulfur);
        AcceptedVolatileAtoms.Add(Atoms.Phosphor);
        AcceptedVolatileAtoms.Add(Atoms.Antimony);
        AcceptedVolatileAtoms.Add(Atoms.Magnesia);
        AcceptedVolatileAtoms.Add(Atoms.Potash);
        AcceptedVolatileAtoms.Add(Atoms.Quicklime);

        //accepted primes
        AcceptedPrimeAtoms.Add(AtomTypes.salt);
        AcceptedPrimeAtoms.Add(AtomTypes.quicksilver);
        AcceptedPrimeAtoms.Add(Atoms.Sulfur);

        //accepted prima materia
        AcceptedPrimaMateriaAtoms.Add(Atoms.Primae);

        //revolution recipes
        RevolutionTransmutation.Add(new(Atoms.Sulfur, Atoms.Quicklime, Atoms.Phosphor));
        RevolutionTransmutation.Add(new(Atoms.Phosphor, Atoms.Sulfur, Atoms.Antimony));
        RevolutionTransmutation.Add(new(Atoms.Antimony, Atoms.Phosphor, Atoms.Magnesia));
        RevolutionTransmutation.Add(new(Atoms.Magnesia, Atoms.Antimony, Atoms.Potash));
        RevolutionTransmutation.Add(new(Atoms.Potash, Atoms.Magnesia, Atoms.Quicklime));
        RevolutionTransmutation.Add(new(Atoms.Quicklime, Atoms.Potash, Atoms.Sulfur));

        //inversion recipes
        InversionTransmutation.Add(new(Atoms.Sulfur, Atoms.Magnesia));
        InversionTransmutation.Add(new(Atoms.Phosphor, Atoms.Potash));
        InversionTransmutation.Add(new(Atoms.Antimony, Atoms.Quicklime));
        InversionTransmutation.Add(new(Atoms.Magnesia, Atoms.Sulfur));
        InversionTransmutation.Add(new(Atoms.Potash, Atoms.Phosphor));
        InversionTransmutation.Add(new(Atoms.Quicklime, Atoms.Antimony));

        //subversion recipes
        SubversionTransmutation.Add(new(Atoms.Sulfur, Atoms.Antimony, Atoms.Potash));
        SubversionTransmutation.Add(new(Atoms.Phosphor, Atoms.Magnesia, Atoms.Quicklime));
        SubversionTransmutation.Add(new(Atoms.Antimony, Atoms.Potash, Atoms.Sulfur));
        SubversionTransmutation.Add(new(Atoms.Magnesia, Atoms.Quicklime, Atoms.Phosphor));
        SubversionTransmutation.Add(new(Atoms.Potash, Atoms.Sulfur, Atoms.Antimony));
        SubversionTransmutation.Add(new(Atoms.Quicklime, Atoms.Phosphor, Atoms.Magnesia));

        //synthesis recipes
        SynthesisTransmutation.Add(new(Atoms.Sulfur, AtomTypes.salt, AtomTypes.quicksilver));
        SynthesisTransmutation.Add(new(AtomTypes.salt, AtomTypes.quicksilver, Atoms.Sulfur));
        SynthesisTransmutation.Add(new(AtomTypes.quicksilver, Atoms.Sulfur, AtomTypes.salt));
        //inverted synthesis recipes, for putting the inputs in the opposite slots
        SynthesisTransmutation.Add(new(AtomTypes.salt, Atoms.Sulfur, AtomTypes.quicksilver));
        SynthesisTransmutation.Add(new(AtomTypes.quicksilver, AtomTypes.salt, Atoms.Sulfur));
        SynthesisTransmutation.Add(new(Atoms.Sulfur, AtomTypes.quicksilver, AtomTypes.salt));

        //Opalescence
        AffluenceTransmutation.Add(new(
            Atoms.Sulfur,
            Atoms.Quicklime,
            Atoms.Potash,
            Atoms.Magnesia,
            Atoms.Antimony,
            Atoms.Phosphor,
            Atoms.Opalescence
        ));
        VolatilityTransmutation.Add(new(
            Atoms.Opalescence,
            Atoms.Sulfur,
            Atoms.Quicklime,
            Atoms.Potash,
            Atoms.Magnesia,
            Atoms.Antimony,
            Atoms.Phosphor
        ));
    }
}