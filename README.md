# BootstrapGridView for ASP.NET Core - How to edit the grid if it is bound to an XPO data source
This example demonstrates how to implement editing capabilities in the grid when it is bound to a large data base via an XPO data source.

## Steps to implement:
1. Place the grid into a Partial View and add required XPO classes.
2. Add required editing rout values to the grid:

```csharp
.Routes(routes => routes
    .MapRoute(r => r
        .Action("GridViewPartial")
        .Controller("Sample"))
    .MapRoute(r => r
        .RouteType(GridViewRouteType.UpdateRow)
        .Action("UpdateRow")
        .Controller("Sample"))
    .MapRoute(r => r
        .RouteType(GridViewRouteType.AddRow)
        .Action("AddNewRow")
        .Controller("Sample"))
    .MapRoute(r => r
        .RouteType(GridViewRouteType.DeleteRow)
        .Action("DeleteRow")
        .Controller("Sample")))
```

3. To correctly get the model from the client, create another item model with the required data fields:

```csharp 
 public class XPEmailModel {

     public int ID {
         get;
         set;
     }
     public string Subject {
         get;
         set;
     }
     public string From {
         get;
         set;
     }
     public DateTime Sent {
         get;
         set;
     }
     public long Size {
         get;
         set;
     }
     public bool HasAttachment {
         get;
         set;
     }
 }
```

4. Create corresponding route actions in the controller:

```csharp 
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
```

5. Use the previously created model in the editing actions. 

