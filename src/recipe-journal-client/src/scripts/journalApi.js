import axios from 'axios'

export default class JournalApi {
    constructor() { }

    /**
     * @param {String} recipeId 
     * @returns {JournalEntryListDto[]}
     */
    async getJournalForRecipe(recipeId) {
        try {
            let result = await axios.get(`/api/v1/recipes/${recipeId}/journal`);
            if(result.data) {
                return result.data.map(d => new JournalEntryListDto(
                    d.id,
                    d.recipeId,
                    d.recipeScale,
                    d.successRating,
                    d.date,
                    d.stickyNext,
                    d.nextDismissed
                ));
            }
        }
        catch {
            //
        }
        return [];
        
        
        // return mockEntries.map(e => new JournalEntryListDto(e.id, e.recipeId, e.recipeScale, e.successRating, e.date, e.stickyNext, e.nextDismissed));
    }
    /**
     * @param {String} entryId 
     * @returns {JournalEntryDto}
     */
    async getJournalEntry(recipeId, entryId) {
        try {
            let result = await axios.get(`/api/v1/recipes/${recipeId}/journal/${entryId}`);
            if(result.data) {
                return new JournalEntryDto(
                    result.data.id,
                    result.data.recipeId,
                    result.data.recipeScale,
                    result.data.successRating,
                    result.data.date,
                    result.data.attemptNotes,
                    result.data.resultNotes,
                    result.data.generalNotes,
                    result.data.nextNotes,
                    result.data.stickyNext,
                    result.data.nextDismissed
                );
            }
        }
        catch {
            //
        }
        return null;
        
        // return mockEntries.find(e => e.id == entryId);
    }
    /**
     * @param {JournalEntryDto} updateEntryDto 
     * @returns {JournalEntryDto}
     */
    async updateJournalEntry(recipeId, entryDto) {
        try {
            let result =await axios.put(`/api/v1/recipes/${recipeId}/journal`, entryDto);
            if(result.data) {
                return new JournalEntryDto(
                    result.data.id,
                    result.data.recipeId,
                    result.data.recipeScale,
                    result.data.successRating,
                    result.data.date,
                    result.data.attemptNotes,
                    result.data.resultNotes,
                    result.data.generalNotes,
                    result.data.nextNotes,
                    result.data.stickyNext,
                    result.data.nextDismissed
                );
            }
        }
        catch {
            //
        }

        return null;
        // return entryDto;
    }
}

export class JournalEntryDto {
    /**
     * 
     * @param {String} id 
     * @param {String} recipeId 
     * @param {Number} recipeScale 
     * @param {Number} successRating 
     * @param {Date} date
     * @param {String} attemptNotes 
     * @param {String} resultNotes 
     * @param {String} generalNotes 
     * @param {String} nextNotes 
     * @param {Boolean} stickyNext 
     * @param {Boolean} nextDismissed 
     */
    constructor(id, recipeId, recipeScale, successRating, date, attemptNotes, resultNotes, generalNotes, nextNotes, stickyNext, nextDismissed) {
        this.id = id;
        this.recipeId = recipeId;
        this.recipeScale = recipeScale;
        this.successRating = successRating;
        this.date = date == null ? null : new Date(date);
        this.attemptNotes = attemptNotes;
        this.resultNotes = resultNotes;
        this.generalNotes = generalNotes;
        this.nextNotes = nextNotes;
        this.stickyNext = stickyNext;
        this.nextDismissed = nextDismissed;
    }
}
export class JournalEntryListDto {
    /**
     * 
     * @param {String} id 
     * @param {String} recipeId 
     * @param {Number} recipeScale 
     * @param {Number} successRating 
     * @param {Date} date 
     * @param {Boolean} stickyNext 
     * @param {Boolean} nextDismissed 
     */
    constructor(id, recipeId, recipeScale, successRating, date, stickyNext, nextDismissed) {
        this.id = id;
        this.recipeId = recipeId;
        this.recipeScale = recipeScale;
        this.successRating = successRating;
        this.date = date == null ? null : new Date(date);
        this.stickyNext = stickyNext;
        this.nextDismissed = nextDismissed;
    }
}

const mockEntries = [
    new JournalEntryDto('mock-entry-id-1', 'recipe-id', 1.1, 0.8, new Date(), 'trying for a better rise', 'still did not rise', 'punched things a lot, broke a plate, very hungry', 'try punching less? may make rise better', true, false),
    new JournalEntryDto('mock-entry-id-2', 'recipe-id', 1, 0.9, new Date(), 'trying for a better rise again', 'still did not rise again', 'punched things less, did not break a plate, less hungry', 'try punching even less? ', true, false),
];