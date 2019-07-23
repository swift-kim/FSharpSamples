namespace XStopWatch

open System
open System.Collections.ObjectModel
open Xamarin.Forms
open Xamarin.Forms.Xaml
open Tizen.Wearable.CircularUI.Forms

type Lap = (int * TimeSpan * TimeSpan)

[<AllowNullLiteral>]
type LapsPage() as this =
    inherit CirclePage()

    static let timeProperty = BindableProperty.Create("Time", typeof<TimeSpan>, typeof<StopWatch>, TimeSpan.Zero)
    static let lapsProperty = BindableProperty.Create("Laps", typeof<ObservableCollection<Lap>>, typeof<LapsPage>)

    let mutable Self : CirclePage = null
    let mutable LapList : CircleListView = null

    do
        this.Laps <- ObservableCollection<Lap>()
        this.LoadFromXaml(typeof<LapsPage>) |> ignore
        Self <- this.FindByName<CirclePage>("Self")
        LapList <- this.FindByName<CircleListView>("LapList")

    static member TimeProperty = timeProperty
    static member LapsProperty = lapsProperty

    member this.Laps
        with get() : ObservableCollection<Lap> = this.GetValue(lapsProperty) :?> ObservableCollection<Lap>
        and set(value : ObservableCollection<Lap>) = this.SetValue(lapsProperty, value)

    member this.Time
        with get() : TimeSpan = this.GetValue(timeProperty) :?> TimeSpan
        and set(value : TimeSpan) = this.SetValue(timeProperty, value)

    member this.AddLap(struct (main, sub) : struct (TimeSpan * TimeSpan)) =
        this.Laps.Add(new Lap(this.Laps.Count + 1, main, sub))

    member this.Reset() =
        this.Laps.Clear()
