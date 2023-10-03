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
.enabled::before {
    content: 'Y';
}
.disabled::before {
    content: 'N';
}
</style>