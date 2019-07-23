namespace CrossXamlTemplate

open Xamarin.Forms
open Xamarin.Forms.Xaml

type MainPage() as this =
    inherit ContentPage()

    do this.LoadFromXaml(typeof<MainPage>) |> ignore
