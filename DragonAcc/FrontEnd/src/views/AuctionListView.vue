<template>
  <div class="auction-list container mt-5 mb-5">
    <h2 class="text-center mb-4">Danh sách đấu giá</h2>
    
    <div v-if="loading" class="text-center">
      <p>Đang tải các phiên đấu giá...</p>
    </div>
    
    <div v-if="errorMessage" class="alert alert-danger">{{ errorMessage }}</div>
    
    <div v-if="auctions.length > 0" class="row g-4">
      <div class="col-md-6 col-lg-4" v-for="(auction, index) in auctions" :key="auction.id">
        <div class="auction-card card shadow-sm">
          <img 
            :src="getFullImageUrl(auction.image)" 
            class="card-img-top" 
            alt="Hình ảnh đấu giá" 
            @load="imageLoaded(index)"
            @error="handleImageError(index)"
            v-if="!imageError[index]"
          />
          <div v-else class="text-center">
            <img src="https://via.placeholder.com/336x198?text=No+Image" alt="No Image" />
          </div>
          <div class="card-body">
            <h5 class="card-title">{{ auction.auctionName }}</h5>
            <p v-if="isAuctionActive(auction)" class="card-text">Giá bắt đầu: {{ auction.startPrice }} VNĐ</p>
            <p v-if="isAuctionActive(auction)" class="card-text">Giá hiện tại: {{ auction.currentPrice }} VNĐ</p>
            <p v-else-if="!isAuctionActive(auction) && auction.currentPrice" class="card-text">Giá cuối cùng: {{ auction.currentPrice }} VNĐ</p>
            <p class="card-text">Ngày bắt đầu: {{ formatDate(auction.startDateTime) }}</p>
            <p v-if="isAuctionActive(auction)" class="card-text">Thời gian còn lại: {{ getRemainingTime(auction) }}</p>
            <div v-if="!isAuctionActive(auction)">
              <p v-if="auction.winnerName" class="text-success">
                <strong>Người chiến thắng:</strong> {{ auction.winnerName }}
              </p>
              <p v-else class="text-danger">
                <strong>Không có người chiến thắng trong phiên đấu giá này.</strong>
              </p>
            </div>
            
            
            <button
            :class="buttonClass(auction)"
            class="btn w-100 mt-3"
            :disabled="!isAuctionActive(auction) && !isAuctionUpcoming(auction)"
            @click="goToAuctionDetail(auction.id)"
          >
            {{ buttonLabel(auction) }}
          </button>
          </div>
        </div>
      </div>
    </div>     
    <div v-else class="alert alert-info">Hiện tại không có phiên đấu giá nào.</div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted, onBeforeUnmount } from 'vue';
import type { AuctionModel } from '@/models/auction-model';
import AuctionApi from '@/api/auction.api';
import { useRouter } from 'vue-router';

