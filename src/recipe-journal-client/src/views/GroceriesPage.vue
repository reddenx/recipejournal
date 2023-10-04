<template>
    <div>
        <h1>Shopping List</h1>

        <div v-for="recipe in recipes" :key="recipe.id">
            {{ recipe.title }} x{{ recipe.scale }}
        </div>

        <hr />

        <div @click="clickedIngredient(ingredient)" v-for="ingredient in ingredients.filter(i => !i.gathered)" :key="ingredient.name">
            {{ ingredient.gathered ? 'Y' : 'N' }}
            {{ ingredient.name
            }}<span v-for="amount in ingredient.amounts" :key="amount.unit"
                >, {{ amount.amount
                }}{{ formatUnit(amount.unit, amount.amount) }}</span
            >
        </div>
        <hr />
        <div @click="clickedIngredient(ingredient)" v-for="ingredient in ingredients.filter(i => i.gathered)" :key="ingredient.name">
            {{ ingredient.gathered ? 'Y' : 'N' }}
            {{ ingredient.name
            }}<span v-for="amount in ingredient.amounts" :key="amount.unit"
                >, {{ amount.amount
                }}{{ formatUnit(amount.unit, amount.amount) }}</span
            >
        </div>
    </div>
</template>

<script>
import ShoppingApi from "../scripts/shoppingApi.js";
import Units from "../scripts/units.js";

const shoppingApi = new ShoppingApi();

export default {
    data: () => ({
        recipes: [],
        ingredients: [],
        gathered: [],
    }),
    async mounted() {
        let shoppingList = await shoppingApi.getShoppingList();
        this.recipes = shoppingList.recipes;
        this.gathered = shoppingList.gathered;
        this.condenseIngredients();
    },
    methods: {
        condenseIngredients() {
            //transform recipe ingredient lists into flat ingredient list with amounts broken down by unit and scale accounted for
            let ingredients = [];
            this.recipes.forEach((r) => ingredients.push(...r.ingredients.map(i => ({ ingredient: i, scale: r.scale }))));

            let hash = {};
            ingredients.forEach((i) => {
                if (!hash[i.ingredient.name])
                    hash[i.ingredient.name] = new ShoppingIngredient(
                        i.ingredient.name,
                        this.gathered.includes(i.ingredient.name)
                    );

                hash[i.ingredient.name].addIngredient(i.ingredient.unit, i.ingredient.amount * i.scale);
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
        }
    },
};

class ShoppingIngredient {
    constructor(name, gathered) {
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

<style>
</style>