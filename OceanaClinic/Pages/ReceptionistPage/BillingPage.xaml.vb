Public Class BillingPage
    Dim _billingItems As ObservableBillingItems
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _billingItems = Me.Resources("billingItems")

    End Sub
    Public Sub refreshBillingItems()
        _billingItems.Clear()
        For Each billingItem As BillingItem In gVars.dbReception.GetAllPatients()
            _patients.Add(Patient)
        Next
    End Sub
End Class
