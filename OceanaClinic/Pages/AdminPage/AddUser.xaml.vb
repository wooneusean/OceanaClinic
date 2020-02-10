Imports System.ComponentModel
Imports MaterialDesignThemes.Wpf

Public Class AddUser
    Implements INotifyPropertyChanged
    Public Event _PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Public Sub PropertyChanged(ByVal e As String)
        RaiseEvent _PropertyChanged(Me, New PropertyChangedEventArgs(e))
    End Sub

    Private _allFieldsFilled As Boolean
    Public ReadOnly Property AllFieldsFilled() As Boolean
        Get
            If String.IsNullOrEmpty(txtEmail.Text) Or String.IsNullOrEmpty(txtFirstname.Text) Or String.IsNullOrEmpty(txtLastname.Text) Or String.IsNullOrEmpty(txtPassword.Text) Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property
    Private _outUser As User
    Public Property OutUser() As User
        Get
            Return _outUser
        End Get
        Set(ByVal value As User)
            _outUser = value
        End Set
    End Property
    Sub New(ByRef inUser As User)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _outUser = inUser
        Me.DataContext = Me
    End Sub

    Private Sub txtLastname_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtEmail.TextChanged, txtFirstname.TextChanged, txtLastname.TextChanged, txtPassword.TextChanged
        PropertyChanged(NameOf(AllFieldsFilled))
    End Sub
End Class
