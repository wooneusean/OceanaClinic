Public Class TreatmentEditor
    Dim ViewModel As New TreatmentEditorViewModel
    Sub New(ByRef t As Treatment)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If String.IsNullOrEmpty(t.TreatmentDesc) Then
            ViewModel.Title = "Add Treatment"
            ViewModel.SubmitButton = "Add"
        Else
            ViewModel.Title = "Edit Treatment"
            ViewModel.SubmitButton = "Save"
        End If
        ViewModel._treatment = t
        DataContext = ViewModel
    End Sub

    Private Sub TextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
    End Sub
End Class
Public Class TreatmentEditorViewModel
    Inherits ObservableObject
    Public _treatment As Treatment
    Property TreatmentDesc() As String
        Get
            Return _treatment.TreatmentDesc
        End Get
        Set(value As String)
            _treatment.TreatmentDesc = value
            OnPropertyChanged(NameOf(TreatmentDesc))
            OnPropertyChanged(NameOf(CanSubmit))
        End Set
    End Property
    Property Title() As String
    Property SubmitButton() As String
    ReadOnly Property CanSubmit() As Boolean
        Get
            Return Not String.IsNullOrWhiteSpace(TreatmentDesc)
        End Get
    End Property
End Class
