using System.ComponentModel.DataAnnotations;

namespace Mde.Project.Api.Dtos
{
    public class BaseDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? EditedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
