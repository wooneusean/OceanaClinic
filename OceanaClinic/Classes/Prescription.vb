Public Class Prescription
	Public Property PrescriptionId As Integer
	Public Property TreatmentId As Integer
	Public Property TransactionId As Integer
	Public Property ItemName As String
	Public Property ItemQuantity As Integer
	Public Property ItemType As BillingItem.ItemTypeEnum
	Sub New()

	End Sub
	Sub New(_prescriptionId As Integer, _treatmentId As Integer, _transactionId As Integer, _itemName As String, _itemQuantity As Integer, _itemType As Integer)
		PrescriptionId = _prescriptionId
		TreatmentId = _treatmentId
		TransactionId = _transactionId
		ItemName = _itemName
		ItemQuantity = _itemQuantity
		ItemType = CType(_itemType, BillingItem.ItemTypeEnum)
	End Sub
End Class
