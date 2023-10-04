<template>
    <div>
        Journal for Recipe: {{ recipe.title }}

        <template v-for="item in journalEntries">
            <div :key="item.listItem.id + '1'" @click="listItemClicked(item)">
                {{ formatDate(item.listItem.date) }} success:
                {{ item.listItem.successRating }} amount:{{
                    item.listItem.recipeScale
                }}
            </div>
            <div
                :key="item.listItem.id + '2'"
                v-if="item.expanded && item.loaded"
            >
                <div v-if="!item.editing">
                    <button type="button" @click="editButtonPressed(item)">
                        edit
                    </button>
                    <div>{{ item.entry.id }} - {{ item.entry.date }}</div>
                    <p>{{ item.entry.attemptNotes }}</p>
                    <p>{{ item.entry.resultNotes }}</p>
                    <p>{{ item.entry.generalNotes }}</p>
                    <p>{{ item.entry.nextNotes }}</p>
                    stickied: {{ item.entry.stickyNext }}, dismissed:
                    {{ item.entry.nextDismissed }}
                </div>
                <div v-if="item.editing">
                    <button type="button" @click="saveButtonPressed(item)">
                        Save
                    </button>
                    <button type="button" @click="cancelButtonPressed(item)">
                        Cancel
                    </button>
                    <div>{{ item.entry.id }} - {{ item.entry.date }}</div>
                    Success: <NumberInput v-model="item.entry.successRating" />
                    Amount: <NumberInput v-model="item.entry.recipeScale" />
                    <TextBoxInput v-model="item.entry.attemptNotes" :label="'attmpted notes'"/>
                    <TextBoxInput v-model="item.entry.resultNotes" :label="'result'" />
                    <TextBoxInput v-model="item.entry.generalNotes" :label="'notes'"/>
                    <TextBoxInput v-model="item.entry.nextNotes" :label="'for next time'" />
                    <ToggleInput
                        v-model="item.entry.stickyNext"
                        :disabledText="'not sticked'"
                        :enabledText="'sticked'"
                    />
                    <ToggleInput
                        v-show="item.entry.stickyNext"
                        v-model="item.entry.nextDismissed"
                        :disabledText="'show'"
                        :enabledText="'dismissed'"
                    />
                </div>
            </div>
        </template>
        <button v-show="journalEntries.length && journalEntries[journalEntries.length-1].listItem.id" type="button" @click="addNewButtonPressed">+New</button>
    </div>
</template>

<script>
import NumberInput from '../components/cms/NumberInput.vue';
import TextBoxInput from "../components/cms/TextBoxInput.vue";
import ToggleInput from "../components/cms/ToggleInput.vue";
import JournalApi, {
    JournalEntryListDto,
    JournalEntryDto,
} from "../scripts/journalApi";
import RecipeApi from "../scripts/recipeApi";

const journalApi = new JournalApi();
const recipeApi = new RecipeApi();

export default {
    components: { TextBoxInput, ToggleInput, NumberInput },
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
        formatDate(date){
            return date?.toLocaleDateString() ?? '';
        },
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
        async editButtonPressed(item) {
            item.editing = true;
        },
        cancelButtonPressed(item) {
            item.editing = false;
            if(!item.listItem.id) {
                let index = this.journalEntries.indexOf(item);
                this.journalEntries.splice(index, 1);
            }
        },
        async saveButtonPressed(item) {
            await item.save();
            item.editing = false;
        },
        addNewButtonPressed() {
            let newModel = new EntryListItemViewmodel(new JournalEntryListDto(null, null, 1, 0.5, null, true, false));
            newModel.loaded = true;
            newModel.entry = new JournalEntryDto(null, null, 1, 0.5, null, '', '', '', '', true, false);
            newModel.expanded = true;
            newModel.editing = true;
            
            this.journalEntries.push(newModel)
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
            updated.date,
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