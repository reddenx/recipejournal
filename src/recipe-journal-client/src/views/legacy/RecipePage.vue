<template>
  <div class="container">
    <h2>{{ recipe.title }}</h2>
    <p>
      {{ recipe.description }}
    </p>

    <div v-for="component in recipe.components" :key="component.name">
      <h3>{{ component.name }}</h3>
      <div class="row">
        <div class="col-auto">
          <div class="card" v-show="component.ingredients.length">
            <div class="card-body">
              <div
                v-for="(ingredient, index) in component.ingredients"
                :key="index"
              >
                {{ ingredient.amount }}{{ ingredient.unit }} -
                {{ ingredient.name }}
              </div>
            </div>
          </div>
        </div>
      </div>
      <p v-for="(instruction, index) in component.instructions" :key="index">
        {{ instruction.description }}
      </p>
    </div>
  </div>
</template>

<script>
import RecipeApi, { RecipeDto } from "../scripts/recipe_api-proto";

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