namespace XStopWatch

open Tizen.Wearable.CircularUI.Forms

type XStopWatch() =
    inherit Xamarin.Forms.Platform.Tizen.FormsApplication()

    override this.OnCreate() =
        base.OnCreate()
        let stopWatch = StopWatchApplication()
        this.LoadApplication(stopWatch)

        stopWatch.AlwaysOnRequest.Add(fun (e) -> WindowExtension.SetAlwaysOn(this.MainWindow, e))

module Program =

    [<EntryPoint>]
    let main args =
        let app = new XStopWatch()
        Xamarin.Forms.Platform.Tizen.Forms.Init(app)
        Renderer.FormsCircularUI.Init()
        app.Run(args)
        0
