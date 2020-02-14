Imports System.ComponentModel

Public Class UserDetails
    Implements INotifyPropertyChanged, INotifyDataErrorInfo
    Public Event _PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
    Public Event _ErrorsChanged As EventHandler(Of DataErrorsChangedEventArgs) Implements INotifyDataErrorInfo.ErrorsChanged
    Dim errorsDictionary As New Dictionary(Of String, List(Of String))

    Public Sub ErrorsChanged(e As String)
        RaiseEvent _ErrorsChanged(Me, New DataErrorsChangedEventArgs(e))
        PropertyChanged(NameOf(AllFieldsFilled))
    End Sub
    Public Sub PropertyChanged(ByVal e As String)
        RaiseEvent _PropertyChanged(Me, New PropertyChangedEventArgs(e))
    End Sub
    Public ReadOnly Property HasErrors As Boolean Implements INotifyDataErrorInfo.HasErrors
        Get
            Try
                Dim errorCount = errorsDictionary.Values.FirstOrDefault().Count
                If errorCount > 0 Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception

            End Try
            Return True
        End Get
    End Property
    Private _allFieldsFilled As Boolean
    Public ReadOnly Property AllFieldsFilled() As Boolean
        Get
            If String.IsNullOrEmpty(txtEmail.Text) Or String.IsNullOrEmpty(txtFirstname.Text) Or String.IsNullOrEmpty(txtLastname.Text) Or
                String.IsNullOrEmpty(txtPassword.Text) Then
                Return False
            Else
                If HasErrors Then
                    Return False
                Else
                    Return True
                End If
            End If
        End Get
    End Property
    Private Sub Validate()
        Task.Run(Sub() ValidateEmail())
    End Sub
    Private Sub ValidateEmail()
        Dim errorList As List(Of String)
        If errorsDictionary.TryGetValue(NameOf(EmField), errorList) = False Then
            errorList = New List(Of String)
        Else
            errorList.Clear()
        End If

        If (String.IsNullOrWhiteSpace(EmField)) Then
            errorList.Add("Email cannot be empty!")
        Else
            Dim u As User = gVars.dbAdmin.GetUserByEmail(EmField)
            If (u IsNot Nothing) Then
                If Not (u.Email.Equals(_currentEmail)) Then
                    errorList.Add("User with this email already exists!")
                End If
            End If
        End If

        errorsDictionary(NameOf(EmField)) = errorList
        ErrorsChanged(NameOf(EmField))
    End Sub
    Public Function GetErrors(e As String) As IEnumerable Implements INotifyDataErrorInfo.GetErrors
        Dim errors As New List(Of String)
        If (Not String.IsNullOrWhiteSpace(e)) Then
            Dim outErrors As New List(Of String)
            errorsDictionary.TryGetValue(e, outErrors)
            Return outErrors
        Else
            Return Nothing
        End If
    End Function
    Public Property EmField() As String
        Get
            Return User.Email
        End Get
        Set(ByVal value As String)
            User.Email = value
        End Set
    End Property
    Public Property User() As User
    Private _currentEmail As String
    Sub New(ByRef _user As User)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        User = _user
        _currentEmail = _user.Email
        Me.DataContext = Me
    End Sub

    Private Sub txtFields_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtFirstname.TextChanged, txtLastname.TextChanged, txtPassword.TextChanged
        PropertyChanged(NameOf(AllFieldsFilled))
    End Sub

    Private Sub txtEmail_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtEmail.TextChanged
        PropertyChanged(NameOf(AllFieldsFilled))
        Validate()
    End Sub
End Class

