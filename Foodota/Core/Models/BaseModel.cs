namespace Foodota.Core.Models;

public class BaseModel
{
	public bool IsActive { get; set; }
	public DateTime CreatedOn { get; set; }
	public DateTime? LastUpdatedOn { get; set; }

}
