<!-- ConfirmationModal.vue -->
<template>
    <div v-if="isVisible" class="modal-overlay">
      <div class="modal-content">
        <h2>{{ title }}</h2>
        <p>{{ message }}</p>
        <div class="modal-buttons">
          <button @click="onConfirm" class="confirm-button">{{ confirmText }}</button>
          <button @click="onCancel" class="cancel-button">{{ cancelText }}</button>
        </div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { defineProps, defineEmits } from 'vue';
  
  const props = defineProps({
    isVisible: { type: Boolean, required: true },
    title: { type: String, default: 'Confirmation' },
    message: { type: String, default: 'Are you sure?' },
    confirmText: { type: String, default: 'Yes' },
    cancelText: { type: String, default: 'No' }
  });
  
  const emit = defineEmits(['confirm', 'cancel']);
  
  const onConfirm = () => emit('confirm');
  const onCancel = () => emit('cancel');
  </script>
  <style scoped>
  .modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 1000;
  }
  
  .modal-content {
    background-color: #fff;
    padding: 24px;
    width: 400px;
    border-radius: 8px;
    box-shadow: 0 4px 16px rgba(0, 0, 0, 0.2);
    text-align: center;
  }
  
  .modal-content h2 {
    font-size: 22px;
    color: #333;
    margin-bottom: 12px;
  }
  
  .modal-content p {
    font-size: 16px;
    color: #666;
    margin-bottom: 20px;
  }
  
  .modal-buttons {
    display: flex;
    justify-content: center;
    gap: 15px;
  }
  
  .confirm-button {
    background-color: #0073b1;
    color: #fff;
    padding: 10px 20px;
    border: none;
    border-radius: 4px;
    font-size: 14px;
    cursor: pointer;
  }
  
  .cancel-button {
    background-color: #ddd;
    color: #333;
    padding: 10px 20px;
    border: none;
    border-radius: 4px;
    font-size: 14px;
    cursor: pointer;
  }
  
  .confirm-button:hover {
    background-color: #005582;
  }
  
  .cancel-button:hover {
    background-color: #ccc;
  }
  </style>
  