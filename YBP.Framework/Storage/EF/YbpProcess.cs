using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YBP.Framework.Storage.EF
{
    public class YbpProcess
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        [StringLength(128)]
        public virtual string InstanceId { get; set; }
        [StringLength(16)]
        public virtual string Pfx { get; set; }
        [StringLength(2048)]
        public virtual string SecurityContext { get; set; }
    }
}
