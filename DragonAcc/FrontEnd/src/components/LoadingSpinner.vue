<template> 
  <div v-if="isLoading" class="loading-overlay"> 
    <div class="loading-container">
      <img style="width: 300px;" src="../assets/hinhdong.gif" alt="Loading Pikachu" class="pikachu-icon" />
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
  background-color: #79bcff;
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

.pikachu-icon {
  width: 150px;
  height: auto;
  animation: bounce 1.5s ease-in-out infinite, float 2.5s ease-in-out infinite;
  position: relative;
}

.loading-bar {
  position: relative;
  top: -20px;
  width: 1000px;
  height: 10px;
  background-color: #79bcff;
  border-radius: 5px;
  overflow: hidden;
  margin-top: 10px;
}

.loading-progress {
  width: 0;
  height: 100%;
  background: linear-gradient(90deg, #79bcff, #1e86ee);
  animation: load 3s linear infinite;
}

@keyframes bounce {
  0%, 100% { transform: translateY(0); }
  50% { transform: translateY(-10px); }
}

@keyframes float {
  0%, 100% { transform: translateY(0); }
  50% { transform: translateY(-5px); }
}

@keyframes load {
  0% { width: 0; }
  100% { width: 100%; }
}
</style>
