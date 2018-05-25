Option Infer On

Imports DevExpress.AspNetCore
Imports DevExpress.AspNetCore.Bootstrap
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Hosting
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.EntityFrameworkCore

Imports System.IO
Imports DevExpress.Xpo
Imports DevExpress.Xpo.DB
Imports SampleCore.Models
Imports SampleCore.Utils

Namespace SampleCore
    Public Class Startup
        Public Sub New(ByVal env As IHostingEnvironment)
            Dim builder = (New ConfigurationBuilder()).SetBasePath(env.ContentRootPath).AddJsonFile("appsettings.json", [optional]:= True, reloadOnChange:= True).AddJsonFile($"appsettings.{env.EnvironmentName}.json", [optional]:= True).AddEnvironmentVariables()
            Configuration = builder.Build()
            ConnectionStringsProvider = New ConnectionStringsProvider(Configuration, env.ContentRootPath)
        End Sub

        Public ReadOnly Property Configuration() As IConfiguration
        Private privateConnectionStringsProvider As ConnectionStringsProvider
        Public Property ConnectionStringsProvider() As ConnectionStringsProvider
            Get
                Return privateConnectionStringsProvider
            End Get
            Private Set(ByVal value As ConnectionStringsProvider)
                privateConnectionStringsProvider = value
            End Set
        End Property

        ' This method gets called by the runtime. Use this method to add services to the container.
        Public Sub ConfigureServices(ByVal services As IServiceCollection)

            services.AddMvc()
            services.AddDevExpressControls(Sub(options)
                options.Bootstrap(Sub(bootstrapOptions)
                    bootstrapOptions.IconSet = BootstrapIconSet.Embedded
                    bootstrapOptions.Mode = BootstrapMode.Bootstrap4
                End Sub)
                options.Resources = ResourcesType.DevExtreme
            End Sub)
            ' Sample data context registration
            services.AddLargeDatabaseUnitOfWork(ConnectionStringsProvider.GetConnectionString("XPLargeDatabaseConnectionString"))
        End Sub

        ' This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        Public Sub Configure(ByVal app As IApplicationBuilder, ByVal env As IHostingEnvironment)
            app.UseDevExpressControls()
            If env.IsDevelopment() Then
                app.UseDeveloperExceptionPage()
            Else
                app.UseExceptionHandler("/Home/Error")
            End If
            app.UseStaticFiles()
                        app.UseMvc(Sub(routes)
                            routes.MapRoute(name:= "default", template:= "{controller=Sample}/{action=GridView}/{id?}")
                        End Sub)
        End Sub
    End Class
End Namespace