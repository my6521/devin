using System.ComponentModel.DataAnnotations;

namespace Devin.SwaggerDocument.Internal
{
    public class SwaggerAuth
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}