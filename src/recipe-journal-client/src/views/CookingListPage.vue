<template>
    <div class="page-container">
        <h1>Recipes</h1>
        <div class="recipe" v-for="recipe in recipes" :key="recipe.id">
            <router-link :to="'/cook/' + recipe.id"
                >{{ recipe.title }} - {{ recipe.durationMinutes }}m -
                {{ recipe.servings }}</router-link
            >
            -- <router-link v-if="isLoggedIn" :to="'/cms/' + recipe.id">edit</router-link>
            <button
                v-if="isLoggedIn && !shoppingRecipeIds.includes(recipe.id)"
                type="button"
                @click="shopRecipeButton(recipe)"
            >
                +SHOP
            </button>
        </div>
        <router-link v-if="isLoggedIn"  :to="'/cms/'">NEW</router-link>
    </div>
</template>

<script>
import RecipeApi from "../scripts/recipeApi";
import ShoppingApi from "../scripts/shoppingApi";
import UserApi from '../scripts/userApi';

const recipeApi = new RecipeApi();
const shoppingApi = new ShoppingApi();
const userApi = new UserApi(); //should probably come up with a user service thingy

export default {
    data: () => ({
        recipes: [],
        shoppingRecipeIds: [],
        isLoggedIn: false,
    }),
    async mounted() {
        this.recipes = await recipeApi.getRecipeList();
        let user = await userApi.getLoggedInUser();
        if(user) {
            let shoppingList = await shoppingApi.getShoppingList();
            this.shoppingRecipeIds = shoppingList.recipes.map((r) => r.id);
            this.isLoggedIn = true;
        } else {
            this.isLoggedIn = false;
        }
    },
    methods: {
        async shopRecipeButton(recipe) {
            let list = await shoppingApi.getShoppingList();
            let scales = list.recipes.map((r) => ({
                id: r.id, scale: r.scale
            }));
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