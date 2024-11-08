<template>
  <div class="lucky-wheel-container">
    <!-- Rules Section -->
    <div class="rule">
      <div
        v-for="(prize, index) in prizes"
        :key="index"
        class="rule__content"
      >
        <div :class="['rule__color', `color-${(index % 9) + 1}`]"></div>
        <div class="rule__text">{{ prize.prize }}</div>
      </div>
    </div>

    <!-- Wheel Section -->
    <div class="wheel">
      <div class="wheel__inner" ref="wheel">
        <div
          v-for="(prize, index) in prizes"
          :key="index"
          class="wheel__sec"
          :style="getWheelSectionStyle(index)"
        ></div>
      </div>
      <div class="wheel__arrow">
        <button
          class="wheel__button"
          @click="spinWheel"
          :disabled="isSpinning"
          aria-label="Spin the lucky wheel"
        >
          Spin
        </button>
      </div>
      <div v-if="isSpinning" class="loading-indicator">
        Spinning...
      </div>
    </div>
    <p>Tốn 1 xu</p>
    <!-- Popup Section -->
    <div class="popup" :class="{ active: showPopup }">
      <div class="popup__container">
        <div class="popup__emotion">
          <i class="fas fa-frown"></i>
        </div>
        <p class="popup__note">{{ popupMessage }}</p>
      </div>
    </div>
    
    <!-- Congratulation Modal -->
    <div
      class="congratulation"
      v-if="showCongratulation"
      @click.self="closeCongratulation"
    >
      <div class="congratulation__container">
        <div class="congratulation__close" @click="closeCongratulation">
          <i class="fas fa-times"></i>
        </div>
        <div class="congratulation__emotion">
          <i class="fas fa-smile"></i>
        </div>
        <p class="congratulation__note">{{ congratulationMessage }}</p>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { ref, onMounted } from 'vue';
import gsap from 'gsap';
import { luckyWheelApi } from '@/api/luckywheel.api';
import { luckyWheelListPrizeApi } from '@/api/luckywheellistprize.api';
import { userStore } from '@/stores/auth';

interface ListPrize {
  id: number;
  prize: string;
}

