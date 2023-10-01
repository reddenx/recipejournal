export default class RecipeApi {
    async getRecipeList() {
        return [new RecipeDto('1', 'German apple pancakes')];
    }
    async getRecipeDetails(id) {
        return recipes.find(r => r.id == id);
    }
    async updateRecipe(updateRecipeDto) { }
    async createRecipe(createRecipeDto) { }
    async getRecipeJournal(id) { }
    async addRecipeJournalEntry(createJournalEntryDto) { }
    async updateRecipeJournalEntry(updateJournalEntryDto) { }
}



//recipe in full read detail with components, steps, and ingredients
class RecipeDto {
    constructor(id, title, description, durationMinutes, servings, components, isDraft, isPublic) {
        this.id = id;
        this.title = title;
        this.description = description;
        this.durationMinutes = durationMinutes;
        this.servings = servings;
        this.components = components;
        this.isDraft = isDraft;
        this.isPublic = isPublic;
    }
}
class RecipeComponentDto {
    constructor(title, description, steps) {
        this.title = title;
        this.description = description;
        this.steps = steps;
    }
}
class RecipeStepDto {
    constructor(title, body, ingredients) {
        this.title = title;
        this.body = body;
        this.ingredients = ingredients;
    }
}
class RecipeIngredientDto {
    constructor(name, unit, amount) {
        this.name = name;
        this.unit = unit;
        this.amount = amount;
    }
}

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




const recipes = [
    new RecipeDto('1', 'German Apply Pancakes', 'fluffy cakes w/ apples', 30, 2, [
        new RecipeComponentDto('', '', [
            new RecipeStepDto('step 1', 'mix together eggs, flour, salt, milk.', [
                new RecipeIngredientDto('egg', 'count', 4),
                new RecipeIngredientDto('flour', 'cup', .75),
                new RecipeIngredientDto('salt', 'teaspoon', 1),
            ]),
            new RecipeStepDto('step 2', 'melt butter into pans, place cut apples into pans, concentric circles usually work best. I need some longer content here to test wrapping and display ugh', [
                new RecipeIngredientDto('butter', 'tablespoon', 4),
                new RecipeIngredientDto('apple', 'count', 2)
            ]),
            new RecipeStepDto('step 3', 'mix together sugar and cinnamon', [
                new RecipeIngredientDto('sugar', 'cup', .33),
                new RecipeIngredientDto('flour', 'cup', .75),
            ]),
            new RecipeStepDto('step 4', 'pour mixture over apples'),
            new RecipeStepDto('step 5', 'sprinkle surgar over pans, covering them evenly')
        ])
    ]),
    new RecipeDto('2', 'Italian Macarons', 'a harder way to do the thing that was already hard!', 3 * 60, 30, [
        new RecipeComponentDto('Cookie', '', [
            new RecipeStepDto('start eggs', 'Place egg white in mixer and start whipping to foam, once foaming add the stabilizer sugar', [
                new RecipeIngredientDto('egg white', 'gram', 100),
                new RecipeIngredientDto('sugar', 'gram', 30)
            ]),
            new RecipeStepDto('start syrup', 'Meanwhile mix sugar and water and put on heat. constantly stir once on heat', [
                new RecipeIngredientDto('sugar', 'gram', 230),
                new RecipeIngredientDto('water', 'gram', 60),
            ]),
            new RecipeStepDto('combine', 'ideally, the syrup will reach 118C around the same time the meringue should be at the right state, this takes practice on timing. The meringue is ready when it\'s still slightly foamy but almost to soft peaks. remove syrup from heat and drizzle slowly down the side of the mixing bowl while egg whites are still being beaten, ensure the syrup is being picked up by the beater and not landing in one area (trying to prevent the egg from entirely cooking). beat this until stiff peaks, in the mean time start your paste', []),
            new RecipeStepDto('the paste', 'sift and combine egg white, powdered sugar, and almond floud, mix gently trying not to add any air to the mixture', [
                new RecipeIngredientDto('egg white', 'gram', 100),
                new RecipeIngredientDto('powdered sugar', 'gram', 250),
                new RecipeIngredientDto('almond floud', 'gram', 260)
            ]),
            new RecipeStepDto('fold', 'once the meringue has reached stiff peaks, fold paste and meringue until right consistency is reached, be sure not to be too forceful and release the oils from the almond floud', []),
            new RecipeStepDto('pipe and rest', 'pipe onto silicon mat, let sit until skin is firm enough to bake (usually 10-30 min in semi-humid climate)', []),
            new RecipeStepDto('bake', 'bake at 300F for 10-17 minutes, experimentation still required here')
        ])
    ])
]