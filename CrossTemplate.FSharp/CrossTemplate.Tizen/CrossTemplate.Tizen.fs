namespace CrossTemplate.Tizen

type CrossTemplate() =
    inherit Xamarin.Forms.Platform.Tizen.FormsApplication ()

    override this.OnCreate() =
        base.OnCreate()
        this.LoadApplication(new CrossTemplate.App())

module Program =

    [<EntryPoint>]
    let main args =
        let app = new CrossTemplate()
        Xamarin.Forms.Platform.Tizen.Forms.Init(app)
        app.Run(args)
        0
