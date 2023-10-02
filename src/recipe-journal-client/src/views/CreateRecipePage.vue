<template>
  <div class="container">
    <div class="row">
      <div class="col">
        <RecipeInput v-model="newRecipe" />
      </div>
    </div>
  </div>
</template>

<script>
import RecipeInput from "../components/RecipeInput";
import { RecipeDto } from "../scripts/recipe_api-proto";

export default {
  components: { RecipeInput },
  data: () => ({
    newRecipe: {},
  }),
  mounted() {
    this.newRecipe = new RecipeDto(null, "NEW RECIPE", "", []);
  },
  methods: {},
};
</script>

<style>
</style>