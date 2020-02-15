Imports System.ComponentModel

Public Class ValidatableObservableObject
    Implements INotifyDataErrorInfo, INotifyPropertyChanged

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Sub OnPropertyChanged(ByVal e As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(e))
    End Sub

    Public PropertyErrorsDictionary As New Dictionary(Of String, List(Of String))
    Public ReadOnly Property HasErrors As Boolean Implements INotifyDataErrorInfo.HasErrors
        Get
            Try
                Dim propErrorsCount As Integer
                For Each propError In PropertyErrorsDictionary
                    For Each prErr In propError.Value
                        propErrorsCount += 1
                    Next
                Next
                If propErrorsCount > 0 Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception

            End Try
            Return True
        End Get
    End Property

    Public Event ErrorsChanged As EventHandler(Of DataErrorsChangedEventArgs) Implements INotifyDataErrorInfo.ErrorsChanged

    Public Function GetErrors(propertyName As String) As IEnumerable Implements INotifyDataErrorInfo.GetErrors
        Dim errors As New List(Of String)
        If Not String.IsNullOrWhiteSpace(propertyName) Then
            PropertyErrorsDictionary.TryGetValue(propertyName, errors)
            Return errors
        Else
            Return Nothing
        End If
    End Function
    Public Sub OnErrorsChanged(e As String)
        RaiseEvent ErrorsChanged(Me, New DataErrorsChangedEventArgs(e))
    End Sub

    Public Overridable Sub Validation(propName As String, ByRef propValue As String, errContent As String, type As String)
        Dim errorList As List(Of String)
        If PropertyErrorsDictionary.TryGetValue(propName, errorList) = False Then
            errorList = New List(Of String)
        Else
            errorList.Clear()
        End If
        Select Case type
            Case "NullWhite"
                If (String.IsNullOrWhiteSpace(propValue)) Then
                    errorList.Add(errContent)
                    OnErrorsChanged(propName)
                End If
            Case "NoYolo"
                If (propValue.Contains("Yolo")) Then
                    errorList.Add(errContent)
                    OnErrorsChanged(propName)
                End If
            Case Else
        End Select
        PropertyErrorsDictionary(propName) = errorList
    End Sub
End Class
