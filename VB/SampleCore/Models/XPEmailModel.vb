Imports Newtonsoft.Json
Imports System
Imports System.ComponentModel.DataAnnotations

Namespace SampleCore.Models
    Public Class XPEmailModel

        Public Property ID() As Integer
        Public Property Subject() As String
        Public Property From() As String
        Public Property Sent() As Date
        Public Property Size() As Long
        Public Property HasAttachment() As Boolean
    End Class
End Namespace