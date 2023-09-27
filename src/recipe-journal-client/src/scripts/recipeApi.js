export default class RecipeApi {
    async getRecipeList() { }
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