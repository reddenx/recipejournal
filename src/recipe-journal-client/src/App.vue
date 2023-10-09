<template>
    <div id="app">
        <div class="layout-nav">
            <navigation-bar />
        </div>
        <router-view />
    </div>
</template>

<script>
import NavigationBar from "./components/NavigationBar.vue";

export default {
    components: { NavigationBar },
    data: () => ({
        test: "hi",
    }),
};
</script>

<style>
.layout-container {
    display: grid;
    grid-template-columns: 1fr auto 1fr;
}
.layout-leftbar {
    /* from 160px+ stacks vertically after that*/
    grid-column-start: 1;
    grid-row-start: 1;
}
.layout-rightbar {
    /* from 160px+ stacks vertically after that*/
    grid-column-start: 3;
    grid-row-start: 1;
}
.layout-body {
    min-width: 30rem;
    grid-column-start: 2;
    grid-row-start: 1;
}
.layout-nav {
    /*  */
    text-align: center;
}
.layout-footer {
}

@media only screen and (max-width: 800px) {
    .layout-container {
        display: grid;
        grid-template-columns: auto;
        grid-template-rows: auto;
        margin-left: .25em;
        margin-right: .25em;
    }
    .layout-leftbar {
        /* from 160px+ stacks vertically after that*/
        grid-column-start: 1;
        grid-row-start: 1;
    }
    .layout-rightbar {
        /* from 160px+ stacks vertically after that*/
        grid-column-start: 1;
        grid-row-start: 2;
    }
    .layout-body {
        min-width: inherit;
        grid-column-start: 1;
        grid-row-start: 3;
    }
}
</style>