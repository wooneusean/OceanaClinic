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
            _newNetTotal += transaction.SubTotal.Value
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
        ValidatePayment()
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

    Private Async Sub btnAddTransactionItem_Click(sender As Object, e As RoutedEventArgs) Handles btnAddTransactionItem.Click
        Dim itemToAdd As New Transaction
        Dim result As Boolean = Await DialogHost.Show(New AddItem(itemToAdd), "RootDialog")
        If result = True Then
            If gVars.dbReception.InsertNewTransaction(itemToAdd, ViewModel.PatientId) > 0 Then
                msgQ.Enqueue("Success! New item added for " + ViewModel.PatientName + "!")
            Else
                msgQ.Enqueue("Failure! Failed to add item!")
            End If
            RefreshTransactions()
        End If
    End Sub
    Private Async Sub btnEditTransactionItem_Click(sender As Object, e As RoutedEventArgs) Handles btnEditTransactionItem.Click
        If dgItems.SelectedIndex = -1 Then
            Return
        End If
        Dim itemToEdit As Transaction = dgItems.SelectedValue
        Dim result As Boolean = Await DialogHost.Show(New EditItem(itemToEdit), "RootDialog")
        If result = True Then
            If gVars.dbReception.UpdateTransaction(itemToEdit) > 0 Then
                msgQ.Enqueue("Success! Updated item for " + ViewModel.PatientName + "!")
            Else
                msgQ.Enqueue("Failure! Failed to add item!")
            End If
            RefreshTransactions()
        End If
    End Sub
    Private Sub ValidatePayment()
        Task.Run(Sub() ViewModel.Validation(NameOf(ViewModel.Payment), ViewModel.Payment, "", "Payment"))
    End Sub
    Private Sub txtPayment_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtPayment.TextChanged
        ValidatePayment()
    End Sub

    Private Sub dgItems_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles dgItems.PreviewKeyDown
        If e.Key = Key.Delete Then
            btnRemoveTransactionItem_Click(sender, Nothing)
        End If
    End Sub

    Private Sub dgItems_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles dgItems.MouseDoubleClick
        If dgItems.SelectedIndex > -1 Then
            btnEditTransactionItem_Click(sender, Nothing)
        End If
    End Sub
End Class

Public Class BillingPageViewModel
    Inherits ValidatableObservableObject
    Public Property PatientId() As Integer
    Public ReadOnly Property CanConfirm() As Boolean
        Get
            If _payment > 0 And Not HasErrors Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    Private _payment As Decimal
    Private _paymentStr As String
    Public Property Payment() As String
        Get
            Return _paymentStr
        End Get
        Set(ByVal value As String)
            _paymentStr = value
            Dim dec As Decimal = 0
            If Decimal.TryParse(value, dec) Then
                _payment = dec
            Else
                _payment = 0
            End If
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
                changeVal = New Currency(_payment - _netTotal.Value)
            Else
                changeVal.Value = _payment - _netTotal.Value
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
    Public Overrides Sub Validation(propName As String, ByRef propValue As String, errContent As String, type As String)
        Dim errorList As List(Of String)
        If PropertyErrorsDictionary.TryGetValue(propName, errorList) = False Then
            errorList = New List(Of String)
        Else
            errorList.Clear()
        End If

        Select Case type
            Case "Payment"
                If (String.IsNullOrWhiteSpace(_paymentStr)) Then
                    errorList.Add("Payment cannot be empty!")
                Else
                    Dim regex As Regex = New Regex("^(\d*\.)?\d+$") ' https://www.regextester.com/95625 decimal number regex
                    If Not regex.IsMatch(_paymentStr) Then
                        errorList.Add("Incorrect format! Correct format: 20.00, 15.9, 9.78")
                    Else
                        If _payment < _netTotal.Value Then
                            errorList.Add("Payment cannot be less than amount due!")
                        End If
                    End If
                End If
            Case Else
        End Select

        PropertyErrorsDictionary(propName) = errorList
        OnErrorsChanged(propName)
        Console.WriteLine(propName)
        OnPropertyChanged(NameOf(CanConfirm))
    End Sub
End Class