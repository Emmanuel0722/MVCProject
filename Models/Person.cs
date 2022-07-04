using System.ComponentModel.DataAnnotations;

namespace MVCProject.Models
{
        public class Person
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(maximumLength: 25, ErrorMessage = "El nombre debe ser mayor de 3 letras y menos de 25 letras.", MinimumLength = 3)]
        [Display(Name = "Nombre")]
        public string? firstName { get; set; }
        
        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(maximumLength: 25, ErrorMessage = "El Apellido debe ser mayor de 3 letras y menos de 25 letras.", MinimumLength = 3)]
        [Display(Name = "Apellido")]
        public string? lastName { get; set; }
        
        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(maximumLength: 10, ErrorMessage = "Por favor introduzca su numero de Telefono o Celular correctamente.", MinimumLength = 10)]
        [Display(Name = "Telefono")]
        public string? cellPhone { get; set; }

        [Display(Name = "Genero")]
        public string? gender { get; set; }
    }
}