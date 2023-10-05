<template>
    <div class="container">
        <h1>Welcome, it's time to cook!</h1>
        <img style="float:right" alt="Vue logo" src="../assets/logo.png" />
        <div style="">
            Features Overview
            <ul>
                <li>Simple recipe layout with scaling calculator</li>
                <li>Recipe CMS for easy input</li>
                <li>Easy shopping list</li>
                <li>Recipe journaling gathering notes for what happened and what to change in the future</li>
                <li>Cooking Goals</li>
            </ul>
        </div>
    </div>
</template>

<script>
export default {
    name: "HomeView",
    data: () => ({}),
    async mounted() {},
    methods: {},
};
</script>

<style scoped>
.container {
    max-width: 40rem;
}
</style>