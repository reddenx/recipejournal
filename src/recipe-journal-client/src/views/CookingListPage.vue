<template>
  <div class="page-container">
    <h1>Recipes</h1>
    <div class="recipe" v-for="recipe in recipes" :key="recipe.id">
      <router-link :to="'/cook/' + recipe.id">{{ recipe.title }} - {{ recipe.durationMinutes }}m -
        {{ recipe.servings }}</router-link>
      
    </div>
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