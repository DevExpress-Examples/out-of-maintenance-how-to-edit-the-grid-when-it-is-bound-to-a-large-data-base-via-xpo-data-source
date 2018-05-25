Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.Extensions.DependencyInjection
Imports System
Imports System.Linq
Imports SampleCore.Models
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering

Namespace SampleCore.Controllers
    Public Class SampleController
        Inherits Controller

        Protected ReadOnly Property LargeDatabaseUnitOfWork() As LargeDatabaseUnitOfWork
            Get
                Return HttpContext.RequestServices.GetService(Of LargeDatabaseUnitOfWork)()
            End Get
        End Property
        Public Function GridView() As IActionResult
            Return View(LargeDatabaseUnitOfWork.Emails)
        End Function
        Public Function GridViewPartial() As IActionResult
            Return PartialView(LargeDatabaseUnitOfWork.Emails)
        End Function
        Public Function AddNewRow(ByVal email As XPEmailModel) As IActionResult
            Dim newInfo As New XPEmail(LargeDatabaseUnitOfWork)
            newInfo.From = email.From
            newInfo.ID = email.ID
            newInfo.Subject = email.Subject
            newInfo.Sent = email.Sent
            newInfo.Size = email.Size
            newInfo.HasAttachment = email.HasAttachment
            LargeDatabaseUnitOfWork.CommitChanges()
            Return PartialView("GridViewPartial", LargeDatabaseUnitOfWork.Emails)
        End Function
        Public Function UpdateRow(ByVal email As XPEmailModel) As IActionResult
            Dim newInfo As XPEmail = LargeDatabaseUnitOfWork.FindObject(Of XPEmail)(CriteriaOperator.Parse("ID = ?", email.ID))
            newInfo.From = email.From
            newInfo.ID = email.ID
            newInfo.Subject = email.Subject
            newInfo.Sent = email.Sent
            newInfo.Size = email.Size
            newInfo.HasAttachment = email.HasAttachment
            LargeDatabaseUnitOfWork.CommitChanges()
            Return PartialView("GridViewPartial", LargeDatabaseUnitOfWork.Emails)
        End Function
        Public Function DeleteRow(ByVal email As XPEmailModel) As IActionResult
            Dim deleteEmailInfo As XPEmail = LargeDatabaseUnitOfWork.FindObject(Of XPEmail)(CriteriaOperator.Parse("ID = ?", email.ID))
            LargeDatabaseUnitOfWork.Delete(deleteEmailInfo)
            LargeDatabaseUnitOfWork.CommitChanges()
            Return PartialView("GridViewPartial", LargeDatabaseUnitOfWork.Emails)
        End Function
    End Class
End Namespace