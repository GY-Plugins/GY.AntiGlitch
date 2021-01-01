namespace GY.AntiGlitch

open System
open Rocket.API

type Config() =
    [<DefaultValue>] val mutable DisableForAdmins: bool
    interface IRocketPluginConfiguration with
             member this.LoadDefaults() =
                 this.DisableForAdmins = true |> ignore