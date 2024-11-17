<template>
  <div class="scene">
    <!-- Falling Gifts Animation -->
    <div class="falling-gifts">
      <i v-for="n in 10" :key="n" class="fas fa-gift gift-icon"></i>
    </div>

    <!-- Fireworks Animation -->
    <div v-if="showFireworks" class="fireworks">
      <div class="firework" v-for="n in 20" :key="n"></div>
    </div>

    <div class="game-container">
      <!-- Card Section -->
      <div class="card-section">
        <div class="card-container" ref="cardContainer">
          <div
            v-for="(card, index) in shuffledRewards"
            :key="card.id"
            class="card"
            :class="{ flipped: card.isFlipped }"
            :style="getCardStyle(index)"
            @click="flipCard(index)"
          >
            <div class="card-face front">
              <i class="fas fa-gift"></i>
            </div>
            <div class="card-face back">
              {{ card.prize }}
            </div>
          </div>
        </div>
        <button class="flip-all-button" @click="flipAllAndShuffle">
          <i class="fas fa-play"></i> Bắt Đầu Trò Chơi
        </button>
      </div>
      <div class="prize-section">
        <h2><i class="fas fa-trophy"></i> Phần Thưởng Của Bạn</h2>
        <ul class="prize-list">
          <li v-for="(prize, index) in userPrizes.slice(0, 9)" :key="index">
            <span>{{ index + 1 }}. {{ prize.prize }}</span>
            <span>{{ formatDate(prize.createdDate) }}</span>
            <span class="btn-nhan">
              <button @click="showContactAdminModal">Nhận</button>
            </span>
          </li>
        </ul>
      </div>
    </div>
    <!-- Modal thông báo liên hệ Admin -->
    <div v-if="showContactAdmin" class="modal-overlay">
      <div class="modal-content">
        <i class="fa fa-smile-wink"></i>
        <p>Chúc bạn may mắn lần sau</p>
        <button @click="closeContactAdminModal">Đóng</button>
      </div>
    </div>
    <!-- Modal thông báo không đủ xu -->
    <div v-if="showInsufficientCoinsModal" class="modal-overlay">
      <div class="modal-content">
        <p>Bạn không đủ xu</p>
        <button @click="closeModal">Đóng</button>
      </div>
    </div>
  </div>
</template>

<script>
import { luckyWheelApi } from '@/api/luckywheel.api';
import { luckyWheelListPrizeApi } from '@/api/luckywheellistprize.api';
import { userStore } from '@/stores/auth';

