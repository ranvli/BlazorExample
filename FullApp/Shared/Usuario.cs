using System.ComponentModel.DataAnnotations;

namespace FullApp.Shared
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(45)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(45)]
        public string Apellido1 { get; set; }

        [MaxLength(45)]
        public string? Apellido2 { get; set; }

        [MaxLength(45)]
        public string? TelefonoMovil { get; set; }
    }
}
