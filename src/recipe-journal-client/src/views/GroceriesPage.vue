<template>
    <div class="layout-container">
        <div class="layout-body">
            <h1>Shopping List</h1>
            Recipes:
            <div v-for="recipe in recipes" :key="recipe.id">
                <ShoppingWidget v-model="recipe.scale" @input="handleScaleChanged(recipe)" />
                {{ recipe.title }} 
            </div>

            <hr />
            <h2>Groceries</h2>
            <div
                v-for="ingredient in ingredients.filter((i) => !i.gathered)"
                :key="ingredient.name"
            >
                <div
                    class="ingredient-line"
                    @click="clickedIngredient(ingredient)"
                >
                    <span class="fa-regular fa-square"></span>
                    {{ ingredient.name
                    }}<span
                        v-for="amount in ingredient.amounts"
                        :key="amount.unit"
                        >, {{ amount.amount
                        }}{{ formatUnit(amount.unit, amount.amount) }}</span
                    >
                </div>
            </div>
            <hr />
            <div
                v-for="ingredient in ingredients.filter((i) => i.gathered)"
                :key="ingredient.name"
            >
                <div
                    class="ingredient-line"
                    @click="clickedIngredient(ingredient)"
                >
                    <span class="fa-regular fa-square-check"></span>
                    {{ ingredient.name
                    }}<span
                        v-for="amount in ingredient.amounts"
                        :key="amount.unit"
                        >, {{ amount.amount
                        }}{{ formatUnit(amount.unit, amount.amount) }}</span
                    >
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import ShoppingWidget from '../components/ShoppingWidget.vue';
import ShoppingApi, {
    ShoppingListDto,
    UpdateShoppingListDto,
} from "../scripts/shoppingApi.js";
import Units from "../scripts/units.js";

const shoppingApi = new ShoppingApi();

export default {
    components: { ShoppingWidget },
    data: () => ({
        recipes: [],
        ingredients: [],
        gatheredIds: [],
    }),
    async mounted() {
        let shoppingList = await shoppingApi.getShoppingList();
        this.recipes = shoppingList.recipes;
        this.gatheredIds = shoppingList.gatheredIds;
        this.condenseIngredients();
    },
    methods: {
        condenseIngredients() {
            //transform recipe ingredient lists into flat ingredient list with amounts broken down by unit and scale accounted for
            let ingredients = [];
            this.recipes.forEach((r) =>
                ingredients.push(
                    ...r.ingredients.map((i) => ({
                        ingredient: i,
                        scale: r.scale,
                    }))
                )
            );

            let hash = {};
            ingredients.forEach((i) => {
                if (!hash[i.ingredient.name])
                    hash[i.ingredient.name] = new ShoppingIngredient(
                        i.ingredient.id,
                        i.ingredient.name,
                        this.gatheredIds.includes(i.ingredient.id)
                    );

                hash[i.ingredient.name].addIngredient(
                    i.ingredient.unit,
                    i.ingredient.amount * i.scale
                );
            });
            this.ingredients = [];
            this.ingredients.push(...Object.values(hash));
            this.ingredients.sort((a, b) => a.name.localeCompare(b.name));
        },
        formatUnit(unit, amount) {
            return Units.getUnitForQty(unit, amount);
        },
        clickedIngredient(ingredient) {
            ingredient.gathered = !ingredient.gathered;

            this.save();
        },
        removeRecipe(recipe) {
            let index = this.recipes.indexOf(recipe);
            this.recipes.splice(index, 1);
            this.condenseIngredients();

            this.save();
        },
        async save() {
            await shoppingApi.updateShoppingList(
                this.recipes.map((r) => ({ id: r.id, scale: r.scale })),
                this.ingredients.filter((i) => i.gathered).map((i) => i.id)
            );
        },
        async handleScaleChanged(recipe) {
            this.condenseIngredients();
            if(recipe.scale == 0) {
                this.removeRecipe(recipe);
            } else {
                this.save();
            }
            
        },
    },
};

class ShoppingIngredient {
    constructor(id, name, gathered) {
        this.id = id;
        this.name = name;
        this.gathered = gathered;
        this.amounts = [];
    }

    addIngredient(unit, amount) {
        let found = this.amounts.find((a) => a.unit == unit);
        if (!found) {
            this.amounts.push(new ShoppingIngredientAmount(unit, amount));
        } else {
            found.amount += amount;
        }
    }
}
class ShoppingIngredientAmount {
    constructor(unit, amount) {
        this.unit = unit;
        this.amount = amount;
    }
}
</script>

<style scoped>
.ingredient-line {
    cursor: pointer;
    display: inline-block;
    width: auto;
    padding: 0.25em;
}
</style>