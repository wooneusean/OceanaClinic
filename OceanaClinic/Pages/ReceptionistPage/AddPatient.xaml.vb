Imports System.Text.RegularExpressions
'https://www.codeproject.com/Tips/876349/WPF-Validation-using-INotifyDataErrorInfo INotifyDataErrorInfo
Public Class AddPatient
    Dim ViewModel As New AddPatientViewModel

    Private Sub ValidateIdentity()
        Task.Run(Sub() ViewModel.Validation(NameOf(ViewModel.IdField), ViewModel.IdField, "", "Identity"))
    End Sub
    Private Sub ValidateMobile()
        Task.Run(Sub() ViewModel.Validation(NameOf(ViewModel.MoField), ViewModel.MoField, "Invalid phone number format!", "PhoneNumber"))
    End Sub
    Sub New(ByRef inPatient As Patient)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ViewModel.OutPatient = inPatient
        DataContext = ViewModel
    End Sub
    Private Sub txtFields_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtFirstname.TextChanged, txtLastname.TextChanged, txtAddress.TextChanged,
        txtAllergies.TextChanged, txtEmail.TextChanged, txtHeight.TextChanged, txtWeight.TextChanged
        ViewModel.OnPropertyChanged("AllFieldsFilled")
    End Sub

    Private Sub txtIdentity_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtIdentity.TextChanged
        ValidateIdentity()
    End Sub
    Private Sub txtMobile_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtMobile.TextChanged
        ValidateMobile()
    End Sub
    'https://programmingistheway.wordpress.com/2017/02/17/only-numbers-in-a-wpf-textbox-with-regular-expressions/
    Private Sub OnlyNumeric_PreviewTextInput(sender As Object, e As TextCompositionEventArgs)
        Dim regex As Regex = New Regex("\D")
        e.Handled = regex.IsMatch(e.Text)
    End Sub
    Private Sub Numeric_TextChanged(sender As Object, e As TextChangedEventArgs)
        Dim s As TextBox = sender
        If String.IsNullOrWhiteSpace(s.Text) Then
            s.Text = "0"
        Else
            Dim regex As Regex = New Regex("\D")
            If regex.IsMatch(s.Text) Then
                Dim n As Integer
                If Integer.TryParse(s.Text, n) Then
                    s.Text = n
                Else
                    s.Text = 0
                End If
            End If
        End If
    End Sub
End Class

Class AddPatientViewModel
    Inherits ValidatableObservableObject
#Region "VM Properties"
    Public Property OutPatient() As Patient
    Public Property FnField() As String
        Get
            Return OutPatient.Firstname
        End Get
        Set(ByVal value As String)
            OutPatient.Firstname = value
        End Set
    End Property
    Public Property IdField() As String
        Get
            Return OutPatient.Identity
        End Get
        Set(ByVal value As String)
            OutPatient.Identity = value
        End Set
    End Property
    Public Property LnField() As String
        Get
            Return OutPatient.Lastname
        End Get
        Set(ByVal value As String)
            OutPatient.Lastname = value
        End Set
    End Property
    Public Property AddrField() As String
        Get
            Return OutPatient.Address
        End Get
        Set(ByVal value As String)
            OutPatient.Address = value
        End Set
    End Property
    Public Property AlleField() As String
        Get
            Return OutPatient.Allergies
        End Get
        Set(ByVal value As String)
            OutPatient.Allergies = value
        End Set
    End Property
    Public Property EmField() As String
        Get
            Return OutPatient.Email
        End Get
        Set(ByVal value As String)
            OutPatient.Email = value
        End Set
    End Property
    Public Property HeField() As Integer
        Get
            Return OutPatient.Height
        End Get
        Set(ByVal value As Integer)
            OutPatient.Height = value
        End Set
    End Property
    Public Property WeField() As Integer
        Get
            Return OutPatient.Weight
        End Get
        Set(ByVal value As Integer)
            OutPatient.Weight = value
        End Set
    End Property
    Public Property MoField() As String
        Get
            Return OutPatient.Mobile
        End Get
        Set(ByVal value As String)
            OutPatient.Mobile = value
        End Set
    End Property
    Private _allFieldsFilled As Boolean
    Public ReadOnly Property AllFieldsFilled() As Boolean
        Get
            If String.IsNullOrEmpty(FnField) Or String.IsNullOrEmpty(LnField) Or String.IsNullOrEmpty(AddrField) Or
                String.IsNullOrEmpty(AlleField) Or String.IsNullOrEmpty(EmField) Or String.IsNullOrEmpty(HeField) Or
                String.IsNullOrEmpty(MoField) Or String.IsNullOrEmpty(IdField) Or String.IsNullOrEmpty(WeField) Then
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
            Case "Identity"
                If (String.IsNullOrWhiteSpace(IdField)) Then
                    errorList.Add("IC Number\Passport Number cannot be empty!")
                ElseIf (gVars.dbReception.GetPatientByIdentity(IdField) IsNot Nothing) Then
                    errorList.Add("User with same IC Number\Passport Number already exist in database!")
                End If
            Case "PhoneNumber"
                Dim regex As Regex = New Regex("^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$") 'https://regexr.com/3c53v phone number regex
                If Not regex.IsMatch(propValue) Then
                    errorList.Add(errContent)
                End If
            Case Else
        End Select

        PropertyErrorsDictionary(propName) = errorList
        OnErrorsChanged(propName)
        OnPropertyChanged(NameOf(AllFieldsFilled))
    End Sub
End Class