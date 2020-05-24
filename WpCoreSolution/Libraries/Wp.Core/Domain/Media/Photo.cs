namespace Wp.Core.Domain.Media
{
    public class Photo : EntityAuditable
  {
    public byte[] PhotoBinary { get; set; }    
    public string MimeType { get; set; }
    public string Name { get; set; }   
  }
}
