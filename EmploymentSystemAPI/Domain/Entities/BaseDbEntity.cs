using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
	public class BaseDbEntity 
	{
		public int Id { get; set; }
		public DateTime CreationDate { get; set; }
		public int CreatedBy { get; set; }
		public int? UpdatedBy { get; set; }
		public DateTime? UpdatedAt { get; set; }

	}
}