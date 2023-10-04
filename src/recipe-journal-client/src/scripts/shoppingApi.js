import axios from 'axios';

export default class ShoppingApi {
    async getShoppingList() {
        try {
            let result = await axios.get('api/v1/shopping');
            if (result.data) {
                return new ShoppingListDto(result.data.recipes.map(r => new ShoppingRecipe(r.id, r.title, r.scale, r.ingredients.map(i => new ShoppingIngredient(i.id, i.name, i.unit, i.amount)))), result.data.gatheredIds);
            }
        } catch {
            return null;
        }
    }
    async updateShoppingList(recipeScales, gatheredIds) {
        try {
            let result = await axios.put('api/v1/shopping', {
                recipeScales,
                gatheredIds
            });
            return result.status == 204;
        } catch {
            return false;
        }
    }
}


//lists recipes in list, ingredients and their shopping status
export class ShoppingListDto {
    constructor(recipes, gatheredIds) {
        this.recipes = recipes;
        this.gatheredIds = gatheredIds;
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

var _shoppingList = {
    recipes: [{
        id: 'guid',
        title: 'macarons',
        scale: 1,
        ingredients: [{
            id: 'sldkjf',
            unit: 'gram',
            amount: 100,
            name: 'almond flour'
        }, {
            id: 'asdfasv',
            unit: 'tablespoon',
            amount: 3,
            name: 'butter',
        }],
    }, {
        id: 'guid2',
        title: 'dutch babies',
        scale: 2,
        ingredients: [{
            id: 'jfiven',
            unit: 'cup',
            amount: '2',
            name: 'almond flour'
        }, {
            id: 'asviebvbv',
            unit: 'tablespoon',
            amount: 4,
            name: 'butter',
        }, {
            id: 'f98a7whv',
            unit: 'count',
            amount: 2,
            name: 'apple'
        }]
    }],
    gatheredIds: ['f98a7whv', 'apple']
}; 