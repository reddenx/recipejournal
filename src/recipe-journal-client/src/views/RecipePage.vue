<template>
  <div class="container">
    <div class="row">
      <div class="col">Recipe Page id:{{ recipeId }}</div>
      <div class="col">
        ingredients:
        <ul
          v-for="component in recipe.components"
          :key="component.order + component.name"
        >
          <li>
            {{ component.name }}
          </li>
          <li
            v-for="ingredient in component.ingredients"
            :key="ingredient.name + ingredient.amount"
          >
            {{ ingredient.amount }}{{ ingredient.unit }} - {{ ingredient.name }}
          </li>
        </ul>
      </div>
      <div class="col">
        <div
          v-for="component in recipe.components"
          :key="component.order + component.name"
        >
          {{ component.name }}: {{ component.instructions }}
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import RecipeApi, { RecipeDto } from "../scripts/recipe_api";

export default {
  props: {
    recipeId: String,
  },
  data: () => ({
    recipe: {},
  }),
  async mounted() {
    let api = new RecipeApi();
    let recipe = await api.getRecipe(this.recipeId);
    this.loadRecipe(recipe);
  },
  methods: {
    /** @param {RecipeDto} recipeDto */
    loadRecipe(recipeDto) {
      this.recipe = recipeDto;
    },
  },
};
</script>

<style>
</style>