<template>
  <div class="component-container">
    <div class="saved-contaienr" v-if="!isEditing">
      <a @click="isEditing = true" class="value">{{ value ? value : label }}</a>
    </div>
    <div class="editing-container" v-if="isEditing">
      <input class="value-input"
        type="text"
        :value="value"
        @input="$emit('input', $event.target.value)"
        :placeholder="label"
        :style="{width: length + 'em'}"
      />
      <button class="save-button" type="button" @click="isEditing = false">
        <i class="fa-regular fa-floppy-disk"></i>
      </button>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    value: String,
    label: String,
    length: Number,
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
.value {
  display: inline-block;
}
.value-input {
  display: inline-block;
}
</style>