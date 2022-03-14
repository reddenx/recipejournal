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
              Name:
              <input
                type="text"
                class="form-control mb-1"
                v-model="component.name"
              />
              <div class="row">
                <div class="col">
                  Ingredients:
                  <div class="card mb-3">
                    <div class="card-body">
                      <ul class="list-group">
                        <li
                          class="list-group-item"
                          v-for="(ingredient, index) in component.ingredients"
                          :key="index"
                        >
                          <div class="row g-1">
                            <div class="col">
                              <input
                                type="text"
                                class="form-control"
                                v-model="ingredient.name"
                                placeholder="name"
                              />
                            </div>
                            <div class="col-2">
                              <input
                                type="number"
                                class="form-control"
                                v-model="ingredient.amount"
                                placeholder="amount"
                              />
                            </div>
                            <div class="col-2">
                              <input
                                type="text"
                                class="form-control"
                                v-model="ingredient.unit"
                                placeholder="unit"
                              />
                            </div>
                            <div class="col-auto">
                              <button
                                type="button"
                                class="btn btn-danger"
                                @click="deleteIngredient(component, ingredient)"
                              >
                                <span class="fa-solid fa-play"> </span>
                                -
                              </button>
                            </div>
                          </div>
                        </li>
                      </ul>
                      <div class="d-grid">
                        <button
                          type="button"
                          class="btn btn-outline-secondary"
                          @click="addIngredient(component)"
                        >
                          +
                        </button>
                      </div>
                    </div>
                  </div>

                  Instructions:
                  <div
                    v-for="(instruction, index) in component.instructions"
                    :key="index"
                  >
                    <div class="row mb-1">
                      <div class="col">
                        <textarea
                          class="form-control"
                          v-model="instruction.description"
                        ></textarea>
                      </div>
                      <div class="col-auto">
                        <button
                          type="button"
                          class="btn btn-danger"
                          @click="deleteInstruction(component, instruction)"
                        >
                          -
                        </button>
                      </div>
                    </div>
                  </div>
                  <div class="d-grid">
                    <button
                      type="button"
                      class="btn btn-outline-secondary"
                      @click="addInstruction(component)"
                    >
                      +Instruction
                    </button>
                  </div>
                </div>
                <div class="col-auto">
                  <button
                    class="btn btn-danger"
                    @click="deleteComponent(component)"
                  >
                    -
                  </button>
                </div>
              </div>
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
    /**
     * @param {RecipeComponentDto} component
     */
    deleteComponent(component) {
      this.recipe.components = this.recipe.components.filter(
        (c) => c !== component
      );
    },
    /**
     * @param {RecipeComponentDto} component
     */
    addIngredient(component) {
      component.ingredients.push(new Ingredient("", null, ""));
    },
    /**
     * @param {RecipeComponentDto} component
     * @param {Ingredient} ingredient
     */
    deleteIngredient(component, ingredient) {
      component.ingredients = component.ingredients.filter(
        (i) => i !== ingredient
      );
    },
    /** @param {RecipeComponentDto} component */
    addInstruction(component) {
      component.instructions.push(new Instruction(""));
    },
    /**
     * @param {RecipeComponentDto} component
     * @param {Instruction} instruction
     */
    deleteInstruction(component, instruction) {
      component.instructions = component.instructions.filter(
        (i) => i !== instruction
      );
    },
  },
};
</script>

<style>
</style>