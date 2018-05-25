Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports DevExpress.Xpo
Imports DevExpress.Xpo.DB
Imports DevExpress.Xpo.Metadata
Imports Microsoft.Extensions.DependencyInjection

Namespace SampleCore.Models
    Public Class LargeDatabaseUnitOfWork
        Inherits UnitOfWork

        Public Sub New(ByVal dataLayer As IDataLayer)
            MyBase.New(dataLayer)
        End Sub

        Public ReadOnly Property Emails() As XPQuery(Of XPEmail)
            Get
                Return Me.Query(Of XPEmail)()
            End Get
        End Property
    End Class

    <Persistent("Emails")> _
    Public Class XPEmail
        Inherits XPLiteObject

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub

        <Key(True)> _
        Public Property ID() As Integer
            Get
                Return GetPropertyValue(Of Integer)("ID")
            End Get
            Set(ByVal value As Integer)
                SetPropertyValue("ID", value)
            End Set
        End Property
        <Size(100)> _
        Public Property Subject() As String
            Get
                Return GetPropertyValue(Of String)("Subject")
            End Get
            Set(ByVal value As String)
                SetPropertyValue("Subject", value)
            End Set
        End Property
        <Size(32)> _
        Public Property From() As String
            Get
                Return GetPropertyValue(Of String)("From")
            End Get
            Set(ByVal value As String)
                SetPropertyValue("From", value)
            End Set
        End Property
        Public Property Sent() As Date
            Get
                Return GetPropertyValue(Of Date)("Sent")
            End Get
            Set(ByVal value As Date)
                SetPropertyValue("Sent", value)
            End Set
        End Property
        Public Property Size() As Long
            Get
                Return GetPropertyValue(Of Long)("Size")
            End Get
            Set(ByVal value As Long)
                SetPropertyValue("Size", value)
            End Set
        End Property
        Public Property HasAttachment() As Boolean
            Get
                Return GetPropertyValue(Of Boolean)("HasAttachment")
            End Get
            Set(ByVal value As Boolean)
                SetPropertyValue("HasAttachment", value)
            End Set
        End Property
    End Class

    Public Module XpoServiceExtensions
        Private ReadOnly EntityTypes() As Type = { GetType(XPEmail) }
        Private ReflectionDictionary As New ReflectionDictionary()

        Sub New()
            ReflectionDictionary.GetDataStoreSchema(EntityTypes)
        End Sub

        <System.Runtime.CompilerServices.Extension> _
        Public Function AddLargeDatabaseUnitOfWork(ByVal services As IServiceCollection, ByVal connectionString As String) As IServiceCollection
            services.AddSingleton(Of LargeDatabaseDataLayer)(Function(serviceProvider)
                Return CreatePooledDataLayer(connectionString)
            End Function)
            services.AddScoped(Of LargeDatabaseUnitOfWork)(Function(serviceProvider)
                Dim dataLayer = serviceProvider.GetService(Of LargeDatabaseDataLayer)()
                Return New LargeDatabaseUnitOfWork(dataLayer)
            End Function)
            Return services
        End Function
        Private Function CreatePooledDataLayer(ByVal connectionString As String) As LargeDatabaseDataLayer
            Using updateDataLayer = XpoDefault.GetDataLayer(connectionString, ReflectionDictionary, AutoCreateOption.DatabaseAndSchema)
                updateDataLayer.UpdateSchema(False, ReflectionDictionary.CollectClassInfos(EntityTypes))
            End Using

            Dim dataStore = XpoDefault.GetConnectionProvider(XpoDefault.GetConnectionPoolString(connectionString), AutoCreateOption.SchemaAlreadyExists)
            Return New LargeDatabaseDataLayer(ReflectionDictionary, dataStore)
        End Function
    End Module
    Public Class LargeDatabaseDataLayer
        Inherits ThreadSafeDataLayer

        Public Sub New(ByVal dictionary As XPDictionary, ByVal provider As IDataStore)
            MyBase.New(dictionary, provider)
        End Sub
    End Class
End Namespace
