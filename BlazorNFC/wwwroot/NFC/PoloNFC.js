﻿// Verifica se o dispositivo possui NFC
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
function VerificaDispositivo(dotNetHelper, ID)
{
    if (VerificarStatusDispositivo()) {
        dotNetHelper.invokeMethodAsync('RetornoVerificacoes', ID, true);
    }
    else {
        dotNetHelper.invokeMethodAsync('RetornoVerificacoes', ID, false);
    }
}

// Gravar um Objeto JSON no cartão NFC
async function GravarJsonNFC(dotNetHelper, item)
{
    if (VerificarStatusDispositivo()) {
        try {
            //console.log("Nome: " + item.nome);
            //console.log("Data de Nascimento: " + item.data);
            //console.log("Hash: " + item.hash);

            const ndef = new NDEFReader();

         /*   await ndef.write("Hello world!");*/

            const encoder = new TextEncoder();

            const NovaInformacao = {
                records: [{
                    recordType: "mime",
                    mediaType: "application/json",
                    data: encoder.encode(JSON.stringify({
                        nome: item.nome,
                        data: item.data,
                        hash: item.hash
                    }))
                }]
            };

            //console.log(NovaInformacao);

            await ndef.write(NovaInformacao);

            dotNetHelper.invokeMethodAsync('GravarNFC', true);

        } catch (error) {

            console.log(error);

            dotNetHelper.invokeMethodAsync('ErroNFC', error);
        }
    }
    else
    {
        dotNetHelper.invokeMethodAsync('ErroNFC', "Dispositivo não possui NFC !");
    }
}

// Verifica HARDWARE
function VerificaHardware(dotNetHelper, ID)
{
    try {
        const ndef = new NDEFReader();
        dotNetHelper.invokeMethodAsync('RetornoVerificacoes', ID, true);
    } catch (err) {
        dotNetHelper.invokeMethodAsync('RetornoVerificacoes', ID, false);
    }
}