export default {
  name: 'LuckyWheelView',
  data() {
    return {
      rewards: [],
      shuffledRewards: [],
      positions: [],
      isShuffling: false,
      gameStarted: false,
      canFlip: true,
      userPrizes: [],
      userId: null,
      showInsufficientCoinsModal: false,
      showContactAdmin: false,
      showFireworks: false, // Biến để kiểm soát hiển thị pháo hoa
    };
  },
  async created() {
    const store = userStore();
    this.userId =
      store.user?.id || JSON.parse(localStorage.getItem('user') || '{}').id;

    await this.fetchRewards();
    await this.fetchUserPrizes();
    this.shuffledRewards = [...this.rewards];
    this.positions = Array.from({ length: this.rewards.length }, (_, i) => i);
  },
  methods: {
    async fetchRewards() {
      try {
        const result = await luckyWheelApi.getAllLuckyWheel();
        if (result.isSuccess) {
          this.rewards = result.data.map((reward) => ({
            ...reward,
            isFlipped: false,
          }));
        } else {
          console.error(result.message);
        }
      } catch (error) {
        console.error('Error fetching rewards:', error);
      }
    },
    async fetchUserPrizes() {
      try {
        const result = await luckyWheelListPrizeApi.getByIdLuckyWheel(
          this.userId
        );
        if (result.isSuccess) {
          this.userPrizes = result.data
            .sort((a, b) => new Date(b.createdDate) - new Date(a.createdDate))
            .slice(0, 10);
        } else {
          console.error(result.message);
        }
      } catch (error) {
        console.error('Error fetching user prizes:', error);
      }
    },
    flipCard(index) {
      if (
        this.gameStarted &&
        !this.isShuffling &&
        !this.shuffledRewards[index].isFlipped &&
        this.canFlip
      ) {
        this.shuffledRewards[index].isFlipped = true;
        this.canFlip = false;
        this.spinWheel(index);
      }
    },
    async spinWheel(index) {
      try {
        const prizeId = this.shuffledRewards[index].id;
        const result = await luckyWheelApi.spin(prizeId);
        if (result.isSuccess) {
          this.$set(this.shuffledRewards, index, {
            ...this.shuffledRewards[index],
            prize: result.data.prize,
            isFlipped: true,
          });
          await this.fetchUserPrizes();

          // Kiểm tra nếu trúng 100000RP để kích hoạt pháo hoa
          if (result.data.prize === '100000RP') {
            this.triggerFireworks();
          }
        } else if (result.message === 'Bạn không đủ xu để quay!') {
          this.showInsufficientCoinsModal = true;
        } else {
          console.error(result.message);
        }
      } catch (error) {
        console.error('Error spinning the wheel:', error);
      } finally {
        this.canFlip = true;
      }
    },
    flipAllAndShuffle() {
      if (this.isShuffling) return;

      this.gameStarted = true;
      this.canFlip = false;
      this.shuffledRewards = this.shuffledRewards.map((card) => ({
        ...card,
        isFlipped: true,
      }));

      setTimeout(() => {
        this.shuffledRewards = this.shuffledRewards.map((card) => ({
          ...card,
          isFlipped: false,
        }));

        this.isShuffling = true;
        const shuffleInterval = setInterval(() => {
          this.shufflePositions();
        }, 500);

        setTimeout(() => {
          clearInterval(shuffleInterval);
          this.isShuffling = false;
          this.canFlip = true;
        }, 5000);
      }, 3000);
    },
    shufflePositions() {
      const i = Math.floor(Math.random() * this.positions.length);
      let j = Math.floor(Math.random() * this.positions.length);
      while (j === i) {
        j = Math.floor(Math.random() * this.positions.length);
      }

      [this.positions[i], this.positions[j]] = [
        this.positions[j],
        this.positions[i],
      ];
      this.applySwapAnimation(i, j);
    },
    applySwapAnimation(i, j) {
      const cardElements = this.$refs.cardContainer.querySelectorAll('.card');
      const card1 = cardElements[i];
      const card2 = cardElements[j];

      card1.style.transition = 'transform 0.5s ease-in-out';
      card2.style.transition = 'transform 0.5s ease-in-out';

      const rect1 = card1.getBoundingClientRect();
      const rect2 = card2.getBoundingClientRect();

      const deltaX = rect2.left - rect1.left;
      const deltaY = rect2.top - rect1.top;

      card1.style.transform = `translate(${deltaX}px, ${deltaY}px)`;
      card2.style.transform = `translate(${-deltaX}px, ${-deltaY}px)`;

      requestAnimationFrame(() => {
        setTimeout(() => {
          card1.style.transform = '';
          card2.style.transform = '';
          [this.shuffledRewards[i], this.shuffledRewards[j]] = [
            this.shuffledRewards[j],
            this.shuffledRewards[i],
          ];
        }, 500);
      });
    },
    getCardStyle(index) {
      const pos = this.positions[index];
      const row = Math.floor(pos / 3) + 1;
      const col = (pos % 3) + 1;
      return {
        gridRow: row,
        gridColumn: col,
        position: 'relative',
        zIndex: 1,
      };
    },
    formatDate(dateString) {
      const options = { day: 'numeric', month: 'numeric', year: 'numeric' };
      return new Date(dateString).toLocaleDateString('vi-VN', options);
    },
    closeModal() {
      this.showInsufficientCoinsModal = false;
    },
    showContactAdminModal() {
      this.showContactAdmin = true;
    },
    closeContactAdminModal() {
      this.showContactAdmin = false;
    },
    // Phương thức kích hoạt pháo hoa
    triggerFireworks() {
      this.showFireworks = true;
      setTimeout(() => {
        this.showFireworks = false;
      }, 3000); // Hiển thị pháo hoa trong 3 giây
    },
  },
};
</script>

