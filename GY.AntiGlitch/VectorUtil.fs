namespace GY.AntiGlitch

open SDG.Unturned
open UnityEngine

type VectorUtil() =
    static member IsNavArea(pos : Vector3) =
        let (res : bool, _ : byte) = LevelNavigation.tryGetNavigation(pos)
        res