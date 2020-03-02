Public Class Treatment
	Public Property TreatmentId As Integer
	Public Property PatientId As Integer
	Public Property TreatmentDesc As String
	Public Property TreatmentDate As Date
	Sub New()

	End Sub
	Sub New(_treatmentId As Integer, _patientId As Integer, _treatmentDesc As String, _treatmentDate As String)
		TreatmentId = _treatmentId
		PatientId = _patientId
		TreatmentDesc = _treatmentDesc
		TreatmentDate = Date.Parse(_treatmentDate)
	End Sub
End Class
