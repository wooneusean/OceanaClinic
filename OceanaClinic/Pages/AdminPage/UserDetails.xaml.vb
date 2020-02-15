Public Class UserDetails
    Dim ViewModel As New UserDetailsViewModel
    Sub New(ByRef _user As User)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ViewModel.User = _user
        ViewModel._currentEmail = _user.Email
        Me.DataContext = ViewModel
    End Sub
    Private Sub ValidateEmail()
        Task.Run(Sub() ViewModel.Validation(NameOf(ViewModel.EmField), ViewModel.EmField, "", "Email"))
    End Sub
    Private Sub txtFields_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtFirstname.TextChanged, txtLastname.TextChanged, txtPassword.TextChanged
        ViewModel.OnPropertyChanged("AllFieldsFilled")
    End Sub
    Private Sub txtEmail_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtEmail.TextChanged
        ValidateEmail()
    End Sub
End Class

Public Class UserDetailsViewModel
    Inherits ValidatableObservableObject
#Region "VM Properties"
    Public _currentEmail As String
    Public Property User() As User
    Public Property EmField() As String
        Get
            Return User.Email
        End Get
        Set(ByVal value As String)
            User.Email = value
        End Set
    End Property
    Public Property FnField() As String
        Get
            Return User.Firstname
        End Get
        Set(ByVal value As String)
            User.Firstname = value
        End Set
    End Property
    Public Property LnField() As String
        Get
            Return User.Lastname
        End Get
        Set(ByVal value As String)
            User.Lastname = value
        End Set
    End Property
    Public Property PwField() As String
        Get
            Return User.Password
        End Get
        Set(ByVal value As String)
            User.Password = value
        End Set
    End Property
    Private _allFieldsFilled As Boolean
    Public ReadOnly Property AllFieldsFilled() As Boolean
        Get
            If String.IsNullOrEmpty(EmField) Or String.IsNullOrEmpty(FnField) Or String.IsNullOrEmpty(LnField) Or
                String.IsNullOrEmpty(PwField) Then
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
#End Region
    Public Overrides Sub Validation(propName As String, ByRef propValue As String, errContent As String, type As String)
        Dim errorList As List(Of String)
        If PropertyErrorsDictionary.TryGetValue(propName, errorList) = False Then
            errorList = New List(Of String)
        Else
            errorList.Clear()
        End If

        Select Case type
            Case "Email"
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
            Case Else
        End Select

        PropertyErrorsDictionary(propName) = errorList
        OnErrorsChanged(propName)
        OnPropertyChanged(NameOf(AllFieldsFilled))
    End Sub
End Class