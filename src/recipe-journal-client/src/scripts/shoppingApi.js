import axios from 'axios';

export default class ShoppingApi {
    async getShoppingList() {
        try {
            let result = await axios.get('api/v1/shopping');
            if (result.data) {
                return new ShoppingListDto(result.data.recipes.map(r => new ShoppingRecipe(r.id, r.title, r.scale, r.ingredients.map(i => new ShoppingIngredient(i.id, i.name, i.unit, i.amount)))), result.data.gatheredIds, result.data.nonrecipeIngredients.map(nr => new NonrecipeShoppingIngredient(nr.id, nr.name, nr.amount)));
            }
        } catch {
            return null;
        }
    }
    async updateShoppingList(recipeScales, gatheredIds, nonrecipeIngredients) {
        try {
            let result = await axios.put('api/v1/shopping', {
                recipeScales,
                gatheredIds,
                nonrecipeIngredients
            });
            return result.status == 204;
        } catch {
            return false;
        }
    }
}


//lists recipes in list, ingredients and their shopping status
export class ShoppingListDto {
    constructor(recipes, gatheredIds, nonrecipeIngredients) {
        this.recipes = recipes;
        this.gatheredIds = gatheredIds;
        this.nonrecipeIngredients = nonrecipeIngredients;
    }
}
export class ShoppingRecipe {
    constructor(id, title, scale, ingredients) {
        this.id = id;
        this.title = title;
        this.scale = scale;
        this.ingredients = ingredients;
    }
}
export class ShoppingIngredient {
    constructor(id, name, unit, amount) {
        this.id = id;
        this.name = name;
        this.unit = unit;
        this.amount = amount;
    }
}
export class NonrecipeShoppingIngredient {
    constructor(id, name, amount) {
        this.id = id;
        this.name = name;
        this.amount = amount
    }
}