Imports System.Text.RegularExpressions

Public Class EditItem
    Dim ViewModel As New AddItemViewModel
    Sub New(ByRef _itemToEdit As Transaction)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ViewModel.ItemToAdd = _itemToEdit
        DataContext = ViewModel
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
End Class