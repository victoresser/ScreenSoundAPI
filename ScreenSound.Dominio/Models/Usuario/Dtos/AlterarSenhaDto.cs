namespace ScreenSound.Dominio.Models;

public class AlterarSenhaDto
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ReNewPassword { get; set; }
}