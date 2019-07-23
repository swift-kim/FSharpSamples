Imports Xamarin.Forms

Public Class App
    Inherits Application

    Public Sub New()
        Dim stack = New StackLayout With {.VerticalOptions = LayoutOptions.Center}

        stack.Children.Add(
            New Label With {.HorizontalTextAlignment = TextAlignment.Center,
                            .Text = "Welcome to Xamarin.Forms with Visual Basic.NET"}
        )

        MainPage = New ContentPage With {.Content = stack}
    End Sub

End Class
