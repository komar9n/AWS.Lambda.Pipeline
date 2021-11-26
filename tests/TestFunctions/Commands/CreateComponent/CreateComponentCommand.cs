using AWS.Lambda.Pipeline.Attributes.Binding;

namespace TestFunctions.Commands.CreateComponent
{
    [FromBody]
    public class CreateComponentCommand
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
