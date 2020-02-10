Public Class UserDetails
    Inherits UserControl
    Public Property User() As User
    Sub New(ByRef _user As User)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        User = _user
        Me.DataContext = Me
    End Sub
End Class