export default defineComponent({
  name: 'AuctionList',
  setup() {
    const auctions = ref<AuctionModel[]>([]);
    const loading = ref(true);
    const errorMessage = ref<string | null>(null);
    const router = useRouter();

    // Các phương thức kiểm tra trạng thái của phiên đấu giá
    const isAuctionActive = (auction: AuctionModel): boolean => {
      const now = new Date();
      const startDate = new Date(auction.startDateTime);
      const durationMs = parseAuctionDuration(auction.timeAuction);
      const endDate = new Date(startDate.getTime() + durationMs);
      return now >= startDate && now <= endDate;
    };

    const isAuctionUpcoming = (auction: AuctionModel): boolean => {
      const now = new Date();
      const startDate = new Date(auction.startDateTime);
      return now < startDate;
    };

    // Định dạng lớp CSS và nhãn cho nút
    const buttonClass = (auction: AuctionModel) => {
      if (isAuctionActive(auction)) return 'btn-success';
      if (isAuctionUpcoming(auction)) return 'btn-warning';
      return 'btn-danger';
    };

    const buttonLabel = (auction: AuctionModel) => {
      if (isAuctionActive(auction)) return 'Tham gia';
      if (isAuctionUpcoming(auction)) return 'Sắp tới';
      return 'Hết hạn';
    };

    // Chuyển đổi duration thành milliseconds
    const parseAuctionDuration = (duration: string): number => {
      const [hours, minutes, seconds] = duration.split(':').map(Number);
      return ((hours * 60 * 60) + (minutes * 60) + seconds) * 1000; // Convert to milliseconds
    };

    // Lấy URL đầy đủ cho hình ảnh
    const getImageUrl = (imagePath: string) => {
      const baseUrl = 'https://localhost:7224/';
      return `${baseUrl}${imagePath}`;
    };

    // Định dạng ngày tháng
    const formatDate = (dateString: string) => {
      const date = new Date(dateString);
      return date.toLocaleDateString('vi-VN', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
      });
    };

    // Fetch auctions
    const fetchAuctions = async () => {
  try {
    const response = await AuctionApi.getAllAuction();
    if (response.data.result && response.data.result.data.length > 0) {
      auctions.value = response.data.result.data;

      // Lấy thông tin người chiến thắng cho các phiên đã kết thúc
      for (const auction of auctions.value) {
        if (!isAuctionActive(auction) && !isAuctionUpcoming(auction)) {
          const winnerResponse = await AuctionApi.getWinnerByAuctionId(auction.id);
          if (winnerResponse.data && winnerResponse.data.result) {
            auction.winnerName = winnerResponse.data.result.winnerName;
          } else {
            auction.winnerName = "";
          }
        }
      }
    } else {
      errorMessage.value = 'Không tìm thấy phiên đấu giá nào.';
    }
  } catch (error) {
    console.error('Lỗi khi tải dữ liệu phiên đấu giá:', error);
    errorMessage.value = 'Không thể tải dữ liệu phiên đấu giá.';
  } finally {
    loading.value = false;
  }
};


    // Xử lý hình ảnh
    const imageError = ref<boolean[]>([]);

    const imageLoaded = (index: number) => {
      // Logic khi hình ảnh đã tải
    };

    const getFullImageUrl = (imagePath: string | null) => {
      if (!imagePath) {
        return 'https://via.placeholder.com/550x500';
      }
      const baseUrl = 'https://localhost:7071/';
      const firstImage = imagePath.split(';')[0];
      return `${baseUrl}${firstImage}`;
    };

    const handleImageError = (index: number) => {
      imageError.value[index] = true;
    };

    // Timer cho thời gian còn lại
    const timer = ref<number>(Date.now());
    let intervalId: number;

    const getRemainingTime = (auction: AuctionModel): string => {
      // Sử dụng timer.value để đảm bảo hàm này được re-evaluate mỗi giây
      timer.value;

      const now = new Date();
      const startDate = new Date(auction.startDateTime);
      const durationMs = parseAuctionDuration(auction.timeAuction);
      const endDate = new Date(startDate.getTime() + durationMs);
      const remainingTimeMs = endDate.getTime() - now.getTime();

      if (remainingTimeMs <= 0) {
        return 'Đã kết thúc';
      }

      const hours = Math.floor(remainingTimeMs / (1000 * 60 * 60));
      const minutes = Math.floor((remainingTimeMs % (1000 * 60 * 60)) / (1000 * 60));
      const seconds = Math.floor((remainingTimeMs % (1000 * 60)) / 1000);

      return `${hours} giờ ${minutes} phút ${seconds} giây`;
    };

    const goToAuctionDetail = (id: number) => {
      router.push({ name: 'auctionDetail', params: { id } });
    };

    onMounted(() => {
      intervalId = setInterval(() => {
        timer.value = Date.now();
      }, 1000);

      fetchAuctions();
    });

    onBeforeUnmount(() => {
      clearInterval(intervalId);
    });

    return {
      auctions,
      loading,
      errorMessage,
      formatDate,
      getImageUrl,
      buttonClass,
      buttonLabel,
      isAuctionActive,
      isAuctionUpcoming,
      imageError,
      imageLoaded,
      getFullImageUrl,
      handleImageError,
      getRemainingTime,
      goToAuctionDetail,
    };
  },
});
</script>
  
  <style scoped>
  .auction-list {
    padding: 20px;
    background-color: #f9f9f9;
    border-radius: 8px;
  }
  
  .auction-card {
    transition: all 0.3s ease;
    border-radius: 10px;
    padding: 10px;
    background-color: #fff;
    box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
    min-height: 464px;
  }
  
  
  .auction-card:hover {
    transform: translateY(-5px);
    box-shadow: 0px 10px 20px rgba(0, 0, 0, 0.1);
  }
  
  .card-img-top {
    height: 200px;
    object-fit: cover;
    border-radius: 10px;
    margin: 10px 0;
    padding: 5px;
    background-color: #f0f0f0;
  }
  
  .btn {
    border-radius: 50px;
  }
  
  .alert-info, .alert-danger {
    text-align: center;
    margin-top: 20px;
  }
  </style>
  