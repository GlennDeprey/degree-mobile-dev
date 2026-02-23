using System.ComponentModel.DataAnnotations;

namespace Mde.Project.Api.Core.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }
        public DateTime? EditedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
