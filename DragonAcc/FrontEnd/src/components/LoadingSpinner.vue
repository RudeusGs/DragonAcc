<template> 
  <div v-if="isLoading" class="loading-overlay"> 
    <div class="loading-container">
      <img style="width: 120px;" src="../assets/hinhdong.gif" alt="Loading Animation" class="loading-icon" />
      <div class="loading-bar">
        <div class="loading-progress"></div>
      </div>
    </div>
  </div> 
</template> 

<script lang="ts"> 
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'LoadingSpinner',
  props: {
    isLoading: { 
      type: Boolean, 
      required: true, 
    },
  },
});
</script>

<style scoped>
.loading-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background-color: #f3f6f8;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999;
  overflow: hidden;
}

.loading-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  position: relative;
}

.loading-icon {
  width: 80px;
  height: auto;
  animation: bounce 1.5s ease-in-out infinite;
}

.loading-bar {
  position: relative;
  top: 15px;
  width: 80%;
  max-width: 600px;
  height: 6px;
  background-color: #d3dce6;
  border-radius: 3px;
  overflow: hidden;
  margin-top: 10px;
}

.loading-progress {
  width: 0;
  height: 100%;
  background: linear-gradient(90deg, #0073b1, #005582);
  animation: load 3s linear infinite;
}

@keyframes bounce {
  0%, 100% { transform: translateY(0); }
  50% { transform: translateY(-5px); }
}

@keyframes load {
  0% { width: 0; }
  100% { width: 100%; }
}
</style>
