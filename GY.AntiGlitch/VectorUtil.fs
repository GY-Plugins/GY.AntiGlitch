namespace GY.AntiGlitch

open SDG.Unturned
open UnityEngine

type VectorUtil() =
    static member CheckPosition(pos : Vector3) =
        let mutable hit = RaycastHit()
        if not (Physics.Raycast(pos, Vector3.down, &hit, (float32)2048, RayMasks.WAYPOINT)) then pos else
        let mutable pos = hit.point + Vector3.up
        pos.y <- (float32)1024
        pos
        
    static member IsNavArea(pos : Vector3) =
        let (res : bool, _ : byte) = LevelNavigation.tryGetNavigation(pos)
        res