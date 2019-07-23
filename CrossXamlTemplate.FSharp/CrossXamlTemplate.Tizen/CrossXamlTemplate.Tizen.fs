namespace CrossXamlTemplate.Tizen

type CrossXamlTemplate() =
    inherit Xamarin.Forms.Platform.Tizen.FormsApplication()

    override this.OnCreate() =
        base.OnCreate()
        this.LoadApplication(new CrossXamlTemplate.App())

module Program =

    [<EntryPoint>]
    let main args =
        let app = new CrossXamlTemplate()
        Xamarin.Forms.Platform.Tizen.Forms.Init(app)
        app.Run(args)
        0
