<template>
  <div class="article-container" v-if="recipe">
    <div class="panel">
      <div class="section">
        ID: {{ recipe.id }}
        <button @click="saveRecipe" class="save-button" type="button">Save</button>
        <div class="section-title">
          <text-input v-model="recipe.title" :label="'title'" />
        </div>
        <div>
          <strong>Author:</strong> {{ recipe.author }} <strong>Time:</strong>
          <number-input
            v-model="recipe.durationMinutes"
            :label="'duration minutes'"
          />
          minutes
          <strong>Yields:</strong>
          <number-input v-model="recipe.servings" :label="'servince'" />
          servings
        </div>
        <p>
          <textbox-input v-model="recipe.description" :label="'description'" />
        </p>
      </div>

      <div class="section-dark">Mode: Public | Draft</div>

      <div class="section">
        <h3>Ingredients</h3>
        TBD
      </div>

      <template v-for="component in recipe.components">
        ID: {{ component.id }}
        <div class="section-dark" :key="component.id + 'h'">
          <button
            class="remove-component-button"
            @click="removeComponent(component)"
          >
            D
          </button>
          <h3>
            <text-input v-model="component.title" :label="'title'" />
          </h3>
        </div>

        <div
          v-if="component.description"
          class="section"
          :key="component.id + 'd'"
        >
          <textbox-input
            v-model="component.description"
            :label="'description'"
          />
        </div>

        <div
          class="section"
          v-for="step in component.steps"
          :key="component.id + step.id"
        >
          <button
            class="remove-step-button"
            @click="removeStep(component, step)"
          >
            D
          </button>
          ID: {{ step.id }}
          <h4>
            <text-input v-model="step.title" :label="'title'" />
          </h4>
          <div
            class="ingredient"
            v-for="ingredient in step.ingredients"
            :key="ingredient.id"
          >
            <button @click="removeIngredient(step, ingredient)">D</button>
            ID: {{ ingredient.id }}
            <text-input v-model="ingredient.name" :label="'ingredient'" /> | 
            <text-input v-model="ingredient.unit" :label="'unit'" /> | 
            <number-input v-model="ingredient.amount" :label="'amount'" />
          </div>
          <button type="button" @click="addIngredient(step)">
            + ingredient
          </button>
          <p>
            <textbox-input v-model="step.body" :label="'step description'" />
          </p>
        </div>
      </template>
      <div class="section-dark">
        <button class="add-step-button" @click="addStep">+ step</button>
        <button class="add-component-button" @click="addComponent">
          + component
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import RecipeApi, {
  RecipeComponentDto,
  RecipeDto,
  RecipeIngredientDto,
  RecipeStepDto,
} from "../scripts/recipeApi";
import TextboxInput from "../components/cms/TextBoxInput.vue";
import TextInput from "../components/cms/TextInput.vue";
import NumberInput from "../components/cms/NumberInput.vue";

const recipeApi = new RecipeApi();

export default {
  components: { TextboxInput, TextInput, NumberInput },
  props: {
    id: String,
  },
  data: () => ({
    recipe: null,
  }),
  async mounted() {
    this.recipe = await recipeApi.getRecipeDetails(this.id);
  },
  methods: {
    async saveRecipe() {
      // recipeApi.updateRecipe()
    },
    removeComponent(component) {
      let index = this.recipe.components.indexOf(component);
      this.recipe.components.splice(index, 1);
    },
    addComponent() {
      this.recipe.components.push(new RecipeComponentDto(null, "", "", []));
    },
    removeStep(component, step) {
      let index = component.steps.indexOf(step);
      component.steps.splice(index, 1);
    },
    addStep(component) {
      component.steps.push(new RecipeStepDto(null, "", "", []));
    },
    addIngredient(step) {
      step.ingredients.push(new RecipeIngredientDto(null, "", "", 0));
    },
    removeIngredient(step, ingredient) {
      let index = step.ingredients.indexOf(ingredient);
      step.ingredients.splice(index, 1);
    },
  },
};
</script>

<style scoped>
/* .remove-component-button {} */
/* .add-component-button {} */
</style>