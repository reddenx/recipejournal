<template>
  <div class="component-container">
    <div v-if="!isEditing" class="saved-container">
      <p>
        <a @click="isEditing = true" class="value">{{ value ? value : label }}</a>
      </p>
    </div>

    <div class="editing-container" v-if="isEditing">
      <textarea
        class="value-input"
        :value="value"
        @input="$emit('input', $event.target.value)"
        :placeholder="label"
      />
      <button class="save-button" type="button" @click="isEditing = false">
        <i class="fa-regular fa-floppy-disk"></i>
      </button>
    </div>
  </div>
</template>

<script>
export default {
  props: ["value", "label"],
  data: () => ({ isEditing: false }),
};
</script>

<style scoped>
.component-container {
  display: block;
}
.saved-container {
  display: inline-block;
}
.editing-container {
  display: block;
}
/* .save-button {} */
/* .edit-button {} */
.value {
  display: inline-block;
}
.value-input {
  display: inline-block;
  height: 7em;
  width: 100%;
}
</style>