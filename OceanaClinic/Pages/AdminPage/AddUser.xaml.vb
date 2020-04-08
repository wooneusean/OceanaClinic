Imports System.Text.RegularExpressions

Public Class AddUser
    Dim ViewModel As New AddUserViewModel
    Sub New(ByRef inUser As User)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ViewModel.OutUser = inUser
        DataContext = ViewModel
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

Class AddUserViewModel
    Inherits ValidatableObservableObject
#Region "VM Properties"
    Public Property OutUser() As User
    Public Property FnField() As String
        Get
            Return OutUser.Firstname
        End Get
        Set(ByVal value As String)
            OutUser.Firstname = value
        End Set
    End Property
    Public Property LnField() As String
        Get
            Return OutUser.Lastname
        End Get
        Set(ByVal value As String)
            OutUser.Lastname = value
        End Set
    End Property
    Public Property PwField() As String
        Get
            Return OutUser.Password
        End Get
        Set(ByVal value As String)
            OutUser.Password = value
        End Set
    End Property
    Public Property EmField() As String
        Get
            Return OutUser.Email
        End Get
        Set(ByVal value As String)
            OutUser.Email = value
        End Set
    End Property
    Private _allFieldsFilled As Boolean
    Public ReadOnly Property AllFieldsFilled() As Boolean
        Get
            If String.IsNullOrEmpty(FnField) Or String.IsNullOrEmpty(LnField) Or String.IsNullOrEmpty(PwField) Or String.IsNullOrEmpty(EmField) Then
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

        Dim emailRegex As Regex = New Regex("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$")

        Select Case type
            Case "Email"
                If (String.IsNullOrWhiteSpace(propValue)) Then
                    errorList.Add("Email cannot be empty!")
                ElseIf (gVars.dbAdmin.GetUserByEmail(propValue) IsNot Nothing) Then
                    errorList.Add("User with same email already exist in database!")
                ElseIf Not emailRegex.IsMatch(propValue) Then
                    errorList.Add("Email is in wrong format!")
                End If
            Case Else
        End Select

        PropertyErrorsDictionary(propName) = errorList
        OnErrorsChanged(propName)
        OnPropertyChanged(NameOf(AllFieldsFilled))
    End Sub
End Class