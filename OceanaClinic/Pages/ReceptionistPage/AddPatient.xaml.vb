Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks
'https://www.codeproject.com/Tips/876349/WPF-Validation-using-INotifyDataErrorInfo INotifyDataErrorInfo
Public Class AddPatient
    Implements INotifyPropertyChanged, INotifyDataErrorInfo

    Dim errorsDictionary As New Dictionary(Of String, List(Of String))
    Public Event _PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
    Public Event _ErrorsChanged As EventHandler(Of DataErrorsChangedEventArgs) Implements INotifyDataErrorInfo.ErrorsChanged

    Public Sub PropertyChanged(ByVal e As String)
        RaiseEvent _PropertyChanged(Me, New PropertyChangedEventArgs(e))
    End Sub
    Public Sub ErrorsChanged(e As String)
        RaiseEvent _ErrorsChanged(Me, New DataErrorsChangedEventArgs(e))
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
    Private Sub Validate()
        Task.Run(Sub() ValidateIdentity())
    End Sub
    Private Sub ValidateIdentity()
        Dim errorList As List(Of String)
        If errorsDictionary.TryGetValue(NameOf(IdField), errorList) = False Then
            errorList = New List(Of String)
        Else
            errorList.Clear()
        End If

        If (String.IsNullOrWhiteSpace(IdField)) Then
            errorList.Add("IC Number\Passport Number cannot be empty!")
        ElseIf (gVars.dbReception.GetPatientByIdentity(IdField) IsNot Nothing) Then
            errorList.Add("User with same IC Number\Passport Number already exist in database!")
        End If

        errorsDictionary(NameOf(IdField)) = errorList
        ErrorsChanged(NameOf(IdField))
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
            If String.IsNullOrEmpty(txtFirstname.Text) Or String.IsNullOrEmpty(txtLastname.Text) Or String.IsNullOrEmpty(txtAddress.Text) Or
                String.IsNullOrEmpty(txtAllergies.Text) Or String.IsNullOrEmpty(txtEmail.Text) Or String.IsNullOrEmpty(txtHeight.Text) Or
                String.IsNullOrEmpty(txtMobile.Text) Or String.IsNullOrEmpty(txtWeight.Text) Then
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
    Sub New(ByRef inPatient As Patient)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        OutPatient = inPatient
        Me.DataContext = Me
    End Sub
    Private Sub txtFields_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtFirstname.TextChanged, txtLastname.TextChanged, txtAddress.TextChanged,
        txtAllergies.TextChanged, txtEmail.TextChanged, txtHeight.TextChanged, txtMobile.TextChanged, txtWeight.TextChanged
        PropertyChanged(NameOf(AllFieldsFilled))
    End Sub

    Private Sub txtIdentity_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtIdentity.TextChanged
        PropertyChanged(NameOf(AllFieldsFilled))
        Validate()
    End Sub
    'https://programmingistheway.wordpress.com/2017/02/17/only-numbers-in-a-wpf-textbox-with-regular-expressions/
    Private Sub OnlyNumeric_PreviewTextInput(sender As Object, e As TextCompositionEventArgs)
        Dim regex As Regex = New Regex("\D")
        e.Handled = regex.IsMatch(e.Text)
    End Sub
#Region "asdasjdsa"
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
    Public Property AllField() As String
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
#End Region
End Class
