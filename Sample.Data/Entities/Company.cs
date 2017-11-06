using Sample.Definitions.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Data.Entities
{
    public class Company : ITitledEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual bool IsApproved { get; set; }
    }
}
