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
                        chave: item.chave,
                        data: item.data,
                    }))
                }]
            };

            await ndef.write(NovaInformacao);

        } catch (error) {
            dotNetHelper.invokeMethodAsync('ErroNFC', error);
        }

        dotNetHelper.invokeMethodAsync('GravadoNFC', true);
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
                    console.log(`Nome: ${json.nome}`);
                    console.log(`ChaveHash: ${json.chavehash}`);
                    console.log(`Chave: ${json.chave}`);
                    console.log(`Data: ${json.data}`);

                    dotNetHelper.invokeMethodAsync('NomeNFC', json.nome);
                    dotNetHelper.invokeMethodAsync('ChaveHashNFC', json.chavehash);
                    dotNetHelper.invokeMethodAsync('ChaveNFC', json.chave);
                    dotNetHelper.invokeMethodAsync('DataNFC', json.data);
                }
            }
        };

    } catch (err) {
        dotNetHelper.invokeMethodAsync('ErroNFC', "Dispositivo não possui NFC !");
    }
}