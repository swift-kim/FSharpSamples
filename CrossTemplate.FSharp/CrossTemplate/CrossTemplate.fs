namespace CrossTemplate

open Xamarin.Forms

type App() as this =
    inherit Application()

    let stack = new StackLayout(VerticalOptions = LayoutOptions.Center)
    do
        stack.Children.Add(
            new Label(HorizontalTextAlignment = TextAlignment.Center,
                      Text = "Welcome to Xamarin.Forms with F#")
        )

        this.MainPage <- new ContentPage(Content = stack)
