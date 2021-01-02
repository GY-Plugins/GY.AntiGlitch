namespace GY.AntiGlitch

open Rocket.API

type Config() =
    [<DefaultValue>] val mutable DisableForAdmins: bool
    interface IRocketPluginConfiguration with
             override this.LoadDefaults() =
                 this.DisableForAdmins <- true