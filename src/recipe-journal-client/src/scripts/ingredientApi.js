


export default class IngredientApi {
    /**
     * gets an ingredient by id
     * @param {string} id ingredient to get
     */
    getIngredient(id) {}
    /**
     * gets a list of ingredients by id
     * @param {string[]} ids a list of ingredients to look up
     */
    getIngredients(ids) {}
    /**
     * gets all ingredient categories
     */
    getIngredientCategories() {}
    /**
     * update the categories a given ingredient is associated with
     * @param {string} ingredientId ingredient id to associate categories with
     * @param {string[]} categoryIds list of category ids to associate with the ingredient
     */
    updateIngredientCategories(ingredientId, categoryIds) {}
    /**
     * updates the ingredient category
     * @param {IngredientCategoryDto} categoryDto category to update
     */
    updateIngredientCategory(categoryDto) {}
}

class IngredientDto {}
class IngredientCategoryDto {}