<template>
    <div>
        <router-link to="/">
            <img alt="Vue logo" src="../assets/logo.png" class="hero-logo-img" />
        </router-link> | 
        <router-link to="/cook">Recipes</router-link> |
        <template v-if="isLoggedIn">
            <!-- <router-link to="/journal">Journal</router-link> | -->
            <!-- <router-link to="/goals">Goals</router-link> | -->
            <router-link to="/groceries">Groceries</router-link> |
        </template>
        <button type="button" v-show="!showLoginComponent && !isLoggedIn" @click="showLoginButtonPressed"> -&gt; </button>
        <div class="nav-inline" v-show="showLoginComponent || isLoggedIn">
            <login-component @login="handleLogin" @logout="handleLogout" />
        </div>
    </div>
</template>

<script>
import LoginComponent from "../components/LoginComponent.vue";

export default {
    components: { LoginComponent },
    data: () => ({
        isLoggedIn: false,
        showLoginComponent: false,
    }),
    methods: {
        handleLogin(username) {
            this.isLoggedIn = true;
        },
        handleLogout() {
            this.isLoggedIn = false;
        },
        showLoginButtonPressed() {
            this.showLoginComponent = true;
        }
    },
};
</script>

<style local>
.nav-inline {
    display: inline-block;
}
.hero-logo-img {
    width: 3em;
    height: 3em;
}
</style>