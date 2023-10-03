<template>
  <div>
    <div v-if="!user">
      <input type="text" v-model="username" /><button type="button" @click="login">
        login
      </button>
    </div>
    <div v-if="user">
      {{ user.username }}
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
  },
};
</script>

<style>
</style>