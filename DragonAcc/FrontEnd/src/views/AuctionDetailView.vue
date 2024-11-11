<template>
  <div class="auction-container container mt-4 mb-5">
    <div class="row">
      <!-- User Information Column -->
      <div class="col-md-3">
        <div class="card shadow-lg mb-4">
          <div class="card-body text-center">
            <h5 class="card-title">Tài khoản của bạn</h5>
            <p class="card-text"><strong>Tên đăng nhập:</strong> {{ user.username }}</p>
            <p class="card-text"><strong>Số dư:</strong> {{ user.balance }} VNĐ</p>
          </div>
        </div>
      </div>
      <!-- Auction Information Column -->
      <div class="col-md-6" v-if="auction">
        <div class="card shadow-lg text-center">
          <div class="card-header text-white">
            <h5 class="card-title mt-2">{{ auction.auctionName }}</h5>
          </div>
          <div class="image-container">
            <img 
              :src="getFullImageUrl(auction.image)" 
              class="card-img-top" 
              alt="Hình ảnh đấu giá" 
            />
          </div>
          <div class="card-body">
            <p class="card-text">{{ auction.prize }}</p>
            <div class="current-price-box">
              <p class="card-text"><strong>Giá hiện tại:</strong> {{ auction.currentPrice }} VNĐ</p>
            </div>
            <!-- Countdown Timer -->
            <p class="card-text text-danger"><strong>Thời gian còn lại:</strong> {{ timeRemaining }}</p>
          </div>
        </div>
        <!-- Place Bid Section -->
        <div class="card shadow-lg mt-4">
          <div class="card-body">
            <h5 class="card-title">Đặt giá của bạn</h5>
            <form @submit.prevent="placeBid">
              <div class="mb-3">
                <label for="bidAmount" class="form-label">Số tiền đặt giá (VNĐ)</label>
                <input type="number" v-model="bidAmount" id="bidAmount" class="form-control" required>
              </div>
              <button type="submit" class="btn btn-primary w-100">Đặt giá</button>
            </form>
          </div>
        </div>
      </div>
      <!-- Participant Information Column -->
      <div class="col-md-3">
        <div class="card shadow-lg mb-4">
          <div class="card-body text-center">
            <h5 class="card-title">Người tham gia</h5>
            <ul class="list-group">
              <li class="list-group-item" v-for="participant in participants" :key="participant.id">{{ participant.name }}</li>
            </ul>
          </div>
        </div>
      </div>
    </div>

    <!-- Bid Result Modal -->
    <div class="modal fade" id="bidResultModal" tabindex="-1" aria-labelledby="bidResultModalLabel" aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="bidResultModalLabel">{{ modalTitle }}</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            {{ modalMessage }}
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-primary" data-bs-dismiss="modal">Đóng</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted, watch } from 'vue';
import { useRoute } from 'vue-router';
import AuctionApi from '@/api/auction.api';
import * as bootstrap from 'bootstrap';
import type { AuctionModel } from '@/models/auction-model';
import { userStore } from '@/stores/auth';

