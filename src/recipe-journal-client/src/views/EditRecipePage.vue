<template>
  <div class="container">
      <RecipeInput v-model="recipe" />
  </div>
</template>

<script>
import RecipeInput from "../components/RecipeInput";
import RecipeApi, { RecipeDto } from "../scripts/recipe_api";

export default {
  components: { RecipeInput },
  props: {
      recipeId: String,
  },
  data: () => ({
    recipe: {},
  }),
  async mounted() {
    let api = new RecipeApi();
    this.recipe = await api.getRecipe(this.recipeId);
  },
  methods: {},
};
</script>

<style>
</style>