<template>
  <div class="auction-container container mt-4 mb-5">
    <div class="row">
      <!-- Cột bên trái: Thông tin phiên đấu giá và giá hiện tại -->
      <div class="col-md-6">
        <div class="card shadow-lg text-center">
          <div class="card-header bg-primary text-white">
            <h5 class="card-title mt-2">{{ auction?.auctionName }}</h5>
            <p class="card-subtitle">{{ auction?.id }}</p>
          </div>
          <div class="image-container">
            <img 
              :src="getFullImageUrl(auction?.image)" 
              class="card-img-top" 
              alt="Hình ảnh đấu giá" 
            />
          </div>
          <div class="card-body">
            <p class="card-text">{{ auction?.prize }}</p>
            <div class="current-price-box">
              <p class="card-text"><strong>Giá hiện tại:</strong> {{ formatCurrency(auction?.currentPrice) }} VNĐ</p>
            </div>
          </div>
        </div>
      </div>
      <!-- Cột bên phải: Diễn biến đấu giá và phần đặt giá -->
      <div class="col-md-6">
        <!-- Hiển thị số dư người dùng -->
        <div class="user-balance-box mb-4">
          <h5>Số dư của bạn: {{ formatCurrency(userBalance) }} VNĐ</h5>
        </div>

        <!-- Diễn biến đấu giá -->
        <div class="card shadow-lg mb-4">
          <div class="card-body">
            <div class="d-flex justify-content-between align-items-center mb-3">
              <h5 class="card-title mb-0">Diễn biến đấu giá</h5>
              <button class="btn btn-secondary btn-sm" @click="fetchBids">Tải lại</button>
            </div>
            <ul class="list-group bid-list">
              <li class="list-group-item d-flex justify-content-between align-items-center" v-for="bid in bids" :key="bid.id">
                <div>
                  <strong>ID Người Đấu Giá:</strong> {{ bid.userID }}<br>
                  <strong>Giá:</strong> {{ formatCurrency(bid.raisePrice) }} VNĐ
                </div>
                <span class="text-muted">{{ formatDateTime(bid.createdDate) }}</span>
              </li>
            </ul>
          </div>
        </div>

        <!-- Phần đặt giá -->
        <div v-if="isAuctionActive" class="card shadow-lg mt-4">
          <div class="card-body">
            <h5 class="card-title">Đặt giá của bạn</h5>
            <p class="card-text text-danger"><strong>Thời gian còn lại:</strong> {{ timeRemaining }}</p>
            <form @submit.prevent="placeBid">
              <div class="mb-3">
                <label for="bidAmount" class="form-label">Số tiền đặt giá (VNĐ)</label>
                <input 
                  type="number" 
                  v-model.number="bidAmount" 
                  id="bidAmount" 
                  class="form-control" 
                  min="1"
                  required
                >
              </div>
              <button type="submit" class="btn btn-primary w-100">Đặt giá</button>
            </form>
          </div>
        </div>
        <div v-else-if="isAuctionUpcoming" class="card shadow-lg mt-4">
          <div class="card-body">
            <p class="card-text text-warning"><strong>Phiên đấu giá sắp diễn ra</strong></p>
          </div>
        </div>
        <div v-else class="card shadow-lg mt-4">
          <div class="card-body">
            <p class="card-text text-secondary"><strong>Phiên đấu giá đã kết thúc</strong></p>
            <div v-if="winner" class="mt-3">
              <h6>Người chiến thắng:</h6>
              <p><strong>Tên:</strong> {{ winner.fullName }}</p>
              <p><strong>Email:</strong> {{ winner.email }}</p>
            </div>
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
          <div class="modal-body">{{ modalMessage }}</div>
          <div class="modal-footer">
            <button type="button" class="btn btn-primary" data-bs-dismiss="modal">Đóng</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted, onBeforeUnmount } from 'vue';
import { useRoute } from 'vue-router';
import AuctionApi from '@/api/auction.api';
import * as bootstrap from 'bootstrap';
import type { AuctionModel } from '@/models/auctionmodel';
import { userStore } from '@/stores/auth';

