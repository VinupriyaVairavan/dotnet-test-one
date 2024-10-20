using System.ComponentModel.DataAnnotations;

namespace FunctionAppTest.Options;

public class ApplicationOptions
{
    public ApplicationOptions(ConnectionStringOptions options)
    {
        Options = options;
    }

    [Required]
    public ConnectionStringOptions Options { get; set; }
}