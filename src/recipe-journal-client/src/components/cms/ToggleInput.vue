<template>
    <div class="component-container">
        <button :class="{ enabled: value, disabled: !value }" @click="inputToggled">
            {{ value ? enabledText : disabledText }}
        </button>
    </div>
</template>

<script>
export default {
    props: {
        value: Boolean,
        enabledText: String,
        disabledText: String,
    },
    data: () => ({}),
    methods: {
        inputToggled(e) {
            this.$emit('input', !this.value);
        }
    }
};
</script>

<style scoped>
.component-container {
    display: inline-block;
}


.disabled,.enabled {
    height: 2.3em;
    border-radius: 8px;
    border: 3px solid #ffffff;
    background-color: #00aeff;
    font-weight: bold;
    color: white;
}

</style>