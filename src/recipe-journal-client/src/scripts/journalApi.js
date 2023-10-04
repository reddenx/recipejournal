import axios from 'axios'

export default class JournalApi {
    constructor() { }

    /**
     * @param {String} recipeId 
     * @returns {JournalEntryListDto[]}
     */
    async getJournalForRecipe(recipeId) {
        return mockEntries.map(e => new JournalEntryListDto(e.id, e.recipeId, e.recipeScale, e.successRating, e.date, e.stickyNext, e.nextDismissed));
    }
    /**
     * @param {String} entryId 
     * @returns {JournalEntryDto}
     */
    async getJournalEntry(entryId) {
        return mockEntries.find(e => e.id == entryId);
    }
    /**
     * @param {JournalEntryDto} updateEntryDto 
     * @returns {JournalEntryDto}
     */
    async updateJournalEntry(entryDto) {
        return entryDto;
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
        this.date = date;
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
        this.date = date;
        this.nextDismissed = nextDismissed;
        this.stickyNext = stickyNext;
    }
}

const mockEntries = [
    new JournalEntryDto('mock-entry-id-1', 'recipe-id', 1.1, 0.8, new Date(), 'trying for a better rise', 'still did not rise', 'punched things a lot, broke a plate, very hungry', 'try punching less? may make rise better', true, false),
    new JournalEntryDto('mock-entry-id-2', 'recipe-id', 1, 0.9, new Date(), 'trying for a better rise again', 'still did not rise again', 'punched things less, did not break a plate, less hungry', 'try punching even less? ', true, false),
];