export default {
  name: 'LuckyWheelView',
  setup() {
    const prizes = ref<ListPrize[]>([]);
    const totalRotation = ref(0);
    const countClicked = ref(0);
    const clicked = ref(false);
    const popupMessage = ref('');
    const showPopup = ref(false);   
    const congratulationMessage = ref('');
    const showCongratulation = ref(false);
    const wheel = ref<HTMLDivElement | null>(null);
    const isSpinning = ref(false); // Indicates if the wheel is spinning

    const store = userStore();
    const userId = store.user?.id || JSON.parse(localStorage.getItem('user') || '{}').id;

    const fetchUserProfile = async () => {
      try {
        if (userId) {
          const response = await luckyWheelListPrizeApi.getByIdLuckyWheel(userId);
          if (response && response.data && response.data.result.isSuccess) {
            const userData = response.data.result.data;
            // Utilize userData if needed
          } else {
            console.error('Failed to fetch user profile:', response?.data?.result?.message);
          }
        } else {
          console.error('User ID not found.');
        }
      } catch (error) {
        console.error('Error fetching user profile:', error);
      }
    };

    onMounted(async () => {
      await fetchUserProfile();

      // Check if wheel ref is available
      if (wheel.value) {
        console.log('Wheel element:', wheel.value);
      } else {
        console.error('Cannot find wheel element.');
      }

      try {
        const response = await luckyWheelApi.getAllLuckyWheel();
        if (response.isSuccess) {
          prizes.value = response.data;

          // Ensure we have 9 prizes
          if (prizes.value.length < 9) {
            const missing = 9 - prizes.value.length;
            for (let i = 0; i < missing; i++) {
              prizes.value.push({
                id: -1, // Placeholder ID
                prize: 'BETTER LUCK NEXT TIME',
              });
            }
          } else if (prizes.value.length > 9) {
            prizes.value = prizes.value.slice(0, 9);
          }
        } else {
          alert(response.message || 'Error fetching prizes.');
        }
      } catch (error) {
        console.error(error);
        alert('Error fetching prizes.');
      }
    });

    // Spin Wheel Handler
    const spinWheel = async (): Promise<void> => {
      if (isSpinning.value) {
        console.log('Wheel is already spinning. Please wait.');
        return;
      }

      if (clicked.value) {
        countClicked.value++;
        console.log(`Click count: ${countClicked.value}`);
        if (countClicked.value <= 2) {
          popupMessage.value = 'STOP PLAYING AROUND!';
        } else if (countClicked.value <= 4) {
          popupMessage.value = 'TOO STUBBORN!';
        } else {
          popupMessage.value = 'TRY TO PRESS THE BUTTON PROPERLY!';
        }
        showPopup.value = true;
        console.log('Showing popup:', popupMessage.value);
        return;
      }

      clicked.value = true;
      isSpinning.value = true;
      console.log('Starting to spin.');

      try {
        const result = await luckyWheelApi.spin();
        console.log('API result:', result);

        if (result.isSuccess) {
          const prize = result.data.prize || result.data.Prize;
          console.log('Prize won:', prize);

          const prizeIndex = prizes.value.findIndex(p => p.prize === prize);
          console.log('Prize index:', prizeIndex);

          if (prizeIndex === -1) {
            throw new Error('Prize not found in the list.');
          }

          const segmentAngle = 360 / prizes.value.length;
          const prizeRotation = (360 - (segmentAngle * prizeIndex) - (segmentAngle / 2));
          const spins = Math.floor(Math.random() * 3) + 4; // Random spins between 4 and 6
          const totalRotationDegrees = spins * 360 + prizeRotation;

          if (wheel.value) {
            console.log('Starting GSAP animation.');

            const tl = gsap.timeline({
              onComplete: () => {
                console.log('Spin animation completed.');
                congratulationMessage.value = `CONGRATULATIONS! YOU WON ${prize.toUpperCase()}`;
                showCongratulation.value = true;

                isSpinning.value = false;
                clicked.value = false;
                countClicked.value = 0;
                totalRotation.value = (totalRotation.value + totalRotationDegrees) % 360; 
              },
            });

            tl.to(wheel.value, {
              rotation: totalRotation.value + totalRotationDegrees,
              duration: 4,
              ease: "power4.out",
            });
          }
        } else {
          console.log('API response was unsuccessful:', result.message);
          popupMessage.value = result.message || 'Spin failed.';
          showPopup.value = true;
          isSpinning.value = false;
          clicked.value = false;
          countClicked.value = 0;
        }
      } catch (error) {
        console.error('Error spinning the wheel:', error);
        popupMessage.value = 'An error occurred during the spin. Please try again.';
        showPopup.value = true;
        isSpinning.value = false;
        clicked.value = false;
        countClicked.value = 0;
      }
    };

    const closeCongratulation = (): void => {
      showCongratulation.value = false;
    };

    const getWheelSectionStyle = (index: number) => {
      const angle = (360 / prizes.value.length) * index;
      const backgroundColor = getColorByIndex(index);
      return {
        transform: `rotate(${angle}deg)`,
        borderTopColor: backgroundColor,
      };
    };

    const getColorByIndex = (index: number) => {
      const colors = [
        '#16a085',
        '#2980b9',
        '#34495e',
        '#f39c12',
        '#d35400',
        '#c0392b',
        '#8e44ad',
        '#27ae60',
        '#e74c3c',
      ];
      return colors[index % colors.length];
    };

    return {
      prizes,
      spinWheel,
      showPopup,
      popupMessage,
      showCongratulation,
      congratulationMessage,
      closeCongratulation,
      getWheelSectionStyle,
      wheel,
      isSpinning,
    };
  },
};
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css?family=Open+Sans:600,300');

*, *::after, *::before {
  outline: none;
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

html, body {
  width: 100%;
  height: 100%;
}

body {
  background-color: #f3f4f6; /* Light gray background similar to LinkedIn */
  font-family: 'Open Sans', sans-serif;
  overflow: hidden;
}

.lucky-wheel-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
}

