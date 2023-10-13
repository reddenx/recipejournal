<template>
    <div class="layout-container">
        <div class="layout-body">
            <h1>Recipes</h1>
            <router-link v-if="isLoggedIn" :to="'/cms/'">NEW</router-link>

            <div class="recipe-list-container">
                <div class="recipe-list-item grid-r" v-for="recipe in recipes" :key="recipe.id" :class="{'private': !recipe.isPublic, 'draft': recipe.isDraft}">
                    <div class="recipe-list-item-top grid-r">
                        <div class="title">
                            <router-link :to="'/cook/' + recipe.id">{{recipe.title}}</router-link>
                        </div>
                    </div>
                    <div class="recipe-list-item-bottom grid-r">
                        <div class="rating-container" v-if="recipe.ratingCount > 0 && typeof recipe.rating == 'number'">
                            <span class="fa-star" :class="{'fa-solid': recipe.starCount >= 1,'fa-regular': recipe.starCount < 1,}"></span>
                            <span class="fa-star" :class="{'fa-solid': recipe.starCount >= 2,'fa-regular': recipe.starCount < 2, }"></span>
                            <span class="fa-star" :class="{'fa-solid': recipe.starCount >= 3,'fa-regular': recipe.starCount < 3,}"></span>
                            <span class="fa-star" :class="{'fa-solid': recipe.starCount >= 4,'fa-regular': recipe.starCount < 4,}"></span>
                            <span class="fa-star" :class="{ 'fa-solid': recipe.starCount == 5, 'fa-regular': recipe.starCount < 5, }"></span>
                            ({{ recipe.ratingCount }})
                        </div>
                        <div class="success-bar-container" v-if="recipe.mySuccess">
                            <div class="success-bar" :style="{ width: recipe.mySuccess * 100 + '%' }"></div>
                        </div>
                        <router-link v-if="isLoggedIn" class="last-cooked-date" :to="'/journal/' + recipe.id">
                            <span class="fa-solid fa-book"></span>
                            <span v-if="recipe.myAttemptCount">({{ recipe.myAttemptCount }})
                                <!-- {{recipe.dateLastAttempted.toLocaleDateString()}} -->
                            </span>
                        </router-link>
                        <div class="duration" v-show="recipe.duration">
                            <span class="fa-solid fa-clock"></span> {{ recipe.duration }}
                        </div>
                        <div class="servings" v-show="recipe.servings">
                            <span class="fa-solid fa-utensils"></span> {{ recipe.servings }}
                        </div>
                        <div v-if="recipe.goalCount">
                            <span class="fa-regular fa-lightbulb"></span> {{ recipe.goalCount }}
                        </div>
                    </div>
                    <div class="recipe-list-item-right">
                        <button v-if="isLoggedIn" type="button" @click="$router.push('/cms/' + recipe.id)">
                            <span class="fa-solid fa-pen-to-square"></span>
                        </button>
                        <ShoppingWidget v-model="recipe.amountShoppingFor" @input="shopRecipeAmountChanged(recipe, $event)" />
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
import ShoppingWidget from "../components/ShoppingWidget.vue";

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
        ratingCount,
        allAttemptCount,
        version,
        dateLastModified,
        dateCreated,
        servings,
        duration,
        mySuccess,
        dateLastAttempted,
        myAttemptCount,
        goalCount,
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
        this.starCount = ratingCount > 0 ? Math.round(1 + rating * 4) : 0;
        this.ratingCount = ratingCount;
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
        this.goalCount = goalCount;
        this.myNextCount = myNextCount;
        this.isPublic = isPublic;
        this.isDraft = isDraft;
        this.amountShoppingFor = amountShoppingFor;
    }
}

export default {
    components: { ShoppingWidget },
    data: () => ({
        recipes: [],
        recipeDtos: [],
        shoppingRecipeIds: [],
        isLoggedIn: false,
    }),
    async mounted() {
        let recipeDtos = await recipeApi.getRecipeList();

        let sortedDtos = [
            ...recipeDtos.filter(dto => dto.isPublic && !dto.isDraft), 
            ...recipeDtos.filter(dto => !dto.isPublic && !dto.isDraft), 
            ...recipeDtos.filter(dto => dto.isDraft)
        ];
        
        this.recipeDtos = sortedDtos;
        let user = await userApi.getLoggedInUser();

        if (user) {
            let shoppingList = await shoppingApi.getShoppingList();

            this.recipes = sortedDtos.map(
                (r) =>
                    new RecipeListItemViewmodel(
                        r.id,
                        r.title,
                        r.tags,
                        r.author,
                        r.rating,
                        null,
                        null,
                        r.version,
                        r.lastModified,
                        r.dateCreated,
                        r.servings,
                        r.durationMinutes,
                        r.personalBest,
                        r.personalLastJournalDate,
                        r.personalJournalCount,
                        r.personalGoalCount,
                        r.personalNoteCount,
                        r.isPublic,
                        r.isDraft,
                        shoppingList.recipes.find((l) => l.id == r.id)?.scale ??
                            0
                    )
            );

            this.shoppingRecipeIds = shoppingList.recipes.map((r) => r.id);
            this.isLoggedIn = true;
        } else {
            this.recipes = this.recipeDtos.map(
                (r) =>
                    new RecipeListItemViewmodel(
                        r.id,
                        r.title,
                        [],
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        r.servings,
                        r.durationMinutes,
                        null,
                        null,
                        null,
                        null,
                        null,
                        true,
                        false,
                        null
                    )
            );

            this.isLoggedIn = false;
        }
    },
    methods: {
        async shopRecipeAmountChanged(recipe) {
            let list = await shoppingApi.getShoppingList();

            let i = list.recipes.findIndex((r) => r.id == recipe.id);
            if (i >= 0) list.recipes.splice(i, 1);

            let scales = list.recipes.map((r) => ({
                id: r.id,
                scale: r.scale,
            }));
            await shoppingApi.updateShoppingList(
                [{ id: recipe.id, scale: recipe.amountShoppingFor }, ...scales],
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
    display: flex;
    flex-direction: row;
    gap: 1em;
}
.recipe-list-item-bottom {
    grid-column-start: 1;
    grid-row-start: 2;
    display: flex;
    flex-direction: row;
    gap: 1em;
}
.recipe-list-item-right {
    grid-column-start: 2;
    grid-row-start: 1;
    grid-row-end: 3;
    display: flex;
    align-items: center;
    flex-direction: row;
}

.author-icon {
    padding: 1em;
}

.draft {
    background-color: #eee;
}
.private {
    background-color: #bbb;
}

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
.success-bar-container {
    width: 5em;
    background-color: rgb(184, 184, 255);
    border-radius: 2.5em;
    height: 70%;
}
.success-bar {
    background-color: rgb(131, 251, 255);
    height: 100%;
    border-top-left-radius: 2.5em;
    border-bottom-left-radius: 2.5em;
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