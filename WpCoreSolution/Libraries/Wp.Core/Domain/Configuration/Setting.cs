
namespace Wp.Core.Domain.Configuration
{
    public class Setting : Entity
    {
        public Setting() { }

        public Setting(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
        //public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
