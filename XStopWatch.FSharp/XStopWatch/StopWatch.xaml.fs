namespace XStopWatch

open System
open Xamarin.Forms
open Xamarin.Forms.Xaml
open Tizen.Wearable.CircularUI.Forms
open Tizen.System

type NStopWatch = System.Diagnostics.Stopwatch

type State = Started = 0 | Stopped = 1 | Paused = 2

[<AllowNullLiteral>]
type StopWatch() as this =
    inherit CirclePage()

    static let stateProperty = BindableProperty.Create("State", typeof<State>, typeof<StopWatch>, State.Stopped)
    static let allTimeProperty = BindableProperty.Create("AllTime", typeof<TimeSpan>, typeof<StopWatch>, TimeSpan.Zero)
    
    static let TimeToRotation(ts : TimeSpan) =
        (float (ts.Seconds * 6)) + (float ts.Milliseconds) / 1000.0 * 6.0

    let mutable Self : CirclePage = null
    let mutable Timebar : CircleProgressBarSurfaceItem = null
    let mutable RootView : AbsoluteLayout = null
    let mutable RedBar : Image = null
    let mutable BlueBar : Image = null
    let mutable StateLabel : Label = null
    let mutable ResetOrLapLabel : Label = null
    let mutable CueBtn : Image = null

    let lapPressed = Event<struct (TimeSpan * TimeSpan)>()
    let stopPressed = Event<_>()
    
    let _mainStopWatch = NStopWatch()
    let _subStopWatch = NStopWatch()

    do
        this.LoadFromXaml(typeof<StopWatch>) |> ignore
        Self <- this.FindByName<CirclePage>("Self")
        Timebar <- this.FindByName<CircleProgressBarSurfaceItem>("Timebar")
        RootView <- this.FindByName<AbsoluteLayout>("RootView")
        RedBar <- this.FindByName<Image>("RedBar")
        BlueBar <- this.FindByName<Image>("BlueBar")
        StateLabel <- this.FindByName<Label>("StateLabel")
        ResetOrLapLabel <- this.FindByName<Label>("ResetOrLapLabel")
        CueBtn <- this.FindByName<Image>("CueBtn")

        this.Stop()

    static member StateProperty = stateProperty
    static member AllTimeProperty = allTimeProperty

    member this.State
        with get() : State = this.GetValue(stateProperty) :?> State
        and set(value : State) = this.SetValue(stateProperty, value)
    
    member this.AllTime
        with get() : TimeSpan = this.GetValue(allTimeProperty) :?> TimeSpan
        and set(value : TimeSpan) = this.SetValue(allTimeProperty, value)

    [<CLIEvent>]
    member this.LapPressed = lapPressed.Publish
    
    [<CLIEvent>]
    member this.StopPressed = stopPressed.Publish
    
    member this.OnTopEventTapped(sender : Object, args : EventArgs) =
        this.DoSpotAnimation(sender :?> Image)
        match this.State with
            | State.Stopped -> this.Start()
            | State.Started -> this.Pause()
            | State.Paused -> this.Start()
            | _ -> ()

    member this.OnBottomEventTapped(sender : Object, args : EventArgs) =
        this.DoSpotAnimation(sender :?> Image)
        match this.State with
            | State.Started -> this.OnLapPressed()
            | State.Paused -> this.Stop()
            | _ -> ()

    member this.DoSpotAnimation(spot : Image) =
        spot.Opacity <- 1.0
        Device.StartTimer(TimeSpan.FromMilliseconds(80.0), fun () -> spot.Opacity <- 0.0; false)
        
    member this.OnLapPressed() =
        match BlueBar.IsVisible with
            | false -> BlueBar.IsVisible <- true
            | _ -> ()

        _subStopWatch.Reset()
        _subStopWatch.Start()
        BlueBar.Rotation <- 0.0
        
        match CueBtn.AnimationIsRunning("CueAnimation") with
            | false -> this.DoShowCueButton()
            | _ -> ()
        
        match _subStopWatch.IsRunning with
            | true -> lapPressed.Trigger(struct (_mainStopWatch.Elapsed, _subStopWatch.Elapsed))
            | _ -> lapPressed.Trigger(struct (_mainStopWatch.Elapsed, _mainStopWatch.Elapsed))
    
    member this.DoShowCueButton() =
        CueBtn.IsVisible <- true

        let X = CueBtn.TranslationX
        let TX = CueBtn.TranslationX - CueBtn.Width
        let anim = Animation()
        let opacityAnim = Animation((fun (f) -> CueBtn.Opacity <- f), 1.0, 0.2)
        let transfAnim = Animation((fun (f) -> CueBtn.TranslationX <- f), X, TX)
        anim.Add(0.0, 1.0, opacityAnim)
        anim.Add(0.0, 1.0, transfAnim)

        let action (f : float) = fun (f) -> (CueBtn.Opacity <- 1.0; CueBtn.TranslationX <- X)
        anim.Commit(CueBtn, "CueAnimation", uint32 16, uint32 1000, null, Action<float, bool> action)

    member this.Start() =
        this.State <- State.Started
        Device.BeginInvokeOnMainThread(fun () -> Power.RequestLock(PowerLock.Cpu, 0))

        _mainStopWatch.Start()
        match _subStopWatch.ElapsedMilliseconds with
            | x when x > 0L -> _subStopWatch.Start()
            | _ -> ()
            
        Timebar.IsVisible <- true

        Device.StartTimer(TimeSpan.FromMilliseconds(10.0), fun () -> this.OnTimeChanged())
        Device.StartTimer(TimeSpan.FromMilliseconds(16.0), fun () -> this.OnTimeBarAnimate())

    member this.Pause() =
        this.State <- State.Paused

        Device.BeginInvokeOnMainThread(fun () -> Power.ReleaseLock(PowerLock.Cpu))

        _mainStopWatch.Stop()
        _subStopWatch.Stop()
        
    member this.Stop() =
        this.State <- State.Stopped
        _mainStopWatch.Reset()
        _subStopWatch.Reset()
        BlueBar.IsVisible <- false
        CueBtn.IsVisible <- false
        Timebar.IsVisible <- false
        this.OnTimeChanged() |> ignore
        this.OnTimeBarAnimate() |> ignore
        stopPressed.Trigger(EventArgs.Empty)

    member this.OnTimeChanged() =
        this.AllTime <- _mainStopWatch.Elapsed
        this.State = State.Started

    member this.OnTimeBarAnimate() =
        let sec = TimeToRotation(_mainStopWatch.Elapsed)
        let lap = TimeToRotation(_subStopWatch.Elapsed)
        let timeBarValue = (float _mainStopWatch.ElapsedMilliseconds / 600000.0) % 1.0

        RedBar.Rotation <- sec
        BlueBar.Rotation <- lap
        Timebar.Value <- timeBarValue

        this.State = State.Started