export default defineComponent({
  inheritAttrs: false,
  setup() {
    const route = useRoute();
    const auction = ref<AuctionModel | null>(null);
    const bidAmount = ref(0);
    const user = ref({
      username: 'john_doe',
      balance: 1000000,
    });
    const auctionId = ref(Number(route.params.id));
    const participants = ref([
      { id: 1, name: 'Nguyễn Văn A' },
      { id: 2, name: 'Trần Thị B' },
    ]);
    const timeRemaining = ref('');
    const store = userStore();
    const userId = store.user?.id || JSON.parse(localStorage.getItem('user') || '{}').id;

    const modalTitle = ref('');
    const modalMessage = ref('');

    const fetchAuction = async () => {
      try {
        const response = await AuctionApi.getByIdAuction(auctionId.value);
        if (response.data && response.data.result) {
          auction.value = response.data.result.data as AuctionModel;
          startCountdown(auction.value.startDateTime, auction.value.timeAuction);
        } else {
          console.error('Không tìm thấy phiên đấu giá');
        }
      } catch (error) {
        console.error('Lỗi khi lấy chi tiết phiên đấu giá:', error);
      }
    };

    const startCountdown = (startDateTime: string, timeAuction: string) => {
      const endDateTime = new Date(new Date(startDateTime).getTime() + parseTimeAuction(timeAuction));
      const updateCountdown = () => {
        const now = new Date();
        const distance = endDateTime.getTime() - now.getTime();
        if (distance > 0) {
          const hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
          const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
          const seconds = Math.floor((distance % (1000 * 60)) / 1000);
          timeRemaining.value = `${hours}h ${minutes}m ${seconds}s`;
        } else {
          timeRemaining.value = 'Phiên đấu giá đã kết thúc';
        }
      };
      updateCountdown();
      setInterval(updateCountdown, 1000);
    };

    const parseTimeAuction = (timeAuction: string) => {
      const parts = timeAuction.split(':');
      const hours = parseInt(parts[0], 10) * 3600 * 1000;
      const minutes = parseInt(parts[1], 10) * 60 * 1000;
      const seconds = parseInt(parts[2], 10) * 1000;
      return hours + minutes + seconds;
    };

    const placeBid = async () => {
  if (!auction.value) return;

  const currentPrice = Number(auction.value.currentPrice) || Number(auction.value.startPrice);
  const bidPrice = Number(bidAmount.value);

  if (bidPrice > currentPrice) {
    if (bidPrice > user.value.balance) {
      modalTitle.value = 'Lỗi';
      modalMessage.value = 'Bạn không đủ số dư để đặt giá!';
      const modalElement = document.getElementById('bidResultModal');
      if (modalElement) {
        new bootstrap.Modal(modalElement).show();
      }
      return;
    }

    const updatedPriceModel = {
      id: auction.value.id,
      currentPrice: bidPrice.toString(),
      winnerId: userId,
    };

    try {
      const response = await AuctionApi.updateCurrentPriceAuction(updatedPriceModel);
      if (response && response.data && response.data.isSuccess) {
        modalTitle.value = 'Thành công';
        modalMessage.value = `Bạn đã đặt giá ${bidPrice} VNĐ thành công!`;
        auction.value.currentPrice = bidPrice.toString();
        user.value.balance -= bidPrice; // Giảm số dư sau khi đặt giá
      } else {
        modalTitle.value = 'Lỗi';
        modalMessage.value = response.data.message || 'Có lỗi xảy ra khi đặt giá.';
      }
    } catch (error) {
      console.error('Lỗi khi đặt giá:', error);
      modalTitle.value = 'Lỗi';
      modalMessage.value = 'Đã xảy ra lỗi khi đặt giá. Vui lòng thử lại.';
    }
  } else {
    modalTitle.value = 'Lỗi';
    modalMessage.value = 'Giá đặt phải cao hơn giá hiện tại!';
  }
  bidAmount.value = 0; // Reset ô nhập giá

  const modalElement = document.getElementById('bidResultModal');
  if (modalElement) {
    new bootstrap.Modal(modalElement).show();
  }
};

    watch(() => route.params.id, (newId) => {
      auctionId.value = Number(newId);
      fetchAuction();
    });

    onMounted(() => {
      fetchAuction();
    });

    const getFullImageUrl = (imagePath: string | null) => {
      if (!imagePath) {
        return 'https://via.placeholder.com/550x500';
      }
      const baseUrl = 'https://localhost:7071/';
      const firstImage = imagePath.split(';')[0];
      return `${baseUrl}${firstImage}`;
    };

    return {
      auction,
      user,
      bidAmount,
      timeRemaining,
      participants,
      placeBid,
      getFullImageUrl,
      modalTitle,
      modalMessage,
    };
  },
});
</script>

<style scoped>
.auction-container {
  padding: 20px;
}

.card {
  border-radius: 10px;
}

.card-title {
  margin-bottom: 10px;
  color: #007bff;
  font-weight: bold;
}

.card-text {
  margin-bottom: 8px;
}

.image-container {
  text-align: center;
  padding: 20px 0;
}

.auction-image {
  width: 100%;
  max-width: 300px;
  height: auto;
  object-fit: cover;
  margin: 0 auto;
  border-radius: 10px;
}

.current-price-box {
  margin-bottom: 10px;
}

.btn-primary {
  background-color: #007bff;
  border-color: #007bff;
}

.btn-primary:hover {
  background-color: #0056b3;
  border-color: #0056b3;
}

.list-group-item {
  background-color: #f8f9fa;
  border: none;
}

.list-group-item:hover {
  background-color: #e9ecef;
}

@media (max-width: 768px) {
  .col-md-6 {
    margin-left: auto;
    margin-right: auto;
  }
}
</style>
