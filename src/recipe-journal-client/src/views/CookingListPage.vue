<template>
    <div class="page-container">
        <h1>Recipes</h1>
        <div class="recipe" v-for="recipe in recipes" :key="recipe.id">
            <router-link :to="'/cook/' + recipe.id"
                >{{ recipe.title }} - {{ recipe.durationMinutes }}m -
                {{ recipe.servings }}</router-link
            >
            -- <router-link :to="'/cms/' + recipe.id">edit</router-link>
            <button
                v-if="!shoppingRecipeIds.includes(recipe.id)"
                type="button"
                @click="shopRecipeButton(recipe)"
            >
                +SHOP
            </button>
        </div>
        <router-link :to="'/cms/'">NEW</router-link>
    </div>
</template>

<script>
import RecipeApi from "../scripts/recipeApi";
import ShoppingApi from "../scripts/shoppingApi";

const recipeApi = new RecipeApi();
const shoppingApi = new ShoppingApi();

export default {
    data: () => ({
        recipes: [],
        shoppingRecipeIds: [],
    }),
    async mounted() {
        this.recipes = await recipeApi.getRecipeList();
        let shoppingList = await shoppingApi.getShoppingList();
        this.shoppingRecipeIds = shoppingList.recipes.map((r) => r.id);
    },
    methods: {
        async shopRecipeButton(recipe) {
            let list = await shoppingApi.getShoppingList();
            let scales = list.recipes.map((r) => {
                r.id, r.scale;
            });
            await shoppingApi.updateShoppingList([
                { id: recipe.id, scale: 1 },
                ...scales,
            ], list.gatheredIds);
            this.shoppingRecipeIds = list.recipes.map(r => r.id);
            this.shoppingRecipeIds.push(recipe.id);
        },
    },
};
</script>

<style>
</style>