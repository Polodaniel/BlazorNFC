// Verifica se o dispositivo possui NFC
function VerificarStatusDispositivo()
{
    if ("NDEFReader" in window) {
        return true;
    }
    else {
        return false;
    }
}

//Retorna para o C# a verificação do Dispositivo
function VerificaDispositivo(dotNetHelper)
{
    if (VerificarStatusDispositivo()) {
        dotNetHelper.invokeMethodAsync('VerificaDispositivo', true);
    }
    else {
        dotNetHelper.invokeMethodAsync('VerificaDispositivo', false);
    }
}