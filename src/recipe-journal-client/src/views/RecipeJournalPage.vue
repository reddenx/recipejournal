<template>
    <div>
        Journal for Recipe: {{ recipe.title }}

        <template v-for="item in journalEntries">
            <div 
                :key="item.listItem.id+'1'"
                @click="listItemClicked(item)"> 
                {{ item.listItem.date.toLocaleDateString() }} success: {{item.listItem.successRating}} amount:{{item.listItem.recipeScale}}
            </div>
            <div :key="item.listItem.id+'2'" v-if="item.expanded && item.loaded">
                <button v-if="!item.editing" type="button" @click="editButtonPressed(item)">edit</button>
                <div>
                    {{item.entry.id}} - {{item.entry.date}}
                </div>
                <p>{{item.entry.attemptNotes}}</p>
                <p>{{item.entry.resultNotes}}</p>
                <p>{{item.entry.generalNotes}}</p>
                <p>{{item.entry.nextNotes}}</p>
                stickied: {{item.entry.stickyNext}}, dismissed: {{item.entry.nextDismissed}}
            </div>
        </template>
    </div>
</template>

<script>
import JournalApi, {
    JournalEntryListDto,
    JournalEntryDto,
} from "../scripts/journalApi";
import RecipeApi from "../scripts/recipeApi";

const journalApi = new JournalApi();
const recipeApi = new RecipeApi();

export default {
    props: {
        recipeId: String,
    },
    data: () => ({
        journalEntries: [],
        recipe: {},
    }),
    async mounted() {
        this.recipe = await recipeApi.getRecipeDetails(this.recipeId);
        await this.reloadJournal();
    },
    methods: {
        async reloadJournal() {
            let entries = await journalApi.getJournalForRecipe(this.recipeId);
            if (entries) {
                this.journalEntries.splice(
                    0,
                    this.journalEntries.length,
                    ...entries.map((e) => new EntryListItemViewmodel(e))
                );
            }
        },
        async listItemClicked(item) {
            await item.loadEntry();
            item.expanded = !item.expanded;
        },
    },
};

class EntryListItemViewmodel {
    /**
     * @param {JournalEntryListDto} listItem
     */
    constructor(listItem) {
        this.listItem = listItem;
        this.entry = null;
        this.expanded = false;
        this.loaded = false;
        this.editing = false;
        this.busy = false;
    }

    async loadEntry() {
        if (this.loaded) return;
        this.busy = true;
        this.entry = await journalApi.getJournalEntry(this.listItem.id);
        this.loaded = true;
        this.busy = false;
    }

    async save() {
        this.busy = true;
        let updated = await journalApi.updateJournalEntry(this.entry);
        this.entry = updated;
        this.listItem = new JournalEntryListDto(
            updated.id,
            updated.recipeId,
            updated.recipeScale,
            updated.successRating,
            updated.data,
            updated.stickyNext,
            updated.nextDismissed
        );
        this.entry = await journalApi.getJournalEntry(this.listItem.id);
        this.busy = false;
    }
}
</script>

<style scoped>
</style>