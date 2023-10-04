


export default class ShoppingApi {
    async getShoppingList() {
        return _shoppingList;
    }
    updateShoppingList(shoppingListDto) {}
}

//lists recipes in list, ingredients and their shopping status
class ShoppingListDto {
    constructor(recipes, gathered) {
        this.recipes = recipes;
        this.gathered = gathered;
    }
}
class ShoppingRecipe {
    constructor(id, title, scale, ingredients) {
        this.id = id;
        this.title = title;
        this.scale = scale;
        this.ingredients = ingredients;
    }
}
class ShoppingIngredients {
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
            unit: 'gram',
            amount: 100,
            name: 'almond flour'
        },{
            unit: 'tablespoon',
            amount: 3,
            name: 'butter',
        }],
    },{
        id: 'guid2',
        title: 'dutch babies',
        scale: 2,
        ingredients: [{
            unit: 'cup',
            amount: '2',
            name: 'almond flour'
        }, {
            unit: 'tablespoon',
            amount: 4,
            name: 'butter',
        }, {
            unit: 'count',
            amount: 2,
            name: 'apple'
        }]
    }],
    gathered: ['cereal', 'apple']
}; 