<template> 
  <div v-if="isLoading" class="loading-overlay"> 
    <img style="width: 300px;" src="../assets/hinhdong.gif" alt="Loading Bunny" class="bunny-icon" />
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
  background-color: #fff5fa;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999;
  overflow: hidden;
}

.bunny-icon {
  width: 150px;
  height: auto;
  animation: bounce 1.5s ease-in-out infinite, float 2.5s ease-in-out infinite;
}

@keyframes bounce {
  0%, 100% { transform: translateY(0); }
  50% { transform: translateY(-10px); }
}

@keyframes float {
  0%, 100% { transform: translateY(0); }
  50% { transform: translateY(-5px); }
}
</style>
