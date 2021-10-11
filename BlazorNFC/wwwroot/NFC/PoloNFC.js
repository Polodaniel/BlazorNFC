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
                        data: item.DataValidade,
                    }))
                }]
            };

            await ndef.write(NovaInformacao);

        } catch (error) {
            dotNetHelper.invokeMethodAsync('ErroNFC', error);
        }

        await dotNetHelper.invokeMethodAsync('GravadoNFC', true);
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

        ndef.addEventListener("readingerror", () => {
            dotNetHelper.invokeMethodAsync('ErroNFC', "Dispositivo não possui NFC !");
        });

        //ndef.addEventListener("reading", ({ message, serialNumber }) => {
        //    log(`> Serial Number: ${serialNumber}`);
        //    log(`> Records: (${message.records.length})`);
        //});

        ndef.onreading = (event) => {
            const decoder = new TextDecoder();
            for (const record of event.message.records) {
                if (record.mediaType === "application/json") {
                    const json = JSON.parse(decoder.decode(record.data));
                    console.log(`Nome: ${json.nome}`);
                    console.log(`ChaveHash: ${json.chavehash}`);
                    console.log(`Chave: ${json.chave}`);
                    console.log(`Data: ${json.data}`);

                    dotNetHelper.invokeMethodAsync('LerNFC', json.nome, json.chavehash, json.chave, json.data);
                }
            }
        };


    } catch (err) {
        dotNetHelper.invokeMethodAsync('ErroNFC', "Dispositivo não possui NFC !");
    }
}