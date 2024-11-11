<template>
  <div class="scene">
    <div class="game-container">
      <!-- Card Section -->
      <div class="card-section">
        <div class="card-container" ref="cardContainer">
          <div
            v-for="(card, index) in shuffledRewards"
            :key="index"
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
          <!-- Limit display to the first 8 items using slice -->
          <li v-for="(prize, index) in userPrizes.slice(0, 9)" :key="index">
            <span>{{ index + 1 }}. {{ prize.prize }}</span>
            <span>{{ formatDate(prize.createdDate) }}</span>
            <span class="btn-nhan"><button>Nhận</button></span>
          </li>
        </ul>
      </div>
    </div>
  </div>
  <div v-if="showInsufficientCoinsModal" class="insufficient-coins-modal-overlay">
    <div class="insufficient-coins-modal">
      <p>Bạn không đủ xu</p>
      <button @click="closeModal">Đóng</button>
    </div>
  </div>
</template>

<script>
import { luckyWheelApi } from '@/api/luckywheel.api';
import { luckyWheelListPrizeApi } from '@/api/luckywheellistprize.api';
import { userStore } from '@/stores/auth';

export default {
  name: 'CardFlip',
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
    };
  },
  async created() {
    const store = userStore();
    this.userId =
      store.user?.id || JSON.parse(localStorage.getItem('user') || '{}').id;

    await this.fetchRewards();
    await this.fetchUserPrizes();
    this.shuffledRewards = [...this.rewards];
    this.positions = Array.from({ length: 9 }, (_, i) => i);
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
        const result = await luckyWheelListPrizeApi.getByIdLuckyWheel(this.userId);
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
        const result = await luckyWheelApi.spin();
        if (result.isSuccess) {
          this.$set(this.shuffledRewards, index, {
            ...this.shuffledRewards[index],
            prize: result.prize,
            isFlipped: true,
          });
          await this.fetchUserPrizes();
        }
        else if (result.message === "Bạn không đủ xu để quay!") {
          this.showInsufficientCoinsModal = true;
        }  else {
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

      [this.positions[i], this.positions[j]] = [this.positions[j], this.positions[i]];
      this.applySwapAnimation(i, j);
    },
    applySwapAnimation(i, j) {
  const cardElements = this.$refs.cardContainer.querySelectorAll('.card');
  const card1 = cardElements[i];
  const card2 = cardElements[j];

  card1.style.transition = 'transform 0.5s ease-in-out';
  card2.style.transition = 'transform 0.5s ease-in-out';

  const containerRect = this.$refs.cardContainer.getBoundingClientRect();
  const rect1 = card1.getBoundingClientRect();
  const rect2 = card2.getBoundingClientRect();

  const deltaX = rect2.left - rect1.left;
  const deltaY = rect2.top - rect1.top;

  // Adjust deltas to be relative to the container
  const adjustedDeltaX = deltaX;
  const adjustedDeltaY = deltaY;

  card1.style.transform = `translate(${adjustedDeltaX}px, ${adjustedDeltaY}px)`;
  card2.style.transform = `translate(${-adjustedDeltaX}px, ${-adjustedDeltaY}px)`;

  // Ensure the browser applies the transform
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
  display: flex;
  justify-content: center;
  align-items: center;
  height: 90vh;
  gap: 20px;
  transform: scale(0.8); /* Thu nhỏ toàn bộ giao diện */
  transform-origin: center;
}

.game-container {
  display: flex;
  align-items: flex-start;
  gap: 30px;
}

.card-section {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.card-container {
  display: grid;
  grid-template-columns: repeat(3, 90px); /* Giảm kích thước card */
  grid-template-rows: repeat(3, 130px);
  gap: 10px;
  position: relative;
  overflow: hidden;
}

.card {
  width: 100px; /* Giảm kích thước card */
  height: 130px;
  perspective: 1000px;
  cursor: pointer;
}

.card .card-face {
  width: 100%;
  height: 100%;
  border-radius: 10px;
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
  font-size: 12px;
  padding: 8px;
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
  padding: 10px 30px;
  font-size: 16px;
  background-color: #1dd1a1;
  color: #fff;
  border: none;
  border-radius: 50px;
  cursor: pointer;
  transition: background-color 0.3s, transform 0.2s;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.flip-all-button i {
  margin-right: 10px;
}

.flip-all-button:hover {
  background-color: #10ac84;
  transform: translateY(-3px);
}

.prize-section {
  width: 300px;
  padding: 20px;
  background-color: #ffffffcc;
  border-radius: 15px;
  backdrop-filter: blur(10px);
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.prize-section h2 {
  margin-bottom: 15px;
  font-size: 16px;
  text-align: center;
  color: #333;
}

.prize-section h2 i {
  color: #fbc531;
  margin-right: 5px;
}

.prize-list {
  list-style: none;
  padding: 0;
  margin: 0;
}

.prize-list li {
  background-color: #f1f2f6;
  padding: 10px;
  margin-bottom: 8px;
  border-radius: 10px;
  font-size: 10px;
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
  padding: 4px 8px; /* Giảm padding để nút nhỏ hơn */
  font-size: 1px; /* Giảm font-size */
  background-color: #1dd1a1;
  color: #fff;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.btn-nhan button:hover {
  background-color: #10ac84; /* Màu nền khi hover */
}

.insufficient-coins-modal-overlay {
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

.insufficient-coins-modal {
  background-color: #fff;
  padding: 20px;
  border-radius: 10px;
  text-align: center;
}

.insufficient-coins-modal p {
  font-size: 16px;
  margin-bottom: 15px;
}

.insufficient-coins-modal button {
  padding: 8px 16px;
  background-color: #1dd1a1;
  color: #fff;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

.insufficient-coins-modal button:hover {
  background-color: #10ac84;
}

@media screen and (max-width: 768px) {
  .game-container {
    flex-direction: column;
    align-items: center;
  }

  .scene {
    height: auto;
    padding: 50px 0;
  }
}

</style>
