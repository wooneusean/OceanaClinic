Public Class Prescription
	Public Property PrescriptionId As Integer
	Public Property TreatmentId As Integer 'can group
	Public Property TransactionId As Integer
	Public Property ItemName As String
	Public Property ItemQuantity As Integer
	Sub New()

	End Sub
	Sub New(_prescriptionId As Integer, _treatmentId As Integer, _transactionId As Integer, _itemName As String, _itemQuantity As Integer)
		PrescriptionId = _prescriptionId
		TreatmentId = _treatmentId
		TransactionId = _transactionId
		ItemName = _itemName
		ItemQuantity = _itemQuantity
	End Sub
End Class
