namespace NUITemplate

open Tizen.NUI
open Tizen.NUI.BaseComponents

type NUITemplate() =
    inherit NUIApplication()

    override this.OnCreate() =
        base.OnCreate()
        this.Initialize()

    member this.Initialize() =
        Window.Instance.KeyEvent.Add(this.OnKeyEvent)

        let text = new TextLabel("Welcome to Tizen NUI")
        text.HorizontalAlignment <- HorizontalAlignment.Center
        text.VerticalAlignment <- VerticalAlignment.Center
        text.TextColor <- Color.White
        text.PointSize <- 7.5f
        text.HeightResizePolicy <- ResizePolicyType.FillToParent
        text.WidthResizePolicy <- ResizePolicyType.FillToParent
        Window.Instance.GetDefaultLayer().Add(text)

        let animation = new Animation(2000)
        animation.AnimateTo(text, "Orientation", new Rotation(new Radian(new Degree(180.0f)), Vector3.XAxis), 0, 500)
        animation.AnimateTo(text, "Orientation", new Rotation(new Radian(new Degree(0.0f)), Vector3.XAxis), 500, 1000)
        animation.Looping <- true
        animation.Play()

    member this.OnKeyEvent(e : Window.KeyEventArgs) =
        match e.Key.State, e.Key.KeyPressedName with
            | Key.StateType.Down, ("XF86Back" | "Escape") -> this.Exit()
            | _ -> ()

module Program =

    [<EntryPoint>]
    let main args =
        let app = new NUITemplate()
        app.Run(args)
        0
