namespace ScreenSound.Dominio.Interfaces;

public interface IBase64Cleaner
{
    byte[] ConverterStringBase64ParaBytes(string base64String);
    string ConverterBytesParaStringBase64(byte[]? imagemBase64);
}