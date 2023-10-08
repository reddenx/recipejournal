<template>
    <div class="login-container">
        <div v-if="!user" class="loggedout-container">
            <input type="text" v-model="username" :disabled="busy" placeholder="username" />
            <input type="password" v-model="password" :disabled="busy" placeholder="pasword" />
            <button
                type="button"
                @click="login"
                :disabled="busy"
            >
                login
            </button>
        </div>
        <div v-if="user" class="loggedin-container">
            <!-- <router-link to="account">{{ user.username }}</router-link> -->
            {{ user.username }}
            <button type="button" @click="logout" :disabled="busy">
                logout
            </button>
        </div>
    </div>
</template>

<script>
import UserApi from "../scripts/userApi";

const userApi = new UserApi();

export default {
    components: {},
    data: () => ({
        username: "",
        password: "",
        busy: true,
        user: null,
    }),
    async mounted() {
        this.busy = true;
        this.user = await userApi.getLoggedInUser();
        if(this.user)
            this.$emit('login', this.user.username);
        this.busy = false;
    },
    methods: {
        async login() {
            this.busy = true;
            let success = await userApi.login(this.username, this.password);
            if (success) {
                this.user = await userApi.getLoggedInUser();
                this.$emit('login', this.user.username);
            }
            this.busy = false;
        },
        async logout() {
            this.busy = true;
            let success = await userApi.logout();
            this.user = null;
            this.username = "";
            this.$emit('logout');
            this.busy = false;
        },
    },
};
</script>

<style scoped>
.login-container {
    display: inline-block;
}
.loggedout-container {
    display: inline-block;
}
.loggedin-container {
    display: inline-block;
}
</style>