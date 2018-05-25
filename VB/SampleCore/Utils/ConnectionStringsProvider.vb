Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports Microsoft.Extensions.Configuration
Imports System.IO

Namespace SampleCore.Utils
    Public Class ConnectionStringsProvider
        Public Sub New(ByVal configuration As IConfiguration, ByVal contentRootPath As String)
            Me.Configuration = configuration
            Me.ContentRootPath = contentRootPath
            DatabasesPath = Path.Combine(Me.ContentRootPath, "App_Data")
        End Sub

        Protected ReadOnly Property Configuration() As IConfiguration
        Protected ReadOnly Property ContentRootPath() As String
        Protected ReadOnly Property DatabasesPath() As String

        Public Function GetConnectionString(ByVal name As String) As String
            Return Configuration.GetConnectionString(name).Replace("%DataDirectory%", DatabasesPath)
        End Function
    End Class
End Namespace
