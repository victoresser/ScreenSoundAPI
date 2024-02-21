using System.ComponentModel.DataAnnotations;

namespace ScreenSound.Dominio.Models;

public class CreateUsuarioDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Compare("Password")]
    [Required]
    public string RePassword { get; set; }
    [Required]
    public DateTime DataNascimento { get; set; }
}