<style scoped>
@import 'https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css';
@import 'https://fonts.googleapis.com/css2?family=Poppins:wght@400;600&display=swap';

* {
  box-sizing: border-box;
}

body {
  font-family: 'Poppins', sans-serif;
  background: linear-gradient(135deg, #f5f7fa, #c3cfe2);
  margin: 0;
  padding: 0;
}

.scene {
  position: relative; /* Để chứa các hiệu ứng absolute */
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  gap: 30px;
  transform-origin: center;
  overflow: hidden; /* Ẩn các hiệu ứng rơi ra ngoài */
}

.game-container {
  display: flex;
  align-items: flex-start;
  gap: 40px;
}

.card-section {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.card-container {
  display: grid;
  grid-template-columns: repeat(3, 120px);
  grid-template-rows: repeat(3, 160px);
  gap: 10px;
  position: relative;
  overflow: hidden;
}

.card {
  width: 120px;
  height: 160px;
  perspective: 1000px;
  cursor: pointer;
}

.card .card-face {
  width: 100%;
  height: 100%;
  border-radius: 15px;
  background-color: #fff;
  position: absolute;
  backface-visibility: hidden;
  transform-style: preserve-3d;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  transition: transform 0.6s;
  display: flex;
  align-items: center;
  justify-content: center;
}

.card .front {
  background: linear-gradient(45deg, #ff6b6b, #f94d6a);
  color: #fff;
  font-size: 30px;
}

.card .back {
  background: #fff;
  color: #333;
  transform: rotateY(180deg);
  font-size: 14px;
  padding: 12px;
  text-align: center;
}

.card.flipped .front {
  transform: rotateY(180deg);
}

.card.flipped .back {
  transform: rotateY(360deg);
}

.flip-all-button {
  margin-top: 20px;
  padding: 12px 30px;
  font-size: 16px;
  background-color: #1dd1a1;
  color: #fff;
  border: none;
  border-radius: 40px;
  cursor: pointer;
  transition: background-color 0.3s, transform 0.2s;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.flip-all-button i {
  margin-right: 10px;
  font-size: 18px;
}

.flip-all-button:hover {
  background-color: #10ac84;
  transform: translateY(-3px);
}

.prize-section {
  width: 350px;
  padding: 25px;
  background-color: #ffffffcc;
  border-radius: 15px;
  backdrop-filter: blur(10px);
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.prize-section h2 {
  margin-bottom: 20px;
  font-size: 18px;
  text-align: center;
  color: #333;
}

.prize-section h2 i {
  color: #fbc531;
  margin-right: 8px;
  font-size: 20px;
}

.prize-list {
  list-style: none;
  padding: 0;
  margin: 0;
}

.prize-list li {
  background-color: #f1f2f6;
  padding: 12px;
  margin-bottom: 10px;
  border-radius: 10px;
  font-size: 12px;
  color: #555;
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.prize-list li:last-child {
  margin-bottom: 0;
}

.prize-list li span:first-child {
  font-weight: 600;
}

.prize-list li span:last-child {
  font-size: 14px;
  color: #999;
}

.btn-nhan button {
  margin-left: 10px;
  padding: 6px 12px;
  font-size: 12px;
  background-color: #1dd1a1;
  color: #fff;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.btn-nhan button:hover {
  background-color: #10ac84;
}

/* Modal Styles */
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
  padding: 30px;
  border-radius: 15px;
  text-align: center;
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
  max-width: 80%;
}

.modal-content i {
  font-size: 90px;
  color: #fbc531;
}

.modal-content p {
  font-size: 18px;
  margin-bottom: 20px;
  color: #333;
}

.modal-content button {
  padding: 10px 20px;
  background-color: #1dd1a1;
  color: #fff;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-size: 16px;
  transition: background-color 0.3s;
}

.modal-content button:hover {
  background-color: #10ac84;
}

/* Falling Gifts Styles */
.falling-gifts {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  pointer-events: none; /* Không can thiệp vào các tương tác khác */
  overflow: hidden;
  z-index: 1;
}

.gift-icon {
  position: absolute;
  top: -50px;
  font-size: 24px;
  color: #ff6b6b;
  animation: fall 5s infinite;
}

@keyframes fall {
  0% {
    transform: translateY(0) rotate(0deg);
    opacity: 1;
  }
  100% {
    transform: translateY(110vh) rotate(360deg);
    opacity: 0;
  }
}

/* Tạo các vị trí ngẫu nhiên cho quà */
.falling-gifts .fa-gift:nth-child(1) {
  left: 10%;
  animation-delay: 0s;
}
.falling-gifts .fa-gift:nth-child(2) {
  left: 20%;
  animation-delay: 1s;
}
.falling-gifts .fa-gift:nth-child(3) {
  left: 30%;
  animation-delay: 2s;
}
.falling-gifts .fa-gift:nth-child(4) {
  left: 40%;
  animation-delay: 3s;
}
.falling-gifts .fa-gift:nth-child(5) {
  left: 50%;
  animation-delay: 4s;
}
.falling-gifts .fa-gift:nth-child(6) {
  left: 60%;
  animation-delay: 0.5s;
}
.falling-gifts .fa-gift:nth-child(7) {
  left: 70%;
  animation-delay: 1.5s;
}
.falling-gifts .fa-gift:nth-child(8) {
  left: 80%;
  animation-delay: 2.5s;
}
.falling-gifts .fa-gift:nth-child(9) {
  left: 90%;
  animation-delay: 3.5s;
}
.falling-gifts .fa-gift:nth-child(10) {
  left: 5%;
  animation-delay: 4.5s;
}

/* Fireworks Styles */
.fireworks {
  position: absolute;
  top: 50%;
  left: 50%;
  width: 200px;
  height: 200px;
  transform: translate(-50%, -50%);
  pointer-events: none;
  z-index: 2;
}

.firework {
  position: absolute;
  width: 4px;
  height: 100px;
  background: radial-gradient(circle, #fbc531, #ff793f);
  top: 100%;
  left: 50%;
  transform: translateX(-50%) rotate(0deg);
  animation: shoot 1s forwards;
}

.firework:nth-child(1) {
  transform: translateX(-50%) rotate(0deg);
  animation-delay: 0s;
}
.firework:nth-child(2) {
  transform: translateX(-50%) rotate(45deg);
  animation-delay: 0.1s;
}
.firework:nth-child(3) {
  transform: translateX(-50%) rotate(90deg);
  animation-delay: 0.2s;
}
.firework:nth-child(4) {
  transform: translateX(-50%) rotate(135deg);
  animation-delay: 0.3s;
}
.firework:nth-child(5) {
  transform: translateX(-50%) rotate(180deg);
  animation-delay: 0.4s;
}
.firework:nth-child(6) {
  transform: translateX(-50%) rotate(225deg);
  animation-delay: 0.5s;
}
.firework:nth-child(7) {
  transform: translateX(-50%) rotate(270deg);
  animation-delay: 0.6s;
}
.firework:nth-child(8) {
  transform: translateX(-50%) rotate(315deg);
  animation-delay: 0.7s;
}
.firework:nth-child(9) {
  transform: translateX(-50%) rotate(30deg);
  animation-delay: 0.8s;
}
.firework:nth-child(10) {
  transform: translateX(-50%) rotate(60deg);
  animation-delay: 0.9s;
}
.firework:nth-child(11) {
  transform: translateX(-50%) rotate(120deg);
  animation-delay: 1s;
}
.firework:nth-child(12) {
  transform: translateX(-50%) rotate(150deg);
  animation-delay: 1.1s;
}
.firework:nth-child(13) {
  transform: translateX(-50%) rotate(210deg);
  animation-delay: 1.2s;
}
.firework:nth-child(14) {
  transform: translateX(-50%) rotate(240deg);
  animation-delay: 1.3s;
}
.firework:nth-child(15) {
  transform: translateX(-50%) rotate(300deg);
  animation-delay: 1.4s;
}
.firework:nth-child(16) {
  transform: translateX(-50%) rotate(330deg);
  animation-delay: 1.5s;
}
.firework:nth-child(17) {
  transform: translateX(-50%) rotate(15deg);
  animation-delay: 1.6s;
}
.firework:nth-child(18) {
  transform: translateX(-50%) rotate(75deg);
  animation-delay: 1.7s;
}
.firework:nth-child(19) {
  transform: translateX(-50%) rotate(135deg);
  animation-delay: 1.8s;
}
.firework:nth-child(20) {
  transform: translateX(-50%) rotate(195deg);
  animation-delay: 1.9s;
}

@keyframes shoot {
  0% {
    height: 100px;
    opacity: 1;
  }
  100% {
    height: 0;
    opacity: 0;
  }
}

/* Additional Styles for Fireworks Effect */
.fireworks::before,
.fireworks::after {
  content: '';
  position: absolute;
  top: 50%;
  left: 50%;
  width: 200px;
  height: 200px;
  border-radius: 50%;
  transform: translate(-50%, -50%);
  background: radial-gradient(circle, rgba(255,255,255,0.8) 0%, rgba(255,255,255,0) 70%);
  animation: explode 1.5s forwards;
}

@keyframes explode {
  from {
    transform: translate(-50%, -50%) scale(0.1);
    opacity: 1;
  }
  to {
    transform: translate(-50%, -50%) scale(1);
    opacity: 0;
  }
}

/* Prize Section Styles */
.prize-section {
  width: 350px;
  padding: 25px;
  background-color: #ffffffcc;
  border-radius: 15px;
  backdrop-filter: blur(10px);
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.prize-section h2 {
  margin-bottom: 20px;
  font-size: 18px;
  text-align: center;
  color: #333;
}

.prize-section h2 i {
  color: #fbc531;
  margin-right: 8px;
  font-size: 20px;
}

.prize-list {
  list-style: none;
  padding: 0;
  margin: 0;
}

.prize-list li {
  background-color: #f1f2f6;
  padding: 12px;
  margin-bottom: 10px;
  border-radius: 10px;
  font-size: 12px;
  color: #555;
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.prize-list li:last-child {
  margin-bottom: 0;
}

.prize-list li span:first-child {
  font-weight: 600;
}

.prize-list li span:last-child {
  font-size: 14px;
  color: #999;
}

.btn-nhan button {
  margin-left: 10px;
  padding: 6px 12px;
  font-size: 12px;
  background-color: #1dd1a1;
  color: #fff;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.btn-nhan button:hover {
  background-color: #10ac84;
}

/* Responsive Styles */
@media screen and (max-width: 768px) {
  .game-container {
    flex-direction: column;
    align-items: center;
    gap: 20px;
  }

  .scene {
    height: auto;
    padding: 50px 0;
  }

  .card-container {
    grid-template-columns: repeat(3, 100px);
    grid-template-rows: repeat(3, 140px);
    gap: 10px;
  }

  .card {
    width: 100px;
    height: 140px;
  }

  .card .front {
    font-size: 24px;
  }

  .card .back {
    font-size: 12px;
    padding: 8px;
  }

  .flip-all-button {
    padding: 10px 25px;
    font-size: 14px;
  }

  .flip-all-button i {
    margin-right: 8px;
    font-size: 16px;
  }

  .prize-section {
    width: 90%;
    padding: 20px;
  }

  .prize-section h2 {
    font-size: 16px;
  }

  .prize-section h2 i {
    font-size: 18px;
    margin-right: 8px;
  }

  .prize-list li {
    padding: 10px;
    font-size: 12px;
  }

  .prize-list li span:last-child {
    font-size: 12px;
  }

  .btn-nhan button {
    padding: 6px 12px;
    font-size: 12px;
  }

  .modal-content {
    padding: 20px;
    border-radius: 10px;
  }

  .modal-content p {
    font-size: 16px;
    margin-bottom: 15px;
  }

  .modal-content button {
    padding: 8px 16px;
    font-size: 14px;
  }
}

/* Additional Styles */
.falling-gifts .fa-gift {
  animation: fall 5s infinite;
}

/* Fireworks Additional Styles */
.fireworks {
  position: absolute;
  top: 50%;
  left: 50%;
  width: 200px;
  height: 200px;
  transform: translate(-50%, -50%);
  pointer-events: none;
  z-index: 2;
}

.firework {
  position: absolute;
  width: 4px;
  height: 100px;
  background: radial-gradient(circle, #fbc531, #ff793f);
  top: 100%;
  left: 50%;
  transform: translateX(-50%) rotate(0deg);
  animation: shoot 1s forwards;
}

.firework:nth-child(1) {
  transform: translateX(-50%) rotate(0deg);
  animation-delay: 0s;
}
.firework:nth-child(2) {
  transform: translateX(-50%) rotate(45deg);
  animation-delay: 0.1s;
}
.firework:nth-child(3) {
  transform: translateX(-50%) rotate(90deg);
  animation-delay: 0.2s;
}
.firework:nth-child(4) {
  transform: translateX(-50%) rotate(135deg);
  animation-delay: 0.3s;
}
.firework:nth-child(5) {
  transform: translateX(-50%) rotate(180deg);
  animation-delay: 0.4s;
}
.firework:nth-child(6) {
  transform: translateX(-50%) rotate(225deg);
  animation-delay: 0.5s;
}
.firework:nth-child(7) {
  transform: translateX(-50%) rotate(270deg);
  animation-delay: 0.6s;
}
.firework:nth-child(8) {
  transform: translateX(-50%) rotate(315deg);
  animation-delay: 0.7s;
}
.firework:nth-child(9) {
  transform: translateX(-50%) rotate(30deg);
  animation-delay: 0.8s;
}
.firework:nth-child(10) {
  transform: translateX(-50%) rotate(60deg);
  animation-delay: 0.9s;
}
.firework:nth-child(11) {
  transform: translateX(-50%) rotate(120deg);
  animation-delay: 1s;
}
.firework:nth-child(12) {
  transform: translateX(-50%) rotate(150deg);
  animation-delay: 1.1s;
}
.firework:nth-child(13) {
  transform: translateX(-50%) rotate(210deg);
  animation-delay: 1.2s;
}
.firework:nth-child(14) {
  transform: translateX(-50%) rotate(240deg);
  animation-delay: 1.3s;
}
.firework:nth-child(15) {
  transform: translateX(-50%) rotate(300deg);
  animation-delay: 1.4s;
}
.firework:nth-child(16) {
  transform: translateX(-50%) rotate(330deg);
  animation-delay: 1.5s;
}
.firework:nth-child(17) {
  transform: translateX(-50%) rotate(15deg);
  animation-delay: 1.6s;
}
.firework:nth-child(18) {
  transform: translateX(-50%) rotate(75deg);
  animation-delay: 1.7s;
}
.firework:nth-child(19) {
  transform: translateX(-50%) rotate(135deg);
  animation-delay: 1.8s;
}
.firework:nth-child(20) {
  transform: translateX(-50%) rotate(195deg);
  animation-delay: 1.9s;
}

@keyframes shoot {
  0% {
    height: 100px;
    opacity: 1;
  }
  100% {
    height: 0;
    opacity: 0;
  }
}

.fireworks::before,
.fireworks::after {
  content: '';
  position: absolute;
  top: 50%;
  left: 50%;
  width: 200px;
  height: 200px;
  border-radius: 50%;
  transform: translate(-50%, -50%);
  background: radial-gradient(circle, rgba(255,255,255,0.8) 0%, rgba(255,255,255,0) 70%);
  animation: explode 1.5s forwards;
}

@keyframes explode {
  from {
    transform: translate(-50%, -50%) scale(0.1);
    opacity: 1;
  }
  to {
    transform: translate(-50%, -50%) scale(1);
    opacity: 0;
  }
}

/* Additional Styles for Fireworks Effect */
.fireworks {
  position: absolute;
  top: 50%;
  left: 50%;
  width: 200px;
  height: 200px;
  transform: translate(-50%, -50%);
  pointer-events: none;
  z-index: 2;
}
</style>
