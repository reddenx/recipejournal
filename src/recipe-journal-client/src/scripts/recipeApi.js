import axios from 'axios';

export default class RecipeApi {
    async getRecipeList() {
        try {
            let result = await axios.get('/api/v1/recipes');
            if (result.data) {
                let dto = result.data;
                return dto.map(r => new RecipeListDto(r.id, r.title, r.durationMinutes, r.servings));
            }
            return null;
        }
        catch {
            return null;
        }
    }
    async getRecipeDetails(id) {
        try {
            let result = await axios.get('/api/v1/recipes/' + id);
            if (result.data) {
                let dto = result.data;
                return new RecipeDto(dto.id, dto.title, dto.description, dto.author, dto.durationMinutes, dto.servings,
                    dto.components.map(c => new RecipeComponentDto(c.id, c.number, c.title, c.description, c.steps.map(s => new RecipeStepDto(s.id, s.number, s.title, s.body, s.ingredients.map(i => new RecipeIngredientDto(i.id, i.number, i.name, i.unit, i.amount)))))),
                    dto.isDraft, dto.isPublic);
            }
            return null;
        } catch {
            return null;
        }
    }
    async updateRecipe(recipeDto) {
        try {
            let result = await axios.put('/api/v1/recipes', recipeDto);
            if (result.data) {
                let dto = new RecipeComponentDto(result.data.id, result.data.number, result.data.title, result.data.description, result.data.steps.map(s => new RecipeStepDto(s.id, s.number, s.title, s.body, s.ingredients.map(i => new RecipeIngredientDto(i.id, i.number, i.name, i.unit, i.amount)))));
                return dto;
            }
            return null;
        } catch {
            return null;
        }
    }
}



//recipe in full read detail with components, steps, and ingredients
export class RecipeDto {
    constructor(id, title, description, author, durationMinutes, servings, components, isDraft, isPublic) {
        this.id = id;
        this.title = title;
        this.description = description;
        this.author = author;
        this.durationMinutes = durationMinutes;
        this.servings = servings;
        this.components = components;
        this.isDraft = isDraft;
        this.isPublic = isPublic;
    }
}
export class RecipeComponentDto {
    constructor(id, number, title, description, steps) {
        this.id = id;
        this.number = number;
        this.title = title;
        this.description = description;
        this.steps = steps;
    }
}
export class RecipeStepDto {
    constructor(id, number, title, body, ingredients) {
        this.id = id;
        this.number = number;
        this.title = title;
        this.body = body;
        this.ingredients = ingredients;
    }
}
export class RecipeIngredientDto {
    constructor(id, number, name, unit, amount) {
        this.id = id;
        this.number = number;
        this.name = name;
        this.unit = unit;
        this.amount = amount;
    }
}

//recipe list item with just description and display metadata
export class RecipeListDto {
    constructor(id, title, durationMinutes, servings) {
        this.id = id;
        this.title = title;
        this.durationMinutes = durationMinutes;
        this.servings = servings;
    }
}
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




// const recipes = [
//     new RecipeDto('1', 'German Apply Pancakes', 'fluffy cakes w/ apples', 'santhmas marky', 30, 2, [
//         new RecipeComponentDto('1', '', '', [
//             new RecipeStepDto('2', 'step 1', 'mix together eggs, flour, salt, milk.', [
//                 new RecipeIngredientDto('3', 'egg', 'count', 4),
//                 new RecipeIngredientDto('4', 'flour', 'cup', .75),
//                 new RecipeIngredientDto('5', 'salt', 'teaspoon', 1),
//             ]),
//             new RecipeStepDto('6', 'step 2', 'melt butter into pans, place cut apples into pans, concentric circles usually work best. I need some longer content here to test wrapping and display ugh', [
//                 new RecipeIngredientDto('7', 'butter', 'tablespoon', 4),
//                 new RecipeIngredientDto('8', 'apple', 'count', 2)
//             ]),
//             new RecipeStepDto('9', 'step 3', 'mix together sugar and cinnamon', [
//                 new RecipeIngredientDto('10', 'sugar', 'cup', .33),
//                 new RecipeIngredientDto('11', 'flour', 'cup', .75),
//             ]),
//             new RecipeStepDto('12', 'step 4', 'pour mixture over apples'),
//             new RecipeStepDto('13', 'step 5', 'sprinkle surgar over pans, covering them evenly')
//         ])
//     ]),
//     new RecipeDto('2', 'Italian Macarons', 'a harder way to do the thing that was already hard!', 'jimmyjon', 3 * 60, 30, [
//         new RecipeComponentDto('14', 'Cookie', '', [
//             new RecipeStepDto('15', 'start eggs', 'Place egg white in mixer and start whipping to foam, once foaming add the stabilizer sugar', [
//                 new RecipeIngredientDto('16', 'egg white', 'gram', 100),
//                 new RecipeIngredientDto('17', 'sugar', 'gram', 30)
//             ]),
//             new RecipeStepDto('18', 'start syrup', 'Meanwhile mix sugar and water and put on heat. constantly stir once on heat', [
//                 new RecipeIngredientDto('19', 'sugar', 'gram', 230),
//                 new RecipeIngredientDto('20', 'water', 'gram', 60),
//             ]),
//             new RecipeStepDto('21', 'combine', 'ideally, the syrup will reach 118C around the same time the meringue should be at the right state, this takes practice on timing. The meringue is ready when it\'s still slightly foamy but almost to soft peaks. remove syrup from heat and drizzle slowly down the side of the mixing bowl while egg whites are still being beaten, ensure the syrup is being picked up by the beater and not landing in one area (trying to prevent the egg from entirely cooking). beat this until stiff peaks, in the mean time start your paste', []),
//             new RecipeStepDto('22', 'the paste', 'sift and combine egg white, powdered sugar, and almond floud, mix gently trying not to add any air to the mixture', [
//                 new RecipeIngredientDto('23', 'egg white', 'gram', 100),
//                 new RecipeIngredientDto('24', 'powdered sugar', 'gram', 250),
//                 new RecipeIngredientDto('25', 'almond floud', 'gram', 260)
//             ]),
//             new RecipeStepDto('26', 'fold', 'once the meringue has reached stiff peaks, fold paste and meringue until right consistency is reached, be sure not to be too forceful and release the oils from the almond floud', []),
//             new RecipeStepDto('27', 'pipe and rest', 'pipe onto silicon mat, let sit until skin is firm enough to bake (usually 10-30 min in semi-humid climate)', []),
//             new RecipeStepDto('28', 'bake', 'bake at 300F for 10-17 minutes, experimentation still required here')
//         ])
//     ])
// ]