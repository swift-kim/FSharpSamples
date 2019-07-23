Public Class SampleApplication
    Inherits Xamarin.Forms.Platform.Tizen.FormsApplication

    Protected Overrides Sub OnCreate()
        MyBase.OnCreate()
        LoadApplication(New App())
    End Sub

End Class

Module Program

    Sub Main(args As String())
        Dim app = New SampleApplication()
        Xamarin.Forms.Platform.Tizen.Forms.Init(app)
        app.Run(args)
    End Sub

End Module
