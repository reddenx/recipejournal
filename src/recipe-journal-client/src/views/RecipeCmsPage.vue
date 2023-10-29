<template>
  <div class="article-container" v-if="recipe">
    <div class="panel">
      <div class="section">
        <!-- ID: {{ recipe.id ? recipe.id : 'NEW!' }} -->
        <button @click="saveRecipe" class="save-button" type="button">
          <i class="fa-regular fa-floppy-disk"></i>
        </button>
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
        <toggle-input v-model="recipe.isPublic" :enabledText="'Public'" :disabledText="'Private'" />
         | 
         <toggle-input v-model="recipe.isDraft" :enabledText="'Draft'" :disabledText="'Published'" />
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
            class="trash"
            @click="removeComponent(component)"
          >
            <span class="fa-solid fa-trash-can"></span>
          </button>
          <h3 class="component-title">
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
            class="trash"
            @click="removeStep(component, step)"
          >
            <span class="fa-solid fa-trash-can"></span>
          </button>
          <!-- ID: {{ step.id ? step.id : 'NEW' }} -->
          <h4 class="step-title">
            <text-input v-model="step.title" :label="'title'" />
          </h4>
          <div
            class=""
            v-for="(ingredient, k) in step.ingredients"
            :key="ingredient.id + i + '-' + j + '-' + k"
          >
            <button class="trash" @click="removeIngredient(step, ingredient)">
              <span class="fa-solid fa-trash-can"></span>
            </button>
            <!-- ID: {{ ingredient.id ? ingredient.id : 'NEW' }} -->
            <text-input v-model="ingredient.name" :label="'ingredient'" :length="8" /> | 
            <text-input v-model="ingredient.unit" :label="'unit'" :length="3" /> | 
            <number-input v-model="ingredient.amount" :label="'amount'" :length="3" />
          </div>
          <br>
          <button type="button" class="edit add-ingredient" @click="addIngredient(step)">+ Ingredient</button>
          <p>
            <textbox-input v-model="step.body" :label="'step description'" />
          </p>
        </div>
        <div class="section-dark" :key="component.id + 'n' + i">
          <button class="add-step-button edit" @click="addStep(component)">+ Step</button>
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
    saving: false,
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
      if(this.saving)
        return;
      this.saving = true;


      //renumber to match cms UI
      this.recipe.components.forEach((c, i) => {
        c.number = i;
        c.steps.forEach((s, j) => {
          s.number = j;
          s.ingredients.forEach((g, k) => {
            g.number = k;
          });
        });
      });
      
      let result = await recipeApi.updateRecipe(this.recipe);
      //if we just saved a new recipe, change url
      if(!this.id && result) {
        this.$router.replace({path:`/cms/${result.id}`});
      }
      this.recipe = result;
      this.saving = false;
    },
    removeComponent(component) {
      let index = this.recipe.components.indexOf(component);
      this.recipe.components.splice(index, 1);
    },
    addComponent() {
      let nextNumber = Math.max(this.recipe.components.map(s => s.number)) + 1;
      this.recipe.components.push(new RecipeComponentDto(null, nextNumber, "", "", []));
    },
    removeStep(component, step) {
      let index = component.steps.indexOf(step);
      component.steps.splice(index, 1);
    },
    addStep(component) {
      let nextNumber = Math.max(component.steps.map(s => s.number)) + 1;
      component.steps.push(new RecipeStepDto(null, nextNumber, "", "", []));
    },
    addIngredient(step) {
      let nextNumber = Math.max(step.ingredients.map(s => s.number)) + 1;
      step.ingredients.push(new RecipeIngredientDto(null, nextNumber, "", "", 0));
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
.section-title {
  display: inline-block;
}
.component-title {
  display: inline-block;
  margin-left: 1em;
  text-align: center;
}
.step-title {
  display: inline-block;
  margin-left: 1em;
}
.add-step-button,.add-ingredient {
  width: initial;
}
</style>