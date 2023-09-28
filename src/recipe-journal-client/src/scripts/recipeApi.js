export default class RecipeApi {
    async getRecipeList() {
        return [new RecipeListDto('German Apply Pancakes', 'fluffy cakes w/ apples', [
            new RecipeComponentDto('','', [
                new RecipeStepDto('', 'mix together eggs, flour, salt, milk.', [
                    new IngredientDto('egg', 'count', 4),
                    new IngredientDto('flour', 'cup', .75),
                    new IngredientDto('salt', 'teaspoon', 1),
                ]),
                new RecipeStepDto('', 'melt butter into pans, place cut apples into pans, concentric circles usually work best', [
                    new IngredientDto('butter', 'tablespoon', 4),
                    new IngredientDto('apple', 'count', 2)
                ]),
                new RecipeStepDto('', 'mix together sugar and cinnamon', [
                    new IngredientDto('sugar', 'cup', .33),
                    new IngredientDto('flour', 'cup', .75),
                ]), 
                new RecipeStepDto('', 'pour mixture over apples'), 
                new RecipeStepDto('', 'sprinkle surgar over pans, covering them evenly')
            ])
        ])];
    }
    async getRecipeDetails(id) { }
    async updateRecipe(updateRecipeDto) { }
    async createRecipe(createRecipeDto) { }
    async getRecipeJournal(id) { }
    async addRecipeJournalEntry(createJournalEntryDto) { }
    async updateRecipeJournalEntry(updateJournalEntryDto) { }
}

//recipe in full read detail with components, steps, and ingredients
class RecipeDto { }
//recipe list item with just description and display metadata
class RecipeListDto { }
//input for updating a recipe from the cms
class UpdateRecipeDto { }
//input for creating a recipe from the cms
class CreateRecipeDto { }
//a journal for a given recipe
class RecipeJournalDto { }
//input for creating a journal entry
class CreateJournalEntryDto { }
//input for updating a journal entry
class UpdateJournalEntryDto { }