namespace CrossXamlTemplate

open Xamarin.Forms
open Xamarin.Forms.Xaml

type App() as this =
    inherit Application()

    do
        this.LoadFromXaml(typeof<App>) |> ignore
        this.MainPage <- MainPage()
