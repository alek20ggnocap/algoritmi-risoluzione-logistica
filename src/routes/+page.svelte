<script lang="ts">
    let finsetraAttiva = "home";
    const tabella: number[][] = [];
    let sommeRighe: number[] = [];
    let sommeColonne: number[] = [];
    let sommaTotale: number = 0;

    let rigaTemp : number | null = null;
    let colonnaTemp: number | null = null;
    $: riga = rigaTemp;
    $: colonna = colonnaTemp; 
    function randomizeValues() {
        for (let i = 0; riga != null && i < riga; i++) {
            tabella[i] = [];
            for (let j = 0; colonna != null && j < colonna; j++) {
                tabella[i][j] = Math.floor(Math.random() * 100);
            }
        }
        console.log(tabella);
    }

    async function calcolaSomme() {
        sommeRighe = tabella.map(riga => riga.reduce((a, b) => a + b, 0));
        sommeColonne = tabella[0].map((_, colIndex) => tabella.reduce((sum, row) => sum + row[colIndex], 0));
        sommaTotale = sommeRighe.reduce((a, b) => a + b, 0);
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
                on:click={() => finsetraAttiva = tab.key}>
                {tab.name}
            </button>
        {/each}
    </nav>

    <!-- CONTENUTO PRINCIPALE -->
    <div class="flex-1 flex flex-col items-center justify-start p-6 overflow-x-auto overflow-y-auto">
        {#if finsetraAttiva === "home"}
            <div class="flex flex-col items-center mt-6 space-y-6">
                <div class="text-center items-center flex row">
                    <p class="p-4">righe</p>
                    <input type="number" placeholder="n-row" class="w-24 text-center mr-4 p-2 border border-amber-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-amber-400" bind:value={riga} />
                    <p class="p-4">colonne</p>
                    <input type="number" placeholder="n-col" class="w-24 text-center mr-4 p-2 border border-amber-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-amber-400" bind:value={colonna} />
                    <button
                        class="bg-amber-500 text-white font-semibold px-6 py-2 rounded-lg shadow hover:bg-amber-600 transition-all"
                        on:click={() => {randomizeValues(); calcolaSomme();}}>
                        Genera Valori Casuali
                    </button>
                </div>
                

                {#if tabella.length > 0}
                    <div class="">
                        <table class="border-collapse border border-amber-400 shadow-lg rounded-lg text-center">
                            <thead class="bg-amber-200">
                                <tr>
                                    <th class="border border-amber-400 w-24 bg-amber-300"></th>
                                    {#each tabella[0] as _, j}
                                        <th class="border border-amber-400 px-4 py-2 font-semibold">D{j + 1}</th>
                                    {/each}
                                    <th class="border border-amber-400 px-4 py-2 font-semibold bg-amber-300">Σ Riga</th>
                                </tr>
                            </thead>
                            <tbody>
                                {#each tabella as row, i}
                                    <tr class="hover:bg-amber-50">
                                        <th class="border border-amber-400 px-4 py-2 bg-amber-100 font-semibold">UP{i + 1}</th>
                                        {#each row as value}
                                            <td class="border border-amber-400">
                                                <input
                                                    type="number"
                                                    class="[&::-webkit-inner-spin-button]:appearance-none w-16 text-center bg-transparent focus:outline-none focus:ring-1 focus:ring-amber-400 rounded"
                                                    bind:value={value}
                                                    on:input={calcolaSomme}
                                                />
                                            </td>
                                        {/each}
                                        <td class="border border-amber-400 bg-amber-100 font-semibold text-sm">
                                            {sommeRighe[i]}
                                        </td>
                                    </tr>
                                {/each}
                            </tbody>
                            <tfoot class="bg-amber-200 font-semibold">
                                <tr>
                                    <th class="border border-amber-400 bg-amber-300">Σ Colonne</th>
                                    {#each sommeColonne as somma}
                                        <td class="border px-4 py-2 border-amber-400">{somma}</td>
                                    {/each}
                                    <td class="border px-4 py-2 border-amber-400 bg-amber-300 font-bold">{sommaTotale}</td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                {:else}
                    <p class="text-gray-600 italic">Clicca il pulsante per generare la tabella.</p>
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
