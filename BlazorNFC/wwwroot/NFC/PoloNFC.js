// Verifica se o dispositivo possui NFC
function VerificarStatusDispositivo()
{
    console.log("2");
    if ("NDEFReader" in window) {
        console.log("3");
        return true;
    }
    else {
        console.log("4");
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
    console.log("1");
    if (VerificarStatusDispositivo()) {
        try {
            console.log("5");
            console.log("Nome: " + item.nome);
            console.log("Chave Hash: " + item.chaveHash);
            console.log("Chave: " + item.chave);
            console.log("Data Validade: " + item.dataValidade);

            const ndef = new NDEFReader();

            console.log("6");
            const encoder = new TextEncoder();
            console.log("7");
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

            console.log("8");
            console.log(NovaInformacao);

            await ndef.write(NovaInformacao);
            console.log("9");
            dotNetHelper.invokeMethodAsync('GravadoNFC', true);
            console.log("10");
        } catch (error) {
            console.log("11");
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
        await ndef.scan();

        dotNetHelper.invokeMethodAsync('RetornoVerificacoes', ID, true);
    } catch (err) {
        dotNetHelper.invokeMethodAsync('RetornoVerificacoes', ID, false);
    }
}