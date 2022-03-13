<template>
  <div>
    <div class="card mb-3">
      <div class="card-body">
        <span v-show="recipe.id">ID: {{ recipe.id }}</span>

        Title:
        <input
          type="text"
          class="form-control mb-3"
          placeholder="Recipe Name"
          v-model="recipe.title"
        />

        Description:
        <textarea
          type="text"
          class="form-control mb-3"
          placeholder="Description"
          v-model="recipe.description"
        />
      </div>
    </div>

    <div class="card">
      <div class="card-body">
        Components:
        <div>
          <div
            class="card"
            v-for="component in recipe.components"
            :key="component.name + component.order"
          >
            <div class="card-body">
              Ingredients:
              <div class="card mb-3">
                <div class="card-body">
                  <ul>
                    <li
                      v-for="(ingredient, index) in component.ingredients"
                      :key="index"
                    >
                      <input
                        type="text"
                        class="form-control"
                        v-model="ingredient.name"
                        placeholder="name"
                      />
                      <input
                        type="number"
                        class="form-control"
                        v-model="ingredient.amount"
                        placeholder="amount"
                      />
                      <input
                        type="text"
                        class="form-control"
                        v-model="ingredient.unit"
                        placeholder="unit"
                      />
                      <button
                        type="button"
                        class="btn btn-danger"
                        @click="deleteIngredient(component, ingredient)"
                      >
                        DELETE
                      </button>
                    </li>
                  </ul>
                  <button
                    type="button"
                    class="btn btn-outline-secondary"
                    @click="addIngredient(component)"
                  >
                    + Ingredient
                  </button>
                </div>
              </div>

              Instructions:

<!-- you left off here building out the instructions add/remove/order inputs -->

              <textarea class="form-control"> </textarea>
              <button
                type="button"
                class="btn btn-outline-secondary"
                @click="addInstruction(component)"
              >
                +Instruction
              </button>
            </div>
          </div>

          <button
            type="button"
            class="btn btn-outline-secondary"
            @click="addComponent"
          >
            + Add Section
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import {
  Ingredient,
  Instruction,
  RecipeComponentDto,
  RecipeDto,
} from "../scripts/recipe_api";

export default {
  props: {
    value: Object,
  },
  data: () => ({
    /** @type {RecipeDto} */
    recipe: new RecipeDto(null, "", "", []),
  }),
  watch: {
    value() {
      this.recipe = this.value;
    },
  },
  mounted() {
    this.recipe = this.value;
  },
  methods: {
    save() {},
    addComponent() {
      let largestComponentOrder = 0;
      this.recipe.components.forEach((c) =>
        c.order > largestComponentOrder
          ? (largestComponentOrder = c.order)
          : null
      );
      this.recipe.components.push(
        new RecipeComponentDto(
          "Component Name",
          largestComponentOrder + 1,
          [],
          []
        )
      );
    },
    addIngredient(component) {
      component.ingredients.push(new Ingredient("", null, ""));
    },
    deleteIngredient(component, ingredient) {
      component.ingredients = component.ingredients.filter(
        (i) => i !== ingredient
      );
    },
    /** @param {RecipeComponentDto} component */
    addInstruction(component) {
      component.instructions.push(new Instruction(""));
    },
  },
};
</script>

<style>
</style>