namespace GY.AntiGlitch

open Rocket.API.Collections
open Rocket.Core.Plugins
open Rocket.Unturned.Chat
open Rocket.Unturned.Player
open SDG.Unturned
open Steamworks
open UnityEngine


type Plugin() =
    inherit RocketPlugin<Config>()
    
    [<DefaultValue>] val mutable Cfg: Config
    override this.Load() =
        this.Cfg = this.Configuration.Instance |> ignore
        
        BarricadeManager.onDeployBarricadeRequested <- DeployBarricadeRequestHandler(fun barricade asset hit point x y z owner group (allow : byref<bool>) -> this.OnDeploy(owner, point, &allow))
        StructureManager.onDeployStructureRequested <- DeployStructureRequestHandler(fun structure asset point x y z owner group (allow : byref<bool>) -> this.OnDeploy(owner, point, &allow))
        // BUG: UnturnedPlayerEvents.add_OnPlayerUpdatePosition(fun player pos -> this.OnPositionUpdated(player, pos))
        
    override this.Unload() =
        BarricadeManager.onDeployBarricadeRequested <- DeployBarricadeRequestHandler(fun barricade asset hit point x y z owner group allow -> ())
        StructureManager.onDeployStructureRequested <- DeployStructureRequestHandler(fun barricade asset point x y z owner group allow -> ())
        // BUG: UnturnedPlayerEvents.remove_OnPlayerUpdatePosition(fun player pos -> this.OnPositionUpdated(player, pos))
    
    member this.OnPositionUpdated(player : UnturnedPlayer, pos : Vector3) =
        if player.IsAdmin && this.Cfg.DisableForAdmins then () else
        if not ((LevelGround.getHeight(pos) - (float32) 1) > pos.y) then () else
        let fixedPos = VectorUtil.CheckPosition(pos)
        player.Teleport(fixedPos, float32(0))
        
    member this.OnDeploy(id : uint64, point : Vector3, allow : byref<bool>) =
        if not (VectorUtil.IsNavArea(point)) then () else
        let player = UnturnedPlayer.FromCSteamID((CSteamID)id)
        UnturnedChat.Say(player, this.Translate("BUILD_NOT_ALLOW", Array.empty), Color.red)
        allow <- false
            
    override this.DefaultTranslations =
         let translation = TranslationList()
         dict["BUILD_NOT_ALLOW", "Строительство в этом регионе запрещено!";] |> Seq.iter(fun kv -> translation.Add(kv.Key, kv.Value))
         translation
    