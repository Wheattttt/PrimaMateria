using PrimaMateria;
using Quintessential;
using System.Linq;
using MonoMod;

public class patch_PeriodicTableScreen : PeriodicTableScreen
{
    public patch_PeriodicTableScreen(bool useTransition) : base(useTransition) { }

    private extern void orig_RenderFrame(float deltaTime);
    private void RenderFrame(float deltaTime)
    {
        orig_RenderFrame(deltaTime);
        Vector2 vector = new Vector2(1516f, 922f);
        Vector2 vector2 = (InputManager.screenSize / 2 - vector / 2 + new Vector2(-2f, -11f)).Rounded();
        TextureRenderer.Render(PrimaMateria.PrimaMateria.periodicTableOverlay, vector2 + new Vector2(83f, 94f));
        // Volatiles
        TextureRenderer.RenderText("_Magnesia_", vector2 + new Vector2(281f, 675f), Assets.fonts.cormorant_12_75, DocumentScreen.textBlack, (TextAlignment)1, 1f, 0.6f, float.MaxValue, float.MaxValue, 0, default(Color), null, int.MaxValue, calculateBounds: false, doRender: true);
        TextureRenderer.RenderText("_Antimony_", vector2 + new Vector2(172f, 612f), Assets.fonts.cormorant_12_75, DocumentScreen.textBlack, (TextAlignment)1, 1f, 0.6f, float.MaxValue, float.MaxValue, 0, default(Color), null, int.MaxValue, calculateBounds: false, doRender: true);
        TextureRenderer.RenderText("_Phosphor_", vector2 + new Vector2(172f, 488f), Assets.fonts.cormorant_12_75, DocumentScreen.textBlack, (TextAlignment)1, 1f, 0.6f, float.MaxValue, float.MaxValue, 0, default(Color), null, int.MaxValue, calculateBounds: false, doRender: true);
        TextureRenderer.RenderText("_Sulfur_", vector2 + new Vector2(281f, 427f), Assets.fonts.cormorant_12_75, DocumentScreen.textBlack, (TextAlignment)1, 1f, 0.6f, float.MaxValue, float.MaxValue, 0, default(Color), null, int.MaxValue, calculateBounds: false, doRender: true);
        TextureRenderer.RenderText("_Quicklime_", vector2 + new Vector2(389f, 488f), Assets.fonts.cormorant_12_75, DocumentScreen.textBlack, (TextAlignment)1, 1f, 0.6f, float.MaxValue, float.MaxValue, 0, default(Color), null, int.MaxValue, calculateBounds: false, doRender: true);
        TextureRenderer.RenderText("_Potash_", vector2 + new Vector2(389f, 612f), Assets.fonts.cormorant_12_75, DocumentScreen.textBlack, (TextAlignment)1, 1f, 0.6f, float.MaxValue, float.MaxValue, 0, default(Color), null, int.MaxValue, calculateBounds: false, doRender: true);

        // Titles
        TextureRenderer.RenderTextVerticalCentered("_The Volatiles_", vector2 + new Vector2(281f, 805f), Assets.fonts.cormorant_22_5, DocumentScreen.textBlack, (TextAlignment)1, 1f, 0.6f, 300f, float.MaxValue, 0, default(Color), null, int.MaxValue);
        TextureRenderer.RenderTextVerticalCentered("_The Cardinals_", vector2 + new Vector2(762f, 805f), Assets.fonts.cormorant_22_5, DocumentScreen.textBlack, (TextAlignment)1, 1f, 0.6f, 300f, float.MaxValue, 0, default(Color), null, int.MaxValue);
        TextureRenderer.RenderTextVerticalCentered("_The Metals_", vector2 + new Vector2(1233f, 805f), Assets.fonts.cormorant_22_5, DocumentScreen.textBlack, (TextAlignment)1, 1f, 0.6f, 300f, float.MaxValue, 0, default(Color), null, int.MaxValue);
    }
}