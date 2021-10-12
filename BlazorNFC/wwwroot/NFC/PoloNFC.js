// Verifica se o dispositivo possui NFC
function VerificarStatusDispositivo() {
    if ("NDEFReader" in window) {
        return true;
    }
    else {
        //console.log("4");
        return false;
    }
}

//Retorna para o C# a verificação do Dispositivo
function VerificaDispositivoNFC(dotNetHelper, ID) {
    if (VerificarStatusDispositivo()) {
        dotNetHelper.invokeMethodAsync('RetornoVerificacoes', ID, true);
    }
    else {
        dotNetHelper.invokeMethodAsync('RetornoVerificacoes', ID, false);
    }
}

// Gravar um Objeto JSON no cartão NFC
async function GravarJsonNFC(dotNetHelper, item) {

    if (VerificarStatusDispositivo()) {
        try {

            const ndef = new NDEFReader();

            const encoder = new TextEncoder();

            const NovaInformacao = {
                records: [{
                    recordType: "mime",
                    mediaType: "application/json",
                    data: encoder.encode(JSON.stringify({
                        nome: item.nome,
                        chavehash: item.chaveHash,
                        chaveitem: item.chave,
                        data: item.data,
                    }))
                }]
            };

            await ndef.write(NovaInformacao);

            dotNetHelper.invokeMethodAsync('GravadoNFC', true);

        } catch (error) {
            dotNetHelper.invokeMethodAsync('ErroNFC', error);
        }
    }
    else {
        dotNetHelper.invokeMethodAsync('ErroNFC', "Dispositivo não possui NFC !");
    }
}

// Verifica HARDWARE
async function VerificaHardware(dotNetHelper, ID) {
    try {
        const ndef = new NDEFReader();
        dotNetHelper.invokeMethodAsync('RetornoVerificacoes', ID, true);
    } catch (err) {
        dotNetHelper.invokeMethodAsync('RetornoVerificacoes', ID, false);
    }
}

// Realiza a Leitura do NFC
async function LerNFC(dotNetHelper) {
    try {
        const ndef = new NDEFReader();

        await ndef.scan();

        ndef.onreading = (event) => {
            const decoder = new TextDecoder();
            for (const record of event.message.records) {
                if (record.mediaType === "application/json") {
                    const json = JSON.parse(decoder.decode(record.data));

                    dotNetHelper.invokeMethodAsync('NomeNFC', json.nome);
                    dotNetHelper.invokeMethodAsync('ChaveHashNFC', json.chavehash);
                    dotNetHelper.invokeMethodAsync('ChaveNFC', json.chaveitem);
                    dotNetHelper.invokeMethodAsync('DataNFC', json.data);
                }
            }
        };

    } catch (err) {
        dotNetHelper.invokeMethodAsync('ErroNFC', "Dispositivo não possui NFC !");
    }
}