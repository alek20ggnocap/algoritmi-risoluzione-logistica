<script lang="ts">
    let finsetraAttiva = "home";
    let tabella: number[][] = [];
    let tabellaNO: number[][] = [];
    let tabellaMC: number[][] = [];
    let stepsNO: number[][][] = [];
    let stepsMC: number[][][] = [];
    let totaleCostiNO: number = 0;
    let totaleCostiMC: number = 0;
    let produzioniNO: number[] = [];
    let richiesteNO: number[] = [];
    let produzioniMC: number[] = [];
    let richiesteMC: number[] = [];
    let produzioni: number[] = [];
    let richieste: number[] = [];
    let sommaTotale: number = 0;
    let sommaProduzioni: number = 0;
    let sommaRichieste: number = 0;
    let isBalanced: boolean = true;
    let riga: number | null = null;
    let colonna: number | null = null;
    let currentStepNO = 0;
    let currentStepMC = 0;
    let maxStepNO = 0;
    let maxStepMC = 0;

    $: riga = null;
    $: colonna = null;

    function generaProduzioniERichieste(righe: number, colonne: number) {
        produzioni = Array.from({ length: righe }, () => Math.floor(Math.random() * 400) + 100);
        const sommaProduzioniGen = produzioni.reduce((a, b) => a + b, 0);

        const pesi = Array.from({ length: colonne }, () => Math.random());
        const sommaPesi = pesi.reduce((a, b) => a + b, 0);

        richieste = pesi.map(p => Math.round((p / sommaPesi) * sommaProduzioniGen));

        const diff = sommaProduzioniGen - richieste.reduce((a, b) => a + b, 0);
        if (diff !== 0) richieste[richieste.length - 1] += diff;

        calcolaSomme();
    }

    function setTabelle(){
    if (riga === null || colonna === null) return;
    if (tabella.length === 0) return;
    
    // Copia la tabella
    tabellaNO = [];
    tabellaMC = [];
    for (let i = 0; i < tabella.length; i++) {
        tabellaNO[i] = [...tabella[i]];
        tabellaMC[i] = [...tabella[i]];
    }
    
    // Copia produzioni e richieste
    produzioniNO = [...produzioni];
    richiesteNO = [...richieste];
    produzioniMC = [...produzioni];
    richiesteMC = [...richieste];
    
    // Reset totali
    totaleCostiNO = 0;
    totaleCostiMC = 0;
}

    function randomizeValues() {
        tabella.length = 0;
        if (riga === null || colonna === null) return;
        for (let i = 0; i < riga; i++) {
            tabella[i] = [];
            for (let j = 0; j < colonna; j++) {
                tabella[i][j] = Math.floor(Math.random() * 100);
            }
        }
        for (let k = 0; k < colonna; k++) {
            richieste[k] = 0;
        }
        for (let l = 0; l < riga; l++) {
            produzioni[l] = 0;
        }
        generaProduzioniERichieste(riga, colonna);
    }

    function onCellInput(i: number, j: number, raw: string) {
        const num = raw === '' ? 0 : Number(raw);
        if (!tabella[i]) tabella[i] = [];
        tabella[i][j] = isNaN(num) ? 0 : num;
        calcolaSomme();
    }

    function algoritmoNordOvest() {
        while (richiesteNO.length > 0 && produzioniNO.length > 0) {
            algoritmoNordOvestStep();
        }
    }
    
    function algoritmoNordOvestStep() {
        if (tabellaNO.length === 0) return;
        if (!isBalanced) return;
        if (richiesteNO.length === 0 || produzioniNO.length === 0) return;
        
        if (richiesteNO[0] === produzioniNO[0]) {
            totaleCostiNO += tabellaNO[0][0] * richiesteNO[0];
            richiesteNO = richiesteNO.slice(1);  // Usa slice invece di shift
            produzioniNO = produzioniNO.slice(1);
            tabellaNO = tabellaNO.slice(1);  // Rimuove la prima riga
        } else if (richiesteNO[0] < produzioniNO[0]) {
            totaleCostiNO += tabellaNO[0][0] * richiesteNO[0];
            produzioniNO[0] -= richiesteNO[0];
            produzioniNO = [...produzioniNO];  // Forza reattività
            richiesteNO = richiesteNO.slice(1);
            tabellaNO = tabellaNO.slice(1);  // Rimuove la prima riga
        } else {
            totaleCostiNO += tabellaNO[0][0] * produzioniNO[0];
            richiesteNO[0] -= produzioniNO[0];
            richiesteNO = [...richiesteNO];  // Forza reattività
            produzioniNO = produzioniNO.slice(1);
            tabellaNO = tabellaNO.map(riga => riga.slice(1));  // Rimuove la prima colonna da ogni riga
        }
    }
    
    // --- Implementazione completa: esegue gli step fino a esaurire righe o colonne ---
    function algoritmoMinimiCosti() {
        if (tabellaMC.length === 0 || (tabellaMC[0] && tabellaMC[0].length === 0)) return;

        while (
            produzioniMC.length > 0 &&
            richiesteMC.length > 0 &&
            tabellaMC.length > 0 &&
            tabellaMC[0].length > 0
        ) {
            algoritmoMinimiCostiStep();
        }
    }

    // --- Step singolo per Minimi Costi ---
    function algoritmoMinimiCostiStep() {
        if (tabellaMC.length === 0) return;
        if (!tabellaMC[0] || tabellaMC[0].length === 0) return;
        if (produzioniMC.length === 0 || richiesteMC.length === 0) return;

        // trova valore minimo
        let minVal = Infinity;
        let iMin = -1;
        let jMin = -1;

        for (let i = 0; i < tabellaMC.length; i++) {
            for (let j = 0; j < tabellaMC[i].length; j++) {
                const v = tabellaMC[i][j];
                if (v < minVal) {
                    minVal = v;
                    iMin = i;
                    jMin = j;
                }
            }
        }

        if (iMin === -1 || jMin === -1) return;

        // quantità assegnata
        const q = Math.min(produzioniMC[iMin], richiesteMC[jMin]);

        // aggiorna costo
        totaleCostiMC += minVal * q;

        // aggiorna produzione/richiesta
        produzioniMC[iMin] -= q;
        richiesteMC[jMin] -= q;

        // forziamo reattività
        produzioniMC = [...produzioniMC];
        richiesteMC = [...richiesteMC];

        // rimozioni
        const prodZero = produzioniMC[iMin] === 0;
        const reqZero = richiesteMC[jMin] === 0;

        if (prodZero && reqZero) {
            // riga
            tabellaMC = tabellaMC.slice(0, iMin).concat(tabellaMC.slice(iMin + 1));
            produzioniMC = produzioniMC.slice(0, iMin).concat(produzioniMC.slice(iMin + 1));

            // colonna
            if (tabellaMC.length > 0) {
                tabellaMC = tabellaMC.map(r =>
                    r.slice(0, jMin).concat(r.slice(jMin + 1))
                );
            }
            richiesteMC = richiesteMC.slice(0, jMin).concat(richiesteMC.slice(jMin + 1));
        } else if (prodZero) {
            tabellaMC = tabellaMC.slice(0, iMin).concat(tabellaMC.slice(iMin + 1));
            produzioniMC = produzioniMC.slice(0, iMin).concat(produzioniMC.slice(iMin + 1));
        } else if (reqZero) {
            tabellaMC = tabellaMC.map(r =>
                r.slice(0, jMin).concat(r.slice(jMin + 1))
            );
            richiesteMC = richiesteMC.slice(0, jMin).concat(richiesteMC.slice(jMin + 1));
        }

        // pulizia col/formato
        if (tabellaMC.length === 0 || (tabellaMC[0] && tabellaMC[0].length === 0)) {
            tabellaMC = [];
        } else {
            const colCount = tabellaMC[0].length;
            tabellaMC = tabellaMC.map(r => {
                if (r.length > colCount) return r.slice(0, colCount);
                if (r.length < colCount) return r.concat(Array(colCount - r.length).fill(0));
                return r;
            });
        }

        // forziamo reattività
        // (tabellaMC non viene più copiato: si modifica direttamente)
        produzioniMC = [...produzioniMC];
        richiesteMC = [...richiesteMC];

        // snapshot step (unica copia necessaria!)
        stepsMC.push(tabellaMC.map(r => [...r]));
    }

    function nextStepNO() {
        if (currentStepNO < stepsNO.length - 1) currentStepNO++;
    }

    function prevStepNO() {
        if (currentStepNO > 0) currentStepNO--;
    }

    function nextStepMC() {
        if (currentStepMC < stepsMC.length - 1) currentStepMC++;
    }

    function prevStepMC() {
        if (currentStepMC > 0) currentStepMC--;
    }

    function resetSteps() {
        currentStepNO = 0;
        currentStepMC = 0;
    }

    function resetShow() {
        if (stepsMC.length != 0 && stepsNO.length != 0)
        {
            stepsMC = [];
            tabellaMC = [];
            produzioniMC = [];
            richiesteMC = [];
            totaleCostiMC = 0;
            stepsMC = [];
            stepsNO = [];
            tabellaNO = [];
            produzioniNO = [];
            richiesteNO = [];
            totaleCostiNO = 0;
            stepsNO = [];
        }
        resetSteps();
    }

    function calcolaSomme() {
        if (tabella.length === 0) {
            richieste = [];
            produzioni = [];
            sommaTotale = 0;
            sommaProduzioni = 0;
            sommaRichieste = 0;
            isBalanced = true;
            return;
        }

        sommaProduzioni = produzioni.reduce((a, b) => a + b, 0);
        sommaRichieste = richieste.reduce((a, b) => a + b, 0);
        isBalanced = sommaProduzioni === sommaRichieste;
        sommaTotale = sommaProduzioni;
    }
</script>

<div class="flex flex-col h-screen w-full bg-amber-50 text-gray-900">
    <!-- HEADER -->
    <header class="h-[5%] bg-amber-600 flex items-center justify-center shadow-md">
        <h1 class="text-lg font-semibold text-white tracking-wide">Progetto Trasporti</h1>
    </header>

    <!-- NAVBAR -->
    <nav class="flex justify-center space-x-6 bg-amber-400 py-3 shadow-md">
        {#each [
            { name: "Home", key: "home" },
            { name: "Algoritmo Nord Ovest", key: "algoritmo nord ovest" },
            { name: "Algoritmo Minimi Costi", key: "algoritmo minimi costi" }
        ] as tab}
            <button
                class="px-4 py-2 rounded-lg font-medium transition-all duration-200
                {finsetraAttiva === tab.key
                    ? 'bg-amber-200 text-gray-900 shadow-inner'
                    : 'bg-amber-100 hover:bg-amber-200'}"
                on:click={() => {finsetraAttiva = tab.key; setTabelle(); resetShow();}}>
                {tab.name}
            </button>
        {/each}
        <button
            class="px-4 py-2 rounded-lg font-medium transition-all duration-200
                {finsetraAttiva === "show"
                    ? 'bg-amber-200 text-gray-900 shadow-inner'
                    : 'bg-amber-100 hover:bg-amber-200'}"
                on:click={() => {finsetraAttiva = "show"; setTabelle();}}>
                Show
        </button>
    </nav>

    <!-- CONTENUTO PRINCIPALE -->
    <div class="flex-1 flex flex-col justify-start p-6 overflow-y-auto">
        {#if finsetraAttiva === "home"}
            <div class="flex flex-col mt-6 space-y-6">

                <div class="flex flex-wrap items-center justify-center gap-4">
                    <div class="flex items-center">
                        <p class="p-2">righe</p>
                        <input type="number" min="0" placeholder="n-row" class="w-24 text-center mr-4 p-2 border border-amber-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-amber-400" bind:value={riga} />
                    </div>
                    <div class="flex items-center">
                        <p class="p-2">colonne</p>
                        <input type="number" min="0" placeholder="n-col" class="w-24 text-center mr-4 p-2 border border-amber-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-amber-400" bind:value={colonna} />
                    </div>
                    <button
                        class="bg-amber-500 text-white font-semibold px-6 py-2 rounded-lg shadow hover:bg-amber-600 transition-all"
                        on:click={randomizeValues}>
                        Genera Valori Casuali
                    </button>
                    {#if tabella.length <= 0}
                        <p class="text-gray-600 justify-center italic">Clicca il pulsante per generare la tabella.</p>
                    {/if}
                </div>

                <!-- AVVISO DI SBILANCIAMENTO -->
                {#if tabella.length > 0 && !isBalanced}
                    <div class="bg-red-300 border-l-4 border-red-500 text-red-950 p-4 rounded-lg shadow-md">
                        <div class="flex items-center">
                            <svg class="w-6 h-6 mr-3" fill="currentColor" viewBox="0 0 20 20">
                                <path fill-rule="evenodd" d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z" clip-rule="evenodd"/>
                            </svg>
                            <div>
                                <p class="font-bold">Attenzione: Tabella Sbilanciata!</p>
                                <p class="text-sm">Somma Produzioni: <span class="font-semibold">{sommaProduzioni}</span> | Somma Richieste: <span class="font-semibold">{sommaRichieste}</span> | Differenza: <span class="font-semibold">{Math.abs(sommaProduzioni - sommaRichieste)}</span></p>
                            </div>
                        </div>
                    </div>
                {/if}

                <!-- CONFERMA DI BILANCIAMENTO -->
                {#if tabella.length > 0 && isBalanced}
                    <div class="bg-green-300 border-l-4 border-green-500 text-green-950 p-4 rounded-lg shadow-md">
                        <div class="flex items-center">
                            <svg class="w-6 h-6 mr-3" fill="currentColor" viewBox="0 0 20 20">
                                <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd"/>
                            </svg>
                            <div>
                                <p class="font-bold">✓ Tabella Bilanciata</p>
                                <p class="text-sm">Somma Produzioni = Somma Richieste = <span class="font-semibold">{sommaTotale}</span></p>
                            </div>
                        </div>
                    </div>
                {/if}

                {#if tabella.length > 0}
                    <div class="w-full overflow-auto border border-amber-200 rounded-lg p-2">
                        <table class="border-collapse table-auto min-w-max text-center">
                            <thead class="bg-amber-200 sticky top-0">
                                <tr>
                                    <th class="border border-amber-400 w-24 bg-amber-300"></th>
                                    {#each tabella[0] as _, j}
                                        <th class="border border-amber-400 px-4 py-2 font-semibold">D{j + 1}</th>
                                    {/each}
                                    <th class="border border-amber-400 px-4 py-2 font-semibold bg-amber-300">Produzione</th>
                                </tr>
                            </thead>

                            <tbody>
                                {#each tabella as row, i}
                                    <tr class="hover:bg-amber-50">
                                        <th class="border border-amber-400 px-4 py-2 bg-amber-100 font-semibold">UP{i + 1}</th>
                                        {#each row as value, j}
                                            <td class="border border-amber-400 px-1 py-1">
                                                <input
                                                    type="number"
                                                    inputmode="numeric"
                                                    class="[&::-webkit-inner-spin-button]:appearance-none w-20 text-center bg-transparent focus:outline-none focus:ring-1 focus:ring-amber-400 rounded"
                                                    bind:value={tabella[i][j]}
                                                    on:input={(e: Event) => onCellInput(i, j, (e.target as HTMLInputElement).value)}
                                                />
                                            </td>
                                        {/each}
                                        <td class="border border-amber-400 bg-amber-100 font-semibold text-sm px-4 py-2">
                                            <input
                                                type="number"
                                                inputmode="numeric"
                                                class="[&::-webkit-inner-spin-button]:appearance-none w-20 text-center bg-transparent focus:outline-none focus:ring-1 focus:ring-amber-400 rounded"
                                                bind:value={produzioni[i]}
                                                on:input={() => calcolaSomme()}
                                            />
                                        </td>
                                    </tr>
                                {/each}
                            </tbody>

                            <tfoot class="bg-amber-200 font-semibold">
                                <tr>
                                    <th class="border border-amber-400 bg-amber-300">Richieste</th>
                                    {#each tabella[0] as _, j}
                                        <td class="border border-amber-400 px-2 py-2">
                                            <input
                                                type="number"
                                                inputmode="numeric"
                                                class="[&::-webkit-inner-spin-button]:appearance-none w-20 text-center bg-transparent focus:outline-none focus:ring-1 focus:ring-amber-400 rounded"
                                                bind:value={richieste[j]}
                                                on:input={() => calcolaSomme()}
                                            />
                                        </td>
                                    {/each}
                                    <td class="border px-4 py-2 border-amber-400 font-bold transition-all duration-300
                                        {isBalanced ? 'bg-green-300 text-green-950' : 'bg-red-300 text-red-950'}">
                                        {#if isBalanced}
                                            {sommaTotale}
                                        {:else}
                                            {sommaProduzioni} ≠ {sommaRichieste}
                                        {/if}
                                    </td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                {/if}
            </div>
        {/if}

        {#if finsetraAttiva === "algoritmo nord ovest"}
            <div class="flex flex-col mt-6 space-y-6">
                
                <!-- AVVISO DI SBILANCIAMENTO -->
                {#if tabellaNO.length > 0 && !isBalanced}
                    <div class="bg-red-300 border-l-4 border-red-500 text-red-950 p-4 rounded-lg shadow-md">
                        <div class="flex items-center">
                            <svg class="w-6 h-6 mr-3" fill="currentColor" viewBox="0 0 20 20">
                                <path fill-rule="evenodd" d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z" clip-rule="evenodd"/>
                            </svg>
                            <div>
                                <p class="font-bold">Attenzione: Tabella Sbilanciata!</p>
                                <p class="text-sm">Somma Produzioni: <span class="font-semibold">{sommaProduzioni}</span> | Somma Richieste: <span class="font-semibold">{sommaRichieste}</span> | Differenza: <span class="font-semibold">{Math.abs(sommaProduzioni - sommaRichieste)}</span></p>
                            </div>
                        </div>
                    </div>
                {/if}

                <!-- CONFERMA DI BILANCIAMENTO -->
                {#if tabellaNO.length > 0 && isBalanced}
                    <div class="bg-green-300 border-l-4 border-green-500 text-green-950 p-4 rounded-lg shadow-md">
                        <div class="flex items-center">
                            <svg class="w-6 h-6 mr-3" fill="currentColor" viewBox="0 0 20 20">
                                <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd"/>
                            </svg>
                            <div>
                                <p class="font-bold">✓ Tabella Bilanciata</p>
                                <p class="text-sm">Somma Produzioni = Somma Richieste = <span class="font-semibold">{sommaTotale}</span></p>
                            </div>
                        </div>
                    </div>
                {/if}

                {#if tabellaNO.length > 0}
                    <div class="w-full overflow-auto border border-amber-200 rounded-lg p-2">
                        <table class="border-collapse table-auto min-w-max text-center">
                            <thead class="bg-amber-200 sticky top-0">
                                <tr>
                                    <th class="border border-amber-400 w-24 bg-amber-300"></th>
                                    {#each tabellaNO[0] as _, j}
                                        <th class="border border-amber-400 px-4 py-2 font-semibold">D{j + 1}</th>
                                    {/each}
                                    <th class="border border-amber-400 px-4 py-2 font-semibold bg-amber-300">Produzione</th>
                                </tr>
                            </thead>

                            <tbody>
                                {#each tabellaNO as row, i}
                                    <tr class="hover:bg-amber-50">
                                        <th class="border border-amber-400 px-4 py-2 bg-amber-100 font-semibold">UP{i + 1}</th>
                                        {#each row as value, j}
                                            <td class="border border-amber-400 px-1 py-1">
                                                <input
                                                    readonly
                                                    type="number"
                                                    inputmode="numeric"
                                                    class="[&::-webkit-inner-spin-button]:appearance-none w-20 text-center bg-transparent focus:outline-none focus:ring-1 focus:ring-amber-400 rounded"
                                                    bind:value={tabellaNO[i][j]}
                                                />
                                            </td>
                                        {/each}
                                        <td class="border border-amber-400 bg-amber-100 font-semibold text-sm px-4 py-2">
                                            <input
                                                readonly
                                                type="number"
                                                inputmode="numeric"
                                                class="[&::-webkit-inner-spin-button]:appearance-none w-20 text-center bg-transparent focus:outline-none focus:ring-1 focus:ring-amber-400 rounded"
                                                bind:value={produzioniNO[i]}
                                            />
                                        </td>
                                    </tr>
                                {/each}
                            </tbody>

                            <tfoot class="bg-amber-200 font-semibold">
                                <tr>
                                    <th class="border border-amber-400 bg-amber-300">Richieste</th>
                                    {#each tabellaNO[0] as _, j}
                                        <td class="border border-amber-400 px-2 py-2">
                                            <input
                                                readonly
                                                type="number"
                                                inputmode="numeric"
                                                class="[&::-webkit-inner-spin-button]:appearance-none w-20 text-center bg-transparent focus:outline-none focus:ring-1 focus:ring-amber-400 rounded"
                                                bind:value={richiesteNO[j]}
                                            />
                                        </td>
                                    {/each}
                                    <td class="border px-4 py-2 border-amber-400 font-bold transition-all duration-300
                                        {isBalanced ? 'bg-green-300 text-green-950' : 'bg-red-300 text-red-950'}">
                                        {#if isBalanced}
                                            {sommaTotale}
                                        {:else}
                                            {sommaProduzioni} ≠ {sommaRichieste}
                                        {/if}
                                    </td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                {:else}
                    <p class="text-gray-600 text-center italic mt-8">
                        Genera prima una tabella nella sezione Home per visualizzare l'algoritmo Nord-Ovest.
                    </p>
                {/if}
            </div>
            <button
                class="mt-6 bg-amber-500 text-white font-semibold px-6 py-2 rounded-lg shadow hover:bg-amber-600 transition-all"
                on:click={algoritmoNordOvestStep}>
                Esegui Step Algoritmo Nord-Ovest
            </button>
            <button
                class="mt-6 bg-amber-500 text-white font-semibold px-6 py-2 rounded-lg shadow hover:bg-amber-600 transition-all"
                on:click={algoritmoNordOvest}>
                Esegui diretto Algoritmo Nord-Ovest
            </button>
            <p>costo totale con algoritmo Nord - Ovest: {totaleCostiNO}</p>
        {/if}

        {#if finsetraAttiva === "algoritmo minimi costi"}
            <div class="flex flex-col mt-6 space-y-6">
                
                <!-- AVVISO DI SBILANCIAMENTO -->
                {#if tabellaMC.length > 0 && !isBalanced}
                    <div class="bg-red-300 border-l-4 border-red-500 text-red-950 p-4 rounded-lg shadow-md">
                        <div class="flex items-center">
                            <svg class="w-6 h-6 mr-3" fill="currentColor" viewBox="0 0 20 20">
                                <path fill-rule="evenodd" d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z" clip-rule="evenodd"/>
                            </svg>
                            <div>
                                <p class="font-bold">Attenzione: Tabella Sbilanciata!</p>
                                <p class="text-sm">Somma Produzioni: <span class="font-semibold">{sommaProduzioni}</span> | Somma Richieste: <span class="font-semibold">{sommaRichieste}</span> | Differenza: <span class="font-semibold">{Math.abs(sommaProduzioni - sommaRichieste)}</span></p>
                            </div>
                        </div>
                    </div>
                {/if}

                <!-- CONFERMA DI BILANCIAMENTO -->
                {#if tabellaMC.length > 0 && isBalanced}
                    <div class="bg-green-300 border-l-4 border-green-500 text-green-950 p-4 rounded-lg shadow-md">
                        <div class="flex items-center">
                            <svg class="w-6 h-6 mr-3" fill="currentColor" viewBox="0 0 20 20">
                                <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd"/>
                            </svg>
                            <div>
                                <p class="font-bold">✓ Tabella Bilanciata</p>
                                <p class="text-sm">Somma Produzioni = Somma Richieste = <span class="font-semibold">{sommaTotale}</span></p>
                            </div>
                        </div>
                    </div>
                {/if}

                {#if tabellaMC.length > 0}
                    <div class="w-full overflow-auto border border-amber-200 rounded-lg p-2">
                        <table class="border-collapse table-auto min-w-max text-center">
                            <thead class="bg-amber-200 sticky top-0">
                                <tr>
                                    <th class="border border-amber-400 w-24 bg-amber-300"></th>
                                    {#each tabellaMC[0] as _, j}
                                        <th class="border border-amber-400 px-4 py-2 font-semibold">D{j + 1}</th>
                                    {/each}
                                    <th class="border border-amber-400 px-4 py-2 font-semibold bg-amber-300">Produzione</th>
                                </tr>
                            </thead>

                            <tbody>
                                {#each tabellaMC as row, i}
                                    <tr class="hover:bg-amber-50">
                                        <th class="border border-amber-400 px-4 py-2 bg-amber-100 font-semibold">UP{i + 1}</th>
                                        {#each row as value, j}
                                            <td class="border border-amber-400 px-1 py-1">
                                                <input
                                                    readonly
                                                    type="number"
                                                    inputmode="numeric"
                                                    class="[&::-webkit-inner-spin-button]:appearance-none w-20 text-center bg-transparent focus:outline-none focus:ring-1 focus:ring-amber-400 rounded"
                                                    bind:value={tabellaMC[i][j]}
                                                />
                                            </td>
                                        {/each}
                                        <td class="border border-amber-400 bg-amber-100 font-semibold text-sm px-4 py-2">
                                            <input
                                                readonly
                                                type="number"
                                                inputmode="numeric"
                                                class="[&::-webkit-inner-spin-button]:appearance-none w-20 text-center bg-transparent focus:outline-none focus:ring-1 focus:ring-amber-400 rounded"
                                                bind:value={produzioniMC[i]}
                                            />
                                        </td>
                                    </tr>
                                {/each}
                            </tbody>

                            <tfoot class="bg-amber-200 font-semibold">
                                <tr>
                                    <th class="border border-amber-400 bg-amber-300">Richieste</th>
                                    {#each tabellaMC[0] as _, j}
                                        <td class="border border-amber-400 px-2 py-2">
                                            <input
                                                readonly
                                                type="number"
                                                inputmode="numeric"
                                                class="[&::-webkit-inner-spin-button]:appearance-none w-20 text-center bg-transparent focus:outline-none focus:ring-1 focus:ring-amber-400 rounded"
                                                bind:value={richiesteMC[j]}
                                            />
                                        </td>
                                    {/each}
                                    <td class="border px-4 py-2 border-amber-400 font-bold transition-all duration-300
                                        {isBalanced ? 'bg-green-300 text-green-950' : 'bg-red-300 text-red-950'}">
                                        {#if isBalanced}
                                            {sommaTotale}
                                        {:else}
                                            {sommaProduzioni} ≠ {sommaRichieste}
                                        {/if}
                                    </td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                {:else}
                    <p class="text-gray-600 text-center italic mt-8">
                        Genera prima una tabella nella sezione Home per visualizzare l'algoritmo Minimi costi.
                    </p>
                {/if}
            </div>
            <button
                class="mt-6 bg-amber-500 text-white font-semibold px-6 py-2 rounded-lg shadow hover:bg-amber-600 transition-all"
                on:click={algoritmoMinimiCostiStep}>
                Esegui Step Algoritmo Minimi costi
            </button>
            <button
                class="mt-6 bg-amber-500 text-white font-semibold px-6 py-2 rounded-lg shadow hover:bg-amber-600 transition-all"
                on:click={algoritmoMinimiCosti}>
                Esegui diretto Algoritmo Minimi costi
            </button>
            <p>costo totale con algoritmo minimi costi: {totaleCostiMC}</p>
        {/if}

        {#if finsetraAttiva === "show"}
            <div class="flex flex-col mt-6 space-y-6">

                <h2 class="text-xl font-bold text-amber-700">Algoritmo Nord-Ovest</h2>
                <p class="text-sm text-gray-700">Step {currentStepNO + 1} di {stepsNO.length}</p>
                {#if stepsNO.length > 0}
                    <table class="border-collapse table-auto min-w-max text-center border border-amber-300">
                        <tbody>
                            {#each stepsNO[currentStepNO] as row, i}
                                <tr>
                                    {#each row as cell}
                                        <td class="border border-amber-400 px-2 py-1">{cell}</td>
                                    {/each}
                                </tr>
                            {/each}
                        </tbody>
                    </table>
                    <div class="flex space-x-2 mt-2">
                        <button on:click={prevStepNO} class="px-4 py-2 bg-amber-500 text-white rounded">Prev</button>
                        <button on:click={nextStepNO} class="px-4 py-2 bg-amber-500 text-white rounded">Next</button>
                    </div>
                    <p class="mt-2 font-semibold">Totale Costi: {totaleCostiNO}</p>
                {:else}
                    <p class="text-gray-600 italic">Esegui prima l'algoritmo Nord-Ovest per vedere gli step.</p>
                {/if}

                <h2 class="text-xl font-bold text-amber-700 mt-6">Algoritmo Minimi Costi</h2>
                <p class="text-sm text-gray-700">Step {currentStepMC + 1} di {stepsMC.length}</p>
                {#if stepsMC.length > 0}
                    <table class="border-collapse table-auto min-w-max text-center border border-amber-300">
                        <tbody>
                            {#each stepsMC[currentStepMC] as row, i}
                                <tr>
                                    {#each row as cell}
                                        <td class="border border-amber-400 px-2 py-1">{cell}</td>
                                    {/each}
                                </tr>
                            {/each}
                        </tbody>
                    </table>
                    <div class="flex space-x-2 mt-2">
                        <button on:click={prevStepMC} class="px-4 py-2 bg-amber-500 text-white rounded">Prev</button>
                        <button on:click={nextStepMC} class="px-4 py-2 bg-amber-500 text-white rounded">Next</button>
                    </div>
                    <p class="mt-2 font-semibold">Totale Costi: {totaleCostiMC}</p>
                {:else}
                    <p class="text-gray-600 italic">Esegui prima l'algoritmo Minimi Costi per vedere gli step.</p>
                {/if}

                {#if stepsNO.length > 0 && stepsMC.length > 0}
                    <div class="mt-4 p-4 bg-amber-100 rounded">
                        <p class="font-bold text-lg">Confronto Costi Totali</p>
                        {#if totaleCostiNO < totaleCostiMC}
                            <p>Nord-Ovest è migliore: {totaleCostiNO} &lt; {totaleCostiMC}</p>
                        {:else if totaleCostiMC < totaleCostiNO}
                            <p>Minimi Costi è migliore: {totaleCostiMC} &lt; {totaleCostiNO}</p>
                        {:else}
                            <p>I costi totali sono uguali: {totaleCostiNO} = {totaleCostiMC}</p>
                        {/if}
                    </div>
                {/if}

            </div>
        {/if}

    </div>

    <!-- FOOTER -->
    <footer class="h-[5%] bg-amber-600 flex items-center justify-center text-white text-sm">
        <p class="text-center">
            by Bamba Amara, Biglioli Enea, Lamberto Luigi & Zanga Alessandro
        </p>
    </footer>
</div>