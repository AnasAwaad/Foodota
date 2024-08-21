namespace Foodota.Core.Models;

public class BaseModel
{

    public BaseModel()
    {
        CreatedOn= DateTime.Now;
        IsActive= true;
    }
    public bool IsActive { get; set; }
	public DateTime CreatedOn { get; set; }
	public DateTime? LastUpdatedOn { get; set; }

}
