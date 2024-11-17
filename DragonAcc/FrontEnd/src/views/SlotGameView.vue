<template>
    <div class="slot-machine">
      <h1>Slot Machine Game</h1>
      <div class="reels">
        <div class="reel" v-for="(reel, index) in reels" :key="index">
          <div
            class="symbols"
            :class="{ spinning: spinningReels[index] }"
            :style="{ animationDuration: spinDurations[index] + 'ms' }"
          >
            <div class="symbol" v-for="symbol in reel" :key="symbol.id">
              <i :class="symbol.icon"></i>
            </div>
          </div>
        </div>
      </div>
      <button @click="spinReels" :disabled="isSpinning">Quay</button>
      <p v-if="result">{{ result }}</p>
  
      <!-- Jackpot Modal -->
      <div v-if="showJackpotModal" class="modal">
        <div class="modal-content">
          <h3>🎉 Jackpot! 🎉</h3>
          <p>Chúc mừng bạn đã trúng jackpot!</p>
          <button @click="closeModal">Đóng</button>
        </div>
      </div>
    </div>
  </template>
  <script>
  export default {
    data() {
      return {
        symbols: [
          { id: 1, icon: 'fas fa-cherry' },
          { id: 2, icon: 'fas fa-lemon' },
          { id: 3, icon: 'fas fa-star' },
          { id: 4, icon: 'fas fa-bell' },
          { id: 5, icon: 'fas fa-diamond' },
          { id: 6, icon: 'fas fa-apple-alt' },
          { id: 7, icon: 'fas fa-anchor' },
          { id: 8, icon: 'fas fa-heart' },
          { id: 9, icon: 'fas fa-crown' },
          { id: 10, icon: 'fas fa-gem' },
        ],
        reels: [[], [], []],
        spinningReels: [false, false, false],
        spinDurations: [2000, 2500, 3000], // Milliseconds cho từng reel
        isSpinning: false,
        result: '',
        showJackpotModal: false,
      };
    },
    mounted() {
      this.resetReels();
    },
    methods: {
      /**
       * Khởi tạo lại các reels với các biểu tượng ngẫu nhiên.
       */
      resetReels() {
        this.reels = this.reels.map(() => {
          // Mỗi reel sẽ chứa 30 biểu tượng để tạo hiệu ứng cuộn mượt mà
          const reelSymbols = [];
          for (let i = 0; i < 30; i++) {
            const randomSymbol = this.symbols[Math.floor(Math.random() * this.symbols.length)];
            reelSymbols.push(randomSymbol);
          }
          return reelSymbols;
        });
      },
      /**
       * Thực hiện quay các reels.
       */
      async spinReels() {
        if (this.isSpinning) return; // Ngăn không cho quay nếu đang quay
        this.isSpinning = true;
        this.result = '';
  
        // Khởi động quay reels
        this.resetReels();
        this.spinningReels = [true, true, true];
  
        // Thời gian quay cho từng reel
        const spinPromises = this.reels.map((reel, index) => {
          return new Promise((resolve) => {
            setTimeout(() => {
              // Dừng reel bằng cách chỉ giữ lại 3 biểu tượng ngẫu nhiên
              const stoppedSymbols = [];
              for (let i = 0; i < 3; i++) {
                const randomSymbol = this.symbols[Math.floor(Math.random() * this.symbols.length)];
                stoppedSymbols.push(randomSymbol);
              }
              this.$set(this.reels, index, stoppedSymbols);
              this.$set(this.spinningReels, index, false);
              resolve();
            }, this.spinDurations[index]);
          });
        });
  
        // Chờ tất cả các reels dừng quay
        await Promise.all(spinPromises);
  
        // Kiểm tra kết quả
        this.checkResult();
  
        this.isSpinning = false;
      },
      /**
       * Kiểm tra kết quả sau khi quay.
       */
      checkResult() {
        // Lấy ID của biểu tượng ở giữa mỗi reel
        const middleSymbols = this.reels.map((reel) => reel[1].id);
  
        // Kiểm tra nếu tất cả các biểu tượng ở giữa đều giống nhau
        const isWin = middleSymbols.every((id) => id === middleSymbols[0]);
  
        if (isWin) {
          // Kiểm tra nếu biểu tượng trúng là một biểu tượng đặc biệt
          const specialIcons = [9, 10]; // Giả sử ID 9 và 10 là biểu tượng đặc biệt
          if (specialIcons.includes(middleSymbols[0])) {
            this.result = `🎉 Jackpot! Bạn đã trúng ${this.getSymbolName(middleSymbols[0])}! 🎉`;
            this.showJackpotModal = true;
            // Thêm logic nhận phần thưởng ở đây (ví dụ: cập nhật số dư, gửi yêu cầu tới backend, v.v.)
          } else {
            this.result = `🎉 Chúc mừng! Bạn đã trúng ${this.getSymbolName(middleSymbols[0])}! 🎉`;
            // Thêm logic nhận phần thưởng ở đây nếu cần
          }
        } else {
          this.result = '😞 Thử lại nhé!';
        }
      },
      /**
       * Lấy tên của biểu tượng dựa trên ID.
       * @param {number} id - ID của biểu tượng.
       * @returns {string} - Tên của biểu tượng.
       */
      getSymbolName(id) {
        const symbol = this.symbols.find((s) => s.id === id);
        return symbol ? symbol.icon.replace('fas fa-', '').toUpperCase() : 'Không rõ';
      },
      /**
       * Đóng modal Jackpot.
       */
      closeModal() {
        this.showJackpotModal = false;
      },
    },
  };
  </script>
 <style scoped>
 .slot-machine {
   text-align: center;
   max-width: 800px;
   margin: 0 auto;
   font-family: 'Arial', sans-serif;
   padding: 20px;
   background-color: #f5f5f5;
   border-radius: 10px;
   box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
 }
 
 h1 {
   margin-bottom: 20px;
   color: #1dd1a1;
   font-size: 36px;
 }
 
 .reels {
   display: flex;
   justify-content: center;
   margin-bottom: 20px;
   position: relative;
   height: 150px; /* 3 biểu tượng x 50px */
   overflow: hidden;
 }
 
 .reel {
   width: 100px;
   height: 150px;
   border: 4px solid #1dd1a1;
   border-radius: 10px;
   margin: 0 10px;
   overflow: hidden;
   position: relative;
   background-color: #fff;
   box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
 }
 
 .symbols {
   position: absolute;
   width: 100%;
   height: 150px; /* 3 biểu tượng x 50px */
   display: flex;
   flex-direction: column;
   transition: transform 0.5s ease-in-out;
 }
 
 .symbol {
   width: 100%;
   height: 50px;
   display: flex;
   align-items: center;
   justify-content: center;
   font-size: 30px;
   color: #e74c3c;
 }
 
 .spinning {
   animation: spin infinite linear;
 }
 
 @keyframes spin {
   0% {
     transform: translateY(0);
   }
   100% {
     transform: translateY(-150px); /* 3 biểu tượng x 50px */
   }
 }
 
 button {
   padding: 15px 30px;
   background-color: #1dd1a1;
   color: #fff;
   border: none;
   border-radius: 50px;
   font-size: 18px;
   cursor: pointer;
   transition: background-color 0.3s ease, transform 0.2s ease;
 }
 
 button:disabled {
   background-color: #a5d6a7;
   cursor: not-allowed;
 }
 
 button:hover:not(:disabled) {
   background-color: #10ac84;
   transform: scale(1.05);
 }
 
 p {
   margin-top: 20px;
   font-size: 20px;
   color: #333;
 }
 
 .modal {
   position: fixed;
   top: 0;
   left: 0;
   width: 100%;
   height: 100%;
   background-color: rgba(0, 0, 0, 0.6);
   display: flex;
   align-items: center;
   justify-content: center;
   z-index: 999;
 }
 
 .modal-content {
   background-color: #fff;
   padding: 30px 50px;
   border-radius: 10px;
   text-align: center;
   box-shadow: 0 4px 20px rgba(0, 0, 0, 0.2);
   animation: fadeIn 0.5s ease-in-out;
 }
 
 .modal-content h3 {
   margin-bottom: 20px;
   color: #1dd1a1;
   font-size: 28px;
 }
 
 .modal-content p {
   font-size: 18px;
   margin-bottom: 30px;
 }
 
 .modal-content button {
   padding: 10px 20px;
   background-color: #1dd1a1;
   color: #fff;
   border: none;
   border-radius: 5px;
   cursor: pointer;
   transition: background-color 0.3s ease;
 }
 
 .modal-content button:hover {
   background-color: #10ac84;
 }
 
 @keyframes fadeIn {
   from {
     opacity: 0;
     transform: scale(0.95);
   }
   to {
     opacity: 1;
     transform: scale(1);
   }
 }
 </style>
 