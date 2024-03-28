using System.Reflection;
using System.Text.RegularExpressions;
using ScreenSound.Dominio.Interfaces;

namespace ScreenSound.Dominio._Base;

public class Base64Cleaner : IBase64Cleaner
{
    public byte[] ConverterStringBase64ParaBytes(string base64String)
    {
        base64String = Regex
            .Replace(base64String, "(data|image|/png|/webpg|/webp|/jpg|/jpeg|base64|,|:|;)", string.Empty);
        
        return Convert.FromBase64String(base64String);
    }
    
    public string ConverterBytesParaStringBase64(byte[]? imagemBase64)
        => $"data:image/png;base64,{Convert.ToBase64String(imagemBase64 ?? Array.Empty<byte>())}";
   
}