/* Adjusted the layout to align prizes and wheel */
.rule {
  margin-right: 50px;
  max-height: 80vh;
  overflow-y: auto;
}

.rule__content {
  display: flex;
  align-items: center;
  margin-bottom: 10px;
}

.rule__color {
  width: 50px;
  height: 50px;
  margin-right: 20px;
}

.color-1 { background-color: #16a085; }
.color-2 { background-color: #2980b9; }
.color-3 { background-color: #34495e; }
.color-4 { background-color: #f39c12; }
.color-5 { background-color: #d35400; }
.color-6 { background-color: #c0392b; }
.color-7 { background-color: #8e44ad; }
.color-8 { background-color: #27ae60; }
.color-9 { background-color: #e74c3c; }

.rule__text {
  font-size: 16px;
  font-weight: bold;
  word-wrap: break-word;
  max-width: 200px;
  color: #2c3e50; /* Darker text color */
}

.wheel {
  width: 312px;
  height: 312px;
  border-radius: 50%;
  border: solid 6px #fff;
  box-shadow: 0 4px 9px 0 rgba(0, 0, 0, 0.1);
  position: relative;
}

.wheel__inner {
  width: 300px;
  height: 300px;
  border-radius: 50%;
  position: relative;
  overflow: hidden;
}

.wheel__sec {
  position: absolute;
  top: 0;
  left: 62px;
  width: 0;
  height: 0;
  border-style: solid;
  border-width: 150px 88px 0;
  border-color: transparent;
  transform-origin: 50% 100%;
}

.wheel__arrow {
  width: 70px;
  height: 70px;
  background: #fff;
  position: absolute;
  top: 121px;
  left: 115px;
  border-radius: 50%;
  display: flex;
  justify-content: center;
  align-items: center;
  font-family: 'Open Sans', sans-serif;
}

.wheel__arrow::after {
  content: '';
  position: absolute;
  width: 0;
  height: 0;
  border-style: solid;
  border-width: 0 10px 20px;
  border-color: transparent;
  border-bottom-color: #fff;
  top: -15px;
  left: 25px;
}

.wheel__button {
  width: 60px;
  height: 60px;
  background-color: #0073b1; /* LinkedIn primary color */
  border: none;
  border-radius: 50%;
  cursor: pointer;
  transition: 0.3s;
  font-weight: 600;
  font-size: 18px;
  color: white; /* White text */
}

.wheel__button:hover {
  background-color: #005582; /* Darker shade on hover */
}

.wheel__button:active {
  border: solid 3px rgba(0, 0, 0, 0.1);
  font-size: 15px;
}

.wheel__button:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

.popup {
  position: fixed;
  width: 30vw;
  top: 0;
  left: 50%;
  transform: translate(-50%, -110%);
  background: #fff;
  box-shadow: 0 4px 9px 0 rgba(0, 0, 0, 0.1);
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 30px 20px;
  transition: all 0.5s;
  opacity: 0;
}

.popup.active {
  transform: translate(-50%, 0);
  opacity: 1;
}

.popup__emotion {
  color: #f39c12;
  text-align: center;
  font-size: 30px;
  margin: 0 0 25px 0;
}

.popup__note {
  text-align: center;
}

.congratulation {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background: rgba(0, 0, 0, 0.2);
  display: flex;
  justify-content: center;
  align-items: center;
}

.congratulation__container {
  width: 40vw;
  padding: 30px;
  background-color: #fff;
  position: relative;
  border-radius: 10px;
  box-shadow: 0 4px 9px 0 rgba(0, 0, 0, 0.1);
}

.congratulation__close {
  position: absolute;
  top: 10px;
  right: 10px;
  color: #c0392b;
  cursor: pointer;
  transition: all 0.5s;
}

.congratulation__close:hover {
  transform: rotate(360deg);
}

.congratulation__emotion {
  color: #f39c12;
  text-align: center;
  margin: 0 0 20px 0;
  font-size: 40px;
}

.congratulation__note {
  text-align: center;
  font-size: 18px;
}

.loading-indicator {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  font-size: 18px;
  color: #333;
}
</style>
