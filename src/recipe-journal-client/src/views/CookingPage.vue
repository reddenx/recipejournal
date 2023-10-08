<template>
    <div class="article-container">
        <div class="panel" v-if="recipe">
            <div class="section">
                <div class="section-title">
                {{ recipe.title }}
                </div>
                <div>
                    <strong>Author:</strong> {{ recipe.author }} <strong>Time:</strong> {{ recipe.durationMinutes }} <strong>Servings:</strong> {{ recipe.servings }}
                    <button type="button" class="slider-button"> ||| </button>
                </div>
                <p>{{ recipe.description }}</p>
            </div>

            <div class="section-dark">
                {{recipe.isPublic ? 'PUBLIC' : 'PRIVATE'}} | {{recipe.isDraft ? 'DRAFT' : 'PUBLISHED'}}
            </div>

            <div class="section">
                <h3>Ingredients</h3>
                <div class="ingredient" v-for="ingredient in allIngredients" :key="ingredient.id">
                    {{ ingredient.name }}: {{ ingredient.amount }}{{ getIngredientUnit(ingredient.unit, ingredient.amount) }}
                </div>
            </div>

            <template v-for="component in recipe.components">
                <div v-if="component.title" class="section-dark" :key="component.id">
                    <h3>{{component.title}}</h3>
                </div>
                <div v-if="component.description" class="section" :key="component.id">
                    <p>{{ component.description }}</p>
                </div>
                <div class="section" v-for="step in component.steps" :key="step.id">
                    <h4>{{ step.title }}</h4>
                    <div class="ingredient" v-for="ingredient in step.ingredients" :key="ingredient.id">
                        {{ ingredient.name }}: {{ ingredient.amount }}
                        {{ getIngredientUnit(ingredient.unit,ingredient.amount) }}
                    </div>
                    <p>
                        {{ step.body }}
                    </p>
                </div>
            </template>
        </div>
    </div>
</template>

<script>
import RecipeApi from "../scripts/recipeApi";
import Units from "../scripts/units";

const recipeApi = new RecipeApi();
export function compressIngredients(recipeDto) {

    let ingredientSum = {};
    recipeDto.components.forEach((c) =>
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

    return summs;
}

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
.article-container {
    max-width: 40rem;
    margin-left: auto;
    margin-right: auto;
    padding:.25em;
}

button.slider-button {
    margin: 0;
    padding: 0;
    border: none;
    background: white;

    border: 2px solid rgb(173, 173, 173);
    border-radius: 4px;
    width:20px;
    height: 20px;
    font-size: 8px;
}

.ingredient::before {
    content: "\00BB";
    font-weight: 800;
    margin-right: 8px;
}

.panel {
    border: 4px solid rgb(53, 19, 0);
    border-radius: 10px;
    padding: 15px 25px;
}
.panel p {
    text-align: justify;
    /* text-indent: 2rem; */
    margin: 0;
    margin-top: 10px;
    line-height: 1.25;
}
.section {
    margin-top:15px;
}
.section:nth-child(1) {
    margin-top:0;
}
.section-dark {
    margin-top:15px;
    background: rgb(53, 19, 0);
    color: white;
    box-sizing: border-box;
    border: none;
    margin-left: -25px;
    margin-right: -25px;
    padding: 10px 25px;
}
.panel h3 {
    margin:0;
}
.section-title {
    font-size: 32px;
    font-weight: 800;
    line-height: 1;
    margin-bottom: 15px;
}
/* .section ~ .section {
    padding-bottom: 10px;
} */
.section + .section {
    border-top: 2px solid #f0f0f0;
    padding-top: 15px;
}

.section.section-completed {
     text-decoration: line-through; 
}

p {
    white-space: pre-wrap;
}
</style>
