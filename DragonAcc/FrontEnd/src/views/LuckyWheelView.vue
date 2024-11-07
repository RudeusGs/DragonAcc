<template>
    <div class="spin-container">
      <!-- Spin Wheel Section -->
      <div class="spin-wheel-section">
        <div class="spin-wheel">
          <div class="wheel" ref="wheel">
            <div
              class="segment"
              v-for="(segment, index) in segments"
              :key="index"
              :style="{ transform: 'rotate(' + index * (360 / segments.length) + 'deg)' }"
            >
              <span>{{ segment }}</span>
            </div>
          </div>
          <button class="spin-button" @click="spinWheel">Quay</button>
        </div>
      </div>
  
      <!-- Winners List Section -->
      <div class="winners-list-section">
        <div class="my-wins">
          <h3>Danh Sách Tôi Trúng Thưởng</h3>
          <ul>
            <li v-for="(win, index) in myWins" :key="index">{{ win }}</li>
          </ul>
        </div>
        <div class="all-winners">
          <h3>Danh Sách Người Trúng Thưởng</h3>
          <ul>
            <li v-for="(winner, index) in allWinners" :key="index">{{ winner }}</li>
          </ul>
        </div>
      </div>
  
      <!-- Modal for Winning Prize -->
      <div class="modal-overlay" v-if="showModal">
        <div class="modal-content">
          <i class="fas fa-trophy modal-icon"></i>
          <h2>Chúc mừng!</h2>
          <p>Bạn đã trúng: {{ winningPrize }}</p>
          <button class="close-button" @click="closeModal">Đóng</button>
        </div>
      </div>
    </div>
  </template>
  
    
  <style scoped>
  /* Container Styling */
  .spin-container {
    display: flex;
    justify-content: space-evenly;
    align-items: center;
    padding: 30px;
    background-color: #eef2f7;
    font-family: "Segoe UI", sans-serif;
    min-height: 100vh;
    box-sizing: border-box;
  }
  
  /* Spin Wheel Section */
  .spin-wheel-section {
    display: flex;
    flex-direction: column;
    align-items: center;
    background-color: #ffffff;
    border-radius: 15px;
    box-shadow: 0 8px 20px rgba(0, 0, 0, 0.1);
    padding: 25px;
    width: 50%;
    max-width: 500px;
  }
  
  .spin-wheel {
    position: relative;
    width: 300px;
    height: 300px;
    border-radius: 50%;
    overflow: hidden;
    border: 8px solid #0d47a1;
  }
  
  .wheel {
    position: absolute;
    width: 100%;
    height: 100%;
    border-radius: 50%;
    transition: transform 7s cubic-bezier(0.25, 0.1, 0.25, 1);
  }
  
  .segment {
    position: absolute;
    width: 50%;
    height: 50%;
    background-color: #1976d2;
    color: #ffffff;
    text-align: center;
    line-height: 2;
    font-size: 12px;
    transform-origin: 100% 100%;
    clip-path: polygon(0 0, 100% 0, 100% 100%, 0 100%);
  }
  
  .segment:nth-child(even) {
    background-color: #1565c0;
  }
  
  .segment span {
    position: absolute;
    transform: rotate(45deg);
    font-weight: bold;
    font-size: 12px;
    margin-top: 20px;
  }
  
  .spin-button {
    margin-top: 20px;
    padding: 12px 30px;
    background-color: #0d47a1;
    color: #ffffff;
    border: none;
    border-radius: 25px;
    font-size: 16px;
    font-weight: bold;
    cursor: pointer;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
    transition: background-color 0.3s, transform 0.2s;
  }
  
  .spin-button:hover {
    background-color: #08306b;
    transform: translateY(-3px);
  }
  
  /* Winners List Section */
  .winners-list-section {
    display: flex;
    flex-direction: column;
    gap: 20px;
    max-width: 300px;
  }
  
  .my-wins,
  .all-winners {
    background-color: #ffffff;
    border-radius: 15px;
    box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
    padding: 20px;
  }
  
  h3 {
    font-size: 18px;
    color: #333;
    text-align: center;
    margin-bottom: 10px;
  }
  
  ul {
    list-style: none;
    padding: 0;
    margin: 0;
    text-align: center;
  }
  
  li {
    font-size: 14px;
    color: #555;
    padding: 5px 0;
  }
  
  /* Modal Styling */
  .modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.6);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 1000;
    animation: fadeIn 0.3s;
  }
  
  .modal-content {
    background-color: #ffffff;
    padding: 30px;
    border-radius: 10px;
    box-shadow: 0 8px 30px rgba(0, 0, 0, 0.3);
    text-align: center;
    max-width: 400px;
  }
  
  .modal-icon {
    font-size: 50px;
    color: #ff9800;
    margin-bottom: 15px;
  }
  
  .modal-content h2 {
    font-size: 22px;
    margin-bottom: 10px;
    color: #333;
  }
  
  .modal-content p {
    font-size: 16px;
    margin-bottom: 20px;
    color: #555;
  }
  
  .close-button {
    background-color: #0d47a1;
    color: #ffffff;
    border: none;
    padding: 10px 20px;
    border-radius: 25px;
    font-size: 14px;
    font-weight: bold;
    cursor: pointer;
    transition: background-color 0.3s;
  }
  
  .close-button:hover {
    background-color: #08306b;
  }
  
  @keyframes fadeIn {
    from {
      opacity: 0;
    }
    to {
      opacity: 1;
    }
  }
  </style>
  
    
  <script setup lang="ts">
  import { ref } from 'vue';
  
  const segments = ['Phần thưởng 1', 'Phần thưởng 2', 'Phần thưởng 3', 'Phần thưởng 4', 'Phần thưởng 5', 'Phần thưởng 6'];
  const myWins = ref<string[]>([]);
  const allWinners = ref<string[]>(['Người 1', 'Người 2', 'Người 3']);
  const showModal = ref(false);
  const winningPrize = ref('');
  const wheel = ref<HTMLElement | null>(null);
  
  const spinWheel = () => {
    const randomIndex = Math.floor(Math.random() * segments.length);
    const degree = randomIndex * (360 / segments.length) + 3600; // Add multiple turns
    if (wheel.value) {
      wheel.value.style.transform = `rotate(${degree}deg)`;
    }
  
    setTimeout(() => {
      showModal.value = true;
      winningPrize.value = segments[randomIndex];
      myWins.value.push(winningPrize.value);
      allWinners.value.push(`Người ${allWinners.value.length + 1} - ${winningPrize.value}`);
    }, 7000); // Show result after 7 seconds
  };
  
  const closeModal = () => {
    showModal.value = false;
  };
  </script>
  