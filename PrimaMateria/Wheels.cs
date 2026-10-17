using MonoMod.Utils;
using Quintessential;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnstableElements;
using static Quintessential.PartCycleDelegate;

namespace PrimaMateria;

internal static class Wheels
{
    public static PartType TullasWheel, FabienneWheel;
    public static Texture tullasIcon = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/tullas");
    public static Texture tullasHover = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/tullas_hover");
    public static Texture fabienneIcon = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/fabienne");
    public static Texture fabienneHover = AssetLoaderHelper.LoadTexture("textures/primamateria/parts/icons/fabienne_hover");

    public static void AddWheelTypes()
    {
        TullasWheel = new PartType
        {
            baseTexture = tullasIcon, // Panel icon
            hoverTexture = tullasHover, // Hovered panel icon
            cost = 30,
            isUnique = true,
            layer = PartTypeLayer.Top,
            isProgrammable = true,
            attachedAtoms = new Dictionary<HexIndex, AtomType>
            {
                {new HexIndex(0, 1),AtomTypes.quicksilver},
                {new HexIndex(1, 0),AtomTypes.salt},
                {new HexIndex(1, -1),Atoms.Sulfur},
                {new HexIndex(0, -1),AtomTypes.quicksilver},
                {new HexIndex(-1, 0),AtomTypes.salt},
                {new HexIndex(-1, 1),Atoms.Sulfur}
            },
            canRotateInSimulation = true,
            permissionCategory = PuzzlePermissions.None,
            CustomPermissionCheck = perms => perms.Contains(PrimaMateria.Instance.GetIdentifier("tullaswheel"))
        };
        QApi.AddPartTypeToPanel(TullasWheel, false);
        PrimaMateria.Instance.AddPartType(TullasWheel, "tullaswheel", (part, pos, editor, renderer) => {});
        PrimaMateria.Instance.AddPuzzlePermission("tullaswheel");

        FabienneWheel = new PartType
        {
            baseTexture = fabienneIcon, // Panel icon
            hoverTexture = fabienneHover, // Hovered panel icon
            cost = 30,
            isUnique = true,
            layer = PartTypeLayer.Top,
            isProgrammable = true,
            attachedAtoms = new Dictionary<HexIndex, AtomType>
            {
                {new HexIndex(0, 1),Atoms.Potash},
                {new HexIndex(1, 0),Atoms.Quicklime},
                {new HexIndex(1, -1),Atoms.Sulfur},
                {new HexIndex(0, -1),Atoms.Phosphor},
                {new HexIndex(-1, 0),Atoms.Antimony},
                {new HexIndex(-1, 1),Atoms.Magnesia}
            },
            canRotateInSimulation = true,
            permissionCategory = PuzzlePermissions.None,
            CustomPermissionCheck = perms => perms.Contains(PrimaMateria.Instance.GetIdentifier("fabiennewheel"))
        };
        QApi.AddPartTypeToPanel(FabienneWheel, false);
        PrimaMateria.Instance.AddPartType(FabienneWheel, "fabiennewheel", (part, pos, editor, renderer) => { });
        PrimaMateria.Instance.AddPuzzlePermission("fabiennewheel");
    }
}