export default defineComponent({
  setup() {
    const route = useRoute();
    const auction = ref<AuctionModel | null>(null);
    const bidAmount = ref<number>(0);
    const bids = ref<any[]>([]);
    const auctionId = ref<number>(Number(route.params.id));
    const timeRemaining = ref<string>('');
    const countdownInterval = ref<number | null>(null);
    const store = userStore();
    const userId = store.user?.id || JSON.parse(localStorage.getItem('user') || '{}').id;
    const userBalance = ref<number>(0);
    const modalTitle = ref('');
    const modalMessage = ref('');
    const isAuctionActive = ref<boolean>(false);
    const isAuctionUpcoming = ref<boolean>(false);
    const winner = ref<any>(null);

    // Fetch auction details by ID
    const fetchAuction = async () => {
      try {
        const response = await AuctionApi.getByIdAuction(auctionId.value);
        if (response.data && response.data.result) {
          auction.value = response.data.result.data as AuctionModel;
          await fetchBids();
          checkAuctionStatus();
          startCountdown();
          if (!isAuctionActive.value && !isAuctionUpcoming.value) {
            await fetchWinner();
          }
        } else {
          console.error('Không tìm thấy phiên đấu giá');
        }
      } catch (error) {
        console.error('Lỗi khi lấy chi tiết phiên đấu giá:', error);
      }
    };

    // Fetch user balance
    const fetchUserBalance = async () => {
      try {
        const response = await AuctionApi.getUserByID(userId);
        if (response.data && response.data.result && response.data.result.data) {
          userBalance.value = parseFloat(response.data.result.data.balance);
          console.log('User Balance:', userBalance.value);
        } else {
          console.error('Không tìm thấy thông tin người dùng');
        }
      } catch (error) {
        console.error('Lỗi khi lấy thông tin người dùng:', error);
      }
    };

    const fetchBids = async () => {
      try {
        const response = await AuctionApi.getAllAuctionDetail(auctionId.value);
        if (response.data && response.data.result) {
          bids.value = response.data.result.data;
          bids.value.sort((a, b) => new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime());
        } else {
          bids.value = [];
        }
      } catch (error) {
        console.error('Lỗi khi lấy danh sách đặt giá:', error);
      }
    };

    const fetchWinner = async () => {
      try {
        const response = await AuctionApi.getWinnerByAuctionId(auctionId.value);
        if (response.data && response.data.result) {
          winner.value = response.data.result.data;
        } else {
          winner.value = null;
        }
      } catch (error) {
        console.error('Lỗi khi lấy người chiến thắng:', error);
      }
    };

    // Countdown timer
    const startCountdown = () => {
      if (!auction.value) return;
      let auctionEndTime: Date | null = null;

      if (!auction.value.startDateTime || !auction.value.timeAuction) {
        console.error('startDateTime hoặc timeAuction bị thiếu');
        return;
      }

      const startDate = new Date(auction.value.startDateTime);
      const duration = parseTimeAuction(auction.value.timeAuction);
      auctionEndTime = new Date(startDate.getTime() + duration);

      if (countdownInterval.value) {
        clearInterval(countdownInterval.value);
      }

      countdownInterval.value = window.setInterval(() => {
        if (!auctionEndTime) return;

        const now = new Date();
        const distance = auctionEndTime.getTime() - now.getTime();

        if (distance > 0) {
          const hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
          const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
          const seconds = Math.floor((distance % (1000 * 60)) / 1000);
          timeRemaining.value = `${hours}h ${minutes}m ${seconds}s`;
        } else {
          timeRemaining.value = 'Phiên đấu giá đã kết thúc';
          clearInterval(countdownInterval.value!);
          isAuctionActive.value = false;
          fetchWinner();
        }
      }, 1000);
    };

    const parseTimeAuction = (timeAuction: string): number => {
      const parts = timeAuction.split(':');
      if (parts.length !== 3) {
        console.error('timeAuction không hợp lệ:', timeAuction);
        return 0;
      }
      const hours = parseInt(parts[0], 10) * 3600 * 1000;
      const minutes = parseInt(parts[1], 10) * 60 * 1000;
      const seconds = parseInt(parts[2], 10) * 1000;
      return hours + minutes + seconds;
    };

    const checkAuctionStatus = () => {
      if (!auction.value) {
        isAuctionActive.value = false;
        isAuctionUpcoming.value = false;
        return;
      }

      const status = getAuctionStatus(auction.value);
      if (status === 'upcoming') {
        isAuctionActive.value = false;
        isAuctionUpcoming.value = true;
      } else if (status === 'ongoing') {
        isAuctionActive.value = true;
        isAuctionUpcoming.value = false;
      } else if (status === 'ended') {
        isAuctionActive.value = false;
        isAuctionUpcoming.value = false;
      }
    };

    const getAuctionStatus = (auction: AuctionModel): string => {
      if (!auction.status) return "ended";

      const now = new Date();
      const startDate = new Date(auction.startDateTime);

      if (now < startDate) return "upcoming";

      const durationMs = parseTimeAuction(auction.timeAuction);
      const endDate = new Date(startDate.getTime() + durationMs);

      if (now >= startDate && now < endDate) {
        return "ongoing";
      }

      return "ended";
    };

    const placeBid = async () => {
      if (!auction.value || !isAuctionActive.value) return;

      const currentPrice = Number(auction.value.currentPrice) || Number(auction.value.startPrice);
      const bidPrice = Number(bidAmount.value);
      console.log('id', auction.value.id);
      if (bidPrice > currentPrice) {
        if (!auction.value.id) {
          console.error('ID của phiên đấu giá bị thiếu');
          return;
        }

        const bidModel = {
          auctionId: auction.value.id,
          userID: userId,
          raisePrice: bidPrice,
        };


        try {
          const response = await AuctionApi.addAuctionDetail(bidModel);
          console.log('Đặt giá:', response.data);
          if (response && response.data && !response.data.isSuccess) {
            modalTitle.value = 'Thành công';
            modalMessage.value = `Bạn đã đặt giá ${formatCurrency(bidPrice)} VNĐ thành công!`;
            bidAmount.value = 0;
            await fetchAuction();
            await fetchBids();
            startCountdown();
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
      bidAmount.value = 0;
      showModal();
    };

    const showModal = () => {
      const modalElement = document.getElementById('bidResultModal');
      if (modalElement) {
        new bootstrap.Modal(modalElement).show();
      }
    };

    const formatDateTime = (dateTimeString: string): string => {
      const date = new Date(dateTimeString);
      return date.toLocaleString('vi-VN');
    };

    const formatCurrency = (value: number | string | undefined): string => {
      if (value === undefined) return '0';
      return Number(value).toLocaleString('vi-VN');
    };

    onMounted(() => {
      fetchAuction();
      fetchUserBalance();
    });

    onBeforeUnmount(() => {
      if (countdownInterval.value) {
        clearInterval(countdownInterval.value);
      }
    });

    const getFullImageUrl = (imagePath: string | null): string => {
      if (!imagePath) {
        return 'https://via.placeholder.com/550x500';
      }
      const baseUrl = 'https://localhost:7071/';
      const firstImage = imagePath.split(',')[0];
      return `${baseUrl}${firstImage}`;
    };

    return {
      auction,
      bidAmount,
      bids,
      timeRemaining,
      placeBid,
      getFullImageUrl,
      modalTitle,
      modalMessage,
      isAuctionActive,
      isAuctionUpcoming,
      userBalance,
      formatDateTime,
      formatCurrency,
      fetchBids,
      winner,
    };
  },
});
</script>

<style scoped>
.auction-container {
  padding: 20px;
}

.image-container {
  max-height: 400px;
  overflow: hidden;
}

.card-img-top {
  width: 100%;
  object-fit: cover;
}

.current-price-box {
  margin-top: 10px;
}

.bid-list {
  max-height: 300px;
  overflow-y: auto;
}

.modal-content {
  text-align: center;
}
</style>
