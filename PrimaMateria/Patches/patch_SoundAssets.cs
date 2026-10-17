using MonoMod;
using PrimaMateria;
using Quintessential;
using System.Linq;
using System.Runtime.Intrinsics.X86;

public class patch_SoundAssets : SoundAssets
{
    public patch_SoundAssets() : base() {}

    private extern void orig_ResetPlayingSounds();
    private void ResetPlayingSounds()
    {
        orig_ResetPlayingSounds();
        Sounds.glyph_calcicbonder.isPlaying = false;
        Sounds.glyph_etherealbonder.isPlaying = false;
        Sounds.glyph_affluence.isPlaying = false;
        Sounds.glyph_alignment.isPlaying = false;
        Sounds.glyph_inversion.isPlaying = false;
        Sounds.glyph_revolution.isPlaying = false;
        Sounds.glyph_sublimation.isPlaying = false;
        Sounds.glyph_subversion.isPlaying = false;
        Sounds.glyph_synthesis.isPlaying = false;
        Sounds.glyph_volatility.isPlaying = false;
    }
}