<template>
    <div class="container">
        <div class="row">
            <div class="col">
                <img alt="Vue logo" src="../assets/logo.png" />
            </div>
            <div class="col">the recipe list!
                <login-component />
            </div>
        </div>
        <div class="row">
            <div class="col">recent entries: TODO (put summary cards here)</div>
        </div>
        <div class="row">
            <div class="col">
                <router-link :to="'cook'">Recipe list here!</router-link>
            </div>
        </div>
    </div>
</template>

<script>
import LoginComponent from '../components/LoginComponent.vue';
import RecipeApi from "../scripts/recipe_api-proto";

export default {
    name: "HomeView",
    components: {LoginComponent},
    data: () => ({}),
    async mounted() {},
    methods: {},
};
</script>
