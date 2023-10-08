<template>
    <div class="article-container">
        <h1>Recipes</h1>
        <div class="recipe-list-container">
            <div class="grid-r" v-for="recipe in recipes" :key="recipe.id">
                <div class="grid-c grid-fill">
                    <div class="grid-r">
                        <div class="title">
                            <router-link :to="'/cook/' + recipe.id">{{ recipe.title }}</router-link>
                        </div>
                        <div class="duration" v-show="recipe.duration">
                            {{ recipe.durationMinutes }} minutes
                        </div>
                        <div class="servings" v-show="recipe.servings">
                            {{ recipe.servings }} servings
                        </div>
                        <!-- <div class="tag">baking</div> -->
                    </div>
                    <div class="grid-r">
                        <div class="rating-container">x x x x o (15)</div>
                        <div class="success-bar-container">
                            <div class="success-bar">|====--|</div>
                        </div>
                        <div class="cooked-counter"></div>
                    </div>
                </div>
                <div class="author-icon">Sean</div>
                <button type="button">Edit</button>
                <button type="button">Shop</button>
            </div>
        </div>
        <div class="recipe" v-for="recipe in recipes" :key="recipe.id">
            --
            <router-link v-if="isLoggedIn" :to="'/cms/' + recipe.id"
                >edit</router-link
            >
            <button
                v-if="isLoggedIn && !shoppingRecipeIds.includes(recipe.id)"
                type="button"
                @click="shopRecipeButton(recipe)"
            >
                +SHOP
            </button>
            <router-link v-if="isLoggedIn" :to="'/journal/' + recipe.id">
                Journal
            </router-link>
        </div>
        <router-link v-if="isLoggedIn" :to="'/cms/'">NEW</router-link>
    </div>
</template>

<script>
import RecipeApi from "../scripts/recipeApi";
import ShoppingApi from "../scripts/shoppingApi";
import UserApi from "../scripts/userApi";

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
        if (user) {
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
                id: r.id,
                scale: r.scale,
            }));
            await shoppingApi.updateShoppingList(
                [{ id: recipe.id, scale: 1 }, ...scales],
                list.gatheredIds
            );
            this.shoppingRecipeIds = list.recipes.map((r) => r.id);
            this.shoppingRecipeIds.push(recipe.id);
        },
    },
};
</script>

<style scoped>
.grid-r {
    display: flex;
    flex-direction: row;
    flex-wrap: nowrap;
    justify-content: flex-start;
}
.grid-c {
    display: flex;
    flex-direction: column;
    flex-wrap: nowrap;
    justify-content: flex-start;
}
.grid-fill {
    flex-grow: 1;
}

.recipe-list-container div {
    border: 1px solid black;
}
.recipe-list-container {
}
.recipe-list-item {
}
.title {
}
.tag {
}
.rating-container {
}
.label {
}
.success-bar {
}
.author-icon {
}
.shop-button {
}
.journal-button {
}
.edit-button {
}
</style>