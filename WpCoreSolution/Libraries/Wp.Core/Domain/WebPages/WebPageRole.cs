namespace Wp.Core.Domain.WebPages
{
    public class WebPageRole : EntityAuditable
  {
    public WebPageRole()
    {
      PermissionLevelId = 1;
    }
  
    public string Name { get; set; }
    public int PermissionLevelId { get; set; } // v,e,c  
    public PermissionLevel PermissionLevel
    {
        get
        {
            return (PermissionLevel)this.PermissionLevelId;
        }
        set
        {
            this.PermissionLevelId = (int)value;
        }
    }

    public int WebPageId { get; set; }
    public virtual WebPage WebPage { get; set; }
  }
}
