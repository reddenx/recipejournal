<template>
  <div class="page-container">
    <h1>Recipes</h1>
    <div class="recipe" v-for="recipe in recipes" :key="recipe.id">
      <router-link :to="'/cook/' + recipe.id"
        >{{ recipe.title }} - {{ recipe.durationMinutes }}m -
        {{ recipe.servings }}</router-link
      > --
      <router-link :to="'/cms/' + recipe.id">edit</router-link>
    </div>
    <router-link :to="'/cms/'">NEW</router-link>
  </div>
</template>

<script>
import RecipeApi from "../scripts/recipeApi";

const recipeApi = new RecipeApi();

export default {
  data: () => ({
    recipes: [],
  }),
  async mounted() {
    this.recipes = await recipeApi.getRecipeList();
  },
};
</script>

<style>
</style>