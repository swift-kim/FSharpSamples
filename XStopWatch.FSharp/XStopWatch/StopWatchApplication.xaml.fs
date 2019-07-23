namespace XStopWatch

open System
open System.ComponentModel
open Xamarin.Forms
open Xamarin.Forms.Xaml

type StopWatchApplication() as this =
    inherit Application()
    
    let mutable RootPage : CarouselPage = null
    let mutable StopWatch : StopWatch = null
    let mutable Laps : LapsPage = null

    let alwaysOnRequest = Event<bool>()

    do
        this.LoadFromXaml(typeof<StopWatchApplication>) |> ignore
        RootPage <- this.FindByName<CarouselPage>("RootPage")
        StopWatch <- this.FindByName<StopWatch>("StopWatch")
        Laps <- this.FindByName<LapsPage>("Laps")

        //TODO: There is a problem with xaml parser when trying to assign LapsPage.Time property.
        //      Thus we temporarily disable it in xaml and set BindingContext manually instead.
        Laps.SetBinding(LapsPage.TimeProperty, "AllTime")
        Laps.BindingContext <- StopWatch
        
        StopWatch.PropertyChanged.Add(fun (e : PropertyChangedEventArgs) ->
            match e.PropertyName with
            | "State" -> alwaysOnRequest.Trigger((StopWatch.State = State.Started))
            | _ -> ()
        )

    [<CLIEvent>]
    member this.AlwaysOnRequest = alwaysOnRequest.Publish

    member this.OnAddLap(s : Object, e : struct (TimeSpan * TimeSpan)) =
        Laps.AddLap(e)

    member this.OnStopLap(s : Object, e : EventArgs) =
        Laps.Reset()
