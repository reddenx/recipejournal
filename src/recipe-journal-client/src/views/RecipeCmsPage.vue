<template>
  <div class="article-container" v-if="recipe">
    <div class="panel">
      <div class="section">
        <!-- ID: {{ recipe.id ? recipe.id : 'NEW!' }} -->
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

      <div class="section-dark">
        Mode: 
        <toggle-input v-model="recipe.isPublic" :enabledText="'PUBLIC'" :disabledText="'PRIVATE'" />
         | 
         <toggle-input v-model="recipe.isDraft" :enabledText="'DRAFT'" :disabledText="'PUBLISHED'" />
      </div>

      <div class="section">
        <h3>Ingredients</h3>
        <div v-for="(ingredient, i ) in ingredients" :key="ingredient.id + '-' + i">
          {{ingredient.name ? ingredient.name : 'INGREDIENT?'}} {{ingredient.amount}} {{getUnitForQty(ingredient.unit, ingredient.amount)}}
        </div>
      </div>

      <template v-for="(component, i) in recipe.components">
        <!-- ID: {{ component.id ? component.id : 'NEW' }} -->
        <div class="section-dark" :key="component.id + 'h' + i">
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
          class="section"
          :key="component.id + 'd' + i"
        >
          <textbox-input
            v-model="component.description"
            :label="'description'"
          />
        </div>

        <div
          class="section"
          v-for="(step, j) in component.steps"
          :key="component.id + step.id + i + '-' + j"
        >
          <button
            class="remove-step-button"
            @click="removeStep(component, step)"
          >
            D
          </button>
          <!-- ID: {{ step.id ? step.id : 'NEW' }} -->
          <h4>
            <text-input v-model="step.title" :label="'title'" />
          </h4>
          <div
            class=""
            v-for="(ingredient, k) in step.ingredients"
            :key="ingredient.id + i + '-' + j + '-' + k"
          >
            <button @click="removeIngredient(step, ingredient)">D</button>
            <!-- ID: {{ ingredient.id ? ingredient.id : 'NEW' }} -->
            <text-input v-model="ingredient.name" :label="'ingredient'" :length="8" /> | 
            <text-input v-model="ingredient.unit" :label="'unit'" :length="3" /> | 
            <number-input v-model="ingredient.amount" :label="'amount'" :length="3" />
          </div>
          <button type="button" @click="addIngredient(step)">
            + ingredient
          </button>
          <p>
            <textbox-input v-model="step.body" :label="'step description'" />
          </p>
        </div>
        <div class="section-dark" :key="component.id + 'n' + i">
          <button class="add-step-button" @click="addStep(component)">+ step</button>
        </div>
      </template>
      <div class="section">
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
import ToggleInput from '../components/cms/ToggleInput.vue';
import { compressIngredients } from "./CookingPage.vue";
import Units from '../scripts/units';

const recipeApi = new RecipeApi();

export default {
  components: { TextboxInput, TextInput, NumberInput, ToggleInput },
  props: {
    id: String,
  },
  data: () => ({
    recipe: null,
    ingredients: [],
  }),
  watch: {
    recipe: {
      deep: true,
      handler() {
        this.ingredients = compressIngredients(this.recipe);
      }
    },
  },
  async mounted() {
    if(this.id) {
      this.recipe = await recipeApi.getRecipeDetails(this.id);
    } else {
      this.recipe = new RecipeDto(null, '', '', 'sean', 0, 0, [], true, false);
    }
  },
  methods: {
    getUnitForQty(unit, amount) {
      return Units.getUnitForQty(unit, amount);
    },
    async saveRecipe() {
      await recipeApi.updateRecipe(this.recipe);
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