using Quintessential;
using System.IO;

namespace PrimaMateria;

internal class PrimaMateria : QuintessentialMod, IDataMod {
    public static PrimaMateria Instance { get; }
    public static Texture periodicTableOverlay;
    public override string ModId => "primamateria";

    public override void Load() { }

    public override void LoadContent() {

        Atoms.AddAtomTypes();
        Parts.AddPartTypes();
        Bonds.AddBondTypes();
        Wheels.AddWheelTypes();
        Sounds.AddSounds();
        API.AddTransmutations();
        periodicTableOverlay = AssetLoaderHelper.LoadTexture("textures/primamateria/periodic_table/overlay");
    }
    public override void LoadCompatContent() { }
    public override void FinaliseContent() { }

    public override void PostLoad() { }
	public override void Unload() { }
}
