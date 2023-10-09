<template>
    <div class="layout-container">
        <div class="layout-body">
            <h1>Recipes</h1>
            <router-link v-if="isLoggedIn" :to="'/cms/'">NEW</router-link>

            <div class="recipe-list-container">
                <div
                    class="recipe-list-item grid-r"
                    v-for="recipe in recipes"
                    :key="recipe.id"
                >
                    <div class="recipe-list-item-top grid-r">
                        <div class="title">
                            <router-link :to="'/cook/' + recipe.id">{{
                                recipe.title
                            }}</router-link>
                        </div>
                        <div class="duration" v-show="recipe.duration">
                            {{ recipe.durationMinutes }} minutes
                        </div>
                        <div class="servings" v-show="recipe.servings">
                            {{ recipe.servings }} servings
                        </div>
                        <!-- <div class="tag">baking</div> -->
                    </div>
                    <div class="recipe-list-item-bottom grid-r">
                        <div class="rating-container">x x x x o (15)</div>
                        <div class="success-bar-container">
                            <div class="success-bar">|====--|</div>
                        </div>
                        <div class="cooked-counter"></div>
                    </div>
                    <div class="recipe-list-item-right">
                        <div class="author-icon">Sean</div>
                        <button type="button">Edit</button>
                        <button type="button">Journal</button>
                        <div class="shopping-widget-container">
                            <button type="button">-</button>
                            <input type="number" />
                            <button type="button">+</button>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
</template>


<script>
import RecipeApi from "../scripts/recipeApi";
import ShoppingApi from "../scripts/shoppingApi";
import UserApi from "../scripts/userApi";

const recipeApi = new RecipeApi();
const shoppingApi = new ShoppingApi();
const userApi = new UserApi(); //should probably come up with a user service thingy

class RecipeListItemViewmodel {
    constructor(
        id,
        title,
        tags,
        author,
        rating,
        allAttemptCount,
        version,
        dateLastModified,
        dateCreated,
        servings,
        duration,
        mySuccess,
        dateLastAttempted,
        myAttemptCount,
        goals,
        myNextCount,
        isPublic,
        isDraft,
        amountShoppingFor
    ) {
        //anonymous fields
        this.id = id;
        this.title = title;
        this.tags = tags?.length ? tags : [];
        this.author = author;
        this.rating = rating;
        this.allAttemptCount = allAttemptCount;
        this.version = version;
        this.dateLastModified = dateLastModified;
        this.dateCreated = dateCreated;
        this.servings = servings;
        this.duration = duration;

        //loggedin fields
        this.mySuccess = mySuccess;
        this.dateLastAttempted = dateLastAttempted;
        this.myAttemptCount = myAttemptCount;
        this.goals = goals;
        this.myNextCount = myNextCount;
        this.isPublic = isPublic;
        this.isDraft = isDraft;
        this.amountShoppingFor = amountShoppingFor;
        this.showShoppingWidget = false;
    }
}

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
.recipe-list-container {
    display: flex;
    flex-direction: column;
    gap: 3px;
}
.recipe-list-item {
    display: grid;
    grid-template-columns: 1fr auto;
    border: 3px solid black;
    border-radius: 10px;
    padding: 3px;
}
.recipe-list-item-top {
    grid-column-start: 1;
    grid-row-start: 1;
}
.recipe-list-item-bottom {
    grid-column-start: 1;
    grid-row-start: 2;
    display: flex;
    flex-direction: row;
}
.recipe-list-item-right {
    grid-column-start: 2;
    grid-row-start: 1;
    grid-row-end: 3;
    display: flex;
    flex-direction: row;
}


.shopping-widget-container {
    max-width: 100px;
}
.shopping-widget-container input {
    max-width: 2em;
}

.author-icon {

}

/* 
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
} */

.recipe-list-container div {
    /* border: 1px solid black; */
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