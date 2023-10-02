<template>
  <div class="component-container">
    <div v-if="!isEditing" class="saved-container">
      <a @click="isEditing = true" style="" class="value">{{ value ? value : label }}</a>
    </div>

    <div v-if="isEditing" class="editing-container">
      <input
        class="value-input"
        :style="{'width': width + 'em'}"
        type="number"
        :value="value"
        @input="$emit('input', $event.target.value)"
        :placeholder="label"
      />
      <button class="save-button" type="button" @click="isEditing = false">
        C
      </button>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    value: Number,
    width: {
      type: Number,
      default: 4,
    },
    label: String
  },
  data: () => ({ isEditing: false }),
};
</script>

<style scoped>
.component-container {
  display: inline-block;
}
.saved-container {
  display: inline-block;
}
.editing-container {
  display: inline-block;
}
/* .save-button {} */
/* .edit-button {} */
/* .value {} */
/* .value-input */
</style>