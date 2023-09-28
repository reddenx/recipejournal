<template>
    <div class="recipe-container" v-if="recipe">
        <div class="recipe-info-container">
            <div class="recipe-title-container">
                {{ recipe.title }}
            </div>

            <div class="recipe-description-container">
                <p>{{ recipe.description }}</p>
            </div>
        </div>
        <div class="recipe-ingredients-container">
            <div
                class="recipe-ingredient"
                v-for="(ingredient, i) in allIngredients"
                :key="i"
            >
                {{ ingredient.name }}: {{ ingredient.amount
                }}{{ getIngredientUnit(ingredient.unit, ingredient.amount) }}
            </div>
        </div>
        <div
            class="recipe-component-container"
            v-for="(component, i) in recipe.components"
            :key="i"
        >
            <div class="recipe-component-info">
                {{ component.title }}
                <p>{{ component.description }}</p>
            </div>
            <div
                class="recipe-step-container"
                v-for="(step, i) in component.steps"
                :key="i"
            >
                <div class="recipe-step-info-container">
                    <div class="recipe-step-title">
                        {{ step.title }}
                    </div>
                </div>

                <div class="recipe-step-ingredients-container">
                    <div
                        class="recipe-step-ingredient"
                        v-for="(ingredient, i) in step.ingredients"
                        :key="i"
                    >
                        {{ ingredient.name }}: {{ ingredient.amount
                        }}{{
                            getIngredientUnit(
                                ingredient.unit,
                                ingredient.amount
                            )
                        }}
                    </div>
                </div>
                {{ step.body }}
                <div style="clear: both"></div>
            </div>
        </div>
    </div>
</template>

<script>
import RecipeApi from "../scripts/recipeApi";
import Units from "../scripts/units";

const recipeApi = new RecipeApi();

export default {
    props: {
        recipeId: String,
    },
    data: () => ({
        recipe: null,
        allIngredients: [],
    }),
    async mounted() {
        let recipe = await recipeApi.getRecipeDetails(this.recipeId);
        if (!recipe) {
            return;
        }

        this.recipe = recipe;
        let ingredientSum = {};
        this.recipe.components.forEach((c) =>
            c.steps.forEach((s) =>
                s.ingredients?.forEach((i) => {
                    if (!ingredientSum[i.name + i.unit]) {
                        ingredientSum[i.name + i.unit] = [];
                    }
                    ingredientSum[i.name + i.unit].push(i);
                })
            )
        );

        let summs = Object.values(ingredientSum).map(i => ({
            name: i[0].name + (i.length > 1 ? '*' : ''),
            unit: i[0].unit,
            amount: i.reduce((sum, item) => sum + item.amount, 0)
        }))

        this.allIngredients.push(...summs);
        
    },
    methods: {
        getIngredientUnit(unit, amount) {
            return Units.getUnitForQty(unit, amount);
        },
    },
};
</script>

<style>
.recipe-ingredients-container {
    border: 1px solid black;
}

.recipe-step-ingredients-container {
    border: 1px solid black;
    float: left;
}
.recipe-step-container {
}

.recipe-step-container:nth-child(2n) {
    background: rgb(231, 231, 231);
}
.recipe-step-container:nth-child(2n + 1) {
    background: rgb(255, 254, 197);
}
</style>