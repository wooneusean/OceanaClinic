Imports System.Text.RegularExpressions

Public Class PatientDetails
    Dim ViewModel As New PatientDetailsViewModel
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
        ViewModel.Patient = inPatient
        ViewModel._currentIdentity = inPatient.Identity
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

Class PatientDetailsViewModel
    Inherits ValidatableObservableObject
#Region "VM Properties"
    Public _currentIdentity As String
    Public Property Patient() As Patient
    Public Property FnField() As String
        Get
            Return Patient.Firstname
        End Get
        Set(ByVal value As String)
            Patient.Firstname = value
        End Set
    End Property
    Public Property IdField() As String
        Get
            Return Patient.Identity
        End Get
        Set(ByVal value As String)
            Patient.Identity = value
        End Set
    End Property
    Public Property LnField() As String
        Get
            Return Patient.Lastname
        End Get
        Set(ByVal value As String)
            Patient.Lastname = value
        End Set
    End Property
    Public Property AddrField() As String
        Get
            Return Patient.Address
        End Get
        Set(ByVal value As String)
            Patient.Address = value
        End Set
    End Property
    Public Property AlleField() As String
        Get
            Return Patient.Allergies
        End Get
        Set(ByVal value As String)
            Patient.Allergies = value
        End Set
    End Property
    Public Property EmField() As String
        Get
            Return Patient.Email
        End Get
        Set(ByVal value As String)
            Patient.Email = value
        End Set
    End Property
    Public Property HeField() As Integer
        Get
            Return Patient.Height
        End Get
        Set(ByVal value As Integer)
            Patient.Height = value
        End Set
    End Property
    Public Property WeField() As Integer
        Get
            Return Patient.Weight
        End Get
        Set(ByVal value As Integer)
            Patient.Weight = value
        End Set
    End Property
    Public Property MoField() As String
        Get
            Return Patient.Mobile
        End Get
        Set(ByVal value As String)
            Patient.Mobile = value
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
                Else
                    Dim p As Patient = gVars.dbReception.GetPatientByIdentity(IdField)
                    If (p IsNot Nothing) Then
                        If Not (p.Identity.Equals(_currentIdentity)) Then
                            errorList.Add("User with this IC Number\Passport Number already exists!")
                        End If
                    End If
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