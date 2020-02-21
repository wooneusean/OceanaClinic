Imports System.Text.RegularExpressions

Public Class AddItem
    Dim ViewModel As New AddItemViewModel
    Dim _billingItems As ObservableBillingItems
    Sub New(ByRef _itemToAdd As Transaction)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _billingItems = Me.Resources("billingItems")
        RefreshBillingItems()
        ViewModel.ItemToAdd = _itemToAdd
        ViewModel.ItemQuantityToAdd = 1
        DataContext = ViewModel
    End Sub
    Private Sub RefreshBillingItems()
        _billingItems.Clear()
        For Each billingItem As BillingItem In gVars.dbReception.GetAllBillingItems()
            _billingItems.Add(billingItem)
        Next
    End Sub

    Private Sub txtQuantity_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtQuantity.TextChanged
        If txtQuantity.Text = "" Then
            txtQuantity.Text = "0"
        End If
    End Sub

    Private Sub txtQuantity_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles txtQuantity.PreviewTextInput
        Dim regex As Regex = New Regex("\D")
        e.Handled = regex.IsMatch(e.Text)
    End Sub
    Public Sub CollectionViewSource_Filter(sender As Object, e As FilterEventArgs)
        Dim bi As BillingItem = e.Item
        If bi IsNot Nothing Then
            If (Not String.IsNullOrEmpty(txtSearch.Text)) Then
                Dim q As String = txtSearch.Text.ToLower
                If (bi.ItemId.ToString.Contains(q) Or bi.Name.ToString.ToLower.Contains(q) Or
                    bi.Price.ToString.Contains(q) Or bi.Type.ToString.ToLower.Contains(q)) Then
                    e.Accepted = True
                Else
                    e.Accepted = False
                End If
            Else
                e.Accepted = True
            End If
        End If
    End Sub
    Private Sub txtSearch_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtSearch.TextChanged
        CollectionViewSource.GetDefaultView(dgDlgBillingItems.ItemsSource).Refresh()
    End Sub

    Private Sub dgDlgBillingItems_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles dgDlgBillingItems.SelectionChanged
        Dim bi As BillingItem = dgDlgBillingItems.SelectedValue
        ViewModel.ItemIdToAdd = bi.ItemId
    End Sub
End Class
Public Class AddItemViewModel
    Inherits ObservableObject
    Private _itemToAdd As Transaction
    Public Property ItemToAdd() As Transaction
        Get
            Return _itemToAdd
        End Get
        Set(value As Transaction)
            _itemToAdd = value
            OnPropertyChanged(NameOf(ItemToAdd))
        End Set
    End Property
    Public Property ItemIdToAdd() As Integer
        Get
            Return _itemToAdd.ItemId
        End Get
        Set(ByVal value As Integer)
            _itemToAdd.ItemId = value
            OnPropertyChanged(NameOf(ItemIdToAdd))
            OnPropertyChanged(NameOf(CanSubmit))
        End Set
    End Property
    Public Property ItemQuantityToAdd() As Integer
        Get
            Return _itemToAdd.Quantity
        End Get
        Set(ByVal value As Integer)
            _itemToAdd.Quantity = value
            OnPropertyChanged(NameOf(ItemQuantityToAdd))
            OnPropertyChanged(NameOf(ItemIdToAdd))
            OnPropertyChanged(NameOf(CanSubmit))
        End Set
    End Property
    Public ReadOnly Property CanSubmit() As Boolean
        Get
            If (_itemToAdd.Quantity > 0) And (_itemToAdd.ItemId <> -1) Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
End Class