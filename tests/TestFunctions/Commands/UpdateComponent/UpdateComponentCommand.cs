using AWS.Lambda.Pipeline.Attributes.Binding;

namespace TestFunctions.Commands.UpdateComponent
{
    [FromBody]
    public class UpdateComponentCommand
    {
        [FromPath("id")]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
