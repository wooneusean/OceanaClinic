Public Class BillingPage
    Dim _billingItems As ObservableBillingItems
    Dim ViewModel As New BillingPageViewModel
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _billingItems = Me.Resources("billingItems")
        ViewModel.NetTotal = New Currency(23302329.5)
        DataContext = ViewModel
    End Sub
    Public Sub refreshBillingItems()
        _billingItems.Clear()
        For Each billingItem As BillingItem In gVars.dbReception.GetBillingItems(ViewModel.PatientId)
            _billingItems.Add(billingItem)
        Next
    End Sub
    Public Sub CollectionViewSource_Filter(sender As Object, e As FilterEventArgs)

    End Sub
End Class

Public Class BillingPageViewModel
    Inherits ObservableObject
    Private _patientId As Integer
    Public Property PatientId() As Integer
        Get
            Return _patientId
        End Get
        Set(ByVal value As Integer)
            _patientId = value
        End Set
    End Property
    Private _payment As Currency
    Public Property Payment() As Double
        Get
            If (_payment Is Nothing) Then
                _payment = New Currency()
                Return _payment.Value
            Else
                Return _payment.Value
            End If
        End Get
        Set(ByVal value As Double)
            If _payment Is Nothing Then
                _payment = New Currency(value)
            Else
                _payment.Value = value
            End If
            OnPropertyChanged("Payment")
            OnPropertyChanged("Change")
        End Set
    End Property
    Private _netTotal As Currency
    Public Property NetTotal() As Currency
        Get
            If (_netTotal Is Nothing) Then
                _netTotal = New Currency()
                Return _netTotal
            Else
                Return _netTotal
            End If
        End Get
        Set(ByVal value As Currency)
            _netTotal = value
            OnPropertyChanged(NameOf(NetTotal))
        End Set
    End Property
    Private _change As Currency
    Public ReadOnly Property Change() As Currency
        Get
            Return New Currency(NetTotal.Value - Payment)
        End Get
    End Property
End Class