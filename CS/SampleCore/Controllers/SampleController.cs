using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using SampleCore.Models;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace SampleCore.Controllers {
    public class SampleController : Controller {

        protected LargeDatabaseUnitOfWork LargeDatabaseUnitOfWork => HttpContext.RequestServices.GetService<LargeDatabaseUnitOfWork>();
        public IActionResult GridView() {
            return View(LargeDatabaseUnitOfWork.Emails);
        }
        public IActionResult GridViewPartial() {
            return PartialView(LargeDatabaseUnitOfWork.Emails);
        }
        public IActionResult AddNewRow(XPEmailModel email) {
            XPEmail newInfo = new XPEmail(LargeDatabaseUnitOfWork);
            newInfo.From = email.From;
            newInfo.ID = email.ID;
            newInfo.Subject = email.Subject;
            newInfo.Sent = email.Sent;
            newInfo.Size = email.Size;
            newInfo.HasAttachment = email.HasAttachment;
            LargeDatabaseUnitOfWork.CommitChanges();
            return PartialView("GridViewPartial", LargeDatabaseUnitOfWork.Emails);
        }
        public IActionResult UpdateRow(XPEmailModel email) {
            XPEmail newInfo = LargeDatabaseUnitOfWork.FindObject<XPEmail>(CriteriaOperator.Parse("ID = ?", email.ID));
            newInfo.From = email.From;
            newInfo.ID = email.ID;
            newInfo.Subject = email.Subject;
            newInfo.Sent = email.Sent;
            newInfo.Size = email.Size;
            newInfo.HasAttachment = email.HasAttachment;
            LargeDatabaseUnitOfWork.CommitChanges();
            return PartialView("GridViewPartial", LargeDatabaseUnitOfWork.Emails);
        }
        public IActionResult DeleteRow(XPEmailModel email) {
            XPEmail deleteEmailInfo = LargeDatabaseUnitOfWork.FindObject<XPEmail>(CriteriaOperator.Parse("ID = ?", email.ID));
            LargeDatabaseUnitOfWork.Delete(deleteEmailInfo);
            LargeDatabaseUnitOfWork.CommitChanges();
            return PartialView("GridViewPartial", LargeDatabaseUnitOfWork.Emails);
        }
    }
}