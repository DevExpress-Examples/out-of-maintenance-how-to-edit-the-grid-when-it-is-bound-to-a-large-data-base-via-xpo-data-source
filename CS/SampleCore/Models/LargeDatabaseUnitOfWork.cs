using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace SampleCore.Models
{
    public class LargeDatabaseUnitOfWork : UnitOfWork {
        public LargeDatabaseUnitOfWork(IDataLayer dataLayer)
            : base(dataLayer) {
        }

        public XPQuery<XPEmail> Emails => this.Query<XPEmail>();
    }

    [Persistent("Emails")]
    public class XPEmail : XPLiteObject {
        public XPEmail(Session session)
            : base(session) {
        }

        [Key(true)]
        public int ID {
            get => GetPropertyValue<int>("ID");
            set => SetPropertyValue("ID", value);
        }
        [Size(100)]
        public string Subject {
            get => GetPropertyValue<string>("Subject");
            set => SetPropertyValue("Subject", value);
        }
        [Size(32)]
        public string From {
            get => GetPropertyValue<string>("From");
            set => SetPropertyValue("From", value);
        }
        public DateTime Sent {
            get => GetPropertyValue<DateTime>("Sent");
            set => SetPropertyValue("Sent", value);
        }
        public long Size {
            get => GetPropertyValue<long>("Size");
            set => SetPropertyValue("Size", value);
        }
        public bool HasAttachment {
            get => GetPropertyValue<bool>("HasAttachment");
            set => SetPropertyValue("HasAttachment", value);
        }
    }

    public static class XpoServiceExtensions {
        static readonly Type[] EntityTypes = new Type[] { typeof(XPEmail) };
        static ReflectionDictionary ReflectionDictionary = new ReflectionDictionary();

        static XpoServiceExtensions() {
            ReflectionDictionary.GetDataStoreSchema(EntityTypes);
        }

        public static IServiceCollection AddLargeDatabaseUnitOfWork(this IServiceCollection services, string connectionString) {
            services.AddSingleton<LargeDatabaseDataLayer>(serviceProvider => {
                return CreatePooledDataLayer(connectionString);
            });
            services.AddScoped<LargeDatabaseUnitOfWork>(serviceProvider => {
                var dataLayer = serviceProvider.GetService<LargeDatabaseDataLayer>();
                return new LargeDatabaseUnitOfWork(dataLayer);
            });
            return services;
        }
        static LargeDatabaseDataLayer CreatePooledDataLayer(string connectionString) {
            using(var updateDataLayer = XpoDefault.GetDataLayer(connectionString, ReflectionDictionary, AutoCreateOption.DatabaseAndSchema)) {
                updateDataLayer.UpdateSchema(false, ReflectionDictionary.CollectClassInfos(EntityTypes));
            }

            var dataStore = XpoDefault.GetConnectionProvider(XpoDefault.GetConnectionPoolString(connectionString), AutoCreateOption.SchemaAlreadyExists);
            return new LargeDatabaseDataLayer(ReflectionDictionary, dataStore);
        }
    }
    public class LargeDatabaseDataLayer : ThreadSafeDataLayer {
        public LargeDatabaseDataLayer(XPDictionary dictionary, IDataStore provider)
            : base(dictionary, provider) {
        }
    }
}
