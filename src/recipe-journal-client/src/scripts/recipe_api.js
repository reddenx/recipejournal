export default class RecipeApi {

    /**
     * @returns {RecipeListItemDto[]}
     */
    async getRecipeList() {
        return new Promise((yay, nay) => {
            yay([
                new RecipeListItemDto('test1', 'French Maracons'),
                new RecipeListItemDto('test2', 'Italian Maracons'),
            ]);
        });
    }

    /**
     * 
     * @param {String} recipeId 
     * @returns {RecipeDto}
     */
    async getRecipe(recipeId) {
        return new Promise((yay, nay) => {
            yay(new RecipeDto('test2', 'Italian Macarons', 'italian macarons, more stable, first make the paste, then fold in meringue', [
                new RecipeComponentDto('paste', 1, [
                    new Ingredient('egg white', '100', 'g'),
                    new Ingredient('powdered sugar', '250', 'g'),
                    new Ingredient('almond flour', '260', 'g'),
                ], [new Instruction('lightly mix together')]),
                new RecipeComponentDto('meringue', 2, [
                    new Ingredient('egg white', 100, 'g'),
                    new Ingredient('water', 60, 'g'),
                    new Ingredient('sugar (syrup)', 230, 'g'),
                    new Ingredient('suger (stabilizer)', 30, 'g'),
                ], [new Instruction('syrup to 188C')]),
                new RecipeComponentDto('', 3, [], new Instruction('preheat to 300F, macronage meringue and paste together, pipe and bake for 10-17 minutes')),
            ]));
        });
    }

    /**
     * 
     * @param {RecipeDto} recipeDto 
     * @returns {RecipeDto}
     */
    async createRecipe(recipeDto) {
        return this.getRecipe();
    }

    /**
     * 
     * @param {RecipeDto} recipeDto 
     */
    async updateRecipe(recipeDto) {
        return new Promise(yay => { yay() });
    }

    /**
     * 
     * @param {String} recipeId 
     */
    async deleteRecipe(recipeId) {
        return new Promise(yay => { yay() });
    }

    /**
     * 
     * @param {String} recipeId 
     * @param {JournalEntryDto} journalEntryDto 
     * @returns {JournalEntryDto}
     */
    async addJournalEntry(recipeId, journalEntryDto) {
        console.error('not implemented');
    }

    /**
     * 
     * @param {String} recipeId 
     * @returns {JournalEntryDto[]}
     */
    async getEntriesForRecipe(recipeId) {
        console.error('not implemented');
    }

    /**
     * 
     * @param {JournalEntryDto} journalEntry
     */
    async updateJournalEntry(journalEntry) { console.error('not implemented'); }

    /**
     * 
     * @param {String} journalEntryId 
     */
    async deleteJournalEntry(journalEntryId) { console.error('not implemented'); }

    /**
     * @param {Number} count
     * @returns {JournalEntryDto[]}
     */
    async getRecentJournalEntries(count) { console.error('not implemented'); }
}

export class RecipeListItemDto {
    /**
     * 
     * @param {String} id 
     * @param {String} title 
     */
    constructor(id, title) {
        this.id = id;
        this.title = title;
    }
}
export class RecipeDto {
    /**
     * 
     * @param {String} id 
     * @param {String} title 
     * @param {String} description 
     * @param {RecipeComponentDto[]} components 
     */
    constructor(id, title, description, components) {
        this.id = id;
        this.title = title;
        this.description = description;
        this.components = components;
    }
}
export class RecipeComponentDto {
    /**
     * 
     * @param {String} name 
     * @param {Number} order 
     * @param {Ingredient[]} ingredients 
     * @param {Instruction[]} instructions 
     */
    constructor(name, order, ingredients, instructions) {
        this.name = name;
        this.order = order;
        this.ingredients = ingredients;
        this.instructions = instructions;
    }
}
export class Ingredient {
    /**
     * 
     * @param {String} name 
     * @param {Number} amount 
     * @param {String} unit 
     */
    constructor(name, amount, unit) {
        this.name = name;
        this.amount = amount;
        this.unit = unit;
    }
}
export class Instruction {
    /**
     * 
     * @param {String} description 
     */
    constructor(description) {
        this.description = description;
    }
}
export class JournalEntryDto {
    /**
     * 
     * @param {String} id 
     * @param {String} author 
     * @param {Date} date 
     * @param {String} description 
     * @param {RecipeDto} recipeUsed 
     */
    constructor(id, author, date, description, recipeUsed) {
        this.id = id;
        this.author = author;
        this.date = date;
        this.description = description;
        this.recipeUsed = recipeUsed;
    }
}