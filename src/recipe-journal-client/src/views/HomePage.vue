<template>
  <div class="container">
    <div class="row">
      <div class="col">
        <img alt="Vue logo" src="../assets/logo.png" />
      </div>
      <div class="col">the sean recipe list!</div>
    </div>
    <div class="row">
      <div class="col">recent entries: TODO (put summary cards here)</div>
    </div>
    <div class="row">
      <div class="col">
        <ul>
          <li v-for="recipe in recipes" :key="recipe.id">
            <router-link :to="'recipes/' + recipe.id">{{
              recipe.title
            }}</router-link>
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>

<script>
import RecipeApi from "../scripts/recipe_api-proto";

export default {
  name: "HomeView",
  components: {},
  data: () => ({
    recentEntries: [],
    recipes: [],
  }),
  async mounted() {
    let api = new RecipeApi();

    //let recentEntries = await api.getRecentJournalEntries();
    //this.recentEntries = recentEntries;

    let recipes = await api.getRecipeList();
    console.log(recipes);
    this.recipes = recipes;
  },
  methods: {},
};
</script>
