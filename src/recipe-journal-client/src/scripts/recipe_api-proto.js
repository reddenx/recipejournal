// export default class RecipeApi {

//     /**
//      * @returns {RecipeListItemDto[]}
//      */
//     async getRecipeList() {
//         return new Promise((yay, nay) => {
//             yay([
//                 new RecipeListItemDto('test1', 'French Maracons'),
//                 new RecipeListItemDto('test2', 'Italian Maracons'),
//             ]);
//         });
//     }

//     /**
//      * 
//      * @param {String} recipeId 
//      * @returns {RecipeDto}
//      */
//     async getRecipe(recipeId) {
//         return new Promise((yay, nay) => {
//             yay(new RecipeDto('test2', 'Italian Macarons', "Italian macarons, the meringue is more stable", [
//                 new RecipeComponentDto('Italian Meringue', 2, [
//                     new Ingredient('egg white', 100, 'g'),
//                     new Ingredient('water', 60, 'g'),
//                     new Ingredient('sugar (syrup)', 230, 'g'),
//                     new Ingredient('suger (stabilizer)', 30, 'g'),
//                 ], [
//                     new Instruction('Place egg white in mixer and start whipping to foam, once foaming add the stabilizer sugar. Meanwhile mix sugar(syrup) and water and put on heat, heat to 118C then slowly drizzle down the side of the mixing bowl into the foam'),
//                     new Instruction("Continue whipping until stiff, it's been said this is roughly 8 minutes but this isn't confirmed."),
//                     new Instruction("While the meringue is stiffening, continue to eggwhite paste and preheat your oven to 300F"),
//                 ]),
//                 new RecipeComponentDto('Eggwhite Paste', 1, [
//                     new Ingredient('egg white', '100', 'g'),
//                     new Ingredient('powdered sugar', '250', 'g'),
//                     new Ingredient('almond flour', '260', 'g'),
//                 ], [
//                     new Instruction('Fold ingredients carefully together as not to release almond oils.')
//                 ]),
//                 new RecipeComponentDto('Combining', 3, [], [
//                     new Instruction('Once the meringue has stiff peaks, preheat to 300F, macronage meringue and paste together, pipe and bake for 10-17 minutes'),
//                 ]),
//             ]));
//         });
//     }

//     /**
//      * 
//      * @param {RecipeDto} recipeDto 
//      * @returns {RecipeDto}
//      */
//     async createRecipe(recipeDto) {
//         return this.getRecipe();
//     }

//     /**
//      * 
//      * @param {RecipeDto} recipeDto 
//      */
//     async updateRecipe(recipeDto) {
//         return new Promise(yay => { yay() });
//     }

//     /**
//      * 
//      * @param {String} recipeId 
//      */
//     async deleteRecipe(recipeId) {
//         return new Promise(yay => { yay() });
//     }

//     /**
//      * 
//      * @param {String} recipeId 
//      * @param {JournalEntryDto} journalEntryDto 
//      * @returns {JournalEntryDto}
//      */
//     async addJournalEntry(recipeId, journalEntryDto) {
//         console.error('not implemented');
//     }

//     /**
//      * 
//      * @param {String} recipeId 
//      * @returns {JournalEntryDto[]}
//      */
//     async getEntriesForRecipe(recipeId) {
//         console.error('not implemented');
//     }

//     /**
//      * 
//      * @param {JournalEntryDto} journalEntry
//      */
//     async updateJournalEntry(journalEntry) { console.error('not implemented'); }

//     /**
//      * 
//      * @param {String} journalEntryId 
//      */
//     async deleteJournalEntry(journalEntryId) { console.error('not implemented'); }

//     /**
//      * @param {Number} count
//      * @returns {JournalEntryDto[]}
//      */
//     async getRecentJournalEntries(count) { console.error('not implemented'); }
// }

// /**
//  * a trimmed down recipe object for use in lists
//  */
// export class RecipeListItemDto {
//     /**
//      * 
//      * @param {String} id 
//      * @param {String} title 
//      */
//     constructor(id, title) {
//         this.id = id;
//         this.title = title;
//     }
// }

// export class RecipeDto {
//     /**
//      * 
//      * @param {String} id 
//      * @param {String} title 
//      * @param {String} description 
//      * @param {RecipeComponentDto[]} components 
//      */
//     constructor(id, title, description, components) {
//         this.id = id;
//         this.title = title;
//         this.description = description;
//         this.components = components;
//     }
// }
// export class RecipeComponentDto {
//     /**
//      * 
//      * @param {String} name 
//      * @param {Number} order 
//      * @param {Ingredient[]} ingredients 
//      * @param {Instruction[]} instructions 
//      */
//     constructor(name, order, ingredients, instructions) {
//         this.name = name;
//         this.order = order;
//         this.ingredients = ingredients;
//         this.instructions = instructions;
//     }
// }
// export class Ingredient {
//     /**
//      * 
//      * @param {String} name 
//      * @param {Number} amount 
//      * @param {String} unit 
//      */
//     constructor(name, amount, unit) {
//         this.name = name;
//         this.amount = amount;
//         this.unit = unit;
//     }
// }
// export class Instruction {
//     /**
//      * 
//      * @param {String} description 
//      */
//     constructor(description) {
//         this.description = description;
//     }
// }
// export class JournalEntryDto {
//     /**
//      * 
//      * @param {String} id 
//      * @param {String} author 
//      * @param {Date} date 
//      * @param {String} description 
//      * @param {RecipeDto} recipeUsed 
//      */
//     constructor(id, author, date, description, recipeUsed) {
//         this.id = id;
//         this.author = author;
//         this.date = date;
//         this.description = description;
//         this.recipeUsed = recipeUsed;
//     }
// }