using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPIODataV4SQLite.DomainModel
{
    [DataContract(Name = "code_iata")]
    public class Code_iata
    {
        [DataMember]
        [Key]
        public string Iata { get; set; }

        [DataMember]
        public string City { get; set; }
    }
}