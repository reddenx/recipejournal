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
    min-width: 40rem;
    grid-column-start: 2;
    grid-row-start: 1;
}
.layout-nav {
    /*  */
    text-align: center;
}
.layout-footer {
}

button.trash {
    width: 2.3em;
    height: 2.3em;
    border-radius: 8px;
    border: 3px solid #e20000;
    background-color: #ff8686;
}
button.edit {
    width: 2.3em;
    height: 2.3em;
    border-radius: 8px;
    border: 3px solid rgb(73, 196, 69);
    background-color: rgb(66, 230, 107);
}
button.save-button {
    width: 2.3em;
    height: 2.3em;
    border-radius: 8px;
    border: 3px solid rgb(45, 97, 175);
    background-color: rgb(103 165 255);
}
a.value {
    cursor: pointer;
}

@media only screen and (max-width: 800px) {
    .layout-container {
        display: grid;
        grid-template-columns: auto;
        grid-template-rows: auto;
        margin-left: 0.25em;
        margin-right: 0.25em;
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