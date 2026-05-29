using Brimstone;
using Quintessential;

namespace PrimaMateria;

public static class PrimaMateriaAtoms
{
    public static AtomType Sulfur, Phosphor, Antimony, Magnesia, Potash, Quicklime, Primae;

    public static void AddAtomTypes()
    {
        // Make quicksilver use custom skin - because of Sulfur, which is a third "prime" element with a yellower tint, it makes sense to deviate Quicksilver from Salt as well.
        class_175.field_1680.field_2290.field_994 = Brimstone.API.GetTexture("textures/atoms/PrimaMateria/quicksilver_diffuse");
        class_175.field_1680.field_2290.field_995 = Brimstone.API.GetTexture("textures/atoms/PrimaMateria/quicksilver_shade");
        class_175.field_1680.field_2287 = Brimstone.API.GetTexture("textures/atoms/PrimaMateria/quicksilver_symbol");

        // Replace the vanilla periodic table
        //class_238.field_1989.field_91.field_799 = Brimstone.API.GetTexture("textures/periodic_table/background");

        // Volatiles
        Sulfur = Brimstone.API.CreateNormalAtom(
            ID: 150, 
            modName: "PrimaMateria",
            name: "Sulfur",
            pathToSymbol: "textures/atoms/PrimaMateria/sulfur_symbol",
            pathToDiffuse: "textures/atoms/PrimaMateria/sulfur_diffuse",
            pathToShade: "textures/atoms/PrimaMateria/sulfur_shade"
        );
        Phosphor = Brimstone.API.CreateNormalAtom(
            ID: 151,
            modName: "PrimaMateria",
            name: "Phosphor",
            pathToSymbol: "textures/atoms/PrimaMateria/phosphorus_symbol",
            pathToDiffuse: "textures/atoms/PrimaMateria/phosphorus_diffuse",
            pathToShade: "textures/atoms/PrimaMateria/phosphorus_shade",
            pathToShadow: "textures/atoms/PrimaMateria/phosphorus_shadow"
        );
        Antimony = Brimstone.API.CreateNormalAtom(
            ID: 152,
            modName: "PrimaMateria",
            name: "Antimony",
            pathToSymbol: "textures/atoms/PrimaMateria/antimony_symbol",
            pathToDiffuse: "textures/atoms/PrimaMateria/antimony_diffuse",
            pathToShade: "textures/atoms/PrimaMateria/antimony_shade"
        );
        Magnesia = Brimstone.API.CreateNormalAtom(
            ID: 153,
            modName: "PrimaMateria",
            name: "Magnesia",
            pathToSymbol: "textures/atoms/PrimaMateria/magnesia_symbol",
            pathToDiffuse: "textures/atoms/PrimaMateria/magnesia_diffuse",
            pathToShade: "textures/atoms/PrimaMateria/magnesia_shade"
        );
        Potash = Brimstone.API.CreateNormalAtom(
            ID: 154,
            modName: "PrimaMateria",
            name: "Potash",
            pathToSymbol: "textures/atoms/PrimaMateria/potash_symbol",
            pathToDiffuse: "textures/atoms/PrimaMateria/potash_diffuse",
            pathToShade: "textures/atoms/PrimaMateria/potash_shade"
        );
        Quicklime = Brimstone.API.CreateNormalAtom(
            ID: 155,
            modName: "PrimaMateria",
            name: "Quicklime",
            pathToSymbol: "textures/atoms/PrimaMateria/quicklime_symbol",
            pathToDiffuse: "textures/atoms/PrimaMateria/quicklime_diffuse",
            pathToShade: "textures/atoms/PrimaMateria/quicklime_shade"
        );

        // Primae
        Primae = Brimstone.API.CreateNormalAtom(
            ID: 161,
            modName: "PrimaMateria",
            name: "Primae",
            pathToSymbol: "textures/atoms/PrimaMateria/primae_symbol",
            pathToDiffuse: "textures/atoms/PrimaMateria/primae_diffuse",
            pathToShade: "textures/atoms/PrimaMateria/primae_shade",
            pathToShadow: "textures/atoms/PrimaMateria/primae_shadow"
        );

        QApi.AddAtomType(Sulfur);
        QApi.AddAtomType(Phosphor);
        QApi.AddAtomType(Antimony);
        QApi.AddAtomType(Magnesia);
        QApi.AddAtomType(Potash);
        QApi.AddAtomType(Quicklime);
        QApi.AddAtomType(Primae);
    }
}