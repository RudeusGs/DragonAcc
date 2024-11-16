<!-- ConfirmationModal.vue -->
<template>
  <div v-if="isVisible" class="modal-overlay">
    <div :class="['modal-content', modalType]">
      <i :class="iconClass" class="modal-icon"></i>
      <h2>{{ title }}</h2>
      <p>{{ message }}</p>
      <div class="modal-buttons">
        <!-- Nút Xác Nhận và Hủy chỉ hiện khi loại modal là 'confirmation' -->
        <button
          v-if="type === 'confirmation'"
          @click="onConfirm"
          class="confirm-button"
        >
          {{ confirmText }}
        </button>
        <button
          v-if="showCancel && type !== 'success' && type !== 'error'"
          @click="onCancel"
          class="cancel-button"
        >
          {{ cancelText }}
        </button>
        <!-- Slot cho các nút bổ sung nếu cần -->
        <slot name="additional-buttons"></slot>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { defineProps, defineEmits, computed } from 'vue';

const props = defineProps({
  isVisible: { type: Boolean, required: true },
  title: { type: String, default: 'Thông báo' },
  message: { type: String, default: 'Bạn có chắc chắn?' },
  confirmText: { type: String, default: 'Đồng ý' },
  cancelText: { type: String, default: 'Hủy' },
  type: { type: String, default: 'confirmation' }, // 'confirmation', 'success', 'error', 'info'
  showCancel: { type: Boolean, default: true }, // Hiển thị nút hủy hay không
});

const emit = defineEmits(['confirm', 'cancel']);

const onConfirm = () => emit('confirm');
const onCancel = () => emit('cancel');

// Tính toán lớp CSS dựa trên loại modal
const modalType = computed(() => {
  switch (props.type) {
    case 'success':
      return 'modal-success';
    case 'error':
      return 'modal-error';
    case 'info':
      return 'modal-info';
    default:
      return 'modal-confirmation';
  }
});

// Tính toán lớp biểu tượng dựa trên loại modal
const iconClass = computed(() => {
  switch (props.type) {
    case 'success':
      return 'fas fa-check-circle';
    case 'error':
      return 'fas fa-exclamation-triangle';
    case 'info':
      return 'fas fa-info-circle';
    default:
      return 'fas fa-question-circle';
  }
});
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
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
  position: relative;
}

.modal-icon {
  font-size: 48px;
  margin-bottom: 16px;
}

.modal-confirmation .modal-icon {
  color: #2196f3; /* Blue for confirmation */
}

.modal-success .modal-icon {
  color: #4caf50; /* Green for success */
}

.modal-error .modal-icon {
  color: #f44336; /* Red for error */
}

.modal-info .modal-icon {
  color: #ff9800; /* Orange for info */
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

/* Các kiểu riêng cho từng loại modal */
.modal-success {
  border-top: 5px solid #4caf50; /* Green border */
}

.modal-error {
  border-top: 5px solid #f44336; /* Red border */
}

.modal-info {
  border-top: 5px solid #ff9800; /* Orange border */
}

.modal-confirmation {
  border-top: 5px solid #2196f3; /* Blue border */
}
</style>
