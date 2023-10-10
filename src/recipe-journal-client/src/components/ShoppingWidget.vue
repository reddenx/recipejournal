<template>
    <div class="shopping-widget-container">
        <button
            class="shopping-cart-button"
            @click="shopRecipeAmountChanged(value + 1)"
            v-show="value == 0"
        >
            <span class="fa-solid fa-cart-plus"></span>
        </button>
        <div v-show="value > 0">
            <button
                type="button"
                @click="minusShopButtonPressed()"
                class="shopping-minus-button"
            >
                <span v-if="value <= 1" class="fa-solid fa-trash-can"></span>
                <span v-else>-</span>
            </button>
            <input
                type="number"
                :value="value"
                @input.stop="shopInputChanged($event.target.value, $event.data)"
            />
            <button
                class="shopping-plus-button"
                type="button"
                @click="shopRecipeAmountChanged(value + 1)"
            >
                +
            </button>
        </div>
    </div>
</template>

<script>
export default {
    props: {
        value: Number,
    },
    methods: {
        async shopRecipeAmountChanged(newValue) {
            this.$emit("input", newValue);
        },
        async minusShopButtonPressed() {
            await this.shopRecipeAmountChanged(Math.max(0, this.value - 1));
        },
        async shopInputChanged(input, data) {
            if(!Number(input) && data == '.')
                return;
            let val = Math.round(Number(input)*100)/100;
            await this.shopRecipeAmountChanged(Math.max(val, 0));
        },
    },
};
</script>

<style scoped>

.shopping-widget-container {
    display: inline-block;
}
.shopping-widget-container button {
    width: 2.3em;
    height: 2.3em;
    border-radius: 8px;
}
.shopping-cart-button {
    border: 3px solid #64d564;
    background-color: #90ff94;
}
.shopping-minus-button {
    border: 3px solid #e20000;
    background-color: #ff8686;
}
.shopping-plus-button {
    border: 3px solid #64d564;
    background-color: #90ff94;
}
.shopping-widget-container input {
    max-width: 3em;
    text-align: center;
    border: 3px solid;
    border-left: none;
    border-right: none;
    border-radius: 5px;
}
</style>