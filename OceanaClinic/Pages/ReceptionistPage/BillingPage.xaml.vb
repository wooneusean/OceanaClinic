Imports System.ComponentModel
Imports System.Text.RegularExpressions

Public Class BillingPage
    Dim _billingItems As ObservableBillingItems
    Dim ViewModel As New BillingPageViewModel
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _billingItems = Me.Resources("billingItems")
        'ViewModel.NetTotal = New Currency(323232.7)
        ViewModel.NetTotal = New Currency(340000.2D)
        ViewModel.PatientId = 0
        ViewModel.Payment = 0
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

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        refreshBillingItems()
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

End Class