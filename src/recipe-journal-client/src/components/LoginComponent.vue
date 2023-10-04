<template>
  <div>
    <div v-if="!user">
      <input type="text" v-model="username" :disabled="busy" /><button type="button" @click="login" :disabled="busy">
        login
      </button>
    </div>
    <div v-if="user">
      {{ user.username }}
      <button type="button" @click="logout" :disabled="busy" >logout</button>
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
    busy: true,
    user: null,
  }),
  async mounted() {
    this.user = await userApi.getLoggedInUser();
    this.busy = true;
  },
  methods: {
    async login() {
      this.busy = true;
      let success = await userApi.login(this.username);
      if (success) {
        this.user = await userApi.getLoggedInUser();
      }
      this.busy = false;
    },
    async logout() {
      this.busy = true
      let success = await userApi.logout();
      this.user = null;
      this.username = "";
      this.busy = false;
    }
  },
};
</script>

<style>
</style>