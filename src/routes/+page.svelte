<script lang="ts">
    let finsetraAttiva = "home";
    const tabella: number[][] = [];
    const tabellaNO: number[][] = [];
    const tabellaMC: number[][] = [];
    const stepsNO: number[][][] = [];
    const stepsMC: number[][][] = [];
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

    let rigaTemp : number | null = null;
    let colonnaTemp: number | null = null;
    let riga: number | null = null;
    let colonna: number | null = null;
    $: riga = rigaTemp;
    $: colonna = colonnaTemp;

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
        tabellaNO.length = 0;
        tabellaMC.length = 0;
        if (!riga || !colonna) return;
        for (let i = 0; i < riga; i++) {
            tabellaNO[i] = [];
            tabellaMC[i] = [];
            for (let j = 0; j < colonna; j++) {
                tabellaNO[i][j] = tabella[i][j];
                tabellaMC[i][j] = tabella[i][j];
            }
        }
        for (let i = 0; i < riga; i++) {
            for (let j = 0; j < colonna; j++) {
                tabellaNO[i][j] = tabella[i][j];
                tabellaMC[i][j] = tabella[i][j];
                stepsNO[0][i][j] = tabella[i][j];
                stepsMC[0][i][j] = tabella[i][j];
            }
        }
        richiesteMC = [...richieste];
        produzioniMC = [...produzioni];
        richiesteNO = [...richieste];
        produzioniNO = [...produzioni];
    }

    function randomizeValues() {
        tabella.length = 0;
        if (!riga || !colonna) return;
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
        // Implementazione dell'algoritmo del Nord-Ovest
    }
    
    function algoritmoMinimiCosti() {
        // Implementazione dell'algoritmo dei Minimi Costi
    }
    
    function algoritmoNordOvestStep() {
        if (tabella.length === 0) return;
        if (!isBalanced) return;
        if (richiesteNO[0] === produzioniNO[0]) {
            totaleCostiNO += tabellaNO[0][0]*richiesteNO[0];
            richiesteNO.shift();
            produzioniNO.shift();
        } else
        if (richiesteNO[0] < produzioniNO[0]) {
            totaleCostiNO += tabellaNO[0][0]*richiesteNO[0];
            produzioniNO[0] -= richiesteNO[0];
            richiesteNO.shift();
            tabellaNO.shift();
        }else {
            totaleCostiNO += tabellaNO[0][0]*produzioniNO[0];
            richiesteNO[0] -= produzioniNO[0];
            produzioniNO.shift();
            tabellaNO.forEach(riga => {
                
            });       
        }
    }
    
    function algoritmoMinimiCostiStep() {
        // Implementazione step by step
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
            { name: "Algoritmo Minimi Costi", key: "algoritmo minimi costi" },
            { name: "Show", key: "show" }
        ] as tab}
            <button
                class="px-4 py-2 rounded-lg font-medium transition-all duration-200
                {finsetraAttiva === tab.key
                    ? 'bg-amber-200 text-gray-900 shadow-inner'
                    : 'bg-amber-100 hover:bg-amber-200'}"
                on:click={() => {finsetraAttiva = tab.key; setTabelle();}}>
                {tab.name}
            </button>
        {/each}
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
            <h1 class="text-3xl font-bold mt-10 text-amber-700">Algoritmo del Nord-Ovest</h1>
            <p class="mt-4 text-gray-700 text-center max-w-xl">
                Qui potrai visualizzare e calcolare il piano iniziale di trasporto secondo il metodo del Nord-Ovest.
            </p>
        {/if}

        {#if finsetraAttiva === "algoritmo minimi costi"}
            <h1 class="text-3xl font-bold mt-10 text-amber-700">Algoritmo dei Minimi Costi</h1>
            <p class="mt-4 text-gray-700 text-center max-w-xl">
                Qui verrà mostrata la soluzione iniziale basata sulla minimizzazione dei costi di trasporto.
            </p>
        {/if}

        {#if finsetraAttiva === "show"}
            <h1 class="text-3xl font-bold mt-10 text-amber-700">Visualizzazione Dati</h1>
            <p class="mt-4 text-gray-700 text-center max-w-xl">
                In questa sezione potrai visualizzare i risultati dei vari algoritmi e confrontarli tra loro.
            </p>
        {/if}
    </div>

    <!-- FOOTER -->
    <footer class="h-[5%] bg-amber-600 flex items-center justify-center text-white text-sm">
        <p class="text-center">
            by Bamba Amara, Biglioli Enea, Lamberto Luigi & Zanga Alessandro
        </p>
    </footer>
</div>