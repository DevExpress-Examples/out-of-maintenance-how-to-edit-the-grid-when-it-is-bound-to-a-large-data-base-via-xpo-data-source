using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace SampleCore.Models {
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
}