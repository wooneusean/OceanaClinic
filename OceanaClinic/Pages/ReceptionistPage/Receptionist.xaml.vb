Public Class Receptionist
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ReceptionistControl.Content = New ReceptionistPage(Me)
    End Sub
End Class
