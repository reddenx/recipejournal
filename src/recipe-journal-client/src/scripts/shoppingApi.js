export default class ShoppingApi {
    async getShoppingList() {
        return _shoppingList;
    }
    updateShoppingList(recipeIds, gatheredIngredientIds) {
        console.log(recipeIds, gatheredIngredientIds);
    }
}

//lists recipes in list, ingredients and their shopping status
export class ShoppingListDto {
    constructor(recipes, gathered) {
        this.recipes = recipes;
        this.gathered = gathered;
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
export class ShoppingIngredients {
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
    gathered: ['cereal', 'apple']
}; 