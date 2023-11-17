namespace ScreenSound.DominioTeste._Util;

public static class AssertExtension
{
    public static async void ComMensagem(this ExcecaoDeDominio exception, string mensagem)
    {
        if (exception.MensagensDeErro.Contains(mensagem))
            Assert.True(true);
        else
            Assert.Fail($"Esperava a mensagem '{mensagem}'");

        await Task.Delay(0);
    }
}
