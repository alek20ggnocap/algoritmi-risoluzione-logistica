<script lang="ts">
    $: finsetraAttiva = "home";
    const tabella: number[][] = [];
</script>

<div class="h-full w-full overflow-x-hidden overflow-y-hidden">
    <header class="h-[5%] bg-amber-600">
        
    </header>
    <div class="h-[90%]">
        <div>

        </div>
        
        {#if finsetraAttiva === "home"}
            <h1 class="text-4xl text-center mt-10">Benvenuto nella pagina principale!</h1>
            
        {/if}
        {#if finsetraAttiva === "algoritmo nord ovest"}
            <h1 class="text-4xl text-center mt-10">Informazioni su di noi</h1>
            
        {/if}
        {#if finsetraAttiva === "algoritmo minimi costi"}
            <h1 class="text-4xl text-center mt-10">Contattaci</h1>
            
        {/if}
        {#if finsetraAttiva === "show"}
            <h1 class="text-4xl text-center mt-10">FAQ</h1>
            
        {/if}
    </div>
    <footer class="h-[5%] bg-amber-600">
        <div class="flex flex-row items-center justify-center h-full">
            <p>
                by Bamba Amara, Biglioli Enea, Lamberto Luigi & Zanga Alessandro
            </p>
        </div>
    </footer>
</div>