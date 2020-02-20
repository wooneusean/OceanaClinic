Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports MaterialDesignThemes.Wpf

Public Class BillingPage
    ''' <summary>
    ''' TO DO:
    ''' 1) MAKE dgItems SHOW CERTAIN COLUMS
    ''' 2) ADD ALL COLUMNS TO TRANSACTION CLASS
    ''' 3) ADD A "Total Price" COLUMN = QUANTITY * PRICE
    ''' 4) FIND A WAY TO ADD TRANSACTION -> INSERT INTO Transactions(ItemId,PatientId,Quantity)
    ''' </summary>
    Dim msgQ As New SnackbarMessageQueue(TimeSpan.FromSeconds(3))
    Dim _transactions As ObservableTransactions
    Dim ViewModel As New BillingPageViewModel
    Dim MainPage As Receptionist
    Sub New(ByRef _mainPage As Receptionist, Optional ByVal patientId As Integer = 0)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _transactions = Me.Resources("transactions")
        MainPage = _mainPage
        ViewModel.PatientId = patientId
        ViewModel.Payment = 0
        RefreshTransactions()
        DataContext = ViewModel
        MySnackbar.MessageQueue = msgQ
    End Sub
    Public Sub RefreshTransactions()
        _transactions.Clear()
        Dim _newNetTotal As Decimal = 0
        For Each transaction As Transaction In gVars.dbReception.GetPatientTransactions(ViewModel.PatientId)
            _transactions.Add(transaction)
            _newNetTotal += transaction.PricePerUnit.Value * transaction.Quantity
        Next
        Dim p As Patient = gVars.dbReception.GetPatientById(ViewModel.PatientId)
        If (p IsNot Nothing) Then
            ViewModel.PatientIdentity = p.Identity
            ViewModel.PatientName = p.Firstname + " " + p.Lastname
        Else
            ViewModel.PatientIdentity = ""
            ViewModel.PatientName = ""
        End If
        ViewModel.NetTotal = New Currency(_newNetTotal)
    End Sub
    Public Sub CollectionViewSource_Filter(sender As Object, e As FilterEventArgs)

    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        RefreshTransactions()
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As RoutedEventArgs) Handles btnLogout.Click
        Dim x As MainWindow = New MainWindow
        x.Show()
        Window.GetWindow(Me).Close()
    End Sub

    Private Sub btnPatients_Click(sender As Object, e As RoutedEventArgs) Handles btnPatients.Click
        Dim rcptPg As ReceptionistPage
        If rcptPg Is Nothing Then
            rcptPg = New ReceptionistPage(MainPage)
        End If
        MainPage.ReceptionistControl.Content = rcptPg
    End Sub

    Private Sub btnConfirmPayment_Click(sender As Object, e As RoutedEventArgs) Handles btnConfirmPayment.Click
        If _transactions.Count > 0 Then
            If gVars.dbReception.ConfirmTransations(_transactions) > 0 Then
                msgQ.Enqueue("Success! Payment is confirmed!")
            Else
                msgQ.Enqueue("Failure! Payment confirmation failed!")
            End If
            txtPayment.Text = "0"
            RefreshTransactions()
        End If
    End Sub

    Private Async Sub btnRemoveTransactionItem_Click(sender As Object, e As RoutedEventArgs) Handles btnRemoveTransactionItem.Click
        If (dgItems.SelectedIndex = -1) Then
            Return
        End If
        Dim result As Boolean = Await DialogHost.Show(dlgRemoveItemConfirmation, "RootDialog")
        Dim selectedTransactions As List(Of Transaction) = UtilityConverter.SelectedItemsToListOfTransactions(dgItems.SelectedItems)
        If result = True Then
            If gVars.dbReception.RemoveTransactions(selectedTransactions) > 0 Then
                msgQ.Enqueue("Success! Removed " + selectedTransactions.Count.ToString + " transactions!")
            Else
                msgQ.Enqueue("Failure! Failed to remove " + selectedTransactions.Count.ToString + " transactions!")
            End If
            RefreshTransactions()
        End If
    End Sub
End Class

Public Class BillingPageViewModel
    Inherits ObservableObject
    'IMPLEMENT VALIDATION
    Public Property PatientId() As Integer
    Private _payment As Decimal
    Public Property Payment() As Decimal
        Get
            Return _payment
        End Get
        Set(ByVal value As Decimal)
            _payment = value
            OnPropertyChanged(NameOf(Payment))
            OnPropertyChanged(NameOf(Change))
        End Set
    End Property
    Private _netTotal As Currency
    Public Property NetTotal() As Currency
        Get
            Return _netTotal
        End Get
        Set(ByVal value As Currency)
            _netTotal = value
            OnPropertyChanged(NameOf(NetTotal))
            OnPropertyChanged(NameOf(Change))
        End Set
    End Property
    Private _change As Currency
    Public ReadOnly Property Change() As Currency
        Get
            Dim changeVal As Currency
            If changeVal Is Nothing Then
                changeVal = New Currency(Payment - NetTotal.Value)
            Else
                changeVal.Value = Payment - NetTotal.Value
            End If
            Return changeVal
        End Get
    End Property
    Private _patientName As String
    Public Property PatientName() As String
        Get
            Return _patientName
        End Get
        Set(ByVal value As String)
            _patientName = value
            OnPropertyChanged(NameOf(PatientName))
        End Set
    End Property
    Private _patientIdentity As String
    Public Property PatientIdentity() As String
        Get
            Return _patientIdentity
        End Get
        Set(ByVal value As String)
            _patientIdentity = value
            OnPropertyChanged(NameOf(PatientIdentity))
        End Set
    End Property
End Class