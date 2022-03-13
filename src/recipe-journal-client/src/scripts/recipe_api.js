export default class RecipeApi {

    /**
     * @returns {RecipeListItemDto[]}
     */
    async getRecipeList() { }

    /**
     * 
     * @param {String} recipeId 
     * @returns {RecipeDto}
     */
    async getRecipe(recipeId) { }

    /**
     * 
     * @param {RecipeDto} recipeDto 
     * @returns {RecipeDto}
     */
    async createRecipe(recipeDto) { }

    /**
     * 
     * @param {RecipeDto} recipeDto 
     */
    async updateRecipe(recipeDto) { }

    /**
     * 
     * @param {String} recipeId 
     */
    async deleteRecipe(recipeId) { }

    /**
     * 
     * @param {String} recipeId 
     * @param {JournalEntryDto} journalEntryDto 
     * @returns {JournalEntryDto}
     */
    async addJournalEntry(recipeId, journalEntryDto) { }

    /**
     * 
     * @param {JournalEntryDto} journalEntry
     */
    async updateJournalEntry(journalEntry) { }

    /**
     * 
     * @param {String} journalEntryId 
     */
    async deleteJournalEntry(journalEntryId) { }
}

export class RecipeListItemDto {
    constructor(id, title) {
        this.id = id;
        this.title = title;
    }
}
export class RecipeDto {
    constructor(id, title, description, components) {
        this.id = id;
        this.title = title;
        this.description = description;
        this.components = components;
    }
}
export class RecipeDescriptionDto {
    constructor(name, order, ingredients, instructions) {
        this.name = name;
        this.order = order;
        this.ingredients = ingredients;
        this.instructions = instructions;
    }
}
export class Ingredient {
    constructor(name, amount, unit) {
        this.name = name;
        this.amount = amount;
        this.unit = unit;
    }
}
export class Instruction {
    constructor(description) {
        this.description = description;
    }
}
export class JournalEntryDto {
    constructor(id, author, date, description, recipeUsed) {
        this.id = id;
        this.author = author;
        this.date = date;
        this.description = description;
        this.recipeUsed = recipeUsed;